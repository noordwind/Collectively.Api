using System;
using Coolector.Api.Modules.Base;
using Coolector.Common.Commands.Remarks;
using Coolector.Core.Commands;
using Coolector.Core.Filters;
using Coolector.Core.Storages;
using Nancy;
using Nancy.Responses;
using Nancy.Security;

namespace Coolector.Api.Modules
{
    public class RemarkModule : ModuleBase
    {
        public RemarkModule(ICommandDispatcher commandDispatcher, IRemarkStorage remarkStorage)
            :base(commandDispatcher, modulePath: "remarks")
        {
            Get("", async args =>
            {
                var query = BindRequest<BrowseRemarks>();
                var remarks = await remarkStorage.BrowseAsync(query);

                return FromPagedResult(remarks);
            });
            Get("{id}", async args =>
            {
                var remark = await remarkStorage.GetAsync((Guid)args.Id);
                if (remark.HasValue)
                    return remark.Value;

                return HttpStatusCode.NotFound;
            });
            Get("{id}/photo", async args =>
            {
                var remarkId = (Guid)args.id;
                var remark = await remarkStorage.GetAsync(remarkId);
                if (remark.HasNoValue)
                    return HttpStatusCode.NotFound;

                var photoStream = await remarkStorage.GetPhotoStreamAsync(remarkId);
                if (photoStream.HasNoValue)
                    return HttpStatusCode.NotFound;

                var response = new StreamResponse(() => photoStream.Value, remark.Value.Photo.ContentType);

                return response.AsAttachment(remark.Value.Photo.Name);
            });
            Post("", async args =>
            {
                this.RequiresAuthentication();
                var command = BindAuthenticatedCommand<CreateRemark>();
                await CommandDispatcher.DispatchAsync(command);
            });
            Delete("", async args =>
            {
                this.RequiresAuthentication();
                var command = BindAuthenticatedCommand<DeleteRemark>();
                await CommandDispatcher.DispatchAsync(command);
            });
        }
    }
}