using CodersCupAward.Models;

namespace CodersCupAward.Helper;

public interface IEmailLogHelper
{
    Task<EmailLog> GetAsync(int id);
    Task<(List<EmailLog> List, int TotalCount)> GetAllAsync(int take, int skip);
    Task<int> SaveAsync(EmailLog entity);
}