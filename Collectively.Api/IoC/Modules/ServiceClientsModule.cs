using Autofac;
using Collectively.Api.Services;
using Collectively.Common.ServiceClients;

namespace Collectively.Api.IoC.Modules
{
    public class ServiceClientsModule : ServiceClientsModuleBase
    {
        protected override void Load(ContainerBuilder builder)
        {
            RegisterService<UserServiceClient, IUserServiceClient>(builder, "users");
        }
    }
}