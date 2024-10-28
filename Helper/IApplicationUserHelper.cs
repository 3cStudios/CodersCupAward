using CodersCupAward.Models;
using CodersCupAward.ViewModels;

namespace CodersCupAward.Helper;

public interface IApplicationUserHelper
{
    Task DeleteAsync(ApplicationUser applicationUser);
    Task<ApplicationUser> GetByUserIdAsync(int applicationUserId);
    Task<ApplicationUser> GetByUserUIdAsync(Guid uId);
    Task<ApplicationUser> GetByUserEmailAsync(string emailAddress);
    Task<ApplicationUser> Login(LoginModel loginModel);
    Task<ApplicationUser> GetByEmailAsync(string email);
    Task<List<ApplicationUser>> GetAllAsync();
    Task<int> SaveAsync(ApplicationUser applicationUser);
    Task<List<ApplicationUser>> GetByOrganizationIdAsync(int organizationId);
}