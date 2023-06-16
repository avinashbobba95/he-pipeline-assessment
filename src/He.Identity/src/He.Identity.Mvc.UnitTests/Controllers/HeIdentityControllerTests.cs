using System.Threading.Tasks;
using He.Identity.Mvc.Controllers;
using He.Identity.Mvc.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace He.Identity.Mvc.UnitTests.Controllers
{
    /// <summary>
    /// HeIdentityController Unit Tests.
    /// </summary>
    public class HeIdentityControllerTests
    {
        /// <summary>
        /// Ensure VerifyEmail returns view model.
        /// </summary>
        [Fact]
        public void VerifyEmailTest()
        {
            var httpContext = Mock.Of<HttpContext>();
            var config = Mock.Of<HeIdentityConfiguration>();
            var managementService = Mock.Of<IIdentityManagementService>();

            Mock.Get(httpContext).SetupGet(c => c.Request.Cookies["emailToVerify"]).Returns("email.to@verify.com");

            var controller = new HeIdentityController(config, managementService);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext,
            };

            var result = controller.VerifyEmail();

            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<VerifyEmailViewModel>(viewResult.ViewData.Model);

            Assert.Equal("email.to@verify.com", model.EmailtoVerify);
        }

        /// <summary>
        /// Verifies email is not present redirects Home.
        /// </summary>
        [Fact]
        public void VerifyEmailNotPresentRedirectsHomeTest()
        {
            var httpContext = Mock.Of<HttpContext>();
            var config = Mock.Of<HeIdentityConfiguration>();
            var managementService = Mock.Of<IIdentityManagementService>();

            Mock.Get(httpContext).SetupGet(c => c.Request.Cookies).Returns((IRequestCookieCollection)null);

            var controller = new HeIdentityController(config, managementService);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext,
            };

            var result = controller.VerifyEmail();

            Assert.Equal("/", ((RedirectResult)result).Url);
        }
    }
}
