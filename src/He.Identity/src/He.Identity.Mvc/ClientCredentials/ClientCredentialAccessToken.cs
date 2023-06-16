namespace He.Identity.Mvc.ClientCredentials
{
    /// <summary>
    /// OAuthTokenResponse.
    /// </summary>
    public class ClientCredentialAccessToken
    {
        /// <summary>
        /// Gets or sets access_token.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets expires_in.
        /// </summary>
        public int ExpiresIn { get; set; }

        /// <summary>
        /// Gets or sets token_type.
        /// </summary>
        public string TokenType { get; set; }
    }
}
