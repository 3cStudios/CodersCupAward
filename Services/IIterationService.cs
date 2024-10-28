using CodersCupAward.Models;

namespace CodersCupAward.Services;

public interface IIterationService
{
    Task DeleteAsync(Iteration iteration);
    Task<List<Iteration>> GetAllAsync(int organizationId);
    Task<int> SaveAsync(Iteration iteration);
}