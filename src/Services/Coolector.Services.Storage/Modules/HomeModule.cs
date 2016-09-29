using Nancy;

namespace Coolector.Services.Storage.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get("/", args => Response.AsJson(new { name = "Coolector.Services.Storage" }));
        }
    }
}