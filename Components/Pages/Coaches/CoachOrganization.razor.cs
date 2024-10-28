using Blazored.Toast.Services;
using CodersCupAward.Extensions;
using CodersCupAward.Helper;
using CodersCupAward.Models;
using CodersCupAward.ViewModels;
using Microsoft.AspNetCore.Components;

namespace CodersCupAward.Components.Pages.Coaches
{
    public partial class CoachOrganization : ComponentBase
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
            _organization = ApplicationSessionViewModel.User.Organization;
        }

        private async Task SaveFormAsync()
        {

            _organization.ModifiedBy = ApplicationSessionViewModel.User.FullName();
            _organization.ModifiedDate = DateTime.UtcNow;

            var organizationId = await OrganizationHelper.SaveAsync(_organization);

            if (organizationId == 0)
            {
                _showWait = false;
                StateHasChanged();
                ToastService.ShowError(Constants.ErrorSavingPerson);
                return;
            }
            _organization.OrganizationId = organizationId;

            ToastService.ShowSuccess(Constants.RecordSaved);
            
        }


    }
}
