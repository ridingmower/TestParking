using Kztek.Model.Models;
using Kztek.Security;
using Kztek.Service.Admin;
using Kztek.Web.Attributes;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Helpers;
using Kztek.Web.Core.Models;
using Kztek.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kztek.Web.Areas.Parking.Controllers
{
    public class tblFtpAccountController : Controller
    {
        private ItblFtpAccountService _tblFtpAccountService;

        public tblFtpAccountController(ItblFtpAccountService _tblFtpAccountService)
        {
            this._tblFtpAccountService = _tblFtpAccountService;
        }

        private const string groupId = ConstField.ParkingID;

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Index(string key, int page = 1, string group = "", string selectedId = "")
        {
            var pageSize = 20;

            var list = _tblFtpAccountService.GetPagingByFirst(key, page, pageSize);

            var gridModel = PageModelCustom<tblFtpAccount>.GetPage(list, page, pageSize);

            ViewBag.keyValue = key;
            ViewBag.groupValue = group;
            ViewBag.selectedIdValue = selectedId;

            return View(gridModel);
        }

        #region Thêm mới

        /// <summary>
        /// Giao diện thêm mới
        /// </summary>
        /// <modified>
        /// Author              Date            Comments
        /// TrungNQ             01/09/2017      Tạo mới
        /// </modified>
        /// <returns></returns>
        [CheckSessionLogin]
        [CheckAuthorize]
        [HttpGet]
        public ActionResult Create(string group = "", string key = "")
        {
            ViewBag.groupValue = group;
            ViewBag.keyValue = key;

            return View();
        }

        /// <summary>
        /// Thực hiện thêm mới
        /// </summary>
        /// <modified>
        /// Author              Date            Comments
        /// TrungNQ             01/09/2017      Tạo mới
        /// </modified>
        /// <param name="obj">Đối tượng</param>
        /// <param name="SaveAndCountinue">Tiếp tục thêm</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(tblFtpAccount obj, bool SaveAndCountinue = false, string group = "", string key = "")
        {
            ViewBag.groupValue = group;
            ViewBag.keyValue = key;

            //Kiểm tra
            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            //
            if (string.IsNullOrWhiteSpace(obj.FtpHost))
            {
                ModelState.AddModelError("FtpHost", FunctionHelper.GetLocalizeDictionary("Home", "notification")["enter_name"]);
                return View(obj);
            }

            //Gán giá trị
            obj.Id = Common.GenerateId();
            obj.FtpPass = CryptoProvider.SimpleEncryptWithPassword(obj.FtpPass, SecurityModel.Session_Key);

            //Thực hiện thêm mới
            var result = _tblFtpAccountService.Create(obj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.Id, obj.FtpHost, "tblFtpAccount", ConstField.ParkingCode, ActionConfigO.Create);

                if (SaveAndCountinue)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction("Create", new { group = group, key = key, selectedId = obj.Id });
                }

                return RedirectToAction("Index", new { group = group, key = key, selectedId = obj.Id });
            }
            else
            {
                return View(obj);
            }
        }

        #endregion Thêm mới

        #region Cập nhật

        /// <summary>
        /// Cập nhật
        /// </summary>
        /// <modified>
        /// Author              Date            Comments
        /// TrungNQ             01/09/2017      Tạo mới
        /// </modified>
        /// <param name="id">Id bản ghi</param>
        /// <param name="pageNumber">Trang hiện tại</param>
        /// <returns></returns>
        [CheckSessionLogin]
        [CheckAuthorize]
        [HttpGet]
        public ActionResult Update(string id, int page = 1, string group = "", string key = "")
        {
            var obj = _tblFtpAccountService.GetById(id);

            ViewBag.PN = page;
            ViewBag.groupValue = group;
            ViewBag.keyValue = key;

            if (obj != null)
            {
                obj.FtpPass = CryptoProvider.SimpleDecryptWithPassword(obj.FtpPass, SecurityModel.Session_Key);
            }

            return View(obj);
        }

        /// <summary>
        /// Thực hiện cập nhật
        /// </summary>
        /// <modified>
        /// Author              Date            Comments
        /// TrungNQ             01/09/2017      Tạo mới
        /// </modified>
        /// <param name="obj">Đối tượng</param>
        /// <param name="objId">Id bản ghi</param>
        /// <param name="pageNumber">Trang hiện tại</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Update(tblFtpAccount obj, int page = 1, string group = "", string key = "")
        {
            //
            ViewBag.PN = page;
            ViewBag.groupValue = group;
            ViewBag.keyValue = key;

            //Kiểm tra
            var oldObj = _tblFtpAccountService.GetById(obj.Id);
            if (oldObj == null)
            {
                ViewBag.Error = FunctionHelper.GetLocalizeDictionary("Home", "notification")["record_does_not_exist"];
                return View(obj);
            }

            //
            if (string.IsNullOrWhiteSpace(obj.FtpHost))
            {
                ModelState.AddModelError("FtpHost", FunctionHelper.GetLocalizeDictionary("Home", "notification")["enter_name"]);
                return View(oldObj);
            }

            if (!ModelState.IsValid)
            {
                return View(oldObj);
            }

            oldObj.FtpHost = obj.FtpHost;
            oldObj.FtpPass = obj.FtpPass;
            oldObj.FtpUser = obj.FtpUser;
            oldObj.FtpPass = CryptoProvider.SimpleEncryptWithPassword(obj.FtpPass, SecurityModel.Session_Key);
            //oldObj.SortOrder = obj.SortOrder;

            //Thực hiện cập nhật
            var result = _tblFtpAccountService.Update(oldObj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.Id, obj.FtpHost, "tblFtpAccount", ConstField.ParkingCode, ActionConfigO.Update);

                return RedirectToAction("Index", new { group = group, key = key, page = page, selectedId = obj.Id });
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
        /// Author              Date            Comments
        /// TrungNQ             01/09/2017      Tạo mới
        /// </modified>
        /// <param name="id">Id bản ghi</param>
        /// <returns></returns>
        public JsonResult Delete(string id)
        {
            var obj = new tblFtpAccount();

            var result = _tblFtpAccountService.DeleteById(id, ref obj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.Id.ToString(), obj.FtpHost, "tblFtpAccount", ConstField.ParkingCode, ActionConfigO.Delete);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion Xóa
    }
}