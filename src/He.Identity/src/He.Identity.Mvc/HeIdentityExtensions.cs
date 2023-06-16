using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using Auth0.AuthenticationApi;
using He.Identity.Mvc.ClientCredentials;
using He.Identity.Mvc.Extensions;
using He.Identity.Mvc.Handlers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace He.Identity.Mvc
{
    /// <summary>
    /// A ServiceCollection extension to add Homes England Authentication to an MVC project.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class HeIdentityExtensions
    {
        private const string RoleClaimType = "http://homesengland.org.uk/claims/roles";
        private const string VerifyEmailErrorDescription = "VERIFY_EMAIL:";

        /// <summary>
        /// Add Homes England Authorisation using JwtBearer.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="heIdentityConfig">Identity Configuration.</param>
        /// <returns>IServiceCollection.</returns>
        public static IServiceCollection AddHeIdentityJwtBearerAuth(this IServiceCollection services, HeIdentityJwtConfiguration heIdentityConfig)
        {
            if (string.IsNullOrWhiteSpace(heIdentityConfig.Audience))
            {
                throw new ArgumentException("Audience must be specified");
            }

            if (string.IsNullOrWhiteSpace(heIdentityConfig.Domain))
            {
                throw new ArgumentException("Domain must be specified");
            }

            services.AddSingleton<HeIdentityConfiguration>(heIdentityConfig);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = $"https://{heIdentityConfig.Domain}";
                options.Audience = heIdentityConfig.Audience;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.NameIdentifier,
                    RoleClaimType = RoleClaimType,
                    ClockSkew = TimeSpan.Zero,
                };
            });

            return services;
        }

        /// <summary>
        /// Add Homes England Authorisation using JwtBearer.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="heIdentityConfig">Identity Configuration.</param>
        /// <returns>IServiceCollection.</returns>
        public static IServiceCollection AddHeIdentityJwtBearerAuths(this IServiceCollection services, HeIdentityMultiSchemeJwtConfiguration heIdentityConfig)
        {
            // Check that the configuration collection is not null
            if (heIdentityConfig == null)
            {
                throw new ArgumentNullException(nameof(heIdentityConfig));
            }

            // Check that at least one configuration has been provided
            if (heIdentityConfig.Configurations.Count < 1)
            {
                throw new ArgumentException("At least one configuration must be provided");
            }

            // Check the validity of each configuration
            for (var i = 0; i < heIdentityConfig.Configurations.Count; i++)
            {
                var configuration = heIdentityConfig.Configurations[i];
                
                if (string.IsNullOrWhiteSpace(configuration.Audience))
                {
                    throw new ArgumentException($"Audience must be specified (Configuration index {i})");
                }

                if (string.IsNullOrWhiteSpace(configuration.Domain))
                {
                    throw new ArgumentException($"Audience must be specified (Configuration index {i})");
                }
            }

            services.AddSingleton(heIdentityConfig);

            var authenticationBuilder = services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });

            var schemes = new List<string>();

            // Add each configuration as a new scheme
            for (var i = 0; i < heIdentityConfig.Configurations.Count; i++)
            {
                var configuration = heIdentityConfig.Configurations[i];

                // Create a unique scheme name for logging purposes
                var schemeName = $"Configuration {i} - {configuration.Domain}";
                schemes.Add(schemeName);

                // Add the scheme with the appropriate values
                authenticationBuilder.AddJwtBearer(schemeName, options =>
                {
                    options.Authority = $"https://{configuration.Domain}";
                    options.Audience = configuration.Audience;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = ClaimTypes.NameIdentifier,
                        RoleClaimType = RoleClaimType,
                        ClockSkew = TimeSpan.Zero,
                    };
                });
            }

            // Set the default policy and include all of the schemes that were created.
            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddAuthenticationSchemes(schemes.ToArray())
                .Build();
            });

            return services;
        }

        /// <summary>
        /// Add Homes England Authorisation using Cookies.
        /// </summary>
        /// <param name="builder">The MVC builder.</param>
        /// <param name="heIdentityConfig">Identity Configuration.</param>
        /// <param name="env">The environment.</param>
        /// <param name="additionalScopes">Any extra required scopes. The following scopes are always included: openid, profile, email, offline_access.</param>
        /// <returns>IServiceCollection.</returns>
        /// <remarks>
        /// Assumes this section is in appsettings.json:
        /// "HeIdentity": {
        ///   "Domain": "",
        ///   "ClientId": "",
        ///   "ClientSecret": ""
        /// }
        /// .
        /// </remarks>
        public static IMvcBuilder AddHeIdentityCookieAuth(this IMvcBuilder builder, HeIdentityCookieConfiguration heIdentityConfig, IWebHostEnvironment env, IEnumerable<string> additionalScopes = null)
        {
            if (string.IsNullOrWhiteSpace(heIdentityConfig.Domain))
            {
                throw new ArgumentException("Domain must be specified");
            }

            if (string.IsNullOrWhiteSpace(heIdentityConfig.ClientId))
            {
                throw new ArgumentException("ClientId must be specified");
            }

            if (string.IsNullOrWhiteSpace(heIdentityConfig.ClientSecret))
            {
                throw new ArgumentException("ClientSecret must be specified");
            }

            builder.Services.AddSingleton<HeIdentityConfiguration>(heIdentityConfig);

            builder.Services.ConfigureCookiePolicyForHeIdentity();

            // Add authentication services
            builder.Services.AddAuthentication(options =>
              {
                  options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                  options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                  options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
              })
            .AddCookie(options =>
            {
                options.Cookie.Name = "he_identity";
                options.AccessDeniedPath = new PathString(HeIdentityPaths.AccessDenied);
                options.LoginPath = new PathString(HeIdentityPaths.SignIn);
                options.ReturnUrlParameter = "redirectUri";
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.None; //TODO is this important?

                options.Events.OnSigningOut = async context =>
                {
                    var revokeRefreshTokenHandler = context.HttpContext.RequestServices.GetRequiredService<RevokeRefreshTokenHandler>();
                    var refreshToken = await context.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);
                    await revokeRefreshTokenHandler.RevokeRefreshTokenAsync(refreshToken);
                };
                options.Events.OnValidatePrincipal = context =>
                {
                    var tokenRefreshHandler = context.HttpContext.RequestServices.GetRequiredService<TokenRefreshHandler>();
                    return tokenRefreshHandler.EnsureTokenRefreshAsync(new CookieValidatePrincipalContextFacade(context), DateTimeOffset.UtcNow);
                };
            })
            .AddOpenIdConnect(HeIdentityConstants.AuthenticationScheme, options =>
            {
                // Set the authority to your Auth0 domain
                options.Authority = $"https://{heIdentityConfig.Domain}";

                // Configure the Auth0 Client ID and Client Secret
                options.ClientId = heIdentityConfig.ClientId;
                options.ClientSecret = heIdentityConfig.ClientSecret;

                // Set response type to code
                options.ResponseType = OpenIdConnectResponseType.Code;
                options.ResponseMode = OpenIdConnectResponseMode.Query;

                // Configure the scope
                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("email");
                options.Scope.Add("offline_access");

                if (additionalScopes != null)
                {
                    foreach (var scope in additionalScopes)
                    {
                        options.Scope.Add(scope);
                    }
                }

                options.CallbackPath = new PathString(HeIdentityPaths.Callback);

                // Configure the Claims Issuer to be Auth0
                options.ClaimsIssuer = "Auth0";

                // Saves tokens to the AuthenticationProperties
                options.SaveTokens = true;

                // Set the correct name claim type
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = ClaimTypes.NameIdentifier,
                    RoleClaimType = RoleClaimType,
                };

                options.Events = new OpenIdConnectEvents
                {
                    OnRedirectToIdentityProvider = context =>
                    {
                        if (context.Properties.Items.ContainsKey("screen_hint"))
                        {
                            context.ProtocolMessage.SetParameter("screen_hint", context.Properties.Items["screen_hint"]);
                        }

                        if (!string.IsNullOrWhiteSpace(heIdentityConfig.Audience))
                        {
                            context.ProtocolMessage.SetParameter("audience", heIdentityConfig.Audience);
                        }

                        return Task.CompletedTask;
                    },
                    OnRedirectToIdentityProviderForSignOut = context =>
                    {
                        var logoutUri = $"https://{heIdentityConfig.Domain}/v2/logout?client_id={heIdentityConfig.ClientId}";

                        var postLogoutUri = context.Properties.RedirectUri;

                        if (!string.IsNullOrEmpty(postLogoutUri))
                        {
                            if (postLogoutUri.StartsWith("/"))
                            {
                                // transform to absolute
                                var request = context.Request;
                                postLogoutUri = request.Scheme + "://" + request.Host + request.PathBase + postLogoutUri;
                            }

                            logoutUri += $"&returnTo={Uri.EscapeDataString(postLogoutUri)}";
                        }

                        context.Response.Redirect(logoutUri);
                        context.HandleResponse();

                        return Task.CompletedTask;
                    },
                    OnRemoteFailure = context =>
                    {
                        if (context.Failure is { Message: "Correlation failed." })
                        {
                            context.Response.Redirect(HeIdentityPaths.SignIn);
                            context.HandleResponse();
                        }

                        if (context.HttpContext.Request.Query.ContainsKey("error") && context.HttpContext.Request.Query["error"] == "unauthorized")
                        {
                            var errorDescription = (string)context.HttpContext.Request.Query["error_description"];

                            if (errorDescription.StartsWith(VerifyEmailErrorDescription))
                            {
                                var emailToVerify = errorDescription.Replace(VerifyEmailErrorDescription, string.Empty).Trim();
                                CookieOptions cookieOptions = new CookieOptions
                                {
                                    HttpOnly = true,
                                    Secure = true,
                                    SameSite = SameSiteMode.Lax,
                                };
                                context.Response.Cookies.Append("emailToVerify", emailToVerify, cookieOptions);
                                context.Response.Redirect(HeIdentityPaths.VerifyEmail);
                                context.HandleResponse();
                            }
                        }

                        return Task.CompletedTask;
                    },
                };

                // when development do not validate SSL certs
                if (env.IsDevelopment())
                {
                    var handler = new HttpClientHandler();
                    handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                    options.BackchannelHttpHandler = handler;
                }
            });

            // Add Auth0 client to help with refreshing of tokens and getting Client Credential access tokens
            builder.Services.AddSingleton<IAuthenticationApiClient>(provider =>
            {
                // reuse the Backchannel created by OpenIdConnect to send HTTP requests to Auth0
                var oidcOptions = provider.GetRequiredService<IOptions<OpenIdConnectOptions>>().Value;
                var connection = new HttpClientAuthenticationConnection(oidcOptions.Backchannel);
                return new AuthenticationApiClient(heIdentityConfig.Domain, connection);
            });

            builder.Services.AddTransient<IClientCredentialHelper, ClientCredentialHelper>();

            builder.Services.AddSingleton(heIdentityConfig);
            builder.Services.AddSingleton<TokenRefreshHandler>();
            builder.Services.AddSingleton<RevokeRefreshTokenHandler>();

            // required for TagHelpers
            builder.Services.AddHttpContextAccessor();

            // Add the He Location controllers and views
            var assembly = Assembly.GetExecutingAssembly();
            builder.AddApplicationPart(assembly);

            builder.Services.Configure<MvcRazorRuntimeCompilationOptions>(options =>
            {
                options.FileProviders.Add(new EmbeddedFileProvider(assembly));
            });

            builder.AddRazorRuntimeCompilation();

            return builder;
        }
    }
}
