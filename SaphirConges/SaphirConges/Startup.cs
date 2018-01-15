using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SaphirConges.Startup))]
namespace SaphirConges
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
