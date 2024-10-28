using CodersCupAward.Models;
using CodersCupAward.Services;

namespace CodersCupAward.Helper
{
    public class CoderPointTrackingHelper : ICoderPointTrackingHelper
    {
        private readonly ICoderPointTrackingService _coderPointTrackingService;


        public CoderPointTrackingHelper(ICoderPointTrackingService coderPointTrackingService)
        {
            _coderPointTrackingService = coderPointTrackingService;
        }
        public async Task DeleteAsync(CoderPointTracking coderPointTracking)
        {
            await _coderPointTrackingService.DeleteAsync(coderPointTracking);
        }

        public async Task<List<CoderPointTracking>> GetAllAsync(int organizationId)
        {
            return await _coderPointTrackingService.GetAllAsync(organizationId);
        }

        public async Task<int> SaveAsync(CoderPointTracking coderPointTracking)
        {
            return await _coderPointTrackingService.SaveAsync(coderPointTracking);

        }

    }
}
