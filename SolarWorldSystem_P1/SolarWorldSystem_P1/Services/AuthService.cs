using System;
using System.Linq;
using SolarWorldERP.Data;
using SolarWorldERP.Helpers;

namespace SolarWorldERP.Services
{
    /// <summary>Handles checking a username and password.</summary>
    public static class AuthService
    {
        /// <summary>
        /// Looks up the user, hashes the typed password and compares it with the stored hash.
        /// Returns the User on success, or null if the username or password is wrong or the user is inactive.
        /// </summary>
        public static User Login(string userName, string password)
        {
            using (var db = new ErpDataContext())
            {
                var user = db.Users.FirstOrDefault(u => u.UserName == userName && u.IsActive);
                if (user == null) return null;
                bool ok = string.Equals(user.PasswordHash, PasswordHelper.Hash(password),
                                        StringComparison.OrdinalIgnoreCase);
                return ok ? user : null;
            }
        }
    }
}
