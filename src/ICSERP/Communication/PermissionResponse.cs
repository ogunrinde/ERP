using ICSERP.Models.UserManagement;

namespace ICSERP.Communication
{
    public class PermissionResponse : BaseResponse
    {
        public RolePermissionModel _rolepermission {get; private set;}

        public PermissionResponse(bool success, string message, RolePermissionModel rolepermission) : base(success, message)
        {
            _rolepermission = rolepermission;
        }

        public PermissionResponse(RolePermissionModel rolepermission) : this(true,string.Empty, rolepermission)
        {

        }

        public PermissionResponse(string message) : this(false, message, null)
        {

        }
    }
}