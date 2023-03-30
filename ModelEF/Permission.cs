using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResortProjectAPI.ModelEF
{
    public class Permission
    {
        [Key]
        [MaxLength(10)]
        public string ID { get; set; }

        [MaxLength(10)]
        public string Name { get; set; }

        public virtual ICollection<Staff> Staffs { get; set; }
    }
}
