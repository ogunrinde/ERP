using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ICSERP.Entities.UserManagament
{
    public class Company
    {
        public int CompanyId  { get; set; }

        [Required]
        public string CompanyName  { get; set; }

        public string CompanyTask {get; set;}

        public bool Is_deleted {get; set;} = false;

        public virtual ICollection<User> Users  { get; set; }

        public virtual ICollection<Department> Departments {get; set;}

        public virtual ICollection<Unit> Units {get; set;}

        public virtual ICollection<Branch> Branches {get; set;}

        public virtual ICollection<Role> Roles { get; set; }
        public DateTime DateCreated {get; set;}

        public DateTime DateDeactivated {get; set;}
    }
}