using CodersCupAward.Helper;
using CodersCupAward.ViewModels;
using Microsoft.AspNetCore.Components;



namespace CodersCupAward.Components.Pages.Registration
{
    public partial class RegisterEmailSentForJoinVerification : ComponentBase
    {
        [Inject] private IEmailHelper EmailHelper { get; set; }
        [Inject] private IApplicationUserHelper ApplicationUserHelper { get; set; }

        [CascadingParameter] public ApplicationSessionViewModel ApplicationSessionViewModel { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var applicationUsers = await ApplicationUserHelper.GetByOrganizationIdAsync(ApplicationSessionViewModel.RegisterUser.OrganizationId);
                var coachUsers = applicationUsers.Where(r => r.ApplicationUserRole.Any(r => r.ApplicationRolesId == Constants.CoachRoleId)).ToList();
                  
                var firsCoachUserId = coachUsers.Min(r => r.ApplicationUserId);
                var coachUser = applicationUsers.Find(r => r.ApplicationUserId == firsCoachUserId);
                if (coachUser != null)
                    await EmailHelper.SendJoinApprovalEmailAsync
                        (ApplicationSessionViewModel.User, coachUser, "en");
            }
        }


    }
}
