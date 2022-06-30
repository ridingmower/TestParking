using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Web.Attributes;
using Kztek.Web.Core.Extensions;
using Kztek.Web.Core.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kztek.Web.Areas.Access.Controllers
{
    public class tblAccessUploadProcessController : Controller
    {
        private ItblAccessUploadProcessService _tblAccessUploadProcessService;
        private ItblAccessControllerService _tblAccessControllerService;
        private ItblCustomerGroupService _tblCustomerGroupService;
        private ItblCardGroupService _tblCardGroupService;
        private ItblSystemConfigService _tblSystemConfigService;
        private IUserService _UserService;
        private ItblCustomerService _tblCustomerService;

        public tblAccessUploadProcessController(ItblAccessUploadProcessService _tblAccessUploadProcessService, ItblCustomerGroupService _tblCustomerGroupService, ItblCardGroupService _tblCardGroupService, IUserService _UserService, ItblAccessControllerService _tblAccessControllerService, ItblSystemConfigService _tblSystemConfigService, ItblCustomerService _tblCustomerService)
        {
            this._tblAccessUploadProcessService = _tblAccessUploadProcessService;
            this._tblCustomerGroupService = _tblCustomerGroupService;
            this._tblCardGroupService = _tblCardGroupService;
            this._UserService = _UserService;
            this._tblAccessControllerService = _tblAccessControllerService;
            this._tblSystemConfigService = _tblSystemConfigService;
            this._tblCustomerService = _tblCustomerService;
        }

        [CheckSessionLogin]
        public ActionResult Index(string key, string cardgroup, string customergroup, string actionvs, string user,string eventtype,string eventstatus, string fromdate, string todate, string chkExport = "0", int page = 1)
        {
            var strCG = new List<string>();
            GetListChild(strCG, customergroup);
            var totalItem = 0;
            var datefrompicker = "";

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            int pageSize = 20;

            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                ////Query lấy dữ liệu
                var listExcel = _tblAccessUploadProcessService.GetReportUploadProcessDetailExcel(key, strCG, fromdate, todate, cardgroup, actionvs, user, eventtype, eventstatus);

                if(listExcel != null && listExcel.Rows.Count > 0)
                {
                    foreach(DataRow item in listExcel.Rows)
                    {
                        if (!string.IsNullOrWhiteSpace(item["CustomerGroupID"].ToString()))
                        {
                            var a = _tblCustomerGroupService.GetById(Guid.Parse(item["CustomerGroupID"].ToString()));

                            item["Nhóm KH"] = a != null ? a.CustomerGroupName : "";
                           
                        }
                        if (!string.IsNullOrWhiteSpace(item["Thiết bị"].ToString()))
                        {
                            var b = _tblAccessControllerService.GetById(Guid.Parse(item["Thiết bị"].ToString()));

                            item["Thiết bị"] = b != null ? b.ControllerName : "";

                        }

                        if (!string.IsNullOrWhiteSpace(item["Hành vi"].ToString()))
                        {
                            var b = FunctionHelper.ActionUploadProcess().FirstOrDefault(n => n.ItemValue.Equals(item["Hành vi"].ToString()));

                            item["Hành vi"] = b != null ? b.ItemText : "";

                        }

                    }
                    listExcel.Columns.Remove("CustomerGroupID");
                }


                //Xuất file theo format
                FormatCell(listExcel, "Báo_cáo_chi_tiết_nạp_hủy_thẻ_vân_tay", "Sheet1", "", "Báo cáo chi tiết nạp / hủy thẻ, vân tay", datefrompicker);

                return RedirectToAction("Index");
            }
            #endregion

            var list = _tblAccessUploadProcessService.GetReportUploadProcessDetail(key, strCG, fromdate, todate, cardgroup,actionvs, user,page, pageSize, ref totalItem, eventtype, eventstatus);

            var gridModel = PageModelCustom<ReporttblAccessUploadProcess>.GetPage(list, page, pageSize, totalItem);


            ViewBag.CardGroups = GetCardGroupList().ToList();
            ViewBag.CardGroupDT = GetCardGroupList().ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;


            ViewBag.Users = GetUserList().ToList();
            ViewBag.UserDT = GetUserList().ToDataTableNullable();
            ViewBag.UserId = user;

            ViewBag.LstCustomerGroups = GetCustomerGroupList().ToList();
            ViewBag.CustomerGroups = GetMenuList();
            ViewBag.CustomerGroupId = customergroup;

            ViewBag.Control = _tblAccessControllerService.GetAllActive().ToList();

            ViewBag.Actions = FunctionHelper.ActionUploadProcess();
            ViewBag.ActionId = actionvs;

            ViewBag.EventTypes = FunctionHelper.EventType();
            ViewBag.EventTypeId = eventtype;

            ViewBag.EventStatus = FunctionHelper.EventStatus();
            ViewBag.EventStatusId = eventstatus;

            ViewBag.KeyWord = key;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");

            return View(gridModel);
        }

        #region Format cell lên excel
        private void FormatCell(DataTable dtData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = "Từ " + titleTime.Split(new[] { '-' })[0] + " đến " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = "STT", ItemValue = "STT" });
            listColumn.Add(new SelectListModel { ItemText = "Thời gian", ItemValue = "Thời gian" });
            listColumn.Add(new SelectListModel { ItemText = "Mã thẻ", ItemValue = "Mã thẻ" });
            listColumn.Add(new SelectListModel { ItemText = "User trên tb", ItemValue = "User trên tb" });        
            listColumn.Add(new SelectListModel { ItemText = "Nhóm thẻ", ItemValue = "Nhóm thẻ" });
            listColumn.Add(new SelectListModel { ItemText = "Hành vi", ItemValue = "Hành vi" });
            listColumn.Add(new SelectListModel { ItemText = "Chủ thẻ", ItemValue = "Chủ thẻ" });        
            listColumn.Add(new SelectListModel { ItemText = "Nhóm KH", ItemValue = "Nhóm KH" });
            listColumn.Add(new SelectListModel { ItemText = "Địa chỉ", ItemValue = "Địa chỉ" });
            listColumn.Add(new SelectListModel { ItemText = "NV thực hiện", ItemValue = "NV thực hiện" });
            listColumn.Add(new SelectListModel { ItemText = "Thiết bị", ItemValue = "Thiết bị" });
            listColumn.Add(new SelectListModel { ItemText = "EventType", ItemValue = "EventType" });
            listColumn.Add(new SelectListModel { ItemText = "Hết hạn", ItemValue = "Hết hạn" });

            //Chuyển dữ liệu về datatable
            // DataTable dt = listData.ToDataTableNullable();

            //Xuất file
            ExportFile(dtData, listColumn, dtHeader, filename, sheetname, comments);
        }
        #endregion

        #region Excel
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


        #endregion

        private string GetListChild(string str, string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    str += id + ",";
                }

                var list = _tblCustomerGroupService.GetAllChildActiveByParentID(id).ToList();
                if (list.Any())
                {
                    foreach (var item in list)
                    {
                        str += item.CustomerGroupID.ToString() + ",";
                        GetListChild(str, item.CustomerGroupID.ToString());
                    }
                }
            }

            return str;
        }

        private IEnumerable<tblCardGroup> GetCardGroupList()
        {
            return _tblCardGroupService.GetAllActive();
        }

        private IEnumerable<User> GetUserList()
        {
            return _UserService.GetAllActive();
        }

        private IEnumerable<tblCustomerGroup> GetCustomerGroupList()
        {
            return _tblCustomerGroupService.GetAllActive();
        }


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
            var list = new List<SelectListModel>();
            //{
            //    new SelectListModel {  ItemValue = "", ItemText = "- Chọn danh mục -" }
            //};
            var MenuList = _tblCustomerGroupService.GetAllActive().ToList();
            var parent = MenuList.Where(c => c.ParentID == "0" || c.ParentID == "");
            if (parent.Any())
            {
                foreach (var item in parent.OrderBy(c => c.SortOrder))
                {
                    //Nếu có thì duyệt tiếp để lưu vào list
                    list.Add(new SelectListModel { ItemValue = item.CustomerGroupID.ToString(), ItemText = item.CustomerGroupName });
                    //Gọi action để lấy danh sách submenu theo id
                    var submenu = Children(item.CustomerGroupID.ToString());
                    //Kiểm tra có submenu không
                    if (submenu.Count > 0)
                    {
                        //Nếu có thì duyệt tiếp để lưu vào list
                        foreach (var item1 in submenu)
                        {
                            list.Add(new SelectListModel { ItemValue = item1.ItemValue.ToString(), ItemText = item.CustomerGroupName + " / " + item1.ItemText });
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
            var menu = _tblCustomerGroupService.GetAllChildByParentID(parentID.ToString()).ToList();
            //Kiểm tra có dữ liệu chưa
            if (menu.Any())
            {
                foreach (var item in menu)
                {
                    //Nếu có thì duyệt tiếp để lưu vào list
                    lst.Add(new SelectListModel { ItemValue = item.CustomerGroupID.ToString(), ItemText = item.CustomerGroupName });
                    //Gọi action để lấy danh sách submenu theo id
                    var submenu = Children(item.CustomerGroupID.ToString());
                    //Kiểm tra có submenu không
                    if (submenu.Count > 0)
                    {
                        foreach (var item1 in submenu)
                        {
                            //Nếu có thì duyệt tiếp để lưu vào list
                            lst.Add(new SelectListModel { ItemValue = item1.ItemValue, ItemText = item.CustomerGroupName + " / " + item1.ItemText });
                        }
                    }
                }
            }
            return lst;
        }

        private void GetListChild(List<string> str, string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                // if (str.Any())
                //  {
                str.Add(id);
                // }

                var list = _tblCustomerGroupService.GetAllChildActiveByParentID(id).ToList();
                if (list.Any())
                {
                    foreach (var item in list)
                    {
                        str.Add(item.CustomerGroupID.ToString());
                        GetListChild(str, item.CustomerGroupID.ToString());
                    }
                }
            }
        }

        public ActionResult PartialCustomer(string id)
        {
            var customer = new tblCustomer();

            if (!string.IsNullOrEmpty(id))
            {
                customer = _tblCustomerService.GetByFingerID(Convert.ToInt32(id));
            }

            return PartialView(customer);
        }
    }
}