using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ptud_project.Models
{
    public class RegisterStoreModel
    {
        [Required]
        [MinLength(1), MaxLength(100)]
        public string store_name { get; set; }
        public string product_category { get; set; }
        public string provider_id { get; set; }
        [Required]
        [MaxLength(11)]
        public string phone { get; set; }
    }
    public class UpdateStoreModel
    {
        public string store_name { get; set; }
        public string product_category { get; set; }
        public string provider_name { get; set; }
        public string phone { get; set; }
    }
}
