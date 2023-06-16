using System;

namespace He.Identity.Mvc
{
    /// <summary>
    /// HeIdentityException.
    /// </summary>
    public class HeIdentityException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HeIdentityException"/> class.
        /// </summary>
        /// <param name="message">message.</param>
        /// <param name="innerException">innerException.</param>
        public HeIdentityException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
