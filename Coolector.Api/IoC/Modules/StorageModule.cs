using System.Linq;
using Autofac;
using Coolector.Api.Filters;
using Coolector.Api.Storages;
using Coolector.Common.Security;

namespace Coolector.Api.IoC.Modules
{
    public class StorageModule : Module
    {
        private readonly static string StorageSettingsKey = "storage-settings";

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x => x.Resolve<ServicesSettings>()
                    .Single(s => s.Name == "storage"))
                .Named<ServiceSettings>(StorageSettingsKey)
                .SingleInstance();

            builder.Register(x => new StorageClient(x.Resolve<ICache>(), 
                    x.Resolve<IFilterResolver>(), 
                    x.Resolve<IServiceAuthenticatorClient>(),
                    x.ResolveNamed<ServiceSettings>(StorageSettingsKey)))
                .As<IStorageClient>()
                .SingleInstance();

            builder.RegisterType<UserStorage>()
                .As<IUserStorage>()
                .SingleInstance();
                
            builder.RegisterType<RemarkStorage>()
                .As<IRemarkStorage>()
                .SingleInstance();

            builder.RegisterType<OperationStorage>()
                .As<IOperationStorage>()
                .SingleInstance();

            builder.RegisterType<StatisticsStorage>()
                .As<IStatisticsStorage>()
                .SingleInstance();

            builder.RegisterType<InMemoryCache>()
                .As<ICache>()
                .SingleInstance();
        }
    }
}
