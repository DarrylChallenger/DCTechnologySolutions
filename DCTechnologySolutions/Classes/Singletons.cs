using DCTechnologySolutions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace DCTechnologySolutions.Classes
{
    public class Singletons
    {
        public static StripeConfig StripeConfig = new StripeConfig();
        public static HttpClient httpStripeClient = new HttpClient();

        public static PayPalConfig PayPalConfig = new PayPalConfig();
        public static HttpClient httpPayPalClient = new HttpClient();
    }
}