using AutoMapper;
using Coolector.Common.DTO.Users;

namespace Coolector.Services.Storage.Framework
{
    public class MapperConfigurationFactory
    {
        private static MapperConfiguration _mapperConfiguration;

        public static MapperConfiguration Create()
        {
            if (_mapperConfiguration == null)
                _mapperConfiguration = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<dynamic, UserDto>();
                });

            return _mapperConfiguration;
        }
    }
}