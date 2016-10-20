using Coolector.Common.Queries;

namespace Coolector.Api.Queries
{
    public class GetAccount : IAuthenticatedQuery
    {
        public string UserId { get; set; }
    }
}