using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DevelopersApplication.Startup))]
namespace DevelopersApplication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
