using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Drinks.Startup))]
namespace Drinks
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
