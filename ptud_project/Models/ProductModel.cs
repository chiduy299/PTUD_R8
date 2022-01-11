using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ptud_project.Models
{
    public class ProductUpdateModel
    {
        public string product_name { get; set; }
        public Decimal unit_price { get; set; }
        public string unit_product_name { get; set; }
        public Int64 product_remaining { get; set; }
    }

}
