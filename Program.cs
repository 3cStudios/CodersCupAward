using Blazored.Toast;
using CodersCupAward;
using CodersCupAward.Components;
using CodersCupAward.Helper;
using CodersCupAward.Models;
using CodersCupAward.Services;
using CodersCupAward.ViewModels;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = builder.Configuration;

if (Environment.GetEnvironmentVariable("3cAzureAppConfig") is null)
{
    //Throw error
    throw new Exception(
        "The AzureAppConfig env variable is missing that is required to read variables from Azure App Configuration");
}
string? hostingEnvironmentName = null;
builder.Logging.AddConfiguration(configuration.GetSection("Logging"));
builder.Logging.AddConsole();
builder.Logging.AddDebug();

hostingEnvironmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

configuration.AddAzureAppConfiguration(options =>
    options
        .Connect(Environment.GetEnvironmentVariable("3cAzureAppConfig"))
        .ConfigureRefresh(refresh =>
        {
            refresh.Register(Constants.ConfigAppServiceName + ":SETTINGS:SENTINEL",
                    refreshAll: true)
                .SetCacheExpiration(new TimeSpan(0, 5, 0));
        })
        .Select("Global.*", LabelFilter.Null)
        .Select(Constants.ConfigAppServiceName + ".*", LabelFilter.Null)
        .Select("Global.*", hostingEnvironmentName)
        .Select(Constants.ConfigAppServiceName + ".*",
            hostingEnvironmentName)


);

builder.Services.AddRazorComponents(options =>
    options.DetailedErrors = builder.Environment.IsDevelopment())

    .AddInteractiveServerComponents();
builder.Services.AddBlazoredToast();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddDbContext<coderscupawardContext>(options =>
    options.UseSqlServer(configuration["CodersCupAward.ConnectionString"],
        providerOptions =>
        {
            providerOptions.EnableRetryOnFailure(
                maxRetryCount: 30, // Maximum number of retry attempts
                maxRetryDelay: TimeSpan.FromSeconds(30), // Maximum delay between retries
                errorNumbersToAdd: null); // Additional error numbers to consider for retry); 
        }),
            ServiceLifetime.Transient);

builder.Services.AddApplicationInsightsTelemetry(new ApplicationInsightsServiceOptions()
{ ConnectionString = $"{configuration["CodersCupAward.ApplicationInsights"]}" });

builder.Services.AddSingleton<ITelemetryInitializer, MyTelemetryInitializer>();
builder.Services.Configure<EmailOptions>(model => model.SendGridApiKey = configuration["Global.Email.SendGridApiKey"]);
builder.Services.Configure<EmailOptions>(model => model.DefaultFrom = configuration["CodersCupAward.DefaultFrom"]);
builder.Services.Configure<EmailOptions>(model => model.DefaultTo = configuration["CodersCupAward.DefaultTo"]);

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<IOrganizationService, OrganizationService>();
builder.Services.AddScoped<IOrganizationHelper, OrganizationHelper>();
builder.Services.AddScoped<IApplicationUserHelper, ApplicationUserHelper>();
builder.Services.AddScoped<IApplicationUserService, ApplicationUserService>();
builder.Services.AddScoped<IApplicationUserPhotoService, ApplicationUserPhotoService>();
builder.Services.AddScoped<IApplicationUserPhotoHelper, ApplicationUserPhotoHelper>();
builder.Services.AddScoped<ITranslationLayerHelper, TranslationLayerHelper>();
builder.Services.AddScoped<ITextTranslatorService, TextTranslatorService>();
builder.Services.AddScoped<IHtmlTemplateService, HtmlTemplateService>();
builder.Services.AddScoped<IHtmlTemplateHelper, HtmlTemplateHelper>();
builder.Services.AddScoped<IEmailLogService, EmailLogService>();
builder.Services.AddScoped<IEmailLogHelper, EmailLogHelper>();
builder.Services.AddScoped<IEmailHelper, EmailHelper>();
builder.Services.AddScoped<IEmailHelper, EmailHelper>();
builder.Services.AddScoped<IApplicationSessionHelper, ApplicationSessionHelper>();
builder.Services.AddScoped<IApplicationSessionService, ApplicationSessionService>();
builder.Services.AddScoped<IApplicationRoleHelper, ApplicationRoleHelper>();
builder.Services.AddScoped<IApplicationRoleService, ApplicationRoleService>();
builder.Services.AddScoped<IApplicationUserRoleHelper, ApplicationUserRoleHelper>();
builder.Services.AddScoped<IApplicationUserRoleService, ApplicationUserRoleService>();
builder.Services.AddScoped<ICoderPointTrackingHelper, CoderPointTrackingHelper>();
builder.Services.AddScoped<ICoderPointTrackingService, CoderPointTrackingService>();
builder.Services.AddScoped<ICoderPointMetricHelper, CoderPointMetricHelper>();
builder.Services.AddScoped<ICoderPointMetricService, CoderPointMetricService>();
builder.Services.AddScoped<ISecurityHelper, SecurityHelper>();
builder.Services.AddScoped<IIterationHelper, IterationHelper>();
builder.Services.AddScoped<IIterationService, IterationService>();


builder.Services.AddCascadingValue(sp => new ApplicationSessionViewModel { ApplicationSessionId = Guid.NewGuid() });


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
