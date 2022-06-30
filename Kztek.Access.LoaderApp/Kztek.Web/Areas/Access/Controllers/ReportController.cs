using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Service;
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
using System.Windows.Forms;

namespace Kztek.Web.Areas.Access.Controllers
{
    public class ReportController : Controller
    {
        #region Service
        private IReportService _ReportService;
        private ItblCardGroupService _tblCardGroupService;
        private ItblSystemConfigService _tblSystemConfigService;
        private ItblCustomerGroupService _tblCustomerGroupService;
        private ItblCustomerService _tblCustomerService;
        private ItblAccessControllerService _tblAccessControllerService;
        private ItblAccessDoorService _tblAccessDoorService;
        private ItblCardService _tblCardService;

        public ReportController(IReportService _ReportService, ItblCardGroupService _tblCardGroupService, ItblSystemConfigService _tblSystemConfigService, ItblCustomerGroupService _tblCustomerGroupService, ItblCustomerService _tblCustomerService, ItblAccessControllerService _tblAccessControllerService, ItblAccessDoorService _tblAccessDoorService, ItblCardService _tblCardService)
        {
            this._ReportService = _ReportService;
            this._tblCardGroupService = _tblCardGroupService;
            this._tblSystemConfigService = _tblSystemConfigService;
            this._tblCustomerGroupService = _tblCustomerGroupService;
            this._tblCustomerService = _tblCustomerService;
            this._tblAccessControllerService = _tblAccessControllerService;
            this._tblAccessDoorService = _tblAccessDoorService;
            this._tblCardService = _tblCardService;
        }

        #endregion

