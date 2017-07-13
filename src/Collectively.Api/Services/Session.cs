using System;

namespace Collectively.Api.Services
{
    public class Session
    {
        public string Token { get; set; }
        public Guid SessionId { get; set; }
        public string SessionKey { get; set; }
        public long Expiry { get; set; }
    }
}