using ICSERP.Entities.UserManagament;

namespace ICSERP.Models.UserManagement
{
    public class RolePermissionModel
    {
        public RolePerm[] RolePerms {get; set;}
    }

    public class DirectUserPermissionModel
    {
        public UserPerm[] UserPerms {get; set;}
    }

    

    public class RolePerm 
    {
        public int RoleId {get;set;}

        public string Documentname {get; set;}

        public bool Success {get; set;} = false;

        public string Message {get; set;} = "";
        public UserPermissionModel Permission {get;set;}
    }

    public class UserPerm 
    {
        public int UserId {get;set;}

        public string Documentname {get; set;}

        public bool Success {get; set;} = false;

        public string Message {get; set;} = "";

        public UserPermissionModel Permission {get;set;}
    }

    
}