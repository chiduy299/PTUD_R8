using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ptud_project.Data
{
    public class Shipper
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        [Required]
        [MinLength(1), MaxLength(100)]
        public string name { get; set; }
        [Required]
        public Int64 created_at { get; set; }
        public string avatar_url { get; set; }
        public Int64 updated_at { get; set; }
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
        [Required]
        public bool is_enabled { get; set; }

        [Required]
        public Int32 number_order_delivered { get; set; }
    }
}
