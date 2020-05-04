using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ICSERP.Entities.UserManagament
{
    public class Unit
    {
        public int UnitId {get; set;}

        [Required]
        public string UnitName {get; set;}

        public bool Is_deleted {get; set;} = false;

        public virtual ICollection<User> Users  { get; set; }

        public int? DepartmentId  { get; set; }

        public int? CompanyId {get; set;}

        public virtual Department Department {get; set;}

        public DateTime DateCreated  { get; set; }

    }
}