        #region sự kiện quẹt thẻ

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportEvent(string KeyWord = "",string status = "",string controllerid = "", string cardgroup = "", string fromdate = "", string todate = "", int page = 1, string chkExport = "0", string datefrompicker = "" , string typeEvent ="")
        {
            var totalItem = 0;
            var pageSize = 20;
            var user = GetCurrentUser.GetUser();
          
            var CardNumbers = "";
            if (!string.IsNullOrWhiteSpace(KeyWord))
            {
                var obj = _tblCustomerService.GetByCode(KeyWord);
                if (obj != null)
                {
                    
                    var objCard = _tblCardService.GetByCustomerId(obj.CustomerID.ToString());
                    foreach (var item in objCard)
                    {
                        CardNumbers += item.CardNumber + ",";
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }
          
            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                ////Query lấy dữ liệu
                var listExcel = _ReportService.GetReportEventExcel(KeyWord, status, controllerid, fromdate, todate, cardgroup ,typeEvent , CardNumbers);

                if(listExcel != null && listExcel.Rows.Count > 0)
                {
                    foreach(DataRow dr in listExcel.Rows)
                    {
                        var customer = new tblCustomer();
                        var cg = new tblCardGroup();
                        if (!string.IsNullOrEmpty(dr["CardNumber"].ToString()))
                        {
                            var card = _tblCardService.GetByCardNumber(dr["CardNumber"].ToString());

                            if (card != null)
                            {
                                if (!string.IsNullOrEmpty(card.CustomerID))
                                {
                                    customer = _tblCustomerService.GetById(Guid.Parse(card.CustomerID));
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(dr["CardGroupID"].ToString()))
                        {
                            cg = _tblCardGroupService.GetById(Guid.Parse(dr["CardGroupID"].ToString()));
                        }

                        if (!string.IsNullOrEmpty(dr["ControllerID"].ToString()))
                        {
                            var controller = _tblAccessControllerService.GetById(Guid.Parse(dr["ControllerID"].ToString()));
                            var door = _tblAccessDoorService.GetByController_Readerindex(dr["ControllerID"].ToString(), dr["ReaderIndex"].ToString());

                            dr["ControllerName"] = controller != null ? controller.ControllerName : "";
                            dr["DoorName"] = door != null ? door.DoorName : "";
                        }
                        dr["CustomerCode"] = customer != null ? customer.CustomerCode : "";
                        dr["CustomerName"] = customer != null ? customer.CustomerName : "";
                        dr["CardGroupName"] = cg != null ? cg.CardGroupName : "";
                        dr["Address"] = customer != null ?  customer.Address : "";
                    }
                }

                listExcel.Columns.Remove("RowNumber");
                listExcel.Columns.Remove("ControllerID");
                listExcel.Columns.Remove("ReaderIndex");
                listExcel.Columns.Remove("CardGroupID");

                //Xuất file theo format
                ReportEventFormatCell(listExcel, "Sự_kiện_quẹt_thẻ", "Sheet1", "", "Sự kiện quẹt thẻ", datefrompicker);

                return RedirectToAction("ReportEvent");
            }
            #endregion

            #region Giao diện
            var list = _ReportService.GetReportEvent(KeyWord, status, controllerid,fromdate,todate,page,pageSize,ref totalItem,cardgroup , typeEvent , CardNumbers);

            var gridModel = PageModelCustom<ReportEvent_Access>.GetPage(list, page, pageSize, totalItem);

            ViewBag.CardGroups = GetCardGroupListMonth().ToList();
            ViewBag.CardGroupDT = GetCardGroupListMonth().ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.Controllers = GetControllerList().ToList();
            ViewBag.ControllerDT = GetControllerList().ToDataTableNullable();
            ViewBag.ControllerID = controllerid;

            ViewBag.Status= status;
            ViewBag.StatusDDL = FunctionHelper.StatusEventAccess();


            ViewBag.TypeEv = typeEvent;
            ViewBag.TypeEvCheck = FunctionHelper.CheckTypeEvent();

            ViewBag.Doors = GetDoor();

            ViewBag.DateFromPickerValue = datefrompicker;
            ViewBag.KeyWord = KeyWord;
            
            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");

            return View(gridModel);
            #endregion
        }

        private void ReportEventFormatCell(DataTable dtData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = "Từ " + titleTime.Split(new[] { '-' })[0] + " đến " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = "Thời gian", ItemValue = "Date" });
            listColumn.Add(new SelectListModel { ItemText = "CardNo", ItemValue = "CardNo" });
            listColumn.Add(new SelectListModel { ItemText = "Mã thẻ", ItemValue = "CardNumber" });
            listColumn.Add(new SelectListModel { ItemText = "Nhóm thẻ", ItemValue = "CardGroupName" });
            listColumn.Add(new SelectListModel { ItemText = "Bộ điều khiển", ItemValue = "ControllerName" });
            listColumn.Add(new SelectListModel { ItemText = "Tên cửa", ItemValue = "DoorName" });
            listColumn.Add(new SelectListModel { ItemText = "Mã KH", ItemValue = "CustomerCode" });
            listColumn.Add(new SelectListModel { ItemText = "Chủ thẻ", ItemValue = "CustomerName" });
            listColumn.Add(new SelectListModel { ItemText = "Địa chỉ", ItemValue = "Address" });
            listColumn.Add(new SelectListModel { ItemText = "Trạng thái", ItemValue = "EventStatus" });
            listColumn.Add(new SelectListModel { ItemText = "Sự kiện", ItemValue = "EventType" });
            //Chuyển dữ liệu về datatable
            // DataTable dt = listData.ToDataTableNullable();

            //Xuất file
            ExportFile(dtData, listColumn, dtHeader, filename, sheetname, comments);
        }

        public ActionResult PartialEvent(string cardnumber,string type)
        {
            var customer = new tblCustomer();
            if (!string.IsNullOrEmpty(cardnumber))
            {
                var card = _tblCardService.GetByCardNumber(cardnumber);

                if(card != null)
                {
                    if (!string.IsNullOrEmpty(card.CustomerID))
                    {
                        customer = _tblCustomerService.GetById(Guid.Parse(card.CustomerID));
                    }
                   
                }
            }

            ViewBag.Type = type;
            return PartialView(customer);
        }
        #endregion

        #region sự kiện quẹt thẻ BAOVIET

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult BAOVIET_ReportEvent(string KeyWord = "", string status = "", string controllerid = "", string cardgroup = "", string fromdate = "", string todate = "", int page = 1, string chkExport = "0", string datefrompicker = "")
        {
            var totalItem = 0;
            var pageSize = 20;
            var user = GetCurrentUser.GetUser();
            string CustomerID = "";


            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }

            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                ////Query lấy dữ liệu
                var listExcel = _ReportService.GetReportEventExcel(KeyWord, status, controllerid, fromdate, todate, cardgroup);

                if (listExcel != null && listExcel.Rows.Count > 0)
                {
                    foreach (DataRow dr in listExcel.Rows)
                    {
                        var customer = new tblCustomer();
                        var cg = new tblCardGroup();

                        if (!string.IsNullOrEmpty(dr["CardNumber"].ToString()))
                        {
                            var card = _tblCardService.GetByCardNumber(dr["CardNumber"].ToString());

                            if (card != null)
                            {
                                if (!string.IsNullOrEmpty(card.CustomerID))
                                {
                                    customer = _tblCustomerService.GetById(Guid.Parse(card.CustomerID));
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(dr["CardGroupID"].ToString()))
                        {
                            cg = _tblCardGroupService.GetById(Guid.Parse(dr["CardGroupID"].ToString()));
                        }

                        if (!string.IsNullOrEmpty(dr["ControllerID"].ToString()))
                        {
                            var controller = _tblAccessControllerService.GetById(Guid.Parse(dr["ControllerID"].ToString()));
                            var door = _tblAccessDoorService.GetByController_Readerindex(dr["ControllerID"].ToString(), dr["ReaderIndex"].ToString());

                            dr["ControllerName"] = controller != null ? controller.ControllerName : "";
                            dr["DoorName"] = door != null ? door.DoorName : "";
                        }

                        dr["CustomerName"] = customer != null ? customer.CustomerName : "";
                        dr["CardGroupName"] = cg != null ? cg.CardGroupName : "";
                        dr["Address"] = customer != null ? customer.Address : "";
                    }
                }

                listExcel.Columns.Remove("RowNumber");
                listExcel.Columns.Remove("ControllerID");
                listExcel.Columns.Remove("ReaderIndex");
                listExcel.Columns.Remove("CardGroupID");

               
                //Xuất file theo format
                BV_ReportEventFormatCell(listExcel, "BÁO_CÁO_SỰ_KIỆN_VÀO_RA", "Sheet1", "", "BÁO CÁO SỰ KIỆN VÀO RA", datefrompicker);

                return RedirectToAction("BV_ReportEvent");
            }
            #endregion

            #region Giao diện
            var list = _ReportService.GetReportEvent(KeyWord, status, controllerid, fromdate, todate, page, pageSize, ref totalItem, cardgroup);

            var gridModel = PageModelCustom<ReportEvent_Access>.GetPage(list, page, pageSize, totalItem);

            ViewBag.CardGroups = GetCardGroupListMonth().ToList();
            ViewBag.CardGroupDT = GetCardGroupListMonth().ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;

            ViewBag.Controllers = GetControllerList().ToList();
            ViewBag.ControllerDT = GetControllerList().ToDataTableNullable();
            ViewBag.ControllerID = controllerid;

            ViewBag.Status = status;
            ViewBag.StatusDDL = FunctionHelper.StatusEventAccess();
            ViewBag.Doors = GetDoor();

            ViewBag.DateFromPickerValue = datefrompicker;
            ViewBag.KeyWord = KeyWord;

            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = !string.IsNullOrWhiteSpace(todate) ? Convert.ToDateTime(todate).ToString("dd/MM/yyyy HH:mm:59") : DateTime.Now.ToString("dd/MM/yyyy 23:59:59");

            return View(gridModel);
            #endregion
        }

        private void BV_ReportEventFormatCell(DataTable dtData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = "Từ " + titleTime.Split(new[] { '-' })[0] + " đến " + titleTime.Split(new[] { '-' })[1];

            var dtHeader = _tblSystemConfigService.getHeaderExcel_BAOVIET(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = "Thời gian", ItemValue = "Date" });
            listColumn.Add(new SelectListModel { ItemText = "Số thẻ", ItemValue = "CardNo" });
            listColumn.Add(new SelectListModel { ItemText = "Mã thẻ", ItemValue = "CardNumber" });
            listColumn.Add(new SelectListModel { ItemText = "Nhóm thẻ", ItemValue = "CardGroupName" });
            //listColumn.Add(new SelectListModel { ItemText = "Bộ điều khiển", ItemValue = "ControllerName" });
            listColumn.Add(new SelectListModel { ItemText = "Tên cửa", ItemValue = "DoorName" });
            listColumn.Add(new SelectListModel { ItemText = "Tên nhân viên", ItemValue = "CustomerName" });
            listColumn.Add(new SelectListModel { ItemText = "Phòng ban", ItemValue = "Address" });
            listColumn.Add(new SelectListModel { ItemText = "Trạng thái", ItemValue = "EventStatus" });

            //Chuyển dữ liệu về datatable
            // DataTable dt = listData.ToDataTableNullable();
            dtData.Columns.Remove("ControllerName");

            //Xuất file
            BAOVIET_ExportFile(dtData, listColumn, dtHeader, filename, sheetname, comments);
        }

