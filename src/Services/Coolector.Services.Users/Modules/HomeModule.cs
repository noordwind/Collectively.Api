using Nancy;

namespace Coolector.Services.Users.Modules
{
    public class HomeModule : ModuleBase
    {
        public HomeModule()
        {
            Get("/", args => Response.AsJson(new { name = "Coolector.Services.Users" }));
        }
    }
}