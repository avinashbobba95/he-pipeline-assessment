using System.Collections.Generic;

namespace He.Identity.Mvc
{
    /// <summary>
    /// Configuration for Homes England Identity service. Use when multiple Authorities are allowed.
    /// </summary>
    public class HeIdentityMultiSchemeJwtConfiguration
    {
        /// <summary>
        /// The default constructor.
        /// </summary>
        public HeIdentityMultiSchemeJwtConfiguration()
        {
            this.Configurations = new List<HeIdentityJwtConfiguration>();
        }

        /// <summary>
        /// A list of HeIdentityJwtConfigurations.
        /// </summary>
        public List<HeIdentityJwtConfiguration> Configurations { get; set; }
    }
}
