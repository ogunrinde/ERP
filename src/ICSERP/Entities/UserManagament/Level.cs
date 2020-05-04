using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ICSERP.Entities.UserManagament
{
    public class Level
    {
        public int LevelId {get; set;}

        [Required]
        public string LevelName {get; set;}

        public int CompanyId {get; set;}

        public bool Is_deleted {get; set;} = false;

        public virtual ICollection<User> Users {get; set;}

        public DateTime DateCreated {get; set;}

        public DateTime DateDeactivated {get; set;}
    }
}