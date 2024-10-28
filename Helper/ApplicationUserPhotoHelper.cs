using CodersCupAward.Models;
using CodersCupAward.Services;


namespace CodersCupAward.Helper
{
    public class ApplicationUserPhotoHelper : IApplicationUserPhotoHelper
    {
        private readonly IApplicationUserPhotoService _applicationUserPhotoService;
        public ApplicationUserPhotoHelper( IApplicationUserPhotoService applicationUserPhotoService )
        {
            _applicationUserPhotoService = applicationUserPhotoService;
            
        }

        public async Task DeleteAsync(ApplicationUserPhoto applicationUserPhoto)
        {
            await _applicationUserPhotoService.DeleteAsync(applicationUserPhoto);
        }

        public async Task<ApplicationUserPhoto> GetAsync(int applicationUserPhotosId)
        {
            return await _applicationUserPhotoService.GetAsync(applicationUserPhotosId);
        }

        public async Task<List<ApplicationUserPhoto>> GetListAsync(int applicationUserId)
        {
            return await _applicationUserPhotoService.GetListAsync(applicationUserId);
        }

        public async Task<int> SaveAsync(ApplicationUserPhoto applicationUserPhoto)
        {
            return await _applicationUserPhotoService.SaveAsync(applicationUserPhoto);
        }

    }
}
