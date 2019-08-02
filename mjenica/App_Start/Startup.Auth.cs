using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mjenica
{

    public static class MjenicaAuthType
    {
        public const string AuthType = "MjenicaWebAppV2";
    }

    public partial class Startup
	{
        public void ConfigureApp(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions()
            {
                AuthenticationType = MjenicaAuthType.AuthType,
                //AuthenticationMode = AuthenticationMode.Active,
                LoginPath = new Microsoft.Owin.PathString("/Racuni"),
                CookieName = ".MJE_ASPAUTH",
                Provider = new CookieAuthenticationProvider(),
                CookieHttpOnly = true,
                ExpireTimeSpan = TimeSpan.FromMinutes(30)

            });
        }
	}
}