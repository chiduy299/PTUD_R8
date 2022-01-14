using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ptud_project.Models
{
    public class Payment
    {
        public class MomoCreatePaymentRequest
        {
            public string partnerCode { get; set; }
            public string requestId { get; set; }
            public Int64 amount { get; set; }
            public string orderId { get; set; }
            public string orderInfo { get; set; }
            public string redirectUrl { get; set; }
            public string ipnUrl { get; set; }
            public string requestType { get; set; }
            public string signature { get; set; }
            public string lang { get; set; }
            public string extraData { get; set; }

        }

        public class MomoCreatePaymentResponse
        {
            public string partnerCode { get; set; }
            public string requestId { get; set; }
            public string orderId { get; set; }
            public Int64 amount { get; set; }
            public Int64 responseTime { get; set; }
            public string message { get; set; }
            public int resultCode { get; set; }
            public string payUrl { get; set; }
            public string deeplink { get; set; }
            public string qrCodeUrl { get; set; }

        }

        public class MomoIPN
        {
            public string partnerCode { get; set; }

            public string orderId { get; set; }
            public string requestId { get; set; }
            public long amount { get; set; }
            public string orderInfo { get; set; }
            public string orderType { get; set; }
            public long transId { get; set; }
            public int resultCode { get; set; }
            public string message { get; set; }

            public string payType { get; set; }
            public long responseTime { get; set; }
            public string extraData { get; set; }
            public string signature { get; set; }
        }

    }
}
