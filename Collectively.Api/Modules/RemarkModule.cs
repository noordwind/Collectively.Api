using Collectively.Api.Commands;
using Collectively.Api.Queries;
using Collectively.Api.Storages;
using Collectively.Api.Validation;
using System.Linq;
using Collectively.Messages.Commands.Remarks;

using System.Collections.Generic;
using Collectively.Messages.Commands.Remarks.Models;
using System;

namespace Collectively.Api.Modules
{
    public class RemarkModule : ModuleBase
    {
        public RemarkModule(ICommandDispatcher commandDispatcher,
            IRemarkStorage remarkStorage,
            IValidatorResolver validatorResolver)
            : base(commandDispatcher, validatorResolver, modulePath: "remarks")
        {
            Get("", async args => await FetchCollection<BrowseRemarks, RemarkDto>
                (async x => await remarkStorage.BrowseAsync(x))
                .MapTo(x => new BasicRemarkDto
                {
                    Id = x.Id,
                    Author = x.Author,
                    Category = x.Category,
                    Location = x.Location,
                    SmallPhotoUrl = x.Photos.FirstOrDefault(p => p.Size == "small")?.Url,
                    Description = x.Description,
                    CreatedAt = x.CreatedAt,
                    State = x.State,
                    Rating = x.Rating
                }).HandleAsync());

            Get("categories", async args => await FetchCollection<BrowseRemarkCategories, RemarkCategoryDto>
                (async x => await remarkStorage.BrowseCategoriesAsync(x)).HandleAsync());

            Get("tags", async args => await FetchCollection<BrowseRemarkTags, TagDto>
                (async x => await remarkStorage.BrowseTagsAsync(x)).HandleAsync());

            Get("{id}", async args => await Fetch<GetRemark, RemarkDto>
                (async x => await remarkStorage.GetAsync(x.Id)).HandleAsync());

            Post("", async args => await For<CreateRemark>()
                .SetResourceId(x => x.RemarkId)
                .OnSuccessAccepted("remarks/{0}")
                .DispatchAsync());

            Put("{remarkId}/photos", async args => await For<AddPhotosToRemark>()
                .OnSuccessAccepted($"remarks/{args.remarkId}")
                .DispatchAsync());

            Delete("{remarkId}/photos/{groupId}", async args => await For<RemovePhotosFromRemark>()
                .Set(x => x.Photos = new List<GroupedFile>
                {   
                    new GroupedFile
                    {
                        GroupId = args.groupId
                    }
                })
                .OnSuccessAccepted($"remarks/{args.remarkId}")
                .DispatchAsync());

            Put("{remarkId}/resolve", async args => await For<ResolveRemark>()
                .OnSuccessAccepted($"remarks/{args.remarkId}")
                .DispatchAsync());

            Delete("{remarkId}", async args => await For<DeleteRemark>()
                .OnSuccessAccepted(string.Empty)
                .DispatchAsync());

            Put("{remarkId}/votes", async args => await For<SubmitRemarkVote>()
                .Set(x => x.CreatedAt = DateTime.UtcNow)
                .OnSuccessAccepted($"remarks/{args.remarkId}")
                .DispatchAsync());

            Delete("{remarkId}/votes", async args => await For<DeleteRemarkVote>()
                .OnSuccessAccepted($"remarks/{args.remarkId}")
                .DispatchAsync());
        }
    }
}