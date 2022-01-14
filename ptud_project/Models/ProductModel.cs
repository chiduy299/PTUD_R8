using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ptud_project.Models
{
    public class CreateProductModel
    {
        public string product_name { get; set; }
        public Double unit_price { get; set; }
        public string unit_product_name { get; set; }
        public string product_category { get; set; }
        public List<string> list_images { get; set; }
        public string provider_id { get; set; }
        public long quantity { get; set; }
    }
    public class ProductUpdateModel
    {
        public string product_name { get; set; }
        public Double unit_price { get; set; }
        public string unit_product_name { get; set; }
        public Int64 product_remaining { get; set; }
    }

}
