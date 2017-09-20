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
                .MapTo(MapToBasicRemark)
                .HandleAsync());

            Get("similar", async args => await FetchCollection<BrowseSimilarRemarks, Remark>
                (async x => await remarkStorage.BrowseSimilarAsync(x))
                .MapTo(MapToBasicRemark)
                .HandleAsync());

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

            Put("{remarkId}/photo", async args => await For<AddPhotosToRemark>()
                .Set(x =>
                {
                    var photo = ToFile();
                    x.Photos = new List<Collectively.Messages.Commands.Models.File>{photo};
                })
                .OnSuccessAccepted($"remarks/{args.remarkId}")
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

        private BasicRemark MapToBasicRemark(Remark remark)
        {
            var model = new BasicRemark
            {
                Id = remark.Id,
                Group = remark.Group == null ? null : new RemarkGroup
                {
                    Id = remark.Group.Id,
                    Name = remark.Group.Name
                },
                Author = remark.Author,
                Category = remark.Category,
                Location = remark.Location,
                SmallPhotoUrl = remark.Photos.FirstOrDefault(p => p.Size == "small")?.Url,
                Description = remark.Description,
                CreatedAt = remark.CreatedAt,
                UpdatedAt = remark.UpdatedAt,
                State = remark.State,
                Status = remark.Status,
                Rating = remark.Rating,
                Distance = remark.Distance,
                CommentsCount = remark.CommentsCount,
                ParticipantsCount = remark.ParticipantsCount,
                ReportsCount = remark.ReportsCount,
                Offering = remark.Offering,
                OfferingProposalsCount = remark.OfferingProposalsCount,
                PositiveVotesCount = remark.PositiveVotesCount,
                NegativeVotesCount = remark.NegativeVotesCount
            };
            if (remark.Photos == null || !remark.Photos.Any())
            {
                return model;
            }
            model.Photo = new BasicRemarkPhoto 
            {
                Small = remark.Photos.FirstOrDefault(p => p.Size == "small")?.Url,
                Medium = remark.Photos.FirstOrDefault(p => p.Size == "medium")?.Url,
                Big = remark.Photos.FirstOrDefault(p => p.Size == "big")?.Url
            };

            return model;
        } 
    }
}