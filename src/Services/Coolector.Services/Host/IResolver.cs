namespace Coolector.Services.Host
{
    public interface IResolver
    {
        T Resolve<T>();
    }
}