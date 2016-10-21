using System.Linq;
using Coolector.Common.Commands.Remarks;
using Coolector.Common.Types;
using Coolector.Dto.Remarks;
using Nancy;
using BrowseRemarkCategories = Coolector.Api.Queries.BrowseRemarkCategories;
using BrowseRemarks = Coolector.Api.Queries.BrowseRemarks;
using GetRemark = Coolector.Api.Queries.GetRemark;
using GetRemarkPhoto = Coolector.Api.Queries.GetRemarkPhoto;
using ICommandDispatcher = Coolector.Api.Commands.ICommandDispatcher;
using IRemarkStorage = Coolector.Api.Storages.IRemarkStorage;

namespace Coolector.Api.Modules
{
    public class RemarkModule : ModuleBase
    {
        public RemarkModule(ICommandDispatcher commandDispatcher, IRemarkStorage remarkStorage)
            : base(commandDispatcher, modulePath: "remarks")
        {
            Get("", async args => await FetchCollection<BrowseRemarks, RemarkDto>
                (async x => await remarkStorage.BrowseAsync(x)).HandleAsync());

            Get("categories", async args => await FetchCollection<BrowseRemarkCategories, RemarkCategoryDto>
                (async x => await remarkStorage.BrowseCategoriesAsync(x)).HandleAsync());

            Get("{id}", async args => await Fetch<GetRemark, RemarkDto>
                (async x => await remarkStorage.GetAsync(x.Id)).HandleAsync());

            //TODO move to fileModule
            //Get("{id}/photo", async args => await Fetch<GetRemarkPhoto, Response>
            //(async x =>
            //    {
            //        var remark = await remarkStorage.GetAsync(x.Id);
            //        if (remark.HasNoValue)
            //            return new Maybe<Response>();

            //        var stream = await remarkStorage.GetPhotoAsync(x.Id, x.Size);
            //        var photo = remark.Value.Photos.FirstOrDefault(p => p.Size == x.Size);

            //        return FromStream(stream, photo., remark.Value.Photo.ContentType);
            //    }
            //).HandleAsync());

            Post("", async args => await For<CreateRemark>().DispatchAsync());

            Put("", async args => await For<ResolveRemark>().DispatchAsync());

            Delete("{remarkId}", async args => await For<DeleteRemark>().DispatchAsync());
        }
    }
}