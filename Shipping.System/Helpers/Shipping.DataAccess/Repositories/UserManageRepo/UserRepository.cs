using FluentResults;
using Microsoft.EntityFrameworkCore;
using Shipping.Application.Abstracts;
using Shipping.Application.Models.UserManagement;
using Shipping.DataAccess.Persistence.DataBase;
using Shipping.Domain.Entities;
using Shipping.Utils.Enums;

namespace Shipping.DataAccess.Repositories.UserManageRepo;

public class UserRepository : IUserRepository
{
    private readonly ShippingDbContext _shippingDb;

    public UserRepository(ShippingDbContext shippingDb)
    {
        _shippingDb = shippingDb;
    }
    
    public async Task<Result<string>> InsertUserAsync(InsertAndUpdateUserCommnd request, CancellationToken cancellationToken)
    {
        var user = await _shippingDb.Users.FirstOrDefaultAsync(x => x.Id == Guid.Parse(request.UserId.ToString()),
            cancellationToken);
        if (user is not null)
            return Result.Fail(new List<string>() { "هذا المستخدم موجود بالفعل" });
        
        var newUser = new User()
        {
            Id = Guid.Parse(request.UserId.ToString()),
            UserName = request.UserName,
            UserType = request.UserType,
            Password = request.Password,
            ActivateState = ActivateState.Active,
        };
        await _shippingDb.Users.AddAsync(newUser, cancellationToken);
        var result = await _shippingDb.SaveChangesAsync(cancellationToken);
        
        if (result <= 0)
            return Result.Fail(new List<string>() { "حدثت مشكلة بالخادم الرجاء الاتصال بالدعم الفني" });
        
        return newUser.Id.ToString();    
    }

    public async Task<Result<string>> InsertEmployeeAsync(InsertAndUpdateEmployeeCommnd request, CancellationToken cancellationToken)
    {
        var user = await _shippingDb.Employees.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(request.UserId.ToString()),
            cancellationToken);
        if (user is not null)
            return Result.Fail(new List<string>() { "هذا المستخدم موجود بالفعل" });
        
        var newUser = new Employee()
        {
            UserId = Guid.Parse(request.UserId.ToString()),
            PhoneNumber = request.PhoneNumber,
            Address = request.Address,
            Name = request.Name,
            BranchId = request.BranchId
        };
        await _shippingDb.Employees.AddAsync(newUser, cancellationToken);
        var result = await _shippingDb.SaveChangesAsync(cancellationToken);
        
        if (result <= 0)
            return Result.Fail(new List<string>() { "حدثت مشكلة بالخادم الرجاء الاتصال بالدعم الفني" });
        