        #endregion

        #region Danh sách thẻ hết hạn

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult ReportCardExpire(string KeyWord = "", string cardgroup = "", string customergroup = "", int page = 1, string date = "", string chkExport = "0", bool IsAlmostExpired = true)
        {
            var totalItem = 0;
            var pageSize = 20;
            var user = GetCurrentUser.GetUser();
            string CustomerID = "";


            var strCG = new List<string>();
            GetListChild(strCG, customergroup);

            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                //Query lấy dữ liệu
                var listExcel = _ReportService.GetReportCardExpireExcel_Access(KeyWord, cardgroup, CustomerID, strCG, user.Id, date, IsAlmostExpired);

                if(listExcel != null && listExcel.Rows.Count > 0)
                {
                    foreach(DataRow dr in listExcel.Rows)
                    {
                        if(dr["IsLock"].ToString().Contains("True"))
                        {
                            dr["Trạng thái"] = "Đã khóa thẻ";                          
                        }
                        else
                        {
                            dr["Trạng thái"] = "Hoạt động";
                        }
                    }
                }
                listExcel.Columns.Remove("IsLock");
                //Xuất file theo format
                ReportCardExpireFormatCell(listExcel, "Danh_sách_thẻ_hết_hạn", "Sheet1", "", "Danh sách thẻ hết hạn", date);

                return RedirectToAction("ReportCardExpire");
            }
            #endregion

