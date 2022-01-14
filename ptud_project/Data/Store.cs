using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace ptud_project.Data
{
    public class Store
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        [Required]
        [MinLength(1), MaxLength(100)]
        public string store_name { get; set; }
        [Required]
        public float rating { get; set; }
        [Required]
        public string provider_id { get; set; }
        [Required]
        public Int64 created_at { get; set; }
        public Int64 update_at { get; set; }
        [Required]
        public bool is_available { get; set; }
        [Required]
        public string phone { get; set; }
        [Required]
        public string id_category { get; set; }
    }
}
