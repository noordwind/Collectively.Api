using Autofac;
using Coolector.Api.Storages;

namespace Coolector.Api.IoC.Modules
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
            builder.RegisterType<RemarkStorage>()
                .As<IRemarkStorage>()
                .InstancePerLifetimeScope();
            builder.RegisterType<InMemoryCache>()
                .As<ICache>()
                .SingleInstance();
        }
    }
}