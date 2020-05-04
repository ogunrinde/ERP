using System.ComponentModel.DataAnnotations;

namespace ICSERP.Models.UserManagement
{
    public class EmployeeRegisterModel
    {
        [Required]
        public string Email {get; set;}

        [Required]
        public string FirstName {get; set;}

        [Required]
        public string LastName {get;set;}


        [Required]
        public string Gender {get; set;}
        
    }
}