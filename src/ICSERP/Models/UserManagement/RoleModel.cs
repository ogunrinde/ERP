using System.ComponentModel.DataAnnotations;

namespace ICSERP.Models.UserManagement
{
    public class RoleModel
    {

        [Required]
        public string RoleName  { get; set; }

        public string Description  { get; set; }
    }
}