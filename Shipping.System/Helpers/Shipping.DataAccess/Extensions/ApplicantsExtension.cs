using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shipping.DataAccess.Persistence.DataBase;
using Shipping.Domain.Models;

namespace Shipping.DataAccess.Extensions;

public static class ApplicantsExtension
{
    public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {


        // void SqlServerOptionsAction(SqlServerDbContextOptionsBuilder options)
        // {
        //     options.MigrationsAssembly("Municipal.DataAccess");
        // }
        //
        // services.AddDbContext<IdentityUsersDbContext>((serviceProvider, dbContextOptionsBuilder) =>
        // {
        //     dbContextOptionsBuilder.UseSqlServer(serviceProvider.GetRequiredService<IConfiguration>().GetConnectionString("IdentityConnection"),
        //         SqlServerOptionsAction);
        //
        // });
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
        
    }
}