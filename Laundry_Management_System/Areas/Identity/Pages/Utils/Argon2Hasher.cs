using Konscious.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Cryptography;

namespace Laundry_Management_System.Utils
{
    public class Argon2Hasher<TUser> : IPasswordHasher<TUser> where TUser : class
    {
        private const int SaltSize = 16;
        private const int HashSize = 16;
        private readonly ILogger<Argon2Hasher<TUser>> _logger;

        public Argon2Hasher(ILogger<Argon2Hasher<TUser>> logger)
        {
            _logger = logger;
        }

        public string HashPassword(TUser user, string password)
        {
            byte[] hashedBytes = HashPassword(password);
            return Convert.ToBase64String(hashedBytes);
        }

        public PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
        {
            try
            {
                byte[] hashedPasswordBytes = Convert.FromBase64String(hashedPassword);

                int saltSize = 16;
                byte[] salt = new byte[saltSize];
                Buffer.BlockCopy(hashedPasswordBytes, 0, salt, 0, saltSize);

                int hashSize = 16;
                byte[] expectedSubkey = new byte[hashSize];
                Buffer.BlockCopy(hashedPasswordBytes, saltSize, expectedSubkey, 0, hashSize);

                byte[] providedPasswordBytes = System.Text.Encoding.UTF8.GetBytes(providedPassword);
                byte[] actualSubkey = GenerateArgon2Hash(providedPasswordBytes, salt);

                if (ByteArraysEqual(actualSubkey, expectedSubkey))
                {
                    return PasswordVerificationResult.Success;
                }
                else
                {
                    return PasswordVerificationResult.Failed;
                }
            }
            catch
            {
                return PasswordVerificationResult.Failed;
            }
        }

        private byte[] HashPassword(string password)
        {
            byte[] salt = CreateSalt();
            byte[] hashedPassword = GenerateArgon2Hash(System.Text.Encoding.UTF8.GetBytes(password), salt);

            byte[] outputBytes = new byte[salt.Length + hashedPassword.Length];
            Buffer.BlockCopy(salt, 0, outputBytes, 0, salt.Length);
            Buffer.BlockCopy(hashedPassword, 0, outputBytes, salt.Length, hashedPassword.Length);

            return outputBytes;
        }

        private byte[] GenerateArgon2Hash(byte[] password, byte[] salt)
        {
            int degreeOfParallelism = 8;
            int iterations = 4;
            int memorySize = 1024 * 1024;

            var argon2 = new Argon2id(password)
            {
                Salt = salt,
                DegreeOfParallelism = degreeOfParallelism,
                Iterations = iterations,
                MemorySize = memorySize
            };

            return argon2.GetBytes(HashSize);
        }

        private byte[] CreateSalt()
        {
            var buffer = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(buffer);
            }

            return buffer;
        }

        private bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (a == null && b == null)
            {
                return true;
            }
            if (a == null || b == null || a.Length != b.Length)
            {
                return false;
            }
            var areSame = true;
            for (var i = 0; i < a.Length; i++)
            {
                areSame &= (a[i] == b[i]);
            }
            return areSame;
        }
    }
}
