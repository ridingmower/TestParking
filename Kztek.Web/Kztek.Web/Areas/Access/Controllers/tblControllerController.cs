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

namespace Kztek.Web.Areas.Access.Controllers
{
    public class tblControllerController : Controller
    {
        #region Khai báo services

        private ItblAccessControllerService _tblAccessControllerService;
        private ItblAccessLineService _tblAccessLineService;
        private ItblAccessControllerGroupService _tblAccessControllerGroupService;
        private ItblAccessDoorService _tblAccessDoorService;
        private static string url = "";

        public tblControllerController(ItblAccessControllerService _tblAccessControllerService, ItblAccessLineService _tblAccessLineService, ItblAccessControllerGroupService _tblAccessControllerGroupService, ItblAccessDoorService _tblAccessDoorService)
        {
            this._tblAccessControllerService = _tblAccessControllerService;
            this._tblAccessLineService = _tblAccessLineService;
            this._tblAccessControllerGroupService = _tblAccessControllerGroupService;
            this._tblAccessDoorService = _tblAccessDoorService;
        }

        #endregion Khai báo services

        #region Danh sách

        /// <summary>
        /// Danh sách bộ điều khiển
        /// </summary>
        /// <modified>
        /// Author              Date            Comments
        /// TrungNQ             01/09/2017      Tạo mới
        /// </modified>
        /// <param name="key">Từ khóa</param>
        /// <param name="pc">Id máy tính</param>
        /// <param name="page">Trang hiện tại</param>
        /// <returns></returns>
        [CheckSessionLogin]
        public ActionResult Index(string key = "", string line = "", string GroupControllerId = "", int page = 1)
        {
            //var totalPage = 0;
            //var totalItem = 0;
            var pageSize = 20;

            var list = _tblAccessControllerService.GetAllPagingByFirst_AccessController(key, line, GroupControllerId, page, pageSize);
            foreach (var item in list)
            {
                if (!String.IsNullOrEmpty(item.ControllerGroupId))
                {
                    var obj = GetControllerGroupList().Where(n => n.Id.ToLower() == item.ControllerGroupId.ToLower()).ToList()/*.FirstOrDefault().Name*/;                
                    if (obj.Count() != 0 )
                    {
                        var ControllerGrName = obj.FirstOrDefault().Name;
                        item.ControllerGroupId = ControllerGrName;
                    }
                    
                }
            }
            var gridModel = PageModelCustom<tblAccessController>.GetPage(list, page, pageSize);

            ViewBag.KeyWord = key;
            ViewBag.Lines = GetLineList();
            ViewBag.LineID = line;
            ViewBag.GroupController = GetControllerGroupList();
            ViewBag.GroupControllerId = GroupControllerId;

            url = Request.Url.PathAndQuery;

            return View(gridModel);
        }

        #endregion Danh sách

        #region Thêm mới

        /// <summary>
        /// Giao diện thêm mới
        /// </summary>
        /// <modified>
        /// Author              Date            Comments
        /// TrungNQ             01/09/2017      Tạo mới
        /// </modified>
        /// <returns></returns>
        [CheckSessionLogin]
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Lines = GetLineList();
            ViewBag.FtController = FunctionHelper.FunctionController();
            ViewBag.GroupController = GetControllerGroupList();

            ViewBag.urlValue = url ?? Request.Url.PathAndQuery;

            return View();
        }

        /// <summary>
        /// Thực hiện thêm mới
        /// </summary>
        /// <modified>
        /// Author              Date            Comments
        /// TrungNQ             01/09/2017      Tạo mới
        /// </modified>
        /// <param name="obj">Đối tượng</param>
        /// <param name="SaveAndCountinue">Thêm liên tục hay không?</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(tblAccessController obj, bool SaveAndCountinue = false)
        {
            //Danh sách sử dụng
            ViewBag.Lines = GetLineList();
            ViewBag.FtController = FunctionHelper.FunctionController();
            ViewBag.GroupController = GetControllerGroupList();

            ViewBag.urlValue = url ?? Request.Url.PathAndQuery;

            //Kiểm tra
            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            //Gán giá trị
            obj.ControllerID = Guid.NewGuid();

            //Thực hiện thêm mới
            var result = _tblAccessControllerService.Create(obj);
            if (result.isSuccess)
            {
                AddOutput(obj);

                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.ControllerID.ToString(), obj.ControllerName, "tblAccessController", ConstField.AccessControlCode, ActionConfigO.Create);

                if (SaveAndCountinue)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction("Create");
                }

