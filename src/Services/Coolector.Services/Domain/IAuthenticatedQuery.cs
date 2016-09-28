namespace Coolector.Services.Domain
{
    public interface IAuthenticatedQuery : IQuery
    {
        string AuthenticatedUserId { get; set; }
    }
}