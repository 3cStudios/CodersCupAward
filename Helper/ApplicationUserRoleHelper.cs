using CodersCupAward.Models;
using CodersCupAward.Services;


namespace CodersCupAward.Helper
{
    public class ApplicationUserRoleHelper : IApplicationUserRoleHelper
    {
        private readonly IApplicationUserRoleService _applicationUserRoleService;
        public ApplicationUserRoleHelper( IApplicationUserRoleService applicationUserRoleService)
        {
            _applicationUserRoleService = applicationUserRoleService;
            
        }

        public async Task DeleteAsync(ApplicationUserRole applicationUserRole)
        {
            await _applicationUserRoleService.DeleteAsync(applicationUserRole);
        }
               
        public async Task<List<ApplicationUserRole>> GetByUserIdAsync(int applicationUserId)
        {
            return await _applicationUserRoleService.GetByUserIdAsync(applicationUserId);
        }

        public async Task<int> SaveAsync(ApplicationUserRole applicationUserRole)
        {
            return await _applicationUserRoleService.SaveAsync(applicationUserRole);
        }

    }
}
