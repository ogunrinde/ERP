using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ICSERP.Entities.UserManagament
{
    public class Role
    {
        public int RoleId  { get; set; }

        [Required]
        public string RoleName  { get; set; }

        public string Description  { get; set; }

        public bool Is_deleted {get; set;} = false;
        public virtual ICollection<UserRole> UserRoles { get; set; }

        public virtual ICollection<RolePermission> RolePermissions { get; set; }

        public int CompanyId { get; set; }

        public virtual Company Company {get; set;}
        public DateTime DateCreated {get; set;}
    }
}