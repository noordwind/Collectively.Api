using System.Threading.Tasks;

namespace Coolector.Tests.EndToEnd.Framework
{
    public interface IAuth0Client
    {
        Task<Auth0SignInResponse> SignInAsync(string username, string password);
    }
}