using System.Security.Claims;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using Shipping.Application.Abstracts;
using Shipping.Application.Models.IdentityModel;
using Shipping.Domain.Models;
using Shipping.Utils.Enums;

namespace Shipping.DataAccess.Repositories.UserManageRepo;

public class IdentityRepository : IIdentityRepository
{
    private readonly UserManager<AppUser> _userManager;

    public IdentityRepository(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

   public async Task<Result<AppUser>> GetIdentityUserById(string userId, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return Result.Fail(new List<string>() { "هذا المستخدم غير موجود" });

        return user;
    }

    public async Task<Result<string>> GetIdentityUserByUserName(string userName, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user != null)
            return Result.Fail(new List<string>() { "هذا المستخدم موجود" });

        return Result.Ok();
    }

    public async Task<Result<string>> ResetIdentityPassword(ResetIdentityPassword command, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(command.UserName);
        if (user == null)
            return Result.Fail(new List<string>() { "هذا المستخدم غير موجود" });

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        if (command.NewPassword != command.ConfiramNewPassword)
            return Result.Fail(new List<string>() { "كلمة المرور غير متطابقة" });

        var result = await _userManager.ResetPasswordAsync(user, token, command.NewPassword);
        
        return result.Succeeded ?
            "تم تغيير كلمة المرور بنجاح" :
            Result.Fail("حدثت مشكلة بالخادم الرجاء الاتصال بالدعم الفني");
    }

    public async Task<Result<AppUser>> SingUp(SingUpCommnd request, CancellationToken cancellationToken)
    {
        if (await _userManager.FindByNameAsync(request.UserName) is not null)
            return Result.Fail(new List<string>() { "اسم المستخدم موجود مسبقا" });
        
        var user = new AppUser
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
        
        if (request.Password.Length <= 7)
            return Result.Fail(new List<string>{ "كلمة المرور اقل من 8 " });
        
        await _userManager.CreateAsync(user, request.Password);
        
        await _userManager.AddToRoleAsync(user, user.UserType.ToString("G"));
        
        return user;
    }

    public async Task<Result<AppUser>> InsertIdentityUser(InsertAndUpdateIdentityUser command, CancellationToken cancellationToken)
    {
        await _userManager.FindByNameAsync(command.UserName);
        
        var user = new AppUser
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
        
        await _userManager.AddToRoleAsync(user, user.UserType.ToString("G"));

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
        user.Address = command.Address;
        
        var result = await _userManager.UpdateAsync(user);
        
        return result.Succeeded ?
            user.Id :
            Result.Fail("حدثت مشكلة بالخادم الرجاء الاتصال بالدعم الفني");    
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

        return result.Succeeded ?
            user.Id :
            Result.Fail("حدثت مشكلة بالخادم الرجاء الاتصال بالدعم الفني");
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

        return result.Succeeded ?
            user.Id :
            Result.Fail("حدثت مشكلة بالخادم الرجاء الاتصال بالدعم الفني");
    }

    public async Task<Result<string>> DeleteUser(string userId, CancellationToken cancellationToken)
    {
        var identity = await GetIdentityUserById(userId, cancellationToken);
        if (identity.IsFailed)
            return Result.Fail(identity.Errors.ToList());
        
        var user = identity.Value;

        if (user.UserType == UserType.Owner)
            return Result.Fail("لا يمكن حذف مستخدم مالك الشركة");
        
        var result = await _userManager.DeleteAsync(user);
        
        return result.Succeeded ?
            user.Id :
            Result.Fail("حدثت مشكلة بالخادم الرجاء الاتصال بالدعم الفني");
    }
}