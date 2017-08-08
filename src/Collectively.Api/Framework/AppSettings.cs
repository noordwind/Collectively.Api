using System;

namespace Collectively.Api.Framework
{
    public class AppSettings
    {
        public string AvatarUrl { get; set; }
        public string ResetPasswordUrl { get; set; }
        public bool ValidateAccountState { get; set; }
    }
}