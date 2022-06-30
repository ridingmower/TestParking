using Kztek.Model.CustomModel;
using Kztek.Model.CustomModel.iParking;
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
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kztek.Web.Areas.Resident.Controllers
{
    public class BM_ResidentController : Controller
    {
        private IBM_ResidentService _BM_ResidentService;
        private IBM_ResidentGroupService _BM_ResidentGroupService;
        private ItblSystemConfigService _tblSystemConfigService;
        private ItblCardService _tblCardService;


        public BM_ResidentController(IBM_ResidentService _BM_ResidentService, IBM_ResidentGroupService _BM_ResidentGroupService,
            ItblSystemConfigService _tblSystemConfigService, ItblCardService _tblCardService)
        {
            this._BM_ResidentService = _BM_ResidentService;
            this._BM_ResidentGroupService = _BM_ResidentGroupService;
            this._tblSystemConfigService = _tblSystemConfigService;
            this._tblCardService = _tblCardService;
        }

        #region Danh sách

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Index(string key = "", string residentGroup = "", int page = 1, string chkExport = "0", string selectedId = "")
        {
            var str = GetListChild("", residentGroup);

            //if (chkExport.Equals("1"))
            //{
            //    var listExcel = _BM_ResidentService.ExcelAllByFirst(key, str, customerstatus);

            //    //Xuất file theo format
            //    PK_CustomerMapFormatCell(listExcel, "Danh_sách_khách_hàng", "Sheet1", "", "Danh sách khách hàng", "");

            //    return RedirectToAction("Index", new { key = key, customergroup = customergroup, customerstatus = customerstatus, page = page });
            //}

            var pageSize = 20;

            var list = _BM_ResidentService.GetAllPagingByFirst(key, str, page, pageSize);
            if (list.Any())
            {
                var lstId = "";
                var lstCustomerId = "";
                foreach (var item in list)
                {
                    lstId += item.ResidentGroupId + ";";
                    lstCustomerId += item.Id.ToString() + ",";
                }

                ViewBag.LstResidentGroup = _BM_ResidentGroupService.GetAll().ToList();
                //ViewBag.listCardCustomer = _PK_CardCustomerService.GetAllCardByListCustomerId(lstCustomerId).ToList();
            }

            var gridModel = PageModelCustom<BM_Resident>.GetPage(list, page, pageSize);

            ViewBag.keyValue = key;
            ViewBag.ResidentGroupValue = residentGroup;
            ViewBag.selectedIdValue = selectedId;

            ViewBag.ResidentGroup = GetMenuList();

            return View(gridModel);
        }

        //Excell
        //private void PK_CustomerMapFormatCell(List<BM_Resident_Excel> listData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        //{
        //    //Nội dung đầu trang
        //    var user = GetCurrentUser.GetUser();

        //    var timeSearch = "";

        //    if (!string.IsNullOrWhiteSpace(titleTime))
        //    {
        //        timeSearch = "Từ " + titleTime.Split(new[] { '-' })[0] + " đến " + titleTime.Split(new[] { '-' })[1];
        //    }

        //    var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

        //    //Header
        //    var listColumn = new List<SelectListModel>();
        //    listColumn.Add(new SelectListModel { ItemText = "STT", ItemValue = "NumberRow" });
        //    listColumn.Add(new SelectListModel { ItemText = "Mã khách hàng", ItemValue = "CustomerCode" });
        //    listColumn.Add(new SelectListModel { ItemText = "Khách hàng", ItemValue = "CustomerName" });
        //    listColumn.Add(new SelectListModel { ItemText = "Chứng minh thư", ItemValue = "CustomerIdentify" });
        //    listColumn.Add(new SelectListModel { ItemText = "Địa chỉ", ItemValue = "CustomerAddress" });
        //    listColumn.Add(new SelectListModel { ItemText = "Số điện thoại", ItemValue = "CustomerMobile" });
        //    listColumn.Add(new SelectListModel { ItemText = "Nhóm khách hàng", ItemValue = "CustomerGroupName" });
        //    listColumn.Add(new SelectListModel { ItemText = "Mã thẻ (Số thẻ)", ItemValue = "Cards" });
        //    listColumn.Add(new SelectListModel { ItemText = "Biển số", ItemValue = "Plates" });
        //    listColumn.Add(new SelectListModel { ItemText = "Hoạt động", ItemValue = "Active" });

        //    //Chuyển dữ liệu về datatable
        //    DataTable dt = listData.ToDataTableNullable();

        //    //Xuất file
        //    ExportFile(dt, listColumn, dtHeader, filename, sheetname, comments);
        //}

        //private void ExportFile(DataTable list = null, List<SelectListModel> listTitle = null, DataTable dtHeader = null, string filename = "", string sheetname = "", string comments = "")
        //{
        //    // Gọi lại hàm để tạo file excel
        //    var stream = FunctionHelper.WriteToExcel(null, list, listTitle, dtHeader, sheetname, comments);
        //    // Tạo buffer memory strean để hứng file excel
        //    var buffer = stream as MemoryStream;
        //    // Đây là content Type dành cho file excel, còn rất nhiều content-type khác nhưng cái này mình thấy okay nhất
        //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //    // Dòng này rất quan trọng, vì chạy trên firefox hay IE thì dòng này sẽ hiện Save As dialog cho người dùng chọn thư mục để lưu
        //    // File name của Excel này là ExcelDemo
        //    Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}-{1}.xlsx", filename, DateTime.Now.ToString("yyyyMMdd")));
        //    // Lưu file excel của chúng ta như 1 mảng byte để trả về response
        //    Response.BinaryWrite(buffer.ToArray());
        //    // Send tất cả ouput bytes về phía clients
        //    Response.Flush();
        //    Response.End();
        //}
        #endregion

        #region Thêm mới
        [CheckSessionLogin]
        [CheckAuthorize]
        [HttpGet]
        public ActionResult Create(string key = "", string residentGroup = "", int page = 1)
        {
            ViewBag.keyValue = key;
            ViewBag.ResidentGroupValue = residentGroup;
            ViewBag.ResidentGroups = GetMenuList();
            // ViewBag.ControllerList = _tblAccessControllerService.GetAllActive();
            // ViewBag.LevelList = _tblAccessLevelService.GetAllActive();

            return View();
        }

        [HttpPost]
        public ActionResult Create(BM_Resident obj,  bool SaveAndCountinue = false, string key = "", string residentGroupId = "", string ResidentGroup = "")
        {
            ViewBag.keyValue = key;
            ViewBag.ResidentGroupValue = residentGroupId;
            ViewBag.ResidentGroups = GetMenuList();
            // ViewBag.ControllerList = _tblAccessControllerService.GetAllActive();
            // ViewBag.LevelList = _tblAccessLevelService.GetAllActive();

            //Kiểm tra
            if (!ModelState.IsValid)
            {
                return View(obj);
            }

           
            //Gán giá trị
            obj.Id = Guid.NewGuid().ToString();
            obj.ResidentGroupId = ResidentGroup;
            obj.DateCreated = DateTime.UtcNow;
            obj.IsDeleted = false;

            //Thực hiện thêm mới
            var result = _BM_ResidentService.Create(obj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.Id.ToString(), obj.Id.ToString(), "BM_Resident", ConstField.ResidentCode, ActionConfigO.Create);

                if (SaveAndCountinue)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction("Create", new { key = key, residentGroupId = residentGroupId, selectedId = obj.Id });
                }

                return RedirectToAction("Index", new { key = key, residentGroupId = residentGroupId, selectedId = obj.Id });
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(obj);
            }
        }
        #endregion

        #region Cập nhật
        [CheckSessionLogin]
        [CheckAuthorize]
        [HttpGet]
        public ActionResult Update(string id, string key = "", string residentGroup = "", int page = 1)
        {
            ViewBag.keyValue = key;
            ViewBag.ResidentGroupValue = residentGroup;
            ViewBag.PN = page;

            ViewBag.ResidentGroups = GetMenuList();

            var obj = _BM_ResidentService.GetById(id);

            return View(obj);
        }

        [HttpPost]
        public ActionResult Update(BM_Resident obj, string key = "", string residentGroupIdSearch = "", int page = 1, string ResidentGroup = "")
        {
            ViewBag.keyValue = key;
            ViewBag.residentGroupIdValue = residentGroupIdSearch;
            ViewBag.PN = page;

            ViewBag.CustomerGroups = GetMenuList();
            // ViewBag.ControllerList = _tblAccessControllerService.GetAllActive();
            //ViewBag.LevelList = _tblAccessLevelService.GetAllActive();

            //Kiểm tra
            var oldObj = _BM_ResidentService.GetById(obj.Id);
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
            oldObj.Name = obj.Name;
            oldObj.Mobile = obj.Mobile;
            oldObj.Email = obj.Email;
            oldObj.CustomerId = obj.CustomerId;
            oldObj.ResidentGroupId = ResidentGroup;
            oldObj.Note = obj.Note;
            oldObj.Description = obj.Description;
            oldObj.Description = obj.Description;

            //Thực hiện cập nhật
            var result = _BM_ResidentService.Update(oldObj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.Id.ToString(), obj.Name, "BM_Resident", ConstField.ResidentCode, ActionConfigO.Update);

                return RedirectToAction("Index", new { page = page, key = key, residentGroupId = residentGroupIdSearch,  selectedId = oldObj.Id });
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(oldObj);
            }
        }
        #endregion

        #region Xóa
        public JsonResult Delete(string id)
        {
            var obj = new BM_Resident();

            //Check tồn tại trong cardcustomer
            //var existedInCard = _tblCardService.GetAllByCustomerId(id);
            //if (existedInCard.Any())
            //{
            //    var result1 = new MessageReport();
            //    result1.Message = "Khách hàng đang sử dụng thẻ. Không thể xóa.";
            //    result1.isSuccess = false;
            //}

            //Check tồn tại trong event
            //var existedInEvent = _PK_VehicleEventService.GetAllEventByCustomerId(id);
            //if (existedInEvent.Any())
            //{
            //    var result1 = new Result();
            //    result1.ErrorCode = 500;
            //    result1.Message = "Khách hàng đang tồn tại trong sự kiện. Không thể xóa.";
            //    result1.Success = false;
            //}

            var result = _BM_ResidentService.DeleteById(id, ref obj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.Id.ToString(), obj.Name, "BM_Resident", ConstField.ResidentCode, ActionConfigO.Delete);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Upload file
        private void UploadFile(HttpPostedFileBase fileUpload)
        {
            if (fileUpload != null)
            {
                string error = "";

                var url = ConfigurationManager.AppSettings["FileUploadAvatar"];

                Common.UploadFile(out error, Server.MapPath(url), fileUpload);
            }
        }
        #endregion

        #region Danh sách hõ trợ
        /// <summary>
        /// Danh sách menu cấp cha
        /// </summary>
        /// <modified>
        /// Author                  Date                Comments
        /// TrungNQ                 04/08/2017          Tạo mới
        /// </modified>
        /// <returns></returns>
        private List<SelectListModel> GetMenuList()
        {
            var list = new List<SelectListModel>
            {
                new SelectListModel {  ItemValue = "", ItemText = "- Chọn danh mục -" }
            };
            var MenuList = _BM_ResidentGroupService.GetAll().ToList();
            var parent = MenuList.Where(c => c.ParentId == "0" || c.ParentId == "");
            if (parent.Any())
            {
                foreach (var item in parent.OrderBy(c => c.Ordering))
                {
                    //Nếu có thì duyệt tiếp để lưu vào list
                    list.Add(new SelectListModel { ItemValue = item.Id.ToString(), ItemText = item.Name });
                    //Gọi action để lấy danh sách submenu theo id
                    var submenu = Children(item.Id.ToString());
                    //Kiểm tra có submenu không
                    if (submenu.Count > 0)
                    {
                        //Nếu có thì duyệt tiếp để lưu vào list
                        foreach (var item1 in submenu)
                        {
                            list.Add(new SelectListModel { ItemValue = item1.ItemValue.ToString(), ItemText = item.Name + " / " + item1.ItemText });
                        }
                        //Phân tách các danh mục
                        list.Add(new SelectListModel { ItemValue = "-1", ItemText = "-----" });
                    }
                    else
                    {
                        //Phân tách các danh mục
                        list.Add(new SelectListModel { ItemValue = "-1", ItemText = "-----" });
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Đệ quy để lấy danh sách con
        /// </summary>
        /// <modified>
        /// Author                  Date                Comments
        /// TrungNQ                 04/08/2017          Tạo mới
        /// </modified>
        /// <param name="parentID"></param>
        /// <returns></returns>
        private List<SelectListModel> Children(string parentID)
        {
            //Khai báo danh sách
            List<SelectListModel> lst = new List<SelectListModel>();
            //Lấy danh sách submenu theo id truyền từ action Parent()
            var menu = _BM_ResidentGroupService.GetAllChildByParentID(parentID.ToString()).ToList();
            //Kiểm tra có dữ liệu chưa
            if (menu.Any())
            {
                foreach (var item in menu)
                {
                    //Nếu có thì duyệt tiếp để lưu vào list
                    lst.Add(new SelectListModel { ItemValue = item.Id.ToString(), ItemText = item.Name });
                    //Gọi action để lấy danh sách submenu theo id
                    var submenu = Children(item.Id.ToString());
                    //Kiểm tra có submenu không
                    if (submenu.Count > 0)
                    {
                        foreach (var item1 in submenu)
                        {
                            //Nếu có thì duyệt tiếp để lưu vào list
                            lst.Add(new SelectListModel { ItemValue = item1.ItemValue, ItemText = item.Name + " / " + item1.ItemText });
                        }
                    }
                }
            }
            return lst;
        }

        private string GetListChild(string str, string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    str += id + ",";
                }

                var list = _BM_ResidentGroupService.GetAllChildActiveByParentID(id).ToList();
                if (list.Any())
                {
                    foreach (var item in list)
                    {
                        str += item.Id.ToString() + ",";
                        GetListChild(str, item.Id.ToString());
                    }
                }
            }

            return str;
        }


        #endregion

        //public PartialViewResult ModalButtonControl(int totalItem = 0, string url = "")
        //{
        //    //var listCardChoice = GetSetFromSession(null);

        //    ViewBag.totalItemValue = totalItem;
        //    ViewBag.urlValue = url;
        //    ViewBag.llevels = GetListAccessLevel();

        //    return PartialView();
        //}
    }
}