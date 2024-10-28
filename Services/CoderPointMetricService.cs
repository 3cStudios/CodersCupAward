using CodersCupAward.Models;
using Microsoft.EntityFrameworkCore;


namespace CodersCupAward.Services
{
    public class CoderPointMetricService : NoTrackingRepository<CoderPointMetric>, ICoderPointMetricService
    {
        private readonly coderscupawardContext _dbContext;

       
        public CoderPointMetricService(coderscupawardContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task DeleteAsync(CoderPointMetric coderPointMetric)
        {
            await UpdateAsync(coderPointMetric);
        }

        public async Task<List<CoderPointMetric>> GetAllAsync(int organizationId)
        {
            var result = await Get()
                .AsNoTracking()
                .Where(c => c.OrganizationId == organizationId)
                .ToListAsync();

            return result ?? new List<CoderPointMetric>();
        }

        public async Task<int> SaveAsync(CoderPointMetric coderPointMetric)
        {
            if (coderPointMetric == null) throw new ArgumentNullException(nameof(coderPointMetric));

            if (coderPointMetric.CoderPointMetricId == 0)
            {
               
                await AddAsync(coderPointMetric);
            }
            else
            {
                await UpdateAsync(coderPointMetric);
            }

            // Return the id for the saved record.
            return coderPointMetric.CoderPointMetricId;
        }

    }
}
