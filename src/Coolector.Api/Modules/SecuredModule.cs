using Nancy;
using Nancy.Security;

namespace Coolector.Api.Modules
{
    public class SecuredModule : NancyModule
    {
        public SecuredModule()
        {
            this.RequiresAuthentication();

            Get("/secured/{name}", args => $"Args: {args.name}, user:{Context.CurrentUser.Identity.Name}");
        }
    }
}