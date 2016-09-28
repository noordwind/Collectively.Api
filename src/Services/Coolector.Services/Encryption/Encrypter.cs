using System;
using System.Security.Cryptography;

namespace Coolector.Services.Encryption
{
    public class Encrypter : IEncrypter
    {
        private const int MinSecureKeySize = 40;
        private const int MaxSecureKeySize = 60;
        private static readonly Random Random = new Random();

        public string GetRandomSecureKey()
        {
            var size = Random.Next(MinSecureKeySize, MaxSecureKeySize);
            var bytes = new byte[size];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);

                return Convert.ToBase64String(bytes);
            }
        }
    }
}