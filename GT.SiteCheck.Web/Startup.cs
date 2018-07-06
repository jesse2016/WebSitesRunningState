using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GT.SiteCheck.Web.Startup))]
namespace GT.SiteCheck.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
