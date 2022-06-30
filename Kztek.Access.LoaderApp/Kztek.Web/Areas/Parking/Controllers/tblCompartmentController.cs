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
    public class tblCompartmentController : Controller
    {
        private ItblCompartmentService _tblCompartmentService;
        public tblCompartmentController(ItblCompartmentService _tblCompartmentService)
        {
            this._tblCompartmentService = _tblCompartmentService;
        }

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Index(string key, int page = 1, string group = "", string selectedId = "")
        {
            var pageSize = 20;

            var list = _tblCompartmentService.GetPagingByFirst(key, page, pageSize);

            var gridModel = PageModelCustom<tblCompartment>.GetPage(list, page, pageSize);

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
        public ActionResult Create(tblCompartment obj, bool SaveAndCountinue = false, string group = "", string key = "")
        {
            ViewBag.groupValue = group;
            ViewBag.keyValue = key;

            //Kiểm tra
            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            if (string.IsNullOrWhiteSpace(obj.CompartmentName))
            {
                ModelState.AddModelError("CompartmentName", "Vui lòng căn hộ");
                return View(obj);
            }

            var objExisted = _tblCompartmentService.GetByName(obj.CompartmentName);
            if (objExisted.Any())
            {
                ViewBag.Error = "Bản ghi đã tồn tại";
                return View(obj);
            }
            //Tạo guid
            obj.CompartmentID = Guid.NewGuid();

            //Thực hiện thêm mới
            var result = _tblCompartmentService.Create(obj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.CompartmentID.ToString(), obj.CompartmentName, "tblCompartment", ConstField.ParkingCode, ActionConfigO.Create);

                if (SaveAndCountinue)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction("Create", new { group = group, key = key, selectedId = obj.CompartmentID });
                }

                return RedirectToAction("Index", new { group = group, key = key , selectedId = obj.CompartmentID });
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
            var obj = _tblCompartmentService.GetById(Guid.Parse(id));

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
        public ActionResult Update(tblCompartment obj, int page = 1, string group = "", string key = "")
        {
            //
            ViewBag.PN = page;
            ViewBag.groupValue = group;
            ViewBag.keyValue = key;

            //Kiểm tra
            var oldObj = _tblCompartmentService.GetById(obj.CompartmentID);
            if (oldObj == null)
            {
                ViewBag.Error = "Bản ghi không tồn tại";
                return View(obj);
            }

            if (string.IsNullOrWhiteSpace(obj.CompartmentName))
            {
                ModelState.AddModelError("CompartmentName", "Vui lòng căn hộ");
                return View(oldObj);
            }

            if (!ModelState.IsValid)
            {
                return View(oldObj);
            }

            var objExisted = _tblCompartmentService.GetByName(obj.CompartmentName);
            if (objExisted.Where(n => !n.CompartmentID.Equals(obj.CompartmentID)).Any())
            {
                ViewBag.Error = "Bản ghi đã tồn tại";
                return View(oldObj);
            }

            oldObj.CompartmentName = obj.CompartmentName;
            oldObj.SortOrder = obj.SortOrder;

            //Thực hiện cập nhật
            var result = _tblCompartmentService.Update(oldObj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.CompartmentID.ToString(), obj.CompartmentName, "tblCompartment", ConstField.ParkingCode, ActionConfigO.Update);

                return RedirectToAction("Index", new { group = group, key = key, page = page, selectedId = obj.CompartmentID });
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
            var obj = new tblCompartment();

            var result = _tblCompartmentService.DeleteById(id, ref obj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.CompartmentID.ToString(), obj.CompartmentName, "tblCompartment", ConstField.ParkingCode, ActionConfigO.Delete);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion Xóa
    }
}