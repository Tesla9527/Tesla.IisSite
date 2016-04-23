using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Tesla.IisSite.Startup))]
namespace Tesla.IisSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
