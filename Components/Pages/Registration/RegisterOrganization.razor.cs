using Blazored.Toast.Services;
using CodersCupAward.Helper;
using CodersCupAward.Models;
using CodersCupAward.ViewModels;
using Microsoft.AspNetCore.Components;
using CodersCupAward.Extensions;



namespace CodersCupAward.Components.Pages.Registration
{
    public partial class RegisterOrganization : ComponentBase
    {
        [Inject] private IToastService ToastService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IOrganizationHelper OrganizationHelper { get; set; }
        [Inject] private ITranslationLayerHelper TranslationLayerHelper { get; set; }
        [Inject] private IApplicationUserHelper ApplicationUserHelper { get; set; }
        [Inject] private IEmailHelper EmailHelper { get; set; }

        [CascadingParameter] public ApplicationSessionViewModel ApplicationSessionViewModel { get; set; }

        private bool _showWait;
        private Organization _organization = new();
        protected override void OnInitialized()
        {
            _organization.OrganizationName = ApplicationSessionViewModel.RegisterUser.Organization.OrganizationName;
        }

        private async Task<bool> RegisterOrganizationAsync()
        {
            if (_organization.OrganizationId == 0)
            {
                _organization.CreatedBy = Constants.Registration;
                _organization.CreatedDate = DateTime.UtcNow;

            }
            else
            {
                _organization.ModifiedBy = ApplicationSessionViewModel.User.FullName();
                _organization.ModifiedDate = DateTime.UtcNow;
            }

            var organizationId = await OrganizationHelper.SaveAsync(_organization);

            if (organizationId == 0)
            {
                _showWait = false;
                StateHasChanged();
                ToastService.ShowError(Constants.ErrorSavingPerson);
                return false;
            }
            _organization.OrganizationId = organizationId;

            return true;
        }

        private async Task<bool> RegisterApplicationUserAsync()
        {

            ApplicationSessionViewModel.RegisterUser.OrganizationId = _organization.OrganizationId;

            var applicationUserId = await ApplicationUserHelper.SaveAsync(ApplicationSessionViewModel.RegisterUser);

            if (applicationUserId == 0)
            {
                _showWait = false;
                StateHasChanged();
                ToastService.ShowError(Constants.ErrorSavingPerson);
                return false;
            }
            ApplicationSessionViewModel.User = await ApplicationUserHelper.GetByUserIdAsync(applicationUserId);


            return true;
        }

        private async Task SaveFormAsync()
        {
            _showWait = true;
            if (!await RegisterOrganizationAsync())
            {
                return;
            }

            if (ApplicationSessionViewModel.RegisterUser != null)
            {
                if (!await RegisterApplicationUserAsync())
                {
                    return;
                }
            }

            NavigationManager.NavigateTo("/RegisterEmailSentForVerification");
            _showWait = false;


        }

    }
}
