using CodersCupAward.Models;
using Microsoft.EntityFrameworkCore;

namespace CodersCupAward.Services
{
    public class ApplicationUserRoleService : NoTrackingRepository<ApplicationUserRole>, IApplicationUserRoleService
    {
        public ApplicationUserRoleService(coderscupawardContext context)
            : base(context)
        {
            // Where possible data access through base class to enforce no tracking conventions.
        }

        public async Task DeleteAsync(ApplicationUserRole applicationUserRole)
        {
            await RemoveAsync(applicationUserRole);
        }
               
        public async Task<List<ApplicationUserRole>> GetByUserIdAsync(int applicationUserId)
        {
            
            var result = await Get()
                .IgnoreQueryFilters()
                .Where(c => c.ApplicationUserId == applicationUserId)
                .ToListAsync();
            return result ?? new List<ApplicationUserRole>();
        }

        public async Task<int> SaveAsync(ApplicationUserRole applicationUserRole)
        {
            if (applicationUserRole == null) throw new ArgumentNullException(nameof(applicationUserRole));

            var existing = await GetByUserIdAsync(applicationUserRole.ApplicationUserId);
            if (existing.Any())
            {
                await DeleteAsync(existing.First());
            }
            await AddAsync(applicationUserRole);
            return applicationUserRole.ApplicationUserRoleId;
        }

    }
}
