using System;
using System.Threading;
using System.Threading.Tasks;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using FluentAssertions;
using He.Identity.Mvc.Handlers;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Moq;
using Xunit;

namespace He.Identity.Mvc.UnitTests.Handlers
{
    /// <summary>
    /// CookieValidatePrincipalContextExtensions Tests.
    /// </summary>
    public class TokenRefreshHandlerTests
    {
        // An OIDC token with an expiry of 2021-03-19T15:37:03
        private const string IdToken = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCIsImtpZCI6IjVBSkxaTElzYXQ4ZW0xR0xDd1dCLSJ9.eyJodHRwOi8vaG9tZXNlbmdsYW5kLm9yZy51ay9jbGFpbXMvcm9sZXMiOltdLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJjaHJpc2plbnNlbnVrKzFAZ21haWwuY29tIiwibmlja25hbWUiOiJjaHJpc2plbnNlbnVrKzEiLCJuYW1lIjoiY2hyaXNqZW5zZW51aysxQGdtYWlsLmNvbSIsInBpY3R1cmUiOiJodHRwczovL3MuZ3JhdmF0YXIuY29tL2F2YXRhci9jYjk2YTAxZTg3NDg4NDY3NWQ0ODE0ZTYwYzRjNGNlYT9zPTQ4MCZyPXBnJmQ9aHR0cHMlM0ElMkYlMkZjZG4uYXV0aDAuY29tJTJGYXZhdGFycyUyRmNoLnBuZyIsInVwZGF0ZWRfYXQiOiIyMDIxLTAzLTE5VDE1OjM2OjAyLjcxOFoiLCJlbWFpbCI6ImNocmlzamVuc2VudWsrMUBnbWFpbC5jb20iLCJlbWFpbF92ZXJpZmllZCI6dHJ1ZSwiaXNzIjoiaHR0cHM6Ly9pZGVudGl0eS1kZXYuaG9tZXNlbmdsYW5kLm9yZy51ay8iLCJzdWIiOiJhdXRoMHw2MDBhOTQ3MThhMGNhZjAwNjk3YzIzMDUiLCJhdWQiOiJ0MWZ6ekcyc3NSeEtkZUY3QUhJdnNiVzNKdlZvM3VpUCIsImlhdCI6MTYxNjE2ODE2MywiZXhwIjoxNjE2MTY4MjIzLCJub25jZSI6IjYzNzUxNzY0OTU0MTEzMzE3Mi5NamxsWVRGaU9HTXRNVGxtTlMwMFpqVXlMV0ZrWlRjdE1EazBPRFF3TWpFMVlXSmlaVFZsWWpKaFl6UXRNalV4TXkwMFltUXhMV0V4TnprdE9HRXlZMkpoTlRnME5EbGsifQ.yl1VGMmMBGyBSfG-OaTX2ZXiCt2uXc3Jm-ahL8Vtp63voJHlRi_nSKrq_HzVYCnqfyfijYKErnoYODIMvghLCPpkA_A1-ODAhoi7F4gkcy3awKYJI4hvmIB2AFBu2-0nAUces6QCcQi695q1ocHq9NMI12SqF7YvH-bpDp4GOP4vGtsztAIbXx21vqu_a6w9yxQ27pVdB5kVBp1MeiSY0cRGXbivi-z87PWwEq7JFhr8CfDEGr2Amrd1w7AYbUDzRKeTFqp5GJYzLkL0W2We5-XSDfZihgNFPGDOkxXGNK7Nsjvb_orpUm0Fi_oFy5YkK0Y5SwQ7r31E8tkyc3XrWw";
        private const string IdTokenExpiryString = "2021-03-19T15:37:03";

        private readonly HeIdentityCookieConfiguration config = new HeIdentityCookieConfiguration
        {
            Audience = "the_audience",
            ClientId = "the_client_id",
            ClientSecret = "the_client_secret",
            RefreshBeforeExpirySeconds = 300,
        };

