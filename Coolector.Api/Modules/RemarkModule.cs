using Coolector.Api.Commands;
using Coolector.Api.Queries;
using Coolector.Api.Storages;
using Coolector.Api.Validation;
using System.Linq;
using Coolector.Services.Remarks.Shared.Commands;
using Coolector.Services.Remarks.Shared.Dto;
using System.Collections.Generic;
using Coolector.Services.Remarks.Shared.Commands.Models;

namespace Coolector.Api.Modules
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
                    Author = x.Author.Name,
                    Category = x.Category.Name,
                    Location = x.Location,
                    SmallPhotoUrl = x.Photos.FirstOrDefault(p => p.Size == "small")?.Url,
                    Description = x.Description,
                    CreatedAt = x.CreatedAt,
                    Resolved = x.Resolved
                }).HandleAsync());

            Get("categories", async args => await FetchCollection<BrowseRemarkCategories, RemarkCategoryDto>
                (async x => await remarkStorage.BrowseCategoriesAsync(x)).HandleAsync());

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
        }
    }
}