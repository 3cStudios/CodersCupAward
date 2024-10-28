using System.Net.Mail;

namespace CodersCupAward.Helper
{
    public static class ValidationHelper
    {
        
        public static bool IsValidEmail(string email)
        {
            try
            {
                var mail = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        
    }
}
