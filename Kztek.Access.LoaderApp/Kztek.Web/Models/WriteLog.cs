using Kztek.Data.SqlHelper;
using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Web.Core.Helpers;
using log4net;
using System;
using System.IO;
using System.Text;
using System.Web;

namespace Kztek.Web.Models
{
    public class WriteLog
    {
        public static void Write(MessageReport report, User currentuser, string objId, string objname, string classname)
        {
            var computername = Common.GetComputerName(HttpContext.Current.Request.UserHostAddress);

            //Lấy classname đang giao tiếp
            ILog log = log4net.LogManager.GetLogger(classname);

            if (report.isSuccess)
            {
                log.InfoFormat("{0}: {1} ({2}_{3}) - {4}", currentuser.Name, report.Message, objId, objname, !string.IsNullOrWhiteSpace(computername) ? computername : "");
            }
            else
            {
                log.ErrorFormat("{0}: {1} ({2}_{3}) - {4}", currentuser.Name, report.Message, objId, objname, !string.IsNullOrWhiteSpace(computername) ? computername : "");
            }
        }
        public static void WriteCus(MessageReport report, User currentuser, string objId, string objname, string oldObj, string Newobj, string classname, string appcode, string actions)
        {
            var computername = Common.GetComputerName(HttpContext.Current.Request.UserHostAddress);
            objname = objname.Replace("'", "");
            var t = new tblLog();
            t.LogID = Guid.NewGuid();
            t.Actions = actions;
            t.AppCode = appcode;
            t.ComputerName = computername;
            t.Date = DateTime.Now;
            t.Description = string.Format("Action: {0} - Name: {1} - Id: {2}", report.Message, objId, objname);
            t.OldInfo = string.Format(" {0}  ", oldObj);
            t.NewInfo = string.Format(" {0} ", Newobj);
            t.IPAddress = computername;
            t.ObjectName = objname;
            t.SubSystemCode = classname;
            t.UserName = currentuser.Username;

            var str = new StringBuilder();

            str.Append("INSERT INTO tblLog (LogID, Date, UserName, AppCode, SubSystemCode, ObjectName, Actions, Description, ComputerName,OldInfo,NewInfo)");

            str.AppendLine("VALUES (");

            str.AppendLine(string.Format("'{0}'", Guid.NewGuid()));
            str.AppendLine(string.Format(", '{0}'", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")));
            str.AppendLine(string.Format(", '{0}'", t.UserName));
            str.AppendLine(string.Format(", '{0}'", t.AppCode));
            str.AppendLine(string.Format(", '{0}'", t.SubSystemCode));
            str.AppendLine(string.Format(", N'{0}'", t.ObjectName));
            str.AppendLine(string.Format(", N'{0}'", t.Actions));
            str.AppendLine(string.Format(", N'{0}'", t.Description));
            str.AppendLine(string.Format(", '{0}'", t.ComputerName));
            str.AppendLine(string.Format(", N'{0}'", t.OldInfo));
            str.AppendLine(string.Format(", N'{0}'", t.NewInfo));
            str.AppendLine(")");

            SqlExQuery<tblLog>.ExcuteNone(str.ToString());
        }
        public static void WriteLogCard(MessageReport report, User currentuser, string objId, string objname, string oldObj, string Newobj, string classname, string appcode, string actions)
        {
            var computername = Common.GetComputerName(HttpContext.Current.Request.UserHostAddress);
            objname = objname.Replace("'", "");
            var t = new tblLog();
            t.LogID = Guid.NewGuid();
            t.Actions = actions;
            t.AppCode = appcode;
            t.ComputerName = computername;
            t.Date = DateTime.Now;
            t.Description = string.Format("Action: {0} - Name: {1} - Id: {2}", report.Message, objId, objname);
            t.OldInfo = string.Format("{0}", oldObj);
            t.NewInfo = string.Format("{0}", Newobj);
            t.IPAddress = computername;
            t.ObjectName = objname;
            t.SubSystemCode = classname;
            t.UserName = currentuser.Username;
            var str = new StringBuilder();

            str.Append("INSERT INTO tblLog (LogID, Date, UserName, AppCode, SubSystemCode, ObjectName, Actions, Description, ComputerName,OldInfo,NewInfo)");

            str.AppendLine("VALUES (");

            str.AppendLine(string.Format("'{0}'", Guid.NewGuid()));
            str.AppendLine(string.Format(", '{0}'", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")));
            str.AppendLine(string.Format(", '{0}'", t.UserName));
            str.AppendLine(string.Format(", '{0}'", t.AppCode));
            str.AppendLine(string.Format(", '{0}'", t.SubSystemCode));
            str.AppendLine(string.Format(", N'{0}'", t.ObjectName));
            str.AppendLine(string.Format(", N'{0}'", t.Actions));
            str.AppendLine(string.Format(", N'{0}'", t.Description));
            str.AppendLine(string.Format(", '{0}'", t.ComputerName));
            str.AppendLine(string.Format(", N'{0}'", t.OldInfo));
            str.AppendLine(string.Format(", N'{0}'", t.NewInfo));
            str.AppendLine(")");

            SqlExQuery<tblLog>.ExcuteNone(str.ToString());
        }
        public static void Write(MessageReport report, User currentuser, string objId, string objname, string classname, string appcode, string actions)
        {
            var computername = Common.GetComputerName(HttpContext.Current.Request.UserHostAddress);
            objname = objname.Replace("'", "");
            var t = new tblLog();
            t.LogID = Guid.NewGuid();
            t.Actions = actions;
            t.AppCode = appcode;
            t.ComputerName = computername;
            t.Date = DateTime.Now;
            t.Description = string.Format("Action: {0} - Name: {1} - Id: {2}", report.Message, objname, objId);
            t.IPAddress = computername;
            t.ObjectName = objname;
            t.SubSystemCode = classname;
            t.UserName = currentuser != null ? currentuser.Username : String.Empty;

            var str = new StringBuilder();

            str.Append("INSERT INTO tblLog (LogID, Date, UserName, AppCode, SubSystemCode, ObjectName, Actions, Description, ComputerName)");

            str.AppendLine("VALUES (");

            str.AppendLine(string.Format("'{0}'", Guid.NewGuid()));
            str.AppendLine(string.Format(", '{0}'", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")));
            str.AppendLine(string.Format(", '{0}'", t.UserName));
            str.AppendLine(string.Format(", '{0}'", t.AppCode));
            str.AppendLine(string.Format(", '{0}'", t.SubSystemCode));
            str.AppendLine(string.Format(", N'{0}'", t.ObjectName));
            str.AppendLine(string.Format(", N'{0}'", t.Actions));
            str.AppendLine(string.Format(", N'{0}'", t.Description));
            str.AppendLine(string.Format(", '{0}'", t.ComputerName));

            str.AppendLine(")");

            SqlExQuery<tblLog>.ExcuteNone(str.ToString());
        }

        public static void Write(MessageReport report, string user, string objId, string objname, string classname, string appcode, string actions)
        {
            var computername = Common.GetComputerName(HttpContext.Current.Request.UserHostAddress);
            objname = objname.Replace("'", "");
            var t = new tblLog();
            t.LogID = Guid.NewGuid();
            t.Actions = actions;
            t.AppCode = appcode;
            t.ComputerName = computername;
            t.Date = DateTime.Now;
            t.Description = string.Format("Action: {0} - Name: {1} - Id: {2}", report.Message, objname, objId);
            t.IPAddress = computername;
            t.ObjectName = objname;
            t.SubSystemCode = classname;
            t.UserName = user;

            var str = new StringBuilder();

            str.Append("INSERT INTO tblLog (LogID, Date, UserName, AppCode, SubSystemCode, ObjectName, Actions, Description, ComputerName)");

            str.AppendLine("VALUES (");

            str.AppendLine(string.Format("'{0}'", Guid.NewGuid()));
            str.AppendLine(string.Format(", '{0}'", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")));
            str.AppendLine(string.Format(", '{0}'", t.UserName));
            str.AppendLine(string.Format(", '{0}'", t.AppCode));
            str.AppendLine(string.Format(", '{0}'", t.SubSystemCode));
            str.AppendLine(string.Format(", N'{0}'", t.ObjectName));
            str.AppendLine(string.Format(", N'{0}'", t.Actions));
            str.AppendLine(string.Format(", N'{0}'", t.Description));
            str.AppendLine(string.Format(", '{0}'", t.ComputerName));

            str.AppendLine(")");

            SqlExQuery<tblLog>.ExcuteNone(str.ToString());
        }


        public static void WriteLogFile(MessageReport report, User currentuser, string objId, string objname, string classname, string appcode, string actions)
        {
            StreamWriter log;
            FileStream fileStream = null;
            DirectoryInfo logDirInfo = null;
            FileInfo logFileInfo;

            var logFilePath = AppDomain.CurrentDomain.BaseDirectory /*+ "lic.dat"*/;
            var computername = Common.GetComputerName(HttpContext.Current.Request.UserHostAddress);
            //tạo folder
            var myDirectory = logFilePath + "systemconfig";
            if (!Directory.Exists(myDirectory))
                Directory.CreateDirectory(myDirectory);

            //string logFilePath = "C:\\Logs\\";
            //string logFilePath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            //logFilePath += "\\";
            //đường dẫn file
            var filePath = myDirectory + "\\" + classname + "-" + System.DateTime.Today.ToString("MM-dd-yyyy") + "." + "dat";
            logFileInfo = new FileInfo(filePath);
            logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
            if (!logDirInfo.Exists) logDirInfo.Create();
            if (!logFileInfo.Exists)
            {
                fileStream = logFileInfo.Create();
            }
            else
            {
                fileStream = new FileStream(filePath, FileMode.Append);
            }
            log = new StreamWriter(fileStream);
            var strLog = string.Format("IPAddress: {0},Action: {1} - Name: {2} - Id: {3},AppCode: {4},SubSystemCode: {5},User: {6},ObjectName: {7}, Action: {8},Date: {9}", computername, report.Message, objname, objId, appcode, classname, currentuser.Username, objname, actions, DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            log.WriteLine(strLog);
            log.Close();
        }
        public static void WriteAPI(MessageReport report, User currentuser, string objId, string objname, string classname, string appcode, string actions)
        {
            var computername = Common.GetComputerName(HttpContext.Current.Request.UserHostAddress);
            objname = objname.Replace("'", "");
            var t = new tblLog();
            t.LogID = Guid.NewGuid();
            t.Actions = actions;
            t.AppCode = appcode;
            t.ComputerName = computername;
            t.Date = DateTime.Now;
            t.Description = string.Format("Action: {0} - Name: {1} - Id: {2}", report.Message, objname, objId);
            t.IPAddress = computername;
            t.ObjectName = objname;
            t.SubSystemCode = classname;
            t.UserName = currentuser != null ? currentuser.Username : String.Empty;

            var str = new StringBuilder();

            str.Append("INSERT INTO tblLog (LogID, Date, UserName, AppCode, SubSystemCode, ObjectName, Actions, Description, ComputerName)");

            str.AppendLine("VALUES (");

            str.AppendLine(string.Format("'{0}'", Guid.NewGuid()));
            str.AppendLine(string.Format(", '{0}'", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")));
            str.AppendLine(string.Format(", '{0}'", t.UserName));
            str.AppendLine(string.Format(", '{0}'", t.AppCode));
            str.AppendLine(string.Format(", '{0}'", t.SubSystemCode));
            str.AppendLine(string.Format(", N'{0}'", t.ObjectName));
            str.AppendLine(string.Format(", N'{0}'", t.Actions));
            str.AppendLine(string.Format(", N'{0}'", t.Description));
            str.AppendLine(string.Format(", '{0}'", t.ComputerName));

            str.AppendLine(")");

            SqlExQuery<tblLog>.ExcuteNone(str.ToString());
        }


    }
}