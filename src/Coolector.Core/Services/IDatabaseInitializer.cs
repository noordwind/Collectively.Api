using System.Threading.Tasks;

namespace Coolector.Core.Services
{
    public interface IDatabaseInitializer
    {
        Task InitializeAsync();
    }
}