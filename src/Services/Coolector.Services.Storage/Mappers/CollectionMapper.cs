using System.Collections.Generic;
using System.Linq;

namespace Coolector.Services.Storage.Mappers
{
    public abstract class CollectionMapper<T> : ICollectionMapper<T>
    {
        protected readonly IMapper<T> Mapper;

        protected CollectionMapper(IMapper<T> mapper)
        {
            Mapper = mapper;
        }
        
        public IEnumerable<T> Map(IEnumerable<object> source)
        {
            return source.Select(item => Mapper.Map(item));
        }
    }
}