using Autofac;
using Coolector.Api.IoC.Modules;

namespace Coolector.Api.IoC
{
    public class ModuleContainer : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<AuthenticationModule>();
            builder.RegisterModule<DispatcherModule>();
            builder.RegisterModule<FilterModule>();
            builder.RegisterModule<StorageModule>();
            builder.RegisterModule<ValidatorModule>();
        }
    }
}