using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ptud_project.Data
{
    public class Customer
    {
        [Key]
        public Guid id_cus { get; set; }
        [Required]
        [MinLength(1),MaxLength(100)]
        public string name { get; set; }
        [Required]
        public Int64 created_at { get; set; }
        public Int64 update_at { get; set; }
        public string cmnd { get; set; }
        public string address { get; set; }
        [Required]
        [MaxLength(11)]
        public string phone { get; set; }
        [Required]
        [MinLength(8)]
        public string password { get; set; }
        [Required]
        [Range(0, 2)]
        public Int16 sex { get; set; }

    }
}
