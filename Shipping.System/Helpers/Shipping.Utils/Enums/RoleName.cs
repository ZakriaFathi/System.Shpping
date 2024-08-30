using Shipping.Utils.Helper;

namespace Shipping.Utils.Enums;

public enum RoleName
{
    [Permission(PermissionNames.GetCustomers, PermissionNames.GetRepresentatives,
        PermissionNames.GetRepresentativesByBranchId, PermissionNames.GetEmployees,
        PermissionNames.GetEmployeesByBranchId, PermissionNames.CreateUser,
        PermissionNames.ChangeUserActivation, PermissionNames.ResetPassword,
        PermissionNames.CreateUserPermissions, PermissionNames.UpdateUserPermissions,
        PermissionNames.UpdateUser, PermissionNames.DeleteUser)]
    UserManagement,
    
    
    [Permission(PermissionNames.CreateOrder, PermissionNames.AcceptanceOrder, PermissionNames.ChangeOrderStateByEmployee,
        PermissionNames.InsertRepresentativeInOrde, PermissionNames.ChangeOrderStateByRepresentative,
        PermissionNames.GetOrderByCustomer, PermissionNames.GetRepresentatives, PermissionNames.GetOrderByRepresentative,
        PermissionNames.GetOrderByBranchId, PermissionNames.ShearchOrder,
        PermissionNames.DeleteOrder)]
    OrderManagement,
    
    [Permission(PermissionNames.CreateCity, PermissionNames.UpdateCity, PermissionNames.GetCitiesByBranchId,
        PermissionNames.GetCities,PermissionNames.DeleteCity)]
    CityManagement,
    
    [Permission(PermissionNames.GetAllPermissions, PermissionNames.GetAllPermissionsByRoleId, PermissionNames.DeletePermission)]
    PermissionManagement,
    
    
    [Permission(PermissionNames.GetAllRoles, PermissionNames.GetRolesByUserId, PermissionNames.DeleteRole)]
    RoleManagement
}