using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ptud_project.Data
{
    public class Product
    {
        [Key]
        public Guid id_product { get; set; }
        [Required]
        [MinLength(1), MaxLength(100)]
        public string product_name { get; set; }
        [Required]
        public float rating { get; set; }
        [Required]
        public Int64 product_remaining { get; set; }
        [Required]
        public Int64 sell_number { get; set; }
        [Required]
        public Int16 create_at { get; set; }
        public Int16 update_at { get; set; }
        public Decimal total_amount { get; set; }
        public Guid? supplier { get; set; }
        [ForeignKey("supplier")]
        public Provider provider { get; set; }

    }
}
