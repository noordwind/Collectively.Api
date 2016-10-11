using Coolector.Common.Queries;

namespace Coolector.Services.Storage.Queries
{
    public class GetUserByName : IQuery
    {
        public string Name { get; set; }
    }
}