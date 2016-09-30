using System.Reflection;
using Autofac;
using Coolector.Common.Types;
using Coolector.Core.Filters;
using Module = Autofac.Module;

namespace Coolector.Core.IoC.Modules
{
    public class FilterModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FilterResolver>().As<IFilterResolver>();
            var coreAssembly = Assembly.Load(new AssemblyName("Coolector.Core"));
            builder.RegisterAssemblyTypes(coreAssembly).AsClosedTypesOf(typeof(IFilter<,>));
        }
    }
}