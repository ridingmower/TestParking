using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;

namespace Kztek.Web.Core.Extensions
{
    public static class ExtensionMethodHelper
    {
        // Thumb anh co goi cau hinh tu web.config
        public static string ImagePathThumb(this string str, string hostContainName, int width, int height)
        {
            var hostContainImages = ConfigurationManager.AppSettings[hostContainName];
            var imgPath = string.IsNullOrEmpty(str) ? "" : string.Format("{0}{1}?width={2}&height={3}", hostContainImages, str, width, height);
            return imgPath;
        }

        public static string ImagePathThumbCus(this string str, int width, int height)
        {
            var imgPath = string.IsNullOrEmpty(str) ? "" : string.Format("{0}?width={1}&height={2}", str, width, height);
            return imgPath;
        }

        // Thumb anh khong goi cau hinh tu web.config
        public static string ImagePathThumb(this string str, int width, int height)
        {
            var imgPath = string.IsNullOrEmpty(str) ? "" : string.Format("{0}?width={1}&height={2}", str, width, height);
            //check null
            try
            {
                if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath("~" + str)))
                {
                    imgPath = string.Format("/Content/Image/default.jpg?width={0}&height={1}", width, height);
                }
            }
            catch { imgPath = string.Format("/Content/Image/default.jpg?width={0}&height={1}", width, height); }

            return imgPath;
        }

        // Thumb anh ve kichs thuoc mac dinh
        public static string ImagePathThumbDefault(this string str, string hostContainName)
        {
            var hostContainImages = ConfigurationManager.AppSettings[hostContainName];
            var imgPath = string.IsNullOrEmpty(str) ? "" : string.Format("{0}{1}", hostContainImages, str);
            return imgPath;
        }
        //Check guid
        public static bool CheckGuid(string guids)
        {
            var isGuid = new Regex(@"^({){0,1}[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}(}){0,1}$"
                    , RegexOptions.Compiled);
            var check = false;
            if (isGuid.IsMatch(guids))
            {
                check = true;
            }
            return check;
        }

        public static string ReplaceSpecialAsEmpty(this string str)
        {
            str = str.Trim().Replace(" ", "-");
            var replaced = Regex.Replace(str, @"[^\d\w\s*]", "-");
            return replaced;
        }

        public static string ReplaceSpecialAsEmpty(this string str, string rep)
        {
            str = str.Trim().Replace(" ", rep);
            var replaced = Regex.Replace(str, @"[^\d\w\s*]", rep);
            return replaced;
        }

        public static string FormatMoney(this string str)
        {
            long Money = -1;

            long.TryParse(str, out Money);

            if (Money < 0) return "0";
            else
            {
                return string.Format("{0:n0}", Money);
            }
        }
    }
}