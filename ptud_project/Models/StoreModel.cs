using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ptud_project.Models
{
    public class RegisterStoreModel
    {
        public string name { get; set; }
        public string email { get; set; }
        public string cmnd { get; set; }
        public string district { get; set; }
        public string street { get; set; }
        public string city { get; set; }
        public string ward { get; set; }
        public string phone { get; set; }
        public string password { get; set; }
        public string confirm_password { get; set; }
        public string area_type { get; set; }
    }
    public class UpdateStoreModel
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
        public string area_type { get; set; }
    }
}
