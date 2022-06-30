using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Web.Core.Extensions;
using Kztek.Web.Core.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kztek.Web.Areas.Parking.Controllers
{
    public class OrderActiveCardController : Controller
    {
        private ItblActiveCardService _tblActiveCardService;
        private IOrderActiveCardService _OrderActiveCardService;
        private ItblCustomerGroupService _tblCustomerGroupService;
        private ItblCardGroupService _tblCardGroupService;
        private ItblSystemConfigService _tblSystemConfigService;

        public OrderActiveCardController(IOrderActiveCardService _OrderActiveCardService, ItblCustomerGroupService _tblCustomerGroupService, ItblCardGroupService _tblCardGroupService, ItblActiveCardService _tblActiveCardService, ItblSystemConfigService _tblSystemConfigService)
        {
            this._tblActiveCardService = _tblActiveCardService;
            this._OrderActiveCardService = _OrderActiveCardService;
            this._tblCustomerGroupService = _tblCustomerGroupService;
            this._tblCardGroupService = _tblCardGroupService;
            this._tblSystemConfigService = _tblSystemConfigService;
        }
        // GET: Parking/OrderActiveCard
        public ActionResult Index(string customergroup = "", string key = "", string cardgroup = "", string fromdate = "", string todate = "", int page = 1)
        {
         
            // var totalPage = 0;
            var totalItem = 0;
            var pageSize = 20;
            double totalMoney = 0;
            var datefrompicker = "";

            var customergroups = GetListChild("", customergroup);

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            #region Giao diện

            var systemconfig = _tblSystemConfigService.GetDefault();

            var list = new List<OrderActiveCard>();

            if (systemconfig.FeeName.Contains("AQUA"))
            {
                list = _OrderActiveCardService.GetListOrder_v2(key, fromdate, todate, cardgroup, customergroups, page, pageSize, ref totalItem, ref totalMoney).ToList();
            }
            else
            {
                list = _OrderActiveCardService.GetListOrder(key, fromdate, todate, cardgroup, customergroups, page, pageSize, ref totalItem).ToList();
            }
                    

            var gridModel = PageModelCustom<OrderActiveCard>.GetPage(list, page, pageSize, totalItem);

            ViewBag.keyValue = key;
            ViewBag.cardgroupidsValue = cardgroup;
            ViewBag.customergroupidsValue = customergroup;          

            ViewBag.cardgroups = GetListCardGroup().ToDataTableNullable();       
            ViewBag.customergroups = GetMenuList();


            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            ViewBag.TotalMoney = totalMoney;
            return View(gridModel);
            #endregion
        }

        public ActionResult AQUAIndex(string customergroup = "", string key = "", string cardgroup = "", string fromdate = "", string todate = "", int page = 1)
        {

            // var totalPage = 0;
            var totalItem = 0;
            var pageSize = 20;
            double totalMoney = 0;
            var datefrompicker = "";

            var customergroups = GetListChild("", customergroup);

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            #region Giao diện

            var systemconfig = _tblSystemConfigService.GetDefault();

            var list = new List<OrderActiveCard>();

            if (systemconfig.FeeName.Contains("AQUA"))
            {
                list = _OrderActiveCardService.GetListOrder_v2(key, fromdate, todate, cardgroup, customergroups, page, pageSize, ref totalItem, ref totalMoney).ToList();
            }
            else
            {
                list = _OrderActiveCardService.GetListOrder(key, fromdate, todate, cardgroup, customergroups, page, pageSize, ref totalItem).ToList();
            }


            var gridModel = PageModelCustom<OrderActiveCard>.GetPage(list, page, pageSize, totalItem);

            ViewBag.keyValue = key;
            ViewBag.cardgroupidsValue = cardgroup;
            ViewBag.customergroupidsValue = customergroup;

            ViewBag.cardgroups = GetListCardGroup().ToDataTableNullable();
            ViewBag.customergroups = GetMenuList();


            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            ViewBag.TotalMoney = totalMoney;
            return View(gridModel);
            #endregion
        }

        /// <summary>
        /// dùng cho PRIDE, không hiển thị số tiền
        /// </summary>
        /// <param name="customergroup"></param>
        /// <param name="key"></param>
        /// <param name="cardgroup"></param>
        /// <param name="fromdate"></param>
        /// <param name="todate"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult PRIDEIndex(string customergroup = "", string key = "", string cardgroup = "", string fromdate = "", string todate = "", int page = 1, string chkExport = "0")
        {

            // var totalPage = 0;
            var totalItem = 0;
            var pageSize = 20;
            double totalMoney = 0;
            var datefrompicker = "";

            var customergroups = GetListChild("", customergroup);

            if (string.IsNullOrEmpty(fromdate))
            {
                fromdate = DateTime.Now.ToString("dd/MM/yyyy 00:00:00");
            }

            if (string.IsNullOrEmpty(todate))
            {
                todate = DateTime.Now.ToString("dd/MM/yyyy 23:59:59");
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            if (chkExport == "1")
            {
                var listExcel = _OrderActiveCardService.GetListOrderExcel_PRIDE(key, fromdate, todate, cardgroup, customergroups, ref totalItem);

                //Xuất file theo format
                FormatCell(listExcel, "Danh_sách_hóa_đơn", "Sheet1", "","Danh sách hóa đơn", fromdate + " - " + todate);
                return RedirectToAction("PRIDEIndex");
            }

            #region Giao diện

            var systemconfig = _tblSystemConfigService.GetDefault();

            var list = new List<OrderActiveCard_Custom>();

            list = _OrderActiveCardService.GetListOrder_PRIDE(key, fromdate, todate, cardgroup, customergroups, page, pageSize, ref totalItem,ref totalMoney).ToList();


            var gridModel = PageModelCustom<OrderActiveCard_Custom>.GetPage(list, page, pageSize, totalItem);

            ViewBag.keyValue = key;
            ViewBag.cardgroupidsValue = cardgroup;
            ViewBag.customergroupidsValue = customergroup;

            ViewBag.cardgroups = GetListCardGroup().ToDataTableNullable();
            ViewBag.customergroups = GetMenuList();


            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");

            ViewBag.TotalMoney = totalMoney;

            return View(gridModel);
            #endregion
        }

        private void FormatCell(DataTable dtData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("report", "ReportDetailMoneyCardDay");
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = DictionarySearch["fromDate"] + ": " + titleTime.Split(new[] { '-' })[0] + " - " + DictionarySearch["toDate"] + " :" + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = "STT", ItemValue = "STT" });
            listColumn.Add(new SelectListModel { ItemText = "Địa chỉ", ItemValue = "Địa chỉ" });
            listColumn.Add(new SelectListModel { ItemText = "Số thẻ", ItemValue = "Số thẻ" });
            listColumn.Add(new SelectListModel { ItemText = "Biển số", ItemValue = "Biển số" });
            listColumn.Add(new SelectListModel { ItemText = "Ngày tạo", ItemValue = "Ngày tạo" });
            listColumn.Add(new SelectListModel { ItemText = "Tổng tiền", ItemValue = "Tổng tiền" });
          

            //Xuất file

            ExportFile(dtData, listColumn, dtHeader, filename, sheetname, comments);

        }

        public PartialViewResult DetailBox(string Id)
        {
            var list = _tblActiveCardService.GetBill_v2(Id);
            var systemconfig = _tblSystemConfigService.GetDefault();

            ViewBag.ISAQUA = systemconfig != null ? (systemconfig.FeeName.Contains("AQUA") ? true : false) : false;
            ViewBag.Id = Id;
            return PartialView(list);
        }

        public PartialViewResult PRIDEDetailBox(string Id)
        {
            var list = _tblActiveCardService.GetBill_v2(Id);
            var systemconfig = _tblSystemConfigService.GetDefault();
          
            ViewBag.Id = Id;
            return PartialView(list);
        }
        public IEnumerable<tblCardGroup> GetListCardGroup()
        {
            var query = _tblCardGroupService.GetAllActive().Where(n => n.CardType == 0);
            return query;
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