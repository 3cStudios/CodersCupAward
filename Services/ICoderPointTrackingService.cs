using CodersCupAward.Models;

namespace CodersCupAward.Services;

public interface ICoderPointTrackingService
{
    Task DeleteAsync(CoderPointTracking coderPointTracking);
    Task<int> SaveAsync(CoderPointTracking coderPointTracking);

    Task<List<CoderPointTracking>> GetAllAsync(int organizationId);
}