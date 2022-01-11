using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ptud_project.Models
{
    public class OrderModel
    {
        public class GetDetailOrderResponse
        {
            public string product_id { get; set; }
            public string product_name { get; set; }
            public float rating { get; set; }
            public string unit_product_name { get; set; }
            public Decimal unit_price { get; set; }
            public Int16 quantity { get; set; }
            public Int64 total { get; set; }
        }
    }
}
