using FluentResults;
using Shipping.Application.Features.Auth.Commands.UpdateCustomer;
using Shipping.Application.Features.UserManagement.Users.Commands.ChangePassword;
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

namespace Shipping.Application.Abstracts;

public interface IUserManagmentRepository
{
    Task<Result<string>> CreateUserAsync(CreateUserRequest request, CancellationToken cancellationToken);
    Task<Result<string>> ChangePasswordAsync(ChangePasswordRequest request, CancellationToken cancellationToken);

    Task<Result<string>> ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken);
    Task<Result<string>> ChangeUserActivationAsync(ChangeUserActivationRequest request, CancellationToken cancellationToken);
    Task<Result<string>> UpdateUserAsync(UpdateUserRequest request, CancellationToken cancellationToken);
    Task<Result<string>> UpdateCustomerAsync(UpdateCustomerRequest request, CancellationToken cancellationToken);
    Task<Result<List<GetCustomersResponse>>> GetCustomersAsync(GetCustomersRequest request, CancellationToken cancellationToken);
    Task<Result<List<GetUsersResponse>>> GetRepresentativesAsync(GetRepresentativesRequest request, CancellationToken cancellationToken);
    Task<Result<List<GetUsersResponse>>> GetEmployeesAsync(GetEmployeesRequest request, CancellationToken cancellationToken);
    Task<Result<List<GetUsersResponse>>> GetRepresentativesByBranchIdAsync(GetRepresentativesByBranchIdRequest request, CancellationToken cancellationToken);
    Task<Result<List<GetUsersResponse>>> GetEmployeesByBranchIdAsync(GetEmployeesByBranchIdRequest request, CancellationToken cancellationToken);
    Task<Result<string>> CreateUserPermissionsAsync(CreateUserPermissionsRequest request, CancellationToken cancellationToken);
    Task<Result<string>> UpdateUserPermissionsAsync(UpdateUserPermissionsRequest request, CancellationToken cancellationToken);
    Task<Result<string>> DeleteUser(DeleteUserRequest request, CancellationToken cancellationToken);

}