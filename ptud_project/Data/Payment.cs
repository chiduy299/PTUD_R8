using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ptud_project.Data
{
    public class Payment
    {
        [Key]
        public Guid id_payment { get; set; }
        [Required]
        [MinLength(1), MaxLength(100)]
        public string payment_name { get; set; }
    }
}
