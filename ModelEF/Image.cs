using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ResortProjectAPI.ModelEF
{
    public class Image
    {
        [Key]
        public string URL { get; set; }

        [MaxLength(10)]
        public string RoomID { get; set; }

        [ForeignKey("RoomID")]
        public virtual Room Room { get; set; }
    }
}
