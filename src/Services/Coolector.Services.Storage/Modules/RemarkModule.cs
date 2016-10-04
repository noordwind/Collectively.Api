using System;
using Coolector.Services.Storage.Files;
using Coolector.Services.Storage.Providers;
using Coolector.Services.Storage.Queries;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Responses;

namespace Coolector.Services.Storage.Modules
{
    public class RemarkModule : ModuleBase
    {
        public RemarkModule(IRemarkProvider remarkProvider, IFileHandler fileHandler) : base("remarks")
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
                var remark = await remarkProvider.GetAsync((Guid)args.id);
                if (remark.HasNoValue)
                    return HttpStatusCode.NotFound;

                var fileStreamInfo = await fileHandler.GetFileStreamInfoAsync(remark.Value.Photo.FileId);
                if (fileStreamInfo.HasNoValue)
                    return HttpStatusCode.NotFound;

                var response = new StreamResponse(() => fileStreamInfo.Value.Stream, fileStreamInfo.Value.ContentType);

                return response.AsAttachment(fileStreamInfo.Value.Name);
            });
        }
    }
}