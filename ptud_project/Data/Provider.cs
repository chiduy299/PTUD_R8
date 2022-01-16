using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ptud_project.Data
{
    public class Provider
    {
        [Key]
        public Guid id_prov{ get; set; }
        [Required]
        [MinLength(1), MaxLength(100)]
        public string name { get; set; }
        [Required]
        [MinLength(1)]
        public string address { get; set; }
        public Int16 rating { get; set; }
        [MaxLength(11)]
        public string phone { get; set; }
        public Guid? owner { get; set; }
        [ForeignKey("owner")]
        public Customer customer { get; set; }
    }
}
