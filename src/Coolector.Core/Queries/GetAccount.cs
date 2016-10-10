using Coolector.Common.Queries;

namespace Coolector.Core.Queries
{
    public class GetAccount : IAuthenticatedQuery
    {
        public string UserId { get; set; }
    }
}