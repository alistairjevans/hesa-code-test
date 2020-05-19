using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using Hesa.Data;
using Hesa.Logic;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Hesa.Portal
{
    /// <summary>
    /// Startup class - initialises ASP.NET Core App.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">Loaded application config.</param>
        /// <param name="environment">Hosting environment details.</param>
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        private IConfiguration Configuration { get; }

        private IWebHostEnvironment Environment { get; }

        /// <summary>
        /// DI Configuration.
        /// </summary>
        /// <param name="services">Service registry.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(
                    Configuration.GetConnectionString("DefaultConnection")));

            ConfigureIdentityServices(services);

            services.AddControllers();
            services.AddRazorPages();

            services.AddTransient<IProfileService, ProfileService>();

            // Add Logic Services
            // TODO: Factor this into Module Dependency initialisation (keeping service definitions in their own assemblies).
            services.AddTransient<ISeedData, SeedDataProvider>();
            services.AddTransient<IUserSearch, UserSearchProvider>();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "Client/build";
            });
        }

        /// <summary>
        /// Configures the request pipeline.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="env">The hosting environment.</param>
        [SuppressMessage(
            "Performance",
            "CA1822:Mark members as static",
            Justification = "Framework-invoke.")]
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (app is null)
            {
                throw new System.ArgumentNullException(nameof(app));
            }

            if (env.IsDevelopment())
            {
                // Seed our test data.
                StartupSeed(app);

                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            // TODO: Re-enable (and enforce) HTTPS redirection.
            //       Disabled for this code test to reduce the chance of ASP.NET Core HTTPS Certificate problems
            //       for the reviewer.
            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "Client";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }

        private void ConfigureIdentityServices(IServiceCollection services)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddDefaultIdentity<HesaUser>(options =>
            {
                if (Environment.IsDevelopment())
                {
                    // Loosen the password restrictions in development to make it easier to test.
                    options.Password.RequireDigit = false;
                    options.Password.RequireNonAlphanumeric = false;
                }
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddIdentityServer()
                .AddApiAuthorization<HesaUser, ApplicationDbContext>();

            services.AddAuthentication()
                .AddIdentityServerJwt();

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy(ApplicationPolicies.RequiresAdminRights, p => p.RequireRole(ApplicationRoles.Administrator));
            });
        }

        /// <summary>
        /// Seed test data into the system.
        /// </summary>
        /// <remarks>
        /// TODO: Move this somewhere else; this sort of behaviour should be in an external app.
        /// </remarks>
        private static void StartupSeed(IApplicationBuilder app)
        {
            // Create a unit of work to do the seeding in.
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var seeder = scope.ServiceProvider.GetService<ISeedData>();

                // We aren't in an async context here, so we will j
                seeder.SeedTestDataAsync().GetAwaiter().GetResult();
            }
        }
    }
}
