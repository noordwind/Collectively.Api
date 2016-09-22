using Autofac;
using Coolector.Infrastructure.Auth0;

namespace Coolector.Infrastructure.IoC.Modules
{
    public class Auth0Module : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Auth0RestClient>()
                .As<IAuth0RestClient>()
                .InstancePerLifetimeScope();
        }
    }
}