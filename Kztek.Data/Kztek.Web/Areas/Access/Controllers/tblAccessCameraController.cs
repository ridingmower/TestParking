using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Web.Attributes;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Helpers;
using Kztek.Web.Core.Models;
using Kztek.Web.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Kztek.Web.Areas.Access.Controllers
{
    public class tblAccessCameraController : Controller
    {
        private ItblAccessCameraService _tblAccessCameraService;
        private ItblAccessControllerService _tblAccessControllerService;

        public tblAccessCameraController(ItblAccessCameraService _tblAccessCameraService, ItblAccessControllerService _tblAccessControllerService)
        {
            this._tblAccessCameraService = _tblAccessCameraService;
            this._tblAccessControllerService = _tblAccessControllerService;
        }

        private const string groupId = ConstField.AccessControlID;

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Index(string key, string control, int page = 1, string group = "", string selectedId = "")
        {
            var pageSize = 20;

            var list = _tblAccessCameraService.GetAllCustomPagingByFirst(key, control, page, pageSize);

            var gridModel = PageModelCustom<tblAccessCameraCustomViewModel>.GetPage(list, page, pageSize);

            ViewBag.Controllers = GetControllerList();

            ViewBag.keyValue = key;
            ViewBag.controllerValue = control;
            ViewBag.groupValue = group;
            ViewBag.selectedIdValue = selectedId;

            return View(gridModel);
        }

        [CheckSessionLogin]
        [CheckAuthorize]
        [HttpGet]
        public ActionResult Create(string key, string controller, string group = "")
        {
            ViewBag.Controllers = GetControllerList();

            ViewBag.CameraType = FunctionHelper.CameraTypes1();
            ViewBag.StreamType = FunctionHelper.StreamTypes1();
            ViewBag.SDK = FunctionHelper.SDKs1();

            ViewBag.keyValue = key;
            ViewBag.controllerValue = controller;
            ViewBag.groupValue = group;

            return View();
        }

        [HttpPost]
        public ActionResult Create(tblAccessCamera obj, string key, string controller, string group = "", bool SaveAndCountinue = false)
        {
            //
            ViewBag.Controllers = GetControllerList();

            //
            ViewBag.CameraType = FunctionHelper.CameraTypes1();
            ViewBag.StreamType = FunctionHelper.StreamTypes1();
            ViewBag.SDK = FunctionHelper.SDKs1();

            //
            ViewBag.keyValue = key;
            ViewBag.controllerValue = controller;
            ViewBag.groupValue = group;

            //
            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            //
            if (string.IsNullOrWhiteSpace(obj.CameraName))
            {
                ModelState.AddModelError("CameraName", FunctionHelper.GetLocalizeDictionary("Home", "notification")["Camera_Name"]);
                return View(obj);
            }

            //
            var existed = _tblAccessCameraService.GetByName(obj.CameraName);
            if (existed != null)
            {
                ModelState.AddModelError("CameraName", FunctionHelper.GetLocalizeDictionary("Home", "notification")["Camera_already_exists"]);
                return View(obj);
            }

            //
            obj.CameraID = Guid.NewGuid();
            obj.Cgi = FunctionHelper.GetCgiByCameraType(obj.CameraType, Convert.ToString(obj.FrameRate), obj.Resolution, obj.SDK, obj.UserName, obj.Password);
            obj.Password = !string.IsNullOrWhiteSpace(obj.Password) ? CryptorEngine.Encrypt(obj.Password, true) : CryptorEngine.Encrypt("", true);
            obj.SortOrder = 0;

            //Thực hiện thêm mới
            var result = _tblAccessCameraService.Create(obj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.CameraID.ToString(), obj.CameraName, "tblAccessCamera", ConstField.ParkingCode, ActionConfigO.Create);

                if (SaveAndCountinue)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction("Create", new { group = group, key = key, controller = controller, selectedId = obj.CameraID });
                }

                return RedirectToAction("Index", new { group = group, key = key, controller = controller, selectedId = obj.CameraID });
            }
            else
            {
                return View(obj);
            }
        }

        [CheckSessionLogin]
        [CheckAuthorize]
        [HttpGet]
        public ActionResult Update(string id, int page = 1, string key = "", string controller = "", string group = "")
        {
            ViewBag.Controllers = GetControllerList();

            ViewBag.CameraType = FunctionHelper.CameraTypes1();
            ViewBag.StreamType = FunctionHelper.StreamTypes1();
            ViewBag.SDK = FunctionHelper.SDKs1();

            ViewBag.keyValue = key;
            ViewBag.controllerValue = controller;
            ViewBag.PN = page;
            ViewBag.groupValue = group;

            var obj = _tblAccessCameraService.GetById(Guid.Parse(id));

            return View(obj);
        }

        [HttpPost]
        public ActionResult Update(tblAccessCamera obj, int page = 1, string key = "", string controller = "", string group = "")
        {
            ViewBag.Controllers = GetControllerList();

            ViewBag.CameraType = FunctionHelper.CameraTypes1();
            ViewBag.StreamType = FunctionHelper.StreamTypes1();
            ViewBag.SDK = FunctionHelper.SDKs1();

            ViewBag.keyValue = key;
            ViewBag.controllerValue = controller;
            ViewBag.PN = page;
            ViewBag.groupValue = group;

            //Kiểm tra
            var oldObj = _tblAccessCameraService.GetById(obj.CameraID);
            if (oldObj == null)
            {
                ViewBag.Error = FunctionHelper.GetLocalizeDictionary("Home", "notification")["record_does_not_exist"];
                return View(obj);
            }

            //
            if (string.IsNullOrWhiteSpace(obj.CameraName))
            {
                ModelState.AddModelError("CameraName", FunctionHelper.GetLocalizeDictionary("Home", "notification")["Camera_Name"]);
                return View(oldObj);
            }

            //
            var existed = _tblAccessCameraService.GetByName_Id(obj.CameraName, obj.CameraID);
            if (existed != null)
            {
                ModelState.AddModelError("CameraName", FunctionHelper.GetLocalizeDictionary("Home", "notification")["Camera_already_exists"]);
                return View(oldObj);
            }

            if (!ModelState.IsValid)
            {
                return View(oldObj);
            }

            oldObj.CameraCode = obj.CameraCode;
            oldObj.CameraName = obj.CameraName;
            oldObj.CameraType = obj.CameraType;
            oldObj.Channel = obj.Channel;
            oldObj.EnableRecording = obj.EnableRecording;
            oldObj.FrameRate = obj.FrameRate;
            oldObj.HttpPort = obj.HttpPort;
            oldObj.HttpURL = obj.HttpURL;
            oldObj.Inactive = obj.Inactive;
            oldObj.Password = "";
            oldObj.ControllerID = obj.ControllerID;
            oldObj.PositionIndex = obj.PositionIndex;
            oldObj.Resolution = obj.Resolution;
            oldObj.RtspPort = obj.RtspPort;
            oldObj.SDK = obj.SDK;
            //oldObj.SortOrder = obj.SortOrder;
            oldObj.StreamType = obj.StreamType;
            oldObj.UserName = obj.UserName;

            oldObj.Cgi = FunctionHelper.GetCgiByCameraType(obj.CameraType, Convert.ToString(obj.FrameRate), obj.Resolution, obj.SDK, obj.UserName, obj.Password);
            oldObj.Password = !string.IsNullOrWhiteSpace(obj.Password) ? CryptorEngine.Encrypt(obj.Password, true) : CryptorEngine.Encrypt("", true);

            //Thực hiện cập nhật
            var result = _tblAccessCameraService.Update(oldObj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.CameraID.ToString(), obj.CameraName, "tblAccessCamera", ConstField.ParkingCode, ActionConfigO.Update);

                return RedirectToAction("Index", new { group = group, page = page, key = key, controller = controller, selectedId = obj.CameraID });
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(oldObj);
            }
        }

        public JsonResult Delete(string id)
        {
            var obj = new tblAccessCamera();

            //var listLand = _PK_LaneService.GetAllByCamera(id);
            //if (listLand.Any())
            //{
            //    var message = new Result();

            //    message.Success = false;
            //    message.Message = "Đang sử dụng trong làn vào ra. Không thể xóa";
            //    message.ErrorCode = 500;

            //    return Json(message, JsonRequestBehavior.AllowGet);
            //}

            var result = _tblAccessCameraService.DeleteById(id, ref obj);

            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.CameraID.ToString(), obj.CameraName, "tblAccessCamera", ConstField.ParkingCode, ActionConfigO.Delete);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private List<tblAccessController> GetControllerList()
        {
            return _tblAccessControllerService.GetAllActive().ToList();
        }

        public JsonResult GetResolution(string key)
        {
            var listCus = new List<string>();

            var list = FunctionHelper.Resolution();

            list = list.Where(n => n.Text.Contains(key)).ToList();

            if (list.Any())
            {
                foreach (var item in list)
                {
                    listCus.Add(item.Text);
                }
            }

            return Json(listCus, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ViewCam(string id)
        {
            var report = new MessageReport() { isSuccess = false };
            try
            {
                var camera = _tblAccessCameraService.GetById(Guid.Parse(id));
                if (camera != null)
                {
                    string url = $"http://localhost:9000/api/getcamview";

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = "POST";
                    request.ContentType = "application/json";

                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        string json = JsonConvert.SerializeObject(camera);

                        streamWriter.Write(json);
                    }

                    request.GetResponse().Dispose();
                    report.isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                report.Message = ex.Message;
            }


            return Json(report, JsonRequestBehavior.AllowGet);
        }
    }
}