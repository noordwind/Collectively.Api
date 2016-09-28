using System.Threading.Tasks;

namespace Coolector.Services.Mongo
{
    public interface IDatabaseSeeder
    {
        Task SeedAsync();
    }
}