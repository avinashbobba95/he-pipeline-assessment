using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace He.Identity.Mvc.UnitTests
{
    /// <summary>
    /// HeIdentityExtensions Tests.
    /// </summary>
    public class HeIdentityExtensionsTests
    {
        /// <summary>
        /// Ensure configuration is specified.
        /// </summary>
        /// <param name="domain">The domain.</param>
        /// <param name="audience">Thre audience.</param>
        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData("specified", "")]
        [InlineData("", "specified")]
        public void AddHeIdentityJwtBearerAuth_MissingConfiguration_ExpectException(string domain, string audience)
        {
            var config = new HeIdentityJwtConfiguration
            {
                Domain = domain,
                Audience = audience,
            };

            var serviceCollection = Mock.Of<IServiceCollection>();

            Assert.Throws<ArgumentException>(() => HeIdentityExtensions.AddHeIdentityJwtBearerAuth(serviceCollection, config));
        }

        /// <summary>
        /// Ensure configuration is specified.
        /// </summary>
        /// <param name="domain">The domain.</param>
        /// <param name="clientId">The clientId.</param>
        /// <param name="clientSecret">The clientSecret.</param>
        [Theory]
        [InlineData(null, null, null)]
        [InlineData("", "", null)]
        [InlineData("specified", "", "")]
        [InlineData("", "specified", "")]
        [InlineData("", "", "specified")]
        public void AddHeIdentityCookieAuth_MissingConfiguration_ExpectException(string domain, string clientId, string clientSecret)
        {
            var config = new HeIdentityCookieConfiguration
            {
                Domain = domain,
                ClientId = clientId,
                ClientSecret = clientSecret,
            };

            var mvcBuilder = Mock.Of<IMvcBuilder>();
            var env = Mock.Of<IWebHostEnvironment>();

            Assert.Throws<ArgumentException>(() => HeIdentityExtensions.AddHeIdentityCookieAuth(mvcBuilder, config, env));
        }
    }
}
