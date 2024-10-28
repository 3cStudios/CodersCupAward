using Blazored.Toast.Services;
using CodersCupAward.Extensions;
using CodersCupAward.Helper;
using CodersCupAward.Models;
using CodersCupAward.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace CodersCupAward.Components.Pages
{
    public partial class MyProfile : ComponentBase
    {
        [Inject] private IToastService ToastService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IApplicationUserHelper ApplicationUserHelper { get; set; }
        [Inject] private IApplicationUserPhotoHelper ApplicationUserPhotoHelper { get; set; }
        [Inject] private ITranslationLayerHelper TranslationLayerHelper { get; set; }
        [Inject] private IJSRuntime JSRuntime { get; set; }
        [Inject] private IApplicationRoleHelper ApplicationRoleHelper { get; set; }
        [Inject] private ICoderPointMetricHelper CoderPointMetricHelper { get; set; }
        [Inject] private IIterationHelper IterationHelper { get; set; }
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
        private List<CoderPointMetric> _coderPointMetrics = new();

        protected override async Task OnInitializedAsync()
        {
            if (ApplicationSessionViewModel.User != null && ApplicationSessionViewModel.User.ApplicationUserId > 0)
            {
                _applicationUser = ApplicationSessionViewModel.User;
                if (_applicationUser.ApplicationUserPhoto.Count != 0)
                {
                    _personImage = _applicationUser.ApplicationUserPhoto.First().Photo;
                }


            }
           
            _coderPointMetrics = await CoderPointMetricHelper.GetAllAsync(ApplicationSessionViewModel.User.OrganizationId);
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
                return;
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
        private async Task SaveFormAsync()
        {
            _showWait = true;
            AddPhotos();
            await RegisterApplicationUserAsync();
            ToastService.ShowSuccess(Constants.RecordSaved);
            
            _showWait = false;
        }
        private string GetMetricDescription(int? coderPointMetricId)
        {
            var coderPointMetric = _coderPointMetrics.FirstOrDefault(r => r.CoderPointMetricId == coderPointMetricId);
            return coderPointMetric?.MetricDescription ?? string.Empty;
        }

        
    }
}
