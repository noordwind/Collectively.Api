using Autofac;
using Coolector.Core.Services;

namespace Coolector.Core.IoC.Modules
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserService>()
                .As<IUserService>()
                .InstancePerLifetimeScope();
        }
    }
}