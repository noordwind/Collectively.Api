using System.Threading.Tasks;

namespace Coolector.Infrastructure.Services
{
    public interface IDatabaseSeeder
    {
        Task SeedAsync();
    }
}