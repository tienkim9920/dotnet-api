using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ResortProjectAPI.ModelEF
{
    public class Booking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [MaxLength(20)]
        public string CustomerID { get; set; }

        [Required]
        [MaxLength(10)]
        public string RoomID { get; set; }

        [Required]
        public DateTime CheckinDate { get; set; }

        [Required]
        public DateTime CheckoutDate { get; set; }

        public string Status { get; set; }

        [Required]
        public int Adult { get; set; }

        [Required]
        public int Child { get; set; }

        [MaxLength(20)]
        public string VoucherCode { get; set; }

        public string FeedBack { get; set; }

        [Range(1,5)]
        public System.Nullable<int> Rate { get; set; }

        //Relation Model
        [ForeignKey("CustomerID")]
        public virtual Customer Customer { get; set; }

        [ForeignKey("RoomID")]
        public virtual Room Room { get; set; }

        [ForeignKey("VoucherCode")]
        public virtual Voucher Voucher { get; set; }

        public virtual ICollection<BookingServices> Services { get; set; }
    }
}
