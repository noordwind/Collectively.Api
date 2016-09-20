using Nancy;

namespace Collector.Api.Modules
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get("/", args => "Hello from Nancy running on CoreCLR");

            Get("/test/{name}", args => $"Test parameter: {args.name}");
        }
    }
}