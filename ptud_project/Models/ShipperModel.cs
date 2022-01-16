using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ptud_project.Models
{
    public class ShipperModel : Controller
    {
        public class RegisterShipperModel
        {
            [Required]
            [MinLength(1), MaxLength(100)]
            public string name { get; set; }
            public string cmnd { get; set; }
            public string address { get; set; }
            [Required]
            [MaxLength(11)]
            public string phone { get; set; }
            [Required]
            [MinLength(8)]
            public string password { get; set; }
            [Required]
            [MinLength(8)]
            public string confirm_password { get; set; }
            [Required]
            [Range(0, 2)]
            public Int16 sex { get; set; }
            public string avatar_url { get; set; }
            [Required]
            public string area_type { get; set; }
        }

        public class ShipperRequestOrderModel
        {
            public string shipper_id { get; set; }
            public string order_id { get; set; }
        }
    }
}
