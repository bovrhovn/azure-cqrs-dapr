﻿using System.Linq;
using System.Security.Cryptography;

namespace CityApp.Engine
{
    public static class RandomGenerator
    {
        private const string AllowableCharacters = "abcdefghijklmnopqrstuvwxyz0123456789";

        public static string GenerateString(int length)
        {
            var bytes = new byte[length];

            using (var random = RandomNumberGenerator.Create())
            {
                random.GetBytes(bytes);
            }

            return new string(bytes.Select(x => AllowableCharacters[x % AllowableCharacters.Length])
                                   .ToArray());
        }
    }
}