using System.Reflection;
using Autofac;
using Coolector.Common.Commands;
using Coolector.Common.Events;
using Coolector.Core.Commands;
using Coolector.Core.Events;
using Module = Autofac.Module;

namespace Coolector.Core.IoC.Modules
{
    public class DispatcherModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CommandDispatcher>()
                .As<ICommandDispatcher>()
                .InstancePerLifetimeScope();

            builder.RegisterType<EventDispatcher>()
                .As<IEventDispatcher>()
                .InstancePerLifetimeScope();

            var coreAssembly = Assembly.Load(new AssemblyName("Coolector.Core"));
            builder.RegisterAssemblyTypes(coreAssembly).AsClosedTypesOf(typeof(ICommandHandler<>));
            builder.RegisterAssemblyTypes(coreAssembly).AsClosedTypesOf(typeof(IEventHandler<>));
        }
    }
}