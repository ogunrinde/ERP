using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ICSERP.Entities.UserManagament
{
    public class Branch
    {
        public  int BranchId  { get; set; }

        [Required]
        public string BranchName  { get; set; }

        public bool Is_deleted {get; set;} = false;

        public int CompanyId {get; set;}

        public virtual Company Company {get; set;}

        public virtual ICollection<User> Users  { get; set; }

        public DateTime DateCreated  { get; set; }

        public DateTime DateDeactivated {get; set;}
    }
}