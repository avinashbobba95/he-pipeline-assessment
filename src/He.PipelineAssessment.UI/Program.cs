using Elsa.CustomWorkflow.Sdk.Extensions;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using FluentValidation;
using He.Identity.Auth0;
using He.Identity.Mvc;
using He.PipelineAssessment.Data.Auth;
using He.PipelineAssessment.Data.SinglePipeline;
using He.PipelineAssessment.Infrastructure.Data;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Infrastructure.Repository.StoredProcedure;
using He.PipelineAssessment.UI;
using He.PipelineAssessment.UI.Common.Utility;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentTool;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentToolWorkflow;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.UpdateAssessmentTool;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.UpdateAssessmentToolWorkflowCommand;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Mappers;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Validators;
using He.PipelineAssessment.UI.Features.SinglePipeline.Sync;
using He.PipelineAssessment.UI.Features.Workflow.QuestionScreenSaveAndContinue;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var pipelineAssessmentConnectionString = builder.Configuration.GetConnectionString("SqlDatabase");

 string auth0AppClientId = builder.Configuration["Auth0Config:ClientId"];
 string auth0AppClientSecret = builder.Configuration["Auth0Config:ClientSecret"];
 string auth0Domain = builder.Configuration["Auth0Config:Domain"];
 string identifier = builder.Configuration["Auth0Config:Identifier"];

var auth0Config = new Auth0Config(auth0Domain,
    auth0AppClientId,
    auth0AppClientSecret);
//#if HE_LIB

var heIdentityConfiguration = new HeIdentityCookieConfiguration
{
    Domain = auth0Config.Domain,
    ClientId = auth0Config.ClientId,
    ClientSecret = auth0Config.ClientSecret,
    SupportEmail = "foo@bar.com"};

var auth0ManagementConfig = new Auth0ManagementConfig(
    auth0Config.Domain,
    auth0Config.ClientId,
    auth0Config.ClientSecret,
    identifier,
    "???");

var env = builder.Environment;
var mvcBuilder = builder.Services.AddControllersWithViews().AddHeIdentityCookieAuth(heIdentityConfiguration, env);

builder.Services.AddSingleton<HttpClient>();
builder.Services.ConfigureIdentityManagementService(x => x.UseAuth0(auth0Config, auth0ManagementConfig));

builder.Services.ConfigureHeCookieSettings(mvcBuilder,
    configure => { configure.WithAspNetCore().WithHeIdentity().WithApplicationInsights(); });

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<RazorViewEngineOptions>(options =>
{
    options.ViewLocationFormats.Add($"/Features/{{1}}/Views/{{0}}{RazorViewEngine.ViewExtension}");
    options.ViewLocationFormats.Add($"/Features/{{1}}/Views/Shared/{{0}}{RazorViewEngine.ViewExtension}");
    options.ViewLocationFormats.Add($"/Views/Shared/{{0}}{RazorViewEngine.ViewExtension}");
});

string serverURl = builder.Configuration["Urls:ElsaServer"];

//TODO: make this an extension in the SDK
builder.Services.AddHttpClient("ElsaServerClient", client =>
{
    client.BaseAddress = new Uri(serverURl);

});

builder.Services.AddScoped<IElsaServerHttpClient, ElsaServerHttpClient>();
builder.Services.AddScoped<IQuestionScreenSaveAndContinueMapper, QuestionScreenSaveAndContinueMapper>();
builder.Services.AddScoped<IAssessmentToolMapper, AssessmentToolMapper>();
builder.Services.AddScoped<IAssessmentToolWorkflowMapper, AssessmentToolWorkflowMapper>();
builder.Services.AddScoped<NonceConfig>();


builder.Services.AddMediatR(typeof(Program).Assembly);
builder.Services.AddApplicationInsightsTelemetry();

builder.Services.AddDbContext<PipelineAssessmentContext>(config =>
    config.UseSqlServer(pipelineAssessmentConnectionString,
        x => x.MigrationsAssembly("He.PipelineAssessment.Infrastructure")));

builder.Services.AddDbContext<PipelineAssessmentStoreProcContext>(config =>
    config.UseSqlServer(pipelineAssessmentConnectionString));

builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<PipelineAssessmentContext>());
builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<PipelineAssessmentStoreProcContext>());

//Validators
builder.Services.AddScoped<IValidator<QuestionScreenSaveAndContinueCommand>, SaveAndContinueCommandValidator>();
builder.Services.AddScoped<IValidator<CreateAssessmentToolCommand>, CreateAssessmentToolCommandValidator>();
builder.Services.AddScoped<IValidator<CreateAssessmentToolWorkflowCommand>, CreateAssessmentToolWorkflowCommandValidator>();
builder.Services.AddScoped<IValidator<UpdateAssessmentToolCommand>, UpdateAssessmentToolCommandValidator>();
builder.Services.AddScoped<IValidator<UpdateAssessmentToolWorkflowCommand>, UpdateAssessmentToolWorkflowCommandValidator>();
builder.Services.AddScoped<ISinglePipelineProvider, SinglePipelineProvider>();

builder.Services.AddDataProtection().PersistKeysToDbContext<PipelineAssessmentContext>();
builder.Services.AddScoped<IAssessmentRepository, AssessmentRepository>();
builder.Services.AddScoped<IStoredProcedureRepository, StoredProcedureRepository>();
builder.Services.AddScoped<IAdminAssessmentToolRepository, AdminAssessmentToolRepository>();
builder.Services.AddScoped<IAdminAssessmentToolWorkflowRepository, AdminAssessmentToolWorkflowRepository>();
builder.Services.AddScoped<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddScoped<ISyncCommandHandlerHelper, SyncCommandHandlerHelper>();


builder.Services.AddScoped<IIdentityClient, IdentityClient>();
builder.Services.AddTransient<BearerTokenHandler>();

builder.Services.AddOptions<IdentityClientConfig>()
.Configure<IConfiguration>((settings, configuration) =>
{
    configuration.GetSection("IdentityClientConfig").Bind(settings);
});

builder.Services.AddSinglePipelineClient(builder.Configuration, builder.Environment.IsDevelopment());


builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    // options.AddPolicy(AuthorizationPolicies.AssignmentToPipelineAssessorRoleRequired, policy => policy.RequireRole(AppRole.PipelineAssessor));
    //options.AddPolicy(AuthorizationPolicies.AssignmentToPipelineAdminRoleRequired, policy => policy.RequireRole(AppRole.PipelineAdmin));
});
builder.Services.AddControllers(config =>
{
    var policy = new AuthorizationPolicyBuilder()
                     .RequireAuthenticatedUser()
                     .Build();
    config.Filters.Add(new AuthorizeFilter(policy));
});

var app = builder.Build();




if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<PipelineAssessmentContext>();
    context.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error/Index");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseMiddleware<SecurityHeaderMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Assessment}/{action=Index}/{id?}");

app.Run();

public static class AppRole
{

    public const string PipelineAdmin = "Pipeline.Admin";
    public const string PipelineAssessor = "Pipeline.Assessor";
}

public static class AuthorizationPolicies
{
    public const string AssignmentToPipelineAssessorRoleRequired = "AssignmentToPipelineAssessorRoleRequired";
    public const string AssignmentToPipelineAdminRoleRequired = "AssignmentToPipelineAdminRoleRequired";
}
