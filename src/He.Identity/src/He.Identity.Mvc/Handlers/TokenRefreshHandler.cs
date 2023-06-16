using System;
using System.Threading.Tasks;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Auth0.Core.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace He.Identity.Mvc.Handlers
{
    /// <summary>
    /// TokenRefreshHandler.
    /// </summary>
    public class TokenRefreshHandler
    {
        private readonly HeIdentityCookieConfiguration heIdentityConfig;
        private readonly IAuthenticationApiClient auth0Client;
        private readonly ILogger<TokenRefreshHandler> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenRefreshHandler"/> class.
        /// </summary>
        /// <param name="heIdentityConfig">Config.</param>
        /// <param name="auth0Client">auth0Client.</param>
        /// <param name="logger">logger.</param>
        public TokenRefreshHandler(HeIdentityCookieConfiguration heIdentityConfig, IAuthenticationApiClient auth0Client, ILogger<TokenRefreshHandler> logger)
        {
            this.heIdentityConfig = heIdentityConfig;
            this.auth0Client = auth0Client;
            this.logger = logger;
        }

        /// <summary>
        /// Ensures ID Tokens and Access Tokens are current.
        /// </summary>
        /// <param name="context">CookieValidatePrincipalContextFacade.</param>
        /// <param name="utcNow">UTC now.</param>
        /// <returns>Task.</returns>
        public async Task EnsureTokenRefreshAsync(ICookieValidatePrincipalContextFacade context, DateTimeOffset utcNow)
        {
            try
            {
                if (!context.UserIsAuthenticated)
                {
                    return;
                }

                // If we are checking the session then we don't want to instigate a token refresh.
                if (context.RequestPathValue == HeIdentityPaths.CheckSession)
                {
                    return;
                }

                var currentRefreshToken = context.GetToken(OpenIdConnectParameterNames.RefreshToken);

                // id_token expiry
                var idTokenExpiry = this.GetIdTokenExpiry(context);

                // access_token expiry
                var accesstokenExpiry = this.GetAccessTokenExpiry(context);

                var expiryThreshold = utcNow.AddSeconds(this.heIdentityConfig.RefreshBeforeExpirySeconds);

                // if id_token and access_token are not about to expire then return. Else renew both.
                if (idTokenExpiry > expiryThreshold &&
                    accesstokenExpiry > expiryThreshold)
                {
                    return;
                }

                var refreshTokenRequest = new RefreshTokenRequest
                {
                    ClientId = this.heIdentityConfig.ClientId,
                    ClientSecret = this.heIdentityConfig.ClientSecret,
                    RefreshToken = currentRefreshToken,
                };

                if (!string.IsNullOrEmpty(this.heIdentityConfig.Audience))
                {
                    refreshTokenRequest.Audience = this.heIdentityConfig.Audience;
                }

                var refreshTokenResponse = await this.auth0Client.GetTokenAsync(refreshTokenRequest);

                var expiresAtSeconds = utcNow.AddSeconds(refreshTokenResponse.ExpiresIn);

                context.UpdateToken("expires_at", expiresAtSeconds.ToString("o"));
                context.UpdateToken(OpenIdConnectParameterNames.IdToken, refreshTokenResponse.IdToken);
                context.UpdateToken(OpenIdConnectParameterNames.AccessToken, refreshTokenResponse.AccessToken);
                context.UpdateToken(OpenIdConnectParameterNames.RefreshToken, refreshTokenResponse.RefreshToken);

                context.ShouldRenew();

                this.logger.LogInformation("Refreshed tokens");
            }
            catch (Exception ex)
            {
                context.RejectPrincipal();

                if (ex is ErrorApiException auth0Error)
                {
                    this.logger.LogInformation($"Rejected principal: {auth0Error.Message}");
                }
                else
                {
                    this.logger.LogInformation(ex, "Rejected principal: exception");
                }
            }
        }

        private DateTimeOffset GetIdTokenExpiry(ICookieValidatePrincipalContextFacade context)
        {
            var idTokenString = context.GetToken(OpenIdConnectParameterNames.IdToken);
            var tokenHandler = new JsonWebTokenHandler();
            var idToken = tokenHandler.ReadJsonWebToken(idTokenString);
            var idTokenExpiryFromEpoch = long.Parse(idToken.GetClaim("exp").Value);
            return DateTimeOffset.FromUnixTimeSeconds(idTokenExpiryFromEpoch);
        }

        private DateTimeOffset GetAccessTokenExpiry(ICookieValidatePrincipalContextFacade context)
        {
            var accessTokenExpiresAt = context.GetToken("expires_at");
            return DateTimeOffset.Parse(accessTokenExpiresAt);
        }
    }
}
