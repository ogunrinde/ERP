namespace ICSERP.Models.UserManagement
{
    public class ReturnModel
    {
        public bool Status {get; set;}
        
        public string Message {get; set;}

        public ReturnRoleModel Role {get; set;} = null;
    }
}