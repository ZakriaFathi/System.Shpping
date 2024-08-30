namespace Shipping.Utils.Enums;

[Flags]
public enum PermissionNames
{
    #region User Management

    GetCustomers = 1,
    GetRepresentatives,
    GetRepresentativesByBranchId,
    GetEmployees,
    GetEmployeesByBranchId,
    CreateUser,
    ChangeUserActivation,
    ResetPassword,
    CreateUserPermissions,
    UpdateUserPermissions,
    UpdateUser,
    DeleteUser = 12,

    #endregion

    #region Order Management

    CreateOrder = 13,
    AcceptanceOrder ,
    ChangeOrderStateByEmployee ,
    InsertRepresentativeInOrde ,    
    ChangeOrderStateByRepresentative ,
    GetOrderByCustomer,
    GetOrderByRepresentative,
    GetOrderByBranchId,
    ShearchOrder,
    DeleteOrder,

    #endregion#
    
    #region City Management

    CreateCity = 23 ,
    UpdateCity,
    GetCitiesByBranchId,
    GetCities,
    DeleteCity,

    #endregion 
    
    #region Permission Management

    GetAllPermissions = 28 ,
    GetAllPermissionsByRoleId,
    DeletePermission,

    #endregion 
    
    #region Role Management

    GetAllRoles = 31 ,
    GetRolesByUserId,
    DeleteRole,

    #endregion
}