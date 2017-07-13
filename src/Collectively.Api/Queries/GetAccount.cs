using Collectively.Common.Queries;

namespace Collectively.Api.Queries
{
    public class GetAccount : IAuthenticatedQuery
    {
        public string UserId { get; set; }
    }
}