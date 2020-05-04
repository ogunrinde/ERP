using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ICSERP.Entities.UserManagament
{
    public class Department
    {
        public int DepartmentId  { get; set; }

        [Required]
        public string DepartmentName  { get; set; }

        public bool Is_deleted {get; set;} = false;

        public virtual ICollection<Unit> Units  { get; set; }

        public virtual ICollection<User> Users  { get; set; }

        public int CompanyId {get; set;}

        public DateTime DateCreated  { get; set; }

        public DateTime DateDeactivated {get; set;}
    }
}