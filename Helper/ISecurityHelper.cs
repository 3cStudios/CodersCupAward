using CodersCupAward.Models;

namespace CodersCupAward.Helper;

public interface ISecurityHelper
{
    string HashPassword(string password);
    Task SetRememberMeTokenAsync(ApplicationUser user);
    Task<string> GetRememberMeTokenAsync();
    Task RemoveRememberMeTokenAsync();
}