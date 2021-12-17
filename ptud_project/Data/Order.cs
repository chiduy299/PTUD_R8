﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ptud_project.Data
{
    public class Order
    {
        [Key]
        public Guid id_order { get; set; }
        [Required]
        public Int64 created_at { get; set; }
        public Int64 update_at { get; set; }
        public Int64 pay_at { get; set; }
        public Int16 total_item { get; set; }
        public Decimal total_amount { get; set; }
        [Required]
        public Int16 status { get; set; }

        public Guid? area { get; set; }
        [ForeignKey("area")]
        public Area area_order { get; set; }

        public Guid? shipping_method { get; set; }
        [ForeignKey("shipping_method")]
        public ShippingServices shipping { get; set; }

        public Guid? payment_method { get; set; }
        [ForeignKey("payment_method")]
        public Payment payment { get; set; }

    }

    public class DetailOrder
    {
        public Guid? order_id { get; set; }
        [ForeignKey("order_id")]
        public Order order { get; set; }
        public Guid? product_id { get; set; }
        [ForeignKey("product_id")]
        public Product product { get; set; }
        public Int64 unit_price { get; set; }
        public Int16 quantity { get; set; }
        public Int64 total { get; set; }
    }
}
