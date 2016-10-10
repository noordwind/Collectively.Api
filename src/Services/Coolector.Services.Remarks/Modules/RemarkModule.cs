using Coolector.Common.Types;
using Coolector.Services.Remarks.Domain;
using Coolector.Services.Remarks.Queries;
using Coolector.Services.Remarks.Services;
using Nancy;

namespace Coolector.Services.Remarks.Modules
{
    public class RemarkModule : ModuleBase
    {
        public RemarkModule(IRemarkService remarkService) : base("remarks")
        {
            Get("", async args => await FetchCollection<BrowseRemarks, Remark>
                (async x => await remarkService.BrowseAsync(x)).HandleAsync());

            Get("categories", async args => await FetchCollection<BrowseCategories, Category>
                (async x => await remarkService.BrowseCategoriesAsync(x)).HandleAsync());

            Get("{id}", async args => await Fetch<GetRemark, Remark>
                (async x => await remarkService.GetAsync(x.Id)).HandleAsync());

            Get("{id}/photo", async args => await Fetch<GetRemarkPhoto, Response>
            (async x =>
                {
                    var stream = await remarkService.GetPhotoAsync(x.Id);
                    if (stream.HasNoValue)
                        return new Maybe<Response>();

                    var streamInfo = stream.Value;

                    return FromStream(streamInfo.Stream, streamInfo.Name, streamInfo.ContentType);
                }
            ).HandleAsync());
        }
    }
}