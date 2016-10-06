using System.Dynamic;
using Coolector.Services.Storage.Mappers;

namespace Coolector.Tests.Services.Storage.Mappers
{
    public abstract class Mapper_specs<T>
    {
        protected static IMapper<T> Mapper;
        protected static T Result;
        protected static dynamic Source;

        protected static void Initialize(IMapper<T> mapper)
        {
            Mapper = mapper;
            Source = new ExpandoObject();
        }

        protected static void Map()
        {
            Result = Mapper.Map(Source);
        }
    }
}