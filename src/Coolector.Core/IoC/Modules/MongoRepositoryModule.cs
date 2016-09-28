using Autofac;
using Coolector.Core.Mongo.Repositories;
using Coolector.Core.Repositories;

namespace Coolector.Core.IoC.Modules
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