using CodersCupAward.Models;
using CodersCupAward.Services;


namespace CodersCupAward.Helper
{
    public class OrganizationHelper : IOrganizationHelper
    {
        private readonly IOrganizationService _organizationService;


        public OrganizationHelper(IOrganizationService organizationService)
        {
            _organizationService = organizationService;

        }

        public async Task DeleteAsync(Organization organization)
        {
            await _organizationService.DeleteAsync(organization);
        }

        public async Task<Organization> OrganizationByIdAsync(int organizationId)
        {
            return await _organizationService.OrganizationByIdAsync(organizationId);
        }

        public async Task<List<Organization>> OrganizationByNameAsync(string organizationName)
        {
            return await _organizationService.OrganizationByNameAsync(organizationName);
        }

        public async Task<List<Organization>> GetAllAsync()
        {
            return await _organizationService.GetAllAsync();
        }

        public async Task<int> SaveAsync(Organization organization)
        {
            return await _organizationService.SaveAsync(organization);
        }

    }
}
