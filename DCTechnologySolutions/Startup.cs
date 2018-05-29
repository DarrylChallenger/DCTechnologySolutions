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

        private void ConfigurePayPal()
        {
            PayPalConfig.secretKey = ConfigurationManager.AppSettings["pp-SecretKey"];
            PayPalConfig.clientId = ConfigurationManager.AppSettings["pp-ClientId"];
        }

        private void ConfigureStripe()
        {
            StripeConfig.publicKey = ConfigurationManager.AppSettings["st-publicKey"];
            StripeConfig.secretKey = ConfigurationManager.AppSettings["st-secretKey"];
            StripeConfig.clientId = ConfigurationManager.AppSettings["st-clientId"];
        }
    }
}
