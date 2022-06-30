using Kztek.Model.CustomModel;
using Kztek.Security;
using Kztek.Web.Core.Extensions;
using System;
using System.IO;
using System.Net;
using System.Web;

namespace Kztek.Web.Core.Helpers
{
    public class Common
    {
        //TK superadmin - Dùng để cấu hình hiển thị menufunction
        public static string passwordSalatAdmin = "8b4b1ac9-d077-4906-8d31-0b6fa482a6d3";

        public static AdminLoginModel SuperAdmin()
        {
            var passwordSalat = passwordSalatAdmin;

            var obj = new AdminLoginModel();
            obj.UserName = SecurityModel.Username;

            return obj;
        }

        public static int[] ConvertStringNumberArrayToInt(string[] arrStringNumber)
        {
            var length = arrStringNumber.Length;
            var countItem = 0;
            for (var i = 0; i < length; i++)
            {
                if (!String.IsNullOrEmpty(arrStringNumber[i]))
                    countItem++;
            }

            var arrInt = new int[countItem];
            for (var i = 0; i < length; i++)
            {
                if (!String.IsNullOrEmpty(arrStringNumber[i]))
                    arrInt[i] = int.Parse(arrStringNumber[i]);
            }

            return arrInt;
        }

        public static string GenerateId()
        {
            return MD52INT(Guid.NewGuid().ToString()).ToString();
        }

        public static int MD52INT(string MDKEY)
        {
            int TOTAL = 0;
            for (int i = 0; i < MDKEY.Length - 1; i++)
            {
                TOTAL += CHAR2INT(MDKEY.Substring(i, 1));
            }
            return TOTAL;
        }

        private static int CHAR2INT(string MYCHAR)
        {
            int NUM = 100;
            switch (MYCHAR)
            {
                case "A":
                    NUM = 19902;
                    break;

                case "B":
                    NUM = 15604;
                    break;

                case "C":
                    NUM = 17505;
                    break;

                case "D":
                    NUM = 15562;
                    break;

                case "E":
                    NUM = 18752;
                    break;

                case "F":
                    NUM = 1765712;
                    break;

                case "0":
                    NUM = 155675;
                    break;

                case "1":
                    NUM = 26767;
                    break;

                case "2":
                    NUM = 2567562;
                    break;

                case "3":
                    NUM = 15675692;
                    break;

                case "4":
                    NUM = 2567532;
                    break;

                case "5":
                    NUM = 1575682;
                    break;

                case "6":
                    NUM = 535392;
                    break;

                case "7":
                    NUM = 15354346;
                    break;

                case "8":
                    NUM = 1723427;
                    break;

                case "9":
                    NUM = 1342399;
                    break;
            }
            return NUM;
        }

        public static string UploadImages(out string error, string fullFolder, HttpPostedFileBase fileUpload)
        {
            var isValid = true;
            var fileName = string.Empty;
            var filePathSaved = string.Empty;
            var e = "";
            //var url = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["CategoryImagesPath"]);
            var name = fileUpload.FileName;
            if (string.Compare(fileUpload.ContentType, "image/png", true) != 0 &&
                string.Compare(fileUpload.ContentType, "image/jpeg", true) != 0 &&
                string.Compare(fileUpload.ContentType, "image/gif", true) != 0)
            {
                e = "Bạn phải sử dụng file ảnh có định dạng jpg, png, gif.";
                isValid = false;
            }
            if (isValid)
            {
                try
                {
                    var extension = Path.GetExtension(fileUpload.FileName) ?? "";
                    fileName = Path.GetFileName(string.Format("{0}{1}", StringUtil.RemoveSpecialCharactersVn(name.Replace(extension, "")).GetNormalizeString(), extension));
                    if (!Directory.Exists(fullFolder)) Directory.CreateDirectory(fullFolder);
                    fileUpload.SaveAs(Path.Combine(fullFolder, fileName ?? ""));
                    filePathSaved = fileName;
                }
                catch (Exception ex)
                {
                    e = ex.Message;
                }
            }
            error = e;
            return filePathSaved;
        }

        public static string UploadFile(out string error, string fullFolder, HttpPostedFileBase fileUpload)
        {
            var isValid = true;
            var fileName = string.Empty;
            var filePathSaved = string.Empty;
            var e = "";
            var name = fileUpload.FileName;

            if (isValid)
            {
                try
                {
                    var extension = Path.GetExtension(fileUpload.FileName) ?? "";
                    fileName = Path.GetFileName(string.Format("{0}{1}", StringUtil.RemoveSpecialCharactersVn(name.Replace(extension, "")).GetNormalizeString(), extension));
                    if (!Directory.Exists(fullFolder)) Directory.CreateDirectory(fullFolder);
                    fileUpload.SaveAs(Path.Combine(fullFolder, fileName ?? ""));
                    filePathSaved = fileName;
                }
                catch (Exception ex)
                {
                    e = ex.Message;
                }
            }

            error = e;
            return filePathSaved;
        }

