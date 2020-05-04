using System.ComponentModel.DataAnnotations;

namespace ICSERP.Models.UserManagement
{
    public class RegisterModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        public string UserType {get; set;}

        [Required]
        public string Password { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public int PhoneNumber {get; set;}

        [Required]
        public string CompanyName {get;set;}

        [Required]
        public string Country {get;set;}

        public string CompanyService {get; set;}

        public string Designation {get; set;}
    }
}