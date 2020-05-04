using System;

namespace ICSERP.Entities.UserManagament
{
    public class UserRole
    {
        public int? UserId {get; set;}

        public virtual User Users {get; set;}

        public int? RoleId {get; set;}

        public virtual Role Roles {get;set;}

        public int CompanyId {get; set;}

        public DateTime DateCreated {get; set;}

        public bool Is_deleted {get; set;} = false;
    }
}