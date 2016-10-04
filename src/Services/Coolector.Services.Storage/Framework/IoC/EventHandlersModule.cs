using Autofac;
using Coolector.Common.Events;
using System.Reflection;
using Module = Autofac.Module;

namespace Coolector.Services.Storage.Framework.IoC
{
    public class EventHandlersModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var coreAssembly = typeof(Startup).GetTypeInfo().Assembly;
            builder.RegisterAssemblyTypes(coreAssembly).AsClosedTypesOf(typeof(IEventHandler<>));
        }
    }
}