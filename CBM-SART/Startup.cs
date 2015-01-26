using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CBM_SART.Startup))]
namespace CBM_SART
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
