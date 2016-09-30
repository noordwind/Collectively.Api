using Coolector.Common.Types;

namespace Coolector.Services.Domain
{
    public interface IAuthenticatedQuery : IQuery
    {
        string AuthenticatedUserId { get; set; }
    }
}