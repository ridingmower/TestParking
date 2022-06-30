using ClosedXML.Excel;
using Kztek.Model.CustomModel;
using Kztek.Model.CustomModel.iParking;
using Kztek.Model.Models;
using Kztek.Service.Admin;
using Kztek.Web.Attributes;
using Kztek.Web.Core.Extensions;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Models;
using Kztek.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Kztek.Web.Areas.Parking.Controllers
{
    public class tblLEDController : Controller
    {
        private ItblLEDService _tblLEDService;
        private ItblPCService _tblPCService;
        private static int count = 0;
        private ItblSystemConfigService _tblSystemConfigService;
        public tblLEDController(ItblSystemConfigService _tblSystemConfigService, ItblLEDService _tblLEDService, ItblPCService _tblPCService)
        {
            this._tblSystemConfigService = _tblSystemConfigService;
            this._tblLEDService = _tblLEDService;
            this._tblPCService = _tblPCService;
        }
        public static void InitializeHangFire()
        {
            var sqlStorage = new Hangfire.SqlServer.SqlServerStorage("connectionString");
            var options = new Hangfire.BackgroundJobServerOptions
            {
                ServerName = "Test Server"
            };
            Hangfire.JobStorage.Current = sqlStorage;
        }
        public tblLEDController()
        {
        }

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Index(string key, string pc, int page = 1, string group = "", string selectedId = "", string chkExport = "0 ")
        {
            var pageSize = 20;
            var datefrompicker = "";

            string fromdate = DateTime.Now.ToString();
            string todate = DateTime.Now.AddDays(1).ToString();
            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }
            //Excel
            //if (chkExport.Equals("1"))
            //{
            //    //Query lấy dữ liệu
            //    var listExcel = _tblLEDService.GetAllCustomPagingByFirst(key, pc, page, pageSize);



            //    //Xuất file theo format
            //    ReportInFormatCell(listExcel, "", "", "Sheet1", "", "", datefrompicker);

            //    return RedirectToAction("ReportIn");
            //}
            var list = _tblLEDService.GetAllCustomPagingByFirst(key, pc, page, pageSize);

            var gridModel = PageModelCustom<tblLEDCustomViewModel>.GetPage(list, page, pageSize);

            ViewBag.PCs = GetPCList();

            ViewBag.keyValue = key;
            ViewBag.pcValue = pc;
            ViewBag.groupValue = group;
            ViewBag.selectedIdValue = selectedId;

            return View(gridModel);
        }

        //public int Count()
        //{
        //    creatReportExcel();

        //    return count;
        //}
        //public string creatReportExcel()
        //{

        //    //Nội dung đầu trang
        //    //var user = GetCurrentUser.GetUser();
        //    var str = new StringBuilder();
        //    str.AppendLine("select * from [MPARKING].[dbo].[tblLED]");
        //    var list = Data.SqlHelper.ExcuteSQL.GetTable(str.ToString(), ConfigurationManager.ConnectionStrings["KztekEntities"].ConnectionString, false);

        //    //var dtData = listexxcel.ToDataTableNullable();
        //    var timeSearch = DateTime.Now.ToString();

        //    DataTable dt = new DataTable();
        //    dt.Columns.Add("header", typeof(string));
        //    dt.Rows.Add("Công ty TSQ Việt nam");
        //    dt.Rows.Add("Tòa Tháp Thiên Niên Kỷ, số 04 Đường Quang Trung, Yết Kiêu, Hà Đông, Hà Nội");
        //    dt.Rows.Add("");
        //    dt.Rows.Add("11/06/2022 1:22:22 CH");
        //    dt.Rows.Add("11/06/2022 1:22:22 CH");
        //    dt.Rows.Add("Người phê duyệt");


        //    //var dtHeader = _tblSystemConfigService.getHeaderExcel("", timeSearch, user.Username);

        //    //Header
        //    var listColumn = new List<SelectListModel>();
        //    listColumn.Add(new SelectListModel { ItemText = "Tên", ItemValue = "Tên" });
        //    listColumn.Add(new SelectListModel { ItemText = "Máy tính", ItemValue = "Máy tính" });
        //    listColumn.Add(new SelectListModel { ItemText = "Comport", ItemValue = "Comport" });
        //    listColumn.Add(new SelectListModel { ItemText = "Nhân viên", ItemValue = "CustomerName" });
        //    listColumn.Add(new SelectListModel { ItemText = "Baudrate", ItemValue = "Baudrate" });
        //    listColumn.Add(new SelectListModel { ItemText = "Trạng thái", ItemValue = "Trạng thái" });
        //    //ReportInOutByCustomerFormatCell(null, "", "Báo_cáo_nhóm_thẻ_của_nhân_viên", "Sheet1", "", "Báo cáo nhóm thẻ của nhân viên", DateTime.Now);
        //    string str1 = "";
        //   str1 =  ReportInFormatCell(list, listColumn, dt, "Báo_cáo_nhóm_thẻ_của_nhân_viên", "Sheet1", "", "Báo cáo nhóm thẻ của nhân viên", DateTime.Now.ToString());
        //    return str1.ToString();
        //}


        //private string ReportInFormatCell(DataTable dtData, List<SelectListModel> listTitle = null, DataTable dtHeader = null, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        //{
        //    string file = "";
        //    System.IO.Stream stream = null;
        //    using (var excelPackage = new OfficeOpenXml.ExcelPackage(stream ?? new System.IO.MemoryStream()))
        //    {
        //        // Tạo author cho file Excel
        //        excelPackage.Workbook.Properties.Author = "FutechJSC";

        //        // Tạo title cho file Excel
        //        excelPackage.Workbook.Properties.Title = "";

        //        // thêm comments
        //        excelPackage.Workbook.Properties.Comments = comments;

        //        // Add Sheet vào file Excel
        //        excelPackage.Workbook.Worksheets.Add(sheetname);

        //        // Lấy Sheet bạn vừa mới tạo ra để thao tác 
        //        var workSheet = excelPackage.Workbook.Worksheets[1];

        //        workSheet.Cells[1, 1, 1, 13].Merge = true;
        //        workSheet.Cells[2, 1, 2, 13].Merge = true;
        //        workSheet.Cells[3, 1, 3, 13].Merge = true;
        //        workSheet.Cells[4, 1, 4, 13].Merge = true;
        //        workSheet.Cells[5, 1, 5, 13].Merge = true;
        //        workSheet.Cells[6, 1, 6, 13].Merge = true;

        //        workSheet.Cells.AutoFitColumns();
        //        workSheet.Cells.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
        //        // Đổ data vào Excel file
        //        var count = 0;

        //        //tao header cho file
        //        var arrApha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        //        //if (listData != null && listData.Rows.Count > 0)
        //        //{
        //        //Load danh sách phần header
        //        if (dtHeader != null && dtHeader.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < dtHeader.Rows.Count; i++)
        //            {
        //                DataRow dr = dtHeader.Rows[i];
        //                workSheet.Cells[i + 1, 1].Value = dr["header"].ToString();
        //                workSheet.Cells[i + 1, 1].Style.Font.Bold = true;
        //            }
        //        }

        //        //Load phần tiêu đề của từng côtj
        //        foreach (var item in listTitle)
        //        {
        //            count++;
        //            workSheet.Cells[dtHeader.Rows.Count + 1, count].Value = item.ItemText;
        //            workSheet.Cells[dtHeader.Rows.Count + 1, count].Style.Font.Bold = true;
        //            workSheet.Cells[dtHeader.Rows.Count + 1, count].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

        //        }
        //        //}

        //        //workSheet.Cells[dtHeader.Rows.Count + 1, 1, listData.Rows.Count + 1, listTitle.Count()].AutoFitColumns();

        //        workSheet.Cells.AutoFitColumns();

        //        //Dòng bắt đầu của dữ liệu
        //        workSheet.Cells.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
        //        workSheet.Cells[dtHeader.Rows.Count + 2, 1].LoadFromDataTable(dtData, false);
        //        workSheet.Cells.Style.Font.Name = "Times New Roman";
        //        workSheet.Cells.Style.Font.Size = 12;
        //        workSheet.Cells.Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
        //        workSheet.Cells.AutoFitColumns();


        //        string AppLocation = "";
        //        AppLocation = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
        //        AppLocation = AppLocation.Replace("file:\\", "");
        //         file = AppLocation + string.Format("\\ExcelFiles\\BC{0}.xlsx",DateTime.Now.ToString("yyyyMMddHHmm"));
        //        System.IO.FileInfo fi = new System.IO.FileInfo(file);
        //        //Save lại
        //        excelPackage.SaveAs(fi);
        //    }

        //    return file;

        //}


        //public void SendMailToList()
        //{
        //    try
        //    {

        //      var file =  creatReportExcel();
        //        if (System.IO.File.Exists(file))
        //        {
        //            Console.WriteLine("file exists");
        //        }
        //        else
        //        {
        //            var listToEmail = new List<string>();
        //            listToEmail.Add("vvt.engineer@gmail.com");
        //            using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
        //            {
        //                client.EnableSsl = true;
        //                client.DeliveryMethod = SmtpDeliveryMethod.Network;
        //                client.UseDefaultCredentials = false;
        //                string password = "jmuvtjnuiskqeref";
        //                client.Credentials = new NetworkCredential("vantuan19961997@gmail.com", password);
        //                MailMessage msg = new MailMessage();
        //                //msg.To.Add("anhnv@kztek.net");

        //                foreach (string mailAddr in listToEmail)
        //                {
        //                    msg.To.Add(mailAddr);
        //                }

        //                msg.From = new MailAddress("nguyenvietanh09031997@gmail.com", "ReportManager");
        //                msg.Subject = "Report: " + DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd");
        //                msg.Body = "";
        //                Attachment dinhkem = new Attachment(file);
        //                msg.Attachments.Add(dinhkem);
        //                //client.SendCompleted += Client_SendCompleted; ;
        //                client.Send(msg);
        //                //isReported = true;
        //                //dgvMessage.Rows.Add(dgvMessage.Rows.Count + 1, DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), "Send Report to Gmail", "Success");
        //                //StaticPool.Logger_Info(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "Send Report to Gmail Success");
        //            }

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new InvalidOperationException("Can't send email. Error: " + ex.Message);
        //    }
        //}
        //private void _sendEmail(string MailTo, string MailSubject)
        //{

        //    try
        //    {
        //        string AppLocation = "";
        //        AppLocation = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
        //        AppLocation = AppLocation.Replace("file:\\", "");
        //        string file = AppLocation + "\\ExcelFiles\\DataFile.xlsx";
        //        MailMessage mail = new MailMessage();
        //        SmtpClient SmtpServer = new SmtpClient("localhost:");
        //        mail.From = new MailAddress("tuanvv@kztek.net");
        //        mail.To.Add(MailTo); // Sending MailTo  
        //        List<string> li = new List<string>();
        //        li.Add("saihacksoft@gmail.com");

        //        mail.CC.Add(string.Join<string>(",", li)); // Sending CC  
        //        mail.Bcc.Add(string.Join<string>(",", li)); // Sending Bcc   
        //        mail.Subject = MailSubject; // Mail Subject  
        //        mail.Body = "Sales Report *This is an automatically generated email, please do not reply*";
        //        System.Net.Mail.Attachment attachment;
        //        attachment = new System.Net.Mail.Attachment(file); //Attaching File to Mail  
        //        mail.Attachments.Add(attachment);
        //        SmtpServer.Port = 60002; //PORT  
        //        SmtpServer.EnableSsl = true;
        //        SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
        //        SmtpServer.UseDefaultCredentials = false;
        //        SmtpServer.Credentials = new NetworkCredential("Email id of Gmail", "Password of Gmail");
        //        SmtpServer.Send(mail);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    throw new NotImplementedException();
        //}

      

        [CheckSessionLogin]
        [CheckAuthorize]
        [HttpGet]
        public ActionResult Create(string key, string pc, string group = "")
        {
            ViewBag.PCs = GetPCList();
            ViewBag.SideIndex = FunctionHelper.HubList1();
            ViewBag.LedType = FunctionHelper.LEDType1();

            ViewBag.keyValue = key;
            ViewBag.pcValue = pc;
            ViewBag.groupValue = group;

            return View();
        }

        [HttpPost]
        public ActionResult Create(tblLEDView obj, string key, string pc, string group = "", bool SaveAndCountinue = false)
        {
            ViewBag.PCs = GetPCList();
            ViewBag.SideIndex = FunctionHelper.HubList1();
            ViewBag.LedType = FunctionHelper.LEDType1();

            ViewBag.keyValue = key;
            ViewBag.pcValue = pc;
            ViewBag.groupValue = group;

            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            if (string.IsNullOrWhiteSpace(obj.LEDName) || (obj.Address <= 0 || obj.Address == null) || obj.SideIndex == null)
            {
                if (string.IsNullOrWhiteSpace(obj.LEDName))
                {
                    ModelState.AddModelError("LEDName", FunctionHelper.GetLocalizeDictionary("Home", "notification")["LEDName"]);

                }
                if (obj.Address <= 0 || obj.Address == null)
                {
                    ModelState.AddModelError("Address", FunctionHelper.GetLocalizeDictionary("Home", "notification")["addIp"]);
                }
                if (obj.SideIndex == null)
                {
                    ModelState.AddModelError("SideIndex", FunctionHelper.GetLocalizeDictionary("Home", "notification")["interface"]);
                }
                return View(obj);
            }

            var existed = _tblLEDService.GetByName(obj.LEDName);
            if (existed != null)
            {
                ModelState.AddModelError("LEDName", FunctionHelper.GetLocalizeDictionary("Home", "notification")["LED_Name_already_exists"]);
                return View(obj);
            }

            //Thực hiện thêm mới
            tblLED objCreate = new tblLED();
            objCreate.LEDName = obj.LEDName;
            objCreate.PCID = obj.PCID;
            objCreate.Comport = obj.Comport;
            objCreate.Baudrate = obj.Baudrate;
            objCreate.RowNumber = obj.RowNumber;
            objCreate.SideIndex = Convert.ToInt32(obj.SideIndex);
            objCreate.Address = Convert.ToInt32(obj.Address);
            objCreate.LedType = obj.LedType;
            objCreate.EnableLED = obj.EnableLED;

            var result = _tblLEDService.Create(objCreate);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.LEDID.ToString(), obj.LEDName, "tblLED", ConstField.ParkingCode, ActionConfigO.Create);

                if (SaveAndCountinue)
                {
                    TempData["Success"] = result.Message;
                    return RedirectToAction("Create", new { group = group, key = key, pc = pc, selectedId = obj.LEDID });
                }

                return RedirectToAction("Index", new { group = group, key = key, pc = pc, selectedId = obj.LEDID });
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
            ViewBag.SideIndex = FunctionHelper.HubList1();
            ViewBag.LedType = FunctionHelper.LEDType1();

            ViewBag.keyValue = key;
            ViewBag.pcValue = pc;
            ViewBag.groupValue = group;

            ViewBag.PN = page;

            var obj = _tblLEDService.GetById(Convert.ToInt32(id));

            tblLEDView objCreate = new tblLEDView();
            objCreate.LEDID = obj.LEDID;
            objCreate.LEDName = obj.LEDName;
            objCreate.PCID = obj.PCID;
            objCreate.RowNumber = obj.RowNumber;
            objCreate.Comport = obj.Comport;
            objCreate.Baudrate = obj.Baudrate;
            objCreate.SideIndex = Convert.ToInt32(obj.SideIndex);
            objCreate.Address = Convert.ToInt32(obj.Address);
            objCreate.LedType = obj.LedType;
            objCreate.EnableLED = obj.EnableLED;

            return View(objCreate);
        }

        [HttpPost]
        public ActionResult Update(tblLEDView obj, int page = 1, string key = "", string pc = "", string group = "")
        {
            ViewBag.PCs = GetPCList();
            ViewBag.SideIndex = FunctionHelper.HubList1();
            ViewBag.LedType = FunctionHelper.LEDType1();

            ViewBag.keyValue = key;
            ViewBag.pcValue = pc;
            ViewBag.groupValue = group;

            ViewBag.PN = page;

            var oldObj = _tblLEDService.GetById(obj.LEDID);
            if (oldObj == null)
            {
                ViewBag.Error = FunctionHelper.GetLocalizeDictionary("Home", "notification")["record_does_not_exist"];
                return View(obj);
            }

            if (string.IsNullOrWhiteSpace(obj.LEDName) || (obj.Address <= 0 || obj.Address == null) || obj.SideIndex == null)
            {
                if (string.IsNullOrWhiteSpace(obj.LEDName))
                {
                    ModelState.AddModelError("LEDName", FunctionHelper.GetLocalizeDictionary("Home", "notification")["LEDName"]);

                }
                if (obj.Address <= 0 || obj.Address == null)
                {
                    ModelState.AddModelError("Address", FunctionHelper.GetLocalizeDictionary("Home", "notification")["addIp"]);
                }
                if (obj.SideIndex == null)
                {
                    ModelState.AddModelError("SideIndex", FunctionHelper.GetLocalizeDictionary("Home", "notification")["interface"]);
                }
                return View(obj);
            }

            var objExisted = _tblLEDService.GetByName_Id(obj.LEDName, obj.LEDID.ToString());
            if (objExisted != null)
            {
                ViewBag.Error = FunctionHelper.GetLocalizeDictionary("Home", "notification")["LED_Name_already_exists"];
                return View(oldObj);
            }

            //Gán giá trị
            oldObj.SideIndex = Convert.ToInt32(obj.SideIndex);
            oldObj.LedType = obj.LedType;
            oldObj.Address = Convert.ToInt32(obj.Address);
            oldObj.Baudrate = obj.Baudrate;
            oldObj.Comport = obj.Comport;
            oldObj.RowNumber = obj.RowNumber;
            oldObj.EnableLED = obj.EnableLED;
            oldObj.LEDName = obj.LEDName;
            oldObj.PCID = obj.PCID;

            //Thực hiện cập nhật
            var result = _tblLEDService.Update(oldObj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.LEDID.ToString(), obj.LEDName, "tblLED", ConstField.ParkingCode, ActionConfigO.Update);

                return RedirectToAction("Index", new { group = group, page = page, key = key, pc = pc, selectedId = obj.LEDID });
            }
            else
            {
                ModelState.AddModelError("", result.Message);
                return View(oldObj);
            }
        }

        public JsonResult Delete(string id)
        {
            var obj = new tblLED();

            var result = _tblLEDService.DeleteById(id, ref obj);
            if (result.isSuccess)
            {
                WriteLog.Write(result, GetCurrentUser.GetUser(), obj.LEDID.ToString(), obj.LEDName, "tblLED", ConstField.ParkingCode, ActionConfigO.Delete);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public void GetPCList3()
        {

        }
        private List<tblPC> GetPCList()
        {
            return _tblPCService.GetAllActive().ToList();
        }
    }
}