using System.Threading.Tasks;

namespace Coolector.Services.Mongo
{
    public interface IDatabaseInitializer
    {
        Task InitializeAsync();
    }
}