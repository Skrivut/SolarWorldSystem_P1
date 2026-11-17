using System;
using System.Security.Cryptography;
using System.Text;

namespace SolarWorldERP.Helpers
{
    public static class PasswordHelper
    {
        // SHA-256 as uppercase hex, matches CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', ...), 2) in SQL Server.
        public static string Hash(string password)
        {
            using (var sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password ?? ""));
                return BitConverter.ToString(bytes).Replace("-", "");
            }
        }
    }
}
