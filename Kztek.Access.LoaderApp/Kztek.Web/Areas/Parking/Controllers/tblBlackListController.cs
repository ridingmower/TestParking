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

namespace Kztek.Web.Areas.Parking.Controllers
{
    public class tblBlackListController : Controller
    {
        private ItblBlackListService _tblBlackListService;
        public tblBlackListController(ItblBlackListService _tblBlackListService)
        {
            this._tblBlackListService = _tblBlackListService;
        }

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Index(string key, int page = 1, string group = "", string selectedId = "")
        {
            var pageSize = 20;

            var list = _tblBlackListService.GetPagingByFirst(key, page, pageSize);

            var gridModel = PageModelCustom<tblBlackList>.GetPage(list, page, pageSize);

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
        public ActionResult Create(tblBlackList obj, bool SaveAndCountinue = false, string group = "", string key = "")
        {
            ViewBag.groupValue = group;
            ViewBag.keyValue = key;

            //Kiểm tra
            if (!ModelState.IsValid)
            {
                return View(obj);
            }
            if (String.IsNullOrEmpty(obj.Plate)||String.IsNullOrWhiteSpace(obj.Plate))
            {
                ViewBag.Error = FunctionHelper.GetLocalizeDictionary("Home", "notification")["enter_Number_plate"];
                return View(obj);
            }

            var objExisted = _tblBlackListService.GetByName_Plate(obj.Name, obj.Plate);
            if (objExisted.Any())
            {
                ViewBag.Error = FunctionHelper.GetLocalizeDictionary("Home", "notification")["exists_Number_plate"] ;
                return View(obj);
            }

            //Thực hiện thêm mới
            var result = _tblBlackListService.Create(obj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.Id.ToString(), obj.Plate, "tblBlackList", ConstField.ParkingCode, ActionConfigO.Create);

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

        #endregion

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
            var obj = _tblBlackListService.GetById(id);

            ViewBag.PN = page;
            ViewBag.groupValue = group;
            ViewBag.keyValue = key;

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
        public ActionResult Update(tblBlackList obj, int page = 1, string group = "", string key = "")
        {
            //
            ViewBag.PN = page;
            ViewBag.groupValue = group;
            ViewBag.keyValue = key;

            //Kiểm tra
            if (String.IsNullOrEmpty(obj.Plate) || String.IsNullOrWhiteSpace(obj.Plate))
            {
                ViewBag.Error = FunctionHelper.GetLocalizeDictionary("Home", "notification")["enter_Number_plate"];
                return View(obj);
            }

            var oldObj = _tblBlackListService.GetById(obj.Id.ToString());
            if (oldObj == null)
            {
                ViewBag.Error = FunctionHelper.GetLocalizeDictionary("Home", "notification")["exists_Number_plate"];
                return View(obj);
            }

            var objExisted = _tblBlackListService.GetByName_Plate(obj.Name, obj.Plate);
            if(objExisted.Where(n => !n.Id.Equals(obj.Id)).Any())
            {
                ViewBag.Error = FunctionHelper.GetLocalizeDictionary("Home", "notification")["record_does_not_exist"];
                return View(obj);
            }

            if (!ModelState.IsValid)
            {
                return View(oldObj);
            }

            oldObj.Name = obj.Name;
            oldObj.Description = obj.Description;
            oldObj.Plate = obj.Plate;

            //Thực hiện cập nhật
            var result = _tblBlackListService.Update(oldObj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.Id.ToString(), obj.Plate, "tblBlackList", ConstField.ParkingCode, ActionConfigO.Update);

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
            var obj = new tblBlackList();

            var result = _tblBlackListService.DeleteById(id, ref obj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.Id.ToString(), obj.Plate, "tblBlackList", ConstField.ParkingCode, ActionConfigO.Delete);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion Xóa
    }
}
