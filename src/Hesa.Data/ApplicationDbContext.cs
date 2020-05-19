using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Hesa.Data
{
    /// <summary>
    /// Defines the application DB context (currently only containing Identity components).
    /// </summary>
    public class ApplicationDbContext : ApiAuthorizationDbContext<HesaUser>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext"/> class.
        /// </summary>
        /// <param name="options">The context options.</param>
        /// <param name="operationalStoreOptions">Identity Server options.</param>
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options, operationalStoreOptions)
        {
        }
    }
}
