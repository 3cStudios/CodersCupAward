using System.Text.RegularExpressions;
using Blazored.Toast.Services;
using CodersCupAward.Helper;
using CodersCupAward.Models;
using CodersCupAward.ViewModels;
using Microsoft.AspNetCore.Components;

namespace CodersCupAward.Components.Pages.Password
{
    public partial class PasswordReset : ComponentBase
    {
        [Inject] private IToastService ToastService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IApplicationUserHelper ApplicationUserHelper { get; set; }
        [Inject] private IApplicationUserPhotoHelper ApplicationUserPhotoHelper { get; set; }
        [Inject] private ITranslationLayerHelper TranslationLayerHelper { get; set; }
        [Inject] private ISecurityHelper SecurityHelper { get; set; }

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
            await ApplicationUserHelper.SaveAsync(ApplicationSessionViewModel.User);
            NavigationManager.NavigateTo("/LeaderBoard");
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
