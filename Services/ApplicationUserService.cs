using CodersCupAward.Models;
using CodersCupAward.ViewModels;
using Microsoft.EntityFrameworkCore;
using CodersCupAward.Helper;

namespace CodersCupAward.Services
{
    public class ApplicationUserService : NoTrackingRepository<ApplicationUser>, IApplicationUserService
    {
        private readonly coderscupawardContext _dbContext;
        private readonly ISecurityHelper _securityHelper;
       
        public ApplicationUserService(coderscupawardContext dbContext, ISecurityHelper securityHelper)
            : base(dbContext)
        {
            _dbContext = dbContext;
            _securityHelper = securityHelper;
        }
        public async Task DeleteAsync(ApplicationUser applicationUser)
        {
            await UpdateAsync(applicationUser);
        }
        public async Task<ApplicationUser> GetByUserIdAsync(int applicationUserId)
        {
            var result = await Get()
                .Include(c => c.ApplicationUserRole)
                .ThenInclude(c => c.ApplicationRoles)
                .Include(c => c.ApplicationUserPhoto)
                .Include(c => c.Organization)
                .Include(c => c.CoderPointTracking)
                .FirstOrDefaultAsync(c => c.ApplicationUserId == applicationUserId && c.IsDeleted == false);
            return result ?? new ApplicationUser();
        }

        public async Task<ApplicationUser> GetByUserUIdAsync(Guid uId)
        {
            var result = await Get()
                    .Include(c => c.ApplicationUserPhoto)
                    .Include(c => c.Organization)
                    .Include(c => c.ApplicationUserRole)
                    .ThenInclude(c => c.ApplicationRoles)
                    .Include(c => c.CoderPointTracking.Where(r => r.IsDeleted == false))
                    .FirstOrDefaultAsync(c => c.ApplicationUserUniqueId == uId && c.IsDeleted == false);
            return result ?? new ApplicationUser();
        }

        public async Task<ApplicationUser> GetByUserEmailAsync(string emailAddress)
        {
            var result = await Get()
                .Include(c => c.ApplicationUserRole)
                .ThenInclude(c => c.ApplicationRoles)
                .Include(c => c.ApplicationUserPhoto)
                .Include(c => c.Organization)
                .FirstOrDefaultAsync(c => c.EmailAddress == emailAddress
                                          && c.IsDeleted == false);
            return result ?? new ApplicationUser();
        }

        public async Task<List<ApplicationUser>> GetByOrganizationIdAsync(int organizationId)
        {
            var result = await Get()
                .Include(c => c.ApplicationUserPhoto)
                .Include(c => c.Organization)
                .Include(c => c.ApplicationUserRole)
                .ThenInclude(c => c.ApplicationRoles)
                .Include(c => c.CoderPointTracking.Where(r=> r.IsDeleted == false))
                .Where(c => c.OrganizationId == organizationId 
                && c.IsDeleted == false)
                .ToListAsync();
            return result ?? new List<ApplicationUser>();
        }

        
        public async Task<ApplicationUser> Login(LoginModel loginModel)
        {
            var hashedPassword = _securityHelper.HashPassword(loginModel.Password);

            var result = await Get()
                .Include(c => c.ApplicationUserPhoto)
                .Include(c => c.Organization)
                .Include(c => c.ApplicationUserRole)
                .ThenInclude(c => c.ApplicationRoles)
                .FirstOrDefaultAsync(c => c.EmailAddress == loginModel.EmailAddress 
                && c.PasswordHash == hashedPassword && c.IsDeleted == false);
            return result ?? new ApplicationUser();
        }
        public async Task<ApplicationUser> GetByEmailAsync(string email)
        {
            var result = await _dbContext.ApplicationUser
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.EmailAddress == email && c.IsDeleted == false);

            return result ?? new ApplicationUser();
        }
        public async Task<List<ApplicationUser>> GetAllAsync()
        {
            var result = await _dbContext.ApplicationUser
                .AsNoTracking()
                .Include(c => c.ApplicationUserPhoto.Where(r => r.IsDeleted == false))
                .Include(c => c.ApplicationUserActivityLog)
                .Include(c => c.ApplicationUserRole)
                .ThenInclude(c => c.ApplicationRoles)
                .Where(c => c.IsDeleted == false)
                .ToListAsync();

            return result ?? new List<ApplicationUser>();
        }
        public async Task<int> SaveAsync(ApplicationUser applicationUser)
        {
            if (applicationUser == null) throw new ArgumentNullException(nameof(applicationUser));

            if (applicationUser.ApplicationUserId == 0)
            {
                // Add the applicationUser.
                
                await AddAsync(applicationUser).ConfigureAwait(true);
            }
            else
            {
                // Get existing record. (Prefer exception if not found or just ignore?
                var existingEntity = await GetByUserIdAsync(applicationUser.ApplicationUserId).ConfigureAwait(true);
                if (existingEntity == null) { throw new Exception("Update could not be completed because existing item could not be found."); }

                // Update existing applicationUser with appropriate values for provided applicationUser.
                MapNewToExisting(applicationUser, ref existingEntity);

                // Update the applicationUser.
                await UpdateAsync(existingEntity).ConfigureAwait(true);
            }

            // Return the id for the saved record.
            return applicationUser.ApplicationUserId;
        }
        private static void MapNewToExisting(ApplicationUser mapFrom, ref ApplicationUser mapTo)
        {
            mapTo.ApplicationUserId = mapFrom.ApplicationUserId;
            mapTo.OrganizationId = mapFrom.OrganizationId;
            mapTo.FirstName = mapFrom.FirstName;
            mapTo.LastName = mapFrom.LastName;
            mapTo.CellPhoneNumber = mapFrom.CellPhoneNumber;
            mapTo.CanRecieveSMS = mapFrom.CanRecieveSMS;
            mapTo.EmailAddress = mapFrom.EmailAddress;
            mapTo.EmailAddressVerified = mapFrom.EmailAddressVerified;
            mapTo.EmailVerificationSentDate = mapFrom.EmailVerificationSentDate;
            mapTo.InvitePending = mapFrom.InvitePending;
            mapTo.InviteSentDate = mapFrom.InviteSentDate;
            mapTo.Username = mapFrom.Username;
            mapTo.PasswordHash = mapFrom.PasswordHash;
            mapTo.CreatedDate = mapFrom.CreatedDate;
            mapTo.CreatedBy = mapFrom.CreatedBy;
            mapTo.ModifiedDate = mapFrom.ModifiedDate;
            mapTo.ModifiedBy = mapFrom.ModifiedBy;
            mapTo.IsActive = mapFrom.IsActive;
            mapTo.IsDeleted = mapFrom.IsDeleted;
        }
    }
}
