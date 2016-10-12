using Newtonsoft.Json;

namespace Coolector.Services.Users.Auth0
{
    public class Auth0User
    {
        [JsonProperty("user_id")]
        public string UserId { get; set; }

        public string Email { get; set; }

        public string Picture { get; set; }

        [JsonProperty("username")]
        public string Name { get; set; }
    }
}