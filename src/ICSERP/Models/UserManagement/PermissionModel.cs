using System.Collections;

namespace ICSERP.Models.UserManagement
{
    public class PermissionModel
    {
        public bool Status {get; set;}
        
        public string DocType {get; set;}

        public string AccessType {get; set;}

        public IList Roles  {get; set;}

        public int CompanyId {get; set;}

        public int UserId {get;set;}

        public int DocumentAccessLevel {get; set;}
    }
}