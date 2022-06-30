using Kztek.Data.AccessEvent.Infrastructure;
using Kztek.Data.Repository;
using Kztek.Data.SqlHelper;
using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Web.Core.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Service.Admin
{
    public interface ItblLockerProcessService
    {
        MessageReport CreateSql(tblLocker model, string actionV, string message,string type = "0");
    }

    public class tblLockerProcessService : ItblLockerProcessService
    {
        private ItblLockerProcessRepository _tblLockerProcessRepository;
       
        public tblLockerProcessService(ItblLockerProcessRepository _tblLockerProcessRepository)
        {
            this._tblLockerProcessRepository = _tblLockerProcessRepository;
           
        }

        public MessageReport CreateSql(tblLocker obj, string actionV, string message, string type = "0")
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {            
                var sb = new StringBuilder();
                sb.AppendLine("INSERT INTO dbo.[tblLockerProcess] (");

                sb.AppendLine("Id");
                sb.AppendLine(", LockerName");
                sb.AppendLine(", LockerReaderIndex");
                sb.AppendLine(", ControllerID");
                sb.AppendLine(", CardNumber");
                sb.AppendLine(", CardNo");
                sb.AppendLine(", DateCreated");
                sb.AppendLine(", UserId");
                sb.AppendLine(", ActionLocker");
                sb.AppendLine(", Type");
                sb.AppendLine(", Description");            
                sb.AppendLine(") VALUES (");

                sb.AppendLine(string.Format("  N'{0}'", Guid.NewGuid().ToString()));
                sb.AppendLine(string.Format(", N'{0}'", obj.Name));
                sb.AppendLine(string.Format(", '{0}'", obj.ReaderIndex));

                sb.AppendLine(string.Format(", '{0}'", obj.ControllerID));
                sb.AppendLine(string.Format(", '{0}'", obj.CardNumber));
                sb.AppendLine(string.Format(", '{0}'", obj.CardNo));
                      
                sb.AppendLine(string.Format(", '{0}'", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")));
                sb.AppendLine(string.Format(", '{0}'", GetCurrentUser.GetUser().Id));
                sb.AppendLine(string.Format(", '{0}'", actionV));
                sb.AppendLine(string.Format(", '{0}'", type));
                sb.AppendLine(string.Format(", '{0}'", message));
                sb.AppendLine(")");

                ExcuteSQL.Execute(sb.ToString());

                re.Message = "Thêm mới thành công";
                re.isSuccess = true;
            }
            catch (Exception ex)
            {
                re.Message = ex.Message;
                re.isSuccess = false;
            }

            return re;
        }
    }
}
