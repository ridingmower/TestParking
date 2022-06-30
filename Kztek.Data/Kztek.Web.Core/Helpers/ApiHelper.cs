using Kztek.Security;
using Kztek.Web.Core.Models;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Web.Core.Helpers
{
    public class ApiHelper
    {
        public static HttpClient client;

        static ApiHelper()
        {
            client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(10);
        }

        public static string GenerateJSON_MobileToken(string userid)
        {
            //
            var now = DateTime.Now;
            var expire = now.AddHours(24);

            //

            var Issuer = "KztekJSC";

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityModel.Keypass));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                Issuer,
                Issuer,
                new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userid),
                },
                expires: expire,
                signingCredentials: credentials);

            var mo = new TokenModel()
            {
                Identifier = userid,
                Expires_In = (int)(expire - now).TotalMinutes,
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };

            return JsonConvert.SerializeObject(mo);
        }

        public static Task<T> ConvertResponse<T>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var t = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
                return Task.FromResult(t);
            }

            return null;
        }

        public static async Task<HttpResponseMessage> HttpGet(string uri, string authorization = "Bearer", string token = "")
        {
            var url = uri;//AppSettingHelper.GetStringFromAppSetting("ConnectionStrings:Host_Api").Result + uri;

            try
            {
                if (!string.IsNullOrWhiteSpace(authorization) && !string.IsNullOrWhiteSpace(token))
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authorization, token);

                return await client.GetAsync(url);
            }
            catch (System.Exception ex)
            {
                var re = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ex.Message)
                };

                return await Task.FromResult(re);
            }
        }

        public static async Task<HttpResponseMessage> HttpPost<T>(string uri, T obj, string authorization = "Bearer", string token = "")
        {
            var url = uri; // AppSettingHelper.GetStringFromAppSetting("ConnectionStrings:Host_Api").Result + uri;

            try
            {
                if (!string.IsNullOrWhiteSpace(authorization) && !string.IsNullOrWhiteSpace(token))
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authorization, token);

                var content = JsonConvert.SerializeObject(obj);

                var data = new StringContent(content, Encoding.UTF8, "application/json");

                return await client.PostAsync(url, data);
            }
            catch (System.Exception ex)
            {
                var re = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ex.Message)
                };

                return await Task.FromResult(re);
            }
        }

        public static async Task<HttpResponseMessage> HttpPut<T>(string uri, T obj, string authorization = "Bearer", string token = "")
        {
            var url = uri; //AppSettingHelper.GetStringFromAppSetting("ConnectionStrings:Host_Api").Result + uri;

            try
            {
                if (!string.IsNullOrWhiteSpace(authorization) && !string.IsNullOrWhiteSpace(token))
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authorization, token);

                var content = JsonConvert.SerializeObject(obj);

                var data = new StringContent(content, Encoding.UTF8, "application/json");

                return await client.PutAsync(url, data);
            }
            catch (System.Exception ex)
            {
                var re = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ex.Message)
                };

                return await Task.FromResult(re);
            }

        }

        public static async Task<HttpResponseMessage> HttpDelete(string uri, string authorization = "Bearer", string token = "")
        {
            var url = uri; // AppSettingHelper.GetStringFromAppSetting("ConnectionStrings:Host_Api").Result + uri;

            try
            {
                if (!string.IsNullOrWhiteSpace(authorization) && !string.IsNullOrWhiteSpace(token))
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authorization, token);

                return await client.DeleteAsync(url);
            }
            catch (System.Exception ex)
            {
                var re = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(ex.Message)
                };

                return await Task.FromResult(re);
            }
        }

        //public static async Task<HttpResponseMessage> HttpGet(HttpContext context, string uri)
        //{
        //    var token = await SessionCookieHelper.CurrentToken(context);

        //    var url = AppSettingHelper.GetStringFromAppSetting("ConnectionStrings:Host_Api").Result + uri;

        //    try
        //    {
        //        using (HttpClient client = new HttpClient())
        //        {
        //            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token != null ? token.Token : "");

        //            return await client.GetAsync(url);
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static async Task<HttpResponseMessage> HttpPost<T>(HttpContext context, string uri, T obj)
        //{
        //    var token = await SessionCookieHelper.CurrentToken(context);

        //    var url = AppSettingHelper.GetStringFromAppSetting("ConnectionStrings:Host_Api").Result + uri;

        //    try
        //    {
        //        var client = new HttpClient();
        //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token != null ? token.Token : "");

        //        var content = JsonConvert.SerializeObject(obj);

        //        var data = new StringContent(content, Encoding.UTF8, "application/json");

        //        return await client.PostAsync(url, data);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public static async Task<HttpResponseMessage> HttpPut<T>(HttpContext context, string uri, T obj)
        //{
        //     var token = await SessionCookieHelper.CurrentToken(context);

        //    var url = AppSettingHelper.GetStringFromAppSetting("ConnectionStrings:Host_Api").Result + uri;

        //    try
        //    {
        //        var client = new HttpClient();
        //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token != null ? token.Token : "");

        //        var content = JsonConvert.SerializeObject(obj);

        //        var data = new StringContent(content, Encoding.UTF8, "application/json");

        //        return await client.PutAsync(url, data);

        //    }
        //    catch (System.Exception ex)
        //    {
        //        throw ex;
        //    }

        //}

        //public static async Task<HttpResponseMessage> HttpDelete(HttpContext context, string uri)
        //{
        //    var token = await SessionCookieHelper.CurrentToken(context);

        //    var url = AppSettingHelper.GetStringFromAppSetting("ConnectionStrings:Host_Api").Result + uri;

        //    var result = new MessageReport(false, "error");

        //    try
        //    {
        //        var client = new HttpClient();
        //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token != null ? token.Token : "");

        //        return await client.DeleteAsync(url);

        //    }
        //    catch (System.Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}
