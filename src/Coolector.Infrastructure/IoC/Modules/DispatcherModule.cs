using System.Reflection;
using Autofac;
using Coolector.Infrastructure.Commands;
using Coolector.Infrastructure.Events;
using Module = Autofac.Module;

namespace Coolector.Infrastructure.IoC.Modules
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
            var infrastructureAssembly = Assembly.Load(new AssemblyName("Coolector.Infrastructure"));
            builder.RegisterAssemblyTypes(coreAssembly).AsClosedTypesOf(typeof(IEventHandler<>));
            builder.RegisterAssemblyTypes(infrastructureAssembly).AsClosedTypesOf(typeof(ICommandHandler<>));
            builder.RegisterAssemblyTypes(infrastructureAssembly).AsClosedTypesOf(typeof(IEventHandler<>));
        }
    }
}