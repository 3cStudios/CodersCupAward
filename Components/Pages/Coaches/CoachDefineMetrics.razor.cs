using CodersCupAward.Components.PageComponents;
using CodersCupAward.Helper;
using CodersCupAward.Models;
using CodersCupAward.ViewModels;
using Microsoft.AspNetCore.Components;

namespace CodersCupAward.Components.Pages.Coaches
{
    public partial class CoachDefineMetrics : ComponentBase
    {
        [Inject] private ICoderPointMetricHelper CoderPointMetricHelper { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [CascadingParameter] public ApplicationSessionViewModel ApplicationSessionViewModel { get; set; }

        private List<CoderPointMetric> _coderPointsMetricList = new();
        private DialogAddMetric _dialogAddMetric;

        protected override async Task OnInitializedAsync()
        {
            _coderPointsMetricList = await CoderPointMetricHelper.GetAllAsync(ApplicationSessionViewModel.User.OrganizationId);
        }

        private void AddMetric()
        {
            _dialogAddMetric.ShowDialog(new CoderPointMetric());
        }

        public void EditMetric(CoderPointMetric coderPointMetric)
        {
            _dialogAddMetric.ShowDialog(coderPointMetric);
        }

        private async Task RefreshListOnSave()
        {
            _coderPointsMetricList = await CoderPointMetricHelper.GetAllAsync(ApplicationSessionViewModel.User.OrganizationId);
        }
    }
}
