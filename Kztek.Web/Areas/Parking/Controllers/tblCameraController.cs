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
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Kztek.Web.Areas.Parking.Controllers
{
    public class tblCameraController : Controller
    {
        private ItblCameraService _tblCameraService;
        private ItblPCService _tblPCService;

        public tblCameraController(ItblCameraService _tblCameraService, ItblPCService _tblPCService)
        {
            this._tblCameraService = _tblCameraService;
            this._tblPCService = _tblPCService;
        }

        private const string groupId = ConstField.ParkingID;

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Index(string key, string pc, int page = 1, string group = "", string selectedId = "")
        {
            var pageSize = 20;

            var list = _tblCameraService.GetAllCustomPagingByFirst(key, pc, page, pageSize);

            var gridModel = PageModelCustom<tblCameraCustomViewModel>.GetPage(list, page, pageSize);

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

            ViewBag.CameraType = FunctionHelper.CameraTypes1();
            ViewBag.StreamType = FunctionHelper.StreamTypes1();
            ViewBag.SDK = FunctionHelper.SDKs1();
            ViewBag.Resolutions = FunctionHelper.AllResolution();
            ViewBag.keyValue = key;
            ViewBag.pcValue = pc;
            ViewBag.groupValue = group;
            var obj = new tblCamera();
            obj.HttpURL = "192.168.1.100";
            obj.HttpPort = "80";
            obj.RtspPort = "554";
        
            return View(obj);
        }

        [HttpPost]
        public ActionResult Create(tblCamera obj, string key, string pc, string group = "", bool SaveAndCountinue = false)
        {
            //
            ViewBag.PCs = GetPCList();

            //
            ViewBag.CameraType = FunctionHelper.CameraTypes1();
            ViewBag.StreamType = FunctionHelper.StreamTypes1();
            ViewBag.SDK = FunctionHelper.SDKs1();
            ViewBag.Resolutions = FunctionHelper.AllResolution();
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
            if (string.IsNullOrWhiteSpace(obj.CameraName))
            {
                ModelState.AddModelError("CameraName", FunctionHelper.GetLocalizeDictionary("Home", "notification")["Camera_Name"]);
                return View(obj);
            }

            //
            var existed = _tblCameraService.GetByName(obj.CameraName);
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
            var result = _tblCameraService.Create(obj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.CameraID.ToString(), obj.CameraName, "tblCamera", ConstField.ParkingCode, ActionConfigO.Create);

                if (SaveAndCountinue)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction("Create", new { group = group, key = key, pc = pc, selectedId = obj.CameraID });
                }

                return RedirectToAction("Index", new { group = group, key = key, pc = pc, selectedId = obj.CameraID });
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

            ViewBag.CameraType = FunctionHelper.CameraTypes1();
            ViewBag.StreamType = FunctionHelper.StreamTypes1();
            ViewBag.SDK = FunctionHelper.SDKs1();
            ViewBag.Resolutions = FunctionHelper.AllResolution();
            ViewBag.keyValue = key;
            ViewBag.pcValue = pc;
            ViewBag.PN = page;
            ViewBag.groupValue = group;

            var obj = _tblCameraService.GetById(Guid.Parse(id));

            return View(obj);
        }

        [HttpPost]
        public ActionResult Update(tblCamera obj, int page = 1, string key = "", string pc = "", string group = "")
        {
            ViewBag.PCs = GetPCList();

            ViewBag.CameraType = FunctionHelper.CameraTypes1();
            ViewBag.StreamType = FunctionHelper.StreamTypes1();
            ViewBag.SDK = FunctionHelper.SDKs1();
            ViewBag.Resolutions = FunctionHelper.AllResolution();
            ViewBag.keyValue = key;
            ViewBag.pcValue = pc;
            ViewBag.PN = page;
            ViewBag.groupValue = group;

            //Kiểm tra
            var oldObj = _tblCameraService.GetById(obj.CameraID);
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
            var existed = _tblCameraService.GetByName_Id(obj.CameraName, obj.CameraID);
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
            oldObj.PCID = obj.PCID;
            oldObj.PositionIndex = obj.PositionIndex;
            oldObj.Resolution = obj.Resolution;
            oldObj.RtspPort = obj.RtspPort;
            oldObj.SDK = obj.SDK;
            //oldObj.SortOrder = obj.SortOrder;
            oldObj.StreamType = obj.StreamType;
            oldObj.UserName = obj.UserName;
            oldObj.ResizeImage = obj.ResizeImage;
            oldObj.Cgi = FunctionHelper.GetCgiByCameraType(obj.CameraType, Convert.ToString(obj.FrameRate), obj.Resolution, obj.SDK, obj.UserName, obj.Password);
            oldObj.Password = !string.IsNullOrWhiteSpace(obj.Password) ? CryptorEngine.Encrypt(obj.Password, true) : CryptorEngine.Encrypt("", true);

            //Thực hiện cập nhật
            var result = _tblCameraService.Update(oldObj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.CameraID.ToString(), obj.CameraName, "tblCamera", ConstField.ParkingCode, ActionConfigO.Update);

                return RedirectToAction("Index", new { group = group, page = page, key = key, pc = pc, selectedId = obj.CameraID });
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(oldObj);
            }
        }

        public JsonResult Delete(string id)
        {
            var obj = new tblCamera();

            //var listLand = _PK_LaneService.GetAllByCamera(id);
            //if (listLand.Any())
            //{
            //    var message = new Result();

            //    message.Success = false;
            //    message.Message = "Đang sử dụng trong làn vào ra. Không thể xóa";
            //    message.ErrorCode = 500;

            //    return Json(message, JsonRequestBehavior.AllowGet);
            //}

            var result = _tblCameraService.DeleteById(id, ref obj);

            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.CameraID.ToString(), obj.CameraName, "tblCamera", ConstField.ParkingCode, ActionConfigO.Delete);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private List<tblPC> GetPCList()
        {
            return _tblPCService.GetAllActive().ToList();
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

        #region edit ảnh camera
        public ActionResult Partial_Image(string camid,string pcid)
        {
            string url = "";

            if (!string.IsNullOrEmpty(pcid))
            {
                var pc = _tblPCService.GetById(Guid.Parse(pcid));

                if (pc != null && !string.IsNullOrEmpty(pc.PicPathIn) && !string.IsNullOrEmpty(camid))
                {
                    var name = string.Format("InitalImage_CamID_{0}.jpg", camid);

                    url =string.Format("{0}\\{1}", pc.PicPathIn, name);

                    using (WebClient client = new WebClient())
                    {
                        var fileName = string.Format("InitalImage_CamID_{0}.jpg", camid);

                        string folderfile = Server.MapPath("/uploads/") + fileName;                                      

                        client.DownloadFile(new Uri(url), folderfile);

                        var upload = ConfigurationManager.AppSettings["FileUploadAvatar"];

                        var path = string.Format("{0}{1}", upload, fileName);

                        ViewBag.Path = path;
                    }
                }
            }          
            
            return PartialView();
        }
        #endregion
    }
}