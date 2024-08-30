using Shipping.Utils.Enums;

namespace Shipping.Utils.Helper;

public class PermissionAttribute : Attribute
{
    public PermissionNames[] Permissions { get; }

    public PermissionAttribute(params PermissionNames[] permissions)
    {
        Permissions = permissions;
    }
}