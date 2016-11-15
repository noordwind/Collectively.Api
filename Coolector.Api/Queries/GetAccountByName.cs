using Coolector.Common.Queries;

namespace Coolector.Api.Queries
{
    public class GetAccountByName : IQuery
    {
        public string Name { get; set; }
    }
}