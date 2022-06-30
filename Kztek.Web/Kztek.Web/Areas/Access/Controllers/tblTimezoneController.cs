using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Web.Attributes;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Models;
using Kztek.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kztek.Web.Areas.Access.Controllers
{
    public class tblTimezoneController : Controller
    {
        #region Khai báo services

        private ItblAccessTimezoneService _tblAccessTimezoneService;
        private static string url = "";

        public tblTimezoneController(ItblAccessTimezoneService _tblAccessTimezoneService)
        {
            this._tblAccessTimezoneService = _tblAccessTimezoneService;
        }

        #endregion Khai báo services

        #region Danh sách

        /// <summary>
        /// Danh sách thời gian
        /// </summary>
        /// <modified>
        /// Author              Date            Comments
        /// TrungNQ             16/09/2017      Tạo mới
        /// </modified>
        /// <param name="key">Từ khóa</param>
        /// <param name="page">Trang hiện tại</param>
        /// <returns></returns>
        [CheckSessionLogin]
        public ActionResult Index(string key = "", int page = 1)
        {
            var pageSize = 20;

            var list = _tblAccessTimezoneService.GetAllPagingByFirst(key, page, pageSize);

            var gridModel = PageModelCustom<tblAccessTimezone>.GetPage(list, page, pageSize);

            ViewBag.KeyWord = key;
            url = Request.Url.PathAndQuery;

            return View(gridModel);
        }

        #endregion Danh sách

        #region Thêm mới

        /// <summary>
        /// Giao diện thêm mới
        /// </summary>
        /// <modified>
        /// Author              Date            Comments
        /// TrungNQ             16/09/2017      Tạo mới
        /// </modified>
        /// <returns></returns>
        [CheckSessionLogin]
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.urlValue = url ?? Request.Url.PathAndQuery;

            return View();
        }

        /// <summary>
        /// Thực hiện thêm mới
        /// </summary>
        /// <modified>
        /// Author              Date            Comments
        /// TrungNQ             16/09/2017      Tạo mới
        /// </modified>
        /// <param name="obj">Đối tượng</param>
        /// <param name="SaveAndCountinue">Tiếp tục hay không?</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(tblAccessTimezoneSubmit obj, bool SaveAndCountinue = false)
        {
            ViewBag.urlValue = url ?? Request.Url.PathAndQuery;

            //Kiểm tra
            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            //Gán giá trị
            var newObj = new tblAccessTimezone();
            newObj.TimezoneName = obj.TimeZoneName;
            newObj.TimeZoneID = obj.TimeZoneID;

            //Mon
            if (!string.IsNullOrWhiteSpace(obj.MonFrom) && !string.IsNullOrWhiteSpace(obj.MonTo))
            {
                newObj.Mon = string.Format("{0}-{1}", obj.MonFrom, obj.MonTo);
            }
            //Tue
            if (!string.IsNullOrWhiteSpace(obj.TueFrom) && !string.IsNullOrWhiteSpace(obj.TueTo))
            {
                newObj.Tue = string.Format("{0}-{1}", obj.TueFrom, obj.TueTo);
            }
            //Wed
            if (!string.IsNullOrWhiteSpace(obj.WedFrom) && !string.IsNullOrWhiteSpace(obj.WedTo))
            {
                newObj.Wed = string.Format("{0}-{1}", obj.WedFrom, obj.WedTo);
            }
            //Thu
            if (!string.IsNullOrWhiteSpace(obj.ThuFrom) && !string.IsNullOrWhiteSpace(obj.ThuTo))
            {
                newObj.Thu = string.Format("{0}-{1}", obj.ThuFrom, obj.ThuTo);
            }
            //Fri
            if (!string.IsNullOrWhiteSpace(obj.FriFrom) && !string.IsNullOrWhiteSpace(obj.FriTo))
            {
                newObj.Fri = string.Format("{0}-{1}", obj.FriFrom, obj.FriTo);
            }
            //Sat
            if (!string.IsNullOrWhiteSpace(obj.SatFrom) && !string.IsNullOrWhiteSpace(obj.SatTo))
            {
                newObj.Sat = string.Format("{0}-{1}", obj.SatFrom, obj.SatTo);
            }
            //Sun
            if (!string.IsNullOrWhiteSpace(obj.SunFrom) && !string.IsNullOrWhiteSpace(obj.SunTo))
            {
                newObj.Sun = string.Format("{0}-{1}", obj.SunFrom, obj.SunTo);
            }

            newObj.Inactive = obj.Inactive;

            //Thực hiện thêm mới
            var result = _tblAccessTimezoneService.Create(newObj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), newObj.Id.ToString(), newObj.TimezoneName, "tblAccessTimezone", ConstField.AccessControlCode, ActionConfigO.Create);

                if (SaveAndCountinue)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction("Create");
                }

                return Redirect(url);
            }
            else
            {
                return View(obj);
            }
        }

        #endregion Thêm mới

        #region Cập nhật

        /// <summary>
        /// Giao diện cập nhật
        /// </summary>
        /// <modified>
        /// Author              Date            Comments
        /// TrungNQ             16/09/2017      Tạo mới
        /// </modified>
        /// <param name="id"> Id bản ghi </param>
        /// <param name="pageNumber">Trang hiện tại</param>
        /// <returns></returns>
        [CheckSessionLogin]
        [HttpGet]
        public ActionResult Update(int id)
        {
            var obj = _tblAccessTimezoneService.GetMappingById(id);

            ViewBag.urlValue = url ?? Request.Url.PathAndQuery;

            return View(obj);
        }

        /// <summary>
        /// Thực hiện cập nhật
        /// </summary>
        /// <modified>
        /// Author              Date            Comments
        /// TrungNQ             16/09/2017      Tạo mới
        /// </modified>
        /// <param name="obj">Đối tượng</param>
        /// <param name="objId">Id bản ghi</param>
        /// <param name="pageNumber">Trang hiện tại</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Update(tblAccessTimezoneSubmit obj)
        {
            ViewBag.urlValue = url ?? Request.Url.PathAndQuery;

            //Kiểm tra
            var oldObj = _tblAccessTimezoneService.GetById(obj.Id);
            if (oldObj == null)
            {
                ViewBag.Error = "Bản ghi không tồn tại";
                return View(obj);
            }

            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            //Gán giá trị
            oldObj.TimezoneName = obj.TimeZoneName;
            oldObj.TimeZoneID = obj.TimeZoneID;

            //Mon
            if (!string.IsNullOrWhiteSpace(obj.MonFrom) && !string.IsNullOrWhiteSpace(obj.MonTo))
            {
                oldObj.Mon = string.Format("{0}-{1}", obj.MonFrom, obj.MonTo);
            }
            //Tue
            if (!string.IsNullOrWhiteSpace(obj.TueFrom) && !string.IsNullOrWhiteSpace(obj.TueTo))
            {
                oldObj.Tue = string.Format("{0}-{1}", obj.TueFrom, obj.TueTo);
            }
            //Wed
            if (!string.IsNullOrWhiteSpace(obj.WedFrom) && !string.IsNullOrWhiteSpace(obj.WedTo))
            {
                oldObj.Wed = string.Format("{0}-{1}", obj.WedFrom, obj.WedTo);
            }
            //Thu
            if (!string.IsNullOrWhiteSpace(obj.ThuFrom) && !string.IsNullOrWhiteSpace(obj.ThuTo))
            {
                oldObj.Thu = string.Format("{0}-{1}", obj.ThuFrom, obj.ThuTo);
            }
            //Fri
            if (!string.IsNullOrWhiteSpace(obj.FriFrom) && !string.IsNullOrWhiteSpace(obj.FriTo))
            {
                oldObj.Fri = string.Format("{0}-{1}", obj.FriFrom, obj.FriTo);
            }
            //Sat
            if (!string.IsNullOrWhiteSpace(obj.SatFrom) && !string.IsNullOrWhiteSpace(obj.SatTo))
            {
                oldObj.Sat = string.Format("{0}-{1}", obj.SatFrom, obj.SatTo);
            }
            //Sun
            if (!string.IsNullOrWhiteSpace(obj.SunFrom) && !string.IsNullOrWhiteSpace(obj.SunTo))
            {
                oldObj.Sun = string.Format("{0}-{1}", obj.SunFrom, obj.SunTo);
            }

            oldObj.Inactive = obj.Inactive;

            //Thực hiện cập nhật
            var result = _tblAccessTimezoneService.Update(oldObj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), oldObj.Id.ToString(), oldObj.TimezoneName, "tblAccessTimezone", ConstField.AccessControlCode, ActionConfigO.Update);

                return Redirect(url);
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(obj);
            }
        }

        #endregion Cập nhật

        #region Xóa

        /// <summary>
        /// Xóa
        /// </summary>
        /// <modified>
        /// Author              Date            Comments
        /// TrungNQ             16/09/2017      Tạo mới
        /// </modified>
        /// <param name="id">Id bản ghi</param>
        /// <returns></returns>
        public JsonResult Delete(int id)
        {
            var result = _tblAccessTimezoneService.DeleteById(id);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), id.ToString(), id.ToString(), "tblAccessTimezone", ConstField.AccessControlCode, ActionConfigO.Delete);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion Xóa
    }
}