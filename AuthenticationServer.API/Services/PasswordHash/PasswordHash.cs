﻿

using System.Security.Cryptography;

namespace AuthenticationServer.API.Services.PasswordHasher
{
    public class PasswordHash : IPasswordHash
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);

        }


        public bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);

        }


    }
}
