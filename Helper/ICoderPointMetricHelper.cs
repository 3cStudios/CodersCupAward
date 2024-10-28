using CodersCupAward.Models;

namespace CodersCupAward.Helper;

public interface ICoderPointMetricHelper
{
    Task DeleteAsync(CoderPointMetric coderPointMetric);
    Task<List<CoderPointMetric>> GetAllAsync(int organizationId);
    Task<int> SaveAsync(CoderPointMetric coderPointMetric);
}