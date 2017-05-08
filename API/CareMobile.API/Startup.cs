using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(CareMobile.API.Startup))]

namespace CareMobile.API
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}