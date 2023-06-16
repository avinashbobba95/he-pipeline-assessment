using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace He.Identity.Mvc.TagHelpers
{
    /// <summary>
    /// Creates a Script tag that runs checking for
    /// user inactivity and redirects to the login page
    /// if the user's session times out.
    /// </summary>
    public class HeIdentityIdleSessionTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeIdentityIdleSessionTagHelper"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">httpContextAccessor.</param>
        public HeIdentityIdleSessionTagHelper(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Gets or sets TimeInSeconds.
        /// </summary>
        public int TimeInSeconds { get; set; }

        /// <summary>
        /// Creates the tag helper.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="output">The output.</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var user = this.httpContextAccessor.HttpContext.User;

            string idleSessionSnippet = user.Identity.IsAuthenticated ? @"
            <script>
                    (function () {
                      setInterval(function () {
                        fetch('/check-session').then(function(r) {
                          if (r.status === 200) {
                            r.text().then(function(text) {
                                if(text === 'false'){
                                    var loc = window.location.origin + '/signout';
                                    return (window.location.href = loc);
                                }
                            })
                          }
                        });
                      }, time * 1000);
                    })();
            </script>" : string.Empty;

            idleSessionSnippet = idleSessionSnippet.Replace("time", this.TimeInSeconds.ToString());
            output.TagName = string.Empty;
            output.Content.SetHtmlContent(idleSessionSnippet);
        }
    }
}
