using System;
using System.Security.Cryptography;
using System.Text;

namespace SolarWorldERP.Helpers
{
    /// <summary>Password hashing, so plain passwords are never stored in the database.</summary>
    public static class PasswordHelper
    {
        /// <summary>
        /// Turns a password into a SHA-256 hash written as uppercase hex text.
        /// Matches CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', ...), 2) in SQL Server,
        /// which is how the sample users were created.
        /// </summary>
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
