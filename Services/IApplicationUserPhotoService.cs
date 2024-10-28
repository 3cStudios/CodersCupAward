using CodersCupAward.Models;

namespace CodersCupAward.Services;

public interface IApplicationUserPhotoService
{
    Task DeleteAsync(ApplicationUserPhoto applicationUserPhoto);
    Task<ApplicationUserPhoto> GetAsync(int applicationUserPhotosId);
    Task<List<ApplicationUserPhoto>> GetListAsync(int applicationUserId);
    Task<int> SaveAsync(ApplicationUserPhoto applicationUserPhoto);
}