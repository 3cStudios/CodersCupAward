using CodersCupAward.Models;

namespace CodersCupAward.Helper;

public interface IHtmlTemplateHelper
{
    Task DeleteAsync(int tenantContactId);
    Task<int> HtmlTemplateUpdateAsync(HtmlTemplate tenantContact);
    Task<List<HtmlTemplate>> GetByTenantAsync();
    Task<HtmlTemplate> HtmlTemplateByIdAsync(int tenantContactId);
    Task<HtmlTemplate> GetAsync(string templateName);
}