using System.Collections.Generic;
using He.Identity.Mvc.Handlers;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace He.Identity.Mvc.UnitTests.Handlers
{
    /// <summary>
    /// MockCookieValidatePrincipalContextFacade.
    /// </summary>
    public class MockCookieValidatePrincipalContextFacade : ICookieValidatePrincipalContextFacade
    {
        private Dictionary<string, string> tokens;

        /// <summary>
        /// Initializes a new instance of the <see cref="MockCookieValidatePrincipalContextFacade"/> class.
        /// </summary>
        /// <param name="userIsAuthenticated">userIsAuthenticated.</param>
        /// <param name="requestPathValue">RrequestPathValue.</param>
        /// <param name="refreshToken">refreshToken.</param>
        /// <param name="idToken">idToken.</param>
        /// <param name="accessTokenExpiresAt">expiresAt.</param>
        public MockCookieValidatePrincipalContextFacade(bool userIsAuthenticated, string requestPathValue, string refreshToken, string idToken, string accessTokenExpiresAt)
        {
            this.UserIsAuthenticated = userIsAuthenticated;
            this.RequestPathValue = requestPathValue;

            this.tokens = new Dictionary<string, string>
            {
                { OpenIdConnectParameterNames.RefreshToken, refreshToken },
                { OpenIdConnectParameterNames.IdToken, idToken },
                { OpenIdConnectParameterNames.AccessToken, null },
                { "expires_at", accessTokenExpiresAt },
            };
        }

        /// <inheritdoc/>
        public bool UserIsAuthenticated { get; set; }

        /// <inheritdoc/>
        public string RequestPathValue { get; set; }

        /// <inheritdoc/>
        public string GetToken(string tokenName)
        {
            return this.tokens[tokenName];
        }

        /// <inheritdoc/>
        public void UpdateToken(string tokenName, string tokenValue)
        {
            this.tokens[tokenName] = tokenValue;
        }

        /// <inheritdoc/>
        public virtual void RejectPrincipal()
        {
        }

        /// <inheritdoc/>
        public virtual void ShouldRenew()
        {
        }
    }
}
