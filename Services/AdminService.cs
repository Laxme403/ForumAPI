using dev_forum_api.Models;

namespace dev_forum_api.Services
{
    public class AdminService
    {
        // Define predefined admin emails here
        private static readonly HashSet<string> AdminEmails = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "admin@devforum.com",
            "magic@admin.com",  // Replace with your actual admin email
            "developer@devforum.com"
            // Add more admin emails as needed
        };

        /// <summary>
        /// Determines if an email should have admin privileges
        /// </summary>
        public static bool IsAdminEmail(string email)
        {
            return AdminEmails.Contains(email);
        }

        /// <summary>
        /// Assigns the appropriate role based on email
        /// </summary>
        public static string GetRoleForEmail(string email)
        {
            return IsAdminEmail(email) ? "Admin" : "User";
        }
    }
}
