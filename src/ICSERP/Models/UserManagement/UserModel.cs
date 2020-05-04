using ICSERP.Entities.UserManagament;

namespace ICSERP.Models.UserManagement
{
    public class UserModel
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string UserType {get; set;}
        public bool Is_Active { get; set; }

        public Role Role {get; set;}

        public int CompanyId {get; set;}
    }
}