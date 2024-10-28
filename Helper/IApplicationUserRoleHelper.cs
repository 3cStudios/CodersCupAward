using CodersCupAward.Models;

namespace CodersCupAward.Helper;

public interface IApplicationUserRoleHelper
{
    Task DeleteAsync(ApplicationUserRole applicationUserRole);
    Task<List<ApplicationUserRole>> GetByUserIdAsync(int applicationUserId);
    Task<int> SaveAsync(ApplicationUserRole applicationUserRole);
}