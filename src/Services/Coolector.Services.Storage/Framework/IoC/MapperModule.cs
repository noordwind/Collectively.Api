using Autofac;
using Coolector.Common.DTO.Users;
using Coolector.Services.Storage.Mappers;

namespace Coolector.Services.Storage.Framework.IoC
{
    public class MapperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserMapper>().As<IMapper<UserDto>>();
        }
    }
}