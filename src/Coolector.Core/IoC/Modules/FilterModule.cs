using System.Reflection;
using Autofac;
using Coolector.Common.Types;
using Module = Autofac.Module;

namespace Coolector.Core.IoC.Modules
{
    public class FilterModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var coreAssembly = Assembly.Load(new AssemblyName("Coolector.Core"));
            builder.RegisterAssemblyTypes(coreAssembly).AsClosedTypesOf(typeof(IFilter<,>));
        }
    }
}