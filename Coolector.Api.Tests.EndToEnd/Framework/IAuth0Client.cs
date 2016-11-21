using System.Threading.Tasks;

namespace Coolector.Api.Tests.EndToEnd.Framework
{
    public interface IAuth0Client
    {
        Task<ApiSignInResponse> SignInAsync(string username, string password);
    }
}