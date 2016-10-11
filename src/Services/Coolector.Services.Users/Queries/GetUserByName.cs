using Coolector.Common.Queries;

namespace Coolector.Services.Users.Queries
{
    public class GetUserByName : IQuery
    {
        public string Name { get; set; }
    }
}