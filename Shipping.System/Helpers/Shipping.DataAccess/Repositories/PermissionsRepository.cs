using FluentResults;
using Microsoft.EntityFrameworkCore;
using Shipping.Application.Abstracts;
using Shipping.Application.Features.UserManagement.Permissions;
using Shipping.Application.Features.UserManagement.Permissions.Commands.DeletePermission;
using Shipping.Application.Features.UserManagement.Permissions.Queries.GetAllPermissions;
using Shipping.Application.Features.UserManagement.Permissions.Queries.GetPermissionsByRoleId;
using Shipping.DataAccess.Persistence.DataBase;
using Shipping.Domain.Entities;

namespace Shipping.DataAccess.Repositories;

public class PermissionsRepository  : IPermissionsRepository
{
    private readonly ShippingDbContext _shippingDb;
    private readonly IIdentityRepository _identityRepository;

    public PermissionsRepository(ShippingDbContext shippingDb, IIdentityRepository identityRepository)
    {
        _shippingDb = shippingDb;
        _identityRepository = identityRepository;
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

        return permissions;
    }
    public async Task<Result> DeleteUserPermissions(DeletePermissionRequest request, CancellationToken cancellationToken)
    { 
        var permissions = await _shippingDb.UserPermissions
            .Where(x => x.CustomerId == request.UserId)
            .ToListAsync(cancellationToken);        
        if (permissions.Count == 0)
            return Result.Fail("هذه الصلاحية غير موجودة");

        _shippingDb.UserPermissions.RemoveRange(permissions);
        var result = await _shippingDb.SaveChangesAsync(cancellationToken);

        if (result <= 0)
            return Result.Fail("حدثت مشكلة بالخادم الرجاء الاتصال بالدعم الفني");
        
        return Result.Ok();
        
    }   
    public async Task<Result<List<GetPermissionsResponse>>> GetPermissionsByUserId(GetAllPermissionsByUserIdRequest request,  CancellationToken cancellationToken)
    {
        var permissions = await _shippingDb.UserPermissions
            .Where(y => y.CustomerId == request.UserId)
            .Include(up => up.Permission)
            .ThenInclude(p => p.Role)
            .GroupBy(y => y.Permission.Role)
            .Select(x => new GetPermissionsResponse()
            {
                RoleName = x.Key.Name,
                Permissions = x.Select(up => new
                    Permissions()
                    {
                        PermissionId = up.Permission.Id,
                        PermissionName = up.Permission.Name
                    }).ToList()
            }).ToListAsync( cancellationToken);
        
        if (permissions.Count <= 0)
            return Result.Fail<List<GetPermissionsResponse>>( "لا توجد مسؤليات للمستخدم" );

        return permissions;
    }
}