using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ResortProjectAPI.ModelEF
{
    public class Room
    {
        [Key]
        [MaxLength(10)]
        public string ID { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [MaxLength(10)]
        [Required]
        public string TypeID { get; set; }

        [Required]
        public string Description { get; set; }

        public string Status { get; set; }

        [Required]
        public int Adult { get; set; }

        [Required]
        public int Child { get; set; }

        [Required]
        public double Price { get; set; }

        //Relation Model
        [ForeignKey("TypeID")]
        public virtual RoomType RoomType { get; set; }

        public virtual ICollection<SuppliesForRoom> Supplies { get; set; }

        public virtual ICollection<Image> Images { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
    }
}
