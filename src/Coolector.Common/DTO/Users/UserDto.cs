using System;

namespace Coolector.Common.DTO.Users
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string PictureUrl { get; set; }
        public string Role { get; set; }
        public string State { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}