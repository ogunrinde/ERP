using System.ComponentModel.DataAnnotations;

namespace ICSERP.Models.UserManagement
{
    public class RoleUserModel
    {

        [Required]
        public int RoleId {get; set;}

        [Required]
        public int UserId {get; set;}


    }
}