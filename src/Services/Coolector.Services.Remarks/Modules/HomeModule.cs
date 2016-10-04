using Nancy;

namespace Coolector.Services.Remarks.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get("/", args => Response.AsJson(new { name = "Coolector.Services.Remarks" }));
        }
    }
}