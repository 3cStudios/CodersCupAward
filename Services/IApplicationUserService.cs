using CodersCupAward.Models;
using CodersCupAward.ViewModels;

namespace CodersCupAward.Services;

public interface IApplicationUserService
{
    Task DeleteAsync(ApplicationUser applicationUser);
    Task<ApplicationUser> GetByUserIdAsync(int applicationUserId);
    Task<ApplicationUser> GetByUserUIdAsync(Guid uId);
    Task<ApplicationUser> Login(LoginModel loginModel);
    Task<ApplicationUser> GetByEmailAsync(string email);
    Task<List<ApplicationUser>> GetAllAsync();
    Task<int> SaveAsync(ApplicationUser applicationUser);
    Task<List<ApplicationUser>> GetByOrganizationIdAsync(int organizationId);
    Task<ApplicationUser> GetByUserEmailAsync(string emailAddress);
}