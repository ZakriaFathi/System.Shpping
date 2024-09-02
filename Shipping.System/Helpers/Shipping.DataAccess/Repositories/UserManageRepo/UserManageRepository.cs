using System.Runtime.CompilerServices;
using System.Security.Claims;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Shipping.Application.Abstracts;
using Shipping.Application.Features.Auth.Commands.UpdateCustomer;
using Shipping.Application.Features.UserManagement.Permissions.Queries.GetAllPermissions;
using Shipping.Application.Features.UserManagement.Users.Commands.ChangeUserActivation;
using Shipping.Application.Features.UserManagement.Users.Commands.CreateUser;
using Shipping.Application.Features.UserManagement.Users.Commands.CreateUserPermissions;
using Shipping.Application.Features.UserManagement.Users.Commands.DeleteUser;
using Shipping.Application.Features.UserManagement.Users.Commands.ResetPassword;
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
using Shipping.Domain.Entities;
using Shipping.Domain.Models;
using Shipping.Utils.Enums;

namespace Shipping.DataAccess.Repositories.UserManageRepo;

public class UserManageRepository : IUserManagmentRepository
{
    private readonly ShippingDbContext _shippingDb;
    private readonly IIdentityRepository _identityRepository; 
    private readonly IUserRepository _userRepository;
    private readonly IPermissionsRepository _permissionsService;

    public UserManageRepository(ShippingDbContext shippingDb, IPermissionsRepository permissionsService, IUserRepository userRepository, IIdentityRepository identityRepository)
    {
        _shippingDb = shippingDb;
        _permissionsService = permissionsService;
        _userRepository = userRepository;
        _identityRepository = identityRepository;
    }


