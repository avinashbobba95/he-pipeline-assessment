using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace He.Identity.Mvc
{
    /// <summary>
    /// Configures Cookie Policy.
    /// </summary>
    internal static class HeIdentityCookiePolicyExtensions
    {
        /// <summary>
        /// Cookie configuration for Auth0. We need to allow cookies posted from external sites.
        /// </summary>
        /// <param name="services">DI services.</param>
        /// <returns>The DI services.</returns>
        [ExcludeFromCodeCoverage]
        public static IServiceCollection ConfigureCookiePolicyForHeIdentity(this IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
                options.OnAppendCookie = cookieContext => CheckSameSite(cookieContext.CookieOptions);
                options.OnDeleteCookie = cookieContext => CheckSameSite(cookieContext.CookieOptions);
            });

            return services;
        }

        private static void CheckSameSite(CookieOptions options)
        {
            if (options.SameSite == SameSiteMode.None && !options.Secure)
            {
                options.SameSite = SameSiteMode.Unspecified;
            }
        }
    }
}
