using Kztek.Data.SqlHelper;
using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Service.Admin.Event;
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
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Net;

namespace Kztek.Web.Areas.Parking.Controllers
{
    public class tblCardController : Controller
    {
        private ItblCardService _tblCardService;
        private ItblCardGroupService _tblCardGroupService;
        private ItblCustomerService _tblCustomerService;
        private ItblCustomerGroupService _tblCustomerGroupService;

        private ItblSystemConfigService _tblSystemConfigService;
        private ItblCompartmentService _tblCompartmentService;
        private ItblCardEventService _tblCardEventService;

        public tblCardController(ItblCompartmentService _tblCompartmentService,ItblCardService _tblCardService, ItblCardGroupService _tblCardGroupService, ItblCustomerService _tblCustomerService, ItblCustomerGroupService _tblCustomerGroupService, ItblSystemConfigService _tblSystemConfigService, ItblCardEventService _tblCardEventService)
        {
            this._tblCardService = _tblCardService;
            this._tblCardGroupService = _tblCardGroupService;
            this._tblCustomerService = _tblCustomerService;
            this._tblCustomerGroupService = _tblCustomerGroupService;

            this._tblSystemConfigService = _tblSystemConfigService;
            this._tblCompartmentService = _tblCompartmentService;
            this._tblCardEventService = _tblCardEventService;
        }

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Index(string key, string cardgroups, string customerid, string customergroups, string active, string fromdate, string todate, string isCheckByTime = "0", int page = 1, string chkExport = "0", string selectedId = "", bool desc = true, string columnQuery = "ImportDate", bool chkFindAutoCapture = false)
        {
            var total = 0;
            var systemconfig = _tblSystemConfigService.GetDefault();
            var str = GetListChild("", customergroups);

            if (chkExport == "1")
            {
                if (systemconfig.FeeName.Contains("PRIDE"))
                {
                    var listExcel = _tblCardService.GetExcelCardByFirstParkingTSQL_v2(key, cardgroups, customerid, str, fromdate, todate, desc, columnQuery, isCheckByTime, "", active, chkFindAutoCapture);

                    var Dictionary = FunctionHelper.GetLocalizeDictionary("tblCard", "Index");
                    //Xuất file theo format
                    PK_CardCustomerFormatCell_v2(listExcel, Dictionary["TitleEx"], "Sheet1", "", Dictionary["Title"], fromdate + " - " + todate);
                }
                else
                {
                    var listExcel = _tblCardService.GetExcelCardByFirstParkingTSQL(key, cardgroups, customerid, str, fromdate, todate, desc, columnQuery, isCheckByTime, "", active, chkFindAutoCapture);

                    var Dictionary = FunctionHelper.GetLocalizeDictionary("tblCard", "Index");
                    //Xuất file theo format
                    PK_CardCustomerFormatCell(listExcel, Dictionary["TitleEx"], "Sheet1", "", Dictionary["Title"], fromdate + " - " + todate);
                }
              
                return RedirectToAction("Index");
            }

            int pageSize = 20;

            var list = _tblCardService.GetAllPagingByFirstParkingTSQL(key, cardgroups, customerid, str, fromdate, todate, desc , columnQuery, ref total, isCheckByTime, page, pageSize, "", active, chkFindAutoCapture);

            var gridModel = PageModelCustom<tblCardCustomViewModel>.GetPage(list, page, pageSize, total);

            //
            ViewBag.keyValue = key;
            ViewBag.cardgroupsValue = cardgroups;
            ViewBag.customeridValue = customerid;
            ViewBag.customergroupsValue = customergroups;
            ViewBag.activeValue = active;
            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = todate;
            ViewBag.isCheckByTimeValue = isCheckByTime;
            ViewBag.columnQueryValue = columnQuery;
            ViewBag.DescValue = desc;
            ViewBag.chkFindAutoCaptureValue = chkFindAutoCapture;
            ViewBag.Compartments = _tblCompartmentService.GetAll();
            //
            ViewBag.selectedIdValue = selectedId;
            ViewBag.selectedCardValue = GetSetFromSession(null);
            ViewBag.isDisplays = GetIsDisplay();

          
            //
            ViewBag.lcardgroups = GetListCardGroup().ToDataTableNullable();
            ViewBag.lcustomergroups = GetMenuList();
            ViewBag.lactives = FunctionHelper.CardStatus();

            

            if (systemconfig.FeeName.Contains("TRANSERCO"))
            {
                ViewBag.ListMoney = _tblCardService.GetMoneyByCardNumber();
            }

            ViewBag.ISTRANSERCO = systemconfig != null ? (systemconfig.FeeName.Contains("TRANSERCO") ? true : false) : false;

            ViewBag.ISMANDARIN1 = systemconfig != null ? systemconfig.IsAutoCapture : true;

            ViewBag.ISPRIDE = systemconfig != null ? (systemconfig.FeeName.Contains("PRIDE") ? true : false) : false;

            ViewBag.System = systemconfig;

            return View(gridModel);
        }

        private bool GetIsDisplay()
        {
            var obj = _tblSystemConfigService.GetDefault();
            bool isDisplay = obj.IsDisplay;
            return isDisplay;
        }

