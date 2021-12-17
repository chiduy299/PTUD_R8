using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ptud_project.Data
{
    public class Area
    {
        [Key]
        public Guid id_area { get; set; }
        [Required]
        [MinLength(1), MaxLength(100)]
        public String area_name { get; set; }
        public string area_description { get; set; }
    }
}
