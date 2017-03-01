using Collectively.Common.Queries;

namespace Collectively.Api.Queries
{
    public class GetNameAvailability : IQuery
    {
        public string Name { get; set; }
    }
}