using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Web.Attributes;
using Kztek.Web.Core.Extensions;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Models;
using Kztek.Web.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kztek.Web.Areas.Locker.Controllers
{
    public class tblCustomerGroupController : Controller
    {
        private ItblCustomerGroupService _tblCustomerGroupService;
        private ItblSystemConfigService _tblSystemConfigService;

        public tblCustomerGroupController(ItblCustomerGroupService _tblCustomerGroupService, ItblSystemConfigService _tblSystemConfigService)
        {
            this._tblCustomerGroupService = _tblCustomerGroupService;
            this._tblSystemConfigService = _tblSystemConfigService;
        }

        #region Danh sách

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Index(string key, int? page, string group = "", string selectedId = "")
        {
            var lst = _tblCustomerGroupService.GetAll().ToList();

            //Viewbag
            ViewBag.Keyword = key;
            //ViewBag.ListMenu = _MenuFunctionService.GetAll().ToList();

            ViewBag.GroupID = group;
            ViewBag.selectedIdValue = selectedId;

            return View(lst);
        }

        #endregion Danh sách

        public PartialViewResult MenuChilden(List<tblCustomerGroup> childList, List<tblCustomerGroup> AllList, string selectedId = "")
        {
            //Viewbag
            ViewBag.ListMenu = AllList;
            ViewBag.selectedIdValue = selectedId;

            return PartialView(childList);
        }

        #region Thêm mới

        [HttpGet]
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Create(string controllername, string parentid)
        {
            //ViewBag
            ViewBag.DDLMenu = GetMenuList();
            ViewBag.parentidValue = parentid;

            return View();
        }

        [HttpPost]
        public ActionResult Create(tblCustomerGroup obj, bool SaveAndCountinue = false)
        {
            //ViewBag
            ViewBag.DDLMenu = GetMenuList();

            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            obj.CustomerGroupID = Guid.NewGuid();

            //Thực hiện thêm mới
            var result = _tblCustomerGroupService.Create(obj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.CustomerGroupID.ToString(), obj.CustomerGroupName, "tblCustomerGroup", ConstField.LockerCode, ActionConfigO.Create);

                if (SaveAndCountinue)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction("Create", new { selectedId = obj.CustomerGroupID, parentid = obj.ParentID });
                }

                return RedirectToAction("Index", new { selectedId = obj.CustomerGroupID });
            }
            else
            {
                return View(obj);
            }
        }

        #endregion Thêm mới

        #region Cập nhật

        [HttpGet]
        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Update(string id)
        {
            ViewBag.DDLMenu = GetMenuList();

            var obj = _tblCustomerGroupService.GetById(Guid.Parse(id));

            return View(obj);
        }

        [HttpPost]
        public ActionResult Update(tblCustomerGroup obj)
        {
            ViewBag.DDLMenu = GetMenuList();

            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            var oldObj = _tblCustomerGroupService.GetById(obj.CustomerGroupID);
            if (oldObj == null)
            {
                return View(obj);
            }

            oldObj.CustomerGroupCode = obj.CustomerGroupCode;
            oldObj.CustomerGroupName = obj.CustomerGroupName;
            oldObj.Description = obj.Description;
            oldObj.Inactive = obj.Inactive;
            oldObj.ParentID = obj.ParentID;
            oldObj.SortOrder = obj.SortOrder;

            //Thực hiện cập nhật
            var result = _tblCustomerGroupService.Update(oldObj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.CustomerGroupID.ToString(), obj.CustomerGroupName, "tblCustomerGroup", ConstField.LockerCode, ActionConfigO.Update);

                return RedirectToAction("Index", new { selectedId = obj.CustomerGroupID });
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(oldObj);
            }
        }

        #endregion Cập nhật

        #region Xóa nhiều

        public JsonResult MutilDelete(string lstId)
        {
            bool isSucccess = _tblCustomerGroupService.DeleteByIds(lstId);
            if (isSucccess)
            {
                MessageReport report = new MessageReport(true, "Xóa thành công");
                WriteLog.Write(report, GetCurrentUser.GetUser(), lstId, lstId, "tblCustomerGroup", ConstField.LockerCode, ActionConfigO.Delete);
            }
            return Json(isSucccess, JsonRequestBehavior.AllowGet);
        }

        #endregion Xóa nhiều

        private List<tblCustomerGroupSubmit> GetMenuList()
        {
            var list = new List<tblCustomerGroupSubmit>
            {
                new tblCustomerGroupSubmit {  CustomerGroupID = "0", CustomerGroupName = "- Chọn danh mục -" }
            };
            var MenuList = _tblCustomerGroupService.GetAllActive();
            var parent = MenuList.Where(c => c.ParentID == "0" || c.ParentID == "");
            if (parent.Any())
            {
                foreach (var item in parent.OrderBy(c => c.SortOrder))
                {
                    //Nếu có thì duyệt tiếp để lưu vào list
                    list.Add(new tblCustomerGroupSubmit { CustomerGroupID = item.CustomerGroupID.ToString(), CustomerGroupName = item.CustomerGroupName });
                    //Gọi action để lấy danh sách submenu theo id
                    var submenu = Children(item.CustomerGroupID.ToString());
                    //Kiểm tra có submenu không
                    if (submenu.Count > 0)
                    {
                        //Nếu có thì duyệt tiếp để lưu vào list
                        foreach (var item1 in submenu)
                        {
                            list.Add(new tblCustomerGroupSubmit { CustomerGroupID = item1.CustomerGroupID, CustomerGroupName = item.CustomerGroupName + " / " + item1.CustomerGroupName });
                        }
                        //Phân tách các danh mục
                        list.Add(new tblCustomerGroupSubmit { CustomerGroupID = "-1", CustomerGroupName = "-----" });
                    }
                    else
                    {
                        //Phân tách các danh mục
                        list.Add(new tblCustomerGroupSubmit { CustomerGroupID = "-1", CustomerGroupName = "-----" });
                    }
                }
            }
            return list;
        }

        private List<tblCustomerGroupSubmit> Children(string parentID)
        {
            //Khai báo danh sách
            List<tblCustomerGroupSubmit> lst = new List<tblCustomerGroupSubmit>();
            //Lấy danh sách submenu theo id truyền từ action Parent()
            var menu = _tblCustomerGroupService.GetAllChildActiveByParentID(parentID).ToList();
            //Kiểm tra có dữ liệu chưa
            if (menu.Any())
            {
                foreach (var item in menu)
                {
                    //Nếu có thì duyệt tiếp để lưu vào list
                    lst.Add(new tblCustomerGroupSubmit { CustomerGroupID = item.CustomerGroupID.ToString(), CustomerGroupName = item.CustomerGroupName });
                    //Gọi action để lấy danh sách submenu theo id
                    var submenu = Children(item.CustomerGroupID.ToString());
                    //Kiểm tra có submenu không
                    if (submenu.Count > 0)
                    {
                        foreach (var item1 in submenu)
                        {
                            //Nếu có thì duyệt tiếp để lưu vào list
                            lst.Add(new tblCustomerGroupSubmit { CustomerGroupID = item1.CustomerGroupID, CustomerGroupName = item.CustomerGroupName + " / " + item1.CustomerGroupName });
                        }
                    }
                }
            }
            return lst;
        }

        public ActionResult ExportExcel()
        {
            var listExcel = _tblCustomerGroupService.GetExcel().ToList();

            if (listExcel != null && listExcel.Count > 0)
            {
                int count = 0;
                foreach (var item in listExcel)
                {
                    count++;
                    item.STT = count.ToString();
                }
            }

            PK_CustomerGroupFormatCell(listExcel, "Danh_sách_nhóm_khách_hàng", "Sheet1", "", "Danh sách nhóm khách hàng", "");

            return RedirectToAction("Index");
        }

        private void PK_CustomerGroupFormatCell(List<tblCustomerGroupExcel> listData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = "";

            if (!string.IsNullOrWhiteSpace(titleTime))
            {
                timeSearch = "Từ " + titleTime.Split(new[] { '-' })[0] + " đến " + titleTime.Split(new[] { '-' })[1];
            }

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = "STT", ItemValue = "STT" });
            listColumn.Add(new SelectListModel { ItemText = "Nhóm khách hàng", ItemValue = "CustomerGroupName" });

            //Chuyển dữ liệu về datatable
            DataTable dt = listData.ToDataTableNullable();

            //Xuất file
            ExportFile(dt, listColumn, dtHeader, filename, sheetname, comments);
        }

        private void ExportFile(DataTable list = null, List<SelectListModel> listTitle = null, DataTable dtHeader = null, string filename = "", string sheetname = "", string comments = "")
        {
            // Gọi lại hàm để tạo file excel
            var stream = FunctionHelper.WriteToExcel(null, list, listTitle, dtHeader, sheetname, comments);
            // Tạo buffer memory strean để hứng file excel
            var buffer = stream as MemoryStream;
            // Đây là content Type dành cho file excel, còn rất nhiều content-type khác nhưng cái này mình thấy okay nhất
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            // Dòng này rất quan trọng, vì chạy trên firefox hay IE thì dòng này sẽ hiện Save As dialog cho người dùng chọn thư mục để lưu
            // File name của Excel này là ExcelDemo
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename={0}-{1}.xlsx", filename, DateTime.Now.ToString("yyyyMMdd")));
            // Lưu file excel của chúng ta như 1 mảng byte để trả về response
            Response.BinaryWrite(buffer.ToArray());
            // Send tất cả ouput bytes về phía clients
            Response.Flush();
            Response.End();
        }
    }
}