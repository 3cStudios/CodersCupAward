using CodersCupAward.Components.PageComponents;
using CodersCupAward.Helper;
using CodersCupAward.Models;
using CodersCupAward.ViewModels;
using Microsoft.AspNetCore.Components;

namespace CodersCupAward.Components.Pages.Coaches
{
    public partial class CoachIterations : ComponentBase
    {
        [Inject] private IIterationHelper IterationHelper { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [CascadingParameter] public ApplicationSessionViewModel ApplicationSessionViewModel { get; set; }

        private List<Iteration> _iterationsList = new();
        private DialogAddIteration _dialogAddIteration;

        protected override async Task OnInitializedAsync()
        {
            _iterationsList = await IterationHelper.GetAllAsync(ApplicationSessionViewModel.User.OrganizationId);
        }

        private void AddIteration()
        {
            _dialogAddIteration.ShowDialog(new Iteration());
        }

        public void EditIteration(Iteration coderPointMetric)
        {
            _dialogAddIteration.ShowDialog(coderPointMetric);
        }

        private async Task RefreshListOnSave()
        {
            _iterationsList = await IterationHelper.GetAllAsync(ApplicationSessionViewModel.User.OrganizationId);
        }
    }
}
