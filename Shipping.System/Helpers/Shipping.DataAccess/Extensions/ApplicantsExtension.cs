using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shipping.DataAccess.Persistence.DataBase;
using Shipping.DataAccess.Persistence.Seeder;
using Shipping.Domain.Models;

namespace Shipping.DataAccess.Extensions;

public static class ApplicantsExtension
{
    public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var identityConnectionString = configuration.GetConnectionString("IdentityConnection");
        if (!string.IsNullOrEmpty(identityConnectionString))
            services.AddDbContextPool<IdentityUsersDbContext>(options => options.UseSqlServer(identityConnectionString)
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging());
        
        services.AddIdentity<AppUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<IdentityUsersDbContext>();
        
        // _____________________________________________________________________________________________
        
        var shippingConnectionString = configuration.GetConnectionString("ShippingDb");
        if (!string.IsNullOrEmpty(shippingConnectionString))
            services.AddDbContextPool<ShippingDbContext>(options => options.UseSqlServer(shippingConnectionString)
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging());

        services.AddTransient<SeedService>();
        using var serviceProvider = services.BuildServiceProvider();
        using var appDbContext = serviceProvider.GetService<ShippingDbContext>();
        appDbContext?.Database.Migrate();
        serviceProvider.GetService<SeedService>()!.Seed().Wait();
    }
}