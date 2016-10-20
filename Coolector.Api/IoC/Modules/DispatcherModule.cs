using System.Reflection;
using Autofac;
using Coolector.Api.Commands;
using Coolector.Common.Commands;
using Coolector.Common.Events;
using Module = Autofac.Module;

namespace Coolector.Api.IoC.Modules
{
    public class DispatcherModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CommandDispatcher>()
                .As<ICommandDispatcher>()
                .InstancePerLifetimeScope();
            
            var coreAssembly = typeof(Startup).GetTypeInfo().Assembly;
            builder.RegisterAssemblyTypes(coreAssembly).AsClosedTypesOf(typeof(ICommandHandler<>));
            builder.RegisterAssemblyTypes(coreAssembly).AsClosedTypesOf(typeof(IEventHandler<>));
        }
    }
}