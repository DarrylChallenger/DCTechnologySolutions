using DCTechnologySolutions.Models;
using Microsoft.Owin;
using Owin;
using System.Configuration;

[assembly: OwinStartupAttribute(typeof(DCTechnologySolutions.Startup))]
namespace DCTechnologySolutions
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            ConfigurePayPal();
        }

        private static void ConfigurePayPal()
        {
            PayPalConfigModel.secretKey = ConfigurationManager.AppSettings["pp-SecretKey"];
            PayPalConfigModel.clientId = ConfigurationManager.AppSettings["pp-ClientId"];
            PayPalConfigModel.AddToCart = ConfigurationManager.AppSettings["pp-AddToCart"];
        }

        private void ConfigureStripe()
        {
            StripeConfig.publicKey = ConfigurationManager.AppSettings["st-publicKey"];
            StripeConfig.secretKey = ConfigurationManager.AppSettings["st-secretKey"];
            StripeConfig.clientId = ConfigurationManager.AppSettings["st-clientId"];
        }
    }
}
