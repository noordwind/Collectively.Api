using Coolector.Common.Types;
using Coolector.Dto.Remarks;
using Coolector.Services.Storage.Providers;
using Coolector.Services.Storage.Queries;
using Nancy;

namespace Coolector.Services.Storage.Modules
{
    public class RemarkModule : ModuleBase
    {
        public RemarkModule(IRemarkProvider remarkProvider) : base("remarks")
        {
            Get("", async args => await FetchCollection<BrowseRemarks, RemarkDto>
                (async x => await remarkProvider.BrowseAsync(x)).HandleAsync());

            Get("categories", async args => await FetchCollection<BrowseRemarkCategories, RemarkCategoryDto>
                (async x => await remarkProvider.BrowseCategoriesAsync(x)).HandleAsync());

            Get("{id}", async args => await Fetch<GetRemark, RemarkDto>
                (async x => await remarkProvider.GetAsync(x.Id)).HandleAsync());

            Get("{id}/photo", async args => await Fetch<GetRemarkPhoto, Response>
            (async x =>
            {
                var stream = await remarkProvider.GetPhotoAsync(x.Id);
                if (stream.HasNoValue)
                    return new Maybe<Response>();

                var streamInfo = stream.Value;

                return FromStream(streamInfo.Stream, streamInfo.Name, streamInfo.ContentType);
            }
            ).HandleAsync());
        }
    }
}