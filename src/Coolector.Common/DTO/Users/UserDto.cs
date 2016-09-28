using System;
using Coolector.Common.DTO.Common;

namespace Coolector.Common.DTO.Users
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public State State { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}