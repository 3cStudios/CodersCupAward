using CodersCupAward.Models;

namespace CodersCupAward.Helper;

public interface IOrganizationHelper
{
    Task DeleteAsync(Organization organization);
    Task<Organization> OrganizationByIdAsync(int organizationId);
    Task<List<Organization>> OrganizationByNameAsync(string organizationName);
    Task<List<Organization>> GetAllAsync();
    Task<int> SaveAsync(Organization organization);
}