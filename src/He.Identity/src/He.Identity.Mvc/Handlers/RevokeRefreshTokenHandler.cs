using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace He.Identity.Mvc.Extensions
{
    /// <summary>
    /// Extension to attempt to revoke the refresh token.
    /// </summary>
    public class RevokeRefreshTokenHandler
    {
        private readonly HeIdentityCookieConfiguration heIdentityConfig;
        private readonly HttpClient backchannel;
        private readonly ILogger<RevokeRefreshTokenHandler> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RevokeRefreshTokenHandler"/> class.
        /// </summary>
        /// <param name="heIdentityConfig">heIdentityConfig.</param>
        /// <param name="openIdConnectOptions">openIdConnectOptions.</param>
        /// <param name="logger">logger.</param>
        public RevokeRefreshTokenHandler(HeIdentityCookieConfiguration heIdentityConfig, IOptions<OpenIdConnectOptions> openIdConnectOptions, ILogger<RevokeRefreshTokenHandler> logger)
        {
            this.heIdentityConfig = heIdentityConfig;
            this.backchannel = openIdConnectOptions.Value.Backchannel;
            this.logger = logger;
        }

        /// <summary>
        /// If there is a refresh token then revokes it from Auth0.
        /// </summary>
        /// <param name="refreshToken">The refresh token to revoke.</param>
        /// <returns>Task.</returns>
        public async Task RevokeRefreshTokenAsync(string refreshToken)
        {
            try
            {
                if (string.IsNullOrEmpty(refreshToken))
                {
                    return;
                }

                var formContent = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        { "client_id", this.heIdentityConfig.ClientId },
                        { "client_secret", this.heIdentityConfig.ClientSecret },
                        { "token", refreshToken },
                    });

                var revokeUri = $"https://{this.heIdentityConfig.Domain}/oauth/revoke";

                await this.backchannel.PostAsync(revokeUri, formContent);

                this.logger.LogInformation("Revoked refresh token");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error revoking refresh token");
            }
        }
    }
}
