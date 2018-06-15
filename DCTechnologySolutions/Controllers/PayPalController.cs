using DCTechnologySolutions.Models;
using PayPal;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DCTechnologySolutions.Controllers
{
    public partial class GalleryController : Controller
    {
        // https://demo.paypal.com/us/demo/navigation?merchant=beauty&page=merchantHome&device=desktop
        public ActionResult PayPalSamples()
        {
            ViewBag.AddToCart = PayPalConfigModel.AddToCart;
            return View();
        }

        public string PayPalCreatePayment(string Sku)
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
                string accessToken;
                OAuthTokenCredential oAuth = new OAuthTokenCredential(PayPalConfigModel.clientId, PayPalConfigModel.secretKey);
                accessToken = oAuth.GetAccessToken();
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

        public string PayPalExecutePayment(string PaymentID, string PayerID, string intent, string orderId, string token, string returnURL, string param)
        {
            try
            {
                Payment payment = new Payment()
                {
                    id = PaymentID,
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

                string accessToken;
                OAuthTokenCredential oAuth = new OAuthTokenCredential(PayPalConfigModel.clientId, PayPalConfigModel.secretKey);
                accessToken = oAuth.GetAccessToken();
                APIContext apiContext = new APIContext(accessToken);
                PaymentExecution paymentExecution = new PaymentExecution()
                {
                    payer_id = PayerID
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

        public ActionResult PayPalCreatePaymentReturn(string intent, string orderId, string paymentID, string billingId, string payerID, string returnUrl, string param)
        {
            // Success, save info
            //return RedirectToAction("PayPalPaymentSuccess", new { param=param } );
            //return RedirectToAction("Index", "Home");
            PayPalReturnModel model = new PayPalReturnModel()
            {
                intent = intent,
                paymentID = paymentID,
                payerID = payerID,
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
                model.payment = Payment.Get(apiContext, paymentID);
            }
            return View(model);
        }

        public ActionResult PayPalCreatePaymentCancel(string intent, string paymentID, string payerID, string cancelURL, string token, string param)
        {
            // Cancel
            ViewBag.intent = intent;
            ViewBag.paymentID = paymentID;
            ViewBag.payerID = payerID;
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
    }
}