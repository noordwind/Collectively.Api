using Nancy;

namespace Coolector.Services.Storage.Modules
{
    public class HomeModule : ModuleBase
    {
        public HomeModule()
        {
            Get("", args => Response.AsJson(new { name = "Coolector.Services.Storage" }));
        }
    }
}