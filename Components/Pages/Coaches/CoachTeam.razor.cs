using System.Runtime.InteropServices;
using Blazored.Toast.Services;
using CodersCupAward.Components.PageComponents;
using CodersCupAward.Extensions;
using CodersCupAward.Helper;
using CodersCupAward.Models;
using CodersCupAward.ViewModels;
using Microsoft.AspNetCore.Components;

namespace CodersCupAward.Components.Pages.Coaches
{
    public partial class CoachTeam : ComponentBase
    {
        [Inject] private IToastService ToastService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IApplicationUserHelper ApplicationUserHelper { get; set; }

        [CascadingParameter] public ApplicationSessionViewModel ApplicationSessionViewModel { get; set; }

        private List<ApplicationUser> applicationUsersList = new();
        private DialogConfirm _dialogConfirm;
        private ApplicationUser _deleteApplicationUser;
        protected override async Task OnInitializedAsync()
        {
            applicationUsersList = await ApplicationUserHelper.GetByOrganizationIdAsync(ApplicationSessionViewModel.User.OrganizationId);
            applicationUsersList = applicationUsersList
                .OrderBy(x => x.LastName)
                .ToList();
        }

        private void AddTeamMember()
        {
            ApplicationSessionViewModel.EditUser = new ApplicationUser();
            NavigationManager.NavigateTo("CoachEditTeamMember");
        }

        private void EditTeamMember(ApplicationUser applicationUser)
        {
            ApplicationSessionViewModel.EditUser = applicationUser;
            NavigationManager.NavigateTo("CoachEditTeamMember");
        }
        private void DeleteTeamMember(ApplicationUser applicationUser)
        {
            _deleteApplicationUser = applicationUser;
            _dialogConfirm.Confirm("Confirm", "Are you sure you want to delete this record?");
        }
        private async Task OnDeleteConfirmation(bool confirmed)
        {
            if (confirmed)
            {
                _deleteApplicationUser.IsDeleted = true;
                _deleteApplicationUser.ModifiedDate = DateTime.Now;
                _deleteApplicationUser.ModifiedBy = ApplicationSessionViewModel.User.FullName();
                await ApplicationUserHelper.DeleteAsync(_deleteApplicationUser);
                applicationUsersList = await ApplicationUserHelper.GetByOrganizationIdAsync(ApplicationSessionViewModel.User.OrganizationId);
                applicationUsersList = applicationUsersList
                    .OrderBy(x => x.LastName)
                    .ToList();
                ToastService.ShowSuccess("Record deleted successfully.");
                StateHasChanged();
            }
        }
    }
}
