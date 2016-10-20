using Coolector.Common.Queries;

namespace Coolector.Api.Queries
{
    public class GetAccoutByName : IQuery
    {
        public string Name { get; set; }
    }
}