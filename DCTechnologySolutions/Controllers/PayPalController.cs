using DCTechnologySolutions.Models;
using PayPal;
using PayPal.Api;
using System.Collections.Generic;
using System.Web.Mvc;

namespace DCTechnologySolutions.Controllers
{
    public partial class GalleryController
    {
        // https://demo.paypal.com/us/demo/navigation?merchant=beauty&page=merchantHome&device=desktop
        public ActionResult PayPalSamples()
        {
            PayPalModel ppm = new PayPalModel()
            {
                PayPalCompanyName = "Challenger Technology Soultions Store "
            };
            return View(ppm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PayPalSamples(PayPalModel ppm)
        {
            ppm.PayPalCompanyName = "Your PayPal Store ";
            return View(ppm);
        }

        public string PayPalCreatePayment(string sku)
        {
            // do a lookup for the sku
            try
            {
                Payment payment = new Payment()
                {
                    intent = "sale",
                    redirect_urls = new RedirectUrls()
                    {
                        cancel_url = Url.Action("PayPalCreatePaymentCancel"),
                        return_url = Url.Action("PayPalCreatePaymentReturn")
                    },
                    payer = new Payer()
                    {
                        payment_method = "paypal"
                    },
                    transactions = new List<Transaction>()
                    {
                        {
                           new Transaction()
                           {
                               amount = new Amount()
                               {
                                   total = "2.01",
                                   currency = "USD",
                               }
                           }
                        },
                    },
                    note_to_payer = "Test Payment"
                };
                // make the call!
                OAuthTokenCredential oAuth = new OAuthTokenCredential(PayPalConfigModel.clientId, PayPalConfigModel.secretKey);
                string accessToken = oAuth.GetAccessToken();
                APIContext apiContext = new APIContext(accessToken);
                Payment result = payment.Create(apiContext);
                string resultString = result.ConvertToJson();

                return resultString;
            }
            catch (PayPalException e)
            {
                return e.Message;
            }
        }

        public string PayPalExecutePayment(string paymentId, string payerId, string intent, string orderId, string token, string returnURL, string param)
        {
            try
            {
                Payment payment = new Payment()
                {
                    id = paymentId,
                    intent = intent,
                    redirect_urls = new RedirectUrls()
                    {
                        cancel_url = Url.Action("PayPalCreatePaymentCancel"),
                        return_url = Url.Action("PayPalCreatePaymentReturn")
                    },
                    payer = new Payer()
                    {
                        payment_method = "paypal"
                    },
                    transactions = new List<Transaction>()
                    {
                        {
                           new Transaction()
                           {
                               amount = new Amount()
                               {
                                   total = "2.02",
                                   currency = "USD",
                               }
                           }
                        },
                    },
                    note_to_payer = "Test Payment"
                };

                OAuthTokenCredential oAuth = new OAuthTokenCredential(PayPalConfigModel.clientId, PayPalConfigModel.secretKey);
                string accessToken = oAuth.GetAccessToken();
                APIContext apiContext = new APIContext(accessToken);
                PaymentExecution paymentExecution = new PaymentExecution()
                {
                    payer_id = payerId
                };
                Payment result = payment.Execute(apiContext, paymentExecution);
                string resultString = result.ConvertToJson();

                return resultString;
            }
            catch (PayPalException e)
            {
                return e.Message;
            }
        }

        public ActionResult PayPalCreatePaymentReturn(string intent, string orderId, string paymentId, string billingId, string payerId, string returnUrl, string param)
        {
            // Success, save info
            //return RedirectToAction("PayPalPaymentSuccess", new { param=param } );
            //return RedirectToAction("Index", "Home");
            PayPalReturnModel model = new PayPalReturnModel()
            {
                intent = intent,
                paymentID = paymentId,
                payerID = payerId,
                orderId = orderId,
                returnUrl = returnUrl
            };
            if (intent != null)
            {
                string accessToken;
                OAuthTokenCredential oAuth = new OAuthTokenCredential(PayPalConfigModel.clientId, PayPalConfigModel.secretKey);
                accessToken = oAuth.GetAccessToken();
                APIContext apiContext = new APIContext(accessToken);
                // Get the Order info
                //model.order = Order.Get(apiContext, orderId);
                // Get the Payer info
                model.payment = Payment.Get(apiContext, paymentId);
            }
            return View(model);
        }

        public ActionResult PayPalCreatePaymentCancel(string intent, string paymentId, string payerId, string cancelURL, string token, string param)
        {
            // Cancel
            ViewBag.intent = intent;
            ViewBag.paymentID = paymentId;
            ViewBag.payerID = payerId;
            ViewBag.cancelURL = cancelURL;
            ViewBag.token = token;

            return View();
        }

        public ActionResult PayPalError(string name, string message, string param)
        {
            ViewBag.name = name;
            ViewBag.message = message;
            return View();
            // Error
        }

        /*public FileContentResult GetCart()
        {
            return FileContentResult f = new ("~/Images/Cart.png", "image/png");
        }*/

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetupPaypalStore(PayPalModel ppm)
        {
            return View(ppm);
        }


    }
}