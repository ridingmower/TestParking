using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Web.Core.Extensions;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Helpers;
using Kztek.Web.Core.Models;
using Kztek.Web.Attributes;
using Kztek.Web.Models;
using System;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Kztek.Data.SqlHelper;

namespace Kztek.Web.Controllers
{
    public class UserController : Controller
    {
        #region Khai báo services

        /// <summary>
        /// Khai báo services
        /// </summary>
        private IUserService _UserService;

        private IMenuFunctionService _MenuFunctionService;
        private IUserRoleService _UserRoleService;
        private IRoleService _RoleService;
        private ItblSystemConfigService _tblSystemConfigService;
        private ItblCardGroupService _tblCardGroupService;
        private IUser_AuthGroupService _User_AuthGroupService;
        public UserController(IUserService _UserService, IMenuFunctionService _MenuFunctionService, IUserRoleService _UserRoleService, IRoleService _RoleService, ItblSystemConfigService _tblSystemConfigService, IUser_AuthGroupService _User_AuthGroupService, ItblCardGroupService _tblCardGroupService)
        {
            this._UserService = _UserService;
            this._MenuFunctionService = _MenuFunctionService;
            this._UserRoleService = _UserRoleService;
            this._RoleService = _RoleService;
            this._tblSystemConfigService = _tblSystemConfigService;
            this._User_AuthGroupService = _User_AuthGroupService;
            this._tblCardGroupService = _tblCardGroupService;
        }

        #endregion Khai báo services

        private static string GroupID = "";

        public List<SelectListModel> CardGroupToDDL()  //bind ServicePackageId to dropdownlist
        {
            var list = new List<SelectListModel>();
            var listCardGroup = _tblCardGroupService.GetAllActive_v2().ToList();
            if (listCardGroup.Any())
            {
                foreach (var item in listCardGroup)
                {
                    list.Add(new SelectListModel { ItemValue = item.CardGroupID.ToString(), ItemText = item.CardGroupName });
                }
            }
            return list;
        }

        #region Danh sách

        /// <summary>
        /// Danh sách
        /// </summary>
        /// <modified>
        /// Author              Date            Comments
        /// TrungNQ             10/12/2016      Tạo mới
        /// </modified>
        /// <param name="key">Từ khóa</param>
        /// <param name="active">Trạng thái</param>
        /// <param name="page">Trang hiện tại</param>
        /// <returns></returns>
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Index(string key, int? page, string group = "")
        {
            //Khai báo
            int pageNumber = (page ?? 1);
            int pageSize = 20;

            //Lấy danh sách phân trang
            var list = _UserService.GetAllPagingByFirst(key, pageNumber, pageSize);
            //Đổ lên grid
            var gridModel = PageModelCustom<User>.GetPage(list, pageNumber, pageSize);
            //ViewBag
            ViewBag.Key = key;
            ViewBag.GroupID = group;
            GroupID = group;

            ViewBag.selectedUserValue = GetSession();

            //Đưa ra giao diện
            return View(gridModel);
        }
        public PartialViewResult RoleList(string userId)
        {
            var roles = _RoleService.GetAllByUserId(userId).ToList();
            return PartialView(roles);
        }
        #endregion Danh sách

        #region Thêm mới

