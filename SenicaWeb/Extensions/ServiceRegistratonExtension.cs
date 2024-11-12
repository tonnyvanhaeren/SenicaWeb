using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SenicaWeb.Data;
using SenicaWeb.IdentityPolicy;
using SenicaWeb.Models;

namespace SenicaWeb.Extensions;

public static class ServiceRegistrationExtension
{
    public static IServiceCollection AddConfigSqlContext(this IServiceCollection services, IConfiguration config)
    {
        // Console.WriteLine(config["SqlServer:ConnectionString"]);

        var sqlConBuilder = new SqlConnectionStringBuilder
        {
            ConnectionString = config["SqlServer:ConnectionString"]!,
            TrustServerCertificate = true,
            UserID = config["SqlServer:User"],
            Password = config["SqlServer:Password"],
            MultipleActiveResultSets = true
        };

        services.AddDbContext<SenicaDbContext>(opt => opt.UseSqlServer(sqlConBuilder.ConnectionString));

        return services;
    }

    public static IServiceCollection AddConfigIdentity(this IServiceCollection services)
    {
        services.AddIdentity<AppUser, IdentityRole>()
            .AddEntityFrameworkStores<SenicaDbContext>()
            .AddDefaultTokenProviders();

/*         services.AddIdentity<ApplicationUser, IdentityRole>( options => options.SignIn.RequireConfirmedAccount = false )
            .AddUserManager<ApplicationUser>()
            .AddRoleManager<IdentityRole>()
            .AddEntityFrameworkStores<SenicaDbContext>()
            .AddDefaultTokenProviders(); */

        services.Configure<IdentityOptions>(opts =>
        {
            opts.SignIn.RequireConfirmedAccount = false;
            opts.Lockout.AllowedForNewUsers = true;
            opts.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
            opts.Lockout.MaxFailedAccessAttempts = 3;

            opts.User.RequireUniqueEmail = true;
            // opts.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyz";
            opts.Password.RequiredLength = 8;
            opts.Password.RequireLowercase = true;
            opts.Password.RequireNonAlphanumeric = false;
            opts.Password.RequireDigit = false;
        });     


        //builder.Services.ConfigureApplicationCookie(opts => opts.LoginPath = "/Authenticate/Login");
        
        services.AddTransient<IPasswordValidator<AppUser>, CustomPasswordPolicy>();
        // services.AddTransient<IUserValidator<AppUser>, CustomUsernameEmailPolicy>();

        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.Name = ".AspNetCore.SenicaWeb.Application";
            options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
            options.SlidingExpiration = true;
        });
        
        return services;
    }

    public static IServiceCollection AddDbContextServices(this ServiceCollection services)
    {
        return services;
    }
    
}
