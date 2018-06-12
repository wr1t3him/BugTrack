using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BugTrack.Startup))]
namespace BugTrack
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
