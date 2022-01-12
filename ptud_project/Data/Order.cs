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
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        [Required]
        public Int64 created_at { get; set; }
        public Int64 updated_at { get; set; }
        public Int64 pay_at { get; set; }
        public Int16 total_item { get; set; }
        public Double total_amount { get; set; }
        [Required]
        public Int16 status { get; set; }
        public string note { get; set; }
        public string reason_cancel { get; set; }
        [Required]
        public string area_id { get; set; }
        [Required]
        public string shipping_method_id { get; set; }
        [Required]
        public string paymnent_method_id { get; set; }
        [Required]
        public string customer_id { get; set; }
        [Required]
        public string provider_id { get; set; }
        public string shipper_id { get; set; }
    }

    public class DetailOrder
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }

        [Required]
        public string order_id { get; set; }
        [Required]
        public string product_id { get; set; }
        public Int64 unit_price { get; set; }
        public Int16 quantity { get; set; }
        public Int64 total { get; set; }
    }
}
