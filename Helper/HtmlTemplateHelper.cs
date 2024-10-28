using CodersCupAward.Models;
using CodersCupAward.Services;

namespace CodersCupAward.Helper
{
    public class HtmlTemplateHelper : IHtmlTemplateHelper
    {
        private readonly IHtmlTemplateService _tenantContactService;

        public HtmlTemplateHelper(IHtmlTemplateService tenantContactService)
        {
            this._tenantContactService = tenantContactService;
        }

        public async Task DeleteAsync(int tenantContactId)
        {
            await _tenantContactService.DeleteAsync(tenantContactId);
        }
        public async Task<int> HtmlTemplateUpdateAsync(HtmlTemplate tenantContact)
        {
            return await _tenantContactService.SaveAsync(tenantContact).ConfigureAwait(true);
        }

        public async Task<List<HtmlTemplate>> GetByTenantAsync()
        {
            return await _tenantContactService.GetByTenantAsync().ConfigureAwait(true);
        }
        
        public async Task<HtmlTemplate> HtmlTemplateByIdAsync(int tenantContactId)
        {
            return await _tenantContactService.GetByIdAsync(tenantContactId).ConfigureAwait(true);
        }
                
        public async Task<HtmlTemplate> GetAsync(string templateName)
        {
            return await _tenantContactService.GetAsync(templateName).ConfigureAwait(true);
        }

        
    }
}
