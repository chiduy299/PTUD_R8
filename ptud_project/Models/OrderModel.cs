using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ptud_project.Models
{
    public class OrderModel
    {

        public class CreateOrderRequest
        {
            public Double total_amount { get; set; }
            public string payment_method_id { get; set; }
            public string customer_id { get; set; }
            public string store_id { get; set; }
            public List<string> products { get; set; }
            public List<int> quantities { get; set; }
        }

        public class GetDetailOrderResponse
        {
            public string product_id { get; set; }
            public string product_name { get; set; }
            public float rating { get; set; }
            public string unit_product_name { get; set; }
            public Double unit_price { get; set; }
            public Int16 quantity { get; set; }
            public Double total { get; set; }
        }
    }
}
