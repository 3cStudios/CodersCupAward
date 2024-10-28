using System.Security.Claims;
using CodersCupAward.Models;

namespace CodersCupAward.ViewModels
{
    public class ApplicationSessionViewModel
    {
        public Guid ApplicationSessionId { get; set; }
        public ApplicationUser User { get; set; }

        public TimeZoneInfo TimeZoneInfo;
        public ApplicationUser RegisterUser { get; set; }
        public ApplicationUser EditUser { get; set; }
        public string RequestedUrl { get; set; }

    }
}
