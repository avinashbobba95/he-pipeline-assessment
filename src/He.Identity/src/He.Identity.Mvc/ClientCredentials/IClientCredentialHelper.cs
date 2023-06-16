using System.Threading.Tasks;

namespace He.Identity.Mvc.ClientCredentials
{
    /// <summary>
    /// ClientCredentialHelper.
    /// </summary>
    public interface IClientCredentialHelper
    {
        /// <summary>
        /// GetAccessToken. Uses the Client Id and Client Secret from HeIdentityCookieConfiguration.
        /// </summary>
        /// <param name="audience">audience.</param>
        /// <returns>Task.</returns>
        Task<ClientCredentialAccessToken> GetAccessTokenAsync(string audience);

        /// <summary>
        /// GetAccessToken.
        /// </summary>
        /// <param name="clientId">clientId.</param>
        /// <param name="clientSecret">clientSecret.</param>
        /// <param name="audience">audience.</param>
        /// <returns>Task.</returns>
        Task<ClientCredentialAccessToken> GetAccessTokenAsync(string clientId, string clientSecret, string audience);
    }
}