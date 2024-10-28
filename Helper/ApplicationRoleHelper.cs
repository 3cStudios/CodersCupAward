using CodersCupAward.Models;
using CodersCupAward.Services;

namespace CodersCupAward.Helper
{
    public class ApplicationRoleHelper : IApplicationRoleHelper
    {
        private readonly IApplicationRoleService _applicationRoleService;


        public ApplicationRoleHelper(IApplicationRoleService applicationRoleService)
        {
            _applicationRoleService = applicationRoleService;
            
        }
       
        public async Task<List<ApplicationRoles>> GetAllAsync()
        {
            return await _applicationRoleService.GetAllAsync();
        }
        
    }
}
