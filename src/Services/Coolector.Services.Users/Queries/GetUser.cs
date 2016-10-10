using Coolector.Common.Queries;

namespace Coolector.Services.Users.Queries
{
    public class GetUser : IQuery
    {
        public string Id { get; set; }
    }
}