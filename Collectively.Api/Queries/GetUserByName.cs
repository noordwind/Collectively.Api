using Collectively.Common.Queries;

namespace Collectively.Api.Queries
{
    public class GetUserByName : IQuery
    {
        public string Name { get; set; }
    }
}