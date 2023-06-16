using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using He.Identity.Mvc.Controllers;
using He.Identity.Mvc.TagHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using Xunit;

namespace He.Identity.Mvc.UnitTests.TagHelpers
{
    /// <summary>
    /// HeIdentitySigninTagHelper Unit Tests.
    /// </summary>
    public class HeIdentitySigninTagHelperTests
    {
        /// <summary>
        /// Process method. When a user is signed in then show a sign out link and the manage account link.
        /// </summary>
        [Fact]
        public void Process_WhenUserIsSignedIn_ExpectSignoutLink_and_ManageAccountLink()
        {
            var (tagHelper, context, output) = this.GetSut(userIsAuthenticated: true, currentControllerRouteName: "AnyApplicationController", manageAccountUrl: "the_manage_account_url");

            tagHelper.Process(context, output);

            string actual = string.Empty;
            using (var sw = new StringWriter())
            {
                output.WriteTo(sw, HtmlEncoder.Default);
                actual = sw.ToString().Trim();
            }

            var expected = @"<li class=""govuk-header__navigation-item nowrap""><a href=""/signout"" class=""govuk-header__link"" title=""Sign out"">Sign out</a></li><li class=""govuk-header__navigation-item nowrap""><a href=""the_manage_account_url?signOutUrl=%2Fsignout&returnUrl=%3A%2F%2F"" class=""govuk-header__link"" title=""Manage account"">Manage account</a></li>";

            Assert.Equal(expected, actual);
        }

        /// <summary>
        /// Process method. When a user is signed in and the manageAccountUrl is not configured then show a sign out link and NOT the manage account link.
        /// </summary>
        [Fact]
        public void Process_WhenUserIsSignedInAndManageAccountUrlNotConfigured_ExpectSignoutLink_and_ManageAccountLink()
        {
            var (tagHelper, context, output) = this.GetSut(userIsAuthenticated: true, currentControllerRouteName: "AnyApplicationController", manageAccountUrl: null);

            tagHelper.Process(context, output);

            string actual = string.Empty;
            using (var sw = new StringWriter())
            {
                output.WriteTo(sw, HtmlEncoder.Default);
                actual = sw.ToString().Trim();
            }

            var expected = @"<li class=""govuk-header__navigation-item nowrap""><a href=""/signout"" class=""govuk-header__link"" title=""Sign out"">Sign out</a></li>";

            Assert.Equal(expected, actual);
        }

        /// <summary>
        /// Process method. When a user is signed out then show a signin link.
        /// </summary>
        [Fact]
        public void Process_WhenUserIsSignedOut_ExpectSigninLink()
        {
            var (tagHelper, context, output) = this.GetSut(userIsAuthenticated: false, currentControllerRouteName: "AnyApplicationController", manageAccountUrl: "the_manage_account_url");

            tagHelper.Process(context, output);

            string actual = string.Empty;
            using (var sw = new StringWriter())
            {
                output.WriteTo(sw, HtmlEncoder.Default);
                actual = sw.ToString();
            }

            var expected = @"<li class=""govuk-header__navigation-item nowrap""><a href=""/signin"" class=""govuk-header__link"" title=""Sign in"">Sign in</a></li>";

            Assert.Equal(expected, actual);
        }

        /// <summary>
        /// Process method. When the controller is HeIdentityController then don't show a link.
        /// </summary>
        [Fact]
        public void Process_WhenHeIdentityController_ExpectNoLink()
        {
            var (tagHelper, context, output) = this.GetSut(userIsAuthenticated: false, currentControllerRouteName: nameof(HeIdentityController).Replace("Controller", string.Empty), manageAccountUrl: "the_manage_account_url");

            tagHelper.Process(context, output);

            string actual = string.Empty;
            using (var sw = new StringWriter())
            {
                output.WriteTo(sw, HtmlEncoder.Default);
                actual = sw.ToString();
            }

            var expected = string.Empty;

            Assert.Equal(expected, actual);
        }

        private (HeIdentitySigninTagHelper, TagHelperContext, TagHelperOutput) GetSut(bool userIsAuthenticated, string currentControllerRouteName, string manageAccountUrl)
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.RouteValues.Add("controller", currentControllerRouteName);

            // This will set Identity.IsAuthenticated to either true or false;
            var identity = userIsAuthenticated ? new GenericIdentity("is authenticated", string.Empty) : Mock.Of<IIdentity>();

            httpContext.User = new ClaimsPrincipal(identity);

            var urlHelper = Mock.Of<IUrlHelper>();

            Mock.Get(urlHelper).Setup(h => h.Action(It.Is<UrlActionContext>(c =>
                c.Action == nameof(HeIdentityController.SignIn) &&
                c.Controller == "HeIdentity")))
            .Returns("/signin");

            Mock.Get(urlHelper).Setup(h => h.Action(It.Is<UrlActionContext>(c =>
                c.Action == nameof(HeIdentityController.SignOut) &&
                c.Controller == "HeIdentity")))
            .Returns("/signout");

            var viewContext = new ViewContext();

            viewContext.HttpContext = httpContext;

            var urlHelperFactory = Mock.Of<IUrlHelperFactory>();
            Mock.Get(urlHelperFactory).Setup(f => f.GetUrlHelper(viewContext)).Returns(urlHelper);

            var httpContextAccessor = Mock.Of<IHttpContextAccessor>();
            Mock.Get(httpContextAccessor).Setup(a => a.HttpContext).Returns(httpContext);

            var config = new HeIdentityCookieConfiguration
            {
                ClientId = "the_client_id",
                ClientSecret = "the_client_secret",
                Domain = "the_domain",
                ManageAccountUrl = manageAccountUrl,
            };

            var tagHelper = new HeIdentitySigninTagHelper(httpContextAccessor, urlHelperFactory, config);

            var context = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                Guid.NewGuid().ToString("N"));

            var output = new TagHelperOutput(
                "he-identity-signin",
                new TagHelperAttributeList(),
                (result, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    tagHelperContent.SetHtmlContent(string.Empty);
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            tagHelper.ViewContext = viewContext;

            return (tagHelper, context, output);
        }
    }
}
