using Coolector.Common.Commands.Remarks;
using Coolector.Common.Events.Users;
using Coolector.Services.Host;
using Coolector.Services.Remarks.Framework;

namespace Coolector.Services.Remarks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebServiceHost
                .Create<Startup>(port: 10002)
                .UseAutofac(Bootstrapper.LifetimeScope)
                .UseRabbitMq()
                .SubscribeToCommand<CreateRemark>()
                .SubscribeToEvent<NewUserSignedIn>()
                .Build()
                .Run();
        }
    }
}
