using Kztek.Model.CustomModel.iParking;
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
    public class tblControllerController : Controller
    {
        private ItblControllerService _tblControllerService;
        private ItblPCService _tblPCService;

        public tblControllerController(ItblControllerService _tblControllerService, ItblPCService _tblPCService)
        {
            this._tblControllerService = _tblControllerService;
            this._tblPCService = _tblPCService;
        }

        private const string groupId = ConstField.ParkingID;

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Index(string key, string pc, int page = 1, string group = "", string selectedId = "")
        {
            var pageSize = 20;

            var list = _tblControllerService.GetAllCustomPagingByFirst(key, pc, page, pageSize);

            var gridModel = PageModelCustom<tblControllerCustomViewModel>.GetPage(list, page, pageSize);

            ViewBag.PCs = GetPCList();

            ViewBag.keyValue = key;
            ViewBag.pcValue = pc;
            ViewBag.groupValue = group;
            ViewBag.selectedIdValue = selectedId;

            return View(gridModel);
        }

        [CheckSessionLogin]
        [CheckAuthorize]
        [HttpGet]
        public ActionResult Create(string key, string pc, string group = "")
        {
            ViewBag.PCs = GetPCList();

            ViewBag.keyValue = key;
            ViewBag.pcValue = pc;
            ViewBag.groupValue = group;

            ViewBag.CommunicationType = FunctionHelper.Communication1();
            ViewBag.LineType = FunctionHelper.LineTypes1();
            ViewBag.Read = FunctionHelper.ReaderTypes1();

            return View();
        }

        [HttpPost]
        public ActionResult Create(tblController obj, string key, string pc, string group = "", bool SaveAndCountinue = false)
        {
            ViewBag.PCs = GetPCList();

            ViewBag.keyValue = key;
            ViewBag.pcValue = pc;
            ViewBag.groupValue = group;

            ViewBag.CommunicationType = FunctionHelper.Communication1();
            ViewBag.LineType = FunctionHelper.LineTypes1();
            ViewBag.Read = FunctionHelper.ReaderTypes1();

            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            if (string.IsNullOrWhiteSpace(obj.ControllerName))
            {
                ModelState.AddModelError("ControllerName", FunctionHelper.GetLocalizeDictionary("Home", "notification")["Controller_Name"]);
                return View(obj);
            }

            var existed = _tblControllerService.GetByName(obj.ControllerName);
            if (existed != null)
            {
                ModelState.AddModelError("ControllerName", FunctionHelper.GetLocalizeDictionary("Home", "notification")["Controller_Name_already_exists"]);
                return View(obj);
            }

            obj.ControllerID = Guid.NewGuid();

            //Thực hiện thêm mới
            var result = _tblControllerService.Create(obj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.ControllerID.ToString(), obj.ControllerName, "tblController", ConstField.ParkingCode, ActionConfigO.Create);

                if (SaveAndCountinue)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction("Create", new { group = group, key = key, pc = pc, selectedId = obj.ControllerID });
                }

                return RedirectToAction("Index", new { group = group, key = key, pc = pc, selectedId = obj.ControllerID });
            }
            else
            {
                return View(obj);
            }
        }

        [CheckSessionLogin]
        [CheckAuthorize]
        [HttpGet]
        public ActionResult Update(string id, int page = 1, string key = "", string pc = "", string group = "")
        {
            ViewBag.PCs = GetPCList();

            ViewBag.keyValue = key;
            ViewBag.pcValue = pc;
            ViewBag.PN = page;
            ViewBag.groupValue = group;

            ViewBag.CommunicationType = FunctionHelper.Communication1();
            ViewBag.LineType = FunctionHelper.LineTypes1();
            ViewBag.Read = FunctionHelper.ReaderTypes1();

            var obj = _tblControllerService.GetById(Guid.Parse(id));

            return View(obj);
        }

        [HttpPost]
        public ActionResult Update(tblController obj, int page = 1, string key = "", string pc = "", string group = "")
        {
            //
            ViewBag.PCs = GetPCList();

            //
            ViewBag.keyValue = key;
            ViewBag.pcValue = pc;
            ViewBag.PN = page;
            ViewBag.groupValue = group;

            //
            ViewBag.CommunicationType = FunctionHelper.Communication1();
            ViewBag.LineType = FunctionHelper.LineTypes1();
            ViewBag.Read = FunctionHelper.ReaderTypes1();

            //
            var oldObj = _tblControllerService.GetById(obj.ControllerID);
            if (oldObj == null)
            {
                ViewBag.Error = FunctionHelper.GetLocalizeDictionary("Home", "notification")["record_does_not_exist"];
                return View(obj);
            }

            //
            if (string.IsNullOrWhiteSpace(obj.ControllerName))
            {
                ModelState.AddModelError("ControllerName", FunctionHelper.GetLocalizeDictionary("Home", "notification")["Controller_Name"]);
                return View(oldObj);
            }

            //
            var existed = _tblControllerService.GetByName_Id(obj.ControllerName, obj.ControllerID);
            if (existed != null)
            {
                ModelState.AddModelError("ControllerName", FunctionHelper.GetLocalizeDictionary("Home", "notification")["Controller_Name_already_exists"]);
                return View(oldObj);
            }

            if (!ModelState.IsValid)
            {
                return View(oldObj);
            }

            //Gán giá trị
            oldObj.ControllerName = obj.ControllerName;
            oldObj.Baudrate = obj.Baudrate;
            oldObj.CommunicationType = obj.CommunicationType;
            oldObj.Comport = obj.Comport;
            oldObj.Inactive = obj.Inactive;
            oldObj.LineTypeID = obj.LineTypeID;
            oldObj.PCID = obj.PCID;
            oldObj.Reader1Type = obj.Reader1Type;
            oldObj.Reader2Type = obj.Reader2Type;
            oldObj.Address = obj.Address;
            //oldObj.SortOrder = obj.SortOrder;

            //Thực hiện cập nhật
            var result = _tblControllerService.Update(oldObj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.ControllerID.ToString(), obj.ControllerName, "tblController", ConstField.ParkingCode, ActionConfigO.Update);

                return RedirectToAction("Index", new { group = group, page = page, key = key, pc = pc, selectedId = obj.ControllerID });
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(oldObj);
            }
        }

        public JsonResult Delete(string id)
        {
            var obj = new tblController();

            var result = _tblControllerService.DeleteById(id, ref obj);

            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.ControllerID.ToString(), obj.ControllerName, "tblController", ConstField.ParkingCode, ActionConfigO.Delete);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private List<tblPC> GetPCList()
        {
            return _tblPCService.GetAllActive().ToList();
        }
    }
}