using Microsoft.Owin;
using Owin;
using System.Web.Hosting;

[assembly: OwinStartupAttribute(typeof(codemode_youtube.Startup))]
namespace codemode_youtube
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
 
    }
}
