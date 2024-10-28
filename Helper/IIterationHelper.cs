using CodersCupAward.Models;

namespace CodersCupAward.Helper;

public interface IIterationHelper
{
    Task DeleteAsync(Iteration iteration);
    Task<List<Iteration>> GetAllAsync(int organizationId);
    Task<int> SaveAsync(Iteration iteration);
}