using System;
using System.Text;
using Jose;
using NLog;
using Coolector.Common.Extensions;

namespace Coolector.Api.Authentication
{
    public class JwtTokenHandler : IJwtTokenHandler
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly JwtTokenSettings _settings;
        private readonly byte[] _jwtSecretKey;

        public JwtTokenHandler(JwtTokenSettings settings)
        {
            _settings = settings;
            _jwtSecretKey = Encoding.Unicode.GetBytes(settings.SecretKey);
        }

        public string Create(string userId)
        {
            var customPayload = new JwtToken
            {
                Sub = userId,
                Exp = DateTime.UtcNow.AddDays(_settings.ExpiryDays).Ticks
            };

            return JWT.Encode(customPayload, _jwtSecretKey, JwsAlgorithm.HS512);
        }

        public JwtToken GetFromAuthorizationHeader(string authorizationHeader)
        {
            var data = authorizationHeader.Trim().Split(' ');
            if (data.Length != 2)
                return null;
            if (data[0].Empty() || data[1].Empty())
                return null;

            var authorizationType = data[0].ToLowerInvariant();
            if (authorizationType != "bearer")
                return null;

            var jwtTwoken = data[1];
            try
            {
                return JWT.Decode<JwtToken>(jwtTwoken, _jwtSecretKey, JwsAlgorithm.HS512);
            }
            catch (Exception exception)
            {
                Logger.Error(exception, "JWT Token generation error.");

                return null;
            }
        }

        public bool IsValid(JwtToken token)
        {
            if (token == null)
                return false;

            var expiry = DateTime.FromBinary(token.Exp);

            return expiry > DateTime.UtcNow;
        }
    }
}