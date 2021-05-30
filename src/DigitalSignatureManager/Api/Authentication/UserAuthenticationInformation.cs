namespace Api.Authentication
{
    /// <summary>
    /// Model for user login. 
    /// </summary>
    public class UserAuthenticationInformation
    {
        /// <summary>
        /// Email that was used for registration
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Password that was used for registration
        /// </summary>
        public string Password { get; set; }
    }
}