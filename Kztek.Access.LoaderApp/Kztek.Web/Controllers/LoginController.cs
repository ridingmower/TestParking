using Kztek.Data.Event.SqlHelper;
using Kztek.Data.SqlHelper;
using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Security;
using Kztek.Service.Admin;
using Kztek.Web.Core.Extensions;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Helpers;
using Kztek.Web.Core.Models;
using Kztek.Web.Models;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Kztek.Web.Controllers
{
    public class LoginController : Controller
    {
        #region Khai báo services

        private IUserService _UserService;
        private IRoleService _RoleService;
        private IUserRoleService _UserRoleService;
        private ItblUserService _tblUserService;
        private ItblSystemConfigService _tblSystemConfigService;

        public LoginController(IUserService _UserService, IRoleService _RoleService, IUserRoleService _UserRoleService, ItblUserService _tblUserService, ItblSystemConfigService _tblSystemConfigService)
        {
            this._UserService = _UserService;
            this._RoleService = _RoleService;
            this._UserRoleService = _UserRoleService;
            this._tblUserService = _tblUserService;
            this._tblSystemConfigService = _tblSystemConfigService;
        }

        #endregion Khai báo services

        #region Đăng nhập

        [HttpGet]
        public ActionResult Index()
        {
            updatetblUserToUser();
            ViewBag.System = _tblSystemConfigService.GetDefault();
            return View(new AdminLoginModel());
        }

        [HttpPost]
        public async Task<ActionResult> LoginADFS(FormCollection form)
        {
            var code = form["code"];

            var adfs_user = await ADFS_Helper.GetUserInfo(code);

            return View("Index");
        }

        [HttpPost]
        public ActionResult Index(AdminLoginModel obj, bool remember, FormCollection form, string areacode)
        {
            //if (SecureDongleProvider.CheckHardKey() == false)
            //{
            //    ModelState.AddModelError("UserName", "Cannot find dongle");
            //    return View(obj);
            //}
            ViewBag.System = _tblSystemConfigService.GetDefault();
            //Kiểm tra hợp lệ
            if (ModelState.IsValid)
            {
                //Kiểm tra có phải Superadmin
                if (obj.UserName == SecurityModel.Username)
                {
                    var password = ConfigurationManager.AppSettings["FUTECHSUPPORTPASS"];
                    password = CryptoProvider.SimpleDecryptWithPassword(password, SecurityModel.Keypass);

                    if (obj.Password == password)
                    {
                        var tAdmin = Common.SuperAdmin();

                        createSessionSuperAdmin(tAdmin);

                        return RedirectToAction("IndexAdmin", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("Password", "Mật khẩu không đúng");
                        return View(obj);
                    }
                }

                //Lấy khách hàng qua email
                var userInfo = _UserService.GetByUserNameOREmail(obj.UserName);

                //Không có thì báo lỗi
                if (userInfo == null)
                {
                    ModelState.AddModelError("UserName", "UserName không tồn tại hoặc nhập sai. Vui lòng kiểm tra lại!");
                    return View();
                }
                else
                {
                    //Kiểm tra tài khoản đã kích hoạt
                    if (userInfo.Active)
                    {
                        //Có thì so khớp password
                        var password = obj.Password.PasswordHashed(userInfo.PasswordSalat);
                        if (!password.Equals(userInfo.Password))
                        {
                            ModelState.AddModelError("", "Email hoặc mật khẩu không đúng");
                            return View(obj);
                        }
                        else
                        {
                            //Khớp thì lưu vào session
                            createSession(remember, userInfo);

                            MessageReport report1 = new MessageReport(true, "Đăng nhập thành công " + userInfo.Name);
                            WriteLog.Write(report1, GetCurrentUser.GetUser(), userInfo.Id, userInfo.Username, "User", ConfigurationManager.AppSettings["FunctionGroupViewDefault"].ToString(), ActionConfigO.Login);

                            return RedirectToAction("Index", "Home", new { Area = ConfigurationManager.AppSettings["FunctionGroupViewDefault"].ToString() });
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Tài khoản chưa được kích hoạt");
                        return View(obj);
                    }
                }
            }

            ModelState.AddModelError("error", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            return View(obj);

            ////Khai báo
            //try
            //{
                
            //}
            //catch (Exception ex)
            //{
            //    return View("Error", new HandleErrorInfo(ex, "Login", "Index"));
            //}
        }

        #endregion Đăng nhập

        #region Đăng xuất

        /// <summary>
        /// Đăng xuất tài khoản
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            var host = Request.Url.Host;
            Session.Abandon();
            var cookie = new HttpCookie(string.Format("{0}_{1}", SessionConfig.MemberCookies, host));
            if (cookie == null)
                return RedirectToAction("Index", "Login", new { Area = "" });

            cookie.Expires = DateTime.Now.AddDays(-1d);
            Response.Cookies.Add(cookie);

            CacheLayer.Clear(ConstField.AllListMenuFunctionCache);

            return RedirectToAction("Index", "Login", new { Area = "" });
        }

        #endregion Đăng xuất

        #region Create Sesion + Cookie

        /// <summary>
        /// Lưu Session
        /// </summary>
        /// <param name="rememberMe"></param>
        /// <param name="passwordEntered"></param>
        /// <param name="user"></param>
        [NonAction]
        private void createSession(bool rememberMe, User user)
        {
            var host = Request.Url.Host;
            // Create user login
            var userLogin = new User
            {
                Id = user.Id,
                Name = user.Name,
                Admin = user.Admin,
                UserAvatar = user.UserAvatar,
                Username = user.Username
                //Password = user.Password
            };

            Session[string.Format("{0}_{1}", SessionConfig.MemberSession, host)] = userLogin;
            if (rememberMe)
            {
                var cookies = new HttpCookie(string.Format("{0}_{1}", SessionConfig.MemberCookies, host));
                cookies["cp_UserId"] = user.Id.ToString(CultureInfo.InvariantCulture);
                cookies.Expires = DateTime.Now.AddMonths(1);
                Response.Cookies.Add(cookies);
            }

            //Xóa cache cũ để cập nhật mới nhất
            var formatUserId = string.Format("{0}_{1}", Kztek.Web.Core.Models.ConstField.MemCacheMember, user.Id);

            CacheLayer.Clear(formatUserId);
        }

        #endregion Create Sesion + Cookie

        #region reCaptcha

        public bool reCaptCha(FormCollection form)
        {
            //Thông tin google recaptcha
            string secret = ConfigurationManager.AppSettings["SecretKey"];
            var response = form["g-recaptcha-response"];

            //Khai báo
            var client = new WebClient();
            var reply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));
            var captchaResponse = JsonConvert.DeserializeObject<RecaptchaResult>(reply);

            //Bắt lỗi
            if (!captchaResponse.Success)
            {
                //Lấy lỗi
                var error = captchaResponse.ErrorCodes[0].ToLower();
                //Kiểm tra xem là lỗi nào để thông báo
                switch (error)
                {
                    case ("missing-input-secret"):
                        TempData["CaptChaMessage"] = "The secret parameter is missing.";
                        break;

                    case ("invalid-input-secret"):
                        TempData["CaptChaMessage"] = "The secret parameter is invalid or malformed.";
                        break;

                    case ("missing-input-response"):
                        TempData["CaptChaMessage"] = "The response parameter is missing.";
                        break;

                    case ("invalid-input-response"):
                        TempData["CaptChaMessage"] = "The response parameter is invalid or malformed.";
                        break;

                    default:
                        TempData["CaptChaMessage"] = "Error occured. Please try again";
                        break;
                }
            }

            //Trả kết quả
            return Convert.ToBoolean(captchaResponse.Success);
        }

        #endregion reCaptcha

        #region Check số lần đăng nhập khi đăng nhập thất bại

        private int CheckLogin(string ip)
        {
            int i = 0;
            if (Session[string.Format("{0}_{1}", "Login", ip)] != null)
            {
                i = Convert.ToInt16(Session[string.Format("{0}_{1}", "Login", ip)]);
                i++;
                Session[string.Format("{0}_{1}", "Login", ip)] = i;
            }
            else
            {
                i++;
                Session[string.Format("{0}_{1}", "Login", ip)] = i;
            }
            return i;
        }

        #endregion Check số lần đăng nhập khi đăng nhập thất bại

        #region Đăng ký

        /// <summary>
        /// Form đăng ký
        /// </summary>
        /// <modified>
        /// Author              Date            Comments
        /// TrungNQ             18/12/2016      Tạo mới
        /// </modified>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Thực hiện đăng ký
        /// </summary>
        /// <modified>
        /// Author              Date            Comments
        /// TrungNQ             18/12/2016      Tạo mới
        /// </modified>
        /// <param name="obj"></param>
        /// <param name="active"></param>
        /// <param name="admin"></param>
        /// <param name="repass"></param>
        /// <param name="agree"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Register(User obj, bool active, bool admin, string repass)
        {
            //Kiểm tra
            if (!string.IsNullOrWhiteSpace(obj.Password) || !string.IsNullOrWhiteSpace(repass))
            {
                var existedEmail = _UserService.GetByEmail(obj.Email);
                if (existedEmail != null)
                {
                    ModelState.AddModelError("", "Email đã tồn tại");
                    return View();
                }

                //Kiểm tra email tồn tại
                var objExist = _UserService.GetByUserName(obj.Username);
                if (objExist != null)
                {
                    ModelState.AddModelError("", "Username đã tồn tại");
                    return View();
                }
                else
                {
                    if (obj.Password.Equals(repass))
                    {
                        //Gán
                        obj.Id = Common.GenerateId();
                        obj.PasswordSalat = Guid.NewGuid().ToString();
                        obj.Password = obj.Password.PasswordHashed(obj.PasswordSalat);
                        obj.DateCreated = DateTime.Now;
                        obj.Active = active;
                        obj.Admin = admin;

                        //Kiểm tra hợp lệ
                        if (ModelState.IsValid)
                        {
                            var isSuccess = _UserService.Create(obj);
                            if (isSuccess.isSuccess)
                            {
                                GenerateData(obj.Id);

                                TempData["Success"] = isSuccess.Message;
                                return RedirectToAction("Index", "Login");
                            }
                            else
                            {
                                ModelState.AddModelError("", isSuccess.Message);
                                return View();
                            }
                        }
                        ModelState.AddModelError("", "Có lỗi xảy ra. Vui lòng kiểm tra lại");
                        return View();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Nhập lại đúng mật khẩu");
                        return View();
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Vui lòng nhập mật khẩu và nhập lại mật khẩu.");
                return View();
            }
        }

        #endregion Đăng ký

        #region Clean session login khi đã đăng nhập thành công

        private void CleanSession(string ip)
        {
            Session.Remove(string.Format("{0}_{1}", "Login", ip));
        }

        #endregion Clean session login khi đã đăng nhập thành công

        #region Generate data

        /// <summary>
        /// Generate data khi đăng ký tài khoản đầu tiên
        /// </summary>
        /// <param name="userid"></param>
        public void GenerateData(string userid)
        {
            //Data
            //string script = StringUtil.GetTextFile(Server.MapPath("~/Templates/DataGenerate/DataGenerate.sql"));
            //ExcuteSQL.Execute(script);

            //Trigger
            string strInsert = StringUtil.GetTextFile(Server.MapPath("~/Templates/DataGenerate/TriggerInsertMenuFunction.sql"));
            ExcuteSQL.Execute(strInsert);
            string strUpdate = StringUtil.GetTextFile(Server.MapPath("~/Templates/DataGenerate/TriggerUpdateMenuFunction.sql"));
            ExcuteSQL.Execute(strUpdate);

            var list = _RoleService.GetAllActive();
            foreach (var item in list.ToList())
            {
                UserRole objJoin = new UserRole();
                objJoin.Id = Common.GenerateId();
                objJoin.UserId = userid;
                objJoin.RoleId = item.Id;
                _UserRoleService.Create(objJoin);
            }
        }

        #endregion Generate data

        #region Check tài khoản

        /// <summary>
        /// Check tài khoản login
        /// </summary>
        /// <param name="email"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        public JsonResult CheckAccount(string username, string pass)
        {
            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(pass))
            {
                var user = _UserService.GetByUserNameOREmail(username);
                if (user != null)
                {
                    pass = pass.PasswordHashed(user.PasswordSalat);
                    if (pass.Equals(user.Password))
                    {
                        return Json(new { isSuccess = true, userid = user.Id, admin = user.Admin }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(new { isSuccess = false, userid = "", admin = false });
        }

        #endregion Check tài khoản

        private void changeTypeColumn()
        {
            //tblGate
            FunctionHelper.ExecuteChangeColumnType("tblGate", "GateID");
        }

        private void updatetblUserToUser()
        {
            //Cấu trúc bảng mới MVC
            string script01 = System.IO.File.ReadAllText(Server.MapPath("~/uploads/file/Data_MVC.sql"));

            var t01 = ExcuteSQL.Execute(script01);

            //Cấu trúc bảng mới MVC
            string script01_1 = System.IO.File.ReadAllText(Server.MapPath("~/uploads/file/Data_MVC_ParkingEvent.sql"));

            var t01_1 = Kztek.Data.Event.SqlHelper.ExcuteSQLEvent.Execute(script01_1);

            //Cấu trúc bổ sung của tblUser
            string script02 = System.IO.File.ReadAllText(Server.MapPath("~/uploads/file/Structure_tblUser.sql"));

            var t02 = ExcuteSQL.Execute(script02);

            //Cấu trúc để với hệ thống iAccessEvent
            string script03 = System.IO.File.ReadAllText(Server.MapPath("~/uploads/file/Data_MVC_AccessEvent.sql"));

            var t03 = Kztek.Data.AccessEvent.SqlHelper.ExcuteSQLEvent.Execute(script03);

            //Bảng mới cho Access
            string script04 = System.IO.File.ReadAllText(Server.MapPath("~/uploads/file/Data_MVC_Access.sql"));

            var t04 = ExcuteSQL.Execute(script04);

            //BẢNG MỚI CHO TRƯỜNG CHINH
            string script05 = System.IO.File.ReadAllText(Server.MapPath("~/uploads/file/Structure_NoteFreeTRANSERCO.sql"));

            var t05 = ExcuteSQLEvent.Execute(script05);

            //Cấu trúc bảng phần Locker model - Phần nạp thẻ cố định
            string script06 = System.IO.File.ReadAllText(Server.MapPath("~/uploads/file/Data_MVC_Locker.sql"));

            var t06 = ExcuteSQL.Execute(script06);

            //Cấu trúc bảng phần Locker Event model
            string script07 = System.IO.File.ReadAllText(Server.MapPath("~/uploads/file/Data_MVC_LockerEvent.sql"));

            var t07 = Kztek.Data.LockerEvent.SqlHelper.ExcuteSQLEvent.Execute(script07);

            //Chuyển dữ liệu tblUser -> User
            var list = _tblUserService.GetAll();
            if (list.Any())
            {
                foreach (var item in list)
                {
                    var obj = _UserService.GetById(item.UserID.ToString());
                    if (obj == null)
                    {
                        var tNew = new User();
                        tNew.Id = item.UserID.ToString();
                        tNew.Username = item.UserName;
                        tNew.Active = !Convert.ToBoolean(item.IsLock);
                        tNew.Admin = item.IsSystem;
                        tNew.DateCreated = DateTime.Now;
                        tNew.Name = item.FullName;
                        tNew.PasswordSalat = Guid.NewGuid().ToString();
                        tNew.IsDeleted = false;
                        tNew.Email = "";

                        var pass = CryptorEngine.Decrypt(item.Password, true);

                        tNew.Password = pass.PasswordHashed(tNew.PasswordSalat);

                        _UserService.Create(tNew);
                    }
                }

                
            }

            //Gán quyền BAOVE
            //var listNewUser = _UserService.GetAllActive();
            //foreach (var item in listNewUser)
            //{
            //    var UserRole = new UserRole()
            //    {
            //        Id = Common.GenerateId(),
            //        RoleId = "",
            //        UserId = item.Id
            //    };
            //}
        }

        /// <summary>
        /// Lưu Session
        /// </summary>
        /// <param name="rememberMe"></param>
        /// <param name="passwordEntered"></param>
        /// <param name="user"></param>
        [NonAction]
        private void createSessionSuperAdmin(AdminLoginModel obj)
        {
            var host = Request.Url.Host;
            // Create user login
            var userLogin = new AdminLoginModel
            {
                UserName = obj.UserName,
            };

            Session[string.Format("{0}_{1}", SessionConfig.SuperAdminSession, host)] = userLogin;
        }

        public ActionResult LogoutSuperAdmin()
        {
            var host = Request.Url.Host;
            Session.Abandon();

            //Session[string.Format("{0}_{1}", SessionConfig.SuperAdminSession, host)]

            return RedirectToAction("Index", "Login", new { Area = "" });
        }
    }
}