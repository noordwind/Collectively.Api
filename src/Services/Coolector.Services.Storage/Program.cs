using Coolector.Services.Host;
using Coolector.Services.Storage.Framework;

namespace Coolector.Services.Storage
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebServiceHost
                .Create<Startup>(port: 10000)
                .UseAutofac(Bootstrapper.LifeTimeScope)
                .UseRabbitMq()
                .Build()
                .Run();
        }
    }
}
