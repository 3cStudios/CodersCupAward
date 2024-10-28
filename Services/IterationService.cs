using CodersCupAward.Models;
using Microsoft.EntityFrameworkCore;


namespace CodersCupAward.Services
{
    public class IterationService : NoTrackingRepository<Iteration>, IIterationService
    {
        private readonly coderscupawardContext _dbContext;

       
        public IterationService(coderscupawardContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task DeleteAsync(Iteration iteration)
        {
            await UpdateAsync(iteration);
        }

        public async Task<List<Iteration>> GetAllAsync(int organizationId)
        {
            var result = await Get()
                .AsNoTracking()
                .Where(c => c.OrganizationId == organizationId)
                .ToListAsync();

            return result ?? new List<Iteration>();
        }

        public async Task<int> SaveAsync(Iteration iteration)
        {
            if (iteration == null) throw new ArgumentNullException(nameof(iteration));

            if (iteration.IterationId == 0)
            {
               
                await AddAsync(iteration);
            }
            else
            {
                await UpdateAsync(iteration);
            }

            // Return the id for the saved record.
            return iteration.IterationId;
        }

    }
}
