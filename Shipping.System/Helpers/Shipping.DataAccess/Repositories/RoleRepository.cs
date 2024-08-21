using FluentResults;
using Microsoft.EntityFrameworkCore;
using Shipping.Application.Abstracts;
using Shipping.Application.Features.UserManagement.Roles;
using Shipping.Application.Features.UserManagement.Roles.Commands.CreateRole;
using Shipping.Application.Features.UserManagement.Roles.Queries.GetAllRoles;
using Shipping.Application.Features.UserManagement.Roles.Queries.GetRolesByUserId;
using Shipping.DataAccess.Persistence.DataBase;
using Shipping.Domain.Entities;

namespace Shipping.DataAccess.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly ShippingDbContext _shippingDb;

    public RoleRepository(ShippingDbContext shippingDb)
    {
        _shippingDb = shippingDb;
    }

    public async Task<Result<List<GetRolesResponse>>> GetAllRoles(GetAllRolesRequest request, CancellationToken cancellationToken)
    {
        var roles = await _shippingDb.Roles.Select(x => new GetRolesResponse()
        {
            Id = x.Id,
            Name = x.Name
        }).ToListAsync(cancellationToken: cancellationToken);
        
        if (roles.Count <= 0)
            return Result.Fail<List<GetRolesResponse>>( "لا توجد مسؤليات " );

        return roles;    
    }

    public async Task<Result<List<GetRolesResponse>>> GetRolesByUserId(GetRolesByUserIdRequest request, CancellationToken cancellationToken)
    {
        var roles = await _shippingDb.UserPermissions
            .Include(up => up.Permission)
            .ThenInclude(p => p.Role)
            .Where(up => up.CustomerId == request.UserId)
            .Select(x => new GetRolesResponse()
            {
                Id = x.Permission.Role.Id,
                Name = x.Permission.Role.Name
            }).Distinct().ToListAsync( cancellationToken);
        
        if (roles.Count <= 0)
            return Result.Fail<List<GetRolesResponse>>( "لا توجد مسؤليات للمستخدم" );

        return roles;
    }

    public async Task<Result<string>> CreateRole(CreateRoleRequest request, CancellationToken cancellationToken)
    {
        var role = await _shippingDb.Roles.FirstOrDefaultAsync(x=>x.Name == request.RoleName, cancellationToken);
        if (role != null)
            return Result.Fail("Role already exists");
        
        role = new Role
        {
            Name = request.RoleName
        };
        _shippingDb.Roles.Add(role);
        await _shippingDb.SaveChangesAsync(cancellationToken);
        return "تم الاضافة";
    }
}