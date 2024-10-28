using Blazored.Toast.Services;
using CodersCupAward.Components.PageComponents;
using CodersCupAward.Extensions;
using CodersCupAward.Helper;
using CodersCupAward.Models;
using CodersCupAward.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace CodersCupAward.Components.Pages.Coaches
{
    public partial class CoachAddPoints: ComponentBase
    {
        [Inject] private ITranslationLayerHelper TranslationLayerHelper { get; set; }
        [Inject] private ICoderPointTrackingHelper CoderPointTrackingHelper { get; set; }
        [Inject] private ICoderPointMetricHelper CoderPointMetricHelper { get; set; }
        [Inject] private IToastService ToastService { get; set; }
        [Inject] private IIterationHelper IterationHelper { get; set; }
        [Inject] private IEmailHelper EmailHelper { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private IJSRuntime JS { get; set; }

        [CascadingParameter] public ApplicationSessionViewModel ApplicationSessionViewModel { get; set; }

        [Parameter] public EventCallback OnPointsChange { get; set; }

        private CoderPointTracking _coderPointTracking = new();
        private CoderPointTracking _coderPointTrackingForDelete;
        private DialogConfirm _dialogConfirm;
        private List<CoderPointMetric> _coderPointMetrics = new();
        private List<Iteration> iterationList = new();
        private List<Iteration> _allIterationList = new();
        private Iteration _selectedIteration;
        private List<CoderPointTracking> _coderPointTrackingList = new();
               

        protected override async Task OnInitializedAsync()
        {
            _coderPointMetrics = await CoderPointMetricHelper.GetAllAsync(ApplicationSessionViewModel.User.OrganizationId);
            await LoadIterationsAsync();
        
        }
        private async Task LoadIterationsAsync()
        {
            _allIterationList = await IterationHelper.GetAllAsync(ApplicationSessionViewModel.User.OrganizationId);
            iterationList = _allIterationList
                .Where(r => r.StartDate.Date <= DateTime.Now.Date)
                .OrderByDescending(r => r.StartDate).ToList();
            _selectedIteration = iterationList.FirstOrDefault() ?? new Iteration();
        }
        private void DeletePoints(CoderPointTracking coderPointTracking)
        {
            _coderPointTrackingForDelete = coderPointTracking;
            _dialogConfirm.Confirm("Confirm", "Are you sure you want to delete this record?");

        }
        private async Task OnDeleteConfirmation(bool confirmed)
        {
            if (confirmed)
            {
                _coderPointTrackingForDelete.IsDeleted = true;
                _coderPointTrackingForDelete.ModifiedDate = DateTime.Now;
                _coderPointTrackingForDelete.ModifiedBy = ApplicationSessionViewModel.User.FullName();
                await CoderPointTrackingHelper.DeleteAsync(_coderPointTrackingForDelete);
                ApplicationSessionViewModel.EditUser.CoderPointTracking.Remove(_coderPointTrackingForDelete);
                await OnPointsChange.InvokeAsync();
                ToastService.ShowSuccess("Record deleted successfully.");
                StateHasChanged();
            }
        }
        private bool FormValidation()
        {
            if (_coderPointTrackingList.Count == 0)
            {
                ToastService.ShowWarning("Please enter points to save.");
                return false;
            }
            if (_selectedIteration == null || _selectedIteration.IterationId == 0)
            {
                ToastService.ShowWarning("Please select an iteration.");
                return false;
            }
            if (string.IsNullOrEmpty(_coderPointTracking.EntryReference))
            {
                ToastService.ShowWarning("Please enter a reference.");
                return false;
            }
            return true;
        }
        private async Task SavePoints()
        {
            if (!FormValidation())
            {
                return;
            }
         
            foreach (var coderPointTracking in _coderPointTrackingList)
            {
                coderPointTracking.CreatedDate = DateTime.Now;
                coderPointTracking.CreatedBy = ApplicationSessionViewModel.User.FullName();
                await CoderPointTrackingHelper.SaveAsync(coderPointTracking);
                ApplicationSessionViewModel.EditUser.CoderPointTracking.Add(coderPointTracking);
            }
            _coderPointTracking = new CoderPointTracking();
            await EmailHelper.SendPointsAwardedAsync(ApplicationSessionViewModel.EditUser, _coderPointTrackingList, _coderPointMetrics, "en");
            ToastService.ShowSuccess("Record saved successfully and team member notification sent.");
            await OnPointsChange.InvokeAsync();
        }
        private string GetMetricDescription(int? coderPointMetricId)
        {
            var coderPointMetric = _coderPointMetrics.FirstOrDefault(r => r.CoderPointMetricId == coderPointMetricId);
            return coderPointMetric?.MetricDescription ?? string.Empty;
        }
        private async Task OnInputChange(ChangeEventArgs e, int coderPointMetricId)
        {
            var value = e.Value.ToString();
            var isNumber = int.TryParse(e.Value.ToString(), out var nValue);
            if (!isNumber)
            {
                await JS.InvokeVoidAsync("InvalidInputValue", coderPointMetricId);
                ToastService.ShowWarning("Input only accepts numeric values.");
                return;
            }

            await JS.InvokeVoidAsync("ValidInputValue", coderPointMetricId);

            if (_coderPointTrackingList.Exists(r => r.CoderPointMetricId == coderPointMetricId))
            {
                _coderPointTrackingList.Remove(_coderPointTrackingList.First(r => r.CoderPointMetricId == coderPointMetricId));
            }
            if (value == string.Empty)
            {
                return;
            }

            _coderPointTrackingList.Add(new CoderPointTracking
            {
                Points = nValue,
                CoderPointMetricId = coderPointMetricId,
                ApplicationUserId = ApplicationSessionViewModel.EditUser.ApplicationUserId,
                IterationId = _selectedIteration.IterationId,
                EntryReference = _coderPointTracking.EntryReference,
                CreatedDate = DateTime.Now,
                CreatedBy = ApplicationSessionViewModel.User.FullName()
            });

        }
        private string GetUserImage()
        {
            if (ApplicationSessionViewModel.EditUser != null
                && ApplicationSessionViewModel.EditUser.ApplicationUserPhoto.Any())
            {
                return ApplicationSessionViewModel.EditUser.ApplicationUserPhoto.First().Photo;
            }
            return Constants.MissingProfileImage;
        }
    }
}
