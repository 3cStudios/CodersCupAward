using CodersCupAward.Exceptions;
using CodersCupAward.Models;
using Microsoft.EntityFrameworkCore;

namespace CodersCupAward.Services
{
    public class EmailLogService : NoTrackingRepository<EmailLog>, IEmailLogService
    {
        public EmailLogService(coderscupawardContext context)
            : base(context)
        {
            // Do not access context or dbset locally.
            // All data access through base class to enforce no tracking conventions.
        }


        public async Task<EmailLog> GetAsync(int id)
        {
            // Return single record by PK.
            var result = await Get().FirstOrDefaultAsync(c => c.EmailLogId == id).ConfigureAwait(true);
            if (result == null)
            {
                throw new MissingRecordException(id + " is missing");
            }
            return result;
        }

        public async Task<(List<EmailLog> List, int TotalCount)> GetAllAsync(int take, int skip)
        {
            var resultCount = await Get()
                .CountAsync();

            var result = await Get()
                .OrderByDescending(r => r.EmailLogId)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
            return (result, resultCount);
        }


        public async Task<int> SaveAsync(EmailLog entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            if (entity.EmailLogId == 0)
            {
                // Add the entity.
                await AddAsync(entity).ConfigureAwait(true);
            }
            else
            {
                // Get existing record. (Prefer exception if not found or just ignore?
                var existingEntity = await GetAsync(entity.EmailLogId).ConfigureAwait(true);
                if (existingEntity == null)
                {
                    throw new MissingRecordException("Update could not be completed because existing item could not be found.");
                }

                // Update existing entity with appropriate values for provided entity.
                MapNewToExisting(entity, ref existingEntity);

                // Update the entity.
                await UpdateAsync(existingEntity).ConfigureAwait(true);
            }

            // Return the id for the saved record.
            return entity.EmailLogId;
        }

        private static void MapNewToExisting(EmailLog mapFrom, ref EmailLog mapTo)
        {
            mapTo.EmailFromAddress = mapFrom.EmailFromAddress;
            mapTo.EmailTo = mapFrom.EmailTo;
            mapTo.EmailCc = mapFrom.EmailCc;
            mapTo.Subject = mapFrom.Subject;
            mapTo.Body = mapFrom.Body;
            mapTo.ReplyTo = mapFrom.ReplyTo;
            mapTo.AttachmentPath = mapFrom.AttachmentPath;
            mapTo.AttachmentName = mapFrom.AttachmentName;

        }
    }
}
