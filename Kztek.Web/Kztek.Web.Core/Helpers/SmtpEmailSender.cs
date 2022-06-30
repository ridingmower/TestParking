using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Kztek.Web.Core.Helpers
{
    public class SmtpEmailSender
    {
        public string SMTPServer { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool EnabledSSL { get; set; }
        public bool UseDefaultCredential { get; set; }
        public bool IsBodyHTML { set; get; }
        public string Signalture { get; set; }

        public bool SendSingleMail(string subject, string mailFrom, string mailTo, string bodyHtml)
        {
            try
            {
                using (var smtp = new SmtpClient())
                {
                    smtp.Host = SMTPServer;
                    smtp.Port = Port;
                    smtp.Credentials = new NetworkCredential(Username, Password);
                    smtp.EnableSsl = EnabledSSL;
                    using (var mail = new MailMessage(mailFrom, mailTo, subject, bodyHtml))
                    {
                        mail.BodyEncoding = Encoding.UTF8;
                        mail.IsBodyHtml = IsBodyHTML;
                        smtp.Send(mail);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(string.Format("Can't send email From: {0} To: {1}!", mailFrom, mailTo) + ex.Message);
            }
        }

        public bool SendMailToList(string subject, string mailFrom, List<string> mailToList, string bodyHtml)
        {
            try
            {
                if (mailToList.Any())
                {
                    if ((mailToList ?? new List<string>()).Any())
                    {
                        using (var smtp = new SmtpClient())
                        {
                            smtp.Port = Port;
                            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                            smtp.UseDefaultCredentials = UseDefaultCredential;
                            smtp.Credentials = new NetworkCredential(Username, Password);
                            smtp.Host = SMTPServer;

                            using (var mail = new MailMessage())
                            {
                                mail.From = new MailAddress(mailFrom);
                                mail.Subject = subject;
                                mail.Body = bodyHtml;

                                mail.BodyEncoding = Encoding.UTF8;
                                mail.IsBodyHtml = IsBodyHTML;
                                foreach (var toMail in mailToList ?? new List<string>())
                                {
                                    mail.To.Add(new MailAddress(toMail));
                                }
                                smtp.Send(mail);
                                return true;
                            }
                        }
                    }
                }
                return false;
                //throw new Exception("Don't have any email address to send!");
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Can't send email. Error: " + ex.Message);
            }
        }

        //public static async Task SendMail(List<string> ToMail, string Subject, string HtmlContent)
        //{
        //    try
        //    {
        //        if ((ToMail != null && ToMail.Count == 0) || ToMail == null)
        //            return;

        //        var FromMail = await AppSettingHelper.GetStringFromAppSetting("Email:System:Account");
        //        var Password = await AppSettingHelper.GetStringFromAppSetting("Email:System:Password");
        //        var port = await AppSettingHelper.GetStringFromAppSetting("Email:System:Port");

        //        MailMessage message = new MailMessage();
        //        SmtpClient smtp = new SmtpClient();
        //        message.From = new MailAddress(FromMail, "Eazyerp");

        //        if (ToMail.Any())
        //        {
        //            foreach (var item in ToMail)
        //            {
        //                message.To.Add(new MailAddress(item));
        //            }
        //        }

        //        message.Subject = Subject;
        //        message.IsBodyHtml = true; //to make message body as html  
        //        message.Body = HtmlContent;
        //        smtp.Port = !string.IsNullOrEmpty(port) ? Convert.ToInt32(port) : 0;
        //        smtp.Host = await AppSettingHelper.GetStringFromAppSetting("Email:System:Host"); //for gmail host  
        //        smtp.EnableSsl = true;
        //        smtp.UseDefaultCredentials = false;
        //        smtp.Credentials = new NetworkCredential(FromMail, Password);
        //        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        //        await smtp.SendMailAsync(message);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}