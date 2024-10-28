using Blazored.Toast.Services;
using CodersCupAward.Helper;
using CodersCupAward.Models;
using CodersCupAward.Services;
using CodersCupAward.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Globalization;
using CodersCupAward.Extensions;
using Microsoft.JSInterop;


namespace CodersCupAward.Components.Pages.Registration
{
    public partial class RegisterUser : ComponentBase
    {
        [Inject] private IToastService ToastService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IApplicationUserHelper ApplicationUserHelper { get; set; }
        [Inject] private IApplicationUserPhotoHelper ApplicationUserPhotoHelper { get; set; }
        [Inject] private ITranslationLayerHelper TranslationLayerHelper { get; set; }
        [Inject] private IJSRuntime JSRuntime { get; set; }

        [CascadingParameter] public ApplicationSessionViewModel ApplicationSessionViewModel { get; set; }

        private string _personImage = Constants.DefaultPersonImage;
        private InputFile _inputFile;
        private const long MaxFileSize = 1073741824;
        private string _extensionName = string.Empty;
        private ApplicationUser _applicationUser = new()
        {
            Organization = new Organization()
        };

        private List<ApplicationUserPhoto> _applicationUserPhotos = new();
        private bool _showWait;
        private void AddPhotos()
        {
            if (_applicationUserPhotos.Count == 0) return;
            
            foreach (var applicationUserPhoto in _applicationUserPhotos)
            {
                if (applicationUserPhoto.ApplicationUserPhotoId > 0) continue;
                applicationUserPhoto.ApplicationUserId = _applicationUser.ApplicationUserId;
                applicationUserPhoto.CreatedBy = Constants.Registration;
                applicationUserPhoto.CreatedDate = DateTime.UtcNow;
                _applicationUser.ApplicationUserPhoto.Add(applicationUserPhoto);
            }
            
        }
        private async Task<bool> RegisterApplicationUserAsync()
        {
            if (_applicationUser.ApplicationUserId == 0)
            {
                _applicationUser.CreatedBy = Constants.Registration;
                _applicationUser.CreatedDate = DateTime.UtcNow;

            }
            else
            {
                _applicationUser.ModifiedBy = ApplicationSessionViewModel.User.FullName();
                _applicationUser.ModifiedDate = DateTime.UtcNow;
            }

            if (ApplicationSessionViewModel.User == null || ApplicationSessionViewModel.User.OrganizationId == 0)
            {
                ApplicationSessionViewModel.RegisterUser = _applicationUser;
                return true;
            }
            var applicationUserId = await ApplicationUserHelper.SaveAsync(_applicationUser);

            if (applicationUserId == 0)
            {
                _showWait = false;
                StateHasChanged();
                ToastService.ShowError(Constants.ErrorSavingPerson);
                return false;
            }
            _applicationUser.ApplicationUserId = applicationUserId;
            ApplicationSessionViewModel.User = _applicationUser;

            return true;
        }
        private async Task OnImageChangeAsync(InputFileChangeEventArgs e)
        {

            foreach (var file in e.GetMultipleFiles())
            {
                try
                {
                    _extensionName = Path.GetExtension(file.Name);
                    var imageFileTypes = new List<string>() {
                        ".png",".jpg",".jpeg"
                    };

                    if (imageFileTypes.Contains(_extensionName.ToLowerInvariant()))
                    {
                        var ms = new MemoryStream();
                        await file.OpenReadStream(MaxFileSize).CopyToAsync(ms);
                        var buffer = ms.ToArray();
                        _applicationUserPhotos.Add(new ApplicationUserPhoto
                        {
                            Photo = $"data:{file.ContentType};base64,{Convert.ToBase64String(buffer)}"
                        });

                    }
                    else
                    {
                        ToastService.ShowWarning(Constants.ErrorInvalidImageType);
                    }

                    if (_applicationUserPhotos.Count != 0)
                    {
                        _personImage = _applicationUserPhotos[_applicationUserPhotos.Count - 1].Photo;
                    }



                    StateHasChanged();
                }
                catch (Exception ex)
                {
                    _showWait = false;
                    StateHasChanged();
                    ToastService.ShowError(ex.Message);
                }


            }

        }
        private async Task SaveFormAsync()
        {
            _showWait = true;
            var userByEmail = await ApplicationUserHelper.GetByEmailAsync(_applicationUser.EmailAddress);

            var userExists = userByEmail.ApplicationUserId > 0;

            if (userExists)
            {
                NavigationManager.NavigateTo("/RegisterUserCheckFailed");
                return;
            }
            AddPhotos();
            await RegisterApplicationUserAsync();
            if (ApplicationSessionViewModel.User == null || ApplicationSessionViewModel.User.OrganizationId == 0)
            {
                NavigationManager.NavigateTo("/RegisterOrganizationCheck");
                return;
            }
            NavigationManager.NavigateTo("/RegisterEmailSentForVerification");
            _showWait = false;
        }

    }
}
