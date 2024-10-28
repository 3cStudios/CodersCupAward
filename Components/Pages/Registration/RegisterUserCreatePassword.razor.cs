using Blazored.Toast.Services;
using CodersCupAward.Helper;
using CodersCupAward.Models;
using CodersCupAward.ViewModels;
using Microsoft.AspNetCore.Components;
using System.Text.RegularExpressions;
using CodersCupAward.Extensions;
using Microsoft.AspNetCore.Components.Authorization;



namespace CodersCupAward.Components.Pages.Registration
{
    public partial class RegisterUserCreatePassword : ComponentBase
    {
        [Inject] private IToastService ToastService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IApplicationUserHelper ApplicationUserHelper { get; set; }
        [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        [Inject] private ITranslationLayerHelper TranslationLayerHelper { get; set; }
        [Inject] private ISecurityHelper SecurityHelper { get; set; }
        [Inject] private IApplicationUserRoleHelper ApplicationUserRoleHelper { get; set; }

        [CascadingParameter] public ApplicationSessionViewModel ApplicationSessionViewModel { get; set; }

        private bool _showWait;

        private string _password = string.Empty;
        private string _confirmPassword = string.Empty;
        private ApplicationUser _applicationUser = new();

        private bool _showPassword;
        private bool _showConfirm;
        private string InputType => _showPassword ? "text" : "password";
        private string InputTypeConfirm => _showConfirm ? "text" : "password";

        private void TogglePasswordVisibility()
        {
            _showPassword = !_showPassword;
        }
        private void ToggleConfirmVisibility()
        {
            _showConfirm = !_showConfirm;
        }
        private async Task SaveFormAsync()
        {
            _showWait = true;
            if (string.IsNullOrEmpty(_password) || string.IsNullOrEmpty(_confirmPassword))
            {
                ToastService.ShowError("Please enter a password and confirm it");
                _showWait = false;
                return;
            }
            if (_password != _confirmPassword)
            {
                ToastService.ShowError("Passwords do not match");
                _showWait = false;
                return;
            }
            if (!IsPasswordStrong(_password))
            {
                ToastService.ShowError("Password is not strong enough. Please create a password that is at least 12 characters long and includes uppercase and lowercase letters, numbers, and special symbols.");
                _showWait = false;
                return;
            }

            ApplicationSessionViewModel.User.PasswordHash = SecurityHelper.HashPassword(_password);
            ApplicationSessionViewModel.User.IsActive = true;
            ApplicationSessionViewModel.User.EmailAddressVerified = true;
            
            await ApplicationUserHelper.SaveAsync(ApplicationSessionViewModel.User);
            
            await SecurityHelper.SetRememberMeTokenAsync(ApplicationSessionViewModel.User);

            if (ApplicationSessionViewModel.User.ApplicationUserRole.Count == 0)
            {
                await ApplicationUserRoleHelper.SaveAsync(new ApplicationUserRole { 
                    ApplicationUserId = ApplicationSessionViewModel.User.ApplicationUserId,
                    ApplicationRolesId = Constants.CoachRoleId,
                    CreatedBy = ApplicationSessionViewModel.User.FullName(),
                    CreatedDate = DateTime.UtcNow,
                });
                
            }
            NavigationManager.NavigateTo("/", true);
            _showWait = false;
        }


        public static bool IsPasswordStrong(string password)
        {
            if (password.Length < 12)
                return false;

            var hasUpperCase = Regex.IsMatch(password, @"[A-Z]");
            var hasLowerCase = Regex.IsMatch(password, @"[a-z]");
            var hasDigit = Regex.IsMatch(password, @"[0-9]");
            var hasSpecialChar = Regex.IsMatch(password, @"[\W_]");

            return hasUpperCase && hasLowerCase && hasDigit && hasSpecialChar;
        }
    }
}
