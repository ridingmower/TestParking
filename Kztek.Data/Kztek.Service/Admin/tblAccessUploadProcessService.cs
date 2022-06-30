using Kztek.Data.Infrastructure;
using Kztek.Data.Repository;
using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Models;
using Kztek.Web.Core.Helpers;
using PagedList;
using System;
using System.Linq;
using System.Collections.Generic;
using Kztek.Model.CustomModel.iAccess;
using System.Text;
using Kztek.Data.SqlHelper;
using System.Data;

namespace Kztek.Service.Admin
{
    public interface ItblAccessUploadProcessService
    {
        IQueryable<tblAccessUploadProcess> GetAll();
        IPagedList<tblAccessUploadProcess> GetAllPagingByFirst(string key, string fromdate, string todate, string cardgroupids, string customergroups, string actionvs, string users, int pageNumber, int pageSize);
        List<ReporttblAccessUploadProcess> GetReportUploadProcessDetail(string KeyWord, List<string> customerGroupId, string _fromdate, string _todate, string CardGroupID, string Actions, string UserID, int pageIndex, int pageSize, ref int total, string type = "", string eventstatus = "");
        DataTable GetReportUploadProcessDetailExcel(string KeyWord, List<string> customerGroupId, string _fromdate, string _todate, string CardGroupID, string Actions, string UserID, string type = "", string eventstatus = "");
        tblAccessUploadProcess GetById(int id);
        MessageReport Create(tblAccessUploadProcess obj);
        MessageReport Update(tblAccessUploadProcess obj);
        MessageReport DeleteById(int id);

        MessageReport SaveProcess(Employee emp, SelectListModelUploadSubmit obj);
    }

    public class tblAccessUploadProcessService : ItblAccessUploadProcessService
    {
        private readonly ItblAccessUploadProcessRepository _tblAccessUploadProcessRepository;
        private readonly ItblCardService _tblCardService;
        private readonly ItblCustomerService _tblCustomerService;
        private readonly IUnitOfWork _UnitOfWork;
        public tblAccessUploadProcessService(ItblAccessUploadProcessRepository _tblAccessUploadProcessRepository, ItblCardService _tblCardService, ItblCustomerService _tblCustomerService, IUnitOfWork _UnitOfWork)
        {
            this._tblAccessUploadProcessRepository = _tblAccessUploadProcessRepository;
            this._tblCardService = _tblCardService;
            this._tblCustomerService = _tblCustomerService;
            this._UnitOfWork = _UnitOfWork;
        }

        public IQueryable<tblAccessUploadProcess> GetAll()
        {
            var query = from n in _tblAccessUploadProcessRepository.Table select n;
            return query;
        }
        public IPagedList<tblAccessUploadProcess> GetAllPagingByFirst(string key, string fromdate, string todate, string cardgroupids, string customergroups, string actionvs, string users, int pageNumber, int pageSize)
        {
            var query = from n in _tblAccessUploadProcessRepository.Table
                        select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.CardNumber.Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(cardgroupids))
            {
                query = query.Where(n => cardgroupids.Contains(n.CardGroupID));
            }

            if (!string.IsNullOrWhiteSpace(customergroups))
            {
                query = query.Where(n => customergroups.Contains(n.CustomerGroupID));
            }

            if (!string.IsNullOrWhiteSpace(actionvs))
            {
                query = query.Where(n => actionvs.Contains(n.Actions));
            }

            if (!string.IsNullOrWhiteSpace(users))
            {
                query = query.Where(n => users.Contains(n.UserID));
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                var fdate = Convert.ToDateTime(fromdate);
                var tdate = Convert.ToDateTime(todate).AddDays(1);

                query = query.Where(n => n.Date >= fdate && n.Date < tdate);
            }
            else
            {
                var fdate = Convert.ToDateTime(DateTime.Now);
                var tdate = Convert.ToDateTime(DateTime.Now).AddDays(1);

                query = query.Where(n => n.Date >= fdate && n.Date < tdate);
            }

            var list = new PagedList<tblAccessUploadProcess>(query.OrderBy(n => n.Date), pageNumber, pageSize);
            return list;
        }

