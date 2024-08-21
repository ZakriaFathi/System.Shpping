using System.Security.Claims;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shipping.Application.Abstracts;
using Shipping.Application.Models.IdentityModel;
using Shipping.Application.Models.UserManagement;
using Shipping.DataAccess.Persistence.DataBase;
using Shipping.Domain;
using Shipping.Domain.Entities;
using Shipping.Domain.Models;
using Shipping.Utils.Enums;

namespace Shipping.DataAccess.Repositories;

public class SherdUserRepository : ISherdUserRepository
{
    private readonly UserManager<User> _userManager;
    private readonly ShippingDbContext _shippingDb;
    public SherdUserRepository(UserManager<User> userManager, ShippingDbContext shippingDb)
    {
        _userManager = userManager;
        _shippingDb = shippingDb;
    }
    
    #region Identity

    public async Task<Result<User>> GetIdentityUserById(string userId, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return Result.Fail(new List<string>() { "هذا المستخدم غير موجود" });

        return user;
    }

    public async Task<Result<User>> GetIdentityUserByUserName(string userName, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user != null)
            return Result.Fail(new List<string>() { "هذا المستخدم موجود" });

        return user;
    }

    public async Task<Result<User>> SingUp(SingUpCommnd request, CancellationToken cancellationToken)
    {
        if (await _userManager.FindByNameAsync(request.UserName) is not null)
            return Result.Fail(new List<string>() { "اسم المستخدم موجود مسبقا" });
        
        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            Address = request.Address,
            UserName = request.UserName,
            UserType = UserType.User,
            ActivateState = ActivateState.Active,
        }; 
        await _userManager.CreateAsync(user, request.Password);
        
        // await _userManager.AddToRoleAsync(user, user.UserType.ToString("G"));
        
        if (request.Password.Length <= 7)
            return Result.Fail(new List<string>{ "كلمة المرور اقل من 8 " });
        
        return user;
    }

    public async Task<Result<User>> InsertIdentityUser(InsertAndUpdateIdentityUser command, CancellationToken cancellationToken)
    {
        await _userManager.FindByNameAsync(command.UserName);
        
        var user = new User
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            PhoneNumber = command.PhoneNumber,
            Email = command.Email,
            Address = command.Address,
            UserName = command.UserName,
            UserType = command.UserType,
            ActivateState = command.ActivateState,
        };
        
        if (command.Password.Length <= 7)
            return Result.Fail(new List<string> { "كلمة المرور اقل من 8 " });
        
        await _userManager.CreateAsync(user, command.Password);
        
        // await _userManager.AddToRoleAsync(user, user.UserType.ToString("G"));

        return user;
    }

    public async Task<Result<string>> UpdateIdentityCustomer(InsertAndUpdateIdentityUser command, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(command.UserId);
        if  (user is null)
            return Result.Fail(new List<string>() { "المستخدم غير موجود" });
        
        user.FirstName = command.FirstName;
        user.LastName = command.LastName;
        user.PhoneNumber = command.PhoneNumber;
        user.Email = command.Email;
        user.Address = command.Address;
        user.UserName = command.UserName;
        user.UserType = command.UserType;
        
        var response = await _userManager.UpdateAsync(user);
        
        if (response.Succeeded)
            return user.Id;

        return Result.Fail("حدثت مشكلة بالخادم الرجاء الاتصال بالدعم الفني");    
    }

    public async Task<Result<string>> ChangeIdentityPassword(ChangeIdentityPassword command, CancellationToken cancellationToken)
    {
        var identity = await GetIdentityUserById(command.UserId, cancellationToken);
        if (identity.IsFailed)
            return Result.Fail(identity.Errors.ToList());
        
        var user = identity.Value;
        
        var password = await _userManager.CheckPasswordAsync(user, command.OldPassword);
        if (password == false)
            return Result.Fail(new List<string>() { "كلمة المرور السابقة غير صحيحة" });
        
        if (command.NewPassword != command.ConfirmNewPassWord)
            return Result.Fail(new List<string>() { "كلمة المرور غير متطابقة" });

        var result = await _userManager.ChangePasswordAsync(user, command.OldPassword, command.NewPassword);
        if (result.Succeeded)
            return "تم تغيير كلمة المرور بنجاح";

        return Result.Fail("حدثت مشكلة في الخادم الرجاء الاتصال بالدعم الفني");
    }

    public async Task<Result<string>> ChangeIdentityActivation(ChangeIdentityActivation command, CancellationToken cancellationToken)
    {
        var identity = await GetIdentityUserById(command.UserId, cancellationToken);
        if (identity.IsFailed)
            return Result.Fail(identity.Errors.ToList());
        
        var user = identity.Value;    
        
        user.ActivateState = command.State;
        var result = await _userManager.UpdateAsync(user);

        return result.Succeeded ?
            "تم تغيير حالة المستخدم بنجاح" :
            Result.Fail("حدثت مشكلة بالخادم الرجاء الاتصال بالدعم الفني");
    }

    public async Task<Result<string>> InsertIdentityUserClaims(InsertAndUpdateIdentityClaims command, CancellationToken cancellationToken)
    {
        var identity = await GetIdentityUserById(command.UserId, cancellationToken);
        if (identity.IsFailed)
            return Result.Fail(identity.Errors.ToList());
        
        var user = identity.Value;   
        
        var claims = new List<Claim>();
        command.Claims.ForEach(item =>
        {
            item.value.ForEach(value => claims.Add(new Claim(item.type, value)));

        });

        var result = await _userManager.AddClaimsAsync(user, claims);

        if (result.Succeeded)
            return user.Id;

        return Result.Fail("حدثت مشكلة في الخادم الرجاء الاتصال بالدعم الفني");
    }

    public async Task<Result<string>> UpdateIdentityUserClaims(InsertAndUpdateIdentityClaims command, CancellationToken cancellationToken)
    {
        var identity = await GetIdentityUserById(command.UserId, cancellationToken);
        if (identity.IsFailed)
            return Result.Fail(identity.Errors.ToList());
        
        var user = identity.Value;
    
        var existingClaims = await _userManager.GetClaimsAsync(user);

        foreach (var claim in existingClaims)
        {
            await _userManager.RemoveClaimAsync(user, claim);
        }

        var claims = new List<Claim>();
        command.Claims.ForEach(item =>
        {
            item.value.ForEach(value => claims.Add(new Claim(item.type, value)));

        });
        var result = await _userManager.AddClaimsAsync(user, claims);

        if (result.Succeeded)
            return user.Id;

        return Result.Fail("حدثت مشكلة في الخادم الرجاء الاتصال بالدعم الفني");
    }

    public async Task<Result<Customer>> GetUserById(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _shippingDb.Customers.FirstOrDefaultAsync(x => x.Id == userId,
            cancellationToken);
        if (user is null)
            return Result.Fail(new List<string>() { "هذا المستخدم غير موجود" });

        return user;    
    }

    public async Task<Result<string>> InsertUserAsync(InsertAndUpdateUserCommnd request, CancellationToken cancellationToken)
    {
        var customer = await _shippingDb.Customers.FirstOrDefaultAsync(x => x.Id == Guid.Parse(request.UserId.ToString()),
            cancellationToken);
        if (customer is not null)
            return Result.Fail(new List<string>() { "هذا المستخدم موجود بالفعل" });
        
        var newUser = new Customer()
        {
            Id = Guid.Parse(request.UserId.ToString()),
            PhoneNumber = request.PhoneNumber,
            Address = request.Address,
            UserName = request.UserName,
            UserType = request.UserType,
            Password = request.Password,
            ActivateState = ActivateState.Active,
            Name = request.Name,
            BranchId = request.BranchId 
        };
        await _shippingDb.Customers.AddAsync(newUser, cancellationToken);
        var result = await _shippingDb.SaveChangesAsync(cancellationToken);
        
        if (result <= 0)
            return Result.Fail(new List<string>() { "حدثت مشكلة بالخادم الرجاء الاتصال بالدعم الفني" });
        
        return newUser.Id.ToString();    
    } 
    public async Task<Result<string>> InsertCustomerAsync(InsertAndUpdateUserCommnd request, CancellationToken cancellationToken)
    {
        var customer = await _shippingDb.Customers.FirstOrDefaultAsync(x => x.Id == Guid.Parse(request.UserId.ToString()),
            cancellationToken);
        if (customer is not null)
            return Result.Fail(new List<string>() { "هذا المستخدم موجود بالفعل" });
        
        var newUser = new Customer()
        {
            Id = Guid.Parse(request.UserId.ToString()),
            PhoneNumber = request.PhoneNumber,
            Address = request.Address,
            UserName = request.UserName,
            UserType = request.UserType,
            Password = request.Password,
            ActivateState = ActivateState.Active,
            Name = request.Name
        };
        await _shippingDb.Customers.AddAsync(newUser, cancellationToken);
        var result = await _shippingDb.SaveChangesAsync(cancellationToken);
        
        if (result <= 0)
            return Result.Fail(new List<string>() { "حدثت مشكلة بالخادم الرجاء الاتصال بالدعم الفني" });
        
        return newUser.Id.ToString();    
    }

    public async Task<Result<string>> ChangePasswordCustomerAsync(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var customer = await GetUserById(request.UserId, cancellationToken);
        if (customer.Value is null)
            return Result.Fail(customer.Errors.ToList());
        
        var user = customer.Value;
        
        if (user.Password != request.OldPassWord)
            return Result.Fail(new List<string>() { "كلمة المرور السابقة غير صحيحة" });

        if (request.NewPassWord != request.ConfirmNewPassWord)
            return Result.Fail(new List<string>() { "كلمة المرور غير متطابقة" });
        
        user.Password = request.NewPassWord;
        var result = await _shippingDb.SaveChangesAsync(cancellationToken);

        if (result <= 0)
            Result.Fail("حدثت مشكلة في الخادم الرجاء الاتصال بالدعم الفني");
       
        return "تم تغيير كلمة المرور بنجاح";    
    }

    public async Task<Result<string>> ChangeCustomerActivationAsync(ChangeUserActivationCommnd request, CancellationToken cancellationToken)
    {
        var customer = await GetUserById(request.UserId, cancellationToken);
        if (customer.Value is null)
            return Result.Fail(customer.Errors.ToList());
        
        var user = customer.Value;
        
        user.ActivateState = request.State;
        var result = await _shippingDb.SaveChangesAsync(cancellationToken);
        if (result <= 0)
            Result.Fail("حدثت مشكلة في الخادم الرجاء الاتصال بالدعم الفني");
        
        return "تم تغيير حالة المستخدم بنجاح";  
    }

    public async Task<Result<string>> UpdateCustomerAsync(InsertAndUpdateUserCommnd request, CancellationToken cancellationToken)
    {
        var customer = await GetUserById(request.UserId, cancellationToken);
        if (customer.Value is null)
            return Result.Fail(customer.Errors.ToList());
        
        var user = customer.Value;    

        
        user.UserName = request.UserName;
        user.PhoneNumber = request.PhoneNumber;
        user.Address = request.Address;
        user.Name = request.Name;

        var result = await _shippingDb.SaveChangesAsync(cancellationToken);
        if (result <= 0)
            Result.Fail("حدثت مشكلة في الخادم الرجاء الاتصال بالدعم الفني");
        
        return "تم تعديل المشترك بنجاح";
    }

    public async Task<Result<string>> CreateUserPermissions(InsertAndUpdateUserPermissions request, CancellationToken cancellationToken)
    {
        var userPermissions = request.Permissions.Select(
            permission => new UserPermission()
            {
                CustomerId = Guid.Parse(request.UserId),
                PermissionId = Guid.Parse(permission),
            }).ToList();

        await _shippingDb.UserPermissions.AddRangeAsync(userPermissions, cancellationToken);
        var result = await _shippingDb.SaveChangesAsync(cancellationToken);

        if (result <= 0)
            return Result.Fail(new List<string>() { "حدثت مشكلة بالخادم الرجاء الاتصال بالدعم الفني" });
        
        return "تمت عملية اضافة المستخدم بنجاح ";        }

    public async Task<Result<string>> UpdateUserPermissions(InsertAndUpdateUserPermissions request, CancellationToken cancellationToken)
    {
        var userPermissions = request.Permissions.Select(
            permission => new UserPermission()
            {
                CustomerId = Guid.Parse(request.UserId),
                PermissionId = Guid.Parse(permission),
            }).ToList();

        await _shippingDb.UserPermissions.AddRangeAsync(userPermissions, cancellationToken);
        var result = await _shippingDb.SaveChangesAsync(cancellationToken);

        if (result <= 0)
            return Result.Fail(new List<string>() { "حدثت مشكلة بالخادم الرجاء الاتصال بالدعم الفني" });
        
        return "تمت عملية اضافة المستخدم بنجاح ";
    }

    #endregion
}