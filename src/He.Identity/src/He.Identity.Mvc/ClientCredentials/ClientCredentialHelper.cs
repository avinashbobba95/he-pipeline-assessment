using System;
using System.Threading.Tasks;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;

namespace He.Identity.Mvc.ClientCredentials
{
    /// <inheritdoc/>
    public class ClientCredentialHelper : IClientCredentialHelper
    {
        private readonly IAuthenticationApiClient auth0Client;
        private readonly HeIdentityCookieConfiguration heIdentityConfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientCredentialHelper"/> class.
        /// </summary>
        /// <param name="auth0Client">authClient.</param>
        /// <param name="heIdentityConfig">heIdentityConfig.</param>
        public ClientCredentialHelper(IAuthenticationApiClient auth0Client, HeIdentityCookieConfiguration heIdentityConfig)
        {
            this.auth0Client = auth0Client;
            this.heIdentityConfig = heIdentityConfig;
        }

        /// <inheritdoc/>
        public Task<ClientCredentialAccessToken> GetAccessTokenAsync(string audience)
        {
            return this.GetAccessTokenAsync(this.heIdentityConfig.ClientId, this.heIdentityConfig.ClientSecret, audience);
        }

        /// <inheritdoc/>
        public async Task<ClientCredentialAccessToken> GetAccessTokenAsync(string clientId, string clientSecret, string audience)
        {
            var request = new ClientCredentialsTokenRequest
            {
                Audience = audience,
                ClientId = clientId,
                ClientSecret = clientSecret,
            };

            try
            {
                var auth0AccessTokenResponse = await this.auth0Client.GetTokenAsync(request);

                var response = new ClientCredentialAccessToken
                {
                    AccessToken = auth0AccessTokenResponse.AccessToken,
                    ExpiresIn = auth0AccessTokenResponse.ExpiresIn,
                    TokenType = auth0AccessTokenResponse.TokenType,
                };

                return response;
            }
            catch (Exception ex)
            {
                throw new HeIdentityException($"Error obtaining access token", ex);
            }
        }
    }
}
