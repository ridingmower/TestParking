using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Web.Attributes;
using Kztek.Web.Core.Extensions;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Helpers;
using Kztek.Web.Core.Models;
using Kztek.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kztek.Web.Areas.Access.Controllers
{
    public class SelfHostConfigController : Controller
    {
        private ISelfHostConfigService _SelfHostConfigService;
        private ItblAccessPCService _tblAccessPCService;

        public SelfHostConfigController(ISelfHostConfigService _SelfHostConfigService, ItblAccessPCService _tblAccessPCService)
        {
            this._SelfHostConfigService = _SelfHostConfigService;
            this._tblAccessPCService = _tblAccessPCService;
        }

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Index(string key, string pcs, int page = 1, string selectedId = "")
        {
            int pageSize = 20;

            var list = _SelfHostConfigService.GetAllPagingByFirst(key, pcs, page, pageSize);

            var gridModel = PageModelCustom<SelfHostConfig>.GetPage(list, page, pageSize);

            ViewBag.keyValue = key;
            ViewBag.pcsValue = pcs;
            ViewBag.selectedIdValue = selectedId;

            ViewBag.pcsList = _tblAccessPCService.GetAllActive();

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

            ViewBag.pcsList = _tblAccessPCService.GetAllActive();

            return View();
        }
        [HttpPost]
        public ActionResult Create(SelfHostConfig obj, string key, string pcs, bool SaveAndCountinue = false)
        {
            ViewBag.keyValue = key;
            ViewBag.pcsValue = pcs;

            ViewBag.pcsList = _tblAccessPCService.GetAllActive();

            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            obj.Id = Common.GenerateId();
            obj.DateCreated = DateTime.Now;

            var result = _SelfHostConfigService.Create(obj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.Id.ToString(), obj.Address, "SelfHostConfig", ConstField.AccessControlCode, ActionConfigO.Create);

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

            var obj = _SelfHostConfigService.GetById(id);

            ViewBag.pcsList = _tblAccessPCService.GetAllActive();

            return View(obj);
        }
        [HttpPost]
        public ActionResult Update(SelfHostConfig obj, string key, string pcs, int page = 1)
        {
            ViewBag.keyValue = key;
            ViewBag.pcsValue = pcs;

            ViewBag.PN = page;

            ViewBag.pcsList = _tblAccessPCService.GetAllActive();

            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            var oldObj = _SelfHostConfigService.GetById(obj.Id);
            if (oldObj == null)
            {
                return View(obj);
            }

            oldObj.Address = obj.Address;
            oldObj.Hostname = obj.Hostname;
            oldObj.PCID = obj.PCID;

            var result = _SelfHostConfigService.Update(oldObj);

            if (result.isSuccess)
            {
                var cuuser = GetCurrentUser.GetUser();

                //Log for hệ thống
                WriteLog.Write(result, cuuser, oldObj.Id.ToString(), oldObj.Address, "SelfHostConfig", ConstField.AccessControlCode, ActionConfigO.Update);

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
            var result = _SelfHostConfigService.DeleteById(id);
            if (result.isSuccess)
            {
                var cuuser = GetCurrentUser.GetUser();

                //Log for hệ thống
                WriteLog.Write(result, cuuser, id, id, "SelfHostConfig", ConstField.AccessControlCode, ActionConfigO.Delete);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}