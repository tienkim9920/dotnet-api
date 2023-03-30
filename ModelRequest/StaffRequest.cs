using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResortProjectAPI.ModelRequest
{
    public class StaffRequest
    {
        [Required(ErrorMessage = "ID is required")]
        [MaxLength(20,ErrorMessage = "ID is too long")]
        [MinLength(5, ErrorMessage = "ID is too short")]
        public string ID { get; set; }

        [MaxLength(50, ErrorMessage = "Name is too long")]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [MinLength(6,ErrorMessage = "Password is too short")]
        [MaxLength(20, ErrorMessage = "Password is too long")]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [MaxLength(10, ErrorMessage = "Permission is too long")]
        public string PermissionID { get; set; }

        [MaxLength(10, ErrorMessage = "Phone is invalid")]
        [Required(ErrorMessage = "Phone is required")]
        public string Phone { get; set; }

        [EmailAddress(ErrorMessage = "Email is invalid")]
        [MaxLength(50, ErrorMessage = "Email is too long")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public bool Gender { get; set; }

        public DateTime Birth { get; set; }
    }
}