            #region Giao diện
            var list = _ReportService.GetReportCardExpire_Access(KeyWord, cardgroup, CustomerID, strCG, page, pageSize, ref totalItem, user.Id, date, IsAlmostExpired);

            var gridModel = PageModelCustom<ReportCardExpire_Access>.GetPage(list, page, pageSize, totalItem);

            ViewBag.KeyWord = KeyWord;
            ViewBag.IsAlmostExpiredValue = IsAlmostExpired;

            ViewBag.CardGroups = GetCardGroupList().ToList();
            ViewBag.CardGroupDT = GetCardGroupList().Where(n => n.CardType == 0).ToDataTableNullable();
            ViewBag.CardGroupId = cardgroup;


            ViewBag.CustomerGroups = GetMenuList();
            ViewBag.CustomerGroupId = customergroup;

            //ViewBag.CustomerGroups = GetCustomerGroupList().ToList();
            //ViewBag.CustomerGroupDT = GetCustomerGroupList().ToDataTableNullable();
            //ViewBag.CustomerGroupId = customergroup;

            ViewBag.Date = date;

            return View(gridModel);
            #endregion
        }

        private void ReportCardExpireFormatCell(DataTable dtData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            //var timeSearch = "Từ " + titleTime.Split(new[] { '-' })[0] + " đến " + titleTime.Split(new[] { '-' })[1];
            var timeSearch = "Đến " + titleTime;

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = "STT", ItemValue = "STT" });
            listColumn.Add(new SelectListModel { ItemText = "CardNo", ItemValue = "Số thẻ" });
            listColumn.Add(new SelectListModel { ItemText = "Mã thẻ", ItemValue = "Mã thẻ" });
            listColumn.Add(new SelectListModel { ItemText = "Nhóm thẻ", ItemValue = "Nhóm thẻ" });
            listColumn.Add(new SelectListModel { ItemText = "Biển số", ItemValue = "Biển số" });
            listColumn.Add(new SelectListModel { ItemText = "Ngày hết hạn", ItemValue = "Ngày hết hạn" });
            listColumn.Add(new SelectListModel { ItemText = "Khách hàng", ItemValue = "Khách hàng" });
            listColumn.Add(new SelectListModel { ItemText = "Địa chỉ", ItemValue = "Địa chỉ" });
            listColumn.Add(new SelectListModel { ItemText = "Nhóm khách hàng", ItemValue = "Nhóm khách hàng" });
            listColumn.Add(new SelectListModel { ItemText = "Trạng thái", ItemValue = "Trạng thái" });

            //Chuyển dữ liệu về datatable
            // DataTable dt = listData.ToDataTableNullable();

            //Xuất file
            ExportFile(dtData, listColumn, dtHeader, filename, sheetname, comments);
        }
        #endregion

        #region Danh sách khách hàng hết hạn
        public ActionResult ReportCustomerExpire(bool IsAlmostExpired = true,string KeyWord = "", string customergroup = "", int page = 1, string date = "", string chkExport = "0")
        {
            var totalItem = 0;
            var pageSize = 20;
            var user = GetCurrentUser.GetUser();


            var strCG = new List<string>();
            GetListChild(strCG, customergroup);

            #region Excel
            //Excel
            if (chkExport.Equals("1"))
            {
                //Query lấy dữ liệu
                var listExcel = _ReportService.GetReportCustomerExpireExcel_Access(KeyWord, strCG, user.Id, date, IsAlmostExpired);

                listExcel.Columns.Remove("Inactive");
                listExcel.Columns.Remove("CustomerID");

                //Xuất file theo format
                ReportCustomerExpireExpireFormatCell(listExcel, "Danh_sách_khách_hàng_hết_hạn", "Sheet1", "", "Danh sách khách hàng hết hạn", date);

                return RedirectToAction("ReportCustomerExpire");
            }
            #endregion

            #region Giao diện
            var list = _ReportService.GetReportCustomerExpire_Access(KeyWord, strCG, page, pageSize, ref totalItem, user.Id, date, IsAlmostExpired);

            var gridModel = PageModelCustom<ReportCustomerExpire_Access>.GetPage(list, page, pageSize, totalItem);

            ViewBag.KeyWord = KeyWord;
            ViewBag.IsAlmostExpiredValue = IsAlmostExpired;


            ViewBag.CustomerGroups = GetMenuList();
            ViewBag.CustomerGroupId = customergroup;
            ViewBag.Date = date;

            return View(gridModel);
            #endregion
        }

        private void ReportCustomerExpireExpireFormatCell(DataTable dtData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = " Đến " + titleTime;

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);

            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = "STT", ItemValue = "STT" });
            listColumn.Add(new SelectListModel { ItemText = "Mã khách hàng", ItemValue = "Mã khách hàng" });
            listColumn.Add(new SelectListModel { ItemText = "Tên khách hàng", ItemValue = "Tên khách hàng" });
            listColumn.Add(new SelectListModel { ItemText = "Nhóm khách hàng", ItemValue = "Nhóm khách hàng" });
            listColumn.Add(new SelectListModel { ItemText = "Trạng thái", ItemValue = "Trạng thái" });          
            listColumn.Add(new SelectListModel { ItemText = "Số thẻ", ItemValue = "Số thẻ" });
            listColumn.Add(new SelectListModel { ItemText = "Mã thẻ", ItemValue = "Mã thẻ" });
            listColumn.Add(new SelectListModel { ItemText = "Biến số", ItemValue = "Biển số" });
            listColumn.Add(new SelectListModel { ItemText = "Ngày hết hạn", ItemValue = "Ngày hết hạn" });
            listColumn.Add(new SelectListModel { ItemText = "UserIDofFinger", ItemValue = "UserIDofFinger" });

            //Chuyển dữ liệu về datatable
            // DataTable dt = listData.ToDataTableNullable();

            //Xuất file
            ExportFile(dtData, listColumn, dtHeader, filename, sheetname, comments);
        }
        #endregion

        #region Danh sách sử dụng
        private IEnumerable<tblCardGroup> GetCardGroupList()
        {
            return _tblCardGroupService.GetAllActive();
        }
        private IEnumerable<tblCustomerGroup> GetCustomerGroupList()
        {
            return _tblCustomerGroupService.GetAllActive();
        }
        private IEnumerable<tblCardGroup> GetCardGroupListMonth()
        {
            return _tblCardGroupService.GetAllActiveMonth();
        }
        private IEnumerable<tblAccessDoor> GetDoor()
        {
            return _tblAccessDoorService.GetAllActive();
        }
        private IEnumerable<tblAccessController> GetControllerList()
        {
            return _tblAccessControllerService.GetAll();
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

        private void BAOVIET_ExportFile(DataTable list = null, List<SelectListModel> listTitle = null, DataTable dtHeader = null, string filename = "", string sheetname = "", string comments = "")
        {
            var logopath = Server.MapPath("~/uploads/baovietlogo.jpg");
        
            // Gọi lại hàm để tạo file excel
            var stream = FunctionHelper.BAOVIET_WriteToExcel(null, list, listTitle, dtHeader, sheetname, comments, logopath);
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
    }
}