        public tblAccessUploadProcess GetById(int id)
        {
            return _tblAccessUploadProcessRepository.GetById(id);
        }

        public MessageReport Create(tblAccessUploadProcess obj)
        {
            MessageReport report;
            try
            {
                _tblAccessUploadProcessRepository.Add(obj);
                Save();
                report = new MessageReport(true, "Thêm thành công");
            }
            catch (Exception ex)
            {
                report = new MessageReport(false, ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
            }
            return report;
        }

        public MessageReport Update(tblAccessUploadProcess obj)
        {
            MessageReport report;
            try
            {
                _tblAccessUploadProcessRepository.Update(obj);
                Save();
                report = new MessageReport(true, "Cập nhật thành công");
            }
            catch (Exception ex)
            {
                report = new MessageReport(false, ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
            }
            return report;
        }

        public MessageReport DeleteById(int id)
        {
            MessageReport report;
            try
            {
                var obj = _tblAccessUploadProcessRepository.GetById(id);
                if (obj != null)
                {
                    _tblAccessUploadProcessRepository.Delete(obj);
                    Save();
                    report = new MessageReport(true, "Xóa thông tin thành công");
                }
                else
                {
                    report = new MessageReport(false, "Thông tin này không tồn tại");
                }
            }
            catch (Exception ex)
            {
                report = new MessageReport(false, ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
            }
            return report;
        }

        //Save change
        public void Save()
        {
            _UnitOfWork.Commit();
        }

        public MessageReport SaveProcess(Employee emp, SelectListModelUploadSubmit obj)
        {
            //
            var user = GetCurrentUser.GetUser();

            //
            var cardgroupid = "";
            var userId = user != null ? user.Id : "";
            var customerid = "";
            var customergroupid = "";
            var controllerid = obj.controllerid;
            var controllerids = emp.ControllerIDs;
            var expiredate = "2099/12/31";
            var desc = obj.desc;

            //Lấy nhóm thẻ
            if (emp.CardNumber != "0")
            {
                var objCard = _tblCardService.GetCustomByCardNumber(emp.CardNumber);
                if (objCard != null)
                {
                    cardgroupid = objCard.CardGroupID;
                    expiredate = objCard.AccessExpireDate.ToString("yyyy/MM/dd");
                    customerid = objCard.CustomerID;
                    customergroupid = objCard.CustomerGroupID;

                    if (obj.isusenewdate)
                    {
                        _tblCardService.UpdateCard(obj.actionV, userId, emp.CardNumber, emp.ExpireDate, true);
                    }
                }
            }

            if (emp.UserIDofFinger > 0)
            {
                //Lấy khách hàng
                var objCustomer = _tblCustomerService.GetByFingerID(emp.UserIDofFinger);
                if (objCustomer != null)
                {
                    if (obj.isusenewdate)
                    {
                        _tblCustomerService.UpdateCustomer(emp.UserIDofFinger.ToString(), emp.ExpireDate, true);
                    }
                }
            }

            var result = new MessageReport(false, "Có lỗi xảy ra");

            try
            {
                var str = new StringBuilder();
                str.AppendLine("INSERT INTO tblAccessUploadProcess(Date, CardNumber, UserIDofFinger, Actions, CardGroupID, UserID, AccessLevelID, CustomerID, CustomerGroupID, SuccessControllerIDs, TotalControllerIDs, EventType, AccessDateExpire, Description) VALUES (");

                str.AppendLine("GETDATE()");
                str.AppendLine(string.Format(", '{0}'", emp.CardNumber));
                str.AppendLine(string.Format(", '{0}'", emp.UserIDofFinger));
                str.AppendLine(string.Format(", '{0}'", obj.actionV));
                str.AppendLine(string.Format(", '{0}'", cardgroupid));
                str.AppendLine(string.Format(", '{0}'", userId));
                str.AppendLine(string.Format(", '{0}'", emp.AccessLevelID));
                str.AppendLine(string.Format(", '{0}'", customerid));
                str.AppendLine(string.Format(", '{0}'", customergroupid));
                str.AppendLine(string.Format(", '{0}'", controllerid));
                str.AppendLine(string.Format(", '{0}'", controllerids));
                str.AppendLine(string.Format(", '{0}'", obj.eventtype));
                str.AppendLine(string.Format(", '{0}'", expiredate));
                str.AppendLine(string.Format(", N'{0}'", desc));

                str.AppendLine(")");

                var t = ExcuteSQL.Execute(str.ToString());

                result.isSuccess = t;
                result.Message = "Thêm mới thành công";

                if (obj.isusenewdate && emp.CardNumber != "0")
                {
                    var str1 = new StringBuilder();

                    str1.AppendLine("INSERT INTO tblAccessUploadProcess(Date, CardNumber, UserIDofFinger, Actions, CardGroupID, UserID, AccessLevelID, CustomerID, CustomerGroupID, SuccessControllerIDs, TotalControllerIDs, EventType, AccessDateExpire, Description) VALUES (");

                    str1.AppendLine("GETDATE()");
                    str1.AppendLine(string.Format(", '{0}'", emp.CardNumber));
                    str1.AppendLine(string.Format(", '{0}'", emp.UserIDofFinger));
                    str1.AppendLine(string.Format(", '{0}'", "EXTEND"));
                    str1.AppendLine(string.Format(", '{0}'", cardgroupid));
                    str1.AppendLine(string.Format(", '{0}'", userId));
                    str1.AppendLine(string.Format(", '{0}'", emp.AccessLevelID));
                    str1.AppendLine(string.Format(", '{0}'", customerid));
                    str1.AppendLine(string.Format(", '{0}'", customergroupid));
                    str1.AppendLine(string.Format(", '{0}'", controllerid));
                    str1.AppendLine(string.Format(", '{0}'", controllerids));
                    str1.AppendLine(string.Format(", '{0}'", obj.eventtype));
                    str1.AppendLine(string.Format(", '{0}'", expiredate));
                    str1.AppendLine(string.Format(", N'{0}'", desc));

                    str1.AppendLine(")");

                    ExcuteSQL.Execute(str1.ToString());
                }

            }
            catch (Exception ex)
            {
                result.isSuccess = false;
                result.Message = ex.Message;
            }

            return result;
        }

        public List<ReporttblAccessUploadProcess> GetReportUploadProcessDetail(string KeyWord, List<string> customerGroupId, string _fromdate, string _todate, string CardGroupID, string Actions, string UserID, int pageIndex, int pageSize, ref int total, string type = "", string eventstatus = "")
        {
            if (!string.IsNullOrEmpty(_fromdate))
            {
                _fromdate = Convert.ToDateTime(_fromdate).ToString("yyyy/MM/dd HH:mm:ss");
            }
            if (!string.IsNullOrEmpty(_todate))
            {
                _todate = Convert.ToDateTime(_todate).ToString("yyyy/MM/dd HH:mm:ss");
            }

            var query = new StringBuilder();

            query.AppendLine("SELECT * FROM(");
            //tblCardProcess
            query.AppendLine("SELECT ROW_NUMBER() OVER(ORDER BY [Date] desc) AS RowNumber, cu.CustomerName, cu.[Address], cu.[CustomerGroupID], c.[CardGroupID], c.[Date], c.[UserID], c.[Actions], c.[UserIDofFinger], c.[SuccessControllerIDs], c.[CardNumber], c.[EventType], c.[AccessDateExpire]");
            query.AppendLine("FROM dbo.[tblAccessUploadProcess] c WITH (NOLOCK)");
            //query.AppendLine("LEFT JOIN tblCardGroup cg ON c.CardGroupID = CONVERT(nvarchar(255), cg.CardGroupID)");
            //query.AppendLine("LEFT JOIN tblUser u on c.UserID = CONVERT(nvarchar(255), u.UserID)");
            //query.AppendLine("LEFT JOIN tblCard ca ON c.CardNumber = ca.CardNumber AND ca.IsDelete = 0");
            query.AppendLine("LEFT JOIN tblCustomer cu ON c.CustomerID = CONVERT(nvarchar(255),cu.CustomerID)");
            query.AppendLine("WHERE 1 = 1");
            query.AppendLine(string.Format("AND c.[Date] >= '{0}' AND c.[Date] <= '{1}'", _fromdate, _todate));

            //Nhom the
            if (!string.IsNullOrWhiteSpace(CardGroupID))
            {
                var t = CardGroupID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and c.CardGroupID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            if (!string.IsNullOrWhiteSpace(Actions))
                query.AppendLine(string.Format("AND c.[Actions] = '{0}'", Actions));
            //User
            if (!string.IsNullOrWhiteSpace(UserID))
            {
                var t = UserID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and c.UserID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            //Nhom KH
            if (customerGroupId.Any())
            {
                query.AppendLine("AND cu.CustomerGroupID IN (");

                var count = 0;
                foreach (var item in customerGroupId)
                {
                    count++;
                    query.AppendLine(string.Format("'{0}'{1}", item, count == customerGroupId.Count ? "" : ","));
                }

                query.AppendLine(")");
            }

            if (!string.IsNullOrWhiteSpace(KeyWord))
                query.AppendLine(string.Format("AND ( c.[CardNumber] LIKE '%{0}%' OR c.[Description] LIKE '%{0}%' )", KeyWord));
            if (!string.IsNullOrWhiteSpace(type))
            {
                query.AppendLine(string.Format("AND c.[EventType] LIKE '%{0}%'", type));
            }
            if (!string.IsNullOrWhiteSpace(eventstatus))
            {
                query.AppendLine(string.Format("AND c.[Description] LIKE '%{0}%'", eventstatus));
            }

            query.AppendLine(") as a");
            query.AppendLine(string.Format("WHERE RowNumber BETWEEN (({0}-1) * {1} + 1) AND ({0} * {1})", pageIndex, pageSize));

            //--COUNT TOTAL
            query.AppendLine("SELECT COUNT(Id) totalCount");
            query.AppendLine("FROM dbo.[tblAccessUploadProcess] c WITH (NOLOCK)");
            //query.AppendLine("LEFT JOIN tblCardGroup cg ON c.CardGroupID = CONVERT(nvarchar(255), cg.CardGroupID)");
            //query.AppendLine("LEFT JOIN tblUser u on c.UserID = CONVERT(nvarchar(255), u.UserID)");
            //query.AppendLine("LEFT JOIN tblCard ca ON c.CardNumber = ca.CardNumber AND ca.IsDelete = 0");
            query.AppendLine("LEFT JOIN tblCustomer cu ON c.CustomerID = CONVERT(nvarchar(255),cu.CustomerID)");
            query.AppendLine("WHERE 1 = 1");
            query.AppendLine(string.Format("AND c.[Date] >= '{0}' AND c.[Date] <= '{1}'", _fromdate, _todate));

            //Nhom the
            if (!string.IsNullOrWhiteSpace(CardGroupID))
            {
                var t = CardGroupID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and c.CardGroupID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            if (!string.IsNullOrWhiteSpace(Actions))
                query.AppendLine(string.Format("AND c.[Actions] = '{0}'", Actions));
            //User
            if (!string.IsNullOrWhiteSpace(UserID))
            {
                var t = UserID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and c.UserID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            //Nhom KH
            if (customerGroupId.Any())
            {
                query.AppendLine("AND cu.CustomerGroupID IN (");

                var count = 0;
                foreach (var item in customerGroupId)
                {
                    count++;
                    query.AppendLine(string.Format("'{0}'{1}", item, count == customerGroupId.Count ? "" : ","));
                }

                query.AppendLine(")");
            }

            if (!string.IsNullOrWhiteSpace(KeyWord))
                query.AppendLine(string.Format("AND ( c.[CardNumber] LIKE '%{0}%' OR c.[Description] LIKE '%{0}%' )", KeyWord));
            if (!string.IsNullOrWhiteSpace(type))
            {
                query.AppendLine(string.Format("AND c.[EventType] LIKE '%{0}%'", type));
            }
            if (!string.IsNullOrWhiteSpace(eventstatus))
            {
                query.AppendLine(string.Format("AND c.[Description] LIKE '%{0}%'", eventstatus));
            }

            var list = ExcuteSQL.GetDataSet(query.ToString(), false);

            total = list.Tables.Count > 1 ? Convert.ToInt32(list.Tables[1].Rows[0]["totalCount"].ToString()) : 0;

            return ExcuteSQL.ConvertTo<ReporttblAccessUploadProcess>(list.Tables[0]);
        }

        public DataTable GetReportUploadProcessDetailExcel(string KeyWord, List<string> customerGroupId, string _fromdate, string _todate, string CardGroupID, string Actions, string UserID, string type = "", string eventstatus = "")
        {
            if (!string.IsNullOrEmpty(_fromdate))
            {
                _fromdate = Convert.ToDateTime(_fromdate).ToString("yyyy/MM/dd HH:mm:ss");
            }
            if (!string.IsNullOrEmpty(_todate))
            {
                _todate = Convert.ToDateTime(_todate).ToString("yyyy/MM/dd HH:mm:ss");
            }

            var query = new StringBuilder();

            query.AppendLine("SELECT * FROM(");
            //tblCardProcess
            query.AppendLine("SELECT ROW_NUMBER() OVER(ORDER BY [Date] desc) AS STT, (select convert(varchar(10), c.[Date], 103) + ' ' + left(convert(varchar(32), c.[Date], 108), 8)) as 'Thời gian', c.CardNumber as 'Mã thẻ', c.[UserIDofFinger] as 'User trên tb', cg.CardGroupName as 'Nhóm thẻ', c.Actions as 'Hành vi', cu.CustomerName as 'Chủ thẻ', '' as 'Nhóm KH',cu.[Address] as 'Địa chỉ', u.UserName as 'NV thực hiện', c.[SuccessControllerIDs] as 'Thiết bị', cu.[CustomerGroupID], c.[EventType], (select convert(varchar(10), c.[AccessDateExpire], 103))  AS 'Hết hạn'");
            query.AppendLine("FROM dbo.[tblAccessUploadProcess] c WITH (NOLOCK)");
            query.AppendLine("LEFT JOIN tblCardGroup cg ON c.CardGroupID = CONVERT(nvarchar(255), cg.CardGroupID)");
            query.AppendLine("LEFT JOIN tblUser u on c.UserID = CONVERT(nvarchar(255), u.UserID)");
            //query.AppendLine("LEFT JOIN tblCard ca ON c.CardNumber = ca.CardNumber AND ca.IsDelete = 0");
            query.AppendLine("LEFT JOIN tblCustomer cu ON c.CustomerID = CONVERT(nvarchar(255),cu.CustomerID)");
            query.AppendLine("WHERE 1 = 1");
            query.AppendLine(string.Format("AND c.[Date] >= '{0}' AND c.[Date] <= '{1}'", _fromdate, _todate));

            //Nhom the
            if (!string.IsNullOrWhiteSpace(CardGroupID))
            {
                var t = CardGroupID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and c.CardGroupID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            if (!string.IsNullOrWhiteSpace(Actions))
                query.AppendLine(string.Format("AND c.[Actions] = '{0}'", Actions));

            //User
            if (!string.IsNullOrWhiteSpace(UserID))
            {
                var t = UserID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and c.UserID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            //Nhom KH
            if (customerGroupId.Any())
            {
                query.AppendLine("AND cu.CustomerGroupID IN (");

                var count = 0;
                foreach (var item in customerGroupId)
                {
                    count++;
                    query.AppendLine(string.Format("'{0}'{1}", item, count == customerGroupId.Count ? "" : ","));
                }

                query.AppendLine(")");
            }
            if (!string.IsNullOrWhiteSpace(KeyWord))
                query.AppendLine(string.Format("AND ( c.[CardNumber] LIKE '%{0}%' OR c.[Description] LIKE '%{0}%' )", KeyWord));

            if (!string.IsNullOrWhiteSpace(type))
            {
                query.AppendLine(string.Format("AND c.[EventType] LIKE '%{0}%'", type));
            }
            if (!string.IsNullOrWhiteSpace(eventstatus))
            {
                query.AppendLine(string.Format("AND c.[Description] LIKE '%{0}%'", eventstatus));
            }

            query.AppendLine(") as a");

            return ExcuteSQL.GetDataSet(query.ToString(), false).Tables[0];
        }
    }
}
