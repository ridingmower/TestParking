using Kztek.Model.CustomModel.iParking;
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
    public class DynamicCardController : Controller
    {
        private IDynamicCardService _DynamicCardService;
        private ItblPCService _tblPCService;
        private ItblControllerService _tblControllerService;
        private ItblCardGroupService _tblCardGroupService;

        public DynamicCardController(IDynamicCardService _DynamicCardService, ItblPCService _tblPCService, ItblControllerService _tblControllerService, ItblCardGroupService _tblCardGroupService)
        {
            this._DynamicCardService = _DynamicCardService;
            this._tblPCService = _tblPCService;
            this._tblControllerService = _tblControllerService;
            this._tblCardGroupService = _tblCardGroupService;
        }

        #region DDL
        private List<tblPC> GetPCList()
        {
            return _tblPCService.GetAllActive().ToList();
        }

        private List<tblCardGroup> GetCardGroupList()
        {
            return _tblCardGroupService.GetAllActive().ToList();
        }

        private List<tblController> GetControllerList()
        {
            return _tblControllerService.GetAllActive().ToList();
        }
        #endregion

        private const string groupId = ConstField.ParkingID;

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Index(string key,string control, string cardgroup, string pc, int page = 1, string group = "", string selectedId = "")
        {
            var pageSize = 20;
            var totalItem = 0;

            var list = _DynamicCardService.GetPaging(key, control, pc, cardgroup, page, pageSize, ref totalItem);

            var gridModel = PageModelCustom<DynamicCardCustomViewModel>.GetPage(list, page, pageSize, totalItem);


            ViewBag.PCs = GetPCList();
            ViewBag.Controllers = GetControllerList();
            ViewBag.CardGroups = GetCardGroupList();

            ViewBag.keyValue = key;
            ViewBag.pcValue = pc;
            ViewBag.controllerValue = control;
            ViewBag.cardgroupValue = cardgroup;
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
            ViewBag.Controllers = GetControllerList();
            ViewBag.CardGroups = GetCardGroupList();

            ViewBag.keyValue = key;
            ViewBag.pcValue = pc;
            ViewBag.groupValue = group;

            var model = new DynamicCard
            {
                CardGroupID = "",
                ControllerID = "",
                PCID = "",
                Button = 0
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(DynamicCard obj, string key, string pc, string group = "", bool SaveAndCountinue = false)
        {
            //
            ViewBag.PCs = GetPCList();
            ViewBag.Controllers = GetControllerList();
            ViewBag.CardGroups = GetCardGroupList();

            //
            ViewBag.keyValue = key;
            ViewBag.pcValue = pc;
            ViewBag.groupValue = group;

            //
            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            //
            var existed = _DynamicCardService.CheckExist(obj.PCID,obj.ControllerID,obj.Button);
            if (existed != null)
            {
                ModelState.AddModelError("", FunctionHelper.GetLocalizeDictionary("Home", "notification")["Info_already_exists"]);
                return View(obj);
            }

            //
            obj.Id = Guid.NewGuid().ToString();
            obj.DateCreated = DateTime.Now;
           

            //Thực hiện thêm mới
            var result = _DynamicCardService.Create(obj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.Id, "", "DynamicCard", ConstField.ParkingCode, ActionConfigO.Create);

                if (SaveAndCountinue)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction("Create", new { group = group, key = key, pc = pc, selectedId = obj.Id });
                }

                return RedirectToAction("Index", new { group = group, key = key, pc = pc, selectedId = obj.Id });
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
            ViewBag.Controllers = GetControllerList();
            ViewBag.CardGroups = GetCardGroupList();

            ViewBag.keyValue = key;
            ViewBag.pcValue = pc;
            ViewBag.PN = page;
            ViewBag.groupValue = group;

            var obj = _DynamicCardService.GetById(id);

            return View(obj);
        }

        [HttpPost]
        public ActionResult Update(DynamicCard obj, int page = 1, string key = "", string pc = "", string group = "")
        {
            ViewBag.PCs = GetPCList();

            ViewBag.Controllers = GetControllerList();
            ViewBag.CardGroups = GetCardGroupList();

            ViewBag.keyValue = key;
            ViewBag.pcValue = pc;
            ViewBag.PN = page;
            ViewBag.groupValue = group;

            //Kiểm tra
            var oldObj = _DynamicCardService.GetById(obj.Id);
            if (oldObj == null)
            {
                ViewBag.Error = FunctionHelper.GetLocalizeDictionary("Home", "notification")["record_does_not_exist"];
                return View(obj);
            }

            var existed = _DynamicCardService.CheckExist(obj.PCID, obj.ControllerID, obj.Button);
            if (existed != null && existed.Id != oldObj.Id)
            {
                ModelState.AddModelError("", FunctionHelper.GetLocalizeDictionary("Home", "notification")["Info_already_exists"]);
                return View(obj);
            }

            if (!ModelState.IsValid)
            {
                return View(oldObj);
            }

            oldObj.ControllerID = obj.ControllerID;
            oldObj.CardGroupID = obj.CardGroupID;
            oldObj.PCID = obj.PCID;
            oldObj.Button = obj.Button;

            //Thực hiện cập nhật
            var result = _DynamicCardService.Update(oldObj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.Id, "", "DynamicCard", ConstField.ParkingCode, ActionConfigO.Update);

                return RedirectToAction("Index", new { group = group, page = page, key = key, pc = pc, selectedId = obj.Id });
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(oldObj);
            }
        }

        public JsonResult Delete(string id)
        {
            var obj = new DynamicCard();

            var result = _DynamicCardService.DeleteById(id, ref obj);

            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.Id, "", "DynamicCard", ConstField.ParkingCode, ActionConfigO.Delete);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }     
    }
}