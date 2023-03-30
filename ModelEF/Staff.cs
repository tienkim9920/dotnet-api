using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ResortProjectAPI.ModelEF
{
    public class Staff
    {
        [Key]
        [MaxLength(20)]
        [Required]
        public string ID { get; set; }

        [MaxLength(50)]
        [Required]
        public string Name { get; set; }

        public DateTime Birth { get; set; }

        [MaxLength(10)]
        [Required]
        public string Phone { get; set; }

        [Required]
        public bool Gender { get; set; }

        [MaxLength(10, ErrorMessage = "Password is to long")]
        public string PermissionID { get; set; }

        [Required]
        [MaxLength(20)]
        public string Password { get; set; }

        [MaxLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [ForeignKey("PermissionID")]
        public virtual Permission Permission { get; set; }
    }
}
