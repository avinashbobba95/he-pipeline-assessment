using He.Identity.Mvc.Controllers;

namespace He.Identity.Mvc
{
    /// <summary>
    /// Configuration for Homes England Identity service.
    /// </summary>
    public class HeIdentityCookieConfiguration : HeIdentityConfiguration
    {
        /// <summary>
        /// Gets or sets the Client Id.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the Client Secret.
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets the Manage Account Url.
        /// </summary>
        public string ManageAccountUrl { get; set; }

        /// <summary>
        /// Gets or sets how long before a session expires should we attempt to refresh the access tokens and the OIDC token.
        /// </summary>
        /// <remarks>
        /// 300 seconds by default (5 minutes).
        /// </remarks>
        public int RefreshBeforeExpirySeconds { get; set; } = 300;
    }
}
