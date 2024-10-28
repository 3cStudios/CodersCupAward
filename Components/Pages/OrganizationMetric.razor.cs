using Blazored.Toast.Services;

using CodersCupAward.Helper;
using CodersCupAward.Models;
using CodersCupAward.ViewModels;
using Microsoft.AspNetCore.Components;


namespace CodersCupAward.Components.Pages
{
    public partial class OrganizationMetric : ComponentBase
    {
        [Inject] private ICoderPointMetricHelper CoderPointMetricHelper { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [CascadingParameter] public ApplicationSessionViewModel ApplicationSessionViewModel { get; set; }

               
        private List<CoderPointMetric> _coderPointsMetricList = new();
        

        protected override async Task OnInitializedAsync()
        {
            _coderPointsMetricList = await CoderPointMetricHelper.GetAllAsync(ApplicationSessionViewModel.User.OrganizationId);
        }
       
    }
}
