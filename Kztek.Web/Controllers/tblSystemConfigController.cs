using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Web.Core.Helpers;
using Kztek.Web.Core.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kztek.Web.Controllers
{
    public class tblSystemConfigController : Controller
    {
        private ItblSystemConfigService _tblSystemConfigService;

        public tblSystemConfigController(ItblSystemConfigService _tblSystemConfigService)
        {
            this._tblSystemConfigService = _tblSystemConfigService;
        }

        [HttpGet]
        public ActionResult Index(string group = "")
        {
            ViewBag.GroupID = group;

            var obj = _tblSystemConfigService.GetDefault();

            if (obj == null)
            {
                obj = new tblSystemConfig();
                obj.SystemConfigID = Guid.NewGuid();
                obj.FeeName = "FUTECH";
                obj.SortOrder = 1;
                obj.DelayTime = 0;

                _tblSystemConfigService.Create(obj);
            }

            return View(obj);
        }

        [HttpPost]
        public ActionResult Index(tblSystemConfig obj, HttpPostedFileBase FileUpload, string group = "")
        {
            ViewBag.GroupID = group;

            //Kiểm tra
            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            var oldObj = _tblSystemConfigService.GetDefault();
            if (oldObj == null)
            {
                oldObj = new tblSystemConfig();

                _tblSystemConfigService.Create(oldObj);
            }

            //Gán
            oldObj.Address = obj.Address;
            //oldObj.al = obj.AlwaysOpenBarrierIn;
            oldObj.Company = obj.Company;
            oldObj.DelayTime = obj.DelayTime;
            oldObj.EnableAlarmMessageBox = obj.EnableAlarmMessageBox;
            oldObj.EnableAlarmMessageBoxIn = obj.EnableAlarmMessageBoxIn;
            oldObj.EnableDeleteCardFailed = obj.EnableDeleteCardFailed;
            oldObj.EnableSoundAlarm = obj.EnableSoundAlarm;
            oldObj.Fax = obj.Fax;
            oldObj.FeeName = obj.FeeName;
            oldObj.KeyA = obj.KeyA;
            oldObj.KeyB = obj.KeyB;
            oldObj.Logo = !string.IsNullOrWhiteSpace(obj.Logo) ? obj.Logo : "";
            oldObj.Para1 = obj.Para1;
            oldObj.Para2 = obj.Para2;
            oldObj.SystemCode = obj.SystemCode;
            oldObj.Tax = !string.IsNullOrWhiteSpace(obj.Tax) ? obj.Tax : "";
            oldObj.Tel = !string.IsNullOrWhiteSpace(obj.Tel) ? obj.Tel : "";
            oldObj.CustomInfo = obj.CustomInfo;
            oldObj.IsSaveButton = obj.IsSaveButton;
            oldObj.isCompartment = obj.isCompartment;
            if (FileUpload != null)
            {
                string error = "";
                oldObj.Background = string.Format("{0}/{1}", ConfigurationManager.AppSettings["FileUploadAvatar"], Common.UploadImages(out error, Server.MapPath(ConfigurationManager.AppSettings["FileUploadAvatar"]), FileUpload));
            }
            else
            {
                oldObj.Background = obj.Background;
            }

            var result = _tblSystemConfigService.Update(oldObj);
            if (result.isSuccess)
            {
                var host = Request.Url.Host;
                Session[string.Format("{0}_{1}", SessionConfig.SystemConfigSession, host)] = null;

                TempData["Success"] = result.Message;
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(obj);
            }
        }
    }
}