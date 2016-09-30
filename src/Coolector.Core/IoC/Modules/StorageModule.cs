using Autofac;
using Coolector.Core.Storages;

namespace Coolector.Core.IoC.Modules
{
    public class StorageModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<StorageClient>()
                .As<IStorageClient>()
                .InstancePerLifetimeScope();
            builder.RegisterType<UserStorage>()
                .As<IUserStorage>()
                .InstancePerLifetimeScope();
            builder.RegisterType<InMemoryCache>()
                .As<ICache>()
                .SingleInstance();
        }
    }
}