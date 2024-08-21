using System.Security.Claims;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Shipping.Application.Abstracts;
using Shipping.Application.Features.UserManagement.Permissions.Queries.GetAllPermissions;
using Shipping.Application.Features.UserManagement.Users.Commands.ChangePassword;
using Shipping.Application.Features.UserManagement.Users.Commands.ChangeUserActivation;
using Shipping.Application.Features.UserManagement.Users.Commands.CreateUser;
using Shipping.Application.Features.UserManagement.Users.Commands.CreateUserPermissions;
using Shipping.Application.Features.UserManagement.Users.Commands.UpdateUser;
using Shipping.Application.Features.UserManagement.Users.Commands.UpdateUserPermissions;
using Shipping.Application.Features.UserManagement.Users.Queries;
using Shipping.Application.Features.UserManagement.Users.Queries.GetAdmins;
using Shipping.Application.Features.UserManagement.Users.Queries.GetAdminsByBranchId;
using Shipping.Application.Features.UserManagement.Users.Queries.GetCustomers;
using Shipping.Application.Features.UserManagement.Users.Queries.GetRepresentatives;
using Shipping.Application.Features.UserManagement.Users.Queries.GetRepresentativesByBranchId;
using Shipping.Application.Models.IdentityModel;
using Shipping.Application.Models.UserManagement;
using Shipping.DataAccess.Persistence.DataBase;
using Shipping.Domain.Models;
using Shipping.Utils.Enums;

namespace Shipping.DataAccess.Repositories;

public class UserManageRepository : IUserManagmentRepository
{
    private readonly ShippingDbContext _shippingDb;
    private readonly ISherdUserRepository _sherdUserRepository; 
    private readonly IPermissionsRepository _permissionsService;

    public UserManageRepository(ShippingDbContext shippingDb, ISherdUserRepository sherdUserRepository, IPermissionsRepository permissionsService)
    {
        _shippingDb = shippingDb;
        _sherdUserRepository = sherdUserRepository;
        _permissionsService = permissionsService;
    }

    public async Task<Result<string>> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _sherdUserRepository.GetIdentityUserByUserName(request.UserName, cancellationToken);
        
        if (!user.IsSuccess)
            return Result.Fail(user.Errors.ToList());
        
        var types = request.UserType switch
        {
            UserTypeVm.SuperAdmin => UserType.SuperAdmin,
            UserTypeVm.Admin => UserType.Admin,
            UserTypeVm.Employee => UserType.Employee,
            UserTypeVm.Representative => UserType.Representative,
            _ => UserType.User
        };

        var identityUser = await _sherdUserRepository.InsertIdentityUser(new InsertAndUpdateIdentityUser()
        {
            PhoneNumber = request.PhoneNumber,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Address = request.Address,
            UserName = request.UserName,
            UserType = types,
            ActivateState = ActivateState.Active,
            Password = request.Password
        }, cancellationToken);
        
        if (!identityUser.IsSuccess)
            return Result.Fail(identityUser.Errors.ToList());

        var insertUser =
            await _sherdUserRepository.InsertUserAsync(new InsertAndUpdateUserCommnd()
            {
                UserId = Guid.Parse(identityUser.Value.Id),
                Password = request.Password,
                UserName = request.UserName,
                UserType = types,
                Address = request.Address,
                PhoneNumber = request.PhoneNumber,
                Name = request.FirstName + " " + request.LastName,
                BranchId = request.BranchId
            }, cancellationToken);
        
        if (!insertUser.IsSuccess)
            return Result.Fail(insertUser.Errors.ToList());
        
        return "تمت عملية اضافة المستخدم بنجاح ";
    }

    public async Task<Result<string>> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken cancellationToken)
    {
        var user = await _sherdUserRepository.GetIdentityUserById(request.UserId, cancellationToken);
        
        if (!user.IsSuccess)
            return Result.Fail(user.Errors.ToList());
        
        var userPassword = await _sherdUserRepository.ChangeIdentityPassword(new ChangeIdentityPassword()
        {
            UserId = user.Value.Id,
            NewPassword = request.NewPassword,
            OldPassword = request.OldPassword,
            ConfirmNewPassWord = request.ConfirmNewPassWord
        }, cancellationToken);
        if (!userPassword.IsSuccess)
            return Result.Fail(userPassword.Errors.ToList());

        var chengePassword =
            await _sherdUserRepository.ChangePasswordCustomerAsync(new ChangePasswordCommand()
            {
                UserId = Guid.Parse(user.Value.Id),
                NewPassWord = request.NewPassword,
                OldPassWord = request.OldPassword,
                ConfirmNewPassWord = request.ConfirmNewPassWord
            }, cancellationToken);
        if (!chengePassword.IsSuccess)
            return Result.Fail(chengePassword.Errors.ToList());
        return "تم تغيير كلمة المرور بنجاح";
    }

    public async Task<Result<string>> ChangeUserActivationAsync(ChangeUserActivationRequest request, CancellationToken cancellationToken)
    {
        var user = await _sherdUserRepository.GetIdentityUserById(request.UserId, cancellationToken);
        
        if (!user.IsSuccess)
            return Result.Fail(user.Errors.ToList());
        
        var changeUserActivation =
            await _sherdUserRepository.ChangeIdentityActivation(new ChangeIdentityActivation()
            {
                UserId = user.Value.Id,
                State = request.State
            }, cancellationToken);
        
        if (!changeUserActivation.IsSuccess)
            return Result.Fail(changeUserActivation.Errors.ToList());

        var chengeActivation =
            await _sherdUserRepository.ChangeCustomerActivationAsync(new ChangeUserActivationCommnd()
                {
                    UserId = Guid.Parse(user.Value.Id),
                    State = request.State
                },
                cancellationToken);
        
        if (!chengeActivation.IsSuccess)
            return Result.Fail(chengeActivation.Errors.ToList());
        
        return "تم تغيير حالة المستخدم بنجاح";
    }

    public async Task<Result<string>> UpdateUserAsync(UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _sherdUserRepository.GetIdentityUserById(request.UserId, cancellationToken);
        
        if (!user.IsSuccess)
            return Result.Fail(user.Errors.ToList());
        
        var updateUser = await _sherdUserRepository.UpdateIdentityCustomer(new InsertAndUpdateIdentityUser()
        {
            UserId = user.Value.Id,
            FirstName = request.FristName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            Address = request.Address,
            UserName = request.UserName,
        },cancellationToken);
        
        if (!updateUser.IsSuccess)
            return Result.Fail(updateUser.Errors.ToList());

        var update =
            await _sherdUserRepository.UpdateCustomerAsync(new InsertAndUpdateUserCommnd()
            {
                UserId = Guid.Parse(user.Value.Id),
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                Name = request.FristName + " " + request.LastName
            }, cancellationToken);

        if (!update.IsSuccess)
            return Result.Fail(update.Errors.ToList());
        
        return "تم تعديل المستخدم بنجاح";
    }

    public async Task<Result<List<GetCustomersResponse>>> GetCustomersAsync(GetCustomersRequest request, CancellationToken cancellationToken)
    {
        var users = await _shippingDb.Customers.Where(u => u.UserType == UserType.User)
            .Select(x => new GetCustomersResponse()
            {
                UserName = x.UserName,
                ActivateState = x.ActivateState,
                Email = x.Email,
                UserType = x.UserType,
                Address = x.Address,
                PhoneNumber = x.PhoneNumber,
                Name = x.Name,
                Password = x.Password
            }).ToListAsync(cancellationToken);


        if (users.Count <= 0) return Result.Fail("لا يوجد مستخدمين");
        
        return users;    
    }

    public async Task<Result<List<GetUsersResponse>>> GetRepresentativesAsync(GetRepresentativesRequest request, CancellationToken cancellationToken)
    {
        var users = await _shippingDb.Customers.Where(u => u.UserType == UserType.Representative)
            .Select(x => new GetUsersResponse()
            {
                UserName = x.UserName,
                ActivateState = x.ActivateState,
                Email = x.Email,
                UserType = x.UserType,
                Address = x.Address,
                PhoneNumber = x.PhoneNumber,
                Name = x.Name,
                Password = x.Password,
                BranchName = x.Branch.Name
            }).ToListAsync(cancellationToken);


        if (users.Count <= 0) return Result.Fail("لا يوجد مستخدمين");
        
        return users; 
    }

    public async Task<Result<List<GetUsersResponse>>> GetAdministratorsAsync(GetAdminsRequest request, CancellationToken cancellationToken)
    {
        var users = await _shippingDb.Customers.Where(u => u.UserType == UserType.Admin)
            .Select(x => new GetUsersResponse()
            {
                UserName = x.UserName,
                ActivateState = x.ActivateState,
                Email = x.Email,
                UserType = x.UserType,
                Address = x.Address,
                PhoneNumber = x.PhoneNumber,
                Name = x.Name,
                Password = x.Password,
                BranchName = x.Branch.Name
            }).ToListAsync(cancellationToken);


        if (users.Count <= 0) return Result.Fail("لا يوجد مستخدمين");
        
        return users; 
    }

    public async Task<Result<List<GetUsersResponse>>> GetRepresentativesByBranchIdAsync(GetRepresentativesByBranchIdRequest request,
        CancellationToken cancellationToken)
    {
        var user = await _shippingDb.Customers
            .Where(x => x.BranchId == request.BranchId && x.UserType == UserType.Representative)
            .Select(x => new GetUsersResponse()
            {
                UserName = x.UserName,
                ActivateState = x.ActivateState,
                Email = x.Email,
                UserType = x.UserType,
                Address = x.Address,
                PhoneNumber = x.PhoneNumber,
                Name = x.Name,
                Password = x.Password,
                BranchName = x.Branch.Name
            }).ToListAsync(cancellationToken);
        
        if (user.Count <= 0) return Result.Fail("لا يوجد مستخدمين");
        
        return user;
    }

    public async Task<Result<List<GetUsersResponse>>> GetAdministratorsByBranchIdAsync(GetAdminsByBranchIdRequest request, CancellationToken cancellationToken)
    {
        var user = await _shippingDb.Customers
            .Where(x => x.BranchId == request.BranchId && x.UserType == UserType.Admin)
            .Select(x => new GetUsersResponse()
            {
                UserName = x.UserName,
                ActivateState = x.ActivateState,
                Email = x.Email,
                UserType = x.UserType,
                Address = x.Address,
                PhoneNumber = x.PhoneNumber,
                Name = x.Name,
                Password = x.Password,
                BranchName = x.Branch.Name
            }).ToListAsync(cancellationToken);
        
        if (user.Count <= 0) return Result.Fail("لا يوجد مستخدمين");
        
        return user;    }

    public async Task<Result<string>> CreateUserPermissionsAsync(CreateUserPermissionsRequest request, CancellationToken cancellationToken)
    {
        var allPermissions =
            await _permissionsService.GetAllPermissions(new GetAllPermissionsRequest(), cancellationToken);
        
        if (!allPermissions.IsSuccess)
            return Result.Fail(allPermissions.Errors.ToList());
        
        
        var claim = new List<Claim>();
        foreach (var t in allPermissions.Value)
        {
            t.Permissions.ForEach(x =>
            {
                request.Permissions.ForEach(y =>
                {
                    if (y.ToString() == x.PermissionId.ToString())
                        claim.Add(new Claim(x.PermissionName, t.RoleName));
                });
            });
        }
        var claims = claim.GroupBy(x => x.Type).Select(y => new UserClaims 
        { 
            type = y.Key, 
            value = y.Select(x => x.Value).ToList() 
        }).ToList();
        
        var identityClims = await _sherdUserRepository.InsertIdentityUserClaims(new InsertAndUpdateIdentityClaims()
        {
            UserId = request.UserId,
            Claims = claims
        }, cancellationToken);
        
        if (!identityClims.IsSuccess)
            return Result.Fail(identityClims.Errors.ToList());

        var userClims = await _sherdUserRepository.CreateUserPermissions(new InsertAndUpdateUserPermissions()
        {
            UserId =   request.UserId,
            Permissions = request.Permissions,
        }, cancellationToken);
        
        if (!userClims.IsSuccess)
            return Result.Fail(userClims.Errors.ToList());

        return " تمت اضافة صلاحيات المستخدم بنجاح ";
    }

    public async Task<Result<string>> UpdateUserPermissionsAsync(UpdateUserPermissionsRequest request, CancellationToken cancellationToken)
    {
        var deleteOldPermissions = await _permissionsService.DeleteUserPermissions(request.UserId, cancellationToken);
        
        if (!deleteOldPermissions.IsSuccess)
            return Result.Fail(deleteOldPermissions.Errors.ToList());
        
        var allPermissions =
            await _permissionsService.GetAllPermissions(new GetAllPermissionsRequest(), cancellationToken);
        
        if (!allPermissions.IsSuccess)
            return Result.Fail(allPermissions.Errors.ToList());
        
        
        var claim = new List<Claim>();
        foreach (var t in allPermissions.Value)
        {
            t.Permissions.ForEach(x =>
            {
                request.Permissions.ForEach(y =>
                {
                    if (y.ToString() == x.PermissionId.ToString())
                        claim.Add(new Claim(x.PermissionName, t.RoleName));
                });
            });
        }
        var claims = claim.GroupBy(x => x.Type).Select(y => new UserClaims 
        { 
            type = y.Key, 
            value = y.Select(x => x.Value).ToList() 
        }).ToList();
        
        var identityClims = await _sherdUserRepository.UpdateIdentityUserClaims(new InsertAndUpdateIdentityClaims()
        {
            UserId = request.UserId.ToString(),
            Claims = claims
        }, cancellationToken);
        
        if (!identityClims.IsSuccess)
            return Result.Fail(identityClims.Errors.ToList());

        var userClims = await _sherdUserRepository.UpdateUserPermissions(new InsertAndUpdateUserPermissions()
        {
            UserId =   request.UserId.ToString(),
            Permissions = request.Permissions,
        }, cancellationToken);
        
        if (!userClims.IsSuccess)
            return Result.Fail(userClims.Errors.ToList());

        return " تمت تعديل صلاحيات المستخدم بنجاح ";
    }
}