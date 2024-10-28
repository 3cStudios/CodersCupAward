using Blazored.Toast.Services;
using CodersCupAward.Helper;
using CodersCupAward.Models;
using CodersCupAward.ViewModels;
using Microsoft.AspNetCore.Components;

namespace CodersCupAward.Components.Pages.Registration
{
    public partial class RegisterOrganizationCheck : ComponentBase
    {
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IOrganizationHelper OrganizationHelper { get; set; }
        [Inject] private IApplicationUserHelper ApplicationUserHelper { get; set; }
        

        [CascadingParameter] public ApplicationSessionViewModel ApplicationSessionViewModel { get; set; }

        private bool _showWait;
        private List<Organization> _similarOrganizations = new();


        protected override async Task OnInitializedAsync()
        {
            _showWait = true;
            _similarOrganizations = await OrganizationHelper
                .OrganizationByNameAsync(ApplicationSessionViewModel.RegisterUser.Organization.OrganizationName);
            if (_similarOrganizations.Count == 0)
            {
                NavigationManager.NavigateTo("/RegisterOrganization");
            }
            else
            {

               _showWait = false;
            }

        }

        private async Task JoinOrganizationAsync(int organizationId)
        {
            ApplicationSessionViewModel.RegisterUser.OrganizationId = organizationId;
            ApplicationSessionViewModel.RegisterUser.IsPendingApproval = true;
            var joinOrganization = _similarOrganizations.Find(r=> r.OrganizationId== organizationId);
            ApplicationSessionViewModel.RegisterUser.Organization = joinOrganization;
            await ApplicationUserHelper.SaveAsync(ApplicationSessionViewModel.RegisterUser);
            ApplicationSessionViewModel.User = ApplicationSessionViewModel.RegisterUser;
            NavigationManager.NavigateTo("/RegisterEmailSentForJoinVerification");
        }

        private void ContinueToRegisterOrganization()
        {
            NavigationManager.NavigateTo("/RegisterOrganization");
        }
    }
}