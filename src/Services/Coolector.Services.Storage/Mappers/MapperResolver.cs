using Autofac;

namespace Coolector.Services.Storage.Mappers
{
    public class MapperResolver : IMapperResolver
    {
        private readonly IComponentContext _context;

        public MapperResolver(IComponentContext context)
        {
            _context = context;
        }

        public IMapper<T> Resolve<T>()
        {
            return _context.Resolve<IMapper<T>>();
        }

        public ICollectionMapper<T> ResolveForCollection<T>()
        {
            return _context.Resolve<ICollectionMapper<T>>();
        }
    }
}