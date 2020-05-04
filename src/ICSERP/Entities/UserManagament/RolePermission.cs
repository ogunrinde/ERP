using System;

namespace ICSERP.Entities.UserManagament
{
    public class RolePermission
    {
        public int RoleId {get; set;}

        public virtual Role Roles {get; set;}

        public int PermissionId {get; set;}

        public virtual Permission Permissions {get; set;}

        public int CompanyId {get; set;}

        public DateTime DateCreated {get; set;}
    }
}