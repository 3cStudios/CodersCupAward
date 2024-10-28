using CodersCupAward.Models;
using CodersCupAward.ViewModels;
using Microsoft.EntityFrameworkCore;
using CodersCupAward.Helper;

namespace CodersCupAward.Services
{
    public class OrganizationService : NoTrackingRepository<Organization>, IOrganizationService
    {
        private readonly coderscupawardContext _dbContext;

       
        public OrganizationService(coderscupawardContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }
                

        public async Task DeleteAsync(Organization organization)
        {
            await UpdateAsync(organization);
        }

        public async Task<Organization> OrganizationByIdAsync(int organizationId)
        {
            var result = await Get()
                .FirstOrDefaultAsync(c => c.OrganizationId == organizationId);
            return result ?? new Organization();
        }

        public async Task<List<Organization>> OrganizationByNameAsync(string organizationName)
        {
            var result = await Get()
                .Where(c => c.OrganizationName == organizationName)
                .ToListAsync();
            return result ?? new List<Organization>();
        }

        public async Task<List<Organization>> GetAllAsync()
        {
            var result = await _dbContext.Organization
                .AsNoTracking()
                .Where(c => c.IsDeleted == false)
                .ToListAsync();
            return result ?? new List<Organization>();
        }



        public async Task<int> SaveAsync(Organization organization)
        {
            if (organization == null) throw new ArgumentNullException(nameof(organization));

            if (organization.OrganizationId == 0)
            {
                // Add the Organization.
                
                await AddAsync(organization).ConfigureAwait(true);
            }
            else
            {
                // Get existing record. (Prefer exception if not found or just ignore?
                var existingEntity = await OrganizationByIdAsync(organization.OrganizationId);
                if (existingEntity == null) { throw new Exception("Update could not be completed because existing item could not be found."); }

                // Update existing Organization with appropriate values for provided Organization.
                MapNewToExisting(organization, ref existingEntity);

                // Update the Organization.
                await UpdateAsync(existingEntity).ConfigureAwait(true);
            }

            // Return the id for the saved record.
            return organization.OrganizationId;
        }

       

        private static void MapNewToExisting(Organization mapFrom, ref Organization mapTo)
        {
            mapTo.OrganizationId = mapFrom.OrganizationId;
            mapTo.OrganizationName = mapFrom.OrganizationName;
            mapTo.DepartmentTeamName = mapFrom.DepartmentTeamName;
            mapTo.Address = mapFrom.Address;
            mapTo.CreatedDate = mapFrom.CreatedDate;
            mapTo.CreatedBy = mapFrom.CreatedBy;
            mapTo.ModifiedDate = mapFrom.ModifiedDate;
            mapTo.ModifiedBy = mapFrom.ModifiedBy;
            mapTo.IsActive = mapFrom.IsActive;
            mapTo.IsDeleted = mapFrom.IsDeleted;
        }
    }
}
