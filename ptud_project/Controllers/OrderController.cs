using Microsoft.AspNetCore.Mvc;
using ptud_project.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ptud_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly MyDbContext _context;

        public OrderController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet("get_order_by_cus/{id}")]
        public IActionResult GetOrderByCustomer(string id)
        {
            var id_request = new Guid(id);
            // using LinQ [Object] Query
            var orders_by_cus = from order in _context.Orders
                           where order.id_customer == id_request
                           select order;
            if (orders_by_cus.Count() != 0)
            {
                return Ok(orders_by_cus);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("get_detail_order_by_id/{id}")]
        public IActionResult GetDetailOrderById(string id)
        {
            var id_request = new Guid(id);
            // using LinQ [Object] Query
            var detail_orders = from detail in _context.DetailOrders
                                where detail.order_id == id_request
                                select detail;
            if (detail_orders.Count() != 0)
            {
                return Ok(detail_orders);
            }
            else
            {
                return NotFound();
            }
        }
    }

}
