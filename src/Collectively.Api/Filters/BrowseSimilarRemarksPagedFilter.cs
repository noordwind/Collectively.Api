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
    public class BrowseSimilarRemarksPagedFilter : IPagedFilter<Remark, BrowseSimilarRemarks>
    {
        public PagedResult<Remark> Filter(IEnumerable<Remark> values, BrowseSimilarRemarks query)
        {
            if (!query.IsLocationProvided())
            {
                return PagedResult<Remark>.Empty;
            }
            if (query.Category.Empty())
            {
                return PagedResult<Remark>.Empty;
            }
            if (query.Page <= 0)
            {
                query.Page = 1;
            }
            if (query.Results <= 0)
            {
                query.Results = 10;
            }
            if (query.Results > 100)
            {
                query.Results = 100;
            }
            if (query.Radius <= 0)
            {
                query.Radius = 10;
            }
            if (query.Radius > 100)
            {
                query.Radius = 100;
            }
            values = values.Where(x => x.Category.Name == query.Category);
            values.SetRemarksDistance(query.Latitude, query.Longitude);

            return values.OrderBy(x => x.Distance).Paginate(query);
        }
    }
}