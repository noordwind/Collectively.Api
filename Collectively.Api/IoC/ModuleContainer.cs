using Autofac;
using Collectively.Api.IoC.Modules;
using Collectively.Common.Security;

namespace Collectively.Api.IoC
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