using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Auth0.AuthenticationApi;
using Auth0.AuthenticationApi.Models;
using FluentAssertions;
using He.Identity.Mvc.ClientCredentials;
using Moq;
using Moq.Protected;
using Xunit;

namespace He.Identity.Mvc.UnitTests.ClientCredentials
{
    /// <summary>
    /// ClientCredentialHelper Tests.
    /// </summary>
    public class ClientCredentialHelperTests
    {
        /// <summary>
        /// GetAccessTokenAsync should create a request expected by Auth0.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task GetAccessTokenAsync_ShouldCreateRequestInExpectedFormatForAuth0()
        {
            var auth0Client = Mock.Of<IAuthenticationApiClient>();

            var config = new HeIdentityCookieConfiguration();

            var auth0AccessTokenResponse = new AccessTokenResponse
            {
                AccessToken = "auth0 access token",
                ExpiresIn = 1234,
                TokenType = "token type",
            };

            Mock.Get(auth0Client).Setup(c => c.GetTokenAsync(
                It.Is<ClientCredentialsTokenRequest>(
                c => c.Audience == "audience" &&
                c.ClientId == "client id" &&
                c.ClientSecret == "client secret"), default)).Returns(Task.FromResult(auth0AccessTokenResponse));

            var helper = new ClientCredentialHelper(auth0Client, config);

            var actualResponse = await helper.GetAccessTokenAsync("client id", "client secret", "audience");

            actualResponse.AccessToken.Should().Be(auth0AccessTokenResponse.AccessToken);
            actualResponse.ExpiresIn.Should().Be(auth0AccessTokenResponse.ExpiresIn);
            actualResponse.TokenType.Should().Be(auth0AccessTokenResponse.TokenType);
        }

        /// <summary>
        /// GetAccessTokenAsync should create a request expected by Auth0.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task GetAccessTokenAsync_WhenError_ExpectHeIdentityException()
        {
            var auth0Client = Mock.Of<IAuthenticationApiClient>();

            var config = new HeIdentityCookieConfiguration();

            var innerException = new ArgumentException("mock exception");
            Mock.Get(auth0Client).Setup(c => c.GetTokenAsync(It.IsAny<ClientCredentialsTokenRequest>(), default)).Throws(innerException);

            var helper = new ClientCredentialHelper(auth0Client, config);

            Func<Task> act = async () => await helper.GetAccessTokenAsync("client id", "client secret", "audience").ConfigureAwait(false);

            await act.Should().ThrowAsync<HeIdentityException>()
                .WithMessage("Error obtaining access token")
                .Where(ex => ex.InnerException.Message == "mock exception");
        }

        /// <summary>
        /// GetAccessTokenAsync should create a request expected by Auth0.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task GetAccessTokenAsync_ShouldCreateRequestUsingConfigInExpectedFormatForAuth0()
        {
            var auth0Client = Mock.Of<IAuthenticationApiClient>();

            var config = new HeIdentityCookieConfiguration
            {
                ClientId = "client id from config",
                ClientSecret = "client secret from config",
            };

            var auth0AccessTokenResponse = new AccessTokenResponse
            {
                AccessToken = "auth0 access token",
                ExpiresIn = 1234,
                TokenType = "token type",
            };

            Mock.Get(auth0Client).Setup(c => c.GetTokenAsync(
                It.Is<ClientCredentialsTokenRequest>(
                c => c.Audience == "audience" &&
                c.ClientId == "client id from config" &&
                c.ClientSecret == "client secret from config"), default)).Returns(Task.FromResult(auth0AccessTokenResponse));

            var helper = new ClientCredentialHelper(auth0Client, config);

            var actualResponse = await helper.GetAccessTokenAsync("audience");

            actualResponse.AccessToken.Should().Be(auth0AccessTokenResponse.AccessToken);
            actualResponse.ExpiresIn.Should().Be(auth0AccessTokenResponse.ExpiresIn);
            actualResponse.TokenType.Should().Be(auth0AccessTokenResponse.TokenType);
        }

        /// <summary>
        /// GetAccessTokenAsync should create a request expected by Auth0.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task GetAccessTokenAsync_WhenErrorUsingConfig_ExpectHeIdentityException()
        {
            var auth0Client = Mock.Of<IAuthenticationApiClient>();

            var config = new HeIdentityCookieConfiguration
            {
                ClientId = "client id from config",
                ClientSecret = "client secret from config",
            };

            var innerException = new ArgumentException("mock exception");
            Mock.Get(auth0Client).Setup(c => c.GetTokenAsync(It.IsAny<ClientCredentialsTokenRequest>(), default)).Throws(innerException);

            var helper = new ClientCredentialHelper(auth0Client, config);

            Func<Task> act = async () => await helper.GetAccessTokenAsync("audience").ConfigureAwait(false);

            await act.Should().ThrowAsync<HeIdentityException>()
                .WithMessage("Error obtaining access token")
                .Where(ex => ex.InnerException.Message == "mock exception");
        }
    }
}
