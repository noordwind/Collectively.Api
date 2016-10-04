using System;
using Coolector.Services.Storage.Providers;
using Coolector.Services.Storage.Queries;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses;

namespace Coolector.Services.Storage.Modules
{
    public class RemarkModule : ModuleBase
    {
        public RemarkModule(IRemarkProvider remarkProvider) : base("remarks")
        {
            Get("", async args =>
            {
                var query = this.Bind<BrowseRemarks>();
                var remarks = await remarkProvider.BrowseAsync(query);

                return FromPagedResult(remarks);
            });
            Get("{id}", async args =>
            {
                var remark = await remarkProvider.GetAsync((Guid)args.id);
                if (remark.HasValue)
                    return remark.Value;

                return HttpStatusCode.NotFound;
            });
            Get("{id}/photo", async args =>
            {
                var stream = await remarkProvider.GetPhotoAsync((Guid)args.id);
                if (stream.HasNoValue)
                    return HttpStatusCode.NotFound;

                var response = new StreamResponse(() => stream.Value.Stream, stream.Value.ContentType);

                return response.AsAttachment(stream.Value.Name);
            });
        }
    }
}