        private void PK_CardCustomerFormatCell(List<tbl_CardExcel> listData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("tblCard", "Index");
            var DictionaryAction = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");

            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = "";

            if (!string.IsNullOrWhiteSpace(titleTime))
            {
                timeSearch = DictionaryAction["fromDate"] +": "+ titleTime.Split(new[] { '-' })[0] +" - "+ DictionaryAction["toDate"] + ": "  + titleTime.Split(new[] { '-' })[1];
            }

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);
            var systemconfig = _tblSystemConfigService.GetDefault();
            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = Dictionary["NumberRow"] , ItemValue = "NumberRow" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["CardNo"], ItemValue = "CardNo" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["CardNumber"], ItemValue = "CardNumber" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["CardGroup"], ItemValue = "CardGroupNumber" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["ExpireDate"], ItemValue = "DateExpire" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["Plate"], ItemValue = "Plates" });

            if(systemconfig != null && systemconfig.FeeName.Contains("TRANSERCO"))
            {
                listColumn.Add(new SelectListModel { ItemText = Dictionary["ContractCode"] , ItemValue = "Description" });
            }
           
            listColumn.Add(new SelectListModel { ItemText = Dictionary["VehicleNames"], ItemValue = "VehicleNames" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["CustomerCode"], ItemValue = "CustomerCode" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["CustomerName"], ItemValue = "CustomerName" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["CustomerGroupName"], ItemValue = "CustomerGroupName" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["Compartment"], ItemValue = "CompartmentId" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["IdentityCard"], ItemValue = "CMT" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["phone"], ItemValue = "SĐT" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["add"], ItemValue = "Address" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["Inactive"], ItemValue = "Inactive" });
           
            listColumn.Add(new SelectListModel { ItemText = "Ngày đăng ký", ItemValue = "DateRegister" });

            //Chuyển dữ liệu về datatable
            DataTable dt = listData.ToDataTableNullable();

            if (systemconfig != null && !systemconfig.FeeName.Contains("TRANSERCO"))
            {
                dt.Columns.Remove("Description");
            }
            //Xuất file
            ExportFile(dt, listColumn, dtHeader, filename, sheetname, comments);
        }

        private void PK_CardCustomerFormatCell_v2(List<tblCardExcel_v2> listData, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        {
            var Dictionary = FunctionHelper.GetLocalizeDictionary("tblCard", "Index");
            var DictionaryAction = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");

            //Nội dung đầu trang
            var user = GetCurrentUser.GetUser();

            var timeSearch = "";

            if (!string.IsNullOrWhiteSpace(titleTime))
            {
                timeSearch = DictionaryAction["fromDate"] + ": " + titleTime.Split(new[] { '-' })[0] + " - " + DictionaryAction["toDate"] + ": " + titleTime.Split(new[] { '-' })[1];
            }

            var dtHeader = _tblSystemConfigService.getHeaderExcel(titleSheet, timeSearch, user.Username);
            var systemconfig = _tblSystemConfigService.GetDefault();
            //Header
            var listColumn = new List<SelectListModel>();
            listColumn.Add(new SelectListModel { ItemText = Dictionary["NumberRow"], ItemValue = "NumberRow" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["CardNo"], ItemValue = "CardNo" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["CardNumber"], ItemValue = "CardNumber" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["CardGroup"], ItemValue = "CardGroupNumber" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["ExpireDate"], ItemValue = "DateExpire" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["Plate"], ItemValue = "Plates" });

            if (systemconfig != null && systemconfig.FeeName.Contains("TRANSERCO"))
            {
                listColumn.Add(new SelectListModel { ItemText = Dictionary["ContractCode"], ItemValue = "Description" });
            }

            listColumn.Add(new SelectListModel { ItemText = Dictionary["VehicleNames"], ItemValue = "VehicleNames" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["CustomerCode"], ItemValue = "CustomerCode" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["CustomerName"], ItemValue = "CustomerName" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["CustomerGroupName"], ItemValue = "CustomerGroupName" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["Compartment"], ItemValue = "CompartmentId" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["IdentityCard"], ItemValue = "CMT" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["phone"], ItemValue = "SĐT" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["add"], ItemValue = "Address" });
            listColumn.Add(new SelectListModel { ItemText = Dictionary["Inactive"], ItemValue = "Inactive" });
            if (systemconfig != null && systemconfig.FeeName.Contains("PRIDE"))
            {
                listColumn.Add(new SelectListModel { ItemText = "Lý do", ItemValue = "DescriptionCard" });
            }
            listColumn.Add(new SelectListModel { ItemText = Dictionary["DateCreatedCard"], ItemValue = "DateCreatedCard" });

            //Chuyển dữ liệu về datatable
            DataTable dt = listData.ToDataTableNullable();

            if (systemconfig != null && !systemconfig.FeeName.Contains("TRANSERCO"))
            {
                dt.Columns.Remove("Description");
            }
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

        [CheckSessionLogin]
        [CheckAuthorize]
        [HttpGet]
        public ActionResult Create(string key, string cardgroups, string customergroups, string active, string fromdate, string todate, string isCheckByTime = "0")
        {
            //
            ViewBag.lcardgroups = GetListCardGroup();
            ViewBag.lcustomergroups = GetMenuList();

            //
            ViewBag.keyValue = key;
            ViewBag.cardgroupsValue = cardgroups;
            ViewBag.customergroupsValue = customergroups;
            ViewBag.activeValue = active;
            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = todate;
            ViewBag.isCheckByTimeValue = isCheckByTime;
            ViewBag.Compartments = GetCompartments();
            var systemconfig = _tblSystemConfigService.GetDefault();
            ViewBag.ISTRANSERCO = systemconfig != null ? (systemconfig.FeeName.Contains("TRANSERCO") ? true : false) : false;
            ViewBag.ISMANDARIN1 = systemconfig != null ? systemconfig.IsAutoCapture : true;
            ViewBag.IsCompartment = systemconfig != null ? systemconfig.isCompartment : true;
            return View();
        }


        public List<SelectListModel> GetCompartments()  //bind ServicePackageId to dropdownlist
        {
            var list = new List<SelectListModel>();
            var listResidentGroup = _tblCompartmentService.GetAll().ToList();
            if (listResidentGroup.Any())
            {
                foreach (var item in listResidentGroup)
                {
                    list.Add(new SelectListModel { ItemValue = item.CompartmentID.ToString(), ItemText = item.CompartmentName });
                }
            }


            return list;
        }

        [HttpPost]
        public ActionResult Create(tblCardSubmit obj, HttpPostedFileBase FileUpload, string key, string cardgroups, string customergroups, string active, string fromdate, string todate, string isCheckByTime = "0", bool SaveAndCountinue = false)
        {
            var systemconfig = _tblSystemConfigService.GetDefault();
            ViewBag.ISTRANSERCO = systemconfig != null ? (systemconfig.FeeName.Contains("TRANSERCO") ? true : false) : false;
            ViewBag.ISMANDARIN1 = systemconfig != null ? systemconfig.IsAutoCapture : true;
            ViewBag.IsCompartment = systemconfig != null ? systemconfig.isCompartment : true;

            //
            ViewBag.lcardgroups = GetListCardGroup();
            ViewBag.lcustomergroups = GetMenuList();
            ViewBag.Compartments = GetCompartments();
            //
            ViewBag.keyValue = key;
            ViewBag.cardgroupsValue = cardgroups;
            ViewBag.customergroupsValue = customergroups;
            ViewBag.activeValue = active;
            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = todate;
            ViewBag.isCheckByTimeValue = isCheckByTime;

            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            //valid form create card
            if ((String.IsNullOrEmpty( obj.CardNo) || String.IsNullOrWhiteSpace(obj.CardNo))
                || (String.IsNullOrEmpty(obj.CardNumber) || String.IsNullOrWhiteSpace(obj.CardNumber))
                || String.IsNullOrEmpty(obj.CardGroupID))
            {
                var Dictionary = FunctionHelper.GetLocalizeDictionary("Home", "notification");
                if (String.IsNullOrEmpty(obj.CardNo) || String.IsNullOrWhiteSpace(obj.CardNo))
                {
                    ModelState.AddModelError("", Dictionary["Please_enter_the_CardNo"]);
                }
                if (String.IsNullOrEmpty(obj.CardNumber) || String.IsNullOrWhiteSpace(obj.CardNumber))
                {
                    ModelState.AddModelError("", Dictionary["Please_enter_the_Card_Number"]);
                }
                if (String.IsNullOrEmpty(obj.CardGroupID))
                {
                    ModelState.AddModelError("", Dictionary["please_select_card_group"]);
                }
                return View(obj);
            }

            var objcard = _tblCardService.GetByCardNumber(obj.CardNumber.Trim());
            if(objcard != null)
            {
                ModelState.AddModelError("CardNumber", "Mã thẻ đã tồn tại");
                return View(obj);
            }

            //
            var map = new tblCard()
            {
                CardID = Guid.NewGuid(),
                CardNo = obj.CardNo,
                CardNumber = obj.CardNumber.Trim(),
                CardGroupID = obj.CardGroupID,
                CustomerID = GetOrSetCustomer(obj, FileUpload),
                AccessLevelID = "",
                ChkRelease = false,
                ImportDate = DateTime.Now,
                DateRegister = Convert.ToDateTime(obj.DtpDateRegisted),
                DateRelease = Convert.ToDateTime(obj.DtpDateReleased),
                ExpireDate = Convert.ToDateTime(obj.DtpDateExpired),
                DateActive = Convert.ToDateTime(obj.DtpDateActive),
                Description = !string.IsNullOrWhiteSpace(obj.CardDescription) ? obj.CardDescription : "",
                IsDelete = false,
                IsLock = obj.CardInActive,
                Plate1 = obj.Plate1,
                Plate2 = obj.Plate2,
                Plate3 = obj.Plate3,
                VehicleName1 = obj.VehicleName1,
                VehicleName2 = obj.VehicleName2,
                VehicleName3 = obj.VehicleName3,
                DVT = obj.DVT,
                AccessExpireDate = Convert.ToDateTime("2099/12/31"),
                DateCancel = DateTime.Now,

                isAutoCapture = obj.IsAutoCapture
            };

            //Thực hiện thêm mới
            var result = _tblCardService.Create(map);
            if (result.isSuccess)
            {
                //Log for hệ thống

                WriteLog.Write(result, GetCurrentUser.GetUser(), map.CardID.ToString(), obj.CardNumber.Trim(), "tblCard", ConstField.ParkingCode, ActionConfigO.Create);

                //Upload file vào folder chứa ảnh /upload/avatars
                UploadFile(FileUpload);

                //
                SaveCardProcessExpend(map,"","", "ADD", GetCurrentUser.GetUser().Id);

                if (SaveAndCountinue)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction("Create", new { key = key, cardgroups = cardgroups, customergroups = customergroups, fromdate = fromdate, todate = todate, active = active, isCheckByTime = isCheckByTime, selectedId = obj.CardID });
                }

                return RedirectToAction("Index", new { key = key, cardgroups = cardgroups, customergroups = customergroups, fromdate = fromdate, todate = todate, active = active, isCheckByTime = isCheckByTime, selectedId = obj.CardID });
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(obj);
            }
        }

        [CheckSessionLogin]
        [CheckAuthorize]
        [HttpGet]
        public ActionResult Update(string id, string key, string cardgroups, string customergroups, string active, string fromdate, string todate, string isCheckByTime = "0", int page = 1)
        {
            ViewBag.lcardgroups = GetListCardGroup();
            ViewBag.lcustomergroups = GetMenuList();
            var systemconfig = _tblSystemConfigService.GetDefault();
            ViewBag.Compartments = GetCompartments();
            //
            ViewBag.keyValue = key;
            ViewBag.cardgroupsValue = cardgroups;
            ViewBag.customergroupsValue = customergroups;
            ViewBag.activeValue = active;
            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = todate;
            ViewBag.isCheckByTimeValue = isCheckByTime;
         
            ViewBag.PN = page;

            var obj = _tblCardService.GetCustomById(Guid.Parse(id));
            if (obj != null)
            {
                var t = _tblCardService.GetById(Guid.Parse(id));
                if (t != null)
                {
                    obj.DtpDateExpired = Convert.ToDateTime(t.ExpireDate).ToString("dd/MM/yyyy");

                    obj.DtpDateRegisted = Convert.ToDateTime(t.DateRegister != null ? t.DateRegister : DateTime.Now).ToString("dd/MM/yyyy");

                    // dự án hòa phát mandarin garden 15/7/2019 dungdt 
                    //Khi thay đổi thông tin thẻ yêu cầu mặc định ngày phát hành thẻ = ngày cập nhật
                    //obj.DtpDateReleased = Convert.ToDateTime(t.DateRelease != null ? t.DateRelease : DateTime.Now).ToString("dd/MM/yyyy");
                    obj.DtpDateReleased = DateTime.Now.ToString("dd/MM/yyyy");
                    obj.DtpDateActive = Convert.ToDateTime(t.DateActive != null ? t.DateActive : DateTime.Now).ToString("dd/MM/yyyy");

                    obj.OldDtpDateRegisted = Convert.ToDateTime(t.DateRegister != null ? t.DateRegister : DateTime.Now).ToString("dd/MM/yyyy");
                    obj.OldDtpDateReleased = Convert.ToDateTime(t.DateRelease != null ? t.DateRelease : DateTime.Now).ToString("dd/MM/yyyy");
                    obj.OldDtpDateActive = Convert.ToDateTime(t.DateActive != null ? t.DateActive : DateTime.Now).ToString("dd/MM/yyyy");
                }
            }

            ViewBag.IsCompartment = systemconfig != null ? systemconfig.isCompartment : true;
            ViewBag.ISTRANSERCO = systemconfig != null ? (systemconfig.FeeName.Contains("TRANSERCO") ? true : false) : false;
           
            ViewBag.ISMANDARIN1 = systemconfig != null ? systemconfig.IsAutoCapture : true;
            var isViettel = false;
            var isDuyTan = false;
            var isPhoenix = false;
           
            try
            {
                isDuyTan = systemconfig != null ? (systemconfig.FeeName.Contains("DREAMLAND_BONANZA_23DUYTAN") ? true : false) : false;
                isPhoenix = systemconfig != null ? (systemconfig.FeeName.Contains("PHOENIX_BACNINH") ? true : false) : false;
            }
            catch 
            {

                isDuyTan = false;
                isPhoenix = false;
            }

            try
            {
                isViettel = systemconfig != null ? (systemconfig.FeeName.Contains(ConfigurationManager.AppSettings["Viettel_Name"]) ? true : false) : false;
            }
            catch
            {
                isViettel = false;
            }

            ViewBag.ISVIETTEL = isViettel;
            ViewBag.ISDUYTAN = isDuyTan;
            ViewBag.ISPHONIX = isPhoenix;
            return View(obj);
        }
        [HttpPost]
        public ActionResult Update(tblCardSubmit obj, HttpPostedFileBase FileUpload, string key, string cardgroups, string customergroups, string active, string fromdate, string todate, string isCheckByTime = "0", int page = 1)
        {
            var systemconfig = _tblSystemConfigService.GetDefault();
            ViewBag.ISTRANSERCO = systemconfig != null ? (systemconfig.FeeName.Contains("TRANSERCO") ? true : false) : false;
            ViewBag.ISMANDARIN1 = systemconfig != null ? systemconfig.IsAutoCapture : true;
            ViewBag.IsCompartment = systemconfig != null ? systemconfig.isCompartment : true;
            ViewBag.Compartments = GetCompartments();
            var isViettel = false;
            var isDuyTan = false;
            var isPhoenix = false;
         
            try
            {
                isDuyTan = systemconfig != null ? (systemconfig.FeeName.Contains("DREAMLAND_BONANZA_23DUYTAN") ? true : false) : false;
                isPhoenix = systemconfig != null ? (systemconfig.FeeName.Contains("PHOENIX_BACNINH") ? true : false) : false;
            }
            catch 
            {

                isDuyTan = false;
                isPhoenix = false;
            }
            try
            {
                isViettel = systemconfig != null ? (systemconfig.FeeName.Contains(ConfigurationManager.AppSettings["Viettel_Name"]) ? true : false) : false;
            }
            catch
            {
                isViettel = false;
            }

            ViewBag.ISVIETTEL = isViettel;
            ViewBag.ISDUYTAN = isDuyTan;
            ViewBag.ISPHONIX = isPhoenix;
            ViewBag.lcardgroups = GetListCardGroup();
            ViewBag.lcustomergroups = GetMenuList();

            //
            ViewBag.keyValue = key;
            ViewBag.cardgroupsValue = cardgroups;
            ViewBag.customergroupsValue = customergroups;
            ViewBag.activeValue = active;
            ViewBag.fromdateValue = fromdate;
            ViewBag.todateValue = todate;
            ViewBag.isCheckByTimeValue = isCheckByTime;

            ViewBag.PN = page;

            var oldObj = _tblCardService.GetCustomById(Guid.Parse(obj.CardID));

            if (oldObj == null)
            {
                return View(obj);
            }

            //valid form update card
            if ((String.IsNullOrEmpty(obj.CardNo) || String.IsNullOrWhiteSpace(obj.CardNo))
                || String.IsNullOrEmpty(obj.CardGroupID))
            {
                var Dictionary = FunctionHelper.GetLocalizeDictionary("Home", "notification");
                if (String.IsNullOrEmpty(obj.CardNo) || String.IsNullOrWhiteSpace(obj.CardNo))
                {
                    ModelState.AddModelError("", Dictionary["Please_enter_the_CardNo"]);
                }
                if (String.IsNullOrEmpty(obj.CardGroupID))
                {
                    ModelState.AddModelError("", Dictionary["please_select_card_group"]);
                }
                return View(obj);
            }




            //Upload File
            if (FileUpload != null)
            {
                var extension = Path.GetExtension(FileUpload.FileName) ?? "";
                var fileName = Path.GetFileName(string.Format("{0}{1}", StringUtil.RemoveSpecialCharactersVn(FileUpload.FileName.Replace(extension, "")).GetNormalizeString(), extension));

                var url = ConfigurationManager.AppSettings["FileUploadAvatar"];
                oldObj.CustomerAvatar = string.Format("{0}{1}", url, fileName);
            }

            //Gán giá trị
            var result = new MessageReport();

            var map = _tblCardService.GetById(Guid.Parse(obj.CardID));
            if (map != null)
            {
                if (isViettel)
                {
                    var existed = _tblCardService.GetByCardNumber_Id(obj.CardNumber, Guid.Parse(obj.CardID));
                    if (existed != null)
                    {
                        ModelState.AddModelError("CardNumber", "Mã thẻ đã tồn tại");
                        return View(obj);
                    }

                    map.CardNumber = obj.CardNumber;
                }

                //Thẻ
                
                map.CardNo = obj.CardNo;
                map.CardGroupID = obj.CardGroupID;
                map.Description = !string.IsNullOrWhiteSpace(obj.CardDescription) ? obj.CardDescription : "";
                map.IsLock = obj.CardInActive;
                map.Plate1 = obj.Plate1;
                map.Plate2 = obj.Plate2;
                map.Plate3 = obj.Plate3;
                map.VehicleName1 = obj.VehicleName1;
                map.VehicleName2 = obj.VehicleName2;
                map.VehicleName3 = obj.VehicleName3;
                map.isAutoCapture = obj.IsAutoCapture;
                map.DVT = obj.DVT;
                //Khách hàng
                obj.CustomerAvatar = oldObj.CustomerAvatar;
                map.CustomerID = GetOrSetCustomer(obj, FileUpload);

                //Ngày giờ
                map.DateRegister = Convert.ToDateTime(obj.DtpDateRegisted);
                map.DateRelease = Convert.ToDateTime(obj.DtpDateReleased);
                map.DateActive = Convert.ToDateTime(obj.DtpDateActive);
                result = _tblCardService.Update(map);
            }

            if (result.isSuccess)
            {
                var grCardOld = new tblCardGroup();
                var grCardNew = new tblCardGroup();
                var s = oldObj.CardGroupID;
                if (!oldObj.CardGroupID.Equals(""))
                {
                    Guid ids = Guid.Parse(oldObj.CardGroupID);
                     grCardOld = _tblCardGroupService.GetById(ids);
                }
                if ( !obj.CardGroupID.Equals(""))
                {
                    Guid ids1 = Guid.Parse(obj.CardGroupID);
                    grCardNew = _tblCardGroupService.GetById(ids1);
                }
               
                var cuuser = GetCurrentUser.GetUser();
                //thông tin cũ
                var oldModel = new CardCustom();
                oldModel.CardNo = oldObj.CardNo;
                oldModel.CardNumber = oldObj.CardNumber;
                oldModel.CustomerName = oldObj.CustomerName;
                oldModel.Plate1 = oldObj.Plate1;
                oldModel.Plate2 = oldObj.Plate2;
                oldModel.Plate3 = oldObj.Plate3;
                oldModel.Mobile = oldObj.CustomerMobile;
                oldModel.CustomerCode = oldObj.CustomerCode;
                oldModel.CardGrname = grCardOld.CardGroupName;
                var jsStrOld = Newtonsoft.Json.JsonConvert.SerializeObject(oldModel);
                //thông tin mới
                var NewModel = new CardCustom();
                NewModel.CardNo = obj.CardNo;
                NewModel.CardNumber = obj.CardNumber;
                NewModel.CustomerName = obj.CustomerName;
                NewModel.Plate1 = obj.Plate1;
                NewModel.Plate2 = obj.Plate2;
                NewModel.Plate3 = obj.Plate3;
                NewModel.Mobile = obj.CustomerMobile;
                NewModel.CustomerCode = obj.CustomerCode;
                NewModel.CardGrname = grCardNew.CardGroupName;
                var jsStrNew = Newtonsoft.Json.JsonConvert.SerializeObject(NewModel);
                //Log for hệ thống
                WriteLog.WriteLogCard(result, cuuser, oldObj.CardID.ToString(), oldObj.CardNumber, jsStrOld, jsStrNew
                 , "tblCard", ConstField.ParkingCode, ActionConfigO.Update);

                WriteLog.WriteLogFile(result, cuuser, oldObj.CardID.ToString(), oldObj.CardNumber + "," + oldObj.CardGroupID + "," + oldObj.CustomerID + "," + oldObj.Plate1 + "," + oldObj.Plate2 + "," + oldObj.Plate3, "tblCard", ConstField.ParkingCode, ActionConfigO.Update);

                //Upload file vào folder chứa ảnh /upload/avatar
                UploadFile(FileUpload);

                //Process
                obj.OldPlate1 = oldObj.Plate1;
                obj.OldPlate2 = oldObj.Plate2;
                obj.OldPlate3 = oldObj.Plate3;
                obj.OldCustomerCode = oldObj.CustomerCode;
                obj.OldCustomerMobile = oldObj.CustomerMobile;
                var list = GetListActionType(obj);
                if (list.Any())
                {
                    foreach (var item in list)
                    {
                        string plate = ((oldObj.Plate1 != null || oldObj.Plate1 != "") ? oldObj.Plate1 : "")  + ((oldObj.Plate2 != null ||  oldObj.Plate2 != ",") ? "," + oldObj.Plate2 : "") ;
                        SaveCardProcessExpend(map, plate, jsStrOld, item, cuuser.Id);
                    }
                }

                //Check Viettel
                if (isViettel)
                {
                    UpdateToViettel(map);
                }

                return RedirectToAction("Index", new { key = key, cardgroups = cardgroups, customergroups = customergroups, fromdate = fromdate, todate = todate, active = active, isCheckByTime = isCheckByTime, selectedId = obj.CardID, page = page });
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(obj);
            }
        }

        public JsonResult Delete(string id)
        {
            var noti = FunctionHelper.GetLocalizeDictionary("Home", "notification");

            var obj = _tblCardService.GetById(Guid.Parse(id));
            if (obj == null)
            {
                var result1 = new MessageReport();
                result1.Message = noti["card_does_not_exist_in_the_system"];
                result1.isSuccess = false;
                return Json(result1, JsonRequestBehavior.AllowGet);
            }

            var existedInEvent = _tblCardEventService.GetAllByCardNumber(obj.CardNumber);
            if (existedInEvent.Any())
            {
                var result1 = new MessageReport();
                result1.Message = noti["card_is_existing_in_the_event"];
                result1.isSuccess = false;
                return Json(result1, JsonRequestBehavior.AllowGet);
            }

            var result = _tblCardService.DeleteById(id);
            if (result.isSuccess)
            {
                var cuuser = GetCurrentUser.GetUser();

                WriteLog.Write(result, cuuser, obj.CardID.ToString(), obj.CardNumber, "tblCard", ConstField.ParkingCode, ActionConfigO.Delete);

                SaveCardProcessExpend(obj,"","", "DELETE", cuuser.Id);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Remove(string id)
        {
            var noti = FunctionHelper.GetLocalizeDictionary("Home", "notification");

            var obj = _tblCardService.GetById(Guid.Parse(id));
            if (obj == null)
            {
                var result1 = new MessageReport();
                result1.Message = noti["card_does_not_exist_in_the_system"];
                result1.isSuccess = false;
                return Json(result1, JsonRequestBehavior.AllowGet);
            }
       
            var result = _tblCardService.DeleteById(id);
            if (result.isSuccess)
            {
                var cuuser = GetCurrentUser.GetUser();

                WriteLog.Write(result, cuuser, obj.CardID.ToString(), obj.CardNumber, "tblCard", ConstField.ParkingCode, ActionConfigO.Delete);

                SaveCardProcessExpend(obj,"","", "DELETE", cuuser.Id);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public IEnumerable<tblCardGroup> GetListCardGroup()
        {
            var query = _tblCardGroupService.GetAllActive();
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
            var DictionaryShare = FunctionHelper.GetLocalizeDictionary("SelectList", "CustomerGroupID");
            var list = new List<SelectListModel>()
            {
                new SelectListModel { ItemValue = "", ItemText =DictionaryShare["SlectlistCusGrp"] }
            };

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
                    str += "'" + id + "'" + ",";
                }

                var list = _tblCustomerGroupService.GetAllChildActiveByParentID(id).ToList();
                if (list.Any())
                {
                    foreach (var item in list)
                    {
                        str += "'" + item.CustomerGroupID.ToString() + "'" + ",";
                        GetListChild(str, item.CustomerGroupID.ToString());
                    }
                }
            }

            return str;
        }

        public JsonResult PreviewImageUpload()
        {
            var str = "";

            for (int i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];
                if (file != null && file.ContentLength > 0)
                {
                    str = FunctionHelper.ConvertImgFileUploadToBase64(file);
                }
            }

            var a = new JsonResult { Data = str, MaxJsonLength = Int32.MaxValue };

            return a;
        }

        public JsonResult GetListCardByKey(string key)
        {
            var listString = new List<string>();

            var list = _tblCardService.GetAllActiveByKey(key);

            foreach (var item in list)
            {
                listString.Add(string.Format("{0}", item.CardNumber));
            }

            return Json(listString.ToArray(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCard(string code)
        {
            var obj = _tblCardService.GetByCardNumber(code);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult GetListCustomerByKey(string key)
        {
            var cus = new List<SelectListModelAutocomplete>();

            var list = _tblCustomerService.GetAllActiveByKeyMaxTake(key, 10);

            var str = "";
            foreach (var item in list)
            {
                str += item.CustomerGroupID + ",";
            }

            var lCuGr = _tblCustomerGroupService.GetAllActiveByListId(str).ToList();

            foreach (var item in list)
            {
                var cuG = lCuGr.FirstOrDefault(n => n.CustomerGroupID.ToString() == item.CustomerGroupID);

                var t = new SelectListModelAutocomplete();

                t.id = item.CustomerCode;
                t.name = string.Format("{0} - {1} - {2} - {3}", item.CustomerName, item.CustomerCode, item.Mobile, cuG != null ? cuG.CustomerGroupName : "");
                t.value = string.Format("{0} - {1} - {2} - {3}", item.CustomerName, item.CustomerCode, item.Mobile, cuG != null ? cuG.CustomerGroupName : "");

                cus.Add(t);
            }
            return Json(cus, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadNoteLock(string key)
        {
            var cus = new List<SelectListModelAutocomplete>();

            var list = FunctionHelper.ListNoteLock();

            if (!string.IsNullOrEmpty(key))
            {
                list = list.Where(n => n.ItemValue.Contains(key.ToLower().Trim()) || n.ItemText.ToLower().Contains(key.ToLower().Trim())).ToList();
            }
           
            foreach (var item in list)
            {              
                var t = new SelectListModelAutocomplete();

                t.id = item.ItemValue;
                t.name = item.ItemText;
                t.value = item.ItemText;

                cus.Add(t);
            }
            return Json(cus, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadNoteUnLock(string key)
        {
            var cus = new List<SelectListModelAutocomplete>();

            var list = FunctionHelper.ListNoteUnLock();

            if (!string.IsNullOrEmpty(key))
            {
                list = list.Where(n => n.ItemValue.Contains(key.ToLower().Trim()) || n.ItemText.ToLower().Contains(key.ToLower().Trim())).ToList();
            }

            foreach (var item in list)
            {
                var t = new SelectListModelAutocomplete();

                t.id = item.ItemValue;
                t.name = item.ItemText;
                t.value = item.ItemText;

                cus.Add(t);
            }
            return Json(cus, JsonRequestBehavior.AllowGet);
        }

        private void UploadFile(HttpPostedFileBase fileUpload)
        {
            if (fileUpload != null)
            {
                string error = "";

                var url = ConfigurationManager.AppSettings["FileUploadAvatar"];

                Common.UploadFile(out error, Server.MapPath(url), fileUpload);
            }
        }

        private string GetOrSetCustomer(tblCardSubmit obj, HttpPostedFileBase FileUpload)
        {
            if (FileUpload != null)
            {
                var extension = Path.GetExtension(FileUpload.FileName) ?? "";
                var fileName = Path.GetFileName(string.Format("{0}{1}", StringUtil.RemoveSpecialCharactersVn(FileUpload.FileName.Replace(extension, "")).GetNormalizeString(), extension));

                var url = ConfigurationManager.AppSettings["FileUploadAvatar"];
                obj.CustomerAvatar = string.Format("{0}{1}", url, fileName);
            }

            var id = "";

            if (!string.IsNullOrWhiteSpace(obj.CustomerID))
            {
                var tCus = _tblCustomerService.GetById(Guid.Parse(obj.CustomerID));
                var CustomerName = tCus.CustomerName;
                var CustomerCode = tCus.CustomerCode;
                if (tCus != null)
                {
                    id = tCus.CustomerID.ToString();

                    tCus.CustomerCode = obj.CustomerCode;
                    tCus.Address = obj.CustomerAddress;
                    tCus.Avatar = obj.CustomerAvatar;
                    tCus.CustomerGroupID = obj.CustomerGroupID;
                    tCus.CustomerName = obj.CustomerName;
                    tCus.Mobile = obj.CustomerMobile;
                    tCus.IDNumber = obj.CustomerIdentify;
                    tCus.CompartmentId = obj.CompartmentId;
                    var result = _tblCustomerService.Update(tCus);

                    //thông tin cũ
                    var oldModel = new CustomerCus();
                    oldModel.CustomerName = CustomerName;
                    oldModel.CustomerCode = CustomerCode;
                    var jsStrOld = Newtonsoft.Json.JsonConvert.SerializeObject(oldModel);
                    //thông tin mới
                    var NewModel = new CustomerCus();
                    NewModel.CustomerName = obj.CustomerName;
                    NewModel.CustomerName = obj.CustomerName;

                    NewModel.CustomerCode = obj.CustomerCode;
                    var jsStrNew = Newtonsoft.Json.JsonConvert.SerializeObject(NewModel);
                    WriteLog.WriteCus(result, GetCurrentUser.GetUser(), tCus.CustomerID.ToString(), tCus.CustomerCode, jsStrOld, jsStrNew, "tblCustomer", ConstField.ParkingCode, ActionConfigO.Update);

                    WriteLog.WriteLogFile(result, GetCurrentUser.GetUser(), tCus.CustomerID.ToString(), tCus.CustomerCode, "tblCustomer", ConstField.ParkingCode, ActionConfigO.Update);

                }
                else
                {
                    tCus = new tblCustomer()
                    {
                        CustomerID = Guid.NewGuid(),
                        Address = obj.CustomerAddress,
                        Avatar = obj.CustomerAvatar,
                        AccessLevelID = "",
                        CompartmentId = obj.CompartmentId,
                        CustomerCode = obj.CustomerCode,
                        CustomerGroupID = obj.CustomerGroupID,
                        CustomerName = obj.CustomerName,
                        Description = obj.CustomerAddress,
                        IDNumber = obj.CustomerIdentify,
                        Inactive = false,
                        Mobile = obj.CustomerMobile,
                        SortOrder = 0,
                        Finger1 = "",
                        Finger2 = "",
                        DevPass = "",
                        AccessExpireDate = Convert.ToDateTime("2099/12/31")
                    };

                    var result = _tblCustomerService.Create(tCus);
                    if (result.isSuccess)
                    {
                        id = tCus.CustomerID.ToString();
                    }
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(obj.CustomerCode))
                {
                    var objC = _tblCustomerService.GetByCode(obj.CustomerCode);

                    if (objC != null)
                    {
                        id = objC.CustomerID.ToString();
                    }
                    else
                    {
                        var tCus = new tblCustomer()
                        {
                            CustomerID = Guid.NewGuid(),
                            Address = obj.CustomerAddress,
                            Avatar = obj.CustomerAvatar,
                            AccessLevelID = "",
                            CompartmentId = obj.CompartmentId,
                            CustomerCode = obj.CustomerCode,
                            CustomerGroupID = obj.CustomerGroupID,
                            CustomerName = obj.CustomerName,
                            Description = obj.CustomerAddress,
                            IDNumber = obj.CustomerIdentify,
                            Inactive = false,
                            Mobile = obj.CustomerMobile,
                            SortOrder = 0,
                            Finger1 = "",
                            Finger2 = "",
                            DevPass = "",
                            AccessExpireDate = Convert.ToDateTime("2099/12/31")
                        };

                        var result = _tblCustomerService.Create(tCus);
                        if (result.isSuccess)
                        {
                            id = tCus.CustomerID.ToString();
                        }
                    }
                }
            }

            return id;
        }

        private List<string> GetListActionType(tblCardSubmit obj)
        {
            //Đổi thẻ
            if (obj.CardNumber != obj.OldCardNumber)
            {
                obj.isChangeCard = true;
            }

            //Khóa thẻ, mở thẻ


            //Phát thẻ
            if (string.IsNullOrWhiteSpace(obj.OldCustomerCode) && !string.IsNullOrWhiteSpace(obj.CustomerCode))
            {
                obj.isReleaseCard = true;
                obj.isChangeCustomer = false;
            }

            //Đổi khách hàng
            if (!string.IsNullOrWhiteSpace(obj.OldCustomerCode) && (!obj.OldCustomerCode.Equals(obj.CustomerCode) || !string.IsNullOrWhiteSpace(obj.CustomerCode)) && (obj.CustomerID != obj.OldCustomerID))
            {
                obj.isChangeCustomer = true;
                obj.isReturnCard = false;
            }

            //Trả thẻ
            if (!string.IsNullOrWhiteSpace(obj.OldCustomerCode) && string.IsNullOrWhiteSpace(obj.CustomerCode))
            {
                obj.isReturnCard = true;
            }

            //if (obj.OldDtpDateActive != obj.DtpDateActive)
            //{
            //    obj.isChangeActiveCard = true;
            //}

            ////
            var str = new List<string>();

            //Xử lý với thẻ
            if (obj.isChangeCard)
            {
                //Cấp mới
                str.Add("ADD");//1
            }
            else
            {
                //if (objMap.isModifiedCard)
                //{
                //    //Sửa thông tin thẻ
                //    str += 8 + ",";
                //}
            }
            if (obj.isReturnCard)
            {
                //Trả thẻ
                str.Add("RETURN");//10
            }

            //Phát thẻ
            if (obj.isReleaseCard)
            {
                str.Add("RELEASE");//11
            }
            //Xử lý với khách hàng
            if (obj.isChangeCustomer)
            {
                //Cấp lại
                str.Add("CHANGE");
            }
            else
            {
                //if (objMap.isModifiedCustomer)
                //{
                //    //Sửa thông tin khách hàng
                //    str += 7 + ",";
                //}
            }

            //Xử lý với thông tin cơ bản
            //if (objMap.isModifiedBaseInfo)
            //{
            //    //Sửa thông tin cơ bản
            //    str += 6 + ",";
            //}

            //Xử lý với ngày gia hạn
            //if (objMap.isModifiedExtendCard)
            //{
            //    //Gia hạn thẻ
            //    str += 3 + ",";
            //}

            //Xử lý với phương tiện xe
            //if (objMap.isModifiedVehicle)
            //{
            //    //Thay đổi thông tin xe
            //    str += 5 + ",";
            //}


            ////Phát thẻ
            //if (obj.isReleaseCard)
            //{
            //    str.Add("RELEASE");//11
            //}
            ////Phát thẻ
            //if (obj.isReleaseCard)
            //{
            //    str.Add("RELEASE");//11
            //}

            //Hoạt động thẻ
            //if (obj.isChangeActiveCard)
            //{
            //    str.Add("ACTIVE");
            //}

            if (obj.OldCardInActive != obj.CardInActive)
            {
                if (obj.CardInActive)
                {
                    str.Add("LOCK");
                }
                else
                {
                    str.Add("UNLOCK");
                }
            }
            //Cập nhật thông tin
            if (obj.OldCustomerName != obj.CustomerName || obj.OldCustomerMobile !=  obj.CustomerMobile || obj.OldCustomerCode != obj.CustomerCode || obj.OldCardGroupID != obj.CardGroupID && (obj.Plate1 == obj.OldPlate1 || obj.Plate2 == obj.OldPlate2 || obj.Plate3 != obj.OldPlate3))
            {
                //Đổi thông tin thẻ
                str.Add("UPDATEINFO");//10
            }
            else if (obj.OldCustomerName != obj.CustomerName || obj.OldCustomerMobile != obj.CustomerMobile || obj.OldCustomerCode != obj.CustomerCode || obj.OldCardGroupID != obj.CardGroupID && (obj.Plate1 != obj.OldPlate1 || obj.Plate2 != obj.OldPlate2 || obj.Plate3 != obj.OldPlate3))
            {
                //Đổi thông tin thẻ
                str.Add("UPDATEINFO");//10
            }
            else if (obj.Plate1 != obj.OldPlate1 || obj.Plate2 != obj.OldPlate2 || obj.Plate3 != obj.OldPlate3 && (obj.OldCustomerName == obj.CustomerName || obj.OldCustomerMobile == obj.CustomerMobile || obj.OldCustomerCode == obj.CustomerCode || obj.OldCardGroupID == obj.CardGroupID))
            {
                //Đổi biển số
                str.Add("CHANGEPLA");//10
            }
           
         
           
            return str;
        }
        //Luu log thêm biển số và thông tin cũ////////
        private void SaveCardProcessExpend(tblCard obj,string plate, string  jsonobj, string action, string userid)
        {
            var str = string.Format("insert into tblCardProcess(Date, CardNumber, Actions, CardGroupID, UserID, CustomerID,Plates,OldInfoCP) values ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}','{6}',N'{7}')", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), obj.CardNumber, action, obj.CardGroupID, userid, obj.CustomerID,plate, jsonobj);

            SqlExQuery<tblCardProcess>.ExcuteNone(str);
        }

        private void SaveCardProcess(tblCardSubmit obj, string action, string userid)
        {
            var str = string.Format("insert into tblCardProcess(Date, CardNumber, Actions, CardGroupID, UserID, CustomerID) values ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), obj.CardNumber, action, obj.CardGroupID, userid, obj.CustomerID);

            SqlExQuery<tblCardProcess>.ExcuteNone(str);
        }

        private void SaveCardExtendProcess(tblCardSubmit obj, string _newexpire, string userid)
        {
            var sb = new StringBuilder();
            sb.AppendLine("INSERT INTO tblActiveCard(Code, [Date], CardNumber, CardNo, Plate, OldExpireDate, [Days], NewExpireDate, CardGroupID, CustomerGroupID, UserID, FeeLevel, CustomerID,IsDelete)");
            sb.AppendLine("VALUES (");

            sb.AppendLine(string.Format("'{0}'", obj.CustomerCode));
            sb.AppendLine(", GETDATE()");
            sb.AppendLine(string.Format(", '{0}'", obj.CardNumber));
            sb.AppendLine(string.Format(", '{0}'", obj.CardNo));
            sb.AppendLine(string.Format(", '{0}'", obj.Plate1 + ";" + obj.Plate2 + ";" + obj.Plate3));
            sb.AppendLine(string.Format(", '{0}'", Convert.ToDateTime(obj.DtpDateExpired).ToString("yyyy/MM/dd")));
            sb.AppendLine(string.Format(", DATEDIFF(DAY, '{0}', '{1}')", Convert.ToDateTime(obj.DtpDateExpired).ToString("yyyy/MM/dd"), Convert.ToDateTime(_newexpire).ToString("yyyy/MM/dd")));
            sb.AppendLine(string.Format(", '{0}'", Convert.ToDateTime(_newexpire).ToString("yyyy/MM/dd")));
            sb.AppendLine(string.Format(", '{0}'", obj.CardGroupID));
            sb.AppendLine(string.Format(", '{0}'", obj.CustomerGroupID));
            sb.AppendLine(string.Format(", '{0}'", userid));
            sb.AppendLine(string.Format(", '{0}'", "0"));
            sb.AppendLine(string.Format(", '{0}'", obj.CustomerID));
            sb.AppendLine(", 0");

            sb.AppendLine(")");

            //Update card
            sb.AppendLine("UPDATE tblCard");
            sb.AppendLine(string.Format("SET ExpireDate = '{0}'", Convert.ToDateTime(_newexpire).ToString("yyyy/MM/dd")));
            sb.AppendLine(string.Format("WHERE CardNumber = '{0}'", obj.CardNumber));

            ExcuteSQL.Execute(sb.ToString());
        }

        public JsonResult GetCustomer(string code)
        {
            var obj = _tblCustomerService.GetByCode(code);
            if (obj != null)
            {
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var obj1 = new tblCustomerSubmit();
                obj1.CustomerID = "";
                obj1.CustomerCode = "";
                obj1.CustomerName = "";
                obj1.IDNumber = "";
                obj1.Mobile = "";
                obj1.CustomerGroupID = "";
                obj1.Address = "";
                obj1.Avatar = "";

                return Json(obj1, JsonRequestBehavior.AllowGet);
            }

        }

        #region Import
        public PartialViewResult ModalImportCard()
        {
            return PartialView();
        }

        public void DownloadFile()
        {
            Common.DownloadFile(Server.MapPath("~/Templates/addNewCard.xlsx"), "addNewCard.xlsx");
        }

        public JsonResult ImportFile()
        {
            //
            var userCard = GetCurrentUser.GetUser();
            var fileUpload = "";

            try
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];
                    if (file != null && file.ContentLength > 0)
                    {
                        var txtError = "";
                        var name = Common.UploadFileWithDatetime(out txtError, Server.MapPath("~/upload/files/import/"), file);

                        var path = Path.Combine(Server.MapPath("~/upload/files/import/"), name);

                        fileUpload = name;

                        if (System.IO.File.Exists(path))
                        {
                            var dt = FunctionHelper.ReadFromExcelCardCustomer(path, ref txtError);
                            if (!string.IsNullOrWhiteSpace(txtError))
                            {
                                var result = new MessageReport();
                                result.Message = txtError;
                                result.isSuccess = false;

                                return Json(result, JsonRequestBehavior.AllowGet);
                            }

                            if (dt != null && dt.Rows.Count > 0)
                            {
                                foreach (DataRow item in dt.Rows)
                                {
                                    //////Khai báo
                                    bool isNewCard = false;
                                    bool isNewCustomer = false;
                                    var customerIdmap = "";

                                    //////Lấy dữ liệu từ file
                                    var stt = item["STT"].ToString().Trim();

                                    //Kiểm tra chỉ khi có STT thì mới thực hiện
                                    
                                    if (!string.IsNullOrWhiteSpace(stt))
                                    {
                                        //Thẻ
                                        var dateRegister = "";
                                        var cardno = item["CardNo"].ToString().Trim();
                                        var cardnumber = item["Mã thẻ"].ToString().Trim();
                                        var cardgroupname = item["Nhóm thẻ"].ToString().Trim();
                                        var dateexpire = !string.IsNullOrWhiteSpace(item["Ngày hết hạn"].ToString().Trim())  ? item["Ngày hết hạn"].ToString().Trim() : DateTime.Now.ToString("dd/MM/yyyy 23:59");
                                        if (!string.IsNullOrWhiteSpace(item["Ngày đăng ký"].ToString().Trim()))
                                        {
                                             dateRegister =   (!item["Ngày đăng ký"].ToString().Equals("01/01/0001")) ? item["Ngày đăng ký"].ToString().Trim() : DateTime.Now.ToString("dd/MM/yyyy 23:59");
                                        }
                                       
                                        var plates = item["Biển số"].ToString().Trim();
                                        var vehiclenames = item["Tên xe"].ToString().Trim();

                                        //Khách hàng
                                        var customercode = item["Mã khách hàng"].ToString().Trim();
                                        var customername = item["Khách hàng"].ToString().Trim();
                                        var customergroupname = item["Nhóm khách hàng"].ToString().Trim();
                                        var compartmentId = "";
                                        if (!string.IsNullOrEmpty( item["Căn hộ"].ToString() ))
                                        {
                                            var objCompartment = _tblCompartmentService.GetByNameDetail(item["Căn hộ"].ToString().Trim());
                                            compartmentId = objCompartment.CompartmentID.ToString();
                                        }

                                        var customeridnumber = item["Chứng minh thư"].ToString().Trim();
                                        var customermobile = item["Số điện thoại"].ToString().Trim();
                                        var customeraddress = item["Địa chỉ"].ToString().Trim();

                                        //Hoạt động
                                        var isLock = string.IsNullOrWhiteSpace(item["Sử dụng"].ToString().Trim()) ? false : (item["Sử dụng"].ToString().Trim().Equals("Hoạt động") ? false : true);

                                        //Id cardgroup
                                        var cardgroupid = GetCardGroupId(cardgroupname);

                                        //Id customergroup
                                        var customergroupid = GetCustomerGroupId(customergroupname);

                                        //Lấy card custom
                                        var cardcustom = GetOrSetCardCustom(cardnumber);

                                        //Lấy khách hàng
                                        var customercustom = _tblCustomerService.GetCustomByCode(customercode);

                                        //////Gán giá trị mới từ form với các trường

                                        //Thẻ
                                        if (cardcustom != null)
                                        {
                                            cardcustom.CardNo = cardno;
                                            cardcustom.CardInActive = isLock;
                                            cardcustom.CardGroupID = cardgroupid;
                                            cardcustom.DtpDateRegisted = dateRegister;
                                        }
                                        else
                                        {
                                            //
                                            isNewCard = true;

                                            //
                                            cardcustom = new tblCardSubmit();
                                            cardcustom.CardID = Guid.NewGuid().ToString();
                                            cardcustom.CardNo = cardno;
                                            cardcustom.CardNumber = cardnumber;
                                            cardcustom.CardGroupID = cardgroupid;
                                            cardcustom.CardInActive = isLock;
                                            cardcustom.DtpDateExpired = dateexpire;
                                            cardcustom.DtpDateRegisted = dateRegister;
                                        }
                                        if (string.IsNullOrEmpty(plates))
                                        {
                                            cardcustom.Plate1 = ""; //anh bình yêu cầu sửa
                                            cardcustom.Plate2 = "";
                                            cardcustom.Plate3 = "";
                                        }

                                        //Gán biển số vào thẻ
                                        var listPlates = plates.Split(new[] { ';' });
                                        if (listPlates.Any())
                                        {
                                            for (int pl = 1; pl <= listPlates.Length; pl++)
                                            {
                                                if (pl == 1)
                                                {
                                                    cardcustom.Plate1 = listPlates[pl - 1];
                                                }

                                                if (pl == 2)
                                                {
                                                    cardcustom.Plate2 = listPlates[pl - 1];
                                                }

                                                if (pl == 3)
                                                {
                                                    cardcustom.Plate3 = listPlates[pl - 1];
                                                }
                                            }
                                        }


                                        if (string.IsNullOrEmpty(vehiclenames))
                                        {
                                            cardcustom.VehicleName1 = ""; //anh bình yêu cầu sửa
                                            cardcustom.VehicleName2 = "";
                                            cardcustom.VehicleName3 = "";
                                        }

                                        //Gán loại xe tương ứng
                                        var listVehicleNames = vehiclenames.Split(new[] { ';' });
                                        if (listVehicleNames.Any())
                                        {
                                            for (int pl = 1; pl <= listVehicleNames.Length; pl++)
                                            {
                                                if (pl == 1)
                                                {
                                                    cardcustom.VehicleName1 = listVehicleNames[pl - 1];
                                                }

                                                if (pl == 2)
                                                {
                                                    cardcustom.VehicleName2 = listVehicleNames[pl - 1];
                                                }

                                                if (pl == 3)
                                                {
                                                    cardcustom.VehicleName3 = listVehicleNames[pl - 1];
                                                }
                                            }
                                        }
                                       
                                       

                                        //Khách hàng
                                        if (!string.IsNullOrWhiteSpace(customercode))
                                        {
                                            if (customercustom != null)
                                            {
                                                customercustom.Address = customeraddress;
                                                customercustom.IDNumber = customeridnumber;
                                                customercustom.Mobile = customermobile;
                                                customercustom.CustomerName = customername;
                                                customercustom.CustomerGroupID = customergroupid;
                                                customercustom.CompartmentId = compartmentId;
                                                
                                            }
                                            else
                                            {
                                                //
                                                isNewCustomer = true;

                                                //
                                                customercustom = new tblCustomerSubmit();
                                                customercustom.CustomerID = Guid.NewGuid().ToString();
                                                customercustom.CustomerCode = customercode;
                                                customercustom.Address = customeraddress;
                                                customercustom.IDNumber = customeridnumber;
                                                customercustom.Mobile = customermobile;
                                                customercustom.CustomerName = customername;
                                                customercustom.CustomerGroupID = customergroupid;
                                                customercustom.CompartmentId = compartmentId;
                                            }

                                            customerIdmap = customercustom.CustomerID.ToString();
                                        }
                                        ////////dtest
                                        //Gán khách hàng vào thẻ
                                        cardcustom.CustomerID = customerIdmap;
                                        if (customercustom != null)
                                        {
                                            cardcustom.CustomerCode = customercode;
                                            cardcustom.CustomerName = customercustom.CustomerName;
                                            cardcustom.CustomerGroupID = customercustom.CustomerGroupID;
                                            cardcustom.CustomerMobile = customercustom.Mobile;
                                            cardcustom.CustomerAddress = customercustom.Address;
                                        }
                                      
                                        //////Nạp cở sở dữ liệu

                                        //Thẻ + Khách hàng
                                        SetCard_Customer(cardcustom, customercustom, isNewCard, isNewCustomer);

                                        //////Lưu sự kiện thao tác

                                        //Gia hạn
                                        var oldexpire = cardcustom.DtpDateExpired;
                                        var newexpire = Convert.ToDateTime(dateexpire).ToString("dd/MM/yyyy");

                                        if (oldexpire != newexpire)
                                        {
                                            SaveCardExtendProcess(cardcustom, dateexpire, userCard.Id);
                                        }

                                        //Còn lại
                                        var listAction = GetListActionType(cardcustom);
                                        if (listAction.Any())
                                        {
                                            foreach (var itemAc in listAction)
                                            {
                                                SaveCardProcess(cardcustom, itemAc, userCard.Id);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                var resultUp1 = new MessageReport();
                resultUp1.isSuccess = true;
                resultUp1.Message = "Upload excel thành công";

                //////Lưu log sự kiện
                WriteLog.Write(resultUp1, userCard, fileUpload, fileUpload, "tblCard", ConstField.ParkingCode, ActionConfigO.ImportExcel);

                return Json(resultUp1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var resultUp3 = new MessageReport();
                resultUp3.isSuccess = false;
                resultUp3.Message = ex.Message;

                return Json(resultUp3, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Import ECOME_DUNGQUAT
        public PartialViewResult ECOME_DUNGQUAT_ModalImportCard()
        {
            return PartialView();
        }

        public JsonResult ECOME_DUNGQUAT_ImportFile()
        {
            //
            var userCard = GetCurrentUser.GetUser();
            var fileUpload = "";

            try
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];
                    if (file != null && file.ContentLength > 0)
                    {
                        var txtError = "";
                        var name = Common.UploadFileWithDatetime(out txtError, Server.MapPath("~/upload/files/import/"), file);

                        var path = Path.Combine(Server.MapPath("~/upload/files/import/"), name);

                        fileUpload = name;

                        if (System.IO.File.Exists(path))
                        {
                            var dataList = FunctionHelper.ReadFromExcel_HPDQ_CSV(path, ref txtError);

                            if (!string.IsNullOrWhiteSpace(txtError))
                            {
                                var result = new MessageReport();
                                result.Message = txtError;
                                result.isSuccess = false;

                                return Json(result, JsonRequestBehavior.AllowGet);
                            }

                            if (dataList != null)
                            {
                                int count = 0;
                                foreach (var item in dataList)
                                {
                                    count++;
                                    string str = item != null ? item.ToString() : "";

                                    if (!string.IsNullOrEmpty(str) && count > 1)
                                    {
                                        var arr = str.Split(',');

                                        if (arr != null & arr.Length > 0)
                                        {
                                            //////Khai báo
                                            bool isNewCard = false;
                                            bool isNewCustomer = false;
                                            var customerIdmap = "";

                                            //////Lấy dữ liệu từ file
                                            var userid = arr[0].ToString().Trim();

                                            //Thẻ
                                            var cardno = arr[18].ToString().Trim();
                                            var cardnumber = arr[18].ToString().Trim();
                                            var cardgroupname = arr[24].ToString().Trim();
                                            var dateexpire = !string.IsNullOrWhiteSpace(arr[25].ToString().Trim()) ? arr[25].ToString().Trim() : DateTime.Now.ToString("dd/MM/yyyy 23:59");
                                            var plates = arr[26].ToString().Trim();
                                            var vehiclenames = arr[27].ToString().Trim();
                                            var importdate = arr[33].ToString().Trim();

                                            //Khách hàng
                                            var customercode = arr[2].ToString().Trim();
                                            var customername = arr[1].ToString().Trim();
                                            var customergroupname = arr[28].ToString().Trim();
                                            var customeridnumber = arr[29].ToString().Trim();
                                            var customermobile = arr[30].ToString().Trim();
                                            var customeraddress = arr[31].ToString().Trim();

                                            try
                                            {
                                                DateTime ad = DateTime.ParseExact(dateexpire, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                                            }
                                            catch (Exception)
                                            {

                                                continue;
                                            }
                                          
                                            //Hoạt động
                                            var isLock = string.IsNullOrWhiteSpace(arr[32].ToString().Trim()) ? false : (arr[32].ToString().Trim().Equals("Hoat dong") ? false : true);

                                            //Id cardgroup
                                            var cardgroupid = GetCardGroupId(cardgroupname);

                                            //Id customergroup
                                            var customergroupid = GetCustomerGroupId(customergroupname);

                                            //Lấy card custom
                                            var cardcustom = GetOrSetCardCustom(cardnumber);

                                            //Lấy khách hàng
                                            var customercustom = _tblCustomerService.GetCustomByCode(customercode);

                                            //////Gán giá trị mới từ form với các trường

                                            //Thẻ
                                            if (cardcustom != null)
                                            {
                                                cardcustom.CardNo = cardno;
                                                cardcustom.CardInActive = isLock;
                                                cardcustom.CardGroupID = cardgroupid;
                                            }
                                            else
                                            {
                                                //
                                                isNewCard = true;

                                                //
                                                cardcustom = new tblCardSubmit();
                                                cardcustom.CardID = Guid.NewGuid().ToString();
                                                cardcustom.CardNo = cardno;
                                                cardcustom.CardNumber = cardnumber;
                                                cardcustom.CardGroupID = cardgroupid;
                                                cardcustom.CardInActive = isLock;
                                                cardcustom.DtpDateExpired = dateexpire;
                                            }
                                            if (string.IsNullOrEmpty(plates))
                                            {
                                                cardcustom.Plate1 = ""; //anh bình yêu cầu sửa
                                                cardcustom.Plate2 = "";
                                                cardcustom.Plate3 = "";
                                            }

                                            //Gán biển số vào thẻ
                                            var listPlates = plates.Split(new[] { '|' });
                                            if (listPlates.Any())
                                            {
                                                for (int pl = 1; pl <= listPlates.Length; pl++)
                                                {
                                                    if (pl == 1)
                                                    {
                                                        cardcustom.Plate1 = listPlates[pl - 1];
                                                    }

                                                    if (pl == 2)
                                                    {
                                                        cardcustom.Plate2 = listPlates[pl - 1];
                                                    }

                                                    if (pl == 3)
                                                    {
                                                        cardcustom.Plate3 = listPlates[pl - 1];
                                                    }
                                                }
                                            }


                                            if (string.IsNullOrEmpty(vehiclenames))
                                            {
                                                cardcustom.VehicleName1 = ""; //anh bình yêu cầu sửa
                                                cardcustom.VehicleName2 = "";
                                                cardcustom.VehicleName3 = "";
                                            }

                                            //Gán loại xe tương ứng
                                            var listVehicleNames = vehiclenames.Split(new[] { '|' });
                                            if (listVehicleNames.Any())
                                            {
                                                for (int pl = 1; pl <= listVehicleNames.Length; pl++)
                                                {
                                                    if (pl == 1)
                                                    {
                                                        cardcustom.VehicleName1 = listVehicleNames[pl - 1];
                                                    }

                                                    if (pl == 2)
                                                    {
                                                        cardcustom.VehicleName2 = listVehicleNames[pl - 1];
                                                    }

                                                    if (pl == 3)
                                                    {
                                                        cardcustom.VehicleName3 = listVehicleNames[pl - 1];
                                                    }
                                                }
                                            }



                                            //Khách hàng
                                            if (!string.IsNullOrWhiteSpace(customercode))
                                            {
                                                if (customercustom != null)
                                                {
                                                    customercustom.Address = customeraddress;
                                                    customercustom.IDNumber = customeridnumber;
                                                    customercustom.Mobile = customermobile;
                                                    customercustom.CustomerName = customername;
                                                    customercustom.CustomerGroupID = customergroupid;
                                                }
                                                else
                                                {
                                                    //
                                                    isNewCustomer = true;

                                                    //
                                                    customercustom = new tblCustomerSubmit();
                                                    customercustom.CustomerID = Guid.NewGuid().ToString();
                                                    customercustom.CustomerCode = customercode;
                                                    customercustom.Address = customeraddress;
                                                    customercustom.IDNumber = customeridnumber;
                                                    customercustom.Mobile = customermobile;
                                                    customercustom.CustomerName = customername;
                                                    customercustom.CustomerGroupID = customergroupid;
                                                }

                                                customerIdmap = customercustom.CustomerID.ToString();
                                            }

                                            //Gán khách hàng vào thẻ
                                            cardcustom.CustomerID = customerIdmap;

                                            //////Nạp cở sở dữ liệu

                                            //Thẻ + Khách hàng
                                            SetCard_Customer(cardcustom, customercustom, isNewCard, isNewCustomer);

                                            //////Lưu sự kiện thao tác

                                            //Gia hạn
                                            var oldexpire = cardcustom.DtpDateExpired;
                                            var newexpire = Convert.ToDateTime(dateexpire).ToString("dd/MM/yyyy");

                                            if (oldexpire != newexpire)
                                            {
                                                SaveCardExtendProcess(cardcustom, dateexpire, userCard.Id);
                                            }

                                            //Còn lại
                                            var listAction = GetListActionType(cardcustom);
                                            if (listAction.Any())
                                            {
                                                foreach (var itemAc in listAction)
                                                {
                                                    SaveCardProcess(cardcustom, itemAc, userCard.Id);
                                                }
                                            }
                                        }
                                    }


                                }
                            }
                        }
                    }
                }

                var resultUp1 = new MessageReport();
                resultUp1.isSuccess = true;
                resultUp1.Message = "Upload excel thành công";

                //////Lưu log sự kiện
                WriteLog.Write(resultUp1, userCard, fileUpload, fileUpload, "tblCard", ConstField.ParkingCode, ActionConfigO.ImportExcel);

                return Json(resultUp1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var resultUp3 = new MessageReport();
                resultUp3.isSuccess = false;
                resultUp3.Message = ex.Message;

                return Json(resultUp3, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        private string GetCardGroupId(string name)
        {
            var obj = _tblCardGroupService.GetByName(name);
            if (obj != null)
            {
                return obj.CardGroupID.ToString();
            }

            return "";
        }

        private string GetCustomerGroupId(string name)
        {
            var obj = _tblCustomerGroupService.GetByName(name);
            if (obj != null)
            {
                return obj.CustomerGroupID.ToString();
            }
            else
            {
                obj = new tblCustomerGroup
                {
                    CustomerGroupID = Guid.NewGuid(),
                    CustomerGroupName = name,
                    ParentID = "0",
                    Inactive = false
                };

                var result = _tblCustomerGroupService.Create(obj);

                if (result.isSuccess)
                {
                    return obj.CustomerGroupID.ToString();
                }
            }

            return "";
        }

        private tblCardSubmit GetOrSetCardCustom(string cardnumber)
        {
            var card = _tblCardService.GetByCardNumber(cardnumber);
            if (card != null)
            {
                var obj = _tblCardService.GetCustomById(card.CardID);

                obj.DtpDateExpired = Convert.ToDateTime(card.ExpireDate).ToString("dd/MM/yyyy");

                obj.DtpDateRegisted = Convert.ToDateTime(card.DateRegister != null ? card.DateRegister : DateTime.Now).ToString("dd/MM/yyyy");
                obj.DtpDateReleased = Convert.ToDateTime(card.DateRelease != null ? card.DateRelease : DateTime.Now).ToString("dd/MM/yyyy");

                obj.OldDtpDateRegisted = Convert.ToDateTime(card.DateRegister != null ? card.DateRegister : DateTime.Now).ToString("dd/MM/yyyy");
                obj.OldDtpDateReleased = Convert.ToDateTime(card.DateRelease != null ? card.DateRelease : DateTime.Now).ToString("dd/MM/yyyy");

                return obj;
            }


            return null;
        }

        private void SetCard_Customer(tblCardSubmit cardsubmit, tblCustomerSubmit customersubmit, bool isNewCard, bool isNewCustomer)
        {
            var str = new StringBuilder();

            //Thẻ
            if (isNewCard)
            {
                str.AppendLine("INSERT INTO [dbo].[tblCard]([CardNo], [CardNumber], [CustomerID], [CardGroupID], [ImportDate], [ExpireDate],[DateRegister], [IsLock], [IsDelete], [Plate1], [VehicleName1], [Plate2], [VehicleName2], [Plate3], [VehicleName3])");

                str.AppendLine("VALUES (");

                str.AppendLine(string.Format("'{0}'", cardsubmit.CardNo));
                str.AppendLine(string.Format(", '{0}'", cardsubmit.CardNumber));
                str.AppendLine(string.Format(", '{0}'", cardsubmit.CustomerID));
                str.AppendLine(string.Format(", '{0}'", cardsubmit.CardGroupID));
                str.AppendLine(",GETDATE()");
                str.AppendLine(string.Format(", '{0}'", Convert.ToDateTime(cardsubmit.DtpDateExpired).ToString("yyyy/MM/dd")));
                str.AppendLine(string.Format(", '{0}'", Convert.ToDateTime(cardsubmit.DtpDateRegisted).ToString("yyyy/MM/dd")));
                str.AppendLine(string.Format(", '{0}'", cardsubmit.CardInActive ? 1 : 0));
                str.AppendLine(", 0");

                str.AppendLine(string.Format(", '{0}'", cardsubmit.Plate1));
                str.AppendLine(string.Format(", '{0}'", cardsubmit.VehicleName1));

                str.AppendLine(string.Format(", '{0}'", cardsubmit.Plate2));
                str.AppendLine(string.Format(", '{0}'", cardsubmit.VehicleName2));

                str.AppendLine(string.Format(", '{0}'", cardsubmit.Plate3));
                str.AppendLine(string.Format(", '{0}'", cardsubmit.VehicleName3));

                str.AppendLine(")");
            }
            else
            {
                str.AppendLine("UPDATE [dbo].[tblCard] SET");
                str.AppendLine(string.Format(" [CustomerID] = '{0}'", cardsubmit.CustomerID));
                str.AppendLine(string.Format(",[IsLock] = '{0}'", cardsubmit.CardInActive ? 1 : 0));

                if (!string.IsNullOrWhiteSpace(cardsubmit.CardNo))
                    str.AppendLine(string.Format(",[CardNo] = '{0}'", cardsubmit.CardNo));

                if (!string.IsNullOrWhiteSpace(cardsubmit.CardGroupID))
                    str.AppendLine(string.Format(",[CardGroupID] = '{0}'", cardsubmit.CardGroupID));

                if (!string.IsNullOrWhiteSpace(cardsubmit.DtpDateExpired))
                    str.AppendLine(string.Format(",[ExpireDate] = '{0}'", Convert.ToDateTime(cardsubmit.DtpDateExpired).ToString("yyyy/MM/dd")));

                if (!string.IsNullOrWhiteSpace(cardsubmit.DtpDateRegisted))
                    str.AppendLine(string.Format(",[DateRegister] = '{0}'", Convert.ToDateTime(cardsubmit.DtpDateRegisted).ToString("yyyy/MM/dd")));

                //if (!string.IsNullOrWhiteSpace(cardsubmit.Plate1))
                str.AppendLine(string.Format(",[Plate1] = '{0}'", cardsubmit.Plate1));

                //if (!string.IsNullOrWhiteSpace(cardsubmit.VehicleName1))
                    str.AppendLine(string.Format(",[VehicleName1] = N'{0}'", cardsubmit.VehicleName1));

                //if (!string.IsNullOrWhiteSpace(cardsubmit.Plate2))
                    str.AppendLine(string.Format(",[Plate2] = '{0}'", cardsubmit.Plate2));

                //if (!string.IsNullOrWhiteSpace(cardsubmit.VehicleName2))
                    str.AppendLine(string.Format(",[VehicleName2] = N'{0}'", cardsubmit.VehicleName2));

                //if (!string.IsNullOrWhiteSpace(cardsubmit.Plate3))
                    str.AppendLine(string.Format(",[Plate3] = '{0}'", cardsubmit.Plate3));

                //if (!string.IsNullOrWhiteSpace(cardsubmit.VehicleName3))
                    str.AppendLine(string.Format(",[VehicleName3] = N'{0}'", cardsubmit.VehicleName3));

                str.AppendLine(string.Format("WHERE CardNumber = '{0}'", cardsubmit.CardNumber));
            }

            //Khách hàng
            if (customersubmit != null)
            {
                if (!string.IsNullOrWhiteSpace(customersubmit.CustomerCode))
                {
                    if (isNewCustomer)
                    {
                        //var k = _tblCustomerService.GetAll().Count();

                        str.AppendLine("INSERT INTO [dbo].[tblCustomer]");
                        str.AppendLine("([CustomerID]");
                        str.AppendLine(", [CustomerName]");
                        str.AppendLine(", [CustomerCode]");
                        str.AppendLine(", [Address]");
                        str.AppendLine(", [Mobile]");
                        str.AppendLine(", [IDNumber]");
                        str.AppendLine(", [CustomerGroupID]");
                        str.AppendLine(",[CompartmentId]");
                        str.AppendLine(", [EnableAccount]");
                        str.AppendLine(", [Inactive]");
                        str.AppendLine(", [UserIDofFinger], [Finger1], [Finger2], [DevPass], [AccessExpireDate])");
                        str.AppendLine(string.Format("VALUES('{0}', N'{1}','{2}', N'{3}', '{4}', '{5}', '{6}','{7}', 1 , 0, 0, '', '', '', '2099-12-31')", customersubmit.CustomerID, customersubmit.CustomerName, customersubmit.CustomerCode, customersubmit.Address, customersubmit.Mobile, customersubmit.IDNumber, customersubmit.CustomerGroupID, customersubmit.CompartmentId));
                    }
                    else
                    {
                        str.AppendLine("UPDATE [dbo].[tblCustomer]");
                        str.AppendLine(string.Format("SET [CustomerName] = N'{0}'", customersubmit.CustomerName));
                        str.AppendLine(string.Format(",[Address] = N'{0}'", customersubmit.Address));
                        str.AppendLine(string.Format(",[Mobile] = N'{0}'", customersubmit.Mobile));
                        str.AppendLine(string.Format(",[IDNumber] = N'{0}'", customersubmit.IDNumber));
                        str.AppendLine(string.Format(",[CustomerGroupID] = '{0}'", customersubmit.CustomerGroupID));
                        str.AppendLine(string.Format(",[CompartmentId] = '{0}'", customersubmit.CompartmentId));
                        str.AppendLine(string.Format("WHERE CONVERT(varchar(50),[CustomerID]) = '{0}'", 
customersubmit.CustomerID));
                    }
                }
            }

            //
            ExcuteSQL.Execute(str.ToString());
        }
        [ValidateInput(false)]
        public PartialViewResult ModalButtonControl(int totalItem = 0, string url = "")
        {
            var listCardChoice = GetSetFromSession(null);

            ViewBag.totalItemValue = totalItem;
            ViewBag.urlValue = url;

            return PartialView(listCardChoice);
        }
        public PartialViewResult AllModalButtonControl(int totalItem = 0, string url = "")
        {
            //var listCardChoice = GetSetFromSession(null);

            //ViewBag.totalItemValue = totalItem;
            //ViewBag.urlValue = url;

            return PartialView();
        }
        public PartialViewResult ShowCardSelected(string cardnumber)
        {
            var listCardChoice = GetSetFromSession(null);

            return PartialView(listCardChoice);
        }

        public JsonResult RemoveAllCardSeleted()
        {
            var host = Request.Url.Host;
            Session[string.Format("{0}_{1}", SessionConfig.CardChoiceActionParkingSession, host)] = new List<string>();

            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddOrRemoveOneAllCardSeleted(List<string> CardNumbers, bool isAdd)
        {
            GetSetFromSession(CardNumbers, isAdd);

            return Json("", JsonRequestBehavior.AllowGet);
        }

        private List<string> GetSetFromSession(List<string> list, bool isAdd = true)
        {
            var host = Request.Url.Host;

            var listCardChoice = (List<string>)Session[string.Format("{0}_{1}", SessionConfig.CardChoiceActionParkingSession, host)];
            if (listCardChoice != null)
            {
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (isAdd)
                        {
                            if (!listCardChoice.Any(n => n.Equals(item)))
                            {
                                listCardChoice.Add(item);
                            }
                        }
                        else
                        {
                            if (listCardChoice.Any(n => n.Equals(item)))
                            {
                                listCardChoice.Remove(item);
                            }
                        }
                    }
                }
            }
            else
            {
                if (list != null)
                {
                    listCardChoice = list;
                }
                else
                {
                    listCardChoice = new List<string>();
                }
            }

            Session[string.Format("{0}_{1}", SessionConfig.CardChoiceActionParkingSession, host)] = listCardChoice;

            return listCardChoice;
        }

        public JsonResult ActionToCards(string type, string mess)
        {
            var DictionaryNoti = FunctionHelper.GetLocalizeDictionary("Home", "notification");
            var result = new MessageReport();
            result.isSuccess = false;
            result.Message = DictionaryNoti["err"];

            var data = GetSetFromSession(null);
            if (data != null && data.Any())
            {
                result = ActionProcess(type, data, mess);
            }
            else
            {
                result.isSuccess = false;
                result.Message = DictionaryNoti["please_select_card"];
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult AllActionToCards(string type, string mess , string key, string cardgr , string cus)
        {
            var DictionaryNoti = FunctionHelper.GetLocalizeDictionary("Home", "notification");
            var result = new MessageReport();
            result.isSuccess = false;
            result.Message = DictionaryNoti["err"];

                //var data = GetSetFromSession(null);
            var data = _tblCardService. GetCustomCard_Customer(key, cardgr ,cus);
         //   var lst = new List<string>
            if (data != null && data.Any())
            {
                result = ActionProcess(type, data, mess);
            }
            else
            {
                result.isSuccess = false;
                result.Message = DictionaryNoti["please_select_card"];
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

       

        private MessageReport ActionProcess(string type, List<string> data, string mess = "")
        {
            var listCard = "";
            var userCard = GetCurrentUser.GetUser();

            var result = new MessageReport();
            result.isSuccess = false;
            result.Message = "Có lỗi xảy ra";

            try
            {
                var str = new StringBuilder();

                string update = "IsDelete = 0";
                switch (type)
                {
                    case "LOCK":
                        update = "IsLock = 1";                       
                        break;
                    case "UNLOCK":
                        update = "IsLock = 0";                      
                        break;
                    case "DELETE":
                        update = "IsDelete = 1";                     
                        break;
                    default:
                        break;
                }

                str.AppendLine(string.Format("UPDATE tblCard SET {0}", update));

                str.AppendLine(", Description = N'" + (!string.IsNullOrWhiteSpace(mess) ? mess : "") + "'");

                str.AppendLine("WHERE CardNumber IN (");
                var count = 0;
                foreach (var item in data)
                {
                    count++;
                    str.AppendLine(string.Format("'{0}'{1}", item, count == data.Count ? "" : ","));

                    listCard += string.Format("{0}{1}", item, count == data.Count ? "" : ",");
                }

                str.AppendLine(")");

                result.isSuccess = ExcuteSQL.Execute(str.ToString());

                if (result.isSuccess)
                {
                    result.Message = "Thực hiện thành công";

                    switch (type)
                    {
                        case "LOCK":
                            WriteLog.Write(result, userCard, listCard, "", "tblCard", ConstField.ParkingCode, ActionConfigO.Lock);
                            break;
                        case "UNLOCK":
                            WriteLog.Write(result, userCard, listCard, "", "tblCard", ConstField.ParkingCode, ActionConfigO.Unlock);
                            break;
                        case "DELETE":
                            WriteLog.Write(result, userCard, listCard, "", "tblCard", ConstField.ParkingCode, ActionConfigO.Delete);
                            break;
                        default:
                            break;
                    }

                    //Lưu lại vào cardprocess
                    var lisf = new StringBuilder();
                    lisf.AppendLine("SELECT * FROM tblCard ");
                    lisf.AppendLine("WHERE CardNumber IN (");

                    var count1 = 0;
                    foreach (var item in data)
                    {
                        count1++;
                        lisf.AppendLine(string.Format("'{0}'{1}", item, count1 == data.Count ? "" : ","));
                    }

                    lisf.AppendLine(")");

                    var k = SqlExQuery<tblCard>.ExcuteQuery(lisf.ToString());

                    foreach (var item in k)
                    {
                        SaveCardProcessExpend(item,"","", type, userCard.Id);
                    }
                }

                if (type == "DELETE")
                {
                    var host = Request.Url.Host;
                    Session[string.Format("{0}_{1}", SessionConfig.CardChoiceActionParkingSession, host)] = new List<string>();
                }
            }
            catch (Exception ex)
            {
                result.isSuccess = false;
                result.Message = ex.Message;
            }

            return result;
        }

        public JsonResult AutoTakePhoto(AutoCapture model)
        {
            var host = Request.Url.Host;
            var result = new MessageReport(false, "Có lỗi xảy ra");

            if(model.type == "1")
            {
                var listCardChoice = (List<string>)Session[string.Format("{0}_{1}", SessionConfig.CardChoiceActionParkingSession, host)];

                if (listCardChoice == null || listCardChoice.Count == 0)
                {
                    result = new MessageReport(false, "Vui lòng chọn thẻ.");
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var strCard = string.Join(",", listCardChoice.Select(n => string.Format("'{0}'", n)));

                    if (!string.IsNullOrEmpty(strCard))
                    {
                        _tblCardService.AutoTakePhoto(strCard);

                        result = new MessageReport(true,string.Format("Cập nhật thành công {0} thẻ",listCardChoice.Count));
                    }
                }
            }
            else
            {
                var str = GetListChild("", model.customergroups);
                var listCardNumber = _tblCardService.GetListCardNumber(model.key, model.cardgroups, str, model.fromdate, model.todate, model.isCheckByTime, model.active, model.chkFindAutoCapture);

                if(listCardNumber != null && listCardNumber.Count > 0)
                {
                    var strCard = string.Join(",", listCardNumber.Select(n => string.Format("'{0}'", n)));

                    if (!string.IsNullOrEmpty(strCard))
                    {
                        _tblCardService.AutoTakePhoto(strCard);

                        result = new MessageReport(true, string.Format("Cập nhật thành công {0} thẻ", listCardNumber.Count));
                    }
                }
                else
                {
                    result = new MessageReport(false, "Không có dữ liệu thẻ được tìm thấy.");
                    return Json(result, JsonRequestBehavior.AllowGet);
                }               
            }
           

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult ModalAutoCapture()
        {
            var list = FunctionHelper.OptionAutoCapture();
            return PartialView(list);
        }


        private async Task<HttpStatusCode> UpdateToViettel(tblCard model)
        {
            var uri = ConfigurationManager.AppSettings["Viettel_Host"] + "parking-management/monthly/ticket/update";

            var postModel = new
            {
                parkingId = Convert.ToInt32(ConfigurationManager.AppSettings["Viettel_ParkingId"]),
                parkingCode = ConfigurationManager.AppSettings["Viettel_ParkingCode"],
                tokenCode = ConfigurationManager.AppSettings["Viettel_TokenCode"],
                cardNo = model.CardNo,
                cardNumber = model.CardNumber,
                id = !string.IsNullOrWhiteSpace(model.ViettelId) ? Convert.ToInt32(model.ViettelId) : 0
            };

            //Get data from api viettel
            var response = await ApiHelper.HttpPost(uri, postModel, "Basic", "bW1jbmliUHY2Q01CZzA3NFplMjF3Y28wUGZnYTo3TDRCUXJ4SGJDYTBxQkR4NTVfUU1rbEFnQVFh");

            return response.StatusCode;
        }
    }
}