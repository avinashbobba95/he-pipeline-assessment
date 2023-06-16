using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using Auth0.Core.Exceptions;
using Auth0.ManagementApi;
using Polly;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace He.Identity.Auth0
{
    public class Auth0Client : IAuth0Client
    {
        private readonly IAsyncPolicy policy;
        private readonly string domain;
        private readonly IAuthenticationApiClient authenticationClient;
        private readonly IBearerTokenClient tokenClient;
        private readonly IManagementConnection managementConnection;

        public string UserConnection { get; }

        public Auth0Client(HttpClient http, Auth0ManagementConfig managementConfig)
            : this(http, GetDefaultPolicy(), managementConfig)
        {
        }

        public Auth0Client(HttpClient http, IAsyncPolicy policy, Auth0ManagementConfig managementConfig)
        {
            this.policy = policy;
            this.domain = managementConfig.Domain;
            this.authenticationClient = new AuthenticationApiClient(managementConfig.Domain, new HttpClientAuthenticationConnection(http));
            this.tokenClient = new BearerTokenClient(async () =>
            {
                var token = await authenticationClient.GetTokenAsync(new ClientCredentialsTokenRequest
                {
                    ClientId = managementConfig.ClientId,
                    ClientSecret = managementConfig.ClientSecret,
                    Audience = managementConfig.Audience
                });

                return new BearerToken(token.IdToken, token.AccessToken, token.RefreshToken, token.ExpiresIn);
            });
            this.managementConnection = new HttpClientManagementConnection(http);
            this.UserConnection = managementConfig.UserConnection;
        }

        public Task Authentication(Func<IAuthenticationApiClient, Task> action)
        {
            return policy.ExecuteAsync(() => action(authenticationClient));
        }

        public Task<T> Authentication<T>(Func<IAuthenticationApiClient, Task<T>> action)
        {
            return policy.ExecuteAsync(() => action(authenticationClient));
        }

        public Task Management(Func<ManagementApiClient, Task> action)
        {
            return policy.ExecuteAsync(async () => await action(await GetManagementClient()));
        }

        public Task<T> Management<T>(Func<ManagementApiClient, Task<T>> action)
        {
            return policy.ExecuteAsync(async () => await action(await GetManagementClient()));
        }

        private async Task<ManagementApiClient> GetManagementClient()
        {
            var token = await tokenClient.GetToken();
            return new ManagementApiClient(token.AccessToken, domain, managementConnection);
        }

        private static AsyncPolicy GetDefaultPolicy()
        {
            return Policy
                .Handle<RateLimitApiException>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(1));
        }
    }
}
