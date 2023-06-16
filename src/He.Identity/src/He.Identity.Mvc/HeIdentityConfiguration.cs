using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace He.Identity.Mvc
{
    /// <summary>
    /// Configuration for Homes England Identity service.
    /// </summary>
    public abstract class HeIdentityConfiguration
    {
        /// <summary>
        /// Gets or sets the auth tenant.
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// Gets or sets the Audience.
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Gets or sets the support email.
        /// </summary>
        public string SupportEmail { get; set; }
    }
}
