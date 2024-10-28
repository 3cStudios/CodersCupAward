using CodersCupAward.Models;

namespace CodersCupAward.Services;

public interface IApplicationRoleService
{
    Task<List<ApplicationRoles>> GetAllAsync();
}