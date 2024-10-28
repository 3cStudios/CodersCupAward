using CodersCupAward.Models;
using Microsoft.EntityFrameworkCore;

namespace CodersCupAward.Services
{
    public class ApplicationUserPhotoService : NoTrackingRepository<ApplicationUserPhoto>, IApplicationUserPhotoService
    {
        public ApplicationUserPhotoService(coderscupawardContext context)
            : base(context)
        {
            // Where possible data access through base class to enforce no tracking conventions.
        }

        public async Task DeleteAsync(ApplicationUserPhoto applicationUserPhoto)
        {
            applicationUserPhoto.IsDeleted = true;
            applicationUserPhoto.ModifiedDate = DateTime.UtcNow;
            applicationUserPhoto.ModifiedBy = applicationUserPhoto.ModifiedBy;
            await UpdateAsync(applicationUserPhoto).ConfigureAwait(true);
        }

        public async Task<ApplicationUserPhoto> GetAsync(int applicationUserPhotosId)
        {
            
            var result = await Get().FirstOrDefaultAsync(c => c.ApplicationUserPhotoId == applicationUserPhotosId).ConfigureAwait(true);
            return result ?? new ApplicationUserPhoto();
        }

        public async Task<List<ApplicationUserPhoto>> GetListAsync(int applicationUserId)
        {
            
            var result = await Get()
                .IgnoreQueryFilters()
                .Where(c => c.ApplicationUserId == applicationUserId
                            && c.IsDeleted == false)
                .ToListAsync();
            return result ?? new List<ApplicationUserPhoto>();
        }

        public async Task<int> SaveAsync(ApplicationUserPhoto applicationUserPhoto)
        {
            if (applicationUserPhoto == null) throw new ArgumentNullException(nameof(applicationUserPhoto));


            await AddAsync(applicationUserPhoto).ConfigureAwait(true);
            return applicationUserPhoto.ApplicationUserPhotoId;
        }

    }
}
