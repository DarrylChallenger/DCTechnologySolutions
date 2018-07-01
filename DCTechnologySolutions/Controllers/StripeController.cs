using DCTechnologySolutions.Models;
using Stripe;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace DCTechnologySolutions.Controllers
{
    /* Pay Now button Sample */
    /* Pay with Card Sample */
    /* Elements Sample
     * First see if user is a Stripe customer - if not create them/log them on (allow them to create a Standard or Express Acct). 
     * Then add a card 
     * Allow them to purchase items. 
     *      Use a PayNow button
     *      Use other dialog
    */

    public partial class GalleryController : Controller
    {
        public ActionResult StripeSamples(StripeModel model)
        {
            SetStripeSecret("");
            StripeModel sm = model ?? new StripeModel();
            try
            {
                StripeAccount account;
                SetStripeSecret(sm.stripeKey);
                if (model.publicKey != null)
                {
                    StripeAccountService accountService = new StripeAccountService();
                    account = accountService.Get();
                    sm.stripeCompanyName = account.BusinessName ?? account.DisplayName;
                }
                GetDisplayData(sm);
            }
            catch (StripeException se) {
                sm.error = se.StripeError;
                sm.isError = true;
            } catch (Exception e) {
                sm.error.Message = e.Message;
                sm.isError = true;
            }
            return View(sm);
        }

        public ActionResult DisplayStripeResults(StripeModel sm)
        {
            // StripeToken token = tokenService.Get(tokId.token);
            StripeAccount account;
            StripeAccountService accountService = new StripeAccountService();
            account = accountService.Get();
            sm.stripeCompanyName = account.BusinessName ?? account.DisplayName;
            GetDisplayData(sm);
            return View(sm);
        }

        [HttpPost]
        public JsonResult ProcessPayNow(StripeSaveTokenRequest tokId)
        {
            SetStripeSecret(tokId.stripeKey);
            //var tokenService = new StripeTokenService();
            StripeCustomer customer = null;
            StripeCharge charge = null;
            StripeModel sm = new StripeModel();

            //Handle tokenization
            string custEmail = "Contact@dctecnologysolutions.com";

            if (tokId.token != null)
            {
                // create a customer
                try
                {
                    string addr1 = tokId.shippingAddress.addressLine.Count >= 1 ? (tokId.shippingAddress.addressLine[0] ?? "") : "";
                    string addr2 = tokId.shippingAddress.addressLine.Count >= 2 ? (tokId.shippingAddress.addressLine[1] ?? "") : "";
                    StripeShippingOptions shippingOptions = new StripeShippingOptions
                    {
                        Name = tokId.shippingAddress.recipient,
                        Line1 = addr1,
                        Line2 = addr2,
                        CityOrTown = tokId.shippingAddress.city,
                        PostalCode = tokId.shippingAddress.postalCode,
                        Country = tokId.shippingAddress.country
                    };
                    StripeCustomerCreateOptions customerCreateOptions = new StripeCustomerCreateOptions
                    {
                        SourceToken = tokId.token,
                        Email = custEmail,
                        Shipping = shippingOptions
                    };
                    customer = AddCustomerViaToken(customerCreateOptions);
                    StripeChargeCreateOptions options = new StripeChargeCreateOptions
                    {
                        Amount = 1999,//pm.StripeData.checkoutAmount,
                        Currency = "usd",
                        Description = "Example Pay Now charge " + DateTime.Now.ToString(),
                        CustomerId = customer.Id,
                        StatementDescriptor = "Blue Circle",// + DateTime.Now.ToString(),
                        ReceiptEmail = custEmail
                    };
                    charge = AddChargeViaCustomer(options);
                }
                catch (StripeException e)
                {
                    sm.error = e.StripeError;
                    sm.isError = true;
                    return Json(sm);
                }
                // Just pass the Ids back to the script rather than the hydrated objects
                sm.charge = new StripeCharge();
                sm.charge.Id = charge.Id;
                sm.customer = new StripeCustomer();
                sm.customer.Id = customer.Id;
                return Json(sm);
            } else {
                // throw an invalid token error
                sm.error = new StripeError
                {
                    Error = "Token is empty",
                };
                sm.isError = true;
                return Json(sm);
            }
        }

        [HttpPost]
        public ActionResult ProcessCheckoutBtn(StripeProcessCardModel spcm)
        {
            SetStripeSecret(spcm.stripeKey);
            StripeModel sm = new StripeModel
            {
                tokenId = spcm.stripeToken,
            };
            try
            {
                StripeAccount account;
                StripeAccountService accountService = new StripeAccountService();
                account = accountService.Get();
                sm.stripeCompanyName = account.BusinessName ?? account.DisplayName; GetDisplayData(sm);
                StripeShippingOptions shippingOptions = new StripeShippingOptions
                {
                    Name = spcm.stripeShippingName,
                    Line1 = spcm.stripeShippingAddressLine1,
                    CityOrTown = spcm.stripeShippingAddressCity,
                    PostalCode = spcm.stripeShippingAddressZip,
                    Country = spcm.stripeShippingAddressCountry
                };
                StripeCustomerCreateOptions customerCreateOptions = new StripeCustomerCreateOptions
                {
                    SourceToken = spcm.stripeToken,
                    Email = spcm.stripeEmail,
                    Shipping = shippingOptions
                };
                sm.customer = AddCustomerViaToken(customerCreateOptions);
                StripeChargeShippingOptions chargeShippingOptions = new StripeChargeShippingOptions
                {

                };
                StripeChargeCreateOptions chargeCreateOptions = new StripeChargeCreateOptions
                {
                    Amount = 1499,//pm.StripeData.checkoutAmount,
                    Currency = "usd",
                    Description = "Example Checkout Charge " + DateTime.Now.ToString(),
                    CustomerId = sm.customer.Id,
                    StatementDescriptor = "Red Square",// + DateTime.Now.ToString(),
                    ReceiptEmail = spcm.stripeEmail
                };
                sm.charge = AddChargeViaCustomer(chargeCreateOptions);
                //AddChargeViaToken();
            } catch (StripeException e)
            {
                sm.error = e.StripeError;
                sm.isError = true;
                View(sm);
            }
            catch (Exception e)
            {
                sm.error = new StripeError
                {
                    Error = e.Message
                };
                sm.isError = true;
                View(sm);
            }
            return View(sm);
        }

        [HttpPost]
        public ActionResult ProcessCardElement(StripeModel sm)
        {
            // Create the charge 
            SetStripeSecret(sm.stripeKey);
            StripeChargeCreateOptions options = new StripeChargeCreateOptions
            {
                Amount = 1299,//pm.StripeData.checkoutAmount,
                Currency = "usd",
                Description = "Example Pay Now charge " + DateTime.Now.ToString(),
                SourceTokenOrExistingSourceId = sm.tokenId,
                StatementDescriptor = "Yellow Triangle",// + DateTime.Now.ToString()
            };
            sm.charge = AddChargeViaToken(options);
            GetDisplayData(sm);
            return View(sm);
        }

        [HttpPost]
        public ActionResult SetupStore(StripeModel sm)
        {
            return View(sm);
        }

        public void SetStripeSecret(string secret)
        {
            if (secret !=null && secret.Length >=7 && secret.Substring(0, 7) == "sk_test")
            {
                StripeConfiguration.SetApiKey(secret);
            } else {
                // Use my app if user does not supply a key
                StripeConfiguration.SetApiKey(ConfigurationManager.AppSettings["st-secretKey"]);//sk_test_HtUydDU48QTml1I2FGLGPJsT
            }
        }

        public StripeCustomer AddCustomerViaToken(StripeCustomerCreateOptions scco)
        {
            StripeCustomerService customerService = new StripeCustomerService();
            return customerService.Create(scco);
        }

        public StripeCharge AddChargeViaCustomer(StripeChargeCreateOptions scco)
        {
            var service = new StripeChargeService();
            return service.Create(scco);
        }

        public StripeCharge AddChargeViaToken(StripeChargeCreateOptions scco)
        {
            var service = new StripeChargeService();
            return service.Create(scco);
        }

        public void GetDisplayData(StripeModel sm)
        {
            // Get customer Info
            if (sm.customer != null)
            {
                StripeCustomerService customerService = new StripeCustomerService();
                sm.customer = customerService.Get(sm.customer.Id);
            }
            // Get Charge Info
            if (sm.charge != null)
            {
                StripeChargeService chargeService = new StripeChargeService();
                sm.charge = chargeService.Get(sm.charge.Id);
            }
            return;
        }

    }
}