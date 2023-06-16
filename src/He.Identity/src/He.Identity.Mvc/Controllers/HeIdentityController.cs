using System;
using System.Threading.Tasks;
using He.Identity.Mvc.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace He.Identity.Mvc.Controllers
{
    /// <summary>
    /// Controllers to support He Identity.
    /// </summary>
    [AllowAnonymous]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class HeIdentityController : Controller
    {
        private readonly HeIdentityConfiguration config;
        private readonly IIdentityManagementService identityManagementService;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeIdentityController"/> class.
        /// </summary>
        /// <param name="config">The identity configuration.</param>
        /// <param name="identityManagementService">The identity management service.</param>
        public HeIdentityController(HeIdentityConfiguration config, IIdentityManagementService identityManagementService)
        {
            this.config = config;
            this.identityManagementService = identityManagementService;
        }

        /// <summary>
        /// The Verify Email page.
        /// </summary>
        /// <param name="attempt">The attempt.</param>
        /// <returns>The Verify Email view.</returns>
        [HttpGet(HeIdentityPaths.VerifyEmail)]
        public IActionResult VerifyEmail(int attempt = 1)
        {
            if ((this.HttpContext.Request.Cookies == null) || string.IsNullOrEmpty(this.HttpContext.Request.Cookies["emailToVerify"]))
            {
                return this.Redirect("/");
            }

            var vm = new VerifyEmailViewModel
            {
                EmailtoVerify = this.HttpContext.Request.Cookies["emailToVerify"],
                Attempt = attempt,
                SupportEmail = this.config.SupportEmail ?? "servicedesk@homesengland.gov.uk",
            };

            return this.View(vm);
        }

        /// <summary>
        /// Signs the user in.
        /// </summary>
        /// <param name="redirectUri">The url to return the user to.</param>
        /// <returns>returnUrl to the identity service login page.</returns>
        [HttpGet(HeIdentityPaths.SignIn)]
        public Task SignIn(string redirectUri = "/")
        {
            var authenticationProperties = new AuthenticationProperties { RedirectUri = redirectUri };
            authenticationProperties.Items.Add("screen_hint", "login");
            authenticationProperties.Items.Add("prompt", "signup");
            return this.HttpContext.ChallengeAsync(HeIdentityConstants.AuthenticationScheme, authenticationProperties);
        }

        /// <summary>
        /// Redirects the user to the signup page.
        /// </summary>
        /// <param name="redirectUri">The url to return the user to.</param>
        /// <returns>Redirection to the identity service signup page.</returns>
        [HttpGet(HeIdentityPaths.SignUp)]
        public Task SignUp(string redirectUri = "/")
        {
            var authenticationProperties = new AuthenticationProperties { RedirectUri = redirectUri };
            authenticationProperties.Items.Add("screen_hint", "signup");
            authenticationProperties.Items.Add("prompt", "signup");
            return this.HttpContext.ChallengeAsync(HeIdentityConstants.AuthenticationScheme, authenticationProperties);
        }

        /// <summary>
        /// Logs the user out.
        /// </summary>
        /// <param name="redirectUri">The url to return the user to.</param>
        /// <returns>Task.</returns>
        [HttpGet(HeIdentityPaths.SignOut)]
        public async Task SignOut(string redirectUri = "/")
        {
            await this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var authenticationProperties = new AuthenticationProperties { RedirectUri = redirectUri };
            await this.HttpContext.SignOutAsync(HeIdentityConstants.AuthenticationScheme, authenticationProperties);
        }

        /// <summary>
        /// Checks user session.
        /// </summary>
        /// <returns>Task.</returns>
        [HttpGet(HeIdentityPaths.CheckSession)]
        public async Task<IActionResult> CheckSession()
        {
            IActionResult result = this.Ok("true");

            try
            {
                var idTokenString = await this.HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);

                if (!string.IsNullOrEmpty(idTokenString))
                {
                    var tokenHandler = new JsonWebTokenHandler();
                    var idToken = tokenHandler.ReadJsonWebToken(idTokenString);
                    var idTokenExpiry = long.Parse(idToken.GetClaim("exp").Value);
                    var idTokenExpiryFromEpoch = DateTimeOffset.FromUnixTimeSeconds(idTokenExpiry);

                    if (idTokenExpiryFromEpoch < DateTimeOffset.UtcNow)
                    {
                        result = this.Ok("false");
                    }
                }
            }
            catch
            {
            }

            return result;
        }

        /// <summary>
        /// Resend the verification email.
        /// </summary>
        /// <param name="email">The email address.</param>
        /// <param name="attempt">The attempt.</param>
        /// <returns>A response.</returns>
        [HttpPost(HeIdentityPaths.ResendVerificationEmail)]
        public async Task<IActionResult> ResendVerificationEmail(string email, int attempt = 1)
        {
            var redirect = this.RedirectToAction(nameof(this.VerifyEmail), new { Attempt = attempt + 1 });

            if (email == null)
            {
                return redirect;
            }

            await this.identityManagementService.ResendVerificationEmail(email);

            return redirect;
        }
    }
}
