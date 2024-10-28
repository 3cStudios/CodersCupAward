using CodersCupAward.Models;

namespace CodersCupAward.Services;

public interface IApplicationSessionService
{
    Task<ApplicationSession> GetBySessionIdAsync(Guid uId);
    Task<Guid> SaveAsync(ApplicationSession applicationSession);
}