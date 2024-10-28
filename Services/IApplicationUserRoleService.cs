using CodersCupAward.Models;

namespace CodersCupAward.Services;

public interface IApplicationUserRoleService
{
    Task DeleteAsync(ApplicationUserRole applicationUserRole);
    Task<List<ApplicationUserRole>> GetByUserIdAsync(int applicationUserId);
    Task<int> SaveAsync(ApplicationUserRole applicationUserRole);
}