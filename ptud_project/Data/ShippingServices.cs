using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ptud_project.Data
{
    public class ShippingServices
    {
        [Key]
        public string id_ship { get; set; }
        [Required]
        [MinLength(1), MaxLength(100)]
        public string shipping_name { get; set; }
        [Required]
        public float rating { get; set; }
        [Required]
        public Int64 shipping_fee { get; set; }
    }
}
