using CMS.Data;
using Microsoft.EntityFrameworkCore;

namespace CMS.Website.Services
{
    public static class ServiceExtensions
    {

        public static void ConfigureLogging(this IServiceCollection services)
        {
            services.AddScoped<ILoggerManager, LoggerManager>();
        }
        public static void ConfigureConnectDB(this IServiceCollection services, string connectStrings)
        {
            services.AddDbContextFactory<CmsContext>(options =>
                options.UseSqlServer(connectStrings), ServiceLifetime.Transient);
        }
        public static void ConfigureConnectDBAuth(this IServiceCollection services, string connectStrings)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectStrings));

        }
        public static void ConfigureTransient(this IServiceCollection services)
        {
            services.AddTransient<RepositoryWrapper>();
        }
        public static void ConfigureDefaultIdentity(this IServiceCollection services)
        {
            services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;               
            })
        .AddRoles<IdentityRole>()
            .AddRoles<IdentityRole>()
            .AddRoleManager<RoleManager<IdentityRole>>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Login";
                options.LogoutPath = "/Identity/Account/Logout";
                options.AccessDeniedPath = "/AccessDenied";

            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            });

        }
        public static void ConfigureRazorPagesAuthorize(this IServiceCollection services)
        {
            services.AddMvc().AddRazorPagesOptions(options =>
            {
                //options.Conventions.AuthorizeAreaFolder("Admin", "/");
                //options.Conventions.AuthorizeAreaFolder("Shopman", "/");
                //options.Conventions.AuthorizeAreaFolder("Member", "/");
                //options.Conventions.AuthorizeAreaFolder("Identity", "/");
            });
        }
    }
}
