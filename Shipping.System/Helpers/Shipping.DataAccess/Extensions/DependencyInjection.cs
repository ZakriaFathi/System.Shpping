using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shipping.Application.Abstracts;
using Shipping.Application.Features.UserManagement.Users.Commands.CreateUser;
using Shipping.DataAccess.Persistence.Seeder;
using Shipping.DataAccess.Repositories;
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
        services.AddTransient<ISherdUserRepository, SherdUserRepository>();
        services.AddTransient<IPermissionsRepository, PermissionsRepository>();
        services.AddTransient<IRoleRepository, RoleRepository>();
        services.AddTransient<ICityRepository, CityRepository>();
        services.AddTransient<IBranchRepository, BranchRepository>();
        services.AddTransient<IOrderRepository, OrderRepository>();
        services.AddTransient<ISherdOrderRepository, SherdOrderRepository>();

        
    }
}