using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResortProjectAPI.ModelEF
{
    public class RoomType
    {
        [Key]
        [MaxLength(10)]
        public string ID { get; set; }

        [MaxLength(30)]
        public string NameType { get; set; }

        public virtual ICollection<Room> Rooms { get; set; }
    }
}
