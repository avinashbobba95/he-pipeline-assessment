using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using He.Identity.Mvc.Extensions;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Xunit;

namespace He.Identity.Mvc.UnitTests.Handlers
{
    /// <summary>
    /// HttpContextExtensions Tests.
    /// </summary>
    public class RevokeRefreshTokenHandlerTests
    {
        /// <summary>
        /// RevokeRefreshTokenAsync should create a valid url.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task RevokeRefreshTokenAsync_WhenHaveRefreshToken_ShouldPostToRevokeEndpoint()
        {
            var refreshToken = "refresh_token_to_revoke";
            var config = new HeIdentityCookieConfiguration
            {
                ClientId = "the_client_id",
                ClientSecret = "the_client_secret",
                Domain = "the_domain",
            };

            HttpRequestMessage actualRequestMessage = null;

            var httpClient = this.GetHttpClient((request) => actualRequestMessage = request);

            var openIdConnectOptions = this.GetOpenIdConnectOptions(httpClient);

            var logger = Mock.Of<ILogger<RevokeRefreshTokenHandler>>();

            var handler = new RevokeRefreshTokenHandler(config, openIdConnectOptions, logger);
            await handler.RevokeRefreshTokenAsync(refreshToken);

            actualRequestMessage.Method.Should().Be(HttpMethod.Post);
            actualRequestMessage.RequestUri.ToString().Should().Be("https://the_domain/oauth/revoke");

            var actualFormData = await ((FormUrlEncodedContent)actualRequestMessage.Content).ReadAsStringAsync();
            actualFormData.Should().Be("client_id=the_client_id&client_secret=the_client_secret&token=refresh_token_to_revoke");
        }

        /// <summary>
        /// RevokeRefreshTokenAsync should create a valid url.
        /// </summary>
        /// <returns>Task.</returns>
        [Fact]
        public async Task RevokeRefreshTokenAsync_WhenNoRefreshToken_ShouldNotPostToRevokeEndpoint()
        {
            // no refresh token
            var refreshToken = string.Empty;

            var config = new HeIdentityCookieConfiguration
            {
                ClientId = "the_client_id",
                ClientSecret = "the_client_secret",
                Domain = "the_domain",
            };

            HttpRequestMessage actualRequestMessage = null;

            var httpClient = this.GetHttpClient((request) => actualRequestMessage = request);

            var openIdConnectOptions = this.GetOpenIdConnectOptions(httpClient);

            var logger = Mock.Of<ILogger<RevokeRefreshTokenHandler>>();

            var handler = new RevokeRefreshTokenHandler(config, openIdConnectOptions, logger);

            await handler.RevokeRefreshTokenAsync(refreshToken);

            // a request message should not have been created
            actualRequestMessage.Should().BeNull();
        }

        private HttpClient GetHttpClient(Action<HttpRequestMessage> actualRequestMessageCallback)
        {
            var httpMessageHandler = Mock.Of<HttpMessageHandler>(MockBehavior.Strict);
            Mock.Get(httpMessageHandler)
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .Callback((HttpRequestMessage request, CancellationToken cancellationToken) => actualRequestMessageCallback(request))
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = HttpStatusCode.OK,
               });

            var httpClient = new HttpClient(httpMessageHandler);

            return httpClient;
        }

        private IOptions<OpenIdConnectOptions> GetOpenIdConnectOptions(HttpClient backchannel)
        {
            var openIdConnectOptions = Mock.Of<IOptions<OpenIdConnectOptions>>();
            Mock.Get(openIdConnectOptions)
                .Setup(c => c.Value)
                .Returns(new OpenIdConnectOptions
                {
                    Backchannel = backchannel,
                });

            return openIdConnectOptions;
        }
    }
}
