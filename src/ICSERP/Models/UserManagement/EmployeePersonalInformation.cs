using System;
using System.ComponentModel.DataAnnotations;

namespace ICSERP.Models.UserManagement
{
    public class EmployeePersonalInformation
    {
                public string Title {get; set;} 

        [Required]
        [StringLength(80)]
        public string FirstName {get; set;}

        [Required]
        [StringLength(80)]
        public string LastName {get; set;}

        [StringLength(80)]
        public string MiddleName {get; set;}

        [Required]
        public string Email1 {get; set;}

        public string Email2 {get; set;}

        [Required]
        [StringLength(50)]
        public string Gender {get; set;}

        [StringLength(80)]
        public string MaritalStatus {get; set;}

        [StringLength(80)]
        public string StateOfOrgin {get; set;}

        [StringLength(80)]
        public string Nationality {get; set;}

        public DateTime DateOfBirth {get; set;}

        public int PhoneNumber1 {get; set;}

        public int PhoneNumber2 {get; set;}

        public string CurrentAddress {get; set;}


        public string CurrentTown {get; set;}

        public string CurrentCity {get; set;}

        public string CurrentState {get; set;}

        public string PermanantAddress {get; set;}

        public string PermamentTown {get; set;}

        public string PermamentCity {get; set;}

        public string PermanentState {get; set;}

        public string LocalGovt {get;set;}
    }
}