using ICSERP.Entities.UserManagament;

namespace ICSERP.Communication
{
    public class ProfileResponse: BaseResponse
    {
        public PersonalInformation _profile {get; private set;}

        public ProfileResponse(bool success, string message, PersonalInformation profile) : base(success, message)
        {
            _profile = profile;
        }

        public ProfileResponse(PersonalInformation profile) : this(true,string.Empty, profile)
        {

        }

        public ProfileResponse(string message) : this(false, message, null)
        {

        }
    }
}