        public static string UploadFileWithDatetime(out string error, string fullFolder, HttpPostedFileBase fileUpload)
        {
            var isValid = true;
            var fileName = string.Empty;
            var filePathSaved = string.Empty;
            var e = "";
            var name = fileUpload.FileName;

            if (isValid)
            {
                try
                {
                    var extension = Path.GetExtension(fileUpload.FileName) ?? "";

                    fileName = Path.GetFileName(string.Format("{0}_{2}{1}", StringUtil.RemoveSpecialCharactersVn(name.Replace(extension, "")).GetNormalizeString(), extension, DateTime.Now.ToString("ddMMyyyyHHmm")));

                    if (!Directory.Exists(fullFolder)) Directory.CreateDirectory(fullFolder);

                    fileUpload.SaveAs(Path.Combine(fullFolder, fileName ?? ""));

                    filePathSaved = fileName;
                }
                catch (Exception ex)
                {
                    e = ex.Message;
                }
            }

            error = e;
            return filePathSaved;
        }

        public static string UploadVideo(out string error, string fullFolder, HttpPostedFileBase fileUpload)
        {
            var isValid = true;
            var fileName = string.Empty;
            var filePathSaved = string.Empty;
            var e = "";
            //var url = HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["CategoryImagesPath"]);
            var name = fileUpload.FileName;
            if (string.Compare(fileUpload.ContentType, "video/mp4", true) != 0 &&
                string.Compare(fileUpload.ContentType, "video/avi", true) != 0 &&
                string.Compare(fileUpload.ContentType, "video/wmv", true) != 0)
            {
                e = "Bạn phải sử dụng file video có định dạng mp4, avi, wmv.";
                isValid = false;
            }
            if (isValid)
            {
                try
                {
                    var extension = Path.GetExtension(fileUpload.FileName) ?? "";
                    fileName = Path.GetFileName(string.Format("{0}{1}", StringUtil.RemoveSpecialCharactersVn(name.Replace(extension, "")).GetNormalizeString(), extension));
                    if (!Directory.Exists(fullFolder)) Directory.CreateDirectory(fullFolder);
                    fileUpload.SaveAs(Path.Combine(fullFolder, fileName ?? ""));
                    filePathSaved = fileName;
                }
                catch (Exception ex)
                {
                    e = ex.Message;
                }
            }
            error = e;
            return filePathSaved;
        }

        /// <summary>
        /// Xóa folder cụ thể (Xóa cả sub folder + file bên trong)
        /// </summary>
        /// <param name="fullfolder">Đường dẫn: C:\\Folder/filedownload.định dạng</param>
        public static void DeleteFolder(string fullfolder)
        {
            if (Directory.Exists(fullfolder))
            {
                Directory.Delete(fullfolder, true);
            }
        }

        /// <summary>
        /// Download file trong folder
        /// </summary>
        /// <param name="fullfolder">Đường dẫn: C:\\Folder/filedownload.định dạng</param>
        /// <param name="filename">Tên file + định dạng</param>
        public static void DownloadFile(string fullfolder, string filename)
        {
            bool isExist = System.IO.File.Exists(fullfolder);

            if (isExist)
            {
                WebClient wc = new WebClient();
                HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", filename));
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                byte[] data = wc.DownloadData(fullfolder);
                HttpContext.Current.Response.BinaryWrite(data);
                HttpContext.Current.Response.End();
            }
        }

        /// <summary>
        /// Xóa file cụ thể trong folder
        /// </summary>
        /// <param name="fullfolder">Đường dẫn: C:\\Folder/file xóa.định dạng</param>
        public static void DeleteFile(string fullfolder)
        {
            if (System.IO.File.Exists(fullfolder))
            {
                try
                {
                    System.IO.File.Delete(fullfolder);
                }
                catch (System.IO.IOException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public static string GetComputerName(string clientIP)
        {
            try
            {
                var hostEntry = Dns.GetHostEntry(clientIP);
                return hostEntry.HostName;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static string GenerateDateString(string prefix, string count)
        {
            var name = "";

            name = string.Format("{0}{1}{2}", prefix, DateTime.Now.ToString("yyyyMMdd"), count);

            return name;
        }

        public static string GenerateDateString(string prefix, int count, DateTime date)
        {
            var name = "";

            name = string.Format("{0}{1}{2}", prefix, date.ToString("yyyyMMdd"), count);

            return name;
        }

        public static string GenerateDateString(string prefix, string count, DateTime date)
        {
            var name = "";

            name = string.Format("{0}{1}{2}", prefix, date.ToString("yyyyMMdd"), count);

            return name;
        }
    }
}