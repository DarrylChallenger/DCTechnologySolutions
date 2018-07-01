using DCTechnologySolutions.Models;
using Stripe;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
/*
namespace DCTechnologySolutions
{
    public class StripeTokensController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public bool Post([FromBody]StripeTokenInfo tokId)
        {
            StripeConfiguration.SetApiKey(StripeConfig.secretKey = ConfigurationManager.AppSettings["st-secretKey"]);//sk_test_HtUydDU48QTml1I2FGLGPJsT
            var tokenService = new StripeTokenService();
            StripeToken token = tokenService.Get(tokId.token);

            //Handle tikenization
            string custEmail = "Contact@dctecnologysolutions.com";

            if (tokId.token != null)
            {
                StripeChargeCreateOptions options = new StripeChargeCreateOptions
                {
                    Amount = 1999,//pm.StripeData.checkoutAmount,
                    Currency = "usd",
                    Description = "Example charge " + DateTime.Now.ToString(),
                    SourceTokenOrExistingSourceId = tokId.token,
                    StatementDescriptor = "Descriptor",
                    ReceiptEmail = custEmail
                };
                var service = new StripeChargeService();
                try
                {
                    StripeCharge charge = service.Create(options);
                }
                catch (StripeException e)
                {
                    StripeError stripeError = e.StripeError;
                }
                return true;
            }
            return false;
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}
*/