using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using ptud_project.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ptud_project.Models.Payment;

namespace ptud_project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public PaymentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("momo/ipn")]
        public IActionResult MomoIPN([FromBody] MomoIPN request)
        {
            try
            {
                var partner_code = _configuration.GetValue<string>("MomoConfig:PARTNER_CODE");
                var access_key = _configuration.GetValue<string>("MomoConfig:ACCESS_KEY");
                var secret_key = _configuration.GetValue<string>("MomoConfig:SECRET_KEY");
                MongoClient dbClient = new MongoClient(_configuration.GetConnectionString("PtudhtttDB"));
                var order = dbClient.GetDatabase("ptudhttt").GetCollection<Order>("Orders").AsQueryable().Where(x => x.order_id == request.orderId && x.status == 0).FirstOrDefault();
                if (order == null)
                    return Ok(new { code = -1, message = "Not existing data" });
                if (request.errorCode != "0")
                    return Ok(new { code = -1, message = "Invalid result code" });
                var rawHash = "accessKey=" + access_key
                + "&amount=" + request.amount.ToString()
                + "&extraData=" + request.extraData
                + "&message=" + request.message
                + "&orderId=" + request.orderId
                + "&orderInfo=" + request.orderInfo
                + "&orderType=" + request.orderType
                + "&partnerCode=" + partner_code
                + "&payType=" + request.payType
                + "&requestId=" + request.requestId
                + "&responseTime=" + request.responseTime
                + "&errorCode=" + request.errorCode
                + "&transId=" + request.transId;
                var signature = Services.helper.GetHMAC(rawHash, secret_key);
                if (signature != request.signature)
                    return Ok(new { code = -1, message = "Invalid signature" });
                order.status = 1;
                order.updated_at = Services.helper.now_to_epoch_time();
                dbClient.GetDatabase("ptudhttt").GetCollection<Order>("Orders").FindOneAndReplace(x => x.id == request.orderId, order);
                return Ok(new {
                    code = 0,
                    message = "Success",
                });
            }
            catch
            {
                return Ok(new { code = -401, message = "Bad Request" });
            }
        }
    }
}
