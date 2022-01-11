using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using ptud_project.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ptud_project.Models.OrderModel;

namespace ptud_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public OrderController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("get_order_by_customer")]
        [Authorize]
        public IActionResult GetOrderByCustomer()
        {
            var id_claim = User.Claims.FirstOrDefault(x => x.Type.Equals("id", StringComparison.InvariantCultureIgnoreCase));
            if (id_claim == null)
            {
                return Ok(new { code = -400, message = "Not existing data"});
            }
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("PtudhtttDB"));
            var orders = dbClient.GetDatabase("ptudhttt").GetCollection<Order>("Orders").AsQueryable().Where(x => x.customer_id == id_claim.Value.ToString());
            return Ok(new
            {
                code = 0,
                message = "Success",
                payload = orders,
            });
        }

        [HttpGet("get_detail_order_by_id/{id}")]
        public IActionResult GetDetailOrderById(string id)
        {
            MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("PtudhtttDB"));
            var details_order = dbClient.GetDatabase("ptudhttt").GetCollection<DetailOrder>("DetailOrders").AsQueryable().Where(x => x.order_id == id);
            List<GetDetailOrderResponse> resp = new List<GetDetailOrderResponse>();
            foreach (var order in details_order)
            {
                var product = dbClient.GetDatabase("ptudhttt").GetCollection<Product>("Products").AsQueryable().Where(x => x.id == order.product_id).SingleOrDefault();
                resp.Add(new GetDetailOrderResponse
                {
                    product_id = product.id,
                    product_name = product.product_name,
                    rating = product.rating,
                    unit_price = product.unit_price,
                    unit_product_name = product.unit_product_name,
                    quantity = order.quantity,
                    total = order.total
                }
                );
            }
            return Ok(new
            {
                code = 0,
                message = "Success",
                payload = resp,
            });
        }
    }

}
