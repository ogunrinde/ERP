using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ICSERP.Entities.UserManagament
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        public string Email { get; set; }
        
        [StringLength(80)]
        public string Username { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public string UserType {get; set;} = "System User";

        public bool Is_Active { get; set; } =  true;

        public DateTime DateCreated {get; set;}

        public DateTime LastActive {get; set;}

        public bool UserDeactivated {get; set;} = false;

        public DateTime DateDeactivated {get; set;}

        public int? Created_By {get; set;}

         public virtual ICollection<UserRole> UserRoles { get; set; }

         public virtual PersonalInformation PersonalInformation {get; set;}

        public int? LevelId {get; set;}

        public virtual Level Level {get; set;}

        public int? UnitId {get; set;}

        public virtual Unit Unit {get; set;}

        public int? DepartmentId {get; set;}

        public virtual Department Department {get; set;}

        public int? BranchId {get; set;}

        public virtual Branch Branch {get; set;}

        public int? CompanyId {get; set;}

        public virtual Company Company {get; set;}
    }
}