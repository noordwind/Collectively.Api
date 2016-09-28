using Nancy;

namespace Coolector.Services.Users.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get("/", args => Response.AsJson(new { name = "Coolector.Services.Users" }));
        }
    }
}