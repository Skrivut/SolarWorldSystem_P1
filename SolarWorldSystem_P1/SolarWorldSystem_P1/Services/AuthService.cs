using System;
using System.Linq;
using SolarWorldERP.Data;
using SolarWorldERP.Helpers;

namespace SolarWorldERP.Services
{
    public static class AuthService
    {
        // Returns the user on success, or null if the username/password is wrong.
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
