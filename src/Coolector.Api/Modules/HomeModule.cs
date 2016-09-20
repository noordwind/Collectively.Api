using System.Threading.Tasks;
using Nancy;
using NLog;

namespace Collector.Api.Modules
{
    public class HomeModule : NancyModule
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public HomeModule()
        {
            Get("/", args => "Hello from Nancy running on CoreCLR");

            Get("/test/{name}", TestAsync);
        }

        private async Task<object> TestAsync(dynamic args)
        {
            Logger.Info($"TestAsync args: {args.name}");

            return $"Test parameter: {args.name}";
        }
    }
}