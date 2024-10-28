using CodersCupAward.Models;
using Microsoft.EntityFrameworkCore;


namespace CodersCupAward.Services
{
    public class ApplicationRoleService : NoTrackingRepository<ApplicationRoles>, IApplicationRoleService
    {
        private readonly coderscupawardContext _dbContext;

       
        public ApplicationRoleService(coderscupawardContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }
       

        public async Task<List<ApplicationRoles>> GetAllAsync()
        {
            var result = await _dbContext.ApplicationRoles
                .AsNoTracking()
                .ToListAsync();

            return result ?? new List<ApplicationRoles>();
        }
        
    }
}
