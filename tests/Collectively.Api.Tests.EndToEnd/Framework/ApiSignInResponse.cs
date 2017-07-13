namespace Collectively.Api.Tests.EndToEnd.Framework
{
    public class ApiSignInResponse
    {
        public string Token { get; set; }
        public string SessionId { get; set; }
        public string SessionKey { get; set; }
        public long Expiry { get; set; }
    }
}