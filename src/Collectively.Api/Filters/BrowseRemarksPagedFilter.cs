using System;
using System.Collections.Generic;
using System.Linq;
using Collectively.Api.Framework;
using Collectively.Api.Queries;
using Collectively.Common.Extensions;
using Collectively.Common.Locations;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Remarks;

namespace Collectively.Api.Filters
{
    public class BrowseRemarksPagedFilter : IPagedFilter<Remark, BrowseRemarks>
    {
        private static readonly int NegativeVotesThreshold = -5;

        public PagedResult<Remark> Filter(IEnumerable<Remark> values, BrowseRemarks query)
        {
            if (!query.IsLocationProvided() && query.AuthorId.Empty() && query.ResolverId.Empty())
            {
                query.Latest = true;
            }
            if (query.Page <= 0)
            {
                query.Page = 1;
            }
            if (query.Results <= 0)
            {
                query.Results = 10;
            }
            if (query.Results >= 1000)
            {
                query.Results = 1000;
            }
            if (query.AuthorId.NotEmpty())
            {
                values = values.Where(x => x.Author.UserId == query.AuthorId);
                if (query.OnlyLiked)
                {
                    values = values.Where(x => x.Votes.Any(v => v.UserId == query.AuthorId && v.Positive));
                }
                else if (query.OnlyDisliked)
                {
                    values = values.Where(x => x.Votes.Any(v => v.UserId == query.AuthorId && !v.Positive));
                }
            }
            if (query.ResolverId.NotEmpty())
            {
                values = values.Where(x => x.State.State == "resolved" 
                    && x.State.User.UserId == query.ResolverId);
            }
            if (!query.Description.Empty())
            {
                values = values.Where(x => x.Description.Contains(query.Description));
            }
            if (query.Categories?.Any() == true)
            {
                values = values.Where(x => query.Categories.Contains(x.Category.Name));
            }
            if (query.Tags?.Any() == true)
            {
                values = values.Where(x => x.Tags.Any(y => query.Tags.Contains(y.Name)));
            }
            if (query.States?.Any() == true)
            {
                values = values.Where(x => query.States.Contains(x.State.State));
            }
            if (!query.Disliked)
            {
                values = values.Where(x => x.Rating > NegativeVotesThreshold);
            }
            if (query.GroupId.HasValue && query.GroupId != Guid.Empty)
            {
                values = values.Where(x => x.Group?.Id == query.GroupId);
            }
            if (query.AvailableGroupId.HasValue && query.AvailableGroupId != Guid.Empty)
            {
                values = values.Where(x => x.AvailableGroups?.Contains(query.AvailableGroupId.Value) == true);
            }
            if (query.UserFavorites.NotEmpty())
            {
                values = values.Where(x => x.UserFavorites.Contains(query.UserFavorites));
            }
            if (query.IsLocationProvided())
            {
                values.SetRemarksDistance(query.Latitude, query.Longitude);
            }

            return SortRemarks(query, values).Paginate(query);
        }

        private static IEnumerable<Remark>  SortRemarks(BrowseRemarks query, 
            IEnumerable<Remark> remarks)
        {
            if(query.OrderBy.Empty() && !query.Latest)
            {
                return remarks;
            }
            if(query.SortOrder.Empty())
            {
                query.SortOrder = "ascending";
            }
            if(query.Latest)
            {
                query.OrderBy = "createdat";
                query.SortOrder = "descending";
            }
            query.OrderBy = query.OrderBy.ToLowerInvariant();
            query.SortOrder = query.SortOrder.ToLowerInvariant();

            switch(query.OrderBy)
            {
                case "userid": return SortRemarks(query, remarks, x => x.Author.UserId);
                case "createdat": return SortRemarks(query, remarks, x => x.CreatedAt);
            }

            return remarks;
        }

        private static IEnumerable<Remark> SortRemarks<TKey>(BrowseRemarks query, 
            IEnumerable<Remark> remarks, Func<Remark, TKey> sortBy)
        {
            switch(query.SortOrder)
            {
                case "ascending": return remarks.OrderBy(sortBy);
                case "descending": return remarks.OrderByDescending(sortBy);
            }

            return remarks;
        }     
    }
}