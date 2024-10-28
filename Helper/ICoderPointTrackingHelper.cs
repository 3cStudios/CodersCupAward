using CodersCupAward.Models;

namespace CodersCupAward.Helper;

public interface ICoderPointTrackingHelper
{
    Task DeleteAsync(CoderPointTracking coderPointTracking);
    Task<int> SaveAsync(CoderPointTracking coderPointTracking);
    Task<List<CoderPointTracking>> GetAllAsync(int organizationId);
}