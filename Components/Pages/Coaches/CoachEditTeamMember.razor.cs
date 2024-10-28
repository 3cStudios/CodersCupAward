using Blazored.Toast.Services;
using CodersCupAward.Components.PageComponents;
using CodersCupAward.Extensions;
using CodersCupAward.Helper;
using CodersCupAward.Models;
using CodersCupAward.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace CodersCupAward.Components.Pages.Coaches
{
    public partial class CoachEditTeamMember : ComponentBase
    {
        [Inject] private IToastService ToastService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IApplicationUserHelper ApplicationUserHelper { get; set; }
        [Inject] private IApplicationUserPhotoHelper ApplicationUserPhotoHelper { get; set; }
        [Inject] private ITranslationLayerHelper TranslationLayerHelper { get; set; }
        [Inject] private IJSRuntime JSRuntime { get; set; }
        [Inject] private IApplicationRoleHelper ApplicationRoleHelper { get; set; }
        [Inject] private IEmailHelper EmailHelper { get; set; }

        [CascadingParameter] public ApplicationSessionViewModel ApplicationSessionViewModel { get; set; }

        
        [Parameter]
        public Guid UID { get; set; }
        

        private string _personImage = Constants.DefaultPersonImage;
        private InputFile _inputFile;
        private const long MaxFileSize = 1073741824;
        private string _extensionName = string.Empty;
        private ApplicationUser _applicationUser = new()
        {
            Organization = new Organization()
        };

        private List<ApplicationUserPhoto> _applicationUserPhotos = new();
        private List<ApplicationRoles> _applicationRoles = new();
        private bool _showWait;
        private InternationalPhoneNumber _cellPhone;
        protected override async Task OnInitializedAsync()
        {

            if (ApplicationSessionViewModel.EditUser != null && ApplicationSessionViewModel.EditUser.ApplicationUserId > 0)
            {
                _applicationUser = ApplicationSessionViewModel.EditUser;
                if (_applicationUser.ApplicationUserPhoto.Count != 0)
                {
                    _personImage = _applicationUser.ApplicationUserPhoto.First().Photo;
                }


            }
            _applicationRoles = await ApplicationRoleHelper.GetAllAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (UID != Guid.Empty)
                {
                    var user = await ApplicationUserHelper.GetByUserUIdAsync(UID);
                    if (user is { ApplicationUserId: > 0 })
                    {
                        _applicationUser = user;
                        if (user.ApplicationUserPhoto.Count != 0)
                        {
                            _personImage = user.ApplicationUserPhoto.First().Photo;
                        }
                        StateHasChanged();
                    }
                }
            }
        }
        private void AddPhotos()
        {
            if (_applicationUserPhotos.Count == 0) return;
            var userPhotos = new List<ApplicationUserPhoto>();
            foreach (var applicationUserPhoto in _applicationUserPhotos)
            {
                if (applicationUserPhoto.ApplicationUserPhotoId > 0) continue;
                applicationUserPhoto.ApplicationUserId = _applicationUser.ApplicationUserId;
                applicationUserPhoto.CreatedBy = Constants.Registration;
                applicationUserPhoto.CreatedDate = DateTime.UtcNow;
                userPhotos.Add(applicationUserPhoto);
            }
            _applicationUser.ApplicationUserPhoto = userPhotos;
        }
        private async Task RegisterApplicationUserAsync()
        {
            if (_applicationUser.ApplicationUserId == 0)
            {
                _applicationUser.CreatedBy = Constants.Registration;
                _applicationUser.CreatedDate = DateTime.UtcNow;
                _applicationUser.OrganizationId = ApplicationSessionViewModel.User.OrganizationId;

            }
            else
            {
                _applicationUser.ModifiedBy = ApplicationSessionViewModel.User.FullName();
                _applicationUser.ModifiedDate = DateTime.UtcNow;
            }

            var applicationUserId = await ApplicationUserHelper.SaveAsync(_applicationUser);

            if (applicationUserId == 0)
            {
                _showWait = false;
                StateHasChanged();
                ToastService.ShowError(Constants.ErrorSavingPerson);

            }
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


        private bool FormIsValid()
        {
            if (string.IsNullOrEmpty(_applicationUser.FirstName))
            {
                ToastService.ShowWarning(Constants.ErrorFirstNameRequired);
                return false;
            }
            if (string.IsNullOrEmpty(_applicationUser.LastName))
            {
                ToastService.ShowWarning(Constants.ErrorLastNameRequired);
                return false;
            }
            if (string.IsNullOrEmpty(_applicationUser.EmailAddress))
            {
                ToastService.ShowWarning(Constants.ErrorEmailAddressRequired);
                return false;
            }

            if (_applicationUser.ApplicationUserRole.Count == 0)
            {
                ToastService.ShowWarning(Constants.ErrorRoleRequired);
                return false;
            }

            if (!ValidationHelper.IsValidEmail(_applicationUser.EmailAddress))
            {
                ToastService.ShowWarning(Constants.ErrorEmailAddressInvalid);
                return false;
            }
            if (!_cellPhone.IsValid())
            {
                ToastService.ShowWarning(Constants.ErrorCellPhoneNumberInvalid);
                return false;
            }
            return true;
        }
        private async Task SaveFormAsync()
        {
            if (!FormIsValid())
            {
                return;
            }

            _showWait = true;
            var isNewUser = _applicationUser.ApplicationUserId == 0;
            AddPhotos();
            await RegisterApplicationUserAsync();

            ToastService.ShowSuccess(isNewUser
                ? Constants.ApplicationLabelTeamMemberCreated
                : Constants.ApplicationLabelTeamMemberUpdated);

            _showWait = false;
        }
        private void ChangeUserRole(ChangeEventArgs e)
        {
            if (!_applicationUser.ApplicationUserRole.Any())
            {
                _applicationUser.ApplicationUserRole = new List<ApplicationUserRole>();
                _applicationUser.ApplicationUserRole.Add(new ApplicationUserRole
                {
                    ApplicationUserId = _applicationUser.ApplicationUserId,
                    ApplicationRolesId = Convert.ToInt32(e.Value)
                });
            }
            else
            {
                _applicationUser.ApplicationUserRole.First().ApplicationRolesId = Convert.ToInt32(e.Value);
            }

        }

        private bool IsUsersCurrentRole(ApplicationRoles applicationRole)
        {
            return _applicationUser.ApplicationUserRole.Any(x => x.ApplicationRolesId == applicationRole.ApplicationRolesId);
        }

        private void AddTeamMember()
        {
            ApplicationSessionViewModel.EditUser = new ApplicationUser();
            _applicationUser = ApplicationSessionViewModel.EditUser;
            _personImage = Constants.DefaultPersonImage;

            StateHasChanged();
        }

        private async Task SendInvitation()
        {
            await EmailHelper.SendEmailVerificationAsync(_applicationUser, "en");
            _applicationUser.EmailVerificationSentDate = DateTime.UtcNow;
            await ApplicationUserHelper.SaveAsync(_applicationUser);
            ToastService.ShowSuccess(Constants.ApplicationLabelTeamMemberInvite);
        }

        private async Task ApproveJoin()
        {
            _applicationUser.IsPendingApproval = false;
            await EmailHelper.SendEmailVerificationAsync(_applicationUser, "en");
            _applicationUser.EmailVerificationSentDate = DateTime.UtcNow;
            await ApplicationUserHelper.SaveAsync(_applicationUser);
            ToastService.ShowSuccess(Constants.ApplicationLabelTeamMemberApproved);
        }

    }
}

