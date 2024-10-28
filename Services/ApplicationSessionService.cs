using CodersCupAward.Models;
using Microsoft.EntityFrameworkCore;


namespace CodersCupAward.Services
{
    public class ApplicationSessionService : NoTrackingRepository<ApplicationSession>, IApplicationSessionService
    {
        private readonly coderscupawardContext _dbContext;

       
        public ApplicationSessionService(coderscupawardContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<ApplicationSession> GetBySessionIdAsync(Guid uId)
        {
            var result = await Get()
                .FirstOrDefaultAsync(c => c.ApplicationSessionId == uId);
            return result ?? new ApplicationSession();
        }

        public async Task<Guid> SaveAsync(ApplicationSession applicationSession)
        {
            if (applicationSession == null) throw new ArgumentNullException(nameof(applicationSession));
            var existingEntity = await GetBySessionIdAsync(applicationSession.ApplicationSessionId);

            if (existingEntity.ApplicationSessionId == Guid.Empty)
            {
                applicationSession.CreatedDate = DateTime.UtcNow;
                await AddAsync(applicationSession);
            }
            else
            {
                MapNewToExisting(applicationSession, ref existingEntity);
                existingEntity.ModifiedDate = DateTime.UtcNow;
                await UpdateAsync(existingEntity);
            }

            
            return applicationSession.ApplicationSessionId;
        }
        private static void MapNewToExisting(ApplicationSession mapFrom, ref ApplicationSession mapTo)
        {
            mapTo.ApplicationSessionId = mapFrom.ApplicationSessionId;
            mapTo.SessionData = mapFrom.SessionData;
            
        }
    }
}
