using CodersCupAward.Extensions;
using CodersCupAward.Helper;
using CodersCupAward.Models;
using CodersCupAward.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace CodersCupAward.Components.Pages.Coaches
{
    public partial class CoachHome : ComponentBase
    {
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IJSRuntime JSRuntime { get; set; }
        [Inject] private IApplicationUserHelper ApplicationUserHelper { get; set; }
        [Inject] private ICoderPointMetricHelper CoderPointMetricHelper { get; set; }
        [Inject] private ICoderPointTrackingHelper CoderPointTrackingHelper { get; set; }
        [Inject] private IOrganizationHelper OrganizationHelper { get; set; }
        [Inject] private IIterationHelper IterationHelper { get; set; }
        [CascadingParameter] public ApplicationSessionViewModel ApplicationSessionViewModel { get; set; }

        private List<ApplicationUser> _teamMembers = new();
        private List<CoderPointMetric> _coderPointMetrics = new();
        private List<CoderPointTracking> _coderPointTrackings = new();
        private List<Iteration> _iterationsList = new();

        private void NavigateToCoachEditTeamMember()
        {
            NavigationManager.NavigateTo("CoachEditTeamMember");
        }

        protected override async Task OnInitializedAsync()
        {
            _teamMembers = await ApplicationUserHelper.GetByOrganizationIdAsync(ApplicationSessionViewModel.User.OrganizationId);
            _coderPointMetrics = await CoderPointMetricHelper.GetAllAsync(ApplicationSessionViewModel.User.OrganizationId);
            _coderPointTrackings = await CoderPointTrackingHelper.GetAllAsync(ApplicationSessionViewModel.User.OrganizationId);
            _iterationsList = await IterationHelper.GetAllAsync(ApplicationSessionViewModel.User.OrganizationId);
            if (HasTeamMembers() && HasTeamMetrics() && HasPoints() && HasIterations())
            {
                var organization = await OrganizationHelper.OrganizationByIdAsync(ApplicationSessionViewModel.User.OrganizationId);
                organization.IsSetupComplete = true;
                organization.ModifiedBy = ApplicationSessionViewModel.User.FullName();
                organization.ModifiedDate = DateTime.UtcNow;
                await OrganizationHelper.SaveAsync(organization);
            }
        }
        
        private bool HasTeamMembers()
        {
            return _teamMembers.Count > 1;
        }
        private bool HasTeamMetrics()
        {
            return _coderPointMetrics.Count > 0;
        }
        private bool HasPoints()
        {
            return _coderPointTrackings.Count > 0;
        }

        private bool HasIterations()
        {
            return _iterationsList.Count > 0;
        }
        
    }
}
