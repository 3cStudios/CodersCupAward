using CodersCupAward.Models;

namespace CodersCupAward.Extensions
{
    public static class ApplicationUserExtensions
    {
        public static string FullName(this ApplicationUser applicationUser)
        {
            if (applicationUser == null)
                return string.Empty;
            
            return $"{applicationUser.FirstName} {applicationUser.LastName}";
        }
    }
}
