using CodersCupAward.Models;
using CodersCupAward.Services;
using Microsoft.EntityFrameworkCore;

namespace CodersCupAward.Helper
{
    public class IterationHelper : IIterationHelper
    {
        private readonly IIterationService _iterationService;

       
        public IterationHelper(IIterationService iterationService)            
        {
            _iterationService = iterationService;
        }
        public async Task DeleteAsync(Iteration iteration)
        {
            await _iterationService.DeleteAsync(iteration);
        }

        public async Task<List<Iteration>> GetAllAsync(int organizationId)
        {
            return await _iterationService.GetAllAsync(organizationId);
        }

        public async Task<int> SaveAsync(Iteration iteration)
        {
           return await _iterationService.SaveAsync(iteration);
        }

    }
}
