namespace He.Identity.Mvc.Handlers
{
    /// <summary>
    /// ICookieValidatePrincipalContextFacade.
    /// </summary>
    public interface ICookieValidatePrincipalContextFacade
    {
        /// <summary>
        /// Gets a value indicating whether the user is authenticated.
        /// </summary>
        bool UserIsAuthenticated { get; }

        /// <summary>
        /// Gets the request path.
        /// </summary>
        string RequestPathValue { get; }

        /// <summary>
        /// Returns a token.
        /// </summary>
        /// <param name="tokenName">The name of the token to retreive.</param>
        /// <returns>The value of the token.</returns>
        string GetToken(string tokenName);

        /// <summary>
        /// Update a token.
        /// </summary>
        /// <param name="tokenName">The name of the token to update.</param>
        /// <param name="tokenValue">The value of the token.</param>
        void UpdateToken(string tokenName, string tokenValue);

        /// <summary>
        /// Renew the cookie.
        /// </summary>
        void ShouldRenew();

        /// <summary>
        /// Reject the incomming principal.
        /// </summary>
        void RejectPrincipal();
    }
}