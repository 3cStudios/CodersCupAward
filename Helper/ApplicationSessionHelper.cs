using CodersCupAward.Services;
using CodersCupAward.ViewModels;
using Microsoft.JSInterop;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;


namespace CodersCupAward.Helper
{
    public class ApplicationSessionHelper : IApplicationSessionHelper
    {
        private readonly IJSRuntime _jSRuntime;
        private readonly IApplicationSessionService _applicationSessionService;

        public ApplicationSessionHelper(IJSRuntime jSRuntime, IApplicationSessionService applicationSessionService) { 

            _jSRuntime = jSRuntime;
            _applicationSessionService = applicationSessionService;
        }

        public async Task<ApplicationSessionViewModel> GetApplicationSession(Guid sessionId)
        {
            var applicationSession = await _applicationSessionService.GetBySessionIdAsync(sessionId);
            return JsonSerializer.Deserialize<ApplicationSessionViewModel>(applicationSession.SessionData) ?? new ApplicationSessionViewModel();
        }
        public async Task TrackSession(ApplicationSessionViewModel applicationSessionViewModel)
        {
            if (applicationSessionViewModel.TimeZoneInfo == null)
            {
                var timeZone = await _jSRuntime.InvokeAsync<string>(Constants.JSfunctionForTimeZone);
                applicationSessionViewModel.TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
                
            }
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                WriteIndented = true
            };

            var jsonString = JsonSerializer.Serialize(applicationSessionViewModel, options);
            await _applicationSessionService.SaveAsync(new Models.ApplicationSession
            {
                ApplicationSessionId = applicationSessionViewModel.ApplicationSessionId,
                SessionData = jsonString

            });
        }
    }
}
