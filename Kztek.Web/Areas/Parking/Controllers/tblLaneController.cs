using Kztek.Model.CustomModel;
using Kztek.Model.CustomModel.iParking;
using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Web.Attributes;
using Kztek.Web.Core.Extensions;
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
    public class tblLaneController : Controller
    {
        private ItblLaneService _tblLaneService;
        private ItblPCService _tblPCService;
        private ItblCameraService _tblCameraService;
        private ItblCardGroupService _tblCardGroupService;
        public tblLaneController(ItblLaneService _tblLaneService, ItblPCService _tblPCService, ItblCameraService _tblCameraService, ItblCardGroupService _tblCardGroupService)
        {
            this._tblLaneService = _tblLaneService;
            this._tblPCService = _tblPCService;
            this._tblCameraService = _tblCameraService;
            this._tblCardGroupService = _tblCardGroupService;
        }

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Index(string key, string pc, int page = 1, string group = "", string selectedId = "")
        {
            var pageSize = 20;

            var list = _tblLaneService.GetAllCustomPagingByFirst(key, pc, page, pageSize);

            var gridModel = PageModelCustom<tblLane>.GetPage(list, page, pageSize);

            ViewBag.PCs = GetPCList();
            ViewBag.LaneTypes = FunctionHelper.LaneTypes1();

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
            ViewBag.LaneType = FunctionHelper.LaneTypes1();
            ViewBag.CheckPlates = FunctionHelper.CheckBS();
            ViewBag.OpenBarrie = FunctionHelper.OpenBarrie();
            ViewBag.PCs = GetPCList();
            ViewBag.VehicleType = FunctionHelper.VehicleType().ToDataTableNullable();
            ViewBag.CardType = FunctionHelper.CardTypes().ToDataTableNullable();

            ViewBag.keyValue = key;
            ViewBag.pcValue = pc;
            ViewBag.groupValue = group;

            var strVehicle = string.Join(",", FunctionHelper.VehicleType().Select(n => n.ItemValue));
            var strCard = string.Join(",", FunctionHelper.CardTypes().Select(n => n.ItemValue));

            var model = new tblLane
            {
                VehicleTypeLeft = strVehicle,
                VehicleTypeRight = strVehicle,
                CardTypeLeft = strCard,
                CardTypeRight = strCard
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(tblLane obj, string key, string pc, string group = "", string vehicleL = "", string vehicleR = "", string cardL = "", string cardR = "", bool SaveAndCountinue = false)
        {
            ViewBag.LaneType = FunctionHelper.LaneTypes1();
            ViewBag.CheckPlates = FunctionHelper.CheckBS();
            ViewBag.OpenBarrie = FunctionHelper.OpenBarrie();
            ViewBag.PCs = GetPCList();
            ViewBag.VehicleType = FunctionHelper.VehicleType().ToDataTableNullable();
            ViewBag.CardType = FunctionHelper.CardTypes().ToDataTableNullable();

            ViewBag.keyValue = key;
            ViewBag.pcValue = pc;
            ViewBag.groupValue = group;

            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            if (string.IsNullOrWhiteSpace(obj.LaneName))
            {
                ModelState.AddModelError("LaneName", FunctionHelper.GetLocalizeDictionary("Home", "notification")["Lane_Name"]);
                return View(obj);
            }

            var existed = _tblLaneService.GetByName(obj.LaneName);
            if (existed != null)
            {
                ModelState.AddModelError("LaneName", FunctionHelper.GetLocalizeDictionary("Home", "notification")["Lane_Name_already_exists"]);
                return View(obj);
            }

            obj.LaneID = Guid.NewGuid();
            obj.CardTypeLeft = cardL;
            obj.CardTypeRight = cardR;
            obj.VehicleTypeLeft = vehicleL;
            obj.VehicleTypeRight = vehicleR;

            //Thực hiện thêm mới
            var result = _tblLaneService.Create(obj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.LaneID.ToString(), obj.LaneName, "tblLane", ConstField.ParkingCode, ActionConfigO.Create);

                if (SaveAndCountinue)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction("Create", new { group = group, key = key, pc = pc, selectedId = obj.LaneID });
                }

                return RedirectToAction("Index", new { group = group, key = key, pc = pc, selectedId = obj.LaneID });
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
            ViewBag.LaneType = FunctionHelper.LaneTypes1();
            ViewBag.CheckPlates = FunctionHelper.CheckBS();
            ViewBag.OpenBarrie = FunctionHelper.OpenBarrie();
            ViewBag.PCs = GetPCList();
            ViewBag.VehicleType = FunctionHelper.VehicleType().ToDataTableNullable();
            ViewBag.CardType = FunctionHelper.CardTypes().ToDataTableNullable();

            ViewBag.keyValue = key;
            ViewBag.pcValue = pc;
            ViewBag.PN = page;
            ViewBag.groupValue = group;

            var obj = _tblLaneService.GetById(Guid.Parse(id));

            return View(obj);
        }

        [HttpPost]
        public ActionResult Update(tblLane obj, int page = 1, string key = "", string pc = "", string vehicleL = "", string vehicleR = "", string cardL = "", string cardR = "", string group = "",string cardgroup = "")
        {
            ViewBag.LaneType = FunctionHelper.LaneTypes1();
            ViewBag.CheckPlates = FunctionHelper.CheckBS();
            ViewBag.OpenBarrie = FunctionHelper.OpenBarrie();
            ViewBag.PCs = GetPCList();
            ViewBag.VehicleType = FunctionHelper.VehicleType().ToDataTableNullable();
            ViewBag.CardType = FunctionHelper.CardTypes().ToDataTableNullable();

            ViewBag.keyValue = key;
            ViewBag.pcValue = pc;
            ViewBag.PN = page;
            ViewBag.groupValue = group;

            var oldObj = _tblLaneService.GetById(obj.LaneID);
            if (oldObj == null)
            {
                ViewBag.Error = FunctionHelper.GetLocalizeDictionary("Home", "notification")["record_does_not_exist"];
                return View(obj);
            }

            if (string.IsNullOrWhiteSpace(obj.LaneName))
            {
                ModelState.AddModelError("LaneName", FunctionHelper.GetLocalizeDictionary("Home", "notification")["Lane_Name"]);
                return View(oldObj);
            }

            var existed = _tblLaneService.GetByName_Id(obj.LaneName, obj.LaneID);
            if (existed != null)
            {
                ModelState.AddModelError("LaneName", FunctionHelper.GetLocalizeDictionary("Home", "notification")["Lane_Name_already_exists"]);
                return View(oldObj);
            }

            if (!ModelState.IsValid)
            {
                return View(oldObj);
            }

            oldObj.LaneName = obj.LaneName;
            oldObj.LaneType = obj.LaneType;
            oldObj.PCID = obj.PCID;
            oldObj.C1 = obj.C1;
            oldObj.C2 = obj.C2;
            oldObj.C3 = obj.C3;
            oldObj.C4 = obj.C4;
            oldObj.C5 = obj.C5;
            oldObj.C6 = obj.C6;
            oldObj.AccessForEachSide = obj.AccessForEachSide;
            oldObj.CheckPlateLevelIn = obj.CheckPlateLevelIn;
            oldObj.CheckPlateLevelOut = obj.CheckPlateLevelOut;
            oldObj.CheckPlateOutOption2 = obj.CheckPlateOutOption2;

            oldObj.OpenBarrieIn = obj.OpenBarrieIn;
            oldObj.OpenBarrieOption1 = obj.OpenBarrieOption1;
            oldObj.OpenBarrieOption2 = obj.OpenBarrieOption2;

            oldObj.IsFree = obj.IsFree;
            oldObj.IsLoop = obj.IsLoop;
            oldObj.Inactive = obj.Inactive;
            oldObj.IsLED = obj.IsLED;
            oldObj.IsPrint = obj.IsPrint;

            oldObj.CardTypeLeft = cardL;
            oldObj.CardTypeRight = cardR;
            oldObj.VehicleTypeLeft = vehicleL;
            oldObj.VehicleTypeRight = vehicleR;

            oldObj.CardGroupId = oldObj.IsLoop ? cardgroup : "";
            //oldObj.SortOrder = obj.SortOrder;

            //Thực hiện cập nhật
            var result = _tblLaneService.Update(oldObj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.LaneID.ToString(), obj.LaneName, "tblLane", ConstField.ParkingCode, ActionConfigO.Update);

                return RedirectToAction("Index", new { group = group, page = page, key = key, pc = pc, selectedId = obj.LaneID });
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(oldObj);
            }
        }

        public JsonResult Delete(string id)
        {
            var obj = new tblLane();

            var result = _tblLaneService.DeleteById(id, ref obj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.LaneID.ToString(), obj.LaneName, "tblLane", ConstField.ParkingCode, ActionConfigO.Delete);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult ListCameraByPC(string pcID, string cameraNumber, string selected)
        {
            List<tblCamera> newList = new List<tblCamera>();

            var list = _tblCameraService.GetAllActiveByPC(pcID);
            if (list.Any())
            {
                foreach (var item in list.ToList())
                {
                    newList.Add(new tblCamera { CameraID = item.CameraID, CameraName = string.Format("{0} ({1})", item.CameraName, item.HttpURL) });
                }
            }

            ViewBag.Number = cameraNumber;
            ViewBag.selectedCamera = selected;
            return PartialView(newList);
        }

        private List<tblPC> GetPCList()
        {
            return _tblPCService.GetAllActive().ToList();
        }

        public PartialViewResult Partial_OptionLoop(string laneid, string selected)
        {
            var list = new List<SelectListModel> { new SelectListModel { ItemValue = "",ItemText = "- Lựa chọn -"} };

            var listCardGroup = _tblCardGroupService.GetByLane(laneid);
            
            if(listCardGroup != null && listCardGroup.Count() > 0)
            {
                foreach(var item in listCardGroup)
                {
                    list.Add(new SelectListModel { ItemValue = item.CardGroupID.ToString(), ItemText = item.CardGroupName });
                }
            }

            ViewBag.Selected = selected;

            return PartialView(list);
        }

    }
}