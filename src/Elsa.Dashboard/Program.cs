using Elsa.CustomInfrastructure.Data;
using Elsa.Dashboard;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Elsa.Dashboard.Extentions;

var builder = WebApplication.CreateBuilder(args);

// For Dashboard.
builder.Services.AddRazorPages();
builder.Services.AddScoped<NonceConfig>();
//builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<ElsaCustomContext>());
//builder.Services.AddDataProtection().PersistKeysToDbContext<ElsaCustomContext>();

builder.AddCustomAuth0Configuration();


builder.Services.AddAuthorization(options =>
{
  options.FallbackPolicy = new AuthorizationPolicyBuilder()
      .RequireAuthenticatedUser()
      .Build();

  // Register other policies here
});
builder.Services.AddControllers(config =>
{
  var policy = new AuthorizationPolicyBuilder()
                   .RequireAuthenticatedUser()
                   .Build();
  config.Filters.Add(new AuthorizeFilter(policy));
});



var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseMiddleware<SecurityHeaderMiddleware>();

app
    .UseStaticFiles()
        .UseStaticFiles(new StaticFileOptions
        {
          FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"www")),
          ServeUnknownFileTypes = true,
          RequestPath = "/static"
        })// For Dashboard.

    .UseRouting()
    .UseDirectoryBrowser();
//.UseEndpoints(endpoints =>
//{
//  // Elsa API Endpoints are implemented as regular ASP.NET Core API controllers.
//  endpoints.MapControllers();

//  // For Dashboard.
//  endpoints.MapFallbackToPage("/_Host");
//});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=home}/{action=index}/{id?}");

app.Run();