        return newUser.Id.ToString();  
    }

    public async Task<Result<string>> InsertRepresentativeAsync(InsertAndUpdateRepresentativeCommnd request, CancellationToken cancellationToken)
    {
        var user = await _shippingDb.Representatives.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(request.UserId.ToString()),
            cancellationToken);
        if (user is not null)
            return Result.Fail(new List<string>() { "هذا المستخدم موجود بالفعل" });
        
        var newUser = new Representative()
        {
            UserId = Guid.Parse(request.UserId.ToString()),
            PhoneNumber = request.PhoneNumber,
            Address = request.Address,
            Name = request.Name,
            BranchId = request.BranchId
        };
        await _shippingDb.Representatives.AddAsync(newUser, cancellationToken);
        var result = await _shippingDb.SaveChangesAsync(cancellationToken);
        
        if (result <= 0)
            return Result.Fail(new List<string>() { "حدثت مشكلة بالخادم الرجاء الاتصال بالدعم الفني" });
        
        return newUser.Id.ToString();      
    }

    public async Task<Result<string>> InsertCustomerAsync(InsertAndUpdateCustomerCommnd request, CancellationToken cancellationToken)
    {
        var user = await _shippingDb.Customers.FirstOrDefaultAsync(x => x.UserId == Guid.Parse(request.UserId.ToString()),
            cancellationToken);
        if (user is not null)
            return Result.Fail(new List<string>() { "هذا المستخدم موجود بالفعل" });
        
        var newUser = new Customer()
        {
            UserId = Guid.Parse(request.UserId.ToString()),
            PhoneNumber = request.PhoneNumber,
            Address = request.Address,
            Name = request.Name,
        };
        await _shippingDb.Customers.AddAsync(newUser, cancellationToken);
        var result = await _shippingDb.SaveChangesAsync(cancellationToken);
        
        if (result <= 0)
            return Result.Fail(new List<string>() { "حدثت مشكلة بالخادم الرجاء الاتصال بالدعم الفني" });
        
        return newUser.Id.ToString();      
    }
    public async Task<Result<string>> ChangeUserActivationAsync(ChangeUserActivationCommnd request, CancellationToken cancellationToken)
    {
        var user = await _shippingDb.Users.FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);
        if (user == null)
            return Result.Fail(new List<string>() { "المستخدم غير موجود" });
        
        user.ActivateState = request.State;

        var result = await _shippingDb.SaveChangesAsync(cancellationToken);
        if (result <= 0)
            Result.Fail("حدثت مشكلة في الخادم الرجاء الاتصال بالدعم الفني");
        
        return "تم تغيير حالة المستخدم بنجاح";
    }
    public async Task<Result<string>> ChangePassword(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _shippingDb.Users.FirstOrDefaultAsync(x=>
            x.Id == request.UserId, cancellationToken);
        if (user is null)
            return Result.Fail("هذا المستخدم غير موجود" );
        
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

    public async Task<Result<string>> UpdatePasswordAsync(UpdatePasswordCommnd request, CancellationToken cancellationToken)
    {
        var user = await _shippingDb.Users.FirstOrDefaultAsync(x => x.UserName == request.UserName, cancellationToken);
        if (user == null)
            return Result.Fail(new List<string>() { "المستخدم غير موجود" });    
        
        user.Password = request.NewPassword;
        var result = await _shippingDb.SaveChangesAsync(cancellationToken);
        if (result <= 0)
            Result.Fail("حدثت مشكلة في الخادم الرجاء الاتصال بالدعم الفني");
        
        return "تم تعديل كلمة المرور بنجاح";
    }

    public async Task<Result<string>> UpdateCustomerAsync(InsertAndUpdateCustomerCommnd request, CancellationToken cancellationToken)
    {
        var user = await _shippingDb.Customers.FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);
        if (user == null)
            return Result.Fail(new List<string>() { "المستخدم غير موجود" });
        
        user.PhoneNumber = request.PhoneNumber;
        user.Address = request.Address;
        user.Name = request.Name;
        
        var result = await _shippingDb.SaveChangesAsync(cancellationToken);
        if (result <= 0)
            Result.Fail("حدثت مشكلة في الخادم الرجاء الاتصال بالدعم الفني");
        
        return "تم تعديل المشترك بنجاح";
    }

    public async Task<Result<string>> UpdateRepresentativeAsync(InsertAndUpdateRepresentativeCommnd request, CancellationToken cancellationToken)
    {
        var user = await _shippingDb.Representatives.FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);
        if (user == null)
            return Result.Fail(new List<string>() { "المستخدم غير موجود" });
        
        user.PhoneNumber = request.PhoneNumber;
        user.Address = request.Address;
        user.Name = request.Name;
        
        var result = await _shippingDb.SaveChangesAsync(cancellationToken);
        if (result <= 0)
            Result.Fail("حدثت مشكلة في الخادم الرجاء الاتصال بالدعم الفني");
        
        return "تم تعديل المشترك بنجاح";    
    }

    public async Task<Result<string>> UpdateEmployeeAsync(InsertAndUpdateEmployeeCommnd request, CancellationToken cancellationToken)
    {
        var user = await _shippingDb.Employees.FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);
        if (user == null)
            return Result.Fail(new List<string>() { "المستخدم غير موجود" });
        
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
                PermissionId = permission,
            }).ToList();

        await _shippingDb.UserPermissions.AddRangeAsync(userPermissions, cancellationToken);
        var result = await _shippingDb.SaveChangesAsync(cancellationToken);

        if (result <= 0)
            return Result.Fail(new List<string>() { "حدثت مشكلة بالخادم الرجاء الاتصال بالدعم الفني" });
        
        return "تمت عملية اضافة المستخدم بنجاح ";        
    }

    public async Task<Result<string>> UpdateUserPermissions(InsertAndUpdateUserPermissions request, CancellationToken cancellationToken)
    {
        var userPermissions = request.Permissions.Select(
            permission => new UserPermission()
            {
                CustomerId = Guid.Parse(request.UserId),
                PermissionId = permission,
            }).ToList();

        await _shippingDb.UserPermissions.AddRangeAsync(userPermissions, cancellationToken);
        var result = await _shippingDb.SaveChangesAsync(cancellationToken);

        if (result <= 0)
            return Result.Fail(new List<string>() { "حدثت مشكلة بالخادم الرجاء الاتصال بالدعم الفني" });
        
        return "تمت عملية اضافة المستخدم بنجاح ";
    }
}