        /// <summary>
        /// Giao diện thêm mới
        /// </summary>
        /// <modified>
        /// Author          Date            Comments
        /// TrungNQ         04/08/2017      Tạo mới
        /// </modified>
        /// <returns></returns>
        [CheckSessionLogin]
        [CheckAuthorize]
        [HttpGet]
        public ActionResult Create(string group = "")
        {
            ViewBag.Selected = "";
            ViewBag.GroupID = group;
            ViewBag.isAuthInView = _tblSystemConfigService.GetDefault().isAuthInView;
            return View();
        }

        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <modified>
        /// Author          Date            Comments
        /// TrungNQ         04/08/2017      Tạo mới
        /// </modified>
        /// <param name="obj">Đối tượng</param>
        /// <param name="rolevalues">Giá trị quyền</param>
        /// <param name="FileUpload">File Upload</param>
        /// <param name="SaveAndCountinue">Tiếp tục thêm</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(User obj, string repass, string rolevalues,string authcardgroupid, HttpPostedFileBase FileUpload, bool SaveAndCountinue = false, string group = "")
        {
            var dictornary = FunctionHelper.GetLocalizeDictionary("Home", "notification");
            //
            ViewBag.Selected = rolevalues;
            ViewBag.GroupID = group;
            ViewBag.isAuthInView = _tblSystemConfigService.GetDefault().isAuthInView;
            //Kiểm tra
            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            var isExisted = _UserService.GetByUserName(obj.Username);
            if (isExisted != null)
            {
                ModelState.AddModelError("Username", dictornary["User_name_already_exists"]);
                return View(obj);
            }

            //Gán giá trị
            obj.Id = Common.GenerateId();
            obj.DateCreated = DateTime.Now;
            obj.PasswordSalat = Guid.NewGuid().ToString();
            obj.IsDeleted = false;

            if (!string.IsNullOrWhiteSpace(obj.Password))
            {
                if (!string.IsNullOrWhiteSpace(repass))
                {
                    if (obj.Password.Equals(repass))
                    {
                        obj.Password = obj.Password.PasswordHashed(obj.PasswordSalat);
                    }
                    else
                    {
                        ModelState.AddModelError("repass", dictornary["re_enter_pass_correct"]);
                        return View(obj);
                    }
                }
                else
                {
                    ModelState.AddModelError("repass", dictornary["re_enter_pass"]);
                    return View(obj);
                }
            }
            else
            {
                obj.Password = "";
                obj.Password = obj.Password.PasswordHashed(obj.PasswordSalat);
            }

            //Thêm mới danh sách quyền
            if (!string.IsNullOrWhiteSpace(rolevalues))
            {
                var ids = rolevalues.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (ids.Any())
                {
                    foreach (var id in ids)
                    {
                        var UserRole = new UserRole();
                        UserRole.Id = Common.GenerateId();
                        UserRole.RoleId = id;
                        UserRole.UserId = obj.Id;

                        _UserRoleService.Create(UserRole);
                    }
                }
            }


            //File upload
            if (FileUpload != null)
            {
                var extension = Path.GetExtension(FileUpload.FileName) ?? "";
                var fileName = Path.GetFileName(string.Format("{0}{1}", StringUtil.RemoveSpecialCharactersVn(FileUpload.FileName.Replace(extension, "")).GetNormalizeString(), extension));
                obj.UserAvatar = string.Format("{0}{1}", ConfigurationManager.AppSettings["FileUploadAvatar"], fileName);
            }

            //Thực hiện thêm mới
            var result = _UserService.Create(obj);
            if (result.isSuccess)
            {
                //FunctionHelper.UploadImage(FileUpload, Server.MapPath(ConfigurationManager.AppSettings["uploadfolder"]));
                //tạo phân quyền nhóm thẻ
                CreateUserAuthGroup(obj.Id, authcardgroupid);

                var name = FunctionHelper.getCurrentGroup(group);

                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.Id, obj.Username, "User", name, ActionConfigO.Create);

                if (SaveAndCountinue)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction("Create");
                }
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(obj);
            }
        }

        #endregion Thêm mới

        #region Cập nhật

        /// <summary>
        /// Giao diện cập nhật
        /// </summary>
        /// <modified>
        /// Author          Date            Comments
        /// TrungNQ         04/08/2017      Tạo mới
        /// </modified>
        /// <param name="id">Id bản ghi</param>
        /// <param name="pageNumber">Trang hiện tại</param>
        /// <returns></returns>
        [CheckSessionLogin]
        [CheckAuthorize]
        [HttpGet]
        public ActionResult Update(string id, int pageNumber = 1, string group = "")
        {
            var str = "";

            ViewBag.PN = pageNumber;

            var obj = _UserService.GetById(id);

            var list = _UserRoleService.GetAllByUserId(obj.Id);
            if (list.Any())
            {
                foreach (var item in list)
                {
                    str += item.RoleId + ",";
                }
            }

            ViewBag.Selected = str;
            ViewBag.GroupID = group;
            ViewBag.isAuthInView = _tblSystemConfigService.GetDefault().isAuthInView;
            if(obj != null)
            {
                var userauthgroup = _User_AuthGroupService.GetByUserId(obj.Id);
                ViewBag.CardGroupIds = userauthgroup != null ? userauthgroup.CardGroupIds : "";
            }
            
            return View(obj);
        }

        /// <summary>
        /// Cập nhật
        /// </summary>
        /// <modified>
        /// Author          Date            Comments
        /// TrungNQ         04/08/2017      Tạo mới
        /// </modified>
        /// <param name="obj">Đối tượng</param>
        /// <param name="objId">Id bản ghi hiện tại</param>
        /// <param name="pass">Mật khẩu</param>
        /// <param name="rolevalues">Danh sách quyền</param>
        /// <param name="FileUpload">File Upload</param>
        /// <param name="pageNumber">Trang hiện tại</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Update(User obj, string pass, string repass, string rolevalues, string authcardgroupid, HttpPostedFileBase FileUpload, int pageNumber = 1, string group = "")
        {
            var dictonary = FunctionHelper.GetLocalizeDictionary("Home", "notification");
            //
            ViewBag.PN = pageNumber;
            ViewBag.Selected = rolevalues;
            ViewBag.GroupID = group;
            ViewBag.isAuthInView = _tblSystemConfigService.GetDefault().isAuthInView;
            //Kiểm tra
            var oldObj = _UserService.GetById(obj.Id);
            if (oldObj == null)
            {
                ViewBag.Error = dictonary["record_does_not_exist"];
                return View(obj);
            }

            if (!ModelState.IsValid)
            {
                return View(oldObj);
            }

            var isExisted = _UserService.GetByUserName_Id(oldObj.Username, oldObj.Id.ToString());
            if (isExisted != null)
            {
                ModelState.AddModelError("Username", dictonary["User_name_already_exists"]);
                return View(oldObj);
            }

            //Gán giá trị
            oldObj.Username = obj.Username;
            oldObj.Active = obj.Active;
            oldObj.Admin = obj.Admin;
            oldObj.Name = obj.Name;

            //Kiểm tra là tài khoản admin
            var currentUser = GetCurrentUser.GetUser();
            if (currentUser.Admin)
            {
                if (oldObj.Id.ToString().Equals(currentUser.Id.ToString()))
                {
                    //oldObj.isAdmin = obj.isAdmin;

                    CreatSession(oldObj);
                }
            }

            //Cập nhật quyền
            if (!string.IsNullOrWhiteSpace(rolevalues))
            {
                var list = _UserRoleService.GetAllByUserId(oldObj.Id).ToList();
                foreach (var item in list)
                {
                    _UserRoleService.DeleteById(item.Id);
                }

                var ids = rolevalues.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (ids.Any())
                {
                    foreach (var id in ids)
                    {
                        var UserRole = new UserRole();
                        UserRole.Id = Common.GenerateId();
                        UserRole.RoleId = id;
                        UserRole.UserId = obj.Id;

                        _UserRoleService.Create(UserRole);
                    }
                }
            }

            //Password
            if (!string.IsNullOrWhiteSpace(pass))
            {
                if (!string.IsNullOrWhiteSpace(repass))
                {
                    if (pass.Equals(repass))
                    {
                        oldObj.Password = pass.PasswordHashed(oldObj.PasswordSalat);
                    }
                    else
                    {
                        ModelState.AddModelError("repass", dictonary["re_enter_pass_correct"]);
                        return View(oldObj);
                    }
                }
                else
                {
                    ModelState.AddModelError("repass", dictonary["re_enter_pass"]);
                    return View(oldObj);
                }
            }

            //File upload
            if (FileUpload != null)
            {
                var extension = Path.GetExtension(FileUpload.FileName) ?? "";
                var fileName = Path.GetFileName(string.Format("{0}{1}", StringUtil.RemoveSpecialCharactersVn(FileUpload.FileName.Replace(extension, "")).GetNormalizeString(), extension));
                oldObj.UserAvatar = string.Format("{0}{1}", ConfigurationManager.AppSettings["FileUploadAvatar"], fileName);
            }

            //Thực hiện cập nhật
            var result = _UserService.Update(oldObj);
            if (result.isSuccess)
            {
                //tạo phân quyền nhóm thẻ
                CreateUserAuthGroup(oldObj.Id, authcardgroupid);

                var name = FunctionHelper.getCurrentGroup(group);
                
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.Id, obj.Username, "User", name, ActionConfigO.Update);

                if(FileUpload != null)
                {
                    var error = "";
                    Common.UploadFile(out error, Server.MapPath(ConfigurationManager.AppSettings["uploadfolder"]), FileUpload);
                }
               
                return RedirectToAction("Index", new { page = pageNumber });
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(oldObj);
            }
        }

        #endregion Cập nhật

        #region Xóa

        /// <summary>
        /// Xóa
        /// </summary>
        /// <modified>
        /// Author          Date            Comments
        /// TrungNQ         04/08/2017      Tạo mới
        /// </modified>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult Delete(string id)
        {
            var obj = _UserService.GetById(id);
            if (obj.Admin)
            {
                var message = new MessageReport();

                message.isSuccess = false;
                message.Message = FunctionHelper.GetLocalizeDictionary("Home", "notification")["Admin_account"];

                return Json(message, JsonRequestBehavior.AllowGet);
            }

            var result = _UserService.DeleteById(id);
            if (result.isSuccess)
            {
                var name = FunctionHelper.getCurrentGroup(GroupID);

                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.Id, obj.Username, "User", name, ActionConfigO.Delete);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion Xóa

        #region Danh sách quyền hạn trong thêm mới, cập nhật

        /// <summary>
        /// Danh sách quyền
        /// </summary>
        /// <modified>
        /// Author          Date            Comments
        /// TrungNQ         04/08/2017      Tạo mới
        /// </modified>
        /// <param name="roles">Danh sách quyền đã chọn</param>
        /// <returns></returns>
        public PartialViewResult RoleListChoice(string roles)
        {
            ViewBag.Selected = roles;
            var list = _RoleService.GetAllActive();
            return PartialView(list);
        }

        #endregion Danh sách quyền hạn trong thêm mới, cập nhật

        private void CreatSession(User obj)
        {
            var host = Request.Url.Host;
            // Create user login
            var userLogin = new User
            {
                Id = obj.Id,
                Admin = obj.Admin,
                UserAvatar = obj.UserAvatar,
                Username = obj.Username
            };

            Session[string.Format("{0}_{1}", SessionConfig.MemberSession, host)] = userLogin;
        }

        #region Cập nhập thông tin người dùng đăng nhập

        /// <summary>
        /// Form sửa thông tin người dùng đăng nhập
        /// </summary>
        /// <modified>
        /// Author              Date            Comments
        /// TrungNQ             10/12/2016      Tạo mới
        /// </modified>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [CheckSessionLogin]
        public ActionResult UserProfile(string id, int tabIndex = 1, string direct = "", string group = "")
        {
            ViewBag.TabIndex = tabIndex;
            ViewBag.Direct = direct;
            ViewBag.GroupId = group;

            var objGroup = FunctionHelper.GroupMenuList().FirstOrDefault(n => n.ItemValue.Equals(group));
            var objGroupArea = objGroup != null ? objGroup.AreaName : "";

            var obj = _UserService.GetById(id);
            if (obj != null)
            {
                return View(obj);
            }
            else
            {
                return RedirectToAction("Index", "Home", new { area = objGroupArea });
            }
        }

        /// <summary>
        /// Thực hiện cập nhật
        /// </summary>
        /// <modified>
        /// Author              Date            Comments
        /// TrungNQ             10/12/2016      Tạo mới
        /// </modified>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UserProfile(User obj, string oldpass, string newpass, string repass, HttpPostedFileBase fileAvatar, FormCollection formCollection, string areacode, int tabIndex = 1, string direct = "", string group = "", string groupListID = "")
        {
            var dictonary = FunctionHelper.GetLocalizeDictionary("Home", "notification");
            ViewBag.Direct = direct;
            ViewBag.GroupId = group;

            var groups = ConfigurationManager.AppSettings["FunctionGroupAllow"].ToString();
            var listGroup = FunctionHelper.GroupMenuList().Where(n => groups.Contains(n.ItemValue)).ToList();
            ViewBag.GroupList = listGroup.Where(n => groupListID.Contains(n.ItemValue)).ToList();

            //Khai báo biến
            string error = string.Empty;

            var oldObj = _UserService.GetById(obj.Id);
            if (oldObj == null)
            {
                ViewBag.Error = dictonary["record_does_not_exist"];
                return View(obj);
            }

            var usernameExist = _UserService.GetByUserName_Id(obj.Username, oldObj.Id);
            if (usernameExist != null)
            {
                ViewBag.Error = dictonary["Username_already_exists"];
                return View(oldObj);
            }

            if (!string.IsNullOrWhiteSpace(obj.Email))
            {
                //Kiểm tra tồn tại email ở bản ghi khác
                var objExist = _UserService.GetByEmail_Id(obj.Email, oldObj.Id);

                //Tồn tại thì báo trùng
                if (objExist != null)
                {
                    ViewBag.Error = dictonary["Email_already_exists"];
                    return View(oldObj);
                }
            }

            if (fileAvatar != null)
            {
                oldObj.UserAvatar = string.Format("{0}/{1}", ConfigurationManager.AppSettings["FileUploadAvatar"], Common.UploadImages(out error, Server.MapPath(ConfigurationManager.AppSettings["FileUploadAvatar"]), fileAvatar));
            }

            oldObj.Username = obj.Username;
            oldObj.Name = obj.Name;
            oldObj.Email = obj.Email;
            oldObj.Phone = obj.Phone;

            if (!string.IsNullOrWhiteSpace(newpass) && !string.IsNullOrWhiteSpace(repass))
            {
                oldpass = oldpass.PasswordHashed(oldObj.PasswordSalat);

                //So khớp
                if (oldpass.Equals(oldObj.Password))
                {
                    //Kiểm tra đã nhập đúng lại mật khẩu mới chưa
                    if (newpass.Equals(repass))
                    {
                        oldObj.Password = newpass;
                        oldObj.Password = oldObj.Password.PasswordHashed(oldObj.PasswordSalat);
                    }
                    else
                    {
                        ViewBag.Error = dictonary["enter_new_password"];
                        return View(oldObj);
                    }
                }
                else
                {
                    ViewBag.Error = dictonary["enter_old_password"];
                    return View(oldObj);
                }
            }

            var isSuccess = _UserService.Update(oldObj);
            if (isSuccess.isSuccess)
            {

                //Xóa cache
                var formatUserId = string.Format("{0}_{1}", Kztek.Web.Core.Models.ConstField.MemCacheMember, oldObj.Id);

                CacheLayer.Clear(formatUserId);

                //ViewBag.Message = "Cập nhật thành công";
                //return View(oldObj);
                TempData["Success"] = dictonary["updateSuccess"];
                return RedirectToAction("UserProfile", "User", new { id = obj.Id, tabIndex = tabIndex, direct = direct, group = group });
            }
            else
            {
                ViewBag.Error = dictonary["err"];
                return View(oldObj);
            }
        }

        #endregion Cập nhập thông tin người dùng đăng nhập

        #region Restore mật khẩu

        /// <summary>
        /// Reset mật khẩu về mđ "123456"
        /// </summary>
        /// <modified>
        /// Author          Date            Comments
        /// TrungNQ         04/08/2017      Tạo mới
        /// </modified>
        /// <param name="id">Id bản ghi</param>
        /// <returns></returns>
        public JsonResult RestorePassToDefault(string id)
        {
            var obj = _UserService.GetById(id);
            if (obj == null)
            {
                return Json(new MessageReport(false, FunctionHelper.GetLocalizeDictionary("Home", "notification")["record_does_not_exist"]), JsonRequestBehavior.AllowGet);
            }

            var newpass = "123456";

            obj.Password = newpass.PasswordHashed(obj.PasswordSalat);
            var result = _UserService.Update(obj);
            if (result.isSuccess)
            {
                var name = FunctionHelper.getCurrentGroup(GroupID);
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.Id, obj.Username, "User", name, ActionConfigO.Restore);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion Restore mật khẩu

        public PartialViewResult ModalButtonControl(int totalItem = 0, string url = "")
        {
            var host = Request.Url.Host;

            var listUserChoice = (List<string>)Session[string.Format("{0}_{1}", SessionConfig.UserChoiceActionParkingSession, host)];

            ViewBag.totalItemValue = totalItem;
            ViewBag.urlValue = url;

            ViewBag.roles = _RoleService.GetAllActive().ToDataTableNullable();

            if (listUserChoice == null)
            {
                listUserChoice = new List<string>();
            }

            return PartialView(listUserChoice);
        }

        public PartialViewResult ShowUserSelected(string User)
        {
            var listUserChoice = GetSession();

            return PartialView(listUserChoice);
        }

        public JsonResult RemoveAllUserSeleted()
        {
            var host = Request.Url.Host;
            Session[string.Format("{0}_{1}", SessionConfig.UserChoiceActionParkingSession, host)] = new List<string>();

            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddOrRemoveOneAllUserSeleted(List<string> lUsers)
        {
            GetSetFromSession(lUsers);

            return Json("", JsonRequestBehavior.AllowGet);
        }

        private List<string> GetSetFromSession(List<string> list)
        {
            var host = Request.Url.Host;

            var listUserChoice = (List<string>)Session[string.Format("{0}_{1}", SessionConfig.UserChoiceActionParkingSession, host)];
            if (listUserChoice != null)
            {
                //if (list != null)
                //{
                //    foreach (var item in list)
                //    {
                //        if (!listUserChoice.Any(n => n.Equals(item)))
                //        {
                //            listUserChoice.Add(item);
                //        }
                //    }

                //}
                listUserChoice = list;
            }
            else
            {
                if (list != null)
                {
                    listUserChoice = list;
                }
                else
                {
                    listUserChoice = new List<string>();
                }
            }

            Session[string.Format("{0}_{1}", SessionConfig.UserChoiceActionParkingSession, host)] = listUserChoice;

            return listUserChoice;
        }
        private List<string> GetSession()
        {
            var host = Request.Url.Host;

            var listUserChoice = (List<string>)Session[string.Format("{0}_{1}", SessionConfig.UserChoiceActionParkingSession, host)];

            return listUserChoice;
        }
        public JsonResult ActionToUsers(string type, string roles)
        {
            var result = new MessageReport();
            result.isSuccess = false;
            result.Message = FunctionHelper.GetLocalizeDictionary("Home", "notification")["err"];

            if (type == "Authorize")
            {
                if (string.IsNullOrWhiteSpace(roles))
                {
                    result.isSuccess = false;
                    result.Message = FunctionHelper.GetLocalizeDictionary("Home", "notification")["choose_authority"];

                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }

            var data = GetSession();
            if (data != null && data.Any())
            {
                result = ActionProcess(type, roles, data);
            }
            else
            {
                result.isSuccess = false;
                result.Message = FunctionHelper.GetLocalizeDictionary("Home", "notification")["choose_user"];
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private MessageReport ActionProcess(string type, string roles, List<string> data)
        {
            var result = new MessageReport();
            result.isSuccess = false;
            result.Message = FunctionHelper.GetLocalizeDictionary("Home", "notification")["err"];

            var lRoles = roles.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            try
            {
                switch (type)
                {
                    case "Authorize":
                        foreach (var item in data)
                        {
                            var list = _UserRoleService.GetAllByUserId(item).ToList();
                            foreach (var itemRole in list)
                            {
                                _UserRoleService.DeleteById(itemRole.Id);
                            }

                            if (lRoles.Any())
                            {
                                foreach (var itemMap in lRoles)
                                {
                                    var UserRole = new UserRole();
                                    UserRole.Id = Common.GenerateId();
                                    UserRole.RoleId = itemMap;
                                    UserRole.UserId = item;

                                    var re = _UserRoleService.Create(UserRole);
                                }
                            }
                        }

                        break;
                    default:
                        break;
                }

                result.isSuccess = true;
                result.Message = FunctionHelper.GetLocalizeDictionary("Home", "notification")["updateSuccess"];
            }
            catch (Exception ex)
            {
                result.isSuccess = false;
                result.Message = ex.Message;
            }

            return result;
        }

        #region Phân quyền nhóm thẻ
        /// <summary>
        /// danh sách nhóm thẻ
        /// </summary>
        /// <param name="CardGroupIds"></param>
        /// <returns></returns>
        public PartialViewResult Partial_AuthGroup(string CardGroupIds)
        {      
            var lstCardGroup = new List<string>();

            if (!string.IsNullOrEmpty(CardGroupIds))
            {
                var arr = CardGroupIds.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                if (arr.Length > 0)
                {
                    foreach (var item in arr)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            lstCardGroup.Add(item);
                        }
                    }
                }
            }

            ViewBag.LSTCardGroup = lstCardGroup;

            var dt = CardGroupToDDL().ToDataTableNullable();
           
            return PartialView(dt);
        }

        /// <summary>
        /// tạo mới, cập nhật phân quyền nhóm thẻ
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="cardgroupIds"></param>
        public void CreateUserAuthGroup(string userid,string cardgroupIds)
        {
            var objsystem = _tblSystemConfigService.GetDefault();
            if(objsystem != null && objsystem.isAuthInView && !string.IsNullOrEmpty(userid))
            {
                var userauthgroup = _User_AuthGroupService.GetByUserId(userid);
                if(userauthgroup == null)
                {
                    userauthgroup = new User_AuthGroup
                    {
                        Id = Common.GenerateId(),
                        UserId = userid,
                        CardGroupIds = cardgroupIds
                    };
                    _User_AuthGroupService.Create(userauthgroup);
                }
                else
                {
                    userauthgroup.CardGroupIds = cardgroupIds;
                    _User_AuthGroupService.Update(userauthgroup);
                }
            }
        }
        #endregion
    }
}