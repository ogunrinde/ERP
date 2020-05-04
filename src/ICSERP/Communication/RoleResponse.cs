using ICSERP.Entities.UserManagament;

namespace ICSERP.Communication
{
    public class RoleResponse: BaseResponse
    {
        public Role _role {get; private set;}

        public RoleResponse(bool success, string message, Role role) : base(success, message)
        {
            _role = role;
        }

        public RoleResponse(Role role) : this(true,string.Empty, role)
        {

        }

        public RoleResponse(string message) : this(false, message, null)
        {

        }
    }
}