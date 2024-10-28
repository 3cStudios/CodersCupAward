using Blazored.Toast.Services;
using CodersCupAward.Helper;
using CodersCupAward.Models;
using CodersCupAward.ViewModels;
using Microsoft.AspNetCore.Components;

namespace CodersCupAward.Components.Pages.Coaches
{
    public partial class CoachPoints : ComponentBase
    {
        [Inject] private IToastService ToastService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IApplicationUserHelper ApplicationUserHelper { get; set; }
        

        [CascadingParameter] public ApplicationSessionViewModel ApplicationSessionViewModel { get; set; }

        private List<ApplicationUser> applicationUsersList = new();
        

        protected override async Task OnInitializedAsync()
        {
            applicationUsersList = await ApplicationUserHelper.GetByOrganizationIdAsync(ApplicationSessionViewModel.User.OrganizationId);
            applicationUsersList = applicationUsersList
                .OrderBy(x => x.LastName)
                .ToList();
            
        }


        private void EditTeamMember(ApplicationUser applicationUser)
        {
            ApplicationSessionViewModel.EditUser = applicationUser;
            NavigationManager.NavigateTo("/CoachAddPoints");
        }

        
    }
}
