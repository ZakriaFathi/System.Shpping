using System.Security.Claims;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shipping.Application.Abstracts;
using Shipping.Application.Features.Branchs.Commands.CreateBranch;
using Shipping.Application.Features.UserManagement.Permissions.Queries.GetAllPermissions;
using Shipping.Application.Features.UserManagement.Users.Commands.CreateUser;
using Shipping.Application.Features.UserManagement.Users.Commands.CreateUserPermissions;
using Shipping.Application.Models.IdentityModel;
using Shipping.Application.Models.UserManagement;
using Shipping.DataAccess.Persistence.DataBase;
using Shipping.Domain.Entities;
using Shipping.Domain.Models;
using Shipping.Utils.Enums;
using Shipping.Utils.Helper;

namespace Shipping.DataAccess.Persistence.Seeder;

public class SeedService
{
    private readonly ShippingDbContext _dbContext;
    private readonly IdentityUsersDbContext _dbIdentityDbContext;
    private readonly IPermissionsRepository _permissionsService;
    private readonly IBranchRepository _branchRepository;
    private readonly IIdentityRepository _identityRepository; 
    private readonly IUserRepository _userRepository;


    public SeedService(ShippingDbContext dbContext, IPermissionsRepository permissionsService, IdentityUsersDbContext dbIdentityDbContext, IBranchRepository branchRepository, IIdentityRepository identityRepository, IUserRepository userRepository)
    {
        _dbContext = dbContext;
        _permissionsService = permissionsService;
        _dbIdentityDbContext = dbIdentityDbContext;
        _branchRepository = branchRepository;
        _identityRepository = identityRepository;
        _userRepository = userRepository;
    }
    
    public async Task Seed()
    {
        await SeedRolesAndPermissions();
        await SeedOwnerAndBranch();
    }
    
    private async Task SeedRolesAndPermissions()
    {
        if (await _dbContext.Roles.AnyAsync()) return;
        if (await _dbContext.Permissions.AnyAsync()) return;
        var rolesPermissions = RolePermissionHelper.GetRolePermissions();
        
        var dbPermissions = await _dbContext.Permissions.ToListAsync();
        var dbRoles = await _dbContext.Roles.ToListAsync();
        
        var roles = new List<Role>();
        var permissions = new List<Permission>();
        rolesPermissions.ForEach(x =>
        {
            roles.Add(Role.Create(x.RoleId, x.RoleName));
            x.Permissions.ForEach(p => permissions.Add(Permission.Create(p.PermissionId,p.PermissionName,x.RoleId)));
        });

        var rolesToAdd = roles.ExceptBy(dbRoles.Select(x => x.Id),x => x.Id);
        var permissionsToAdd = permissions.ExceptBy(dbPermissions.Select(x => x.Id),x => x.Id);
        
        await _dbContext.Roles.AddRangeAsync(rolesToAdd);
        await _dbContext.Permissions.AddRangeAsync(permissionsToAdd);

        await _dbContext.SaveChangesAsync();
    }

    private async Task SeedOwnerAndBranch()
    {
        var user = await _dbIdentityDbContext.Users.ToListAsync();
        var owmer = await _dbContext.Users.ToListAsync();
        var branchs = await _dbContext.Branchs.ToListAsync();
        
        if (user.Any() || owmer.Any() || branchs.Any()) return;
        
        var branch = await _branchRepository.CreateBranchAsync(new CreateBranchRequest()
        {
            Name = "طرابلس",
            IsMajor = true
        }, CancellationToken.None);

        var identityUser = await _identityRepository.InsertIdentityUser(new InsertAndUpdateIdentityUser()
        {
            PhoneNumber = "",
            FirstName = "Owner",
            LastName = "Owner",
            Address = "",
            UserName = "Owner",
            UserType = UserType.Owner,
            ActivateState = ActivateState.Active,
            Password = "12345678",
        }, CancellationToken.None);

        var userProfile = await _userRepository.InsertUserAsync(new InsertAndUpdateUserCommnd()
        {
            UserId = Guid.Parse(identityUser.Value.Id),
            UserName = identityUser.Value.UserName,
            UserType = identityUser.Value.UserType,
            Password = "12345678",
        }, CancellationToken.None);

        var insertUser = await _userRepository.InsertEmployeeAsync(new InsertAndUpdateEmployeeCommnd()
        {
            UserId = Guid.Parse(identityUser.Value.Id),
            Address = identityUser.Value.Address,
            PhoneNumber = identityUser.Value.PhoneNumber,
            Name = "Owner Owner",
            BranchId = Guid.Parse(branch.Value)
        }, CancellationToken.None);

        await CreateUserPermissions(identityUser.Value.Id, CancellationToken.None);
    }

    private async Task<Result<string>> CreateUserPermissions(string userId, CancellationToken cancellationToken)
    {
        var allPermissions =
            await _permissionsService.GetAllPermissions(new GetAllPermissionsRequest(), cancellationToken);
        
        if (!allPermissions.IsSuccess)
            return Result.Fail(allPermissions.Errors.ToList());
        
        var claim = new List<Claim>();
        foreach (var role in allPermissions.Value)
        {
            foreach (var permission in role.Permissions)
            {
                claim.Add(new Claim(role.RoleName, permission.PermissionName));
            }
        }

        if (claim.Count <= 0)
            return Result.Fail("لا يوجد صلاحيات");
        
        var claims = claim.GroupBy(x => x.Type).Select(y => new UserClaims 
        { 
            type = y.Key, 
            value = y.Select(x => x.Value).ToList() 
        }).ToList();
        
        var identityClims = await _identityRepository.InsertIdentityUserClaims(new InsertAndUpdateIdentityClaims()
        {
            UserId = userId,
            Claims = claims
        }, cancellationToken);
        
        if (!identityClims.IsSuccess)
            return Result.Fail(identityClims.Errors.ToList());

        var userClims = await _userRepository.CreateUserPermissions(new InsertAndUpdateUserPermissions()
        {
            UserId =   userId,
            Permissions = allPermissions.Value.SelectMany(r => r.Permissions.Select(p => p.PermissionId)).ToList(),
        }, cancellationToken);
        
        if (!userClims.IsSuccess)
            return Result.Fail(userClims.Errors.ToList());

        return " تمت اضافة صلاحيات المستخدم بنجاح ";
    }
}