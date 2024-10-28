using CodersCupAward.ViewModels;

namespace CodersCupAward.Helper;

public interface IApplicationSessionHelper
{
    Task<ApplicationSessionViewModel> GetApplicationSession(Guid sessionId);
    Task TrackSession(ApplicationSessionViewModel applicationSessionViewModel);
}