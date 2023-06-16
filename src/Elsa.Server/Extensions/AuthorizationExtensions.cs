//using He.Identity.Auth0;
//using He.Identity.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using He.Identity.Mvc;
using He.Identity.Auth0;

namespace Elsa.Server.Extensions
{
    public static class AuthorizationExtensions
    {
        public static void AddCustomAuth0Configuration(this WebApplicationBuilder builder)
        {

            string auth0AppClientId = builder.Configuration["DashboardAuth0Config:ClientId"];
            string auth0AppClientSecret = builder.Configuration["DashboardAuth0Config:ClientSecret"];
            string auth0Domain = builder.Configuration["DashboardAuth0Config:Domain"];
            string identifier = builder.Configuration["DashboardAuth0Config:Identifier"];
            string supportEmail = builder.Configuration["DashboardAuth0Config:SupportEmail"];

            var auth0Config = new Auth0Config(auth0Domain, auth0AppClientId, auth0AppClientSecret);

            var heIdentityConfiguration = new HeIdentityCookieConfiguration
            {
                Domain = auth0Config.Domain,
                ClientId = auth0Config.ClientId,
                ClientSecret = auth0Config.ClientSecret,
                SupportEmail = supportEmail
            };

            var auth0ManagementConfig = new Auth0ManagementConfig(
                                        auth0Config.Domain,
                                        auth0Config.ClientId,
                                        auth0Config.ClientSecret,
                                        identifier,
                                        "???");

            var env = builder.Environment;

            var mvcBuilder = builder.Services.AddControllersWithViews().AddHeIdentityCookieAuth(heIdentityConfiguration, env);

            builder.Services.AddSingleton((HeIdentityConfiguration)heIdentityConfiguration);

            builder.Services.ConfigureIdentityManagementService(x => x.UseAuth0(auth0Config, auth0ManagementConfig));

            builder.Services.ConfigureHeCookieSettings(mvcBuilder,
                configure => { configure.WithAspNetCore().WithHeIdentity().WithApplicationInsights(); });
        }

        public static void AddCustomAuthentication(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder().RequireRole("ElsaDashboard.Admin").Build();

                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                          .RequireAuthenticatedUser()
                          .Build();
            });
        }
    }

}
