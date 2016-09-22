using System.Threading.Tasks;
using Nancy;
using Nancy.Security;

namespace Coolector.Api.Modules
{
    public class SecuredModule : NancyModule
    {
        public SecuredModule()
        {
            this.RequiresAuthentication();

            Get("/secured", async args =>
            {
                var dto = new
                {
                    user = Context.CurrentUser.Identity.Name
                };

                return await Task.FromResult(dto);
            });
        }
    }
}