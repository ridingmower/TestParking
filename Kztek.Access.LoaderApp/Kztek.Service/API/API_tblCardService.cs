using Kztek.Data.Infrastructure;
using Kztek.Data.Repository;
using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kztek.Web.Core.Functions;
using System.Data;
using Kztek.Data.SqlHelper;

namespace Kztek.Service.API
{
    public interface IAPI_tblCardService
    {
        #region Api Old - dùng cho các c trình đang chạy
        IEnumerable<tblCard> GetAll();

        tblCard GetById(Guid id);

        MessageReport Create(tblCard obj);

        MessageReport Update(tblCard obj);

        MessageReport DeleteById(tblCard obj);

        tblCard GetCardByCardNumberOrCardNo(string key, bool exactSearch);
        tblCard GetByCardNumber_Id(string cardnumber);

        List<API_3rd_list_tblCard> GetAllPaging(string key, string cardgroups, string customerid, string customergroups, string active, string fromdate, string todate, int pageNumber, int pageSize, ref int TotalItem, ref int TotalPage);

        bool AddNewActiveCard(string listCardNumber, int _feelevel, string olddate, string _newexpire, string userId, bool chbEnableMinusActive, bool chbEnableDateActive = false, string dateactive = "");
        #endregion

        #region Api_new
        List<API_3rd_list_tblCard> GetAllPaging_New(string key, string cardgroups, string customerid, string customergroups, string Islock, string fromdate, string todate, int pageNumber, int pageSize, ref int TotalItem, ref int TotalPage);
        tblCardSubmit_API GetCustomById(Guid id);
        void SaveCardProcessExpend(tblCard map, string plate, string jsStrOld, string item, string v);
        void SaveCardProcess(tblCard obj, string v1, string v2);
        #endregion


    }

    public class API_tblCardService : IAPI_tblCardService
    {
        private ItblCardRepository _tblCardRepository;
        private ItblCardGroupRepository _tblCardGroupRepository;
        private ItblCustomerRepository _tblCustomerRepository;
        private IUnitOfWork _UnitOfWork;

        public API_tblCardService(ItblCardRepository _tblCardRepository, ItblCardGroupRepository _tblCardGroupRepository, ItblCustomerRepository _tblCustomerRepository, ItblCustomerGroupRepository _tblCustomerGroupRepository, ItblAccessLevelRepository _tblAccessLevelRepository, IUnitOfWork _UnitOfWork)
        {
            this._tblCustomerRepository = _tblCustomerRepository;
            this._tblCardRepository = _tblCardRepository;
            this._tblCardGroupRepository = _tblCardGroupRepository;
            this._UnitOfWork = _UnitOfWork;
        }

        private void Save()
        {
            _UnitOfWork.Commit();
        }

        public tblCard GetById(Guid id)
        {
            return _tblCardRepository.GetById(id);
        }

        public MessageReport Update(tblCard obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblCardRepository.Update(obj);

                Save();

                re.Message = FunctionHelper.GetLocalizeDictionary("Home", "notification")["updateSuccess"];
                re.isSuccess = true;

                //Lưu cardprocess
                //SaveCardProcess(obj, "CHANGE", "Chỉnh sửa từ API");
            }
            catch (Exception ex)
            {
                re.Message = ex.Message;
                re.isSuccess = false;
            }

            return re;
        }

        public MessageReport Create(tblCard obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblCardRepository.Add(obj);

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

        public void SaveCardProcess(tblCard obj, string action, string userid)
        {
            var str = string.Format("insert into tblCardProcess(Date, CardNumber, Actions, CardGroupID, UserID, CustomerID) values ('{0}', '{1}', '{2}', '{3}', N'{4}', '{5}')", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), obj.CardNumber, action, obj.CardGroupID, userid, obj.CustomerID);

            SqlExQuery<tblCardProcess>.ExcuteNone(str);
        }

        public MessageReport DeleteById(tblCard obj)
        {

            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                obj.IsDelete = true;

                _tblCardRepository.Update(obj);

                Save();

                re.Message = FunctionHelper.GetLocalizeDictionary("Home", "notification")["DeleteSuccess"];
                re.isSuccess = true;

              
            }
            catch (Exception ex)
            {
                re.Message = ex.Message;
                re.isSuccess = false;
            }

            return re;
        }

        public IEnumerable<tblCard> GetAll()
        {
            var query = from n in _tblCardRepository.Table
                        where n.IsDelete == false
                        select n;

            return query;
        }

        public tblCard GetCardByCardNumberOrCardNo(string key, bool exactSearch)
        {
            if (exactSearch)
            {
                var query = from n in _tblCardRepository.Table
                            where /*n.IsLock == false &&*/ (n.CardNumber == key || n.CardNo == key) && n.IsDelete == false
                            //orderby n.CardNo ascending
                            select n;
                return query.FirstOrDefault();
            }
            else
            {
                var query = from n in _tblCardRepository.Table
                            where /*n.IsLock == false &&*/ (n.CardNumber.Contains(key) || n.CardNo.Contains(key)) && n.IsDelete == false
                            //orderby n.CardNo ascending
                            select n;
                return query.FirstOrDefault();
            }
        }


        public tblCard GetByCardNumber_Id(string cardnumber)
        {
            var query = from n in _tblCardRepository.Table
                        where n.CardNumber == cardnumber
                        select n;

            return query.FirstOrDefault();
        }

        public List<API_3rd_list_tblCard> GetAllPaging(string key, string cardgroups, string customerid, string customergroups, string active, string fromdate, string todate, int pageNumber, int pageSize, ref int TotalItem, ref int TotalPage)
        {

            //public List<tblCardCustomViewModel> GetAllPagingByFirstParkingTSQL(string key, string cardgroups, string customerid, string customergroups, string fromdate, string todate, bool desc, string columnQuery, ref int total, string ischeckbytime = "0", int pageNumber = 1, int pageSize = 20, string accesslevelids = "", string active = "", bool isfindautocapture = false)
            //{
            var sb = new StringBuilder();
            sb.AppendLine("SELECT * FROM (");
            sb.AppendLine("Select ROW_NUMBER() OVER(ORDER BY c.DateRegister desc) AS RowNumber,");
            sb.AppendLine("CONVERT(varchar(50), c.CardID) AS 'CardID',");
            sb.AppendLine("c.CardNo,");
            sb.AppendLine("c.CardNumber,");
            sb.AppendLine("c.CardGroupID,");
            sb.AppendLine("c.CustomerID,");
            sb.AppendLine("c.ImportDate,");
            sb.AppendLine("c.DateRegister,");
            sb.AppendLine("c.ExpireDate,");
            sb.AppendLine("c.AccessExpireDate,");
            sb.AppendLine("c.AccessLevelID,");
            sb.AppendLine("c.Plate1,");
            sb.AppendLine("c.Plate2,");
            sb.AppendLine("c.Plate3,");
            sb.AppendLine("c.VehicleName1,");
            sb.AppendLine("c.VehicleName2,");
            sb.AppendLine("c.VehicleName3,");
            sb.AppendLine("c.IsLock,");
            sb.AppendLine("c.Description as DescriptionCard,");
            sb.AppendLine("cu.Description,");
            sb.AppendLine("cg.CardGroupName,");
            sb.AppendLine("cu.CustomerName,");
            sb.AppendLine("cu.CustomerCode,");
            sb.AppendLine("cu.Mobile AS 'CustomerMobile',");
            sb.AppendLine("cu.Address AS 'CustomerAddress',");
            sb.AppendLine("cu.IDNumber AS 'CustomerIDNumber',");
            sb.AppendLine("cu.CustomerGroupID,");
            sb.AppendLine("cug.CustomerGroupName");

            sb.AppendLine("FROM [dbo].[tblCard] c WITH(NOLOCK)");

            sb.AppendLine("LEFT JOIN tblCardGroup cg ON c.CardGroupID = CONVERT(varchar(50), cg.CardGroupID)");
            sb.AppendLine("LEFT JOIN tblCustomer cu ON c.CustomerID = CONVERT(varchar(50), cu.CustomerID)");
            sb.AppendLine("LEFT JOIN tblCustomerGroup cug ON cu.CustomerGroupID = CONVERT(varchar(50), cug.CustomerGroupID)");

            sb.AppendLine("WHERE 1=1 AND c.IsDelete = 0");

            //Điều kiện chính

            if (!string.IsNullOrWhiteSpace(key))
            {
                sb.AppendLine(string.Format("AND (c.CardNo LIKE '%{0}%' OR c.CardNumber LIKE '%{0}%' OR c.Plate1 LIKE '%{0}%' OR c.Plate2 LIKE '%{0}%' OR c.Plate3 LIKE '%{0}%' OR c.VehicleName1 LIKE N'%{0}%' OR c.VehicleName2 LIKE N'%{0}%' OR c.VehicleName3 LIKE N'%{0}%' OR cu.CustomerName LIKE N'%{0}%' OR cu.CustomerCode LIKE N'%{0}%' OR cu.Mobile LIKE '%{0}%' OR cu.Address LIKE '%{0}%')", key));
            }

            if (!string.IsNullOrWhiteSpace(cardgroups))
            {
                var arrCardGroup = cardgroups.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var attCG = string.Join(",", arrCardGroup);

                sb.AppendLine("AND c.CardGroupID IN ('");

                sb.AppendLine(attCG);

                sb.AppendLine("')");
            }

            if (!string.IsNullOrWhiteSpace(customergroups))
            {
                var arrCustomerGroup = customergroups.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var attCG = string.Join(",", arrCustomerGroup);

                sb.AppendLine("AND cu.CustomerGroupID IN (");

                sb.AppendLine(attCG);

                sb.AppendLine(")");
            }

            if (!string.IsNullOrWhiteSpace(customerid))
            {
                sb.AppendLine(string.Format("AND c.CustomerID = '{0}'", customerid));
            }

            //switch (ischeckbytime)
            //{
            //    case "1"://Ngày nhập thẻ
            //        if (!string.IsNullOrWhiteSpace(fromdate))
            //        {
            //            var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd");

            //            sb.AppendLine(string.Format("AND c.ImportDate >= '{0}'", fdate));
            //        }

            //        if (!string.IsNullOrWhiteSpace(todate))
            //        {
            //            var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd");

            //            //query = query.Where(n => n.ImportDate < tdate);
            //            sb.AppendLine(string.Format("AND c.ImportDate < '{0}'", tdate));
            //        }
            //        break;
            //    case "2"://Ngày hết hạn
            //        if (!string.IsNullOrWhiteSpace(fromdate))
            //        {
            //            var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd");

            //            //query = query.Where(n => n.ExpireDate >= fdate);
            //            sb.AppendLine(string.Format("AND c.ExpireDate >= '{0}'", fdate));
            //        }

            //        if (!string.IsNullOrWhiteSpace(todate))
            //        {
            //            var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd");

            //            //query = query.Where(n => n.ExpireDate < tdate);
            //            sb.AppendLine(string.Format("AND c.ExpireDate < '{0}'", tdate));
            //        }
            //        break;
            //    default:
            //        break;
            //}

            if (!string.IsNullOrWhiteSpace(active))
            {
                switch (active)
                {
                    case "0":

                        sb.AppendLine(string.Format("AND c.IsLock = {0}", "0"));

                        break;
                    case "1":

                        sb.AppendLine(string.Format("AND c.IsLock = {0}", "1"));

                        break;
                    default:
                        break;
                }
            }

            sb.AppendLine(") as a");
            sb.AppendLine(string.Format("WHERE RowNumber BETWEEN (({0}-1) * {1} + 1) AND ({0} * {1})", pageNumber, pageSize));

            //var listData = SqlExQuery<API_3rd_list_tblCard>.ExcuteQuery(sb.ToString());

            //var list = ExcuteSQLEvent.GetDataSet(sb.ToString(), false);
            //var list = SqlExQuery.GetDataSet(sb.ToString(), false);
            // var list = Data.Event.SqlHelper.SqlExQuery<API_3rd_list_tblCard>.ExcuteQuery(sb.ToString());
            var listData = SqlExQuery<API_3rd_list_tblCard>.ExcuteQuery(sb.ToString());



            //Tính tổng
            sb.Clear();
            sb.AppendLine("SELECT COUNT(*) TotalCount");

            sb.AppendLine("FROM tblCard c WITH(NOLOCK)");

            sb.AppendLine("LEFT JOIN tblCardGroup cg ON c.CardGroupID = CONVERT(varchar(50), cg.CardGroupID)");
            sb.AppendLine("LEFT JOIN tblCustomer cu ON c.CustomerID = CONVERT(varchar(50), cu.CustomerID)");
            sb.AppendLine("LEFT JOIN tblCustomerGroup cug ON cu.CustomerGroupID = CONVERT(varchar(50), cug.CustomerGroupID)");

            sb.AppendLine("WHERE 1=1 AND c.IsDelete = 0");


            //Điều kiện chính

            if (!string.IsNullOrWhiteSpace(key))
            {
                sb.AppendLine(string.Format("AND (c.CardNo LIKE '%{0}%' OR c.CardNumber LIKE '%{0}%' OR c.Plate1 LIKE '%{0}%' OR c.Plate2 LIKE '%{0}%' OR c.Plate3 LIKE '%{0}%' OR c.VehicleName1 LIKE N'%{0}%' OR c.VehicleName2 LIKE N'%{0}%' OR c.VehicleName3 LIKE N'%{0}%' OR cu.CustomerName LIKE N'%{0}%' OR cu.CustomerCode LIKE N'%{0}%' OR cu.Mobile LIKE '%{0}%' OR cu.Address LIKE '%{0}%')", key));
            }

            if (!string.IsNullOrWhiteSpace(cardgroups))
            {
                var arrCardGroup = cardgroups.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var attCG = string.Join(",", arrCardGroup);

                sb.AppendLine("AND c.CardGroupID IN ('");

                sb.AppendLine(attCG);

                sb.AppendLine("')");
            }

            if (!string.IsNullOrWhiteSpace(customergroups))
            {
                var arrCustomerGroup = customergroups.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var attCG = string.Join(",", arrCustomerGroup);

                sb.AppendLine("AND cu.CustomerGroupID IN (");

                sb.AppendLine(attCG);

                sb.AppendLine(")");
            }

            if (!string.IsNullOrWhiteSpace(customerid))
            {
                sb.AppendLine(string.Format("AND c.CustomerID = '{0}'", customerid));
            }


            //switch (ischeckbytime)
            //{
            //    case "1"://Ngày nhập thẻ
            //        if (!string.IsNullOrWhiteSpace(fromdate))
            //        {
            //            var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd");

            //            sb.AppendLine(string.Format("AND c.ImportDate >= '{0}'", fdate));
            //        }

            //        if (!string.IsNullOrWhiteSpace(todate))
            //        {
            //            var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd");

            //            //query = query.Where(n => n.ImportDate < tdate);
            //            sb.AppendLine(string.Format("AND c.ImportDate < '{0}'", tdate));
            //        }
            //        break;
            //    case "2"://Ngày hết hạn
            //        if (!string.IsNullOrWhiteSpace(fromdate))
            //        {
            //            var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd");

            //            //query = query.Where(n => n.ExpireDate >= fdate);
            //            sb.AppendLine(string.Format("AND c.ExpireDate >= '{0}'", fdate));
            //        }

            //        if (!string.IsNullOrWhiteSpace(todate))
            //        {
            //            var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd");

            //            //query = query.Where(n => n.ExpireDate < tdate);
            //            sb.AppendLine(string.Format("AND c.ExpireDate < '{0}'", tdate));
            //        }
            //        break;
            //    default:
            //        break;
            //}

            if (!string.IsNullOrWhiteSpace(active))
            {
                switch (active)
                {
                    case "0":

                        sb.AppendLine(string.Format("AND c.IsLock = {0}", "0"));

                        break;
                    case "1":

                        sb.AppendLine(string.Format("AND c.IsLock = {0}", "1"));

                        break;
                    default:
                        break;
                }
            }

            var _total = SqlExQuery<TotalPaging>.ExcuteQueryFirst(sb.ToString());
            TotalItem = _total != null ? _total.TotalCount : 0;
            TotalPage = TotalItem % pageSize > 0 ? TotalItem / pageSize + 1 : TotalItem / pageSize;
            return listData;

            //TotalItem = list2.Tables.Count > 1 ? Convert.ToInt32(list2.Tables[1].Rows[0]["totalCount"].ToString()) : 0;
            //return ExcuteSQLEvent.ConvertTo<API_3rd_list_tblCard>(list.Tables[0]);

        }

        public bool AddNewActiveCard(string listCardNumber, int _feelevel, string olddate, string _newexpire, string userId, bool chbEnableMinusActive, bool chbEnableDateActive = false, string dateactive = "")
        {
            var sb = new StringBuilder();
            sb.AppendLine("INSERT INTO tblActiveCard(Code, [Date], CardNumber, CardNo, Plate, OldExpireDate, [Days], NewExpireDate, CardGroupID, CustomerGroupID, UserID, FeeLevel, CustomerID,IsDelete)");
            sb.AppendLine("SELECT CASE WHEN cus.CustomerCode IS NULL THEN '0' ELSE cus.CustomerCode END,GETDATE(), ca.Cardnumber,ca.CardNo");
            sb.AppendLine(", CAST(CASE WHEN ca.Plate2 <> '' THEN ISNULL(ca.Plate1,'') + ';' + ISNULL(ca.Plate2,'') WHEN ca.Plate3 <> '' THEN ISNULL(ca.Plate1,'') + ';' + ISNULL(ca.Plate2,'') + ';' + ISNULL(ca.Plate3,'') WHEN ca.Plate1 IS NULL THEN '' ELSE ca.Plate1 END AS nvarchar(50)) as Plate");
            sb.AppendLine(string.Format(", '{1}', DATEDIFF(DAY, ca.[ExpireDate], '{0}')", _newexpire, olddate));
            sb.AppendLine(string.Format(", '{0}', ca.CardGroupID, CASE WHEN  cus.CustomerGroupID IS NULL THEN '0' ELSE cus.CustomerGroupID END,'{2}','{1}', CASE WHEN ca.CustomerID IS NULL THEN '0' ELSE ca.CustomerID END,0", _newexpire, _feelevel, userId));
            sb.AppendLine("from tblCard ca");
            sb.AppendLine("LEFT join tblCustomer cus on ca.CustomerID = CONVERT(varchar(255), cus.CustomerID)");
            sb.AppendLine("where ca.IsDelete = 0 and ca.IsLock=0");
            //Neu so ngay gia han <0 va neu ko check thi ko cho gia han
            if (chbEnableMinusActive == false)
            {
                sb.AppendLine(string.Format("and DATEDIFF(DAY, ca.[ExpireDate], '{0}') >=0  AND ca.[ExpireDate] <= '{0}'", _newexpire));
            }

            if (!string.IsNullOrWhiteSpace(listCardNumber))
            {
                //where in
                sb.AppendLine(string.Format(" and ca.CardNumber IN ({0})", listCardNumber));
            }

            //Update lai bang tblCard
            //sb.AppendLine("UPDATE");
            //sb.AppendLine("ca");
            //sb.AppendLine(string.Format("SET ca.ExpireDate = '{0}'", _newexpire));

            //if (chbEnableDateActive)
            //{
            //    sb.AppendLine(string.Format(", ca.DateActive = '{0}'", dateactive));
            //}

            //sb.AppendLine("FROM");
            //sb.AppendLine("tblCard AS ca");
            //sb.AppendLine("LEFT join tblCustomer c on ca.CustomerID = CONVERT(varchar(255), c.CustomerID)");
            //sb.AppendLine("WHERE IsDelete = 0 and ca.IsLock=0 ");

            //if (chbEnableMinusActive == false)
            //{
            //    sb.AppendLine(string.Format("and DATEDIFF(DAY, ca.[ExpireDate], '{0}') >=0  AND ca.[ExpireDate] <= '{0}'", _newexpire));
            //}
            //if (!string.IsNullOrWhiteSpace(listCardNumber))
            //{
            //    //where in
            //    sb.AppendLine(string.Format(" and ca.CardNumber IN ({0})", listCardNumber));
            //}

            if (chbEnableDateActive)
            {
                sb.AppendLine("insert into tblCardProcess(Date, Actions, CardGroupID, UserID, CustomerID, CardNumber)");
                sb.AppendLine(string.Format("SELECT GETDATE(),'ACTIVE', ca.CardGroupID,  '{0}', CASE WHEN ca.CustomerID IS NULL THEN '0' ELSE ca.CustomerID END , ca.Cardnumber", userId));
                sb.AppendLine("from tblCard ca");
                sb.AppendLine("LEFT join tblCustomer cus on ca.CustomerID = CONVERT(varchar(255), cus.CustomerID)");
                sb.AppendLine("where ca.IsDelete = 0 and ca.IsLock=0");
                //Neu so ngay gia han <0 va neu ko check thi ko cho gia han
                if (chbEnableMinusActive == false)
                {
                    sb.AppendLine(string.Format("and DATEDIFF(DAY, ca.[ExpireDate], '{0}') >=0  AND ca.[ExpireDate] <= '{0}'", _newexpire));
                }

                if (!string.IsNullOrWhiteSpace(listCardNumber))
                {
                    //where in
                    sb.AppendLine(string.Format(" and ca.CardNumber IN ({0})", listCardNumber));
                }
            }

            return ExcuteSQL.Execute(sb.ToString());
        }


        #region Api New
        public List<API_3rd_list_tblCard> GetAllPaging_New(string key, string cardgroups, string customerid, string customergroups, string Islock, string fromdate, string todate, int pageNumber, int pageSize, ref int TotalItem, ref int TotalPage)
        {
            var sb = new StringBuilder();
            sb.AppendLine("SELECT * FROM (");
            sb.AppendLine("Select ROW_NUMBER() OVER(ORDER BY c.DateRegister desc) AS RowNumber,");
            sb.AppendLine("CONVERT(varchar(50), c.CardID) AS 'CardID',");
            sb.AppendLine("c.CardNo,");
            sb.AppendLine("c.CardNumber,");
            sb.AppendLine("c.CardGroupID,");
            sb.AppendLine("c.CustomerID,");
            sb.AppendLine("c.ImportDate,");
            sb.AppendLine("c.DateRegister as RegisterDate,");
            sb.AppendLine("c.ExpireDate,");
            sb.AppendLine("c.AccessExpireDate,");
            sb.AppendLine("c.AccessLevelID,");
            sb.AppendLine("c.Plate1 As Plate,");
            sb.AppendLine("c.Plate2,");
            sb.AppendLine("c.Plate3,");
            sb.AppendLine("c.VehicleName1 As VehicleName,");
            sb.AppendLine("c.VehicleName2,");
            sb.AppendLine("c.VehicleName3,");
            sb.AppendLine("c.IsLock AS Active,");
            sb.AppendLine("c.Description as DescriptionCard,");
            sb.AppendLine("cu.Description,");
            sb.AppendLine("cg.CardGroupName,");
            sb.AppendLine("cu.CustomerName,");
            sb.AppendLine("cu.CustomerCode,");
            sb.AppendLine("cu.Mobile AS 'CustomerMobile',");
            sb.AppendLine("cu.Address AS 'CustomerAddress',");
            sb.AppendLine("cu.IDNumber AS 'CustomerIDNumber',");
            sb.AppendLine("cu.CustomerGroupID,");
            sb.AppendLine("cug.CustomerGroupName");

            sb.AppendLine("FROM [dbo].[tblCard] c WITH(NOLOCK)");

            sb.AppendLine("LEFT JOIN tblCardGroup cg ON c.CardGroupID = CONVERT(varchar(50), cg.CardGroupID)");
            sb.AppendLine("LEFT JOIN tblCustomer cu ON c.CustomerID = CONVERT(varchar(50), cu.CustomerID)");
            sb.AppendLine("LEFT JOIN tblCustomerGroup cug ON cu.CustomerGroupID = CONVERT(varchar(50), cug.CustomerGroupID)");

            sb.AppendLine("WHERE 1=1 AND c.IsDelete = 0");

            //Điều kiện chính

            if (!string.IsNullOrWhiteSpace(key))
            {
                sb.AppendLine(string.Format("AND (c.CardNo LIKE '%{0}%' OR c.CardNumber LIKE '%{0}%' OR c.Plate1 LIKE '%{0}%' OR c.Plate2 LIKE '%{0}%' OR c.Plate3 LIKE '%{0}%' OR c.VehicleName1 LIKE N'%{0}%' OR c.VehicleName2 LIKE N'%{0}%' OR c.VehicleName3 LIKE N'%{0}%' OR cu.CustomerName LIKE N'%{0}%' OR cu.CustomerCode LIKE N'%{0}%' OR cu.Mobile LIKE '%{0}%' OR cu.Address LIKE '%{0}%')", key));
            }

            if (!string.IsNullOrWhiteSpace(cardgroups))
            {
                var arrCardGroup = cardgroups.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var attCG = string.Join(",", arrCardGroup);
                sb.AppendLine(string.Format("AND c.CardGroupID IN ('{0}')", attCG));
            }

            if (!string.IsNullOrWhiteSpace(customergroups))
            {
                var arrCustomerGroup = customergroups.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var attCG = string.Join(",", arrCustomerGroup);
                sb.AppendLine(string.Format("AND cu.CustomerGroupID IN ('{0}')", attCG));
            }

            if (!string.IsNullOrWhiteSpace(customerid))
            {
                sb.AppendLine(string.Format("AND c.CustomerID = '{0}'", customerid));
            }

            if (!string.IsNullOrWhiteSpace(fromdate))
            {
                var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd");

                //query = query.Where(n => n.ExpireDate >= fdate);
                sb.AppendLine(string.Format("AND c.ExpireDate >= '{0}'", fdate));
            }

            if (!string.IsNullOrWhiteSpace(todate))
            {
                var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd");

                //query = query.Where(n => n.ExpireDate < tdate);
                sb.AppendLine(string.Format("AND c.ExpireDate < '{0}'", tdate));
            }

            //switch (ischeckbytime)
            //{
            //    case "1"://Ngày nhập thẻ
            //        if (!string.IsNullOrWhiteSpace(fromdate))
            //        {
            //            var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd");

            //            sb.AppendLine(string.Format("AND c.ImportDate >= '{0}'", fdate));
            //        }

            //        if (!string.IsNullOrWhiteSpace(todate))
            //        {
            //            var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd");

            //            //query = query.Where(n => n.ImportDate < tdate);
            //            sb.AppendLine(string.Format("AND c.ImportDate < '{0}'", tdate));
            //        }
            //        break;
            //    case "2"://Ngày hết hạn
            //        if (!string.IsNullOrWhiteSpace(fromdate))
            //        {
            //            var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd");

            //            //query = query.Where(n => n.ExpireDate >= fdate);
            //            sb.AppendLine(string.Format("AND c.ExpireDate >= '{0}'", fdate));
            //        }

            //        if (!string.IsNullOrWhiteSpace(todate))
            //        {
            //            var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd");

            //            //query = query.Where(n => n.ExpireDate < tdate);
            //            sb.AppendLine(string.Format("AND c.ExpireDate < '{0}'", tdate));
            //        }
            //        break;
            //    default:
            //        break;
            //}

            if (!string.IsNullOrWhiteSpace(Islock))
            {
                if (Convert.ToBoolean(Islock))
                    sb.AppendLine(string.Format("AND c.IsLock = {0}", "1"));
                else
                    sb.AppendLine(string.Format("AND c.IsLock = {0}", "0"));
            }

            sb.AppendLine(") as a");
            sb.AppendLine(string.Format("WHERE RowNumber BETWEEN (({0}-1) * {1} + 1) AND ({0} * {1})", pageNumber, pageSize));

            //var listData = SqlExQuery<API_3rd_list_tblCard>.ExcuteQuery(sb.ToString());

            //var list = ExcuteSQLEvent.GetDataSet(sb.ToString(), false);
            //var list = SqlExQuery.GetDataSet(sb.ToString(), false);
            // var list = Data.Event.SqlHelper.SqlExQuery<API_3rd_list_tblCard>.ExcuteQuery(sb.ToString());
            var listData = SqlExQuery<API_3rd_list_tblCard>.ExcuteQuery(sb.ToString());



            //Tính tổng
            sb.Clear();
            sb.AppendLine("SELECT COUNT(*) TotalCount");

            sb.AppendLine("FROM tblCard c WITH(NOLOCK)");

            sb.AppendLine("LEFT JOIN tblCardGroup cg ON c.CardGroupID = CONVERT(varchar(50), cg.CardGroupID)");
            sb.AppendLine("LEFT JOIN tblCustomer cu ON c.CustomerID = CONVERT(varchar(50), cu.CustomerID)");
            sb.AppendLine("LEFT JOIN tblCustomerGroup cug ON cu.CustomerGroupID = CONVERT(varchar(50), cug.CustomerGroupID)");

            sb.AppendLine("WHERE 1=1 AND c.IsDelete = 0");


            //Điều kiện chính

            if (!string.IsNullOrWhiteSpace(key))
            {
                sb.AppendLine(string.Format("AND (c.CardNo LIKE '%{0}%' OR c.CardNumber LIKE '%{0}%' OR c.Plate1 LIKE '%{0}%' OR c.Plate2 LIKE '%{0}%' OR c.Plate3 LIKE '%{0}%' OR c.VehicleName1 LIKE N'%{0}%' OR c.VehicleName2 LIKE N'%{0}%' OR c.VehicleName3 LIKE N'%{0}%' OR cu.CustomerName LIKE N'%{0}%' OR cu.CustomerCode LIKE N'%{0}%' OR cu.Mobile LIKE '%{0}%' OR cu.Address LIKE '%{0}%')", key));
            }

            if (!string.IsNullOrWhiteSpace(cardgroups))
            {
                var arrCardGroup = cardgroups.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var attCG = string.Join(",", arrCardGroup);
                sb.AppendLine(string.Format("AND c.CardGroupID IN ('{0}')", attCG));
            }

            if (!string.IsNullOrWhiteSpace(customergroups))
            {
                var arrCustomerGroup = customergroups.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var attCG = string.Join(",", arrCustomerGroup);
                sb.AppendLine(string.Format("AND cu.CustomerGroupID IN ('{0}')", attCG));
            }

            if (!string.IsNullOrWhiteSpace(customerid))
            {
                sb.AppendLine(string.Format("AND c.CustomerID = '{0}'", customerid));
            }

            if (!string.IsNullOrWhiteSpace(fromdate))
            {
                var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd");

                //query = query.Where(n => n.ExpireDate >= fdate);
                sb.AppendLine(string.Format("AND c.ExpireDate >= '{0}'", fdate));
            }

            if (!string.IsNullOrWhiteSpace(todate))
            {
                var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd");

                //query = query.Where(n => n.ExpireDate < tdate);
                sb.AppendLine(string.Format("AND c.ExpireDate < '{0}'", tdate));
            }

            //switch (ischeckbytime)
            //{
            //    case "1"://Ngày nhập thẻ
            //        if (!string.IsNullOrWhiteSpace(fromdate))
            //        {
            //            var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd");

            //            sb.AppendLine(string.Format("AND c.ImportDate >= '{0}'", fdate));
            //        }

            //        if (!string.IsNullOrWhiteSpace(todate))
            //        {
            //            var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd");

            //            //query = query.Where(n => n.ImportDate < tdate);
            //            sb.AppendLine(string.Format("AND c.ImportDate < '{0}'", tdate));
            //        }
            //        break;
            //    case "2"://Ngày hết hạn
            //        if (!string.IsNullOrWhiteSpace(fromdate))
            //        {
            //            var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd");

            //            //query = query.Where(n => n.ExpireDate >= fdate);
            //            sb.AppendLine(string.Format("AND c.ExpireDate >= '{0}'", fdate));
            //        }

            //        if (!string.IsNullOrWhiteSpace(todate))
            //        {
            //            var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd");

            //            //query = query.Where(n => n.ExpireDate < tdate);
            //            sb.AppendLine(string.Format("AND c.ExpireDate < '{0}'", tdate));
            //        }
            //        break;
            //    default:
            //        break;
            //}
            if (String.IsNullOrEmpty(Islock))
            {
                if (Convert.ToBoolean(Islock))
                    sb.AppendLine(string.Format("AND c.IsLock = {0}", "1"));
                else
                    sb.AppendLine(string.Format("AND c.IsLock = {0}", "0"));
            }
            

            var _total = SqlExQuery<TotalPaging>.ExcuteQueryFirst(sb.ToString());
            TotalItem = _total != null ? _total.TotalCount : 0;
            TotalPage = TotalItem % pageSize > 0 ? TotalItem / pageSize + 1 : TotalItem / pageSize;
            return listData;


        }

        public tblCardSubmit_API GetCustomById(Guid id)
        {
            var query = from n in _tblCardRepository.Table
                        join m in _tblCustomerRepository.Table on n.CustomerID equals m.CustomerID.ToString() into n_m
                        from m in n_m.DefaultIfEmpty()
                        where n.CardID == id
                        select new tblCardSubmit_API()
                        {
                            //Thẻ
                            CardID = n.CardID.ToString(),
                            CardNo = n.CardNo,
                            CardNumber = n.CardNumber,
                            CardDescription = n.Description,
                            CardGroupID = n.CardGroupID,
                            CardInActive = n.IsLock,
                            Plate1 = n.Plate1,
                            Plate2 = n.Plate2,
                            Plate3 = n.Plate3,
                            VehicleName1 = n.VehicleName1,
                            VehicleName2 = n.VehicleName2,
                            VehicleName3 = n.VehicleName3,
                            DVT = n.DVT,
                            CardType = n.CardType.ToString(),

                            //Khách hàng
                            CustomerID = n.CustomerID,
                            CustomerAddress = m.Address,
                            CustomerAvatar = m.Avatar,
                            CustomerIdentify = m.IDNumber,
                            CustomerCode = m.CustomerCode,
                            CustomerGroupID = m.CustomerGroupID,
                            CustomerMobile = m.Mobile,
                            CustomerName = m.CustomerName,
                            CompartmentId = m.CompartmentId,
                            //Ngày tháng
                            //DtpDateExpired = Convert.ToDateTime(n.ExpireDate).ToString("dd/MM/yyyy"),
                            //DtpDateRegisted = Convert.ToDateTime(n.DateRegister).ToString("dd/MM/yyyy"),
                            //DtpDateReleased = Convert.ToDateTime(n.DateRelease).ToString("dd/MM/yyyy"),
                            //DtpDateActive = Convert.ToDateTime(n.DateActive).ToString("dd/MM/yyyy"),
                            //DtpDateActive = string.Format("{0:dd.MM.yy}", n.DateActive),

                            //Dữ liệu cũ dành cho process
                            OldCardInActive = n.IsLock,
                            OldCardNo = n.CardNo,
                            OldCardNumber = n.CardNumber,
                            OldCardDescription = n.Description,
                            OldCardGroupID = n.CardGroupID,
                            OldPlate1 = n.Plate1,
                            OldPlate2 = n.Plate2,
                            OldPlate3 = n.Plate3,
                            OldCustomerID = n.CustomerID,
                            OldCustomerAddress = m.Address,
                            OldCustomerIdentify = m.IDNumber,
                            OldCustomerAvatar = m.Avatar,
                            OldCustomerCode = m.CustomerCode,
                            OldCustomerGroupID = m.CustomerGroupID,
                            OldCustomerMobile = m.Mobile,

                            OldCustomerName = m.CustomerName,


                            AccessLevelID = n.AccessLevelID,
                            OldAccessLevelID = n.AccessLevelID,

                            IsAutoCapture = n.isAutoCapture
                            //Ngày giờ
                            //OldDtpDateRegisted = Convert.ToDateTime(n.DateRegister).ToString("dd/MM/yyyy"),
                            //OldDtpDateReleased = Convert.ToDateTime(n.DateRelease).ToString("dd/MM/yyyy"),
                        };

            return query.FirstOrDefault();
        }

        public void SaveCardProcessExpend(tblCard obj, string plate, string jsonobj, string action, string userid)
        {
            var str = string.Format("insert into tblCardProcess(Date, CardNumber, Actions, CardGroupID, UserID, CustomerID,Plates,OldInfoCP) values ('{0}', '{1}', '{2}', '{3}', N'{4}', '{5}','{6}',N'{7}')", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), obj.CardNumber, action, obj.CardGroupID, userid, obj.CustomerID, plate, jsonobj);

            SqlExQuery<tblCardProcess>.ExcuteNone(str);
        }

      


        #endregion
    }
}

