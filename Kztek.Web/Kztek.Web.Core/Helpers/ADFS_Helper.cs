using Kztek.Model.CustomModel.ADFS;
using Kztek.Model.Models;
using Kztek.Model.Models.API;
using Kztek.Web.Core.Functions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Kztek.Web.Core.Helpers
{
    public static class ADFS_Helper
    {
        private static string ADFSClientId { get => ConfigurationManager.AppSettings.Get("ADFSClientId"); }

        private static string ADFSClientSecret { get => ConfigurationManager.AppSettings.Get("ADFSClientSecret"); }

        private static string ADFSAuthority { get => ConfigurationManager.AppSettings.Get("ADFSAuthority"); }

        private static string ADFSDiscoveryDoc { get => ConfigurationManager.AppSettings.Get("ADFSDiscoveryDoc"); }

        private static string ADFSDiscoveryKey { get => ConfigurationManager.AppSettings.Get("ADFSDiscoveryKey"); }

        private static string ADFSPostLogoutRedirectUri { get => ConfigurationManager.AppSettings.Get("ADFSPostLogoutRedirectUri"); }

        private static string ADFSRedirect { get => ConfigurationManager.AppSettings.Get("ADFSRedirect"); }

        static ADFS_Helper()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
        }

        public static async Task<string> GetLoginUrl()
        {
            var uri = $"{ADFSAuthority}/oauth2/authorize?client_id={ADFSClientId}&response_type=code&response_mode=form_post&redirect_uri={ADFSRedirect}";

            return await Task.FromResult(uri);
        }

        public static async Task<ADFS_User> GetUserInfo(string code)
        {
            ADFS_User result = null;
            try
            {
                WebClient wc = new WebClient();
                NameValueCollection parameters = new NameValueCollection();
                parameters.Add("client_id", ADFSClientId);
                parameters.Add("client_secret", ADFSClientSecret);
                parameters.Add("code", code);
                parameters.Add("redirect_uri", ADFSRedirect);
                parameters.Add("scope", "openid");
                parameters.Add("grant_type", "authorization_code");
                string responseStr = Encoding.UTF8.GetString(wc.UploadValues(ADFSAuthority.TrimEnd('/') + "/oauth2/token", parameters));
                dynamic json = JsonConvert.DeserializeObject<dynamic>(responseStr);

                result = new ADFS_User()
                {
                    Id_Token = json["id_token"],
                    Refresh_Token = json["refresh_token"],
                    Access_Token = json["access_token"]
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var parsedJwt = tokenHandler.ReadToken(result.Id_Token) as JwtSecurityToken;
                var a = tokenHandler.ReadToken(result.Access_Token) as JwtSecurityToken;
                result.Email = parsedJwt.Claims.Where(c => c.Type == "upn")
                              .Select(c => c.Value).FirstOrDefault(); // email user login 
            }
            catch { }

            return await Task.FromResult(result);
        }
    }
}
