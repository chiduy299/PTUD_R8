﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
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
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        [Required]
        [MinLength(1), MaxLength(100)]
        public string product_name { get; set; }
        [Required]
        public float rating { get; set; }
        [Required]
        public Double unit_price { get; set; }
        [Required]
        public string unit_product_name { get; set; }
        [Required]
        public Int64 product_remaining { get; set; }
        [Required]
        public Int64 sell_number { get; set; }
        [Required]
        public Int64 created_at { get; set; }
        public Int64 updated_at { get; set; }
        [Required]
        public string provider_id { get; set; }
        [Required]
        public bool is_available { get; set; }

        [Required]
        public string id_category { get; set; }

    }
}
