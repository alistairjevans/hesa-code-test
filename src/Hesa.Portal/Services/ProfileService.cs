using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Hesa.Data;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;

namespace Hesa.Portal
{
    /// <summary>
    /// Custom profile service to set custom claims and role data.
    /// </summary>
    public class ProfileService : IProfileService
    {
        private UserManager<HesaUser> _userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileService"/> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        public ProfileService(UserManager<HesaUser> userManager)
        {
            _userManager = userManager;
        }

        /// <inheritdoc/>
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var user = await _userManager.GetUserAsync(context.Subject);

            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim("firstName", user.FirstName),
                new Claim("lastName", user.LastName),
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(JwtClaimTypes.Role, role));
            }

            context.IssuedClaims.AddRange(claims);
        }

        /// <inheritdoc/>
        public async Task IsActiveAsync(IsActiveContext context)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var user = await _userManager.GetUserAsync(context.Subject);

            context.IsActive = user != null;
        }
    }
}
