namespace Coolector.Api.Authentication
{
    public interface IJwtTokenHandler
    {
        string Create(string userId);
        JwtToken GetFromAuthorizationHeader(string authorizationHeader);
        bool IsValid(JwtToken token);
    }
}