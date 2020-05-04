using System.ComponentModel.DataAnnotations;

namespace ICSERP.Models.UserManagement
{
    public class ReturnRoleModel
    {
        public int RoleId {get; set;}
        [Required]
        public string RoleName  { get; set; }

        public string Description  { get; set; }
    }
}