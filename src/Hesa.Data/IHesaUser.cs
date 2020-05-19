namespace Hesa.Data
{
    /// <summary>
    /// Interface defining any model that represents an application user.
    /// </summary>
    /// <remarks>
    /// Using this because I don't want to expose the entire ASP.NET Core identity model to consumers of user information.
    /// It also helps to have a standard definition of user properties that are required of any user-style models.
    /// </remarks>
    public interface IHesaUser
    {
        /// <summary>
        /// Gets the PK of the user record.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Gets the username.
        /// </summary>
        public string UserName { get; }

        /// <summary>
        /// Gets the first name.
        /// </summary>
        public string FirstName { get; }

        /// <summary>
        /// Gets the last name.
        /// </summary>
        public string LastName { get; }

        /// <summary>
        /// Gets the user's email.
        /// </summary>
        public string Email { get; }
    }
}
