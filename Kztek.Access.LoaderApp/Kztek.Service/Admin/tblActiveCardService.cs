
using Kztek.Data.Infrastructure;
using Kztek.Data.Repository;
using Kztek.Data.SqlHelper;
using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Web.Core.Functions;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Service.Admin
{
    public interface ItblActiveCardService
    {
        IPagedList<tblActiveCardCustomViewModel> GetAllPagingByFirst(string key, string cardno, string fromdate, string todate, string cardgroupids, string customergroupids, string users, int pageNumber, int pageSize);
        //dùng cho trường chinh
        IPagedList<tblActiveCardCustomViewModel> GetAllPagingByFirstTRANSERCO(string key,string typepay, string cardno, string fromdate, string todate, string cardgroupids, string customergroupids, string users, int pageNumber, int pageSize);

        List<tblActiveCard_Excel> ReportAllByFirst(string key, string cardno, string fromdate, string todate, string cardgroupids, string customergroupids, string users);

        //dùng cho trường chinh
        List<tblActiveCard_ExcelTRANSERCO> ReportAllByFirstTRANSERCO(string key, string typepay, string cardno, string fromdate, string todate, string cardgroupids, string customergroupids, string users);
        List<tblActiveCard_Excel> ReportAllByFirstApec(string key, string typepay, string cardno, string fromdate, string todate, string cardgroupids, string customergroupids, string users);
        tblActiveCard GetById(string id);
        MessageReport DeleteById(string id);
        //bill
        List<tblActiveCardCustomViewModel> GetBill(string cardnumbers);
        List<tblActiveCardCustomViewModel> GetBill_v2(string orderId);

        string GetOrderIdByCardNumbers(string cardnumbers);

        //dùng cho PRIDE
        List<tblActiveCard_ExcelPRIDE> ReportAllByFirst_PRIDE(string key, string cardno, string fromdate, string todate, string cardgroupids, string customergroupids, string users);

        IPagedList<tblActiveCardCustomViewModel> GetAllPagingByFirst_PRIDE(string key, string cardno, string fromdate, string todate, string cardgroupids, string customergroupids, string users, int pageNumber, int pageSize);

        string GetTotalMoney(string fromdate, string todate);

        string GetTotalMoneyActiveCard(string key, string cardgroupids, string customergroupids, string users, string fromdate, string todate);
        int GetCountByOrderId(string orderId);

        List<ExtendList> GetDetailExtend(string keyword, bool IsFilterByTime, string _fromdate, string _todate, string CardGroupID, string CustomerID, List<string> CustomerGroupID, string UserID, int pageIndex, int pageSize, ref int total, ref long _totalmoneys);
        DataTable GetDetailExtend_Excel(string keyword, bool IsFilterByTime, string _fromdate, string _todate, string CardGroupID, string CustomerID, List<string> CustomerGroupID, string UserID);
        MessageReport Update(tblActiveCard obj);
        void DeleteExtendBySubId(string subid);
    }

    public class tblActiveCardService : ItblActiveCardService
    {
        private ItblActiveCardRepository _tblActiveCardRepository;
        private IExtendCardRepository _ExtendCardRepository;
        private ItblCardGroupRepository _tblCardGroupRepository;
        private ItblCustomerGroupRepository _tblCustomerGroupRepository;
        private ItblCustomerRepository _tblCustomerRepository;
        private IUserRepository _UserRepository;
        private ItblCustomerService _tblCustomerService;
        private IUnitOfWork _UnitOfWork;

        private IUser_AuthGroupService _User_AuthGroupService;
        private string AuthCardGroupIds = "";

        public tblActiveCardService(ItblActiveCardRepository _tblActiveCardRepository, ItblCardGroupRepository _tblCardGroupRepository, ItblCustomerGroupRepository _tblCustomerGroupRepository, IUserRepository _UserRepository, ItblCustomerService _tblCustomerService, ItblCustomerRepository _tblCustomerRepository, IUser_AuthGroupService _User_AuthGroupService, IExtendCardRepository _ExtendCardRepository, IUnitOfWork _UnitOfWork)
        {
            this._tblActiveCardRepository = _tblActiveCardRepository;
            this._ExtendCardRepository = _ExtendCardRepository;
            this._tblCardGroupRepository = _tblCardGroupRepository;
            this._tblCustomerGroupRepository = _tblCustomerGroupRepository;
            this._tblCustomerRepository = _tblCustomerRepository;
            this._UserRepository = _UserRepository;
            this._tblCustomerService = _tblCustomerService;
            this._UnitOfWork = _UnitOfWork;

            this._User_AuthGroupService = _User_AuthGroupService;

            AuthCardGroupIds = _User_AuthGroupService.GetAuthCardGroupIds();
        }

        public IPagedList<tblActiveCardCustomViewModel> GetAllPagingByFirst(string key, string cardno, string fromdate, string todate, string cardgroupids, string customergroupids, string users, int pageNumber, int pageSize)
        {
            var query = from n in _tblActiveCardRepository.Table
                        join m in _tblCardGroupRepository.Table on n.CardGroupID equals m.CardGroupID.ToString() into n_m
                        from m in n_m.DefaultIfEmpty()
                        join g in _tblCustomerGroupRepository.Table on n.CustomerGroupID equals g.CustomerGroupID.ToString() into m_g
                        from g in m_g.DefaultIfEmpty()
                        join u in _UserRepository.Table on n.UserID equals u.Id into n_u
                        from u in n_u.DefaultIfEmpty()
                        where n.IsDelete == false

                        select new tblActiveCardCustomViewModel()
                        {
                            CardGroupID = n.CardGroupID,
                            CardGroupName = m.CardGroupName,
                            CardNo = n.CardNo,
                            CardNumber = n.CardNumber,
                            CustomerGroupID = n.CustomerGroupID,
                            CustomerGroupName = g.CustomerGroupName,
                            CustomerID = n.CustomerID,
                            Date = n.Date,
                            Days = n.Days,
                            FeeLevel = n.FeeLevel,
                            IsDelete = n.IsDelete,
                            NewExpireDate = n.NewExpireDate,
                            OldExpireDate = n.OldExpireDate,
                            Plate = n.Plate,
                            UserID = u.Id,
                            Id = n.Id,
                            Code = n.Code,
                            UserName = u.Username
                        };

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.CardNumber.Contains(key) || n.Plate.Contains(key) || n.CardNo.Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(cardgroupids))
            {
                query = query.Where(n => cardgroupids.Contains(n.CardGroupID));
            }

            if (!string.IsNullOrEmpty(AuthCardGroupIds))
            {
                var lst = AuthCardGroupIds.Split(';');
                query = query.Where(n => lst.Contains(n.CardGroupID.ToString()));
            }

            if (!string.IsNullOrWhiteSpace(customergroupids))
            {
                query = query.Where(n => customergroupids.Contains(n.CustomerGroupID));
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
                var time = DateTime.Now;
                var fdate = Convert.ToDateTime(time.ToString("dd/MM/yyyy 00:00:00"));
                var tdate = Convert.ToDateTime(time.AddDays(1).ToString("dd/MM/yyyy 00:00:00"));

                query = query.Where(n => n.Date >= fdate && n.Date < tdate);
            }

            var list = new PagedList<tblActiveCardCustomViewModel>(query.OrderByDescending(n => n.Date), pageNumber, pageSize);

            return list;
        }

        public List<ExtendList> GetDetailExtend(string keyword,bool IsFilterByTime, string _fromdate, string _todate, string CardGroupID, string CustomerID, List<string> CustomerGroupID, string UserID, int pageIndex, int pageSize, ref int total, ref long _totalmoneys)
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
            query.AppendLine("SELECT ROW_NUMBER() OVER(ORDER BY ac.DateCreated desc) AS RowNumber, Date,CardNo, CardNumber, CardGroupID, Plate, c.CustomerName as CustomerID, cg.CustomerGroupName as CustomerGroupID, OldExpireDate, NewExpireDate, FeeLevel, u.Username as UserID,c.Address,ac.DateCreated ");
            query.AppendLine(" FROM ExtendCard ac WITH (NOLOCK) ");
            query.AppendLine(" LEFT JOIN tblCustomer c ON ac.CustomerID = CONVERT(nvarchar(50), c.CustomerID)");
            query.AppendLine(" LEFT JOIN tblCustomerGroup cg ON ac.CustomerGroupID = CONVERT(nvarchar(50), cg.CustomerGroupID)");
            query.AppendLine(" LEFT JOIN dbo.[User] u ON ac.UserID = u.Id");
            query.AppendLine("WHERE IsDelete = 0");

            query.AppendLine(string.Format("{0}", IsFilterByTime ? string.Format("AND ac.[DateCreated] >= '{0}' AND ac.[DateCreated] < '{1}' ", _fromdate, _todate) : string.Format("AND ac.[Date] >= '{0}' AND ac.[Date] < '{1}' ", _fromdate, _todate)));

          //  query.AppendLine(string.Format("AND ac.DateCreated >= '{0}' AND ac.DateCreated < '{1}'", _fromdate, _todate));

            if (!string.IsNullOrWhiteSpace(keyword))
                query.AppendLine(string.Format("AND (Plate LIKE '%{0}%' OR CardNumber LIKE '%{0}%' OR CardNo LIKE '%{0}%')", keyword));



            //if (!string.IsNullOrWhiteSpace(CustomerID))
            //    query.AppendLine(string.Format("AND CustomerID = '{0}'", CustomerID));

            if (CustomerGroupID.Any())
            {
                query.AppendLine("AND ac.CustomerGroupID IN (");

                var count = 0;
                foreach (var item in CustomerGroupID)
                {
                    count++;
                    query.AppendLine(string.Format("'{0}'{1}", item, count == CustomerGroupID.Count ? "" : ","));
                }

                query.AppendLine(")");
            }

            if (!string.IsNullOrWhiteSpace(CardGroupID))
            {
                var t = CardGroupID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and CardGroupID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }
            if (!string.IsNullOrWhiteSpace(AuthCardGroupIds))
            {
                var t = AuthCardGroupIds.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and CardGroupID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            if (!string.IsNullOrWhiteSpace(UserID))
            {
                var t = UserID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and UserID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            query.AppendLine(") as a");
            query.AppendLine(string.Format("WHERE RowNumber BETWEEN (({0}-1) * {1} + 1) AND ({0} * {1})", pageIndex, pageSize));

            //--COUNT TOTAL
            query.AppendLine("SELECT COUNT(*) totalCount,SUM(FeeLevel) AS totalMoney FROM (");
            query.AppendLine("SELECT ac.Id,ac.FeeLevel");
            query.AppendLine(" FROM ExtendCard ac WITH (NOLOCK) ");
            query.AppendLine(" LEFT JOIN tblCustomer c ON ac.CustomerID = CONVERT(nvarchar(50), c.CustomerID) ");
            query.AppendLine(" LEFT JOIN tblCustomerGroup cg ON ac.CustomerGroupID = CONVERT(nvarchar(50), cg.CustomerGroupID)");
            query.AppendLine(" LEFT JOIN dbo.[User] u ON ac.UserID = u.Id");
            query.AppendLine("WHERE IsDelete = 0");

            query.AppendLine(string.Format("{0}", IsFilterByTime ? string.Format("AND ac.[DateCreated] >= '{0}' AND ac.[DateCreated] < '{1}' ", _fromdate, _todate) : string.Format("AND ac.[Date] >= '{0}' AND ac.[Date] < '{1}' ", _fromdate, _todate)));

         //   query.AppendLine(string.Format("AND ac.DateCreated >= '{0}' AND ac.DateCreated < '{1}'", _fromdate, _todate));

            if (!string.IsNullOrWhiteSpace(keyword))
                query.AppendLine(string.Format("AND (Plate LIKE '%{0}%' OR CardNumber LIKE '%{0}%' OR CardNo LIKE '%{0}%' )", keyword));

            //if (!string.IsNullOrWhiteSpace(CustomerID))
            //    query.AppendLine(string.Format("AND CustomerID = '{0}'", CustomerID));

            if (CustomerGroupID.Any())
            {
                query.AppendLine("AND ac.CustomerGroupID IN (");

                var count = 0;
                foreach (var item in CustomerGroupID)
                {
                    count++;
                    query.AppendLine(string.Format("'{0}'{1}", item, count == CustomerGroupID.Count ? "" : ","));
                }

                query.AppendLine(")");
            }

            if (!string.IsNullOrWhiteSpace(CardGroupID))
            {
                var t = CardGroupID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and CardGroupID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }
            if (!string.IsNullOrWhiteSpace(AuthCardGroupIds))
            {
                var t = AuthCardGroupIds.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and CardGroupID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }
            if (!string.IsNullOrWhiteSpace(UserID))
            {
                var t = UserID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and UserID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }


            query.AppendLine(") as a");


            var list = Data.SqlHelper.ExcuteSQL.GetDataSet(query.ToString(), false);
            total = list.Tables.Count > 1 ? Convert.ToInt32(list.Tables[1].Rows[0]["totalCount"].ToString()) : 0;
            _totalmoneys = !string.IsNullOrEmpty(list.Tables[1].Rows[0]["totalMoney"].ToString()) ? Convert.ToInt32(list.Tables[1].Rows[0]["totalMoney"].ToString()) : 0;
            return Data.SqlHelper.ExcuteSQL.ConvertTo<ExtendList>(list.Tables[0]);
        }

        public DataTable GetDetailExtend_Excel(string keyword, bool IsFilterByTime, string _fromdate, string _todate, string CardGroupID, string CustomerID, List<string> CustomerGroupID, string UserID)
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

            query.AppendLine("SELECT RowNumber as STT,CardNo as 'Số thẻ', CardNumber as 'Mã thẻ', CustomerID as 'Họ tên',CustomerGroupID as 'Nhóm KH',FeeLevel as 'Giá',UserID as 'Người thực hiện',(select convert(varchar(10), Date, 103) + ' ' + left(convert(varchar(32), Date, 108),8)) as 'Ngày thanh toán',(select convert(varchar(10), DateCreated, 103) + ' ' + left(convert(varchar(32), DateCreated, 108),8)) as 'Ngày tạo' FROM(");
            query.AppendLine("SELECT ROW_NUMBER() OVER(ORDER BY ac.DateCreated desc) AS RowNumber, Date,CardNo, CardNumber, CardGroupID, Plate, c.CustomerName as CustomerID, cg.CustomerGroupName as CustomerGroupID, OldExpireDate, NewExpireDate, FeeLevel, u.Username as UserID,c.Address,ac.DateCreated ");
            query.AppendLine(" FROM ExtendCard ac WITH (NOLOCK) ");
            query.AppendLine(" LEFT JOIN tblCustomer c ON ac.CustomerID = CONVERT(nvarchar(50), c.CustomerID)");
            query.AppendLine(" LEFT JOIN tblCustomerGroup cg ON ac.CustomerGroupID = CONVERT(nvarchar(50), cg.CustomerGroupID)");
            query.AppendLine(" LEFT JOIN dbo.[User] u ON ac.UserID = u.Id");
            query.AppendLine("WHERE IsDelete = 0");

            query.AppendLine(string.Format("{0}", IsFilterByTime ? string.Format("AND ac.[DateCreated] >= '{0}' AND ac.[DateCreated] < '{1}' ", _fromdate, _todate) : string.Format("AND ac.[Date] >= '{0}' AND ac.[Date] < '{1}' ", _fromdate, _todate)));

          //  query.AppendLine(string.Format("AND ac.DateCreated >= '{0}' AND ac.DateCreated < '{1}'", _fromdate, _todate));

            if (!string.IsNullOrWhiteSpace(keyword))
                query.AppendLine(string.Format("AND (Plate LIKE '%{0}%' OR CardNumber LIKE '%{0}%' OR CardNo LIKE '%{0}%')", keyword));



            //if (!string.IsNullOrWhiteSpace(CustomerID))
            //    query.AppendLine(string.Format("AND CustomerID = '{0}'", CustomerID));

            if (CustomerGroupID.Any())
            {
                query.AppendLine("AND ac.CustomerGroupID IN (");

                var count = 0;
                foreach (var item in CustomerGroupID)
                {
                    count++;
                    query.AppendLine(string.Format("'{0}'{1}", item, count == CustomerGroupID.Count ? "" : ","));
                }

                query.AppendLine(")");
            }

            if (!string.IsNullOrWhiteSpace(CardGroupID))
            {
                var t = CardGroupID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and CardGroupID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }
            if (!string.IsNullOrWhiteSpace(AuthCardGroupIds))
            {
                var t = AuthCardGroupIds.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and CardGroupID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            if (!string.IsNullOrWhiteSpace(UserID))
            {
                var t = UserID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and UserID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            query.AppendLine(") as a");

            var list = Data.SqlHelper.ExcuteSQL.GetDataSet(query.ToString(), false);

            return list.Tables[0];
        }

        public IPagedList<tblActiveCardCustomViewModel> GetAllPagingByFirst_PRIDE(string key, string cardno, string fromdate, string todate, string cardgroupids, string customergroupids, string users, int pageNumber, int pageSize)
        {
            var query = from n in _tblActiveCardRepository.Table
                        join m in _tblCardGroupRepository.Table on n.CardGroupID equals m.CardGroupID.ToString() into n_m
                        from m in n_m.DefaultIfEmpty()
                        join g in _tblCustomerGroupRepository.Table on n.CustomerGroupID equals g.CustomerGroupID.ToString() into m_g
                        from g in m_g.DefaultIfEmpty()
                        join c in _tblCustomerRepository.Table on n.CustomerID equals c.CustomerID.ToString() into m_c
                        from c in m_c.DefaultIfEmpty()
                        join u in _UserRepository.Table on n.UserID equals u.Id into n_u
                        from u in n_u.DefaultIfEmpty()
                        where n.IsDelete == false

                        select new tblActiveCardCustomViewModel()
                        {
                            CardGroupID = n.CardGroupID,
                            CardGroupName = m.CardGroupName,
                            CardNo = n.CardNo,
                            CardNumber = n.CardNumber,
                            CustomerGroupID = n.CustomerGroupID,
                            CustomerGroupName = g.CustomerGroupName,
                            CustomerID = n.CustomerID,
                            Date = n.Date,
                            Days = n.Days,
                            FeeLevel = n.FeeLevel,
                            IsDelete = n.IsDelete,
                            NewExpireDate = n.NewExpireDate,
                            OldExpireDate = n.OldExpireDate,
                            Plate = n.Plate,
                            UserID = u.Id,
                            Id = n.Id,
                            Code = n.Code,
                            UserName = u.Username,
                            Address = c.Address,
                            AddressUnsign = c.AddressUnsign
                        };

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.CardNumber.Contains(key.Trim()) || n.Plate.Contains(key.Trim()) || n.CardNo.Contains(key.Trim()) || n.Address.Contains(key.Trim()) || n.AddressUnsign.Contains(key.Trim()));
            }

            if (!string.IsNullOrWhiteSpace(cardgroupids))
            {
                query = query.Where(n => cardgroupids.Contains(n.CardGroupID));
            }

            if (!string.IsNullOrWhiteSpace(customergroupids))
            {
                query = query.Where(n => customergroupids.Contains(n.CustomerGroupID));
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
                var time = DateTime.Now;
                var fdate = Convert.ToDateTime(time.ToString("dd/MM/yyyy 00:00:00"));
                var tdate = Convert.ToDateTime(time.AddDays(1).ToString("dd/MM/yyyy 00:00:00"));

                query = query.Where(n => n.Date >= fdate && n.Date < tdate);
            }          

            var list = new PagedList<tblActiveCardCustomViewModel>(query.OrderByDescending(n => n.Date), pageNumber, pageSize);

            return list;
        }

        public List<tblActiveCardCustomViewModel> GetBill(string cardnumbers)
        {
            var query = new StringBuilder();
            query.AppendLine("Select top 1 with ties ac.CardNumber,ac.Plate,ac.FeeLevel,ac.OldExpireDate,ac.NewExpireDate,c.CustomerName");
            query.AppendLine("From tblActiveCard ac");
            query.AppendLine("left join tblCustomer c on ac.CustomerID = CONVERT(varchar(255), c.CustomerID)");
            query.AppendLine("where IsDelete = 0");

            if (!string.IsNullOrEmpty(cardnumbers))
            {
                cardnumbers = cardnumbers.Replace("'", "");
                var t = cardnumbers.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and CardNumber IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }               
            }
           
            query.AppendLine("Order By Row_Number() over (Partition By CardNumber Order By date Desc)");

            var list = Data.SqlHelper.ExcuteSQL.GetDataSet(query.ToString(), false);

            return Data.SqlHelper.ExcuteSQL.ConvertTo<tblActiveCardCustomViewModel>(list.Tables[0]);
        }

        public List<tblActiveCardCustomViewModel> GetBill_v2(string orderId)
        {
            var query = new StringBuilder();
            query.AppendLine("Select ac.CardNumber,ac.CardNo,ac.Plate,ac.FeeLevel,ac.OldExpireDate,ac.NewExpireDate,c.CustomerName,c.Address");
            query.AppendLine("From tblActiveCard ac");
            query.AppendLine("left join tblCustomer c on ac.CustomerID = CONVERT(varchar(255), c.CustomerID)");
            query.AppendLine("where IsDelete = 0");

            if (!string.IsNullOrEmpty(orderId))
            {
                query.AppendLine(string.Format("and ac.OrderId = '{0}'",orderId));
            }

            query.AppendLine("Order By date Desc");

            var list = Data.SqlHelper.ExcuteSQL.GetDataSet(query.ToString(), false);

            return Data.SqlHelper.ExcuteSQL.ConvertTo<tblActiveCardCustomViewModel>(list.Tables[0]);
        }

        //dùng cho trường chinh
        public IPagedList<tblActiveCardCustomViewModel> GetAllPagingByFirstTRANSERCO(string key, string typepay, string cardno, string fromdate, string todate, string cardgroupids, string customergroupids, string users, int pageNumber, int pageSize)
        {
            var query = from n in _tblActiveCardRepository.Table
                        join m in _tblCardGroupRepository.Table on n.CardGroupID equals m.CardGroupID.ToString() into n_m
                        from m in n_m.DefaultIfEmpty()
                        join g in _tblCustomerGroupRepository.Table on n.CustomerGroupID equals g.CustomerGroupID.ToString() into m_g
                        from g in m_g.DefaultIfEmpty()
                        join u in _UserRepository.Table on n.UserID equals u.Id into n_u
                        from u in n_u.DefaultIfEmpty()
                        where n.IsDelete == false

                        select new tblActiveCardCustomViewModel()
                        {
                            CardGroupID = n.CardGroupID,
                            CardGroupName = m.CardGroupName,
                            CardNo = n.CardNo,
                            CardNumber = n.CardNumber,
                            CustomerGroupID = n.CustomerGroupID,
                            CustomerGroupName = g.CustomerGroupName,
                            CustomerID = n.CustomerID,
                            Date = n.Date,
                            Days = n.Days,
                            FeeLevel = n.FeeLevel,
                            IsDelete = n.IsDelete,
                            NewExpireDate = n.NewExpireDate,
                            OldExpireDate = n.OldExpireDate,
                            Plate = n.Plate,
                            UserID = u.Id,
                            Id = n.Id,
                            Code = n.Code,
                            UserName = u.Username,
                            ContractCode = n.ContractCode,
                            IsTransferPayment = n.IsTransferPayment,
                            Tax = g.Tax
                        };

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.CardNumber.Contains(key) || n.Plate.Contains(key) || n.CardNo.Contains(key) || n.ContractCode.Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(cardgroupids))
            {
                query = query.Where(n => cardgroupids.Contains(n.CardGroupID));
            }

            if (!string.IsNullOrWhiteSpace(customergroupids))
            {
                query = query.Where(n => customergroupids == n.CustomerGroupID);
            }

            if (!string.IsNullOrWhiteSpace(users))
            {
                query = query.Where(n => users.Contains(n.UserID));
            }

            if (!string.IsNullOrWhiteSpace(typepay))
            {
                var check = typepay.Equals("0") ? true : false;
                query = query.Where(n => n.IsTransferPayment == check);
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                var fdate = Convert.ToDateTime(fromdate);
                var tdate = Convert.ToDateTime(todate).AddDays(1);

                query = query.Where(n => n.Date >= fdate && n.Date < tdate);
            }
            else
            {
                var time = DateTime.Now;
                var fdate = Convert.ToDateTime(time.ToString("dd/MM/yyyy 00:00:00"));
                var tdate = Convert.ToDateTime(time.AddDays(1).ToString("dd/MM/yyyy 00:00:00"));

                query = query.Where(n => n.Date >= fdate && n.Date < tdate);
            }

            var list = new PagedList<tblActiveCardCustomViewModel>(query.OrderByDescending(n => n.Date), pageNumber, pageSize);

            return list;
        }

        public List<tblActiveCard_Excel> ReportAllByFirst(string key, string cardno, string fromdate, string todate, string cardgroupids, string customergroupids, string users)
        {
            var query = from n in _tblActiveCardRepository.Table
                        join m in _tblCardGroupRepository.Table on n.CardGroupID equals m.CardGroupID.ToString() into n_m
                        from m in n_m.DefaultIfEmpty()
                        join g in _tblCustomerGroupRepository.Table on n.CustomerGroupID equals g.CustomerGroupID.ToString() into m_g
                        from g in m_g.DefaultIfEmpty()
                        join u in _UserRepository.Table on n.UserID equals u.Id into n_u
                        from u in n_u.DefaultIfEmpty()
                        where n.IsDelete == false

                        select new tblActiveCardCustomViewModel()
                        {
                            CardGroupID = n.CardGroupID,
                            CardGroupName = m.CardGroupName,
                            CardNo = n.CardNo,
                            CardNumber = n.CardNumber,
                            CustomerGroupID = n.CustomerGroupID,
                            CustomerGroupName = g.CustomerGroupName,
                            CustomerID = n.CustomerID,
                            Date = n.Date,
                            Days = n.Days,
                            FeeLevel = n.FeeLevel,
                            IsDelete = n.IsDelete,
                            NewExpireDate = n.NewExpireDate,
                            OldExpireDate = n.OldExpireDate,
                            Plate = n.Plate,
                            UserID = u.Id,
                            Id = n.Id,
                            Code = n.Code,
                            UserName = u.Username
                        };

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.CardNumber.Contains(key) || n.Plate.Contains(key) || n.CardNo.Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(cardgroupids))
            {
                query = query.Where(n => cardgroupids.Contains(n.CardGroupID));
            }
            if (!string.IsNullOrEmpty(AuthCardGroupIds))
            {
                var lst = AuthCardGroupIds.Split(';');
                query = query.Where(n => lst.Contains(n.CardGroupID.ToString()));
            }

            if (!string.IsNullOrWhiteSpace(customergroupids))
            {
                query = query.Where(n => customergroupids.Contains(n.CustomerGroupID));
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
                var time = DateTime.Now;
                var fdate = Convert.ToDateTime(time.ToString("dd/MM/yyyy 00:00:00"));
                var tdate = Convert.ToDateTime(time.AddDays(1).ToString("dd/MM/yyyy 00:00:00"));

                query = query.Where(n => n.Date >= fdate && n.Date < tdate);
            }

            var list = new List<tblActiveCard_Excel>();

            if (query.Any())
            {
                var strCustomer = new List<string>();
                foreach (var item in query)
                {
                    strCustomer.Add(item.CustomerID);
                }

                var listC = _tblCustomerService.GetAllByListId(strCustomer).ToList();

                var count = 0;
                foreach (var item in query)
                {
                    var objC = listC.FirstOrDefault(n => n.CustomerID.ToString() == item.CustomerID);

                    count++;
                    var obj = new tblActiveCard_Excel()
                    {
                        NumberRow = count,
                        CardGroupName = item.CardGroupName,
                        CardNo = item.CardNo,
                        CardNumber = item.CardNumber,
                        CustomerGroupName = item.CustomerGroupName,
                        CustomerName = objC != null ? objC.CustomerName : "",
                        DateCreated = item.Date.Value.ToString(),
                        Days = item.Days,
                        Money = item.FeeLevel,
                        NewDate = item.NewExpireDate.Value.ToString("dd/MM/yyyy"),
                        OldDate = item.OldExpireDate.Value.ToString("dd/MM/yyyy"),
                        Plate = item.Plate,
                        UserName = item.UserName
                    };

                    list.Add(obj);
                }
            }

            return list.OrderByDescending(n => n.DateCreated).ToList();
        }

        public List<tblActiveCard_ExcelPRIDE> ReportAllByFirst_PRIDE(string key, string cardno, string fromdate, string todate, string cardgroupids, string customergroupids, string users)
        {
            var query = from n in _tblActiveCardRepository.Table
                        join m in _tblCardGroupRepository.Table on n.CardGroupID equals m.CardGroupID.ToString() into n_m
                        from m in n_m.DefaultIfEmpty()
                        join g in _tblCustomerGroupRepository.Table on n.CustomerGroupID equals g.CustomerGroupID.ToString() into m_g
                        from g in m_g.DefaultIfEmpty()
                        join c in _tblCustomerRepository.Table on n.CustomerID equals c.CustomerID.ToString() into m_c
                        from c in m_c.DefaultIfEmpty()
                        join u in _UserRepository.Table on n.UserID equals u.Id into n_u
                        from u in n_u.DefaultIfEmpty()
                        where n.IsDelete == false

                        select new tblActiveCardCustomViewModel()
                        {
                            CardGroupID = n.CardGroupID,
                            CardGroupName = m.CardGroupName,
                            CardNo = n.CardNo,
                            CardNumber = n.CardNumber,
                            CustomerGroupID = n.CustomerGroupID,
                            CustomerGroupName = g.CustomerGroupName,
                            CustomerID = n.CustomerID,
                            Date = n.Date,
                            Days = n.Days,
                            FeeLevel = n.FeeLevel,
                            IsDelete = n.IsDelete,
                            NewExpireDate = n.NewExpireDate,
                            OldExpireDate = n.OldExpireDate,
                            Plate = n.Plate,
                            UserID = u.Id,
                            Id = n.Id,
                            Code = n.Code,
                            UserName = u.Username,
                            Address = c.Address,
                            AddressUnsign = c.AddressUnsign
                        };

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.CardNumber.Contains(key.Trim()) || n.Plate.Contains(key.Trim()) || n.CardNo.Contains(key.Trim()) || n.Address.Contains(key.Trim()) || n.AddressUnsign.Contains(key.Trim()));
            }

            if (!string.IsNullOrWhiteSpace(cardgroupids))
            {
                query = query.Where(n => cardgroupids.Contains(n.CardGroupID));
            }

            if (!string.IsNullOrWhiteSpace(customergroupids))
            {
                query = query.Where(n => customergroupids.Contains(n.CustomerGroupID));
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
                var time = DateTime.Now;
                var fdate = Convert.ToDateTime(time.ToString("dd/MM/yyyy 00:00:00"));
                var tdate = Convert.ToDateTime(time.AddDays(1).ToString("dd/MM/yyyy 00:00:00"));

                query = query.Where(n => n.Date >= fdate && n.Date < tdate);
            }

            var list = new List<tblActiveCard_ExcelPRIDE>();

            if (query.Any())
            {
                var strCustomer = new List<string>();
                var totalmoney = 0;
                foreach (var item in query)
                {
                    strCustomer.Add(item.CustomerID);
                }

                var listC = _tblCustomerService.GetAllByListId(strCustomer).ToList();

                var count = 0;
                foreach (var item in query.OrderByDescending(n => n.Date))
                {
                    var objC = listC.FirstOrDefault(n => n.CustomerID.ToString() == item.CustomerID);
                    totalmoney += item.FeeLevel;
                    count++;
                    var obj = new tblActiveCard_ExcelPRIDE()
                    {
                        NumberRow = count,
                        CardGroupName = item.CardGroupName,
                        CardNo = item.CardNo,
                        CardNumber = item.CardNumber,
                        CustomerGroupName = item.CustomerGroupName,
                        CustomerName = objC != null ? objC.CustomerName : "",
                        DateCreated = item.Date.Value.ToString("dd/MM/yyyy HH:mm"),
                        Days = item.Days.ToString(),                   
                        NewDate = item.NewExpireDate.Value.ToString("dd/MM/yyyy"),
                        OldDate = item.OldExpireDate.Value.ToString("dd/MM/yyyy"),
                        Plate = item.Plate,
                        UserName = item.UserName,
                        Money = item.FeeLevel,
                        Address = item.Address
                    };

                    list.Add(obj);
                }

                list = list.OrderByDescending(n => n.DateCreated).ToList();

                
            }

            return list;
        }

        //dùng cho trường chinh
        public List<tblActiveCard_ExcelTRANSERCO> ReportAllByFirstTRANSERCO(string key,string typepay, string cardno, string fromdate, string todate, string cardgroupids, string customergroupids, string users)
        {
            var query = from n in _tblActiveCardRepository.Table
                        join m in _tblCardGroupRepository.Table on n.CardGroupID equals m.CardGroupID.ToString() into n_m
                        from m in n_m.DefaultIfEmpty()
                        join g in _tblCustomerGroupRepository.Table on n.CustomerGroupID equals g.CustomerGroupID.ToString() into m_g
                        from g in m_g.DefaultIfEmpty()
                        join u in _UserRepository.Table on n.UserID equals u.Id into n_u
                        from u in n_u.DefaultIfEmpty()
                        where n.IsDelete == false

                        select new tblActiveCardCustomViewModel()
                        {
                            CardGroupID = n.CardGroupID,
                            CardGroupName = m.CardGroupName,
                            CardNo = n.CardNo,
                            CardNumber = n.CardNumber,
                            CustomerGroupID = n.CustomerGroupID,
                            CustomerGroupName = g.CustomerGroupName,
                            CustomerID = n.CustomerID,
                            ContractCode = n.ContractCode,
                            Date = n.Date,
                            Days = n.Days,
                            FeeLevel = n.FeeLevel,
                            IsDelete = n.IsDelete,
                            NewExpireDate = n.NewExpireDate,
                            OldExpireDate = n.OldExpireDate,
                            Plate = n.Plate,
                            UserID = u.Id,
                            Id = n.Id,
                            Code = n.Code,
                            UserName = u.Username,
                            IsTransferPayment = n.IsTransferPayment,
                            Tax = g.Tax
                        };

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.CardNumber.Contains(key) || n.Plate.Contains(key) || n.CardNo.Contains(key) || n.ContractCode.Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(cardgroupids))
            {
                query = query.Where(n => cardgroupids.Contains(n.CardGroupID));
            }

            if (!string.IsNullOrWhiteSpace(customergroupids))
            {
                query = query.Where(n => customergroupids == n.CustomerGroupID);
            }

            if (!string.IsNullOrWhiteSpace(users))
            {
                query = query.Where(n => users.Contains(n.UserID));
            }
            if (!string.IsNullOrWhiteSpace(typepay))
            {
                var check = typepay.Equals("0") ? true : false;
                query = query.Where(n => n.IsTransferPayment == check);
            }
            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                var fdate = Convert.ToDateTime(fromdate);
                var tdate = Convert.ToDateTime(todate).AddDays(1);

                query = query.Where(n => n.Date >= fdate && n.Date < tdate);
            }
            else
            {
                var time = DateTime.Now;
                var fdate = Convert.ToDateTime(time.ToString("dd/MM/yyyy 00:00:00"));
                var tdate = Convert.ToDateTime(time.AddDays(1).ToString("dd/MM/yyyy 00:00:00"));

                query = query.Where(n => n.Date >= fdate && n.Date < tdate);
            }

            query = query.OrderByDescending(n => n.Date);

            var list = new List<tblActiveCard_ExcelTRANSERCO>();

            if (query.Any())
            {
                var strCustomer = new List<string>();
                foreach (var item in query)
                {
                    strCustomer.Add(item.CustomerID);
                }

                var listC = _tblCustomerService.GetAllByListId(strCustomer).ToList();

                var count = 0;
                foreach (var item in query)
                {
                    var objC = listC.FirstOrDefault(n => n.CustomerID.ToString() == item.CustomerID);

                    count++;
                    var obj = new tblActiveCard_ExcelTRANSERCO()
                    {
                        NumberRow = count,
                        CardGroupName = item.CardGroupName,
                        CardNo = item.CardNo,
                        CardNumber = item.CardNumber,
                        CustomerGroupName = item.CustomerGroupName,
                        CustomerName = objC != null ? objC.CustomerName : "",
                        ContractCode = item.ContractCode,
                        DateCreated = item.Date.Value.ToString(),
                        Days = item.Days,
                        Money = item.FeeLevel,
                        NewDate = item.NewExpireDate.Value.ToString("dd/MM/yyyy"),
                        OldDate = item.OldExpireDate.Value.ToString("dd/MM/yyyy"),
                        Plate = item.Plate,
                        UserName = item.UserName,
                        TransferPaymentValue = item.IsTransferPayment ? "Chuyển khoản" : "Tiền mặt",
                        Tax = item.Tax
                    };

                    list.Add(obj);
                }
            }
            return list;
        }

        public MessageReport DeleteById(string id)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                var obj = new tblActiveCard();

                obj = GetById(id);

                if (obj != null)
                {
                    obj.IsDelete = true;
                    _tblActiveCardRepository.Update(obj);
                    Save();

                    re.Message = "Xóa thành công";
                    re.isSuccess = true;
                }
                else
                {
                    re.Message = "Bản ghi không tồn tại";
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

        private void Save()
        {
            _UnitOfWork.Commit();
        }

        public tblActiveCard GetById(string id)
        {
            return _tblActiveCardRepository.GetById(Convert.ToInt32(id));
        }

        public string GetOrderIdByCardNumbers(string cardnumbers)
        {
            var query = from n in _tblActiveCardRepository.Table
                        where cardnumbers.Contains(n.CardNumber) && !string.IsNullOrEmpty(n.OrderId)
                        orderby n.Date descending
                        select n;
            var obj = query.FirstOrDefault();

            string orderid = obj != null ? obj.OrderId : ""; 

            return orderid;
        }

        public string GetTotalMoney(string fromdate, string todate)
        {
            if (!string.IsNullOrEmpty(fromdate))
            {
                fromdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd HH:mm:ss");
            }
            if (!string.IsNullOrEmpty(todate))
            {
                todate = Convert.ToDateTime(todate).ToString("yyyy/MM/dd HH:mm:ss");
            }

            var query = new StringBuilder();
            query.AppendLine("select ISNULL(SUM(Price),0) as TotalPrice from OrderActiveCard");
            query.AppendLine(string.Format("where DateCreated >= '{0}' and DateCreated <= '{1}'", fromdate,todate));

            var money = Data.SqlHelper.ExcuteSQL.GetDataSet(query.ToString(), false).Tables[0].Rows[0]["TotalPrice"].ToString();

            return money != "0" ? Convert.ToDouble(money).ToString("###,###") : "0";
        }

        public string GetTotalMoneyActiveCard(string key, string cardgroupids, string customergroupids, string users, string fromdate, string todate)
        {
            if (!string.IsNullOrEmpty(fromdate))
            {
                fromdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd HH:mm:ss");
            }
            if (!string.IsNullOrEmpty(todate))
            {
                todate = Convert.ToDateTime(todate).ToString("yyyy/MM/dd HH:mm:ss");
            }

            var query = new StringBuilder();
            query.AppendLine("select ISNULL(SUM(a.FeeLevel),0) as TotalPrice from tblActiveCard a");
            query.AppendLine("left join tblCustomer c on a.CustomerID = CONVERT(nvarchar(50), c.CustomerID)");
            query.AppendLine(string.Format("where a.IsDelete = 'False' and a.Date >= '{0}' and a.Date <= '{1}'", fromdate, todate));

            if (!string.IsNullOrWhiteSpace(key))
            {              
                query.AppendLine(string.Format("and (a.CardNumber LIKE '%{0}%' or a.CardNo LIKE '%{0}%' or a.Plate LIKE N'%{0}%' or c.Address LIKE N'%{0}%' or c.AddressUnsign LIKE N'%{0}%')", key.Trim()));
            }

            if (!string.IsNullOrWhiteSpace(cardgroupids))
            {
                var t = cardgroupids.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and CardGroupID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            if (!string.IsNullOrWhiteSpace(customergroupids))
            {
                var t = customergroupids.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and c.CustomerGroupID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            if (!string.IsNullOrWhiteSpace(users))
            {
                var t = users.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and UserID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            var money = Data.SqlHelper.ExcuteSQL.GetDataSet(query.ToString(), false).Tables[0].Rows[0]["TotalPrice"].ToString();

            return money != "0" ? Convert.ToDouble(money).ToString("###,###") : "0";
        }

        public int GetCountByOrderId(string orderId)
        {
            int count = 0;
            var query = new StringBuilder();
            query.AppendLine("select count(*) as totalCount from tblActiveCard");
            query.AppendLine(string.Format("where OrderId='{0}'", orderId));

            var str = Data.SqlHelper.ExcuteSQL.GetDataSet(query.ToString(), false).Tables[0].Rows[0]["totalCount"].ToString();

            count = !string.IsNullOrEmpty(str) ? Convert.ToInt32(str) : 0;

            return count;
        }

        public void DeleteExtendBySubId(string subid)
        {
            var query = new StringBuilder();
            query.AppendLine("delete from ExtendCard");
            query.AppendLine(string.Format("where SubId='{0}'", subid));

            ExcuteSQL.Execute(query.ToString());
        }

       

        public MessageReport Update(tblActiveCard obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblActiveCardRepository.Update(obj);

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

        public List<tblActiveCard_Excel> ReportAllByFirstApec(string key, string typepay, string cardno, string fromdate, string todate, string cardgroupids, string customergroupids, string users)
        {
            var query = from n in _tblActiveCardRepository.Table
                        join m in _tblCardGroupRepository.Table on n.CardGroupID equals m.CardGroupID.ToString() into n_m
                        from m in n_m.DefaultIfEmpty()
                        join g in _tblCustomerGroupRepository.Table on n.CustomerGroupID equals g.CustomerGroupID.ToString() into m_g
                        from g in m_g.DefaultIfEmpty()
                        join u in _UserRepository.Table on n.UserID equals u.Id into n_u
                        from u in n_u.DefaultIfEmpty()
                        where n.IsDelete == false

                        select new tblActiveCardCustomViewModel()
                        {
                            CardGroupID = n.CardGroupID,
                            CardGroupName = m.CardGroupName,
                            CardNo = n.CardNo,
                            CardNumber = n.CardNumber,
                            CustomerGroupID = n.CustomerGroupID,
                            CustomerGroupName = g.CustomerGroupName,
                            CustomerID = n.CustomerID,
                            Date = n.Date,
                            Days = n.Days,
                            FeeLevel = n.FeeLevel,
                            IsDelete = n.IsDelete,
                            NewExpireDate = n.NewExpireDate,
                            OldExpireDate = n.OldExpireDate,
                            Plate = n.Plate,
                            UserID = u.Id,
                            Id = n.Id,
                            Code = n.Code,
                            UserName = u.Username
                        };

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.CardNumber.Contains(key) || n.Plate.Contains(key) || n.CardNo.Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(cardgroupids))
            {
                query = query.Where(n => cardgroupids.Contains(n.CardGroupID));
            }
            if (!string.IsNullOrEmpty(AuthCardGroupIds))
            {
                var lst = AuthCardGroupIds.Split(';');
                query = query.Where(n => lst.Contains(n.CardGroupID.ToString()));
            }

            if (!string.IsNullOrWhiteSpace(customergroupids))
            {
                query = query.Where(n => customergroupids.Contains(n.CustomerGroupID));
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
                var time = DateTime.Now;
                var fdate = Convert.ToDateTime(time.ToString("dd/MM/yyyy 00:00:00"));
                var tdate = Convert.ToDateTime(time.AddDays(1).ToString("dd/MM/yyyy 00:00:00"));

                query = query.Where(n => n.Date >= fdate && n.Date < tdate);
            }

            var list = new List<tblActiveCard_Excel>();

            if (query.Any())
            {
                var strCustomer = new List<string>();
                foreach (var item in query)
                {
                    strCustomer.Add(item.CustomerID);
                }

                var listC = _tblCustomerService.GetAllByListId(strCustomer).ToList();

                var count = 0;
                foreach (var item in query)
                {
                    var objC = listC.FirstOrDefault(n => n.CustomerID.ToString() == item.CustomerID);

                    count++;
                    var obj = new tblActiveCard_Excel()
                    {
                        NumberRow = count,
                        CardGroupName = item.CardGroupName,
                        CardNo = item.CardNo,
                        CardNumber = item.CardNumber,
                        CustomerGroupName = item.CustomerGroupName,
                        CustomerName = objC != null ? objC.CustomerName : "",
                        DateCreated = item.Date.Value.ToString(),
                        Days = item.Days,
                        Money = item.FeeLevel,
                        NewDate = item.NewExpireDate.Value.ToString("dd/MM/yyyy"),
                        OldDate = item.OldExpireDate.Value.ToString("dd/MM/yyyy"),
                        Plate = item.Plate,
                        UserName = item.UserName
                    };

                    list.Add(obj);
                }
            }

            return list.OrderByDescending(n => n.DateCreated).ToList();
        }
    }
}
