using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shipping.Application.Abstracts;
using Shipping.Application.Features.UserManagement.Users.Commands.CreateUser;
using Shipping.DataAccess.Persistence.DataBase;
using Shipping.DataAccess.Persistence.Seeder;
using Shipping.DataAccess.Repositories;
using Shipping.DataAccess.Repositories.OrderRepo;
using Shipping.DataAccess.Repositories.UserManageRepo;
using Shipping.Utils.Options;

namespace Shipping.DataAccess.Extensions;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
    { 
        var provider = services.BuildServiceProvider();
        var config = provider.GetService<IConfiguration>();
        
        var connectivityOptions = config!.GetSection("JWT");
        services.Configure<JWT>(connectivityOptions);
        
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            cfg.RegisterServicesFromAssembly(typeof(CreateUserRequest).Assembly);
        });
        
        services.AddDbContext(configuration);
        services.AddAuthentications(configuration);
        services.AddHttpContextAccessor();
        
        services.AddTransient<IAuthRepository, AuthRepository>();
        services.AddTransient<IUserManagmentRepository, UserManageRepository>();
        services.AddTransient<IPermissionsRepository, PermissionsRepository>();
        services.AddTransient<ICityRepository, CityRepository>();
        services.AddTransient<IBranchRepository, BranchRepository>();
        services.AddTransient<IOrderRepository, OrderRepository>();
        services.AddTransient<ISherdOrderRepository, SherdOrderRepository>();
        services.AddTransient<IIdentityRepository, IdentityRepository>();
        services.AddTransient<IUserRepository, UserRepository>();

        services.AddTransient<SeedService>();
        
        using var serviceProvider = services.BuildServiceProvider();
        
        using var appDbContext = serviceProvider.GetService<ShippingDbContext>();
        appDbContext?.Database.Migrate(); 
        using var appDbContext1 = serviceProvider.GetService<IdentityUsersDbContext>();
        appDbContext1?.Database.Migrate();
        
        serviceProvider.GetService<SeedService>()!.Seed().Wait();

    }
}