using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCTechnologySolutions.Models
{
    public class PayPalConfigModel
    {
        public static string secretKey { get; set; }
        public static string clientId { get; set; }
        public static string AddToCart { get; set; }
    }

    public class PayPalReturnModel
    {
        public string intent { get; set; }
        public string paymentID { get; set; }
        public string payerID { get; set; }
        public string orderId { get; set; }
        public string returnUrl { get; set; }
        public Order order { get; set; }
        public Payment payment { get; set; }
    }
}