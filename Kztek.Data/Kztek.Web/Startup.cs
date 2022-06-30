using System;
using System.Threading.Tasks;
using System.Web.Http;
using Hangfire;
using Kztek.Model.Models;
using Kztek.Web.API;
using Kztek.Web.Areas.Parking.Controllers;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;

[assembly: OwinStartup(typeof(Kztek.Web.Startup))]

namespace Kztek.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //Enables SignalR
            //app.MapSignalR();
            //GlobalHost.Configuration.ConnectionTimeout = TimeSpan.FromSeconds(40);
            //GlobalHost.Configuration.DisconnectTimeout = TimeSpan.FromSeconds(30);
            //GlobalHost.Configuration.KeepAlive = TimeSpan.FromSeconds(10);
            ConfigureAuth(app);
            //var obj = new tblLEDController();
            //RecurringJob.AddOrUpdate(() => obj.GetPCList3(), Cron.Minutely);
            //app.UseHangfireServer();
            //GlobalConfiguration.Configure(WebApiConfig.Register);
        }
        public void ConfigureAuth(IAppBuilder app)
        {

            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            var myProvider = new AuthorizationServerProvider();
            OAuthAuthorizationServerOptions options = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(24),
                Provider = myProvider,
                RefreshTokenProvider = new RefreshTokenProvider()
            };
            app.UseOAuthAuthorizationServer(options);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
        }

    }
}
