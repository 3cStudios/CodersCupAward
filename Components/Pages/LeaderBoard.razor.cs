using CodersCupAward.Helper;
using CodersCupAward.Models;
using CodersCupAward.ViewModels;
using Microsoft.AspNetCore.Components;


namespace CodersCupAward.Components.Pages
{
    public partial class LeaderBoard : ComponentBase
    {
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IApplicationUserHelper ApplicationUserHelper { get; set; }
        [Inject] private IIterationHelper IterationHelper { get; set; }

        [CascadingParameter] public ApplicationSessionViewModel ApplicationSessionViewModel { get; set; }

        private List<ApplicationUser> applicationUsersList = new();
        private List<Iteration> iterationList = new();
        private List<Iteration> _allIterationList = new();
        private Iteration _selectedIteration;
        private bool _showWait;
        protected override async Task OnInitializedAsync()
        {
            _showWait = true;
            if (ApplicationSessionViewModel.User != null &&  ApplicationSessionViewModel.User.OrganizationId > 0)
            {
                _allIterationList = await IterationHelper.GetAllAsync(ApplicationSessionViewModel.User.OrganizationId);
                iterationList = _allIterationList
                    .Where(r => r.StartDate.Date <= DateTime.Now.Date)
                    .OrderByDescending(r => r.StartDate).ToList();
                _selectedIteration = iterationList.FirstOrDefault() ?? new Iteration();
                await UpdateLeaderBoard();
                _showWait = false;
            }
        }

        private async Task UpdateLeaderBoard()
        {
            _showWait = true;
            applicationUsersList = await ApplicationUserHelper.GetByOrganizationIdAsync(ApplicationSessionViewModel.User.OrganizationId);
            if (_selectedIteration.IterationId > 0)
            {
                applicationUsersList = applicationUsersList
                    .Where(x => x.CoderPointTracking.Any(r => r.IterationId == _selectedIteration.IterationId))
                    .ToList();
            }
            applicationUsersList = applicationUsersList
                .OrderByDescending(x => x.CoderPointTracking.Sum(r => r.Points))
                .ThenBy(x => x.LastName)
                .ToList();
            _showWait = false;
        }

       
    }
}
