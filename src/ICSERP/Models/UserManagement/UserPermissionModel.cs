namespace ICSERP.Models.UserManagement
{
    public class UserPermissionModel
    {
        public int PersmissionId {get; set;} = 0;
        public string DocumentType {get; set;}

        public int DocumentAccessLevel {get; set;} = 0;


        public bool Create {get; set;} = false;

        public bool Update {get; set;} = false;

        public bool Read {get; set;} = false;

        public bool Delete {get; set;} = false;


        public bool Upload {get; set;} = false;

        public bool Download {get; set;} = false;

        public bool Approval {get; set;} = false;

        public bool Amend {get; set;}

        public bool Cancel {get; set;}

        public bool SetPermission {get; set;} = false;

        public int CompanyId {get;set;}
    }
}