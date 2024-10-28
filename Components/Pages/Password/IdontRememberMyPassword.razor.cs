using Blazored.Toast.Services;
using CodersCupAward.Helper;
using CodersCupAward.Models;
using CodersCupAward.ViewModels;
using Microsoft.AspNetCore.Components;


namespace CodersCupAward.Components.Pages.Password
{
    public partial class IdontRememberMyPassword : ComponentBase
    {
        [Inject] private IToastService ToastService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IApplicationUserHelper ApplicationUserHelper { get; set; }
        [Inject] private IEmailHelper EmailHelper { get; set; }
        [CascadingParameter] public ApplicationSessionViewModel ApplicationSessionViewModel { get; set; }

        private ApplicationUser _applicationUser = new()
        {
            Organization = new Organization()
        };

        private bool _showWait;

        private async Task SaveFormAsync()
        {
            _showWait = true;
            var user = await ApplicationUserHelper.GetByUserEmailAsync(_applicationUser.EmailAddress);
            if (user.ApplicationUserId == 0)
            {
                ToastService.ShowError("Email address not found");
                _showWait = false;
                return;
            }
            await EmailHelper.SendEmailPasswordResetAsync(user, "en");
            NavigationManager.NavigateTo("/IdontRememberMyPasswordEmailSent");
            _showWait = false;
        }
    }
}
