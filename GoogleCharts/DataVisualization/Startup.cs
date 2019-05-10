using Microsoft.Owin;
using Owin;

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
