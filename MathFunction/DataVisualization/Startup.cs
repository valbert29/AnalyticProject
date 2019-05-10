using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DataVisualization.Startup))]
namespace DataVisualization
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
