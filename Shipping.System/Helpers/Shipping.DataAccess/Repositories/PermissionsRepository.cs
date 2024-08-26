using FluentResults;
using Microsoft.EntityFrameworkCore;
using Shipping.Application.Abstracts;
using Shipping.Application.Features.UserManagement.Permissions;
using Shipping.Application.Features.UserManagement.Permissions.Commands.CreatePermission;
using Shipping.Application.Features.UserManagement.Permissions.Commands.DeletePermission;
using Shipping.Application.Features.UserManagement.Permissions.Queries.GetAllPermissions;
using Shipping.Application.Features.UserManagement.Permissions.Queries.GetPermissionsByRoleId;
using Shipping.DataAccess.Persistence.DataBase;
using Shipping.Domain.Entities;

namespace Shipping.DataAccess.Repositories;

public class PermissionsRepository  : IPermissionsRepository
{
    private readonly ShippingDbContext _shippingDb;

    public PermissionsRepository(ShippingDbContext shippingDb)
    {
        _shippingDb = shippingDb;
    }

    public async Task<Result<List<GetPermissionsResponse>>> GetAllPermissions(GetAllPermissionsRequest request, CancellationToken cancellationToken)
    {
        var permissions = await _shippingDb.Permissions
            .Include(p => p.Role)
            .GroupBy(y => y.Role)
            .Select(g => new GetPermissionsResponse()
            {
                RoleName = g.First(x => x.RoleId == x.Role.Id).Role.Name,
                Permissions = g.Select(up => new
                    Permissions()
                    {
                        PermissionId = up.Id,
                        PermissionName = up.Name
                    }).ToList()
            }).ToListAsync(cancellationToken);
        
        if (permissions.Count <= 0)
            return Result.Fail<List<GetPermissionsResponse>>( "لا يوجد صلاحيات" );

        return permissions;    }

    public async Task<Result<List<GetPermissionsResponse>>> GetAllPermissionsByRoleId(GetAllPermissionsByRoleIdRequest request, CancellationToken cancellationToken)
    {
        var permissions = await _shippingDb.Permissions
            .Include(up => up.Role)
            .Where(up => up.RoleId == request.RoleId)
            .GroupBy(up => up.Role)
            .Select(g => new GetPermissionsResponse()
            {
                RoleName =g.First(x => x.RoleId == x.Role.Id).Role.Name,
                Permissions = g.Select(up => new Permissions()
                {
                    PermissionId = up.Id,
                    PermissionName = up.Name
                }).ToList()
            }).ToListAsync(cancellationToken);
        
        if (permissions.Count <= 0)
            return Result.Fail<List<GetPermissionsResponse>>( "هذه الصلاحية غير موجودة" );
        
        return permissions;
    }

    public async Task<Result<string>> CreatePermission(CreatePermissionRequest request, CancellationToken cancellationToken)
    {
        var permission = await _shippingDb.Permissions.FirstOrDefaultAsync(x=>x.RoleId == request.RoleId, cancellationToken);
        if (permission != null)
            return Result.Fail("هذي الصلاحية موجودة");    
        
        permission = new Permission
        {
            Name = request.PermissionName,
            RoleId = request.RoleId
        };
        _shippingDb.Permissions.Add(permission);
        await _shippingDb.SaveChangesAsync(cancellationToken);
        
        return "تم الاضافة";
    }

    public async Task<Result> DeleteUserPermissions(Guid CustomerId, CancellationToken cancellationToken)
    {
        var permissions = await _shippingDb.UserPermissions.FirstOrDefaultAsync(x => x.CustomerId == CustomerId, cancellationToken);
        
        if (permissions is null)
            return Result.Fail("هذه الصلاحية غير موجودة");

        var result =  _shippingDb.UserPermissions.Remove(permissions);
        await _shippingDb.SaveChangesAsync(cancellationToken);


        return Result.Ok();
        
    }   
    public async Task<Result<string>> DeletePermissions(DeletePermissionRequest request, CancellationToken cancellationToken)
    {
        var permissions = await _shippingDb.Permissions.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        
        if (permissions is null)
            return Result.Fail("هذه الصلاحية غير موجودة");

        var result =  _shippingDb.Permissions.Remove(permissions);
        await _shippingDb.SaveChangesAsync(cancellationToken);
        
        return "تم الحذف";    
    }
}