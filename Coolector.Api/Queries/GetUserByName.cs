using Coolector.Common.Queries;

namespace Coolector.Api.Queries
{
    public class GetUserByName : IQuery
    {
        public string Name { get; set; }
    }
}