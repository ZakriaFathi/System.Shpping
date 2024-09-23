using Shipping.Utils.Helper;

namespace Shipping.Utils.Enums;

public enum RoleName
{
    [Permission(PermissionNames.View,PermissionNames.Create, PermissionNames.Edit ,PermissionNames.Delete)]
    UserManagement,
    
    [Permission(PermissionNames.View,PermissionNames.Create, PermissionNames.Edit ,PermissionNames.Delete)]
    BranchManagement,
    
    [Permission(PermissionNames.View,PermissionNames.Create, PermissionNames.Edit ,PermissionNames.Delete)]
    CityManagement,
    
    [Permission(PermissionNames.View, PermissionNames.Edit ,PermissionNames.Delete)]
    OrderManagement,
    
    [Permission(PermissionNames.View, PermissionNames.Delete)]
    PermissionManagement,
    
}

public enum RoleType
{
    UserManagement,
    OrderManagement, 
    CityManagement,
    PermissionManagement,
    BranchManagement
}