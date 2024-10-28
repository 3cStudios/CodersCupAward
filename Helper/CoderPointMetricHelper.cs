using CodersCupAward.Models;
using CodersCupAward.Services;
using Microsoft.EntityFrameworkCore;

namespace CodersCupAward.Helper
{
    public class CoderPointMetricHelper : ICoderPointMetricHelper
    {
        private readonly ICoderPointMetricService _coderPointMetricService;


        public CoderPointMetricHelper(ICoderPointMetricService coderPointMetricService)
        {
            _coderPointMetricService = coderPointMetricService;
        }
        public async Task DeleteAsync(CoderPointMetric coderPointMetric)
        {
            await _coderPointMetricService.DeleteAsync(coderPointMetric);

        }

        public async Task<List<CoderPointMetric>> GetAllAsync(int organizationId)
        {
            return await _coderPointMetricService.GetAllAsync(organizationId);
        }

        public async Task<int> SaveAsync(CoderPointMetric coderPointMetric)
        {
            return await _coderPointMetricService.SaveAsync(coderPointMetric);
        }

    }
}
