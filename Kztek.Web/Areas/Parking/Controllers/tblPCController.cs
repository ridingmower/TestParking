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
    public class tblPCController : Controller
    {
        private ItblPCService _tblPCService;
        private ItblGateService _tblGateService;

        public tblPCController(ItblPCService _tblPCService, ItblGateService _tblGateService)
        {
            this._tblPCService = _tblPCService;
            this._tblGateService = _tblGateService;
        }

        private const string groupId = ConstField.ParkingID;

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Index(string key, string gate, int page = 1, string group = "", string selectedId = "")
        {
            var pageSize = 20;

            var list = _tblPCService.GetAllCustomPagingByFirst(key, gate, page, pageSize);

            var gridModel = PageModelCustom<tblPCCustomViewModel>.GetPage(list, page, pageSize);

            ViewBag.Gates = GetGateList();

            ViewBag.keyValue = key;
            ViewBag.groupValue = group;
            ViewBag.gateValue = gate;
            ViewBag.selectedIdValue = selectedId;

            return View(gridModel);
        }

        [CheckSessionLogin]
        [CheckAuthorize]
        [HttpGet]
        public ActionResult Create(string group = "", string key = "", string gate = "")
        {
            ViewBag.Gates = GetGateList();

            ViewBag.keyValue = key;
            ViewBag.groupValue = group;
            ViewBag.gateValue = gate;

            return View();
        }
        [HttpPost]
        public ActionResult Create(tblPC obj, bool SaveAndCountinue = false, string group = "", string key = "", string gate = "")
        {
            ViewBag.Gates = GetGateList();

            ViewBag.keyValue = key;
            ViewBag.groupValue = group;
            ViewBag.gateValue = gate;

            var DictionaryAction = FunctionHelper.GetLocalizeDictionary("Home", "notification");
            //
            if (string.IsNullOrWhiteSpace(obj.ComputerName))
            {
                ModelState.AddModelError("ComputerName", DictionaryAction["Computer_Name"]);
                return View(obj);
            }

            if (string.IsNullOrWhiteSpace(obj.IPAddress))
            {
                ModelState.AddModelError("IPAddress", DictionaryAction["ip"]);
                return View(obj);
            }

            //
            var existedName = _tblPCService.GetByName(obj.IPAddress);
            if (existedName != null)
            {
                ModelState.AddModelError("IPAddress", DictionaryAction["ip_already_exists"]);
                return View(obj);
            }

            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            obj.PCID = Guid.NewGuid();

            //Thực hiện thêm mới
            var result = _tblPCService.Create(obj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.PCID.ToString(), obj.ComputerName, "tblPC", ConstField.ParkingCode, ActionConfigO.Create);

                if (SaveAndCountinue)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction("Create", new { group = group, key = key, gate = obj.GateID, selectedId = obj.PCID });
                }

                return RedirectToAction("Index", new { group = group, key = key, gate = gate, selectedId = obj.PCID });
            }
            else
            {
                return View(obj);
            }
        }

        [CheckSessionLogin]
        [CheckAuthorize]
        [HttpGet]
        public ActionResult Update(string id, int page = 1, string group = "", string key = "", string gate = "")
        {
            var obj = _tblPCService.GetById(Guid.Parse(id));

            ViewBag.Gates = GetGateList();

            ViewBag.keyValue = key;
            ViewBag.groupValue = group;
            ViewBag.gateValue = gate;
            ViewBag.PN = page;

            return View(obj);
        }

        [HttpPost]
        public ActionResult Update(tblPC obj, int page = 1, string group = "", string key = "", string gate = "")
        {
            //
            ViewBag.Gates = GetGateList();

            //
            ViewBag.keyValue = key;
            ViewBag.groupValue = group;
            ViewBag.gateValue = gate;
            ViewBag.PN = page;

            //Kiểm tra
            var DictionaryAction = FunctionHelper.GetLocalizeDictionary("Home", "notification");
            var oldObj = _tblPCService.GetById(obj.PCID);
            if (oldObj == null)
            {
                ViewBag.Error = DictionaryAction["record_does_not_exist"];
                return View(obj);
            }

            //
            if (string.IsNullOrWhiteSpace(obj.ComputerName))
            {
                ModelState.AddModelError("ComputerName", DictionaryAction["Computer_Name"]);
                return View(oldObj);
            }

            if (string.IsNullOrWhiteSpace(obj.IPAddress))
            {
                ModelState.AddModelError("IPAddress", DictionaryAction["ip"]);
                return View(oldObj);
            }

            //
            var existedName = _tblPCService.GetByName_Id(obj.IPAddress, obj.PCID);
            if (existedName != null)
            {
                ModelState.AddModelError("IPAddress", DictionaryAction["ip_already_exists"]);
                return View(oldObj);
            }

            //
            if (!ModelState.IsValid)
            {
                return View(oldObj);
            }

            //Gán giá trị
            oldObj.ComputerName = obj.ComputerName;
            oldObj.Description = obj.Description;
            oldObj.IPAddress = obj.IPAddress;
            oldObj.Inactive = obj.Inactive;
            oldObj.PicPathIn = obj.PicPathIn;
            oldObj.PicPathOut = obj.PicPathOut;
            oldObj.VideoPath = obj.VideoPath;
            oldObj.GateID = obj.GateID;
            //oldObj.SortOrder = obj.SortOrder;

            //Thực hiện cập nhật
            var result = _tblPCService.Update(oldObj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.PCID.ToString(), obj.ComputerName, "tblPC", ConstField.ParkingCode, ActionConfigO.Update);

                return RedirectToAction("Index", new { group = group, key = key, gate = gate,  page = page, selectedId = obj.PCID });
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(oldObj);
            }
        }

        public JsonResult Delete(string id)
        {
            var obj = new tblPC();

            //var listCamera = _PK_CameraService.GetAllByPC(id);
            //if (listCamera.Any())
            //{
            //    var message = new Result();

            //    message.Success = false;
            //    message.Message = "Đang sửa dụng trong camera. Không thể xóa";
            //    message.ErrorCode = 500;

            //    return Json(message, JsonRequestBehavior.AllowGet);
            //}

            //var listController = _PK_HwControllerService.GetAllByPC(id);
            //if (listController.Any())
            //{
            //    var message = new Result();

            //    message.Success = false;
            //    message.Message = "Đang sửa dụng trong bộ điều khiển. Không thể xóa";
            //    message.ErrorCode = 500;

            //    return Json(message, JsonRequestBehavior.AllowGet);
            //}

            var result = _tblPCService.DeleteById(id, ref obj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.PCID.ToString(), obj.ComputerName, "tblPC", ConstField.ParkingCode, ActionConfigO.Delete);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private List<tblGate> GetGateList()
        {
            return _tblGateService.GetAllActive().ToList();
        }
    }
}