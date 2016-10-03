using System.Reflection;
using Autofac;
using Coolector.Services.Storage.Mappers;
using Module = Autofac.Module;

namespace Coolector.Services.Storage.Framework.IoC
{
    public class MapperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MapperResolver>().As<IMapperResolver>();
            var coreAssembly = typeof(Startup).GetTypeInfo().Assembly;
            builder.RegisterAssemblyTypes(coreAssembly).AsClosedTypesOf(typeof(IMapper<>));
            builder.RegisterAssemblyTypes(coreAssembly).AsClosedTypesOf(typeof(ICollectionMapper<>));
        }
    }
}