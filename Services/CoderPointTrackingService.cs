using CodersCupAward.Models;
using Microsoft.EntityFrameworkCore;


namespace CodersCupAward.Services
{
    public class CoderPointTrackingService : NoTrackingRepository<CoderPointTracking>, ICoderPointTrackingService
    {
        private readonly coderscupawardContext _dbContext;

       
        public CoderPointTrackingService(coderscupawardContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task DeleteAsync(CoderPointTracking coderPointTracking)
        {
            await UpdateAsync(coderPointTracking);
        }

        public async Task<List<CoderPointTracking>> GetAllAsync(int organizationId)
        {
            var result = await Get()
                .AsNoTracking()
                .Include(c => c.ApplicationUser)
                .Where(c => c.ApplicationUser.OrganizationId == organizationId)
                .ToListAsync();

            return result ?? new List<CoderPointTracking>();
        }
        public async Task<int> SaveAsync(CoderPointTracking coderPointTracking)
        {
            if (coderPointTracking == null) throw new ArgumentNullException(nameof(coderPointTracking));

            if (coderPointTracking.CoderPointTrackingId == 0)
            {
               
                await AddAsync(coderPointTracking);
            }
            else
            {
                await UpdateAsync(coderPointTracking);
            }

            // Return the id for the saved record.
            return coderPointTracking.CoderPointTrackingId;
        }

    }
}
