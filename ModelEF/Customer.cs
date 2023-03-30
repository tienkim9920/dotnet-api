using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ResortProjectAPI.ModelEF
{
    public class Customer
    {
        [Key]
        [MaxLength(20, ErrorMessage = "ID contains at most 20 characters")]
        [Required(ErrorMessage = "ID is required")]
        public string ID { get; set; }

        [MaxLength(50, ErrorMessage = "Name contains at most 50 characters")]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public DateTime Birth { get; set; }

        [MaxLength(10)]
        public string Phone { get; set; }

        public bool Gender { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MaxLength(20, ErrorMessage = "Password contains at most 20 characters")]
        public string Password { get; set; }

        public string Email { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
