using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Web.Core.Functions
{
   public class SendHelper
    {

        //public static  void  SendMailToList()
        //{
        //    try
        //    {
        //        creatReportExcel();
        //        // string subject, string mailFrom, List< string > mailToList, string bodyHtml
        //        string bodyHtml = "";
        //        string subject = "";
        //        List<string> mailToList = new List<string>();

        //        var mailFrom = "vvt.engineer@gmail.com";
        //        mailToList.Add("vantuan19961997@gmail.com");
        //        mailToList.Add("tuanvv@kztek.net");

        //        string AppLocation = "";
        //        AppLocation = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
        //        AppLocation = AppLocation.Replace("file:\\", "");
        //        string file = AppLocation + "\\ExcelFiles\\DataFile.xlsx";
        //        if (mailToList.Any())
        //        {
        //            if ((mailToList ?? new List<string>()).Any())
        //            {
        //                using (var smtp = new System.Net.Mail.SmtpClient())
        //                {
        //                    smtp.Port = 60008;//Port;
        //                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
        //                    smtp.UseDefaultCredentials = true;
        //                    smtp.Credentials = new System.Net.NetworkCredential(mailFrom, "0975067057");
        //                    smtp.Host = "smtpout.asia.secureserver.net";// SMTPServer;

        //                    using (var mail = new System.Net.Mail.MailMessage())
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

        //private static void creatReportExcel()
        //{

        //    //Nội dung đầu trang
        //   // var user = GetCurrentUser.GetUser();
        //    var str = new StringBuilder();
        //    str.AppendLine("select * from tblLED");
        //    var list = Kztek.Data.Event.SqlHelper. ExcuteSQLEvent.GetDataSet(str.ToString(), false);
        //    var listexxcel = list.Tables[0];
        //    ////var dt = ExcuteHelper
        //    ////var listexxcel = _tblLEDService.GetAll().ToList();
        //    //var dtData = listexxcel.ToDataTableNullable();
        //    var timeSearch = DateTime.Now.ToString();

        //    //var dtHeader = _tblSystemConfigService.getHeaderExcel("", timeSearch, "");

        //    //Header
        //    var listColumn = new List<Model.CustomModel.SelectListModel>();
        //    listColumn.Add(new Model.CustomModel.SelectListModel { ItemText = "Tên", ItemValue = "Tên" });
        //    listColumn.Add(new Model.CustomModel.SelectListModel { ItemText = "Máy tính", ItemValue = "Máy tính" });
        //    listColumn.Add(new Model.CustomModel.SelectListModel { ItemText = "Comport", ItemValue = "Comport" });
        //    listColumn.Add(new Model.CustomModel.SelectListModel { ItemText = "Nhân viên", ItemValue = "CustomerName" });
        //    listColumn.Add(new Model.CustomModel.SelectListModel { ItemText = "Baudrate", ItemValue = "Baudrate" });
        //    listColumn.Add(new Model.CustomModel.SelectListModel { ItemText = "Trạng thái", ItemValue = "Trạng thái" });
        //    //ReportInOutByCustomerFormatCell(null, "", "Báo_cáo_nhóm_thẻ_của_nhân_viên", "Sheet1", "", "Báo cáo nhóm thẻ của nhân viên", DateTime.Now);
        //    ReportInFormatCell(listexxcel, listColumn, null, "Báo_cáo_nhóm_thẻ_của_nhân_viên", "Sheet1", "", "Báo cáo nhóm thẻ của nhân viên", DateTime.Now.ToString());
        //}

        //private static void ReportInFormatCell(System.Data.DataTable dtData, List<Model.CustomModel.SelectListModel> listTitle = null, System.Data.DataTable dtHeader = null, string filename = "", string sheetname = "", string comments = "", string titleSheet = "", string titleTime = "")
        //{
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
        //                System.Data.DataRow dr = dtHeader.Rows[i];
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
        //        string file = AppLocation + "\\ExcelFiles\\BC01.xlsx";
        //        System.IO.FileInfo fi = new System.IO.FileInfo(file);
        //        //Save lại
        //        excelPackage.SaveAs(fi);
        //    }



        //}

    }
}
