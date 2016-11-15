using Coolector.Common.Queries;

namespace Coolector.Api.Queries
{
    public class GetNameAvailability : IQuery
    {
        public string Name { get; set; }
    }
}