using Coolector.Common.Queries;

namespace Coolector.Services.Storage.Queries
{
    public class GetUser : IQuery
    {
        public string Id { get; set; }
    }
}