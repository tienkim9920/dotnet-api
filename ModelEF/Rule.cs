using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ResortProjectAPI.ModelEF
{
    public class Rule
    {
        [Key]
        [MaxLength(10)]
        public string RuleCode { get; set; }

        public string Description { get; set; }

        [Required]
        public int Condition { get; set; }
    }
}
