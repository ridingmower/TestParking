using Kztek.Data.Infrastructure;
using Kztek.Data.Repository;
using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Web.Core.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Service.Admin
{
    public interface IOrderActiveCardService
    {
        IEnumerable<OrderActiveCard> GetAll();

        OrderActiveCard GetById(string id);

        MessageReport Create(OrderActiveCard obj);

        MessageReport Update(OrderActiveCard obj);

        MessageReport DeleteById(string id);
        List<OrderActiveCard> GetListOrder(string key, string _fromdate, string _todate, string CardGroupID, string CustomerGroupID, int pageIndex, int pageSize, ref int total);

        List<OrderActiveCard> GetListOrder_v2(string key, string _fromdate, string _todate, string CardGroupID, string CustomerGroupID, int pageIndex, int pageSize, ref int total, ref double totalMoney);

        DataTable GetListOrderExcel_PRIDE(string key, string _fromdate, string _todate, string CardGroupID, string CustomerGroupID, ref int total);

        List<OrderActiveCard_Custom> GetListOrder_PRIDE(string key, string _fromdate, string _todate, string CardGroupID, string CustomerGroupID, int pageIndex, int pageSize, ref int total, ref double totalMoney);
    }

    public class OrderActiveCardService : IOrderActiveCardService
    {
        private IOrderActiveCardRepository _OrderActiveCardRepository;
        private IUnitOfWork _UnitOfWork;

        public OrderActiveCardService(IOrderActiveCardRepository _OrderActiveCardRepository, IUnitOfWork _UnitOfWork)
        {
            this._OrderActiveCardRepository = _OrderActiveCardRepository;
            this._UnitOfWork = _UnitOfWork;
        }

        private void Save()
        {
            _UnitOfWork.Commit();
        }

        public MessageReport Create(OrderActiveCard obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _OrderActiveCardRepository.Add(obj);

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

        public MessageReport DeleteById(string id)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                var obj = GetById(id);
                if (obj != null)
                {
                    _OrderActiveCardRepository.Delete(n => n.Id.ToString() == id);

                    Save();

                    re.Message = FunctionHelper.GetLocalizeDictionary("Home", "notification")["DeleteSuccess"];
                    re.isSuccess = true;
                }
                else
                {
                    re.Message = FunctionHelper.GetLocalizeDictionary("Home", "notification")["record_does_not_exist"];
                    re.isSuccess = false;
                }
            }
            catch (Exception ex)
            {
                re.Message = ex.Message;
                re.isSuccess = false;
            }

            return re;
        }

        public IEnumerable<OrderActiveCard> GetAll()
        {
            var query = from n in _OrderActiveCardRepository.Table
                        select n;

            return query;
        }

        public OrderActiveCard GetById(string id)
        {
            return _OrderActiveCardRepository.GetById(id);
        }


        public MessageReport Update(OrderActiveCard obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _OrderActiveCardRepository.Update(obj);

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

        public List<OrderActiveCard> GetListOrder(string key, string _fromdate, string _todate, string CardGroupID, string CustomerGroupID, int pageIndex, int pageSize, ref int total)
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
            query.AppendLine("select ROW_NUMBER() OVER(ORDER BY o.[DateCreated] desc) AS RowNumber,Id,DateCreated,");
            query.AppendLine("(select SUM(FeeLevel) from tblActiveCard ac where ac.OrderId = o.Id) as Price, ");
            query.AppendLine("(select top 1 Address from tblActiveCard ac");
            query.AppendLine("left join tblCustomer cus on ac.CustomerID = CONVERT(nvarchar(50), cus.CustomerID) ");
            query.AppendLine("where ac.OrderId = o.Id) as Note ");
            query.AppendLine("from OrderActiveCard o where 1 = 1");

            query.AppendLine(string.Format("AND o.[DateCreated] >= '{0}' AND o.[DateCreated] <= '{1}'", _fromdate, _todate));

            query.AppendLine("AND o.Id IN (");

            query.AppendLine("Select distinct OrderId from tblActiveCard c");        
            query.AppendLine("left join tblCustomer cus on c.CustomerID = CONVERT(nvarchar(50), cus.CustomerID)");
            query.AppendLine("where c.IsDelete = 'False'");

            if (!string.IsNullOrEmpty(key))
            {
                if (!string.IsNullOrWhiteSpace(key))
                {
                    query.AppendLine(string.Format("and (c.CardNumber LIKE '%{0}%' or c.CardNo LIKE '%{0}%' or c.Plate LIKE '%{0}%'", key));

                    query.AppendLine(string.Format("or cus.Address LIKE N'%{0}%'", key.Trim()));

                    query.AppendLine(string.Format("or cus.AddressUnsign LIKE N'%{0}%')", key.Trim().Replace(",", "").Replace(".", "").ToLower()));
                }
            }

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
                 
            if (!string.IsNullOrWhiteSpace(CustomerGroupID))
            {
                var t = CustomerGroupID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and cus.CustomerGroupID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            query.AppendLine(")");

            query.AppendLine(") as a");
            query.AppendLine(string.Format("WHERE RowNumber BETWEEN (({0}-1) * {1} + 1) AND ({0} * {1})", pageIndex, pageSize));

            query.AppendLine("select Count(*) totalCount");         
            query.AppendLine("from OrderActiveCard o where 1 = 1");

            query.AppendLine(string.Format("AND o.[DateCreated] >= '{0}' AND o.[DateCreated] <= '{1}'", _fromdate, _todate));

            query.AppendLine("AND o.Id IN (");

            query.AppendLine("Select distinct OrderId from tblActiveCard c");
            query.AppendLine("left join tblCustomer cus on c.CustomerID = CONVERT(nvarchar(50), cus.CustomerID)");
            query.AppendLine("where c.IsDelete = 'False'");

            if (!string.IsNullOrEmpty(key))
            {
                if (!string.IsNullOrWhiteSpace(key))
                {
                    query.AppendLine(string.Format("and (c.CardNumber LIKE '%{0}%' or c.CardNo LIKE '%{0}%' or c.Plate LIKE '%{0}%'", key));

                    query.AppendLine(string.Format("or cus.Address LIKE N'%{0}%'", key.Trim()));

                    query.AppendLine(string.Format("or cus.AddressUnsign LIKE N'%{0}%')", key.Trim().Replace(",", "").Replace(".", "").ToLower()));
                }
            }

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

            if (!string.IsNullOrWhiteSpace(CustomerGroupID))
            {
                var t = CustomerGroupID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and cus.CustomerGroupID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            query.AppendLine(")");

            var list = Data.SqlHelper.ExcuteSQL.GetDataSet(query.ToString(), false);
            total = list.Tables.Count > 1 ? Convert.ToInt32(list.Tables[1].Rows[0]["totalCount"].ToString()) : 0;
            return Data.SqlHelper.ExcuteSQL.ConvertTo<OrderActiveCard>(list.Tables[0]);
        }

        public List<OrderActiveCard> GetListOrder_v2(string key, string _fromdate, string _todate, string CardGroupID, string CustomerGroupID, int pageIndex, int pageSize, ref int total,ref double totalMoney)
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
            query.AppendLine("select ROW_NUMBER() OVER(ORDER BY o.[DateCreated] desc) AS RowNumber,Id,DateCreated,");
            query.AppendLine("o.Price, ");
            query.AppendLine("(select top 1 Address from tblActiveCard ac");
            query.AppendLine("left join tblCustomer cus on ac.CustomerID = CONVERT(nvarchar(50), cus.CustomerID) ");
            query.AppendLine("where ac.OrderId = o.Id) as Note ");
            query.AppendLine("from OrderActiveCard o where 1 = 1");

            query.AppendLine(string.Format("AND o.[DateCreated] >= '{0}' AND o.[DateCreated] <= '{1}'", _fromdate, _todate));

            query.AppendLine("AND o.Id IN (");

            query.AppendLine("Select distinct OrderId from tblActiveCard c");
            query.AppendLine("left join tblCustomer cus on c.CustomerID = CONVERT(nvarchar(50), cus.CustomerID)");
            query.AppendLine("where c.IsDelete = 'False'");

            if (!string.IsNullOrEmpty(key))
            {
                if (!string.IsNullOrWhiteSpace(key))
                {
                    query.AppendLine(string.Format("and (c.CardNumber LIKE '%{0}%' or c.CardNo LIKE '%{0}%' or c.Plate LIKE '%{0}%'", key));

                    query.AppendLine(string.Format("or cus.Address LIKE N'%{0}%'", key.Trim()));

                    query.AppendLine(string.Format("or cus.AddressUnsign LIKE N'%{0}%')", key.Trim().Replace(",", "").Replace(".", "").ToLower()));
                }
            }

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

            if (!string.IsNullOrWhiteSpace(CustomerGroupID))
            {
                var t = CustomerGroupID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and cus.CustomerGroupID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            query.AppendLine(")");

            query.AppendLine(") as a");
            query.AppendLine(string.Format("WHERE RowNumber BETWEEN (({0}-1) * {1} + 1) AND ({0} * {1})", pageIndex, pageSize));

            query.AppendLine("select Count(*) totalCount,ISNULL(Sum(o.Price),0) as totalMoney");
            query.AppendLine("from OrderActiveCard o where 1 = 1");

            query.AppendLine(string.Format("AND o.[DateCreated] >= '{0}' AND o.[DateCreated] <= '{1}'", _fromdate, _todate));

            query.AppendLine("AND o.Id IN (");

            query.AppendLine("Select distinct OrderId from tblActiveCard c");
            query.AppendLine("left join tblCustomer cus on c.CustomerID = CONVERT(nvarchar(50), cus.CustomerID)");
            query.AppendLine("where c.IsDelete = 'False'");

            if (!string.IsNullOrEmpty(key))
            {
                if (!string.IsNullOrWhiteSpace(key))
                {
                    query.AppendLine(string.Format("and (c.CardNumber LIKE '%{0}%' or c.CardNo LIKE '%{0}%' or c.Plate LIKE '%{0}%'", key));

                    query.AppendLine(string.Format("or cus.Address LIKE N'%{0}%'", key.Trim()));

                    query.AppendLine(string.Format("or cus.AddressUnsign LIKE N'%{0}%')", key.Trim().Replace(",", "").Replace(".", "").ToLower()));
                }
            }

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

            if (!string.IsNullOrWhiteSpace(CustomerGroupID))
            {
                var t = CustomerGroupID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and cus.CustomerGroupID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            query.AppendLine(")");

            var list = Data.SqlHelper.ExcuteSQL.GetDataSet(query.ToString(), false);
            total = list.Tables.Count > 1 ? Convert.ToInt32(list.Tables[1].Rows[0]["totalCount"].ToString()) : 0;
            totalMoney = list.Tables.Count > 1 ? Convert.ToDouble(list.Tables[1].Rows[0]["totalMoney"].ToString()) : 0;
            return Data.SqlHelper.ExcuteSQL.ConvertTo<OrderActiveCard>(list.Tables[0]);
        }

        public List<OrderActiveCard_Custom> GetListOrder_PRIDE(string key, string _fromdate, string _todate, string CardGroupID, string CustomerGroupID, int pageIndex, int pageSize, ref int total, ref double totalMoney)
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
            query.AppendLine("select ROW_NUMBER() OVER(ORDER BY o.[DateCreated] desc) AS RowNumber,Id,DateCreated,");
            query.AppendLine("o.Price, ");

            query.AppendLine("STUFF(( SELECT '/ ' + ac.CardNo");
            query.AppendLine("FROM tblActiveCard ac");
            query.AppendLine("where ac.OrderId = o.Id and ac.IsDelete = 'False'");
            query.AppendLine("FOR XML PATH('') ), 1, 1, '') AS CardNo");
            query.AppendLine(", ");

            query.AppendLine("STUFF(( SELECT '/ ' + ac.Plate");
            query.AppendLine("FROM tblActiveCard ac");
            query.AppendLine("where ac.OrderId = o.Id and ac.IsDelete = 'False'");
            query.AppendLine("FOR XML PATH('') ), 1, 1, '') AS Plate");
            query.AppendLine(", ");

            query.AppendLine("(select top 1 Address from tblActiveCard ac");
            query.AppendLine("left join tblCustomer cus on ac.CustomerID = CONVERT(nvarchar(50), cus.CustomerID) ");
            query.AppendLine("where ac.OrderId = o.Id and ac.IsDelete = 'False') as Note ");
            query.AppendLine("from OrderActiveCard o where 1 = 1");

            query.AppendLine(string.Format("AND o.[DateCreated] >= '{0}' AND o.[DateCreated] <= '{1}'", _fromdate, _todate));

            query.AppendLine("AND o.Id IN (");

            query.AppendLine("Select distinct OrderId from tblActiveCard c");
            query.AppendLine("left join tblCustomer cus on c.CustomerID = CONVERT(nvarchar(50), cus.CustomerID)");
            query.AppendLine("where c.IsDelete = 'False'");

            if (!string.IsNullOrEmpty(key))
            {
                if (!string.IsNullOrWhiteSpace(key))
                {
                    query.AppendLine(string.Format("and (c.CardNumber LIKE '%{0}%' or c.CardNo LIKE '%{0}%' or c.Plate LIKE '%{0}%'", key));

                    query.AppendLine(string.Format("or cus.Address LIKE N'%{0}%'", key.Trim()));

                    query.AppendLine(string.Format("or cus.AddressUnsign LIKE N'%{0}%')", key.Trim().Replace(",", "").Replace(".", "").ToLower()));
                }
            }

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

            if (!string.IsNullOrWhiteSpace(CustomerGroupID))
            {
                var t = CustomerGroupID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and cus.CustomerGroupID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            query.AppendLine(")");

            query.AppendLine(") as a");
            query.AppendLine(string.Format("WHERE RowNumber BETWEEN (({0}-1) * {1} + 1) AND ({0} * {1})", pageIndex, pageSize));

            query.AppendLine("select Count(*) totalCount,ISNULL(Sum(o.Price),0) as totalMoney");
            query.AppendLine("from OrderActiveCard o where 1 = 1");

            query.AppendLine(string.Format("AND o.[DateCreated] >= '{0}' AND o.[DateCreated] <= '{1}'", _fromdate, _todate));

            query.AppendLine("AND o.Id IN (");

            query.AppendLine("Select distinct OrderId from tblActiveCard c");
            query.AppendLine("left join tblCustomer cus on c.CustomerID = CONVERT(nvarchar(50), cus.CustomerID)");
            query.AppendLine("where c.IsDelete = 'False'");

            if (!string.IsNullOrEmpty(key))
            {
                if (!string.IsNullOrWhiteSpace(key))
                {
                    query.AppendLine(string.Format("and (c.CardNumber LIKE '%{0}%' or c.CardNo LIKE '%{0}%' or c.Plate LIKE '%{0}%'", key));

                    query.AppendLine(string.Format("or cus.Address LIKE N'%{0}%'", key.Trim()));

                    query.AppendLine(string.Format("or cus.AddressUnsign LIKE N'%{0}%')", key.Trim().Replace(",", "").Replace(".", "").ToLower()));
                }
            }

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

            if (!string.IsNullOrWhiteSpace(CustomerGroupID))
            {
                var t = CustomerGroupID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and cus.CustomerGroupID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            query.AppendLine(")");

            var list = Data.SqlHelper.ExcuteSQL.GetDataSet(query.ToString(), false);
            total = list.Tables.Count > 1 ? Convert.ToInt32(list.Tables[1].Rows[0]["totalCount"].ToString()) : 0;
            totalMoney = list.Tables.Count > 1 ? Convert.ToDouble(list.Tables[1].Rows[0]["totalMoney"].ToString()) : 0;
            return Data.SqlHelper.ExcuteSQL.ConvertTo<OrderActiveCard_Custom>(list.Tables[0]);
        }

        public DataTable GetListOrderExcel_PRIDE(string key, string _fromdate, string _todate, string CardGroupID, string CustomerGroupID, ref int total)
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

            query.AppendLine("SELECT RowNumber as STT,Note as 'Địa chỉ',CardNo as 'Số thẻ',Plate as 'Biển số' ,(select convert(varchar(10), DateCreated, 103) + ' ' + left(convert(varchar(32), DateCreated, 108), 8)) as 'Ngày tạo',Price as 'Tổng tiền' FROM(");
            query.AppendLine("select ROW_NUMBER() OVER(ORDER BY o.[DateCreated] desc) AS RowNumber,Id,DateCreated,");
            query.AppendLine("o.Price, ");

            query.AppendLine("STUFF(( SELECT '/ ' + ac.CardNo");
            query.AppendLine("FROM tblActiveCard ac");
            query.AppendLine("where ac.OrderId = o.Id and ac.IsDelete = 'False'");
            query.AppendLine("FOR XML PATH('') ), 1, 1, '') AS CardNo");
            query.AppendLine(", ");

            query.AppendLine("STUFF(( SELECT '/ ' + ac.Plate");
            query.AppendLine("FROM tblActiveCard ac");
            query.AppendLine("where ac.OrderId = o.Id and ac.IsDelete = 'False'");
            query.AppendLine("FOR XML PATH('') ), 1, 1, '') AS Plate");
            query.AppendLine(", ");

            query.AppendLine("(select top 1 Address from tblActiveCard ac");
            query.AppendLine("left join tblCustomer cus on ac.CustomerID = CONVERT(nvarchar(50), cus.CustomerID) ");
            query.AppendLine("where ac.OrderId = o.Id and ac.IsDelete = 'False') as Note ");
            query.AppendLine("from OrderActiveCard o where 1 = 1");

            query.AppendLine(string.Format("AND o.[DateCreated] >= '{0}' AND o.[DateCreated] <= '{1}'", _fromdate, _todate));

            query.AppendLine("AND o.Id IN (");

            query.AppendLine("Select distinct OrderId from tblActiveCard c");
            query.AppendLine("left join tblCustomer cus on c.CustomerID = CONVERT(nvarchar(50), cus.CustomerID)");
            query.AppendLine("where c.IsDelete = 'False'");

            if (!string.IsNullOrEmpty(key))
            {
                if (!string.IsNullOrWhiteSpace(key))
                {
                    query.AppendLine(string.Format("and (c.CardNumber LIKE '%{0}%' or c.CardNo LIKE '%{0}%' or c.Plate LIKE '%{0}%'", key));

                    query.AppendLine(string.Format("or cus.Address LIKE N'%{0}%'", key.Trim()));

                    query.AppendLine(string.Format("or cus.AddressUnsign LIKE N'%{0}%')", key.Trim().Replace(",", "").Replace(".", "").ToLower()));
                }
            }

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

            if (!string.IsNullOrWhiteSpace(CustomerGroupID))
            {
                var t = CustomerGroupID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and cus.CustomerGroupID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            query.AppendLine(")");

            query.AppendLine(") as a");
           

            query.AppendLine("select Count(*) totalCount,ISNULL(Sum(o.Price),0) as totalMoney");
            query.AppendLine("from OrderActiveCard o where 1 = 1");

            query.AppendLine(string.Format("AND o.[DateCreated] >= '{0}' AND o.[DateCreated] <= '{1}'", _fromdate, _todate));

            query.AppendLine("AND o.Id IN (");

            query.AppendLine("Select distinct OrderId from tblActiveCard c");
            query.AppendLine("left join tblCustomer cus on c.CustomerID = CONVERT(nvarchar(50), cus.CustomerID)");
            query.AppendLine("where c.IsDelete = 'False'");

            if (!string.IsNullOrEmpty(key))
            {
                if (!string.IsNullOrWhiteSpace(key))
                {
                    query.AppendLine(string.Format("and (c.CardNumber LIKE '%{0}%' or c.CardNo LIKE '%{0}%' or c.Plate LIKE '%{0}%'", key));

                    query.AppendLine(string.Format("or cus.Address LIKE N'%{0}%'", key.Trim()));

                    query.AppendLine(string.Format("or cus.AddressUnsign LIKE N'%{0}%')", key.Trim().Replace(",", "").Replace(".", "").ToLower()));
                }
            }

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

            if (!string.IsNullOrWhiteSpace(CustomerGroupID))
            {
                var t = CustomerGroupID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and cus.CustomerGroupID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            query.AppendLine(")");

            var list = Data.SqlHelper.ExcuteSQL.GetDataSet(query.ToString(), false);
            total = list.Tables.Count > 1 ? Convert.ToInt32(list.Tables[1].Rows[0]["totalCount"].ToString()) : 0;
            var totalMoney = list.Tables.Count > 1 ? Convert.ToDouble(list.Tables[1].Rows[0]["totalMoney"].ToString()) : 0;
            var dt = list.Tables[0];

            dt.Rows.Add(0,"Tổng tiền","","",null, totalMoney);

            return dt;
        }
    }
}
