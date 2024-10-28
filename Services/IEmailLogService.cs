using CodersCupAward.Models;

namespace CodersCupAward.Services;

public interface IEmailLogService
{
    Task<EmailLog> GetAsync(int id);
    Task<(List<EmailLog> List, int TotalCount)> GetAllAsync(int take, int skip);
    Task<int> SaveAsync(EmailLog entity);
}