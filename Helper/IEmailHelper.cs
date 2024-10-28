using CodersCupAward.Models;
using CodersCupAward.ViewModels;

namespace CodersCupAward.Helper;

public interface IEmailHelper
{
    Task<int> SendEmailAsync(MessageEmailViewModel messageEmailViewModel);
    Task<int> SendInvitationEmailAsync(ApplicationUser applicationUser, string languageCode);
    Task<int> SendEmailVerificationAsync(ApplicationUser applicationUser, string languageCode);
    Task<int> SendJoinApprovalEmailAsync(ApplicationUser applicationUser,
        ApplicationUser coachUser, string languageCode);
    Task<int> SendEmailPasswordResetAsync(ApplicationUser applicationUser, string languageCode);
    Task<int> SendLeaderboardUpdateAsync(ApplicationUser applicationUser, string languageCode);
    Task<int> SendPointsAwardedAsync(ApplicationUser applicationUser, List<CoderPointTracking> coderPointTrackingLis, List<CoderPointMetric> coderPointMetrics, string languageCode);
}