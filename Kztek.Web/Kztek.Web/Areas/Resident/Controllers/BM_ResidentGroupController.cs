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

namespace Kztek.Web.Areas.Resident.Controllers
{
    public class BM_ResidentGroupController : Controller
    {
        private IBM_ResidentGroupService _BM_ResidentGroupService;
        private ItblSystemConfigService _tblSystemConfigService;

        public BM_ResidentGroupController(IBM_ResidentGroupService _BM_ResidentGroupService, ItblSystemConfigService _tblSystemConfigService)
        {
            this._BM_ResidentGroupService = _BM_ResidentGroupService;
            this._tblSystemConfigService = _tblSystemConfigService;
        }

        #region Danh sách

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Index(string key, int? page, string group = "", string selectedId = "")
        {
            var lst = _BM_ResidentGroupService.GetAll().ToList();

            //Viewbag
            ViewBag.Keyword = key;
            //ViewBag.ListMenu = _MenuFunctionService.GetAll().ToList();

            ViewBag.GroupID = group;
            ViewBag.selectedIdValue = selectedId;

            return View(lst);
        }

        #endregion Danh sách

        public PartialViewResult MenuChilden(List<BM_ResidentGroup> childList, List<BM_ResidentGroup> AllList, string selectedId = "")
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
        public ActionResult Create(BM_ResidentGroup obj, bool SaveAndCountinue = false)
        {
            //ViewBag
            ViewBag.DDLMenu = GetMenuList();

            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            obj.Id = Guid.NewGuid().ToString();
            obj.IsDeleted = false;
            //Thực hiện thêm mới
            var result = _BM_ResidentGroupService.Create(obj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.Id.ToString(), obj.Name, "BM_ResidentGroup", ConstField.ResidentCode, ActionConfigO.Create);

                if (SaveAndCountinue)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction("Create", new { selectedId = obj.Id, parentid = obj.ParentId });
                }

                return RedirectToAction("Index", new { selectedId = obj.Id });
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

            var obj = _BM_ResidentGroupService.GetById(id);

            return View(obj);
        }

        [HttpPost]
        public ActionResult Update(BM_ResidentGroup obj)
        {
            ViewBag.DDLMenu = GetMenuList();

            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            var oldObj = _BM_ResidentGroupService.GetById(obj.Id);
            if (oldObj == null)
            {
                return View(obj);
            }

            oldObj.Name = obj.Name;
            oldObj.Ordering = obj.Ordering;
            oldObj.ParentId = obj.ParentId;

            //Thực hiện cập nhật
            var result = _BM_ResidentGroupService.Update(oldObj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.Id.ToString(), obj.Name, "BM_ResidentGroup", ConstField.ResidentCode, ActionConfigO.Update);

                return RedirectToAction("Index", new { selectedId = obj.Id });
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
            bool isSucccess = _BM_ResidentGroupService.DeleteByIds(lstId);
            if (isSucccess)
            {
                MessageReport report = new MessageReport(true, "Xóa thành công");
                WriteLog.Write(report, GetCurrentUser.GetUser(), lstId, lstId, "BM_ResidentGroup", ConstField.ResidentCode, ActionConfigO.Delete);
            }
            return Json(isSucccess, JsonRequestBehavior.AllowGet);
        }

        #endregion Xóa nhiều

        private List<BM_ResidentGroupSubmit> GetMenuList()
        {
            var list = new List<BM_ResidentGroupSubmit>
            {
                new BM_ResidentGroupSubmit {  Id = "0", Name = "- Chọn nhóm cư dân -" }
            };
            var MenuList = _BM_ResidentGroupService.GetAll();
            var parent = MenuList.Where(c => c.ParentId == "0" || c.ParentId == "");
            if (parent.Any())
            {
                foreach (var item in parent.OrderBy(c => c.Ordering))
                {
                    //Nếu có thì duyệt tiếp để lưu vào list
                    list.Add(new BM_ResidentGroupSubmit { Id = item.Id.ToString(), Name = item.Name });
                    //Gọi action để lấy danh sách submenu theo id
                    var submenu = Children(item.Id.ToString());
                    //Kiểm tra có submenu không
                    if (submenu.Count > 0)
                    {
                        //Nếu có thì duyệt tiếp để lưu vào list
                        foreach (var item1 in submenu)
                        {
                            list.Add(new BM_ResidentGroupSubmit { Id = item1.Id, Name = item.Name + " / " + item1.Name });
                        }
                        //Phân tách các danh mục
                        list.Add(new BM_ResidentGroupSubmit { Id = "-1", Name = "-----" });
                    }
                    else
                    {
                        //Phân tách các danh mục
                        list.Add(new BM_ResidentGroupSubmit { Id = "-1", Name = "-----" });
                    }
                }
            }
            return list;
        }

        private List<BM_ResidentGroupSubmit> Children(string parentID)
        {
            //Khai báo danh sách
            List<BM_ResidentGroupSubmit> lst = new List<BM_ResidentGroupSubmit>();
            //Lấy danh sách submenu theo id truyền từ action Parent()
            var menu = _BM_ResidentGroupService.GetAllChildActiveByParentID(parentID).ToList();
            //Kiểm tra có dữ liệu chưa
            if (menu.Any())
            {
                foreach (var item in menu)
                {
                    //Nếu có thì duyệt tiếp để lưu vào list
                    lst.Add(new BM_ResidentGroupSubmit { Id = item.Id.ToString(), Name = item.Name });
                    //Gọi action để lấy danh sách submenu theo id
                    var submenu = Children(item.Id.ToString());
                    //Kiểm tra có submenu không
                    if (submenu.Count > 0)
                    {
                        foreach (var item1 in submenu)
                        {
                            //Nếu có thì duyệt tiếp để lưu vào list
                            lst.Add(new BM_ResidentGroupSubmit { Id = item1.Id, Name = item.Name + " / " + item1.Name });
                        }
                    }
                }
            }
            return lst;
        }

        //public ActionResult ExportExcel()
        //{
        //    var listExcel = _BM_ResidentGroupService.GetExcel().ToList();

        //    if (listExcel != null && listExcel.Count > 0)
        //    {
        //        int count = 0;
        //        foreach (var item in listExcel)
        //        {
        //            count++;
        //            item.STT = count.ToString();
        //        }
        //    }

        //    PK_CustomerGroupFormatCell(listExcel, "Danh_sách_nhóm_khách_hàng", "Sheet1", "", "Danh sách nhóm khách hàng", "");

        //    return RedirectToAction("Index");
        //}

        //private void PK_CustomerGroupFormatCell(List<BM_ResidentGroupExcel> listData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
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
        //    listColumn.Add(new SelectListModel { ItemText = "STT", ItemValue = "STT" });
        //    listColumn.Add(new SelectListModel { ItemText = "Nhóm khách hàng", ItemValue = "CustomerGroupName" });

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
    }
}