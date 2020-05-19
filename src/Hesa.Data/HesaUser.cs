using Microsoft.AspNetCore.Identity;

namespace Hesa.Data
{
    /// <summary>
    /// Model that represents an application user; derived from the built-in ASP.NET Core Identity model.
    /// </summary>
    public class HesaUser : IdentityUser, IHesaUser
    {
        /// <summary>
        /// Gets or sets the user's first name.
        /// </summary>
        public string FirstName { get; set; } = default!;

        /// <summary>
        /// Gets or sets the user's first name.
        /// </summary>
        public string LastName { get; set; } = default!;
    }
}
