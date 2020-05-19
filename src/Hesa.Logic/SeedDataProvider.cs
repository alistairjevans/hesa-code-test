using System.Text.Json;
using System.Threading.Tasks;
using Hesa.Data;
using Microsoft.AspNetCore.Identity;

namespace Hesa.Logic
{
    /// <summary>
    /// Provides the functionality to seed the database with test data.
    /// </summary>
    public class SeedDataProvider : ISeedData
    {
        private readonly UserManager<HesaUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="SeedDataProvider"/> class.
        /// </summary>
        /// <param name="userManager">The identity user manager.</param>
        /// <param name="roleManager">The identity role manager.</param>
        public SeedDataProvider(UserManager<HesaUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <inheritdoc/>
        public async Task SeedTestDataAsync()
        {
            // Create the admin role if it doesn't exist.
            if (!await _roleManager.RoleExistsAsync(ApplicationRoles.Administrator))
            {
                await _roleManager.CreateAsync(new IdentityRole(ApplicationRoles.Administrator));
            }

            // If there are no admin users, then create one.
            if ((await _userManager.GetUsersInRoleAsync(ApplicationRoles.Administrator)).Count == 0)
            {
                // Dummy admin user.
                var user = new HesaUser
                {
                    UserName = "testadmin",
                    Email = "testadmin@hesa.ac.uk",
                    FirstName = "Joe",
                    LastName = "Bloggs",
                };

                await _userManager.CreateAsync(user, "Password100");

                await _userManager.AddToRoleAsync(user, ApplicationRoles.Administrator);
            }

            if (!await _roleManager.RoleExistsAsync(ApplicationRoles.Student))
            {
                await _roleManager.CreateAsync(new IdentityRole(ApplicationRoles.Student));
            }

            // Generate example users from seed data.
            if ((await _userManager.GetUsersInRoleAsync(ApplicationRoles.Student)).Count == 0)
            {
                var seedData = JsonSerializer.Deserialize<HesaUser[]>(SeedData.seed_users);

                foreach (var seedUser in seedData)
                {
                    await _userManager.CreateAsync(seedUser, "Password100");

                    await _userManager.AddToRoleAsync(seedUser, ApplicationRoles.Student);
                }
            }
        }
    }
}
