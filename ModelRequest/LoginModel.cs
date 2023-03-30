using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResortProjectAPI.ModelRequest
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Username is required")]
        [MaxLength(20,ErrorMessage = "Username contains at most 20 characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MaxLength(20, ErrorMessage = "Password contains at most 20 characters")]
        public string Password { get; set; }
    }
}
