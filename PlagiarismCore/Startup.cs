using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(PlagiarismCore.Startup))]

namespace PlagiarismCore
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            AuthConfig.Configure(app);
        }
    }
}
