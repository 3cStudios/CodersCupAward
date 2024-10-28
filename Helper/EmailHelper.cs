using System.Text;
using CodersCupAward.Models;
using CodersCupAward.Services;
using CodersCupAward.ViewModels;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace CodersCupAward.Helper
{
    public class EmailHelper : IEmailHelper
    {
        private readonly IHtmlTemplateService _htmlTemplateService;
        private readonly EmailOptions _emailOptions;
        private readonly IEmailLogService _emailLogService;
        private readonly ITextTranslatorService _textTranslatorService;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EmailHelper(IHtmlTemplateService htmlTemplateService,
        
            IOptions<EmailOptions> emailOptions,
            IEmailLogService emailLogService,
            ITextTranslatorService textTranslatorService,
            IWebHostEnvironment env,
            IHttpContextAccessor httpContextAccessor)
        {
            _htmlTemplateService = htmlTemplateService;
            _emailOptions = emailOptions.Value;
            _emailLogService = emailLogService;
            _textTranslatorService = textTranslatorService;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<int> SendEmailAsync(MessageEmailViewModel messageEmailViewModel)
        {
            var client = new SendGridClient(_emailOptions.SendGridApiKey);
            var from = new EmailAddress(messageEmailViewModel.EmailFromAddress);
            var subject = messageEmailViewModel.Subject;
            var to = new EmailAddress(messageEmailViewModel.EmailTo);
            if (_env.EnvironmentName is "Development" or "Dev_Region")
            {
                to = new EmailAddress(Constants.DevelopmentEmailAddress);
            }

            var htmlContent = messageEmailViewModel.Body;

            var msg = MailHelper.CreateSingleEmail(from, to, subject, string.Empty, htmlContent);
            if (!string.IsNullOrEmpty(messageEmailViewModel.EmailCc))
            {
                msg.AddBcc(new EmailAddress(messageEmailViewModel.EmailCc));
            }
            var response = await client.SendEmailAsync(msg);
            if (response.IsSuccessStatusCode)
            {
                return await _emailLogService.SaveAsync(new EmailLog
                {
                    EmailLogId = 0,
                    EmailFromAddress = messageEmailViewModel.EmailFromAddress,
                    EmailTo = messageEmailViewModel.EmailTo,
                    Subject = messageEmailViewModel.Subject,
                    Body = messageEmailViewModel.Body,
                    ReplyTo = messageEmailViewModel.EmailFromAddress,
                    InsertDate = DateTime.UtcNow,
                });
            }
            return -1;
        }

        private static string FillBaseTemplate(string baseTemplate, string titleLine, string contentBody)
        {

            var messageBody = baseTemplate;
            messageBody = messageBody.Replace(Constants.LogoUrlTag, Constants.DefaultLogoUrl);
            messageBody = messageBody.Replace(Constants.TitleLineTag, titleLine);
            messageBody = messageBody.Replace(Constants.FormContentTag, contentBody);
            messageBody = messageBody.Replace(Constants.MadeByUrlTag, Constants.BaseUrl);

            return messageBody;
        }

        private string CurrentUrl()
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                return "https://www.CodersCupAward.com";
            }
            var request = _httpContextAccessor.HttpContext.Request;
            var host = request.Host;
            var path = request.Path;
            return $"{request.Scheme}://{host}";
        }

        public async Task<int> SendInvitationEmailAsync(ApplicationUser applicationUser, string languageCode)
        {
            var emailBaseTemplate = await _htmlTemplateService.GetAsync(Constants.EmailBaseTemplate);
            var requestTemplate = await _htmlTemplateService.GetAsync(Constants.EmailInvitationTemplate);
            var messageBody = requestTemplate.HTML;
            
            messageBody = messageBody
                .Replace(Constants.OrganizationNameTag, $"{applicationUser.Organization.OrganizationName}")
                .Replace(Constants.InvitationUrlTag, $"{CurrentUrl()}/Invitation/{applicationUser.ApplicationUserUniqueId}");

            var emailBody = FillBaseTemplate(emailBaseTemplate.HTML, Constants.UserInviteSubject, messageBody);

            var messageViewModel = new MessageEmailViewModel
            {
                EmailTo = applicationUser.EmailAddress,
                Subject = Constants.UserInviteSubject,
                Body = emailBody,
                EmailFromAddress = _emailOptions.DefaultFrom
            };


            return await SendEmailAsync(messageViewModel);

        }

        public async Task<int> SendEmailVerificationAsync(ApplicationUser applicationUser, string languageCode)
        {
            var emailBaseTemplate = await _htmlTemplateService.GetAsync(Constants.EmailBaseTemplate);
            var requestTemplate = await _htmlTemplateService.GetAsync(Constants.EmailVerificationTemplate);
            var messageBody = requestTemplate.HTML;

            messageBody = messageBody
                .Replace(Constants.OrganizationNameTag, $"{applicationUser.Organization.OrganizationName}")
                .Replace(Constants.InvitationUrlTag, $"{CurrentUrl()}/RegisterEmailVerification/{applicationUser.ApplicationUserUniqueId}");

            var emailBody = FillBaseTemplate(emailBaseTemplate.HTML, Constants.UserEmailVerification, messageBody);

            var messageViewModel = new MessageEmailViewModel
            {
                EmailTo = applicationUser.EmailAddress,
                Subject = Constants.UserEmailVerification,
                Body = emailBody,
                EmailFromAddress = _emailOptions.DefaultFrom
            };


            return await SendEmailAsync(messageViewModel);

        }

        public async Task<int> SendJoinApprovalEmailAsync(ApplicationUser applicationUser,
            ApplicationUser coachUser,string languageCode)
        {
            var emailBaseTemplate = await _htmlTemplateService.GetAsync(Constants.EmailBaseTemplate);
            var requestTemplate = await _htmlTemplateService.GetAsync(Constants.EmailJoinVerificationTemplate);
            var messageBody = requestTemplate.HTML;

            messageBody = messageBody
                .Replace(Constants.OrganizationNameTag, $"{applicationUser.Organization.OrganizationName}")
                .Replace("{%FirstName%}", $"{applicationUser.FirstName}")
                .Replace("{%LastName%}", $"{applicationUser.LastName}")
                .Replace("{%JoinVerificationUrl%}", $"{CurrentUrl()}/CoachEditTeamMember/{applicationUser.ApplicationUserUniqueId}");

            var emailBody = FillBaseTemplate(emailBaseTemplate.HTML, Constants.TeamMemberJoinApproval, messageBody);

            var messageViewModel = new MessageEmailViewModel
            {
                EmailTo = coachUser.EmailAddress,
                Subject = Constants.TeamMemberJoinApproval,
                Body = emailBody,
                EmailFromAddress = _emailOptions.DefaultFrom
            };


            return await SendEmailAsync(messageViewModel);

        }

        public async Task<int> SendEmailPasswordResetAsync(ApplicationUser applicationUser, string languageCode)
        {
            var emailBaseTemplate = await _htmlTemplateService.GetAsync(Constants.EmailBaseTemplate);
            var requestTemplate = await _htmlTemplateService.GetAsync(Constants.EmailPasswordResetTemplate);
            var messageBody = requestTemplate.HTML;

            messageBody = messageBody
                .Replace(Constants.OrganizationNameTag, $"{applicationUser.Organization.OrganizationName}")
                .Replace(Constants.PasswordResetUrlTag, $"{CurrentUrl()}/PasswordResetEmailVerification/{applicationUser.ApplicationUserUniqueId}");

            var emailBody = FillBaseTemplate(emailBaseTemplate.HTML, Constants.PasswordResetSubject, messageBody);

            var messageViewModel = new MessageEmailViewModel
            {
                EmailTo = applicationUser.EmailAddress,
                Subject = Constants.PasswordResetSubject,
                Body = emailBody,
                EmailFromAddress = _emailOptions.DefaultFrom
            };


            return await SendEmailAsync(messageViewModel);

        }

        public async Task<int> SendLeaderboardUpdateAsync(ApplicationUser applicationUser, string languageCode)
        {
            var emailBaseTemplate = await _htmlTemplateService.GetAsync(Constants.EmailBaseTemplate);
            var requestTemplate = await _htmlTemplateService.GetAsync(Constants.EmailLeaderboardUpdateTemplate);
            var messageBody = requestTemplate.HTML;

            messageBody = messageBody
                .Replace(Constants.OrganizationNameTag, $"{applicationUser.Organization.OrganizationName}")
                .Replace(Constants.LeaderboardUrlTag, $"{CurrentUrl()}/LeaderBoard/{applicationUser.ApplicationUserUniqueId}");

            var emailBody = FillBaseTemplate(emailBaseTemplate.HTML, Constants.LeaderBoardUpdate, messageBody);

            var messageViewModel = new MessageEmailViewModel
            {
                EmailTo = applicationUser.EmailAddress,
                Subject = Constants.LeaderBoardUpdate,
                Body = emailBody,
                EmailFromAddress = _emailOptions.DefaultFrom
            };


            return await SendEmailAsync(messageViewModel);

        }

        public async Task<int> SendPointsAwardedAsync(ApplicationUser applicationUser, List<CoderPointTracking> coderPointTrackingLis,
            List<CoderPointMetric> coderPointMetrics, string languageCode)
        {
            var emailBaseTemplate = await _htmlTemplateService.GetAsync(Constants.EmailBaseTemplate);
            var requestTemplate = await _htmlTemplateService.GetAsync(Constants.EmailPointsAwardedTemplate);
            var messageBody = requestTemplate.HTML;

            var pointsAwardedHtml = new StringBuilder();
            pointsAwardedHtml.Append("<table><tr><th style=\"padding:5px;\">Points</th><th style=\"padding:5px;\">Metric</th></tr>");

            foreach (var coderPointTracking in coderPointTrackingLis)
            {
                pointsAwardedHtml.Append($"<tr><td style=\"padding:5px;\">{coderPointTracking.Points}</td><td style=\"padding:5px;\">{coderPointMetrics.First(r=>r.CoderPointMetricId== coderPointTracking.CoderPointMetricId).MetricDescription}</td></tr>");
            }
            pointsAwardedHtml.Append("</table>");

            messageBody = messageBody
                .Replace(Constants.ReferenceTag, $"{applicationUser.CoderPointTracking.First().EntryReference}")
                .Replace(Constants.PointsAwardedTag, pointsAwardedHtml.ToString());

            var emailBody = FillBaseTemplate(emailBaseTemplate.HTML, Constants.PointsAwarded, messageBody);

            var messageViewModel = new MessageEmailViewModel
            {
                EmailTo = applicationUser.EmailAddress,
                Subject = Constants.PointsAwarded,
                Body = emailBody,
                EmailFromAddress = _emailOptions.DefaultFrom
            };


            return await SendEmailAsync(messageViewModel);

        }


    }
}