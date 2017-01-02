using Autofac;
using Coolector.Api.IoC.Modules;
using Coolector.Common.Security;

namespace Coolector.Api.IoC
{
    public class ModuleContainer : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<DispatcherModule>();
            builder.RegisterModule<FilterModule>();
            builder.RegisterModule<StorageModule>();
            builder.RegisterModule<ValidatorModule>();
        }
    }
}