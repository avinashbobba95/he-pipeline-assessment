using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace He.Identity.Mvc.Handlers
{
    /// <summary>
    /// A wrapper around CookieValidatePrincipalContext.
    /// </summary>
    public class CookieValidatePrincipalContextFacade : ICookieValidatePrincipalContextFacade
    {
        private readonly CookieValidatePrincipalContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="CookieValidatePrincipalContextFacade"/> class.
        /// </summary>
        /// <param name="context">context.</param>
        public CookieValidatePrincipalContextFacade(CookieValidatePrincipalContext context)
        {
            this.context = context;
        }

        /// <inheritdoc/>
        public bool UserIsAuthenticated => this.context.Principal.Identity.IsAuthenticated;

        /// <inheritdoc/>
        public string RequestPathValue => this.context.Request.Path.Value;

        /// <inheritdoc/>
        public string GetToken(string tokenName) => this.context.Properties.GetTokenValue(tokenName);

        /// <inheritdoc/>
        public void UpdateToken(string tokenName, string tokenValue) => this.context.Properties.UpdateTokenValue(tokenName, tokenValue);

        /// <inheritdoc/>
        public void ShouldRenew()
        {
            this.context.ShouldRenew = true;
        }

        /// <inheritdoc/>
        public void RejectPrincipal()
        {
            this.context.RejectPrincipal();
        }
    }
}
