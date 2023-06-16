using System;
using System.Collections.Generic;
using He.Identity.Mvc.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.WebUtilities;

namespace He.Identity.Mvc.TagHelpers
{
    /// <summary>
    /// Creates a Singin/Signout link.
    /// </summary>
    public class HeIdentitySigninTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUrlHelperFactory urlHelperFactory;
        private readonly HeIdentityCookieConfiguration heIdentityConfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeIdentitySigninTagHelper"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">httpContextAccessor.</param>
        /// <param name="urlHelperFactory">urlHelperFactory.</param>
        /// <param name="heIdentityConfig">heIdentityConfig.</param>
        public HeIdentitySigninTagHelper(IHttpContextAccessor httpContextAccessor, IUrlHelperFactory urlHelperFactory, HeIdentityCookieConfiguration heIdentityConfig)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.urlHelperFactory = urlHelperFactory;
            this.heIdentityConfig = heIdentityConfig;
        }

        /// <summary>
        /// Gets or sets the ViewContext.
        /// </summary>
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        /// <summary>
        /// Gets Request.
        /// </summary>
        protected HttpRequest Request => this.ViewContext.HttpContext.Request;

        /// <summary>
        /// Process.
        /// </summary>
        /// <param name="context">context.</param>
        /// <param name="output">output.</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var httpContext = this.httpContextAccessor.HttpContext;
            var currentController = httpContext.Request.RouteValues["controller"].ToString();
            var heIdentityController = nameof(HeIdentityController).Replace("Controller", string.Empty);

            output.TagName = null;
            output.Attributes.Clear();

            if (currentController == heIdentityController)
            {
                return;
            }

            var urlHelper = this.urlHelperFactory.GetUrlHelper(this.ViewContext);

            if (httpContext.User.Identity.IsAuthenticated)
            {
                var signOutUrl = urlHelper.Action(nameof(HeIdentityController.SignOut), heIdentityController);
                output.Content.AppendHtml($@"<li class=""govuk-header__navigation-item nowrap""><a href=""{signOutUrl}"" class=""govuk-header__link"" title=""Sign out"">Sign out</a></li>");

                this.AppendManageAccountHtml(output);
            }
            else
            {
                var signInUrl = urlHelper.Action(nameof(HeIdentityController.SignIn), heIdentityController);
                output.Content.AppendHtml($@"<li class=""govuk-header__navigation-item nowrap""><a href=""{signInUrl}"" class=""govuk-header__link"" title=""Sign in"">Sign in</a></li>");
            }
        }

        private void AppendManageAccountHtml(TagHelperOutput output)
        {
            if (string.IsNullOrWhiteSpace(this.heIdentityConfig.ManageAccountUrl))
            {
                return;
            }

            var signOutBuilder = new UriBuilder(this.Request.Scheme, this.Request.Host.Host);
            if (this.Request.Host.Port.HasValue)
            {
                signOutBuilder.Port = this.Request.Host.Port.Value;
            }

            signOutBuilder.Path = HeIdentityPaths.SignOut;

            var queryParams = new Dictionary<string, string>()
            {
                { "signOutUrl", signOutBuilder.ToString() },
                { "returnUrl", this.Request.GetDisplayUrl() },
            };

            var manageAccountUrl = QueryHelpers.AddQueryString(this.heIdentityConfig.ManageAccountUrl, queryParams);

            var html = $@"<li class=""govuk-header__navigation-item nowrap""><a href=""{manageAccountUrl}"" class=""govuk-header__link"" title=""Manage account"">Manage account</a></li>";

            output.Content.AppendHtml(html);
        }
    }
}
