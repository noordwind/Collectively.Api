using Autofac;
using Coolector.Core.Repositories;
using Coolector.Infrastructure.Mongo.Repositories;

namespace Coolector.Infrastructure.IoC.Modules
{
    public class MongoRepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserRepository>()
                .As<IUserRepository>()
                .InstancePerLifetimeScope();
        }
    }
}