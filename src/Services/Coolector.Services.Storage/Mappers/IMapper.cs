namespace Coolector.Services.Storage.Mappers
{
    public interface IMapper<out T>
    {
        T Map(dynamic source);
    }
}