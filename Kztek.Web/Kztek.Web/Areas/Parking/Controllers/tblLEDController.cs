using ClosedXML.Excel;
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

        public tblLEDController(ItblLEDService _tblLEDService, ItblPCService _tblPCService)
        {
            this._tblLEDService = _tblLEDService;
            this._tblPCService = _tblPCService;
        }

        public tblLEDController()
        {
        }

        [CheckSessionLogin]
        [CheckAuthorize]
        public ActionResult Index(string key, string pc, int page = 1, string group = "", string selectedId = "" , string chkExport = "0 ")
        {
            var pageSize = 20;
            var datefrompicker = "";

            string fromdate = DateTime.Now.ToString();
            string todate = DateTime.Now.AddDays(1). ToString();
            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                datefrompicker = fromdate + "-" + todate;
            }
            //Excel
            if (chkExport.Equals("1"))
            {
                //Query lấy dữ liệu
                var listExcel = _tblLEDService.GetAllCustomPagingByFirst(key, pc, page, pageSize);

               

                //Xuất file theo format
                ReportInFormatCell(listExcel, "", "", "Sheet1", "","", datefrompicker);

                return RedirectToAction("ReportIn");
            }
            var list = _tblLEDService.GetAllCustomPagingByFirst(key, pc, page, pageSize);

            var gridModel = PageModelCustom<tblLEDCustomViewModel>.GetPage(list, page, pageSize);

            ViewBag.PCs = GetPCList();

            ViewBag.keyValue = key;
            ViewBag.pcValue = pc;
            ViewBag.groupValue = group;
            ViewBag.selectedIdValue = selectedId;

            return View(gridModel);
        }
        //public void SaveExcel(/*string ckExport */)
        //{
        //    //Excel
        //    //if (ckExport.Equals("1"))
        //    //{


        //        //Query lấy dữ liệu
        //        var listexxcel = _tblLEDService.GetAll();
        //        var dt = listexxcel.ToDataTableNullable();
        //        string AppLocation = "";
        //        AppLocation = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
        //        AppLocation = AppLocation.Replace("file:\\", "");
        //        string file = AppLocation + "\\ExcelFiles\\DataFile.xlsx";

        //        using (XLWorkbook wb = new XLWorkbook())
        //        {
        //            wb.Worksheets.Add(dt);
        //            wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        //            wb.Style.Font.Bold = true;
        //            wb.SaveAs(file);
        //        }
        //    //Xuất file theo format

        //    SendMailToList("","", null, "");
            
        //    //}
        //    //return RedirectToAction("ReportIn");
        //}
        //public void SendMailToList(string subject, string mailFrom, List<string> mailToList, string bodyHtml)
        //{
        //    try
        //    {
        //        string AppLocation = "";
        //        AppLocation = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
        //        AppLocation = AppLocation.Replace("file:\\", "");
        //        string file = AppLocation + "\\ExcelFiles\\DataFile.xlsx";
        //        if (mailToList.Any())
        //        {
        //            if ((mailToList ?? new List<string>()).Any())
        //            {
        //                using (var smtp = new SmtpClient())
        //                {
        //                    smtp.Port = 343;//Port;
        //                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        //                    smtp.UseDefaultCredentials = true;
        //                    smtp.Credentials = new NetworkCredential("", "");
        //                    smtp.Host = "";// SMTPServer;

        //                    using (var mail = new MailMessage())
        //                    {
        //                        mail.From = new MailAddress(mailFrom);
        //                        mail.Subject = subject;
        //                        mail.Body = bodyHtml;
        //                        System.Net.Mail.Attachment attachment;
        //                        attachment = new System.Net.Mail.Attachment(file); //Attaching File to Mail  
        //                        mail.BodyEncoding = Encoding.UTF8;
        //                        mail.IsBodyHtml = true;//IsBodyHTML;
        //                        foreach (var toMail in mailToList ?? new List<string>())
        //                        {
        //                            mail.To.Add(new MailAddress(toMail));
        //                        }
        //                        smtp.Send(mail);
                                
        //                    }
        //                }
        //            }
        //        }
        //        //return false;
        //        ////throw new Exception("Don't have any email address to send!");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new InvalidOperationException("Can't send email. Error: " + ex.Message);
        //    }
        //}
        //private void _sendEmail(string MailTo, string MailSubject)
        //{
            
        //        try
        //        {
        //            string AppLocation = "";
        //            AppLocation = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
        //            AppLocation = AppLocation.Replace("file:\\", "");
        //            string file = AppLocation + "\\ExcelFiles\\DataFile.xlsx";
        //            MailMessage mail = new MailMessage();
        //            SmtpClient SmtpServer = new SmtpClient("localhost:");
        //            mail.From = new MailAddress("tuanvv@kztek.net");
        //            mail.To.Add(MailTo); // Sending MailTo  
        //            List<string> li = new List<string>();
        //            li.Add("saihacksoft@gmail.com");
                   
        //            mail.CC.Add(string.Join<string>(",", li)); // Sending CC  
        //            mail.Bcc.Add(string.Join<string>(",", li)); // Sending Bcc   
        //            mail.Subject = MailSubject; // Mail Subject  
        //            mail.Body = "Sales Report *This is an automatically generated email, please do not reply*";
        //            System.Net.Mail.Attachment attachment;
        //            attachment = new System.Net.Mail.Attachment(file); //Attaching File to Mail  
        //            mail.Attachments.Add(attachment);
        //            SmtpServer.Port = 60002; //PORT  
        //            SmtpServer.EnableSsl = true;
        //            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
        //            SmtpServer.UseDefaultCredentials = false;
        //            SmtpServer.Credentials = new NetworkCredential("Email id of Gmail", "Password of Gmail");
        //            SmtpServer.Send(mail);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }

        //    throw new NotImplementedException();
        //}
    
        private void ReportInFormatCell(PagedList.IPagedList<tblLEDCustomViewModel> listExcel, object excelcol, object p1, string v1, string v2, object p2, object datefrompicker)
        {
            throw new NotImplementedException();
        }

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

            if (string.IsNullOrWhiteSpace(obj.LEDName) || (obj.Address <= 0 || obj.Address == null) ||  obj.SideIndex == null)
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

            if (string.IsNullOrWhiteSpace(obj.LEDName) || (obj.Address <= 0 || obj.Address == null) ||  obj.SideIndex == null)
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
            oldObj.SideIndex  = Convert.ToInt32(obj.SideIndex);
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