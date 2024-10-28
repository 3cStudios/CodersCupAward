using CodersCupAward.Models;
using CodersCupAward.Services;

namespace CodersCupAward.Helper
{
    public class EmailLogHelper : IEmailLogHelper
    {
        
        private readonly IEmailLogService _emailLogService;
        

        public EmailLogHelper( IEmailLogService emailLogService)
        {
            _emailLogService = emailLogService;
        }
        
        public async Task<EmailLog> GetAsync(int id)
        {
            return await _emailLogService.GetAsync(id);
        }
        public async Task<(List<EmailLog> List, int TotalCount)> GetAllAsync(int take, int skip)
        {
            return await _emailLogService.GetAllAsync(take, skip);
        }
        public async Task<int> SaveAsync(EmailLog entity)
        {
            return await _emailLogService.SaveAsync(entity);
        }

    }
}