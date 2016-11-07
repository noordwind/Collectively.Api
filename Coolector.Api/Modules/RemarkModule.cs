using System.Linq;
using Coolector.Api.Commands;
using Coolector.Api.Queries;
using Coolector.Api.Storages;
using Coolector.Api.Validation;
using Coolector.Common.Commands.Remarks;
using Coolector.Dto.Remarks;

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

            Post("", async args => await For<CreateRemark>().DispatchAsync());

            Put("", async args => await For<ResolveRemark>().DispatchAsync());

            Delete("{remarkId}", async args => await For<DeleteRemark>().DispatchAsync());
        }
    }
}