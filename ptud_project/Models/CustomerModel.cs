using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ptud_project.Models
{
    public class RegisterCustomerModel
    {
        [Required]
        [MinLength(1), MaxLength(100)]
        public string name { get; set; }
        public string email { get; set; }
        public string cmnd { get; set; }
        public string district { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string ward { get; set; }
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
        [Range(0,2)]
        public Int16 sex { get; set; }
        public string avatar_url { get; set; }
        [Required]
        public string area_type { get; set; }
    }

    public class UpdateCustomerModel
    {
        public string name { get; set; }
        public string email { get; set; }
        public string cmnd { get; set; }
        public string district { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string ward { get; set; }
        public string password { get; set; }
        public string confirm_password { get; set; }
        public Int16 sex { get; set; }
        public string avatar_url { get; set; }
        public string area_type { get; set; }
    }

    public class LoginCustomerModel
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}
