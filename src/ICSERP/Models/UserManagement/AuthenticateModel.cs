using System.ComponentModel.DataAnnotations;

namespace ICSERP.Models.UserManagement
{
    public class AuthenticateModel
    {
        [Required]
        public string Email {get; set;}

        [Required]
        public string password {get; set;}
    }
}