using CodersCupAward.Models;

namespace CodersCupAward.Services;

public interface ICoderPointMetricService
{
    Task DeleteAsync(CoderPointMetric coderPointMetric);
    Task<List<CoderPointMetric>> GetAllAsync(int organizationId);
    Task<int> SaveAsync(CoderPointMetric coderPointMetric);
}