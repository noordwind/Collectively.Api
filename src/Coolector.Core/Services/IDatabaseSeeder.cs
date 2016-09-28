using System.Threading.Tasks;

namespace Coolector.Core.Services
{
    public interface IDatabaseSeeder
    {
        Task SeedAsync();
    }
}