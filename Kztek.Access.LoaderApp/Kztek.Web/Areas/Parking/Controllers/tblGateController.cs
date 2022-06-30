using Kztek.Model.CustomModel;
using Kztek.Model.Models;
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
    public class tblGateController : Controller
    {
        private ItblGateService _tblGateService;
        private ItblPCService _tblPCService;

        public tblGateController(ItblGateService _tblGateService, ItblPCService _tblPCService)
        {
            this._tblGateService = _tblGateService;
            this._tblPCService = _tblPCService;
        }

        private const string groupId = ConstField.ParkingID;

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Index(string key, int page = 1, string group = "", string selectedId = "")
        {
            var pageSize = 20;

            var list = _tblGateService.GetPagingByFirst(key, page, pageSize);

            var gridModel = PageModelCustom<tblGate>.GetPage(list, page, pageSize);

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
        public ActionResult Create(tblGate obj, bool SaveAndCountinue = false, string group = "", string key = "")
        {
            ViewBag.groupValue = group;
            ViewBag.keyValue = key;

            //Kiểm tra
            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            //
            if (string.IsNullOrWhiteSpace(obj.GateName))
            {
                ModelState.AddModelError("GateName", FunctionHelper.GetLocalizeDictionary("Home", "notification")["enter_name"]);
                return View(obj);
            }

            //
            var objExisted = _tblGateService.GetByName(obj.GateName);
            if (objExisted != null)
            {
                ViewBag.Error = FunctionHelper.GetLocalizeDictionary("Home", "notification")["record_does_not_exist"]; ;
                return View(obj);
            }

            //Gán giá trị
            obj.GateID = Guid.NewGuid();

            //Thực hiện thêm mới
            var result = _tblGateService.Create(obj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.GateID.ToString(), obj.GateName, "tblGate", ConstField.ParkingCode, ActionConfigO.Create);

                if (SaveAndCountinue)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction("Create", new { group = group, key = key, selectedId = obj.GateID });
                }

                return RedirectToAction("Index", new { group = group, key = key, selectedId = obj.GateID });
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
            var obj = _tblGateService.GetById(Guid.Parse(id));

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
        public ActionResult Update(tblGate obj, int page = 1, string group = "", string key = "")
        {
            //
            ViewBag.PN = page;
            ViewBag.groupValue = group;
            ViewBag.keyValue = key;

            //Kiểm tra
            var oldObj = _tblGateService.GetById(obj.GateID);
            if (oldObj == null)
            {
                ViewBag.Error = FunctionHelper.GetLocalizeDictionary("Home", "notification")["record_does_not_exist"];
                return View(obj);
            }

            //
            if (string.IsNullOrWhiteSpace(obj.GateName))
            {
                ModelState.AddModelError("GateName", FunctionHelper.GetLocalizeDictionary("Home", "notification")["enter_name"]);
                return View(oldObj);
            }

            if (!ModelState.IsValid)
            {
                return View(oldObj);
            }

            var objExisted = _tblGateService.GetByName_Id(obj.GateName, obj.GateID);
            if (objExisted != null)
            {
                ViewBag.Error = FunctionHelper.GetLocalizeDictionary("Home", "notification")["record_already_exist"];
                return View(oldObj);
            }

            oldObj.GateName = obj.GateName;
            oldObj.Description = obj.Description;
            oldObj.Inactive = obj.Inactive;
            //oldObj.SortOrder = obj.SortOrder;

            //Thực hiện cập nhật
            var result = _tblGateService.Update(oldObj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.GateID.ToString(), obj.GateName, "tblGate", ConstField.ParkingCode, ActionConfigO.Update);

                return RedirectToAction("Index", new { group = group, key = key, page = page, selectedId = obj.GateID });
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
            var obj = new tblGate();

            var listPC = _tblPCService.GetAllByGateId(id);
            if (listPC.Any())
            {
                var message = new MessageReport();

                message.isSuccess = false;
                message.Message = FunctionHelper.GetLocalizeDictionary("Home", "notification")["Message_del_fail_computer"];

                return Json(message, JsonRequestBehavior.AllowGet);
            }

            var result = _tblGateService.DeleteById(id, ref obj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.GateID.ToString(), obj.GateName, "tblGate", ConstField.ParkingCode, ActionConfigO.Delete);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion Xóa
    }
}