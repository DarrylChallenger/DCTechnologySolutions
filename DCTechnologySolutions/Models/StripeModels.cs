using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DCTechnologySolutions.Models
{
    public class StripeConfig
    {
        public static string token { get; set; }
        public static string publicKey { get; set; }
        public static string secretKey { get; set; }
        public static string clientId { get; set; }
    }

    public class ShippingAddress
    {
        public List<string> addressLine { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string postalCode { get; set; }
        public string recipient { get; set; }
        public string region { get; set; }
    }

    public class BillingAddress
    {
        [Required]
        [Display(Name="Name")]
        public string name { get; set; }
        [Required]
        [Display(Name = "Address")]
        public string address { get; set; }
        [Required]
        [Display(Name = "City")]
        public string city { get; set; }
        [Required]
        [Display(Name = "State")]
        public string state { get; set; }
        [Display(Name = "Country")]
        public string country { get; set; }
        [Required]
        [Display(Name = "ZIP Code")]
        public string postalCode { get; set; }
        [Display(Name = "Region")]
        public string region { get; set; }

    }

    //[DataContract]
    public class StripeSaveTokenRequest
    {
        [DataMember] public string token { get; set; }
        [DataMember] public string email { get; set; }
        [DataMember] public Guid orderId { get; set; }
        [DataMember] public string stripeKey { get; set; }
        [DataMember] public ShippingAddress shippingAddress { get; set; }
}

public class StripeSaveTokenResponse
    {
        [DataMember] public string custId { get; set; }
        [DataMember] public string chargeId { get; set; }
    }

    public class StripeModel
    {
        public StripeModel()
        {
            charge = null;
            error = new StripeError();
            customer = null;// new StripeCustomer();
            charge = null;// new StripeCharge();
            // set these two values to ture to ensure proper behavior
            livemode = true;
            used = true;
            checkoutAmount = 998;
            saveCard = true;
            isError = false;
            stripeKey = "sk_test_LRlhXB1wikIe5Se6tuSXCjD4"; // DCTechnologySolutions
            publicKey = "pk_test_9VBmb0MLFOio86PO162GMNFp"; // DCTechnologySolutions
            shippingAddress = new ShippingAddress();
            billingAddress = new BillingAddress();
        }

        //public StripeConstants stripeConstants { get; set; }
        [Display(Name = "Stripe Secret Key")] public string stripeKey { get; set; }
        [Display(Name = "Stripe Publishable Key")] public string publicKey { get; set; }
        public string clientId { get; set; }
        public string stripeCompanyName { get; set; }
        public StripeCharge charge { get; set; }
        public StripeCustomer customer { get; set; }
        public StripeError error { get; set; }
        public string tokenId { get; set; }
        public bool livemode { get; set; }
        public string type { get; set; }
        public bool used { get; set; }
        public string stripeEmail { get; set; }
        public int checkoutAmount { get; set; }
        public bool saveCard { get; set; }
        [Display(Name = "Email Address")] [EmailAddress] public string mailAddr { get; set; }
        public bool isError { get; set; }
        public ShippingAddress shippingAddress { get; set; }
        [Required]
        public BillingAddress billingAddress { get; set; }
    }

    public class StripeProcessCardModel
    {
        public string stripeKey { get; set; }
        public string stripeToken { get; set; }
        public string stripeEmail { get; set; }
        public string stripeName { get; set; }
        public string stripeBillingName { get; set; }
        public string stripeBillingAddressCity { get; set; }
        public string stripeBillingAddressCountry { get; set; }
        public string stripeBillingAddressLine1 { get; set; }
        public string stripeBillingAddressZip { get; set; }
        public string stripeBillingAddressName { get; set; }
        public string stripeShippingName { get; set; }
        public string stripeShippingAddressCity { get; set; }
        public string stripeShippingAddressCountry { get; set; }
        public string stripeShippingAddressLine1 { get; set; }
        public string stripeShippingAddressZip { get; set; }
        public string stripeShippingAddressName { get; set; }
        //public string stripe { get; set; }
    }
}