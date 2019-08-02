using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[assembly:OwinStartupAttribute(typeof(mjenica.Startup))]
namespace mjenica
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureApp(app);
        }
    }
}