                return Redirect(url);
            }
            else
            {
                return View(obj);
            }
        }

        #endregion Thêm mới

        #region Cập nhật

        /// <summary>
        /// Giao diện cập nhật
        /// </summary>
        /// <modified>
        /// Author              Date            Comments
        /// TrungNQ             01/09/2017      Tạo mới
        /// </modified>
        /// <param name="id">Id bản ghi</param>
        /// <param name="pageNumber">Trang hiện tại</param>
        /// <returns></returns>
        [CheckSessionLogin]
        [HttpGet]
        public ActionResult Update(string id , string ob ="")
        {
           
            var obj = _tblAccessControllerService.GetById(Guid.Parse(id));
            ViewBag.obs = obj.FunctionController;
            ViewBag.FtController = FunctionHelper.FunctionController();
            ViewBag.Lines = GetLineList();
            ViewBag.urlValue = url ?? Request.Url.PathAndQuery;
            ViewBag.GroupController = GetControllerGroupList();
            return View(obj);
        }

        /// <summary>
        /// Thực hiện cập nhật
        /// </summary>
        /// <modified>
        /// Author              Date            Comments
        /// TrungNQ             01/09/2017      Tạo mới
        /// </modified>
        /// <param name="obj">Đối tượng</param>
        /// <param name="objId">Id bản ghi</param>
        /// <param name="pageNumber">Trang hiện tại</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Update(tblAccessController obj)
        {
            ViewBag.FtController = FunctionHelper.FunctionController();
            //Danh sách sử dụng
            ViewBag.Lines = GetLineList();
            ViewBag.GroupController = GetControllerGroupList();

            ViewBag.urlValue = url ?? Request.Url.PathAndQuery;

            //Kiểm tra
            var oldObj = _tblAccessControllerService.GetById(obj.ControllerID);
            if (oldObj == null)
            {
                ViewBag.Error = "Bản ghi không tồn tại";
                return View(obj);
            }

            if (!ModelState.IsValid)
            {
                return View(oldObj);
            }

            //Gán giá trị
            oldObj.ControllerName = obj.ControllerName;
            oldObj.Inactive = obj.Inactive;
            oldObj.LineID = obj.LineID;
            oldObj.Address = obj.Address;
            oldObj.FunctionController = obj.FunctionController;
            oldObj.ControllerGroupId = obj.ControllerGroupId;
            oldObj.IsAddOutput = obj.IsAddOutput;
            oldObj.NumberOutput = obj.NumberOutput;

            //Thực hiện cập nhật
            var result = _tblAccessControllerService.Update(oldObj);
            if (result.isSuccess)
            {
                AddOutput(obj);

                WriteLog.Write(result, GetCurrentUser.GetUser(), oldObj.ControllerID.ToString(), oldObj.ControllerName, "tblAccessController", ConstField.AccessControlCode, ActionConfigO.Update);

                return Redirect(url);
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(oldObj);
            }
        }

        #endregion Cập nhật

        #region Xóa

        /// <summary>
        /// Xóa
        /// </summary>
        /// <modified>
        /// Author              Date            Comments
        /// TrungNQ             01/09/2017      Tạo mới
        /// </modified>
        /// <param name="id">Id bản ghi</param>
        /// <returns></returns>
        public JsonResult Delete(string id)
        {
            var result = _tblAccessControllerService.DeleteById(id);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), id, id, "tblAccessController", ConstField.AccessControlCode, ActionConfigO.Delete);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #endregion Xóa


        /// <summary>
        /// Thêm output tự động
        /// </summary>
        /// <param name="model"></param>
        void AddOutput(tblAccessController model)
        {
            if (model.IsAddOutput && model.NumberOutput > 0)
            {
                for(int i = 1; i <= model.NumberOutput; i++)
                {
                    var obj = new tblAccessDoor
                    {
                        ControllerID = model.ControllerID.ToString(),
                        DoorName = "Output " + i.ToString(),
                        Inactive = false,
                        Ordering = i,
                        ReaderIndex = i.ToString(),
                        DoorID = Guid.NewGuid()
                    };

                    var isExist = _tblAccessDoorService.GetByController_Readerindex(obj.ControllerID,obj.ReaderIndex);
                    if (isExist == null)
                    {
                        _tblAccessDoorService.Create(obj);
                    }
                }
            }
        }

        private List<tblAccessLine> GetLineList()
        {
            return _tblAccessLineService.GetAllActive().ToList();
        }

        private List<tblAccessControllerGroup> GetControllerGroupList()
        {
            return _tblAccessControllerGroupService.GetAll().OrderBy(n=>n.SortOrder).ToList();
        }
    }
}