using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResortProjectAPI.ModelEF
{
    public class Supply
    {
        [Key]
        [MaxLength(10)]
        public string ID { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        public int Total { get; set; }

        public virtual ICollection<SuppliesForRoom> Rooms { get; set; }
    }
}
