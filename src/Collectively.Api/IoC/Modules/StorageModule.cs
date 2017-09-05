using System.Linq;
using Autofac;
using Collectively.Api.Filters;
using Collectively.Api.Storages;
using Collectively.Common.Security;
using Collectively.Common.ServiceClients;

namespace Collectively.Api.IoC.Modules
{
    public class StorageModule : Module
    {
        private readonly static string StorageSettingsKey = "storage-settings";

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(x => x.Resolve<ServicesSettings>()
                    .Single(s => s.Title == "storage-service"))
                .Named<ServiceSettings>(StorageSettingsKey)
                .SingleInstance();

            builder.Register(x => new StorageClient(
                    x.Resolve<IServiceClient>(),
                    x.Resolve<IMemoryCache>(), 
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

            builder.RegisterType<NotificationSettingsStorage>()
                .As<INotificationSettingsStorage>()
                .SingleInstance();

            builder.RegisterType<GroupStorage>()
                .As<IGroupStorage>()
                .SingleInstance();

            builder.RegisterType<OrganizationStorage>()
                .As<IOrganizationStorage>()
                .SingleInstance();

            builder.RegisterType<InMemoryCache>()
                .As<IMemoryCache>()
                .SingleInstance();
        }
    }
}
