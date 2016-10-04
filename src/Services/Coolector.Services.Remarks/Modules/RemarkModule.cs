using System;
using Coolector.Services.Remarks.Queries;
using Coolector.Services.Remarks.Services;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses;

namespace Coolector.Services.Remarks.Modules
{
    public class RemarkModule : ModuleBase
    {
        public RemarkModule(IRemarkService remarkService) : base("remarks")
        {
            Get("", async args =>
            {
                var query = this.Bind<BrowseRemarks>();
                var remarks = await remarkService.BrowseAsync(query);

                return FromPagedResult(remarks);
            });
            Get("{id}", async args =>
            {
                var remark = await remarkService.GetAsync((Guid)args.id);
                if (remark.HasValue)
                    return remark.Value;

                return HttpStatusCode.NotFound;
            });
            Get("{id}/photo", async args =>
            {
                var stream = await remarkService.GetPhotoAsync((Guid) args.id);
                if (stream.HasNoValue)
                    return HttpStatusCode.NotFound;

                var response = new StreamResponse(() => stream.Value.Stream, stream.Value.ContentType);

                return response.AsAttachment(stream.Value.Name);
            });
        }
    }
}