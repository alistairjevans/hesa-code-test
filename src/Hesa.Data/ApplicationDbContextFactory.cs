using System.IO;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Hesa.Data
{
    /// <summary>
    /// Provides a Db Context factory that allows the migration tooling to 'discover' the DB context.
    /// </summary>
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        /// <inheritdoc/>
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.design.json")
                .Build();

            var dbContextBuilder = new DbContextOptionsBuilder();

            var connectionString = configuration.GetConnectionString("design");

            dbContextBuilder.UseSqlite(connectionString);

            return new ApplicationDbContext(dbContextBuilder.Options, Options.Create(new OperationalStoreOptions()));
        }
    }
}
