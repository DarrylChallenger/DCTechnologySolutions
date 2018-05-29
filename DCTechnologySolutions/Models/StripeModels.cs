using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DCTechnologySolutions.Models
{
    public class StripeConfig
    {
        public static string token { get; set; }
        public static string publicKey { get; set; }
        public static string secretKey { get; set; }
        public static string clientId { get; set; }
    }

    public class StripeModels
    {
        public StripeModels()
        {
            charge = null;
            stripeError = null;
            // set these two values to ture to ensure proper behavior
            livemode = true;
            used = true;
            checkoutAmount = 998;

        }
        //public StripeConstants stripeConstants { get; set; }
        public string publicKey { get; set; }
        public string clientId { get; set; }
        public StripeCharge charge { get; set; }
        public StripeError stripeError { get; set; }
        public string tokenId { get; set; }
        public bool livemode { get; set; }
        public string type { get; set; }
        public bool used { get; set; }
        public string stripeEmail { get; set; }
        public int checkoutAmount { get; set; }
    }
}