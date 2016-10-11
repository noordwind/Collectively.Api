using Coolector.Tests.EndToEnd.Framework;

namespace Coolector.Tests.EndToEnd.API
{
    public abstract class ModuleBase_specs
    {
        protected static IHttpClient HttpClient = new CustomHttpClient("http://localhost:5000");
    }
}