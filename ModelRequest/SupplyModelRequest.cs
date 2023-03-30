using ResortProjectAPI.ModelEF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResortProjectAPI.ModelRequest
{
    public class SupplyModelRequest
    {
        [Required]
        [MaxLength(10)]
        public string id { get; set; }

        [Required]
        [MaxLength(30)]
        public string name { get; set; }

        public string editType { get; set; } 

        [Range(0,int.MaxValue)]
        public int? count { get; set; }
    }
}
