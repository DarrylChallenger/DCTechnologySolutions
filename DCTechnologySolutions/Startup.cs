using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DCTechnologySolutions.Startup))]
namespace DCTechnologySolutions
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