        /// <summary>
        /// Tokens should get refreshed.
        /// </summary>
        /// <param name="nowOffsetFromIdTokenExpiry">how many seconds from the IdToken should 'now' be.</param>
        /// <param name="accessTokensOffsetFromIdTokenExpiry">how many seconds from the IdToken should the access_token expiry be.</param>
        /// <param name="shoudlRefresh">Give the times should we be making  acall to refresh the tokens.</param>
        /// <returns>Task.</returns>
        /// <remarks>As the OIDC token contains the exp date and is encoded then it's simpler to specify 'now' and the access_token expiry date in terms of offsets from the OIDC token expiry date.</remarks>
        [Theory]
        [InlineData(-301, 0, false)] // idToken is not about to expire, Access Token is not about to expire. No need to refresh.
        [InlineData(-300, 1000, true)] // idToken is going to expire, Access Token is not about to expire. Need to refresh.
        [InlineData(-301, -1, true)] // idToken is not about to expire, Access Token is about to expire. Need to refresh.
        public async Task ShouldRefreshTokensIfTheyAreAboutToExpire(int nowOffsetFromIdTokenExpiry, int accessTokensOffsetFromIdTokenExpiry, bool shoudlRefresh)
        {
            var idTokenExpiry = DateTimeOffset.Parse(IdTokenExpiryString);

            var utcNow = idTokenExpiry.AddSeconds(nowOffsetFromIdTokenExpiry);

            var accessTokenExpiresAt = idTokenExpiry.AddSeconds(accessTokensOffsetFromIdTokenExpiry).ToString("o");

            var currentRefreshToken = "the_refresh_token";

            var context = new Mock<MockCookieValidatePrincipalContextFacade>(true, "/some_path", currentRefreshToken, IdToken, accessTokenExpiresAt).Object;

            RefreshTokenRequest actualRefreshTokenRequest = null;
            var (auth0client, accessTokenResponse) = this.GetAuthenticationApiClient((r, _) => actualRefreshTokenRequest = r);

            var logger = Mock.Of<ILogger<TokenRefreshHandler>>();

            var handler = new TokenRefreshHandler(this.config, auth0client, logger);
            await handler.EnsureTokenRefreshAsync(context, utcNow);

            // Assert
            if (shoudlRefresh)
            {
                actualRefreshTokenRequest.Audience.Should().Be(this.config.Audience);
                actualRefreshTokenRequest.ClientId.Should().Be(this.config.ClientId);
                actualRefreshTokenRequest.ClientSecret.Should().Be(this.config.ClientSecret);
                actualRefreshTokenRequest.RefreshToken.Should().Be(currentRefreshToken);

                var expectedExpiresAt = utcNow.AddSeconds(accessTokenResponse.ExpiresIn).ToString("o");

                context.GetToken("expires_at").Should().Be(expectedExpiresAt);
                context.GetToken(OpenIdConnectParameterNames.IdToken).Should().Be(accessTokenResponse.IdToken);
                context.GetToken(OpenIdConnectParameterNames.AccessToken).Should().Be(accessTokenResponse.AccessToken);
                context.GetToken(OpenIdConnectParameterNames.RefreshToken).Should().Be(accessTokenResponse.RefreshToken);

                Mock.Get(context).Verify(c => c.ShouldRenew(), Times.Once);
                Mock.Get(context).Verify(c => c.RejectPrincipal(), Times.Never);
            }
            else
            {
                actualRefreshTokenRequest.Should().BeNull();
                Mock.Get(auth0client).Verify(c => c.GetTokenAsync(It.IsAny<RefreshTokenRequest>(), It.IsAny<CancellationToken>()), Times.Never);
            }
        }

        /// <summary>
        /// If the tokens fail to refresh then the principal should be rejected.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task ShouldRejectPrincipalIfFailedToRefreshTokens()
        {
            var idTokenExpiry = DateTimeOffset.Parse(IdTokenExpiryString);
            var utcNow = idTokenExpiry.AddSeconds(-300);
            var accessTokenExpiresAt = idTokenExpiry.AddSeconds(-1).ToString("o");

            var context = new Mock<MockCookieValidatePrincipalContextFacade>(true, "/some_path", "the_refresh_token", IdToken, accessTokenExpiresAt).Object;

            var auth0client = Mock.Of<IAuthenticationApiClient>();
            Mock.Get(auth0client).Setup(c => c.GetTokenAsync(It.IsAny<RefreshTokenRequest>(), It.IsAny<CancellationToken>()))
                .Throws(new Exception("cannot refresh the token"));

            var logger = Mock.Of<ILogger<TokenRefreshHandler>>();

            var handler = new TokenRefreshHandler(this.config, auth0client, logger);

            await handler.EnsureTokenRefreshAsync(context, utcNow);

            Mock.Get(context).Verify(c => c.RejectPrincipal(), Times.Once);
        }

        /// <summary>
        /// If the user is not logged in then we shouldn't attempt to refresh the tokens.
        /// </summary>
        /// <param name="userIsAuthenticated">Is the user authenticated.</param>
        /// <param name="requestPathValue">The path of the endpoint being requested.</param>
        /// <returns>Task.</returns>
        [Theory]
        [InlineData(false, "/some_path")] // User is not logged in so don't attempt to refresh the tokens.
        [InlineData(true, HeIdentityPaths.CheckSession)]// We are checking the session so don't try to refresh the tokens.
        public async Task ShouldNotAttemptToRefreshTokenIfNotNeeded(bool userIsAuthenticated, string requestPathValue)
        {
            var idTokenExpiry = DateTimeOffset.Parse(IdTokenExpiryString);
            var utcNow = idTokenExpiry.AddSeconds(-300);
            var accessTokenExpiresAt = idTokenExpiry.AddSeconds(-1).ToString("o");

            var context = new Mock<MockCookieValidatePrincipalContextFacade>(userIsAuthenticated, requestPathValue, "the_refresh_token", IdToken, accessTokenExpiresAt).Object;

            RefreshTokenRequest actualRefreshTokenRequest = null;
            var (auth0client, accessTokenResponse) = this.GetAuthenticationApiClient((r, _) => actualRefreshTokenRequest = r);

            var logger = Mock.Of<ILogger<TokenRefreshHandler>>();

            var handler = new TokenRefreshHandler(this.config, auth0client, logger);

            await handler.EnsureTokenRefreshAsync(context, utcNow);

            actualRefreshTokenRequest.Should().BeNull();
            Mock.Get(auth0client).Verify(c => c.GetTokenAsync(It.IsAny<RefreshTokenRequest>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        private (IAuthenticationApiClient, AccessTokenResponse) GetAuthenticationApiClient(Action<RefreshTokenRequest, CancellationToken> refreshTokenCallback)
        {
            var accessTokenResponse = new AccessTokenResponse
            {
                ExpiresIn = 1200,
                IdToken = "new_id_token",
                AccessToken = "new_access_token",
                RefreshToken = "new_refresh_token",
            };

            var auth0client = Mock.Of<IAuthenticationApiClient>();
            Mock.Get(auth0client).Setup(c => c.GetTokenAsync(It.IsAny<RefreshTokenRequest>(), It.IsAny<CancellationToken>()))
                .Callback(refreshTokenCallback)
                .Returns(Task.FromResult(accessTokenResponse));

            return (auth0client, accessTokenResponse);
        }
    }
}
