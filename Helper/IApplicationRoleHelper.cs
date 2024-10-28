using CodersCupAward.Models;

namespace CodersCupAward.Helper;

public interface IApplicationRoleHelper
{
    Task<List<ApplicationRoles>> GetAllAsync();
}