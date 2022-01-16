using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ptud_project.Models
{
    public class RegisterProviderModel
    {
        [Required]
        [MinLength(1), MaxLength(100)]
        public string name { get; set; }
        [Required]
        [MinLength(1)]
        public string address { get; set; }
        public string phone { get; set; }
        [Required]
        [MinLength(8)]
        public string password { get; set; }
        [Required]
        [MinLength(8)]
        public string confirm_password { get; set; }
    }

    public class UpdateProviderModel
    {
        public string name { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public string password { get; set; }
        public string confirm_password { get; set; }
    }
    public class ProviderLoginModel
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}
