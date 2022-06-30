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

namespace Kztek.Web.Areas.Locker.Controllers
{
    public class SelfHostConfigController : Controller
    {
        private ItblLockerSelfHostService _tblLockerSelfHostService;
        private ItblLockerPCService _tblLockerPCService;

        public SelfHostConfigController(ItblLockerSelfHostService _tblLockerSelfHostService, ItblLockerPCService _tblLockerPCService)
        {
            this._tblLockerSelfHostService = _tblLockerSelfHostService;
            this._tblLockerPCService = _tblLockerPCService;
        }

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Index(string key, string pcs, int page = 1, string selectedId = "")
        {
            int pageSize = 20;

            var list = _tblLockerSelfHostService.GetAllPagingByFirst(key, pcs, page, pageSize);

            var gridModel = PageModelCustom<tblLockerSelfHost>.GetPage(list, page, pageSize);

            ViewBag.keyValue = key;
            ViewBag.pcsValue = pcs;
            ViewBag.selectedIdValue = selectedId;

            ViewBag.pcsList = _tblLockerPCService.GetAllActive();

            return View(gridModel);
        }

        [CheckSessionLogin]
        [CheckAuthorize]
        [HttpGet]
        public ActionResult Create(string key, string pcs, string selectedId)
        {
            ViewBag.keyValue = key;
            ViewBag.pcsValue = pcs;
            ViewBag.selectedIdValue = selectedId;

            ViewBag.pcsList = _tblLockerPCService.GetAllActive();

            return View();
        }
        [HttpPost]
        public ActionResult Create(tblLockerSelfHost obj, string key, string pcs, bool SaveAndCountinue = false)
        {
            ViewBag.keyValue = key;
            ViewBag.pcsValue = pcs;

            ViewBag.pcsList = _tblLockerPCService.GetAllActive();

            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            obj.Id = Common.GenerateId();
            obj.DateCreated = DateTime.Now;

            var result = _tblLockerSelfHostService.Create(obj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.Id.ToString(), obj.Address, "tblLockerSelfHost", ConstField.LockerCode, ActionConfigO.Create);

                if (SaveAndCountinue)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction("Create", new { key = key, pcs = pcs, selectedId = obj.Id, page = 1 });
                }

                return RedirectToAction("Index", new { key = key, pcs = pcs, selectedId = obj.Id, page = 1 });
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(obj);
            }
        }

        [CheckSessionLogin]
        [CheckAuthorize]
        [HttpGet]
        public ActionResult Update(string id, string key, string pcs, int page = 1)
        {
            ViewBag.keyValue = key;
            ViewBag.pcsValue = pcs;

            ViewBag.PN = page;

            var obj = _tblLockerSelfHostService.GetById(id);

            ViewBag.pcsList = _tblLockerPCService.GetAllActive();

            return View(obj);
        }
        [HttpPost]
        public ActionResult Update(tblLockerSelfHost obj, string key, string pcs, int page = 1)
        {
            ViewBag.keyValue = key;
            ViewBag.pcsValue = pcs;

            ViewBag.PN = page;

            ViewBag.pcsList = _tblLockerPCService.GetAllActive();

            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            var oldObj = _tblLockerSelfHostService.GetById(obj.Id);
            if (oldObj == null)
            {
                return View(obj);
            }

            oldObj.Address = obj.Address;
            oldObj.Hostname = obj.Hostname;
            oldObj.PCID = obj.PCID;

            var result = _tblLockerSelfHostService.Update(oldObj);

            if (result.isSuccess)
            {
                var cuuser = GetCurrentUser.GetUser();

                //Log for hệ thống
                WriteLog.Write(result, cuuser, oldObj.Id.ToString(), oldObj.Address, "tblLockerSelfHost", ConstField.LockerCode, ActionConfigO.Update);

                return RedirectToAction("Index", new { key = key, pcs = pcs, selectedId = obj.Id, page = page });
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(obj);
            }
        }

        public JsonResult Delete(string id)
        {
            var result = _tblLockerSelfHostService.DeleteById(id);
            if (result.isSuccess)
            {
                var cuuser = GetCurrentUser.GetUser();

                //Log for hệ thống
                WriteLog.Write(result, cuuser, id, id, "tblLockerSelfHost", ConstField.LockerCode, ActionConfigO.Delete);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}