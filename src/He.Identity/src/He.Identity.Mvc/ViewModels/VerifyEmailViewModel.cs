namespace He.Identity.Mvc.ViewModels
{
    /// <summary>
    /// View Model.
    /// </summary>
    public class VerifyEmailViewModel
    {
        /// <summary>
        /// Gets or sets the email that needs to be verified.
        /// </summary>
        public string EmailtoVerify { get; set; }

        /// <summary>
        /// Gets or sets the attempt.
        /// </summary>
        public int Attempt { get; set; }

        /// <summary>
        /// Gets or sets the support email.
        /// </summary>
        public string SupportEmail { get; set; }
    }
}
