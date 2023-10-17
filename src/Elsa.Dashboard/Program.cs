using Elsa.CustomInfrastructure.Data;
using Elsa.CustomInfrastructure.Extensions;
using Elsa.CustomWorkflow.Sdk;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.Dashboard;
using Elsa.Dashboard.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var elsaCustomConnectionString = builder.Configuration.GetConnectionString("ElsaCustom");

// For Dashboard.
builder.Services.AddRazorPages();

builder.Services.AddScoped<NonceConfig>();

builder.Services.AddDbContext<ElsaCustomContext>(config =>
    config.UseSqlServer(
        elsaCustomConnectionString,
        x => x.MigrationsAssembly("Elsa.CustomInfrastructure")));

builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<ElsaCustomContext>());
builder.Services.AddDataProtection().PersistKeysToDbContext<ElsaCustomContext>();
builder.Services.AddApplicationInsightsTelemetry();

builder.Services.Configure<Urls>(
            builder.Configuration.GetSection("Urls"));

builder.Services.Configure<Auth0Config>(
  builder.Configuration.GetSection("Auth0Config"));

Dictionary<string, TokenConfig> tokenProviderConfig = new Dictionary<string, TokenConfig>();

var domain = builder.Configuration["Auth0Config:Domain"];
var clientId = builder.Configuration["Auth0Config:MachineToMachineClientId"];
var clientSecret = builder.Configuration["Auth0Config:MachineToMachineClientSecret"];
var audience = builder.Configuration["Auth0Config:Audience"];
var pipelineDomain = builder.Configuration["PipelineAuth0Config:Domain"];
var pipelineClientId = builder.Configuration["PipelineAuth0Config:MachineToMachineClientId"];
var pipelineClientSecret = builder.Configuration["PipelineAuth0Config:MachineToMachineClientSecret"];
var pipelineAudience = builder.Configuration["PipelineAuth0Config:Audience"];
var serviceConfig = new TokenConfig(domain, clientId, clientSecret, audience);
var pipelineConfig = new TokenConfig(pipelineDomain, pipelineClientId, pipelineClientSecret, pipelineAudience);

tokenProviderConfig.Add(TokenProviderKeys.ElsaServer, serviceConfig);
tokenProviderConfig.Add(TokenProviderKeys.Pipeline, serviceConfig);

TokenProvider provider = new TokenProvider(tokenProviderConfig);

builder.Services.AddSingleton<ITokenProvider>(provider);


string serverURl = builder.Configuration["Urls:ElsaServer"];
builder.Services.AddHttpClient("ElsaServerClient", client =>
{
  client.BaseAddress = new Uri(serverURl);
});

string pipelineUrl = builder.Configuration["Urls:Pipeline"];
builder.Services.AddHttpClient("PipelineClient", client =>
{
  client.BaseAddress = new Uri(pipelineUrl);
});


builder.Services.AddScoped<IElsaServerHttpClient, ElsaServerHttpClient>();
builder.Services.AddScoped<IPipelineAssessmentHttpClient, PipelineAssessmentHttpClient>();

// For Authentication
builder.AddCustomAuth0Configuration();
builder.Services.AddCustomAuthentication();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Error");

}

app.Use((context, next) =>
{
  context.Request.Scheme = "https";
  return next();
});


app.Use((context, next) =>
{
  context.Request.Scheme = "https";
  return next();
});

app.UseMiddleware<SecurityHeaderMiddleware>();

app.UseStaticFiles().UseStaticFiles(new StaticFileOptions
{
  FileProvider =
      new Microsoft.Extensions.FileProviders.PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(),
        @"www")),
  ServeUnknownFileTypes = true,
  RequestPath = "/static"
})
  // For Dashboard.
  .UseRouting()
  .UseAuthentication()
  .UseAuthorization()
  .UseEndpoints(endpoints =>
  {
    // Elsa API Endpoints are implemented as regular ASP.NET Core API controllers.
    endpoints
      .MapControllers().RequireAuthorization();
    // For Dashboard.
    endpoints.MapFallbackToPage("/_Host");
  });
app.Run();
