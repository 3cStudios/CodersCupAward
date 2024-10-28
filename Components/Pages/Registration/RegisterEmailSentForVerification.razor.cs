using CodersCupAward.Helper;
using CodersCupAward.ViewModels;
using Microsoft.AspNetCore.Components;



namespace CodersCupAward.Components.Pages.Registration
{
    public partial class RegisterEmailSentForVerification : ComponentBase
    {
        [Inject] private IEmailHelper EmailHelper { get; set; }
        [Inject] private IApplicationUserHelper ApplicationUserHelper { get; set; }
        [CascadingParameter] public ApplicationSessionViewModel ApplicationSessionViewModel { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await EmailHelper.SendEmailVerificationAsync(ApplicationSessionViewModel.User, "en");
                ApplicationSessionViewModel.User.EmailVerificationSentDate = DateTime.UtcNow;
                await ApplicationUserHelper.SaveAsync(ApplicationSessionViewModel.User);
            }
        }


    }
}
