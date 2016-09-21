using System.Threading.Tasks;

namespace Coolector.Infrastructure.Services
{
    public interface IDatabaseInitializer
    {
        Task InitializeAsync();
    }
}