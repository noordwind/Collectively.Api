using Newtonsoft.Json;

namespace Coolector.Infrastructure.DTO.Users
{
    public class Auth0User
    {
        [JsonProperty("user_id")]
        public string UserId { get; set; }

        public string Email { get; set; }

        public string Picture { get; set; }
    }
}