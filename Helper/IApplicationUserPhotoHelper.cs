using CodersCupAward.Models;

namespace CodersCupAward.Helper;

public interface IApplicationUserPhotoHelper
{
    Task DeleteAsync(ApplicationUserPhoto applicationUserPhoto);
    Task<ApplicationUserPhoto> GetAsync(int applicationUserPhotosId);
    Task<List<ApplicationUserPhoto>> GetListAsync(int applicationUserId);
    Task<int> SaveAsync(ApplicationUserPhoto applicationUserPhoto);
}