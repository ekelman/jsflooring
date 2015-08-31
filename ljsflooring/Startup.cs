using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ljsflooring.Startup))]
namespace ljsflooring
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
