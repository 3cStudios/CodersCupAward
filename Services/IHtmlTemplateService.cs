using CodersCupAward.Models;

namespace CodersCupAward.Services;

public interface IHtmlTemplateService
{
    Task DeleteAsync(int tenantContactId);
    Task<List<HtmlTemplate>> GetByTenantAsync();
    Task<HtmlTemplate> GetByIdAsync(int tenantContactId);
    Task<HtmlTemplate> GetAsync(string templateName);
    Task<int> SaveAsync(HtmlTemplate entity);
}