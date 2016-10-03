namespace Coolector.Services.Storage.Mappers
{
    public interface IMapperResolver
    {
        IMapper<T> Resolve<T>();
        ICollectionMapper<T> ResolveForCollection<T>();
    }
}