using Collectively.Api.Commands;
using Collectively.Api.Queries;
using Collectively.Api.Storages;
using Collectively.Api.Validation;
using System.Linq;
using Collectively.Messages.Commands.Remarks;
using System.Collections.Generic;
using System;
using Collectively.Services.Storage.Models.Remarks;
using Collectively.Messages.Commands.Models;

namespace Collectively.Api.Modules
{
    public class RemarkModule : ModuleBase
    {
        public RemarkModule(ICommandDispatcher commandDispatcher,
            IRemarkStorage remarkStorage,
            IValidatorResolver validatorResolver)
            : base(commandDispatcher, validatorResolver, modulePath: "remarks")
        {
            Get("", async args => await FetchCollection<BrowseRemarks, Remark>
                (async x => await remarkStorage.BrowseAsync(x))
                .MapTo(x => new BasicRemark
                {
                    Id = x.Id,
                    Author = x.Author,
                    Category = x.Category,
                    Location = x.Location,
                    SmallPhotoUrl = x.Photos.FirstOrDefault(p => p.Size == "small")?.Url,
                    Description = x.Description,
                    CreatedAt = x.CreatedAt,
                    State = x.State,
                    Rating = x.Rating,
                    CommentsCount = x.CommentsCount,
                    ParticipantsCount = x.ParticipantsCount
                }).HandleAsync());

            Get("categories", async args => await FetchCollection<BrowseRemarkCategories, RemarkCategory>
                (async x => await remarkStorage.BrowseCategoriesAsync(x)).HandleAsync());

            Get("tags", async args => await FetchCollection<BrowseRemarkTags, Tag>
                (async x => await remarkStorage.BrowseTagsAsync(x)).HandleAsync());

            Get("{id}", async args => await Fetch<GetRemark, Remark>
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

            Put("{remarkId}/process", async args => await For<ProcessRemark>()
                .OnSuccessAccepted($"remarks/{args.remarkId}")
                .DispatchAsync());

            Put("{remarkId}/renew", async args => await For<RenewRemark>()
                .OnSuccessAccepted($"remarks/{args.remarkId}")
                .DispatchAsync());

            Delete("{remarkId}", async args => await For<DeleteRemark>()
                .OnSuccessAccepted()
                .DispatchAsync());

            Put("{remarkId}/votes", async args => await For<SubmitRemarkVote>()
                .Set(x => x.CreatedAt = DateTime.UtcNow)
                .OnSuccessAccepted($"remarks/{args.remarkId}")
                .DispatchAsync());

            Delete("{remarkId}/votes", async args => await For<DeleteRemarkVote>()
                .OnSuccessAccepted($"remarks/{args.remarkId}")
                .DispatchAsync());

            Delete("{remarkId}/states/{stateId}", async args => await For<DeleteRemarkState>()
                .OnSuccessAccepted($"remarks/{args.remarkId}")
                .DispatchAsync());
        }
    }
}