    public async Task<Result<string>> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var customer = await _shippingDb.Employees.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(request.UserId), cancellationToken);

        var user = await _identityRepository.GetIdentityUserByUserName(request.UserName, cancellationToken);
        if (!user.IsSuccess)
            return Result.Fail(user.Errors.ToList());
        
        UserType? types = request.UserType switch
        {
            UserTypeVm.Employee => UserType.Employee,
            UserTypeVm.Representative => UserType.Representative,
            _ => null
        };

        if (types is null)
            return Result.Fail("User type not found");

        var identityUser = await _identityRepository.InsertIdentityUser(new InsertAndUpdateIdentityUser()
        {
            PhoneNumber = request.PhoneNumber,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Address = request.Address,
            UserName = request.UserName,
            UserType = types.Value,
            ActivateState = ActivateState.Active,
            Password = request.Password
        }, cancellationToken);

        if (!identityUser.IsSuccess)
            return Result.Fail(identityUser.Errors.ToList());

        var userProfile = await _userRepository.InsertUserAsync(new InsertAndUpdateUserCommnd()
        {
            UserId = Guid.Parse(identityUser.Value.Id),
            UserName = identityUser.Value.UserName,
            UserType = identityUser.Value.UserType,
            Password = request.Password,
        }, cancellationToken);

        if (!userProfile.IsSuccess)
            return Result.Fail(userProfile.Errors.ToList());

        var insertUser = identityUser.Value.UserType switch
        {
            UserType.Representative => await _userRepository.InsertRepresentativeAsync(
                new InsertAndUpdateRepresentativeCommnd()
                {
                    UserId = Guid.Parse(identityUser.Value.Id),
                    Address = identityUser.Value.Address,
                    PhoneNumber = identityUser.Value.PhoneNumber,
                    Name = request.FirstName + " " + request.LastName,
                    BranchId = customer.BranchId.Value

                }, cancellationToken),

            UserType.Employee => await _userRepository.InsertEmployeeAsync(new InsertAndUpdateEmployeeCommnd()
            {
                UserId = Guid.Parse(identityUser.Value.Id),
                Address = identityUser.Value.Address,
                PhoneNumber = identityUser.Value.PhoneNumber,
                Name = request.FirstName + " " + request.LastName,
                BranchId = customer.BranchId.Value
            }, cancellationToken),
            _ => Result.Fail(new List<string>() { "حدثت مشكلة بالخادم الرجاء الاتصال بالدعم الفني" })
        };

        if (!insertUser.IsSuccess)
            return Result.Fail(insertUser.Errors.ToList());

        return identityUser.Value.Id;

    }
    
    public async Task<Result<string>> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken)
    {
        var user = await _identityRepository.ResetIdentityPassword(new ResetIdentityPassword()
        {
            UserName = request.UserName,
            NewPassword = request.NewPassword,
            ConfiramNewPassword = request.ConfiramNewPassword,
        }, cancellationToken);
        if (!user.IsSuccess)
            return Result.Fail(user.Errors.ToList());
        
        var resetPassword = await _userRepository.UpdatePasswordAsync(new UpdatePasswordCommnd()
        {
            UserName = request.UserName,
            NewPassword = request.NewPassword,
        }, cancellationToken);
        
        if (!resetPassword.IsSuccess)
            return Result.Fail(resetPassword.Errors.ToList());
        
        return "تم تغيير كلمة المرور بنجاح";
    }

    public async Task<Result<string>> ChangeUserActivationAsync(ChangeUserActivationRequest request, CancellationToken cancellationToken)
    {
        var user = await _identityRepository.GetIdentityUserById(request.UserId, cancellationToken);
        
        if (!user.IsSuccess)
            return Result.Fail(user.Errors.ToList());
        
        var changeUserActivation =
            await _identityRepository.ChangeIdentityActivation(new ChangeIdentityActivation()
            {
                UserId = user.Value.Id,
                State = request.State
            }, cancellationToken);
        
        if (!changeUserActivation.IsSuccess)
            return Result.Fail(changeUserActivation.Errors.ToList());

        var chengeActivation =
            await _userRepository.ChangeUserActivationAsync(new ChangeUserActivationCommnd()
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
        var user = await _identityRepository.GetIdentityUserById(request.UserId, cancellationToken);
        
        if (!user.IsSuccess)
            return Result.Fail(user.Errors.ToList());
        
        var updateUser = await _identityRepository.UpdateIdentityCustomer(new InsertAndUpdateIdentityUser()
        {
            UserId = user.Value.Id,
            FirstName = request.FristName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            Address = request.Address,
        },cancellationToken);
        
        if (!updateUser.IsSuccess)
            return Result.Fail(updateUser.Errors.ToList());

        var update = user.Value.UserType switch
        {
            UserType.Representative => await _userRepository.UpdateRepresentativeAsync(
                new InsertAndUpdateRepresentativeCommnd()
                {
                    UserId = Guid.Parse(updateUser.Value),
                    PhoneNumber = request.PhoneNumber,
                    Address = request.Address,
                    Name = request.FristName + " " + request.LastName
                }, cancellationToken),
            UserType.Employee => await _userRepository.UpdateEmployeeAsync(
                new InsertAndUpdateEmployeeCommnd()
                {
                    UserId = Guid.Parse(updateUser.Value),
                    PhoneNumber = request.PhoneNumber,
                    Address = request.Address,
                    Name = request.FristName + " " + request.LastName
                }, cancellationToken)
        };
        if (!update.IsSuccess)
            return Result.Fail(update.Errors.ToList());
        
        return "تم تعديل المستخدم بنجاح";

    }

    public async Task<Result<string>> UpdateCustomerAsync(UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        var customer = await _shippingDb.Customers.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(request.UserId), cancellationToken);

        if (customer != null)
        {
            var updateUser = await _identityRepository.UpdateIdentityCustomer(new InsertAndUpdateIdentityUser()
            {
                UserId = customer.UserId.ToString(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
            },cancellationToken);
        
            if (!updateUser.IsSuccess)
                return Result.Fail(updateUser.Errors.ToList());
            
            customer.PhoneNumber = request.PhoneNumber;
            customer.Address = request.Address;
            customer.Name = request.FirstName + " " + request.LastName;
        }

        var result = await _shippingDb.SaveChangesAsync(cancellationToken);
        if (result <= 0)
            Result.Fail("حدثت مشكلة في الخادم الرجاء الاتصال بالدعم الفني");
        
        return "تم تعديل بنجاح";
    }

    public async Task<Result<List<GetCustomersResponse>>> GetCustomersAsync(GetCustomersRequest request,
        CancellationToken cancellationToken)
    {
        var users = await _shippingDb.Customers
            .Include(x => x.User)
            .Select(x => new GetCustomersResponse()
            {
                UserId = x.UserId,
                UserName = x.User.UserName,
                ActivateState = x.User.ActivateState,
                UserType = x.User.UserType,
                Address = x.Address,
                PhoneNumber = x.PhoneNumber,
                Name = x.Name,
                Password = x.User.Password
            }).ToListAsync(cancellationToken);


        if (users.Count <= 0) return Result.Fail("لا يوجد زباين");

        return users;
    }

    public async Task<Result<List<GetUsersResponse>>> GetRepresentativesAsync(GetRepresentativesRequest request, CancellationToken cancellationToken)
    {
        var employee = await _shippingDb.Employees.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(request.UserId), cancellationToken);

        var users = await _shippingDb.Representatives
            .Where(x=>employee != null && x.BranchId == employee.BranchId)
            .Select(x => new GetUsersResponse()
            {
                UserId = x.UserId,
                UserName = x.User.UserName,
                ActivateState = x.User.ActivateState,
                UserType = x.User.UserType,
                Address = x.Address,
                PhoneNumber = x.PhoneNumber,
                Name = x.Name,
                Password = x.User.Password,
                BranchName = x.Branch.Name
            }).ToListAsync(cancellationToken);
        
        if (users.Count <= 0)
            return Result.Fail("لا يوجد مندوبين");

        return users;
    }

    public async Task<Result<List<GetUsersResponse>>> GetEmployeesAsync(GetEmployeesRequest request, CancellationToken cancellationToken)
    {
        var employee = await _shippingDb.Employees.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(request.UserId), cancellationToken);

        var users = await _shippingDb.Employees
            .Where(x=>employee != null && x.BranchId == employee.BranchId)
            .Select(x => new GetUsersResponse()
            {
                UserId = x.UserId,
                UserName = x.User.UserName,
                ActivateState = x.User.ActivateState,
                UserType = x.User.UserType,
                Address = x.Address,
                PhoneNumber = x.PhoneNumber,
                Name = x.Name,
                Password = x.User.Password,
                BranchName = x.Branch.Name
            }).ToListAsync(cancellationToken);
        
        if (users.Count <= 0)
            return Result.Fail("لا يوجد موظفين");

        return users;
    }

    public async Task<Result<List<GetUsersResponse>>> GetRepresentativesByBranchIdAsync(GetRepresentativesByBranchIdRequest request,
        CancellationToken cancellationToken)
    {
        if (request.BranchId == Guid.Empty)
            return await GetRepresentatives(cancellationToken);
        
        var users = await _shippingDb.Representatives
            .Where(x=>x.BranchId == request.BranchId)
            .Select(x => new GetUsersResponse()
            {
                UserId = x.UserId,
                UserName = x.User.UserName,
                ActivateState = x.User.ActivateState,
                UserType = x.User.UserType,
                Address = x.Address,
                PhoneNumber = x.PhoneNumber,
                Name = x.Name,
                Password = x.User.Password,
                BranchName = x.Branch.Name
            }).ToListAsync(cancellationToken);
        
        if (users.Count <= 0)
            return Result.Fail("لا يوجد مندوبين");

        return users;
    }

    public async Task<Result<List<GetUsersResponse>>> GetEmployeesByBranchIdAsync(GetEmployeesByBranchIdRequest request, CancellationToken cancellationToken)
    {
        if (request.BranchId == Guid.Empty)
            return await GetEmployees(cancellationToken);
        
        var users = await _shippingDb.Employees
            .Where(x=>x.BranchId == request.BranchId)
            .Select(x => new GetUsersResponse()
            {
                UserId = x.UserId,
                UserName = x.User.UserName,
                ActivateState = x.User.ActivateState,
                UserType = x.User.UserType,
                Address = x.Address,
                PhoneNumber = x.PhoneNumber,
                Name = x.Name,
                Password = x.User.Password,
                BranchName = x.Branch.Name
            }).ToListAsync(cancellationToken);
        
        if (users.Count <= 0)
            return Result.Fail("لا يوجد موظفين");

        return users;    }

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
                    if (y == x.PermissionId)
                        claim.Add(new Claim(t.RoleName, x.PermissionName));
                });
            });
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
            UserId = request.UserId,
            Claims = claims
        }, cancellationToken);
        
        if (!identityClims.IsSuccess)
            return Result.Fail(identityClims.Errors.ToList());

        var userClims = await _userRepository.CreateUserPermissions(new InsertAndUpdateUserPermissions()
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
                        claim.Add(new Claim(t.RoleName, x.PermissionName));
                });
            });
        }
        var claims = claim.GroupBy(x => x.Type).Select(y => new UserClaims 
        { 
            type = y.Key, 
            value = y.Select(x => x.Value).ToList() 
        }).ToList();
        
        var identityClims = await _identityRepository.UpdateIdentityUserClaims(new InsertAndUpdateIdentityClaims()
        {
            UserId = request.UserId.ToString(),
            Claims = claims
        }, cancellationToken);
        
        if (!identityClims.IsSuccess)
            return Result.Fail(identityClims.Errors.ToList());

        var userClims = await _userRepository.UpdateUserPermissions(new InsertAndUpdateUserPermissions()
        {
            UserId =   request.UserId.ToString(),
            Permissions = request.Permissions,
        }, cancellationToken);
        
        if (!userClims.IsSuccess)
            return Result.Fail(userClims.Errors.ToList());

        return " تمت تعديل صلاحيات المستخدم بنجاح ";
    }

    public async Task<Result<string>> DeleteUser(DeleteUserRequest request, CancellationToken cancellationToken)
    {
        var user = await _shippingDb.Users.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if(user == null)
            return Result.Fail("هذا المستخدم غير موجود");
        
        _shippingDb.Users.Remove(user);
        await _shippingDb.SaveChangesAsync(cancellationToken);
        
        return "تم الحذف";    
    }
    
    private async Task<Result<List<GetUsersResponse>>> GetRepresentatives(CancellationToken cancellationToken)
    {
        var users = await _shippingDb.Representatives
            .Select(x => new GetUsersResponse()
            {
                UserId = x.UserId,
                UserName = x.User.UserName,
                ActivateState = x.User.ActivateState,
                UserType = x.User.UserType,
                Address = x.Address,
                PhoneNumber = x.PhoneNumber,
                Name = x.Name,
                Password = x.User.Password,
                BranchName = x.Branch.Name
            }).ToListAsync(cancellationToken);
        
        if (users.Count <= 0)
            return Result.Fail("لا يوجد مندوبين");

        return users;
    }
    
    private async Task<Result<List<GetUsersResponse>>> GetEmployees(CancellationToken cancellationToken)
    {
        var users = await _shippingDb.Employees
            .Select(x => new GetUsersResponse()
            {
                UserId = x.UserId,
                UserName = x.User.UserName,
                ActivateState = x.User.ActivateState,
                UserType = x.User.UserType,
                Address = x.Address,
                PhoneNumber = x.PhoneNumber,
                Name = x.Name,
                Password = x.User.Password,
                BranchName = x.Branch.Name
            }).ToListAsync(cancellationToken);
        
        if (users.Count <= 0)
            return Result.Fail("لا يوجد موظفين");

        return users;
    }
}