using Nancy;
using NLog;

namespace Coolector.Api.Modules
{
    public class HomeModule : NancyModule
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public HomeModule()
        {
            Get("/", args => "Hello from Nancy running on CoreCLR");
        }
    }
}