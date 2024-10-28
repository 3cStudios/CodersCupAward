using CodersCupAward.Exceptions;
using CodersCupAward.Models;
using Microsoft.EntityFrameworkCore;

namespace CodersCupAward.Services
{
    public class HtmlTemplateService : NoTrackingRepository<HtmlTemplate>, IHtmlTemplateService
    {
        public HtmlTemplateService(coderscupawardContext context)
            :base(context)
        {
            // Do not access context or dbset locally.
            // All data access through base class to enforce no tracking conventions.
        }

        public async Task DeleteAsync(int tenantContactId)
        {
            // Get existing record. (Prefer exception if not found or just ignore?
            var entity = await GetByIdAsync(tenantContactId).ConfigureAwait(true);
            if (entity != null)
            {
                // Remove the entity.
                await RemoveAsync(entity).ConfigureAwait(true);
            }
        }

        public async Task<List<HtmlTemplate>> GetByTenantAsync()
        {
            return await Get().ToListAsync().ConfigureAwait(true);
        }
        
        public async Task<HtmlTemplate> GetByIdAsync(int tenantContactId)
        {
            // Return single record by PK.
            var entity= await Get().FirstOrDefaultAsync(c => c.HtmlTemplateId == tenantContactId).ConfigureAwait(true);
            return entity ?? new HtmlTemplate();
        }

        public async Task<HtmlTemplate> GetAsync(string templateName)
        {
            var result = await Get().FirstOrDefaultAsync(r => r.TemplateName == templateName).ConfigureAwait(true);
            if (result == null)
                throw new MissingTemplateException(templateName + "is missing");

            return result;
        }

       
        public async Task<int> SaveAsync(HtmlTemplate entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            if (entity.HtmlTemplateId == 0)
            {
                // Add the entity.
                await AddAsync(entity).ConfigureAwait(true);
            }
            else
            {
                // Get existing record. (Prefer exception if not found or just ignore?
                var existingEntity = await GetByIdAsync(entity.HtmlTemplateId).ConfigureAwait(true);
                if (existingEntity == null) { throw new Exception("Update could not be completed because existing item could not be found."); }

                // Update existing entity with appropriate values for provided entity.
                MapNewToExisting(entity, ref existingEntity);

                // Update the entity.
                await UpdateAsync(existingEntity).ConfigureAwait(true);
            }

            // Return the id for the saved record.
            return entity.HtmlTemplateId;
        }

        private static void MapNewToExisting(HtmlTemplate mapFrom, ref HtmlTemplate mapTo)
        {
            mapTo.HtmlTemplateId = mapFrom.HtmlTemplateId;
            mapTo.TemplateName = mapFrom.TemplateName;
            mapTo.HTML = mapFrom.HTML;
            mapTo.HtmlTemplateId = mapFrom.HtmlTemplateId;
            mapTo.CreatedBy = mapFrom.CreatedBy;
            mapTo.CreatedDate = mapFrom.CreatedDate;
            mapTo.ModifiedBy = mapFrom.ModifiedBy;
            mapTo.ModifiedDate = mapFrom.ModifiedDate;

      
        }
    }
}
