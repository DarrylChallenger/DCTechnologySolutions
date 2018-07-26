using System.Configuration;
using PayPal.Api;

namespace DCTechnologySolutions.Models
{
    public class PayPalModel
    {
        public PayPalModel()
        {
            SecretKey = PayPalConfigModel.secretKey;
            ClientId = PayPalConfigModel.clientId;
        }
        public string SecretKey { get; set; }
        public string ClientId { get; set; }
        public string UserSecrectKey { get; set; }
        public string UserClientId { get; set; }
        public string PayPalCompanyName { get; set; }
    }

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