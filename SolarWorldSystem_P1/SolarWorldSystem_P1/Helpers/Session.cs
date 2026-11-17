namespace SolarWorldERP.Helpers
{
    /// <summary>
    /// Remembers who is logged in while the program runs. frmLogin fills it in;
    /// every other form and service reads it (for example to show or hide menus by role).
    /// </summary>
    public static class Session
    {
        /// <summary>Login name, e.g. staff1. Also saved as CreatedBy / ApprovedBy on records.</summary>
        public static string UserName = "";

        /// <summary>Display name, e.g. Office Staff.</summary>
        public static string FullName = "";

        /// <summary>Staff, Owner or Driver.</summary>
        public static string Role = "";
    }
}
