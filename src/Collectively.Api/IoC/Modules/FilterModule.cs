using System.Reflection;
using Autofac;
using Collectively.Api.Filters;
using Collectively.Common.Types;
using Module = Autofac.Module;

namespace Collectively.Api.IoC.Modules
{
    public class FilterModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FilterResolver>().As<IFilterResolver>();
            var assembly = typeof(Startup).GetTypeInfo().Assembly;
            builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(IFilter<,>));
        }
    }
}