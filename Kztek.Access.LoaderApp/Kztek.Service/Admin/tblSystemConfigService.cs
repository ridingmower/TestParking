using Kztek.Data.Infrastructure;
using Kztek.Data.Repository;
using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kztek.Web.Core.Functions;

namespace Kztek.Service.Admin
{
    public interface ItblSystemConfigService
    {
        tblSystemConfig GetDefault();

        MessageReport Create(tblSystemConfig obj);

        MessageReport Update(tblSystemConfig obj);

        DataTable getHeaderExcel(string title1, string title2, string username);

        DataTable getHeaderExcelBVDK_ReportTotalMoneyByCardGroup(string title1, string title2, string username, int numberspace = 100);
        DataTable getHeaderExcel_BAOVIET(string title1, string title2, string username);
    }

    public class tblSystemConfigService : ItblSystemConfigService
    {
        private ItblSystemConfigRepository _tblSystemConfigRepository;
        private IUnitOfWork _UnitOfWork;

        public tblSystemConfigService(ItblSystemConfigRepository _tblSystemConfigRepository, IUnitOfWork _UnitOfWork)
        {
            this._tblSystemConfigRepository = _tblSystemConfigRepository;
            this._UnitOfWork = _UnitOfWork;
        }

        private void Save()
        {
            _UnitOfWork.Commit();
        }

        public MessageReport Create(tblSystemConfig obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblSystemConfigRepository.Add(obj);

                Save();

                re.Message = FunctionHelper.GetLocalizeDictionary("Home", "notification")["addSuccess"];
                re.isSuccess = true;
            }
            catch (Exception ex)
            {
                re.Message = ex.Message;
                re.isSuccess = false;
            }

            return re;
        }

        public tblSystemConfig GetDefault()
        {
            var query = from n in _tblSystemConfigRepository.Table
                        select n;

            return query.FirstOrDefault();
        }

        public DataTable getHeaderExcel(string title1, string title2, string username)
        {
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("header", typeof(string));
                var dtconfig = GetDefault();
                if (dtconfig != null)
                {
                    dt.Rows.Add(dtconfig.Company);
                    dt.Rows.Add(dtconfig.Address);
                    dt.Rows.Add(title1);
                    dt.Rows.Add(title2);
                    dt.Rows.Add(DictionarySearch["bookkeeper"] + " :"+ username);
                    dt.Rows.Add(DictionarySearch["approver"]);
                }

                return dt;
            }
            catch
            { }

            return null;
        }
        public DataTable getHeaderExcel_BAOVIET(string title1, string title2, string username)
        {
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("header", typeof(string));
                var dtconfig = GetDefault();
                if (dtconfig != null)
                {
                    dt.Rows.Add(title1);
                    dt.Rows.Add(dtconfig.Company);
                    dt.Rows.Add(dtconfig.Address);                   
                    dt.Rows.Add(title2);
                    dt.Rows.Add(DictionarySearch["bookkeeper"] + " :" + username);
                    dt.Rows.Add(DictionarySearch["approver"]);
                }

                return dt;
            }
            catch
            { }

            return null;
        }
        public DataTable getHeaderExcelBVDK_ReportTotalMoneyByCardGroup(string title1, string title2, string username,int numberspace = 100)
        {
            var DictionarySearch = FunctionHelper.GetLocalizeDictionary("report", "reportSearch");
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("header", typeof(string));
                var dtconfig = GetDefault();
                if (dtconfig != null)
                {                   
                    dt.Rows.Add(dtconfig.Company.PadRight(numberspace, ' ') + "CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM");
                    dt.Rows.Add(dtconfig.Address.PadRight(numberspace - 10, ' ') + "Độc lập - Tự do - Hạnh phúc");
                   
                    dt.Rows.Add("");
                    dt.Rows.Add(title1);
                    dt.Rows.Add(title2);
                }

                return dt;
            }
            catch
            { }

            return null;
        }

        public MessageReport Update(tblSystemConfig obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblSystemConfigRepository.Update(obj);

                Save();

                re.Message = FunctionHelper.GetLocalizeDictionary("Home", "notification")["updateSuccess"];
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
