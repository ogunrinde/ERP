using ICSERP.Models.UserManagement;

namespace ICSERP.Communication
{
    public class UserPermissionResponse : BaseResponse
    {
        public DirectUserPermissionModel _userpermission {get; private set;}

        public UserPermissionResponse(bool success, string message, DirectUserPermissionModel userpermission) : base(success, message)
        {
            _userpermission = userpermission;
        }

        public UserPermissionResponse(DirectUserPermissionModel userpermission) : this(true,string.Empty, userpermission)
        {

        }

        public UserPermissionResponse(string message) : this(false, message, null)
        {

        }
    }
}