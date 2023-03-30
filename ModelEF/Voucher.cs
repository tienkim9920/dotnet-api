using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResortProjectAPI.ModelEF
{
    public class Voucher
    {
        [Key]
        [MaxLength(20)]
        public string Code { get; set; }

        [Required]
        [Range(0,100)]
        public int Discount { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        [Required]
        public int Condition { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
