using Kztek.Model.CustomModel;
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
    public class tblVehicleGroupController : Controller
    {
        private ItblVehicleGroupService _tblVehicleGroupService;

        public tblVehicleGroupController(ItblVehicleGroupService _tblVehicleGroupService)
        {
            this._tblVehicleGroupService = _tblVehicleGroupService;
        }

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Index(string group = "")
        {
            var list = _tblVehicleGroupService.GetAll();
            ViewBag.groupValue = group;
            return View(list);
        }

        [CheckSessionLogin]
        [CheckAuthorize]
        [HttpGet]
        public ActionResult Create(string group = "", string vehicletype = "")
        {
            ViewBag.VehicleTypes = GetListVehicleType();
            ViewBag.VehicleTypeValue = vehicletype;
            ViewBag.Group = group;

            return View();
        }

        [HttpPost]
        public ActionResult Create(tblVehicleGroup obj, string group = "", string vehicletype = "", bool SaveAndCountinue = false)
        {
            ViewBag.VehicleTypes = GetListVehicleType();
            ViewBag.VehicleTypeValue = vehicletype;
            ViewBag.Group = group;

            //Kiểm tra
            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            if (string.IsNullOrWhiteSpace(obj.VehicleGroupName))
            {
                ModelState.AddModelError("VehicleGroupName", "Vui lòng nhập tên nhóm xe");
                return View(obj);
            }

            //Gán giá trị
            obj.VehicleGroupID = Guid.NewGuid();

            //Thực hiện thêm mới
            var result = _tblVehicleGroupService.Create(obj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.VehicleGroupID.ToString(), obj.VehicleGroupName, "tblVehicleGroup", ConstField.ParkingCode, ActionConfigO.Create);

                if (SaveAndCountinue)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction("Create");
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View(obj);
            }
        }

        [CheckSessionLogin]
        [CheckAuthorize]
        [HttpGet]
        public ActionResult Update(string id, int page = 1, string group = "")
        {
            var obj = _tblVehicleGroupService.GetById(Guid.Parse(id));

            ViewBag.VehicleTypes = GetListVehicleType();
            ViewBag.Group = group;
            ViewBag.PN = page;

            return View(obj);
        }

        [HttpPost]
        public ActionResult Update(tblVehicleGroup obj, int page = 1, string group = "")
        {
            ViewBag.VehicleTypes = GetListVehicleType();
            ViewBag.Group = group;
            ViewBag.PN = page;

            //Kiểm tra
            var oldObj = _tblVehicleGroupService.GetById(obj.VehicleGroupID);
            if (oldObj == null)
            {
                ViewBag.Error = "Bản ghi không tồn tại";
                return View(obj);
            }

            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            if (string.IsNullOrWhiteSpace(obj.VehicleGroupName))
            {
                ModelState.AddModelError("VehicleGroupName", "Vui lòng nhập tên nhóm xe");
                return View(oldObj);
            }

            //Gán giá trị
            oldObj.VehicleGroupCode = obj.VehicleGroupCode;
            oldObj.Inactive = obj.Inactive;
            oldObj.LimitNumber = obj.LimitNumber;
            oldObj.VehicleGroupName = obj.VehicleGroupName;
            oldObj.SortOrder = obj.SortOrder;
            oldObj.VehicleType = obj.VehicleType;

            //Thực hiện thêm mới
            var result = _tblVehicleGroupService.Update(oldObj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.VehicleGroupID.ToString(), obj.VehicleGroupName, "tblVehicleGroup", ConstField.ParkingCode, ActionConfigO.Update);

                return RedirectToAction("Index", new { page = page });
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(oldObj);
            }
        }

        public JsonResult Delete(string id)
        {
            var obj = new tblVehicleGroup();

            //var listCardGroup = _tblCardGroupService.GetAllByVehicleGroupId(id);
            //if (listCardGroup.Any())
            //{
            //    var re = new Result();
            //    re.ErrorCode = 500;
            //    re.Message = "Nhóm xe đang được sử dụng trong nhóm thẻ. Không thể xóa!";
            //    re.Success = false;

            //    return Json(re, JsonRequestBehavior.AllowGet);
            //}

            var result = _tblVehicleGroupService.DeleteById(id, ref obj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.VehicleGroupID.ToString(), obj.VehicleGroupName, "tblVehicleGroup", ConstField.ParkingCode, ActionConfigO.Delete);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private List<SelectListModel> GetListVehicleType()
        {
            return FunctionHelper.VehicleType();
        }
    }
}