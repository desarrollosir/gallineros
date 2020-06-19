using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebAppGallineros.Startup))]
namespace WebAppGallineros
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
