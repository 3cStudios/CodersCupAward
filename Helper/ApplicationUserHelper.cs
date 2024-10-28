using CodersCupAward.Models;
using CodersCupAward.ViewModels;
using CodersCupAward.Services;

namespace CodersCupAward.Helper
{
    public class ApplicationUserHelper : IApplicationUserHelper
    {
        private readonly IApplicationUserService _applicationUserService;
        private readonly IApplicationUserPhotoHelper _applicationUserPhotoHelper;
        private readonly IApplicationUserRoleHelper _applicationUserRoleHelper;
       
        public ApplicationUserHelper(IApplicationUserService applicationUserService, 
            IApplicationUserPhotoHelper applicationUserPhotoHelper,
            IApplicationUserRoleHelper applicationUserRoleHelper
            )
        {
            _applicationUserService = applicationUserService;
            _applicationUserPhotoHelper = applicationUserPhotoHelper;
            _applicationUserRoleHelper = applicationUserRoleHelper;
        }

        public async Task DeleteAsync(ApplicationUser applicationUser)
        {
            await _applicationUserService.DeleteAsync(applicationUser);
        }

        public async Task<ApplicationUser> GetByUserIdAsync(int applicationUserId)
        {
            return await _applicationUserService.GetByUserIdAsync(applicationUserId);
        }

        public async Task<ApplicationUser> GetByUserUIdAsync(Guid uId)
        {
            return await _applicationUserService.GetByUserUIdAsync(uId);
        }
        public async Task<ApplicationUser> GetByUserEmailAsync(string emailAddress)
        {
            return await _applicationUserService.GetByUserEmailAsync(emailAddress);
        }
        

        public async Task<List<ApplicationUser>> GetByOrganizationIdAsync(int organizationId)
        {
            return await _applicationUserService.GetByOrganizationIdAsync(organizationId);
        }
        public async Task<ApplicationUser> Login(LoginModel loginModel)
        {
            return await _applicationUserService.Login(loginModel);
        }

        public async Task<ApplicationUser> GetByEmailAsync(string email)
        {
            return await _applicationUserService.GetByEmailAsync(email);
        }

        public async Task<List<ApplicationUser>> GetAllAsync()
        {
            return await _applicationUserService.GetAllAsync();
        }

        private async Task ProcessPhotos(ApplicationUser applicationUser)
        {
            if (applicationUser.ApplicationUserPhoto.Count <= 0) return;
            foreach (var photo in applicationUser.ApplicationUserPhoto)
            {
                if (photo.ApplicationUserPhotoId > 0) continue;
                photo.CreatedBy = applicationUser.CreatedBy;
                photo.CreatedDate = DateTime.UtcNow;
                photo.ApplicationUserId = applicationUser.ApplicationUserId;
                await _applicationUserPhotoHelper.SaveAsync(photo);
            }
        }

        private async Task ProcessRoles(ApplicationUser applicationUser)
        {
            if (applicationUser.ApplicationUserRole.Count <= 0) return;
            foreach (var role in applicationUser.ApplicationUserRole)
            {
                if (role.ApplicationUserRoleId > 0) continue;
                role.CreatedBy = applicationUser.CreatedBy;
                role.CreatedDate = DateTime.UtcNow;
                role.ApplicationUserId = applicationUser.ApplicationUserId;
                await _applicationUserRoleHelper.SaveAsync(role);
            }
        }

        public async Task<int> SaveAsync(ApplicationUser applicationUser)
        {
            var result= await _applicationUserService.SaveAsync(applicationUser);
            
            if (result > 0)
            {
                await ProcessPhotos(applicationUser);
                await ProcessRoles(applicationUser);
            }
            
            return result;
        }

       

    }
}
