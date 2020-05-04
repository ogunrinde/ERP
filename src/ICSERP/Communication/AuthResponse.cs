using ICSERP.Entities.UserManagament;

namespace ICSERP.Communication
{
    public class AuthResponse: BaseResponse
    {
        public User _user {get; private set;}

        public AuthResponse(bool success, string message, User user) : base(success, message)
        {
            _user = user;
        }

        public AuthResponse(User user) : this(true,string.Empty, user)
        {

        }

        public AuthResponse(string message) : this(false, message, null)
        {

        }
    }
}