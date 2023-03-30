using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ResortProjectAPI.ModelEF
{
    public class SuppliesForRoom
    {
        [Key, Column(Order = 1)]
        [MaxLength(10)]
        public string RoomID { get; set; }

        [Key, Column(Order = 2)]
        [MaxLength(10)]
        public string SupplyID { get; set; }

        [Required]
        public int Count { get; set; }

        [ForeignKey("RoomID")]
        public virtual Room Room { get; set; }

        [ForeignKey("SupplyID")]
        public virtual Supply Supply { get; set; }
    }
}
