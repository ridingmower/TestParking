using Kztek.Data.Event.Infrastructure;
using Kztek.Data.Event.Repository;
using Kztek.Data.Event.SqlHelper;
using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Model.Models.Event;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Service.Admin.Event
{
    public interface ItblCardEventDeleteService
    {
        IPagedList<tblCardEventDelete> GetAll(string key, int pageNumber, int pageSize);
        List<tblCardEventDeleteCustom> GetAllSql(string key, string fromdate, string todate, int pageIndex, int pageSize, ref int total);
        tblCardEventDelete GetById(string id);
        MessageReport DeleteById(string id);

        List<ReportInOut108> GetListEventInOut(string KeyWord, string number, List<string> CustomerGroupID, bool IsFilterByTimeIn, string fromdate, string todate, string CardGroupID, string LaneID, string UserID, string CustomerGroupId, int pageIndex, int pageSize, ref int total, ref decimal totalMoney);
        bool DeleteEvent(List<string> list, string userid);
        bool RestoreDeleteEvent(string KeyWord, List<string> CustomerGroupID, string fromdate, string todate, string CardGroupID, string LaneID, string UserID, string RowId);

        TotalEventDelete GetTotalMoney(string KeyWord, List<string> CustomerGroupID, bool IsFilterByTimeIn, string fromdate, string todate, string CardGroupID, string LaneID, string UserID, string CustomerGroupId);

        List<ReportInOut108> GetHistoryDeleteEvent(string KeyWord, List<string> CustomerGroupID, bool IsFilterByTimeIn, string fromdate, string todate, string CardGroupID, string LaneID, string UserID, string CustomerGroupId, int pageIndex, int pageSize, ref int total);
        DataTable GetHistoryDeleteEvent_Excel(string KeyWord, List<string> CustomerGroupID, bool IsFilterByTimeIn, string fromdate, string todate, string CardGroupID, string LaneID, string UserID, string CustomerGroupId, ref int total);

    }
    public class tblCardEventDeleteService : ItblCardEventDeleteService
    {
        private ItblCardEventDeleteRepository _tblCardEventDeleteRepository;
        private IUnitOfWork _UnitOfWork;
        public tblCardEventDeleteService(ItblCardEventDeleteRepository _tblCardEventDeleteRepository, IUnitOfWork _UnitOfWork)
        {
            this._tblCardEventDeleteRepository = _tblCardEventDeleteRepository;
            this._UnitOfWork = _UnitOfWork;

        }
        private void Save()
        {
            _UnitOfWork.Commit();
        }
        public IPagedList<tblCardEventDelete> GetAll(string key, int pageNumber, int pageSize)
        {
            var query = from n in _tblCardEventDeleteRepository.Table
                        select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.Id.Contains(key) || n.EventId.Contains(key) || n.UserId.Contains(key));
            }

            var list = new PagedList<tblCardEventDelete>(query.OrderByDescending(n => n.DateCreated), pageNumber, pageSize);

            return list;
        }
        public List<tblCardEventDeleteCustom> GetAllSql(string key, string fromdate, string todate, int pageIndex, int pageSize, ref int total)
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
            query.AppendLine("SELECT * FROM (");
            query.AppendLine("SELECT ROW_NUMBER() OVER(ORDER BY c.[DateCreated] desc) as RowNumber,c.*,u.Username FROM tblCardEventDelete c");
            query.AppendLine("LEFT JOIN MPARKING.dbo.[User] u ON c.UserId = u.Id");
            query.AppendLine(string.Format("{0}", string.Format("WHERE c.[DateCreated] >= '{0}' AND c.[DateCreated] <= '{1}' ", fromdate, todate)));
            if (!string.IsNullOrEmpty(key))
                query.AppendLine(string.Format("AND (u.Username LIKE '%{0}%' OR c.Id LIKE '%{0}%' OR c.EventId LIKE '%{0}%' OR c.UserId LIKE '%{0}%')", key));

            query.AppendLine("");
            query.AppendLine(") AS A");
            query.AppendLine(string.Format("WHERE RowNumber BETWEEN (({0}-1) * {1} + 1) AND ({0} * {1})", pageIndex, pageSize));
            query.AppendLine("SELECT COUNT(Id) AS totalCount FROM (");
            query.AppendLine("SELECT c.*,u.Username FROM tblCardEventDelete c");
            query.AppendLine("LEFT JOIN MPARKING.dbo.[User] u ON c.UserId = u.Id");
            query.AppendLine(string.Format("{0}", string.Format("WHERE c.[DateCreated] >= '{0}' AND c.[DateCreated] <= '{1}' ", fromdate, todate)));
            if (!string.IsNullOrEmpty(key))
                query.AppendLine(string.Format("AND (u.Username LIKE '%{0}%' OR c.Id LIKE '%{0}%' OR c.EventId LIKE '%{0}%' OR c.UserId LIKE '%{0}%')", key));

            query.AppendLine("");
            query.AppendLine(") AS A");
            var list = ExcuteSQLEvent.GetDataSet(query.ToString(), false);
            total = list.Tables.Count > 1 ? Convert.ToInt32(list.Tables[1].Rows[0]["totalCount"].ToString()) : 0;

            return ExcuteSQLEvent.ConvertTo<tblCardEventDeleteCustom>(list.Tables[0]);
        }
        public tblCardEventDelete GetById(string id)
        {
            return _tblCardEventDeleteRepository.GetById(id);
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
                    _tblCardEventDeleteRepository.Delete(obj);

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

        public List<ReportInOut108> GetListEventInOut(string KeyWord, string number, List<string> CustomerGroupID, bool IsFilterByTimeIn, string fromdate, string todate, string CardGroupID, string LaneID, string UserID, string CustomerGroupId, int pageIndex, int pageSize, ref int total, ref decimal totalMoney)
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
            query.AppendLine("SELECT *,SUM(Moneys) Over() AS TotalMoneyPercent FROM(");
            query.AppendLine("SELECT ROW_NUMBER() OVER(ORDER BY a.[DatetimeOut] desc) as RowNumber,a.*");
            query.AppendLine("FROM(");
            query.AppendLine("SELECT");

            if (!string.IsNullOrEmpty(number) && !number.Equals("100"))
            {
                query.AppendLine(string.Format("TOP {0} percent", number));
            }

            query.AppendLine("CONVERT(nvarchar(50), e.[Id]) + '_CARD' as Id, e.[CardNo], e.[CardNumber], CAST(CASE WHEN e.[PlateIn] <> '' THEN e.[PlateIn] ELSE e.[PlateOut] END AS nvarchar(50)) as Plate, e.[DatetimeIn], e.[DatetimeOut], e.[PicDirIn] as PicIn1, REPLACE(e.[PicDirIn], 'PLATEIN.JPG', 'OVERVIEWIN.JPG') as PicIn2, e.[PicDirOut] as PicOut1, REPLACE(e.[PicDirOut], 'PLATEOUT.JPG', 'OVERVIEWOUT.JPG') as PicOut2, e.[CardGroupID], e.[CustomerName], e.[LaneIDIn], e.[LaneIDOut], e.[UserIDIn], e.[UserIDOut], e.[Moneys]");

            query.AppendLine("FROM dbo.[tblCardEvent] e WITH(NOLOCK)");

            query.AppendLine("WHERE e.[IsDelete] = 0 and e.[EventCode] = '2' and e.[Moneys] > 0 and e.[IsFree] = 'False'");
            query.AppendLine(string.Format("{0}", IsFilterByTimeIn ? string.Format("AND e.[DatetimeIn] >= '{0}' AND e.[DatetimeIn] <= '{1}' ", fromdate, todate) : string.Format("AND e.[DatetimeOut] >= '{0}' AND e.[DatetimeOut] <= '{1}' ", fromdate, todate)));


            //Nhom the
            if (!string.IsNullOrWhiteSpace(CardGroupID))
            {
                var t = CardGroupID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and e.CardGroupID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            //Lan
            if (!string.IsNullOrWhiteSpace(LaneID))
            {
                var t = LaneID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;
                    var _count = 0;
                    query.AppendLine("and (e.LaneIDIn IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" ) ");

                    query.AppendLine("OR e.[LaneIDOut] IN (");

                    foreach (var item in t)
                    {
                        _count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, _count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" ) )");
                }
            }

            //User
            if (!string.IsNullOrWhiteSpace(UserID))
            {
                var t = UserID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;
                    var _count = 0;
                    query.AppendLine("and (e.UserIDIn IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" ) ");

                    query.AppendLine("OR e.[UserIDOut] IN (");

                    foreach (var item in t)
                    {
                        _count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, _count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" ) )");
                }
            }

            //Nhom KH
            if (CustomerGroupID.Any())
            {
                query.AppendLine("AND e.CustomerGroupID IN (");

                var count = 0;
                foreach (var item in CustomerGroupID)
                {
                    count++;
                    query.AppendLine(string.Format("'{0}'{1}", item, count == CustomerGroupID.Count ? "" : ","));
                }

                query.AppendLine(")");
            }

            if (!string.IsNullOrWhiteSpace(KeyWord))
                query.AppendLine(string.Format("AND (e.[CardNumber] LIKE '%{0}%' OR e.[CardNo] LIKE '%{0}%' OR e.[PlateIn] LIKE '%{0}%' OR e.[PlateOut] LIKE '%{0}%')", KeyWord));

            if (!string.IsNullOrEmpty(number) && !number.Equals("100"))
            {
                query.AppendLine("ORDER BY NEWID()");
            }
            query.AppendLine(") as a");
            query.AppendLine(") as TEMP");
            //  query.AppendLine(string.Format("WHERE RowNumber BETWEEN (({0}-1) * {1} + 1) AND ({0} * {1})", pageIndex, pageSize));

            ////--Count Total
            query.AppendLine("SELECT COUNT(Id) as totalCount");
            query.AppendLine("FROM ( SELECT");

            if (!string.IsNullOrEmpty(number) && !number.Equals("100"))
            {
                query.AppendLine(string.Format("TOP {0} percent", number));
            }
            query.AppendLine("Id FROM dbo.[tblCardEvent]");
            query.AppendLine("e WITH(NOLOCK)");

            query.AppendLine("WHERE e.[IsDelete] = 0 and e.[EventCode] = '2' and e.[Moneys] > 0 and e.[IsFree] = 'False'");
            query.AppendLine(string.Format("{0}", IsFilterByTimeIn ? string.Format("AND e.[DatetimeIn] >= '{0}' AND e.[DatetimeIn] <= '{1}' ", fromdate, todate) : string.Format("AND e.[DatetimeOut] >= '{0}' AND e.[DatetimeOut] <= '{1}' ", fromdate, todate)));

            //Nhom the
            if (!string.IsNullOrWhiteSpace(CardGroupID))
            {
                var t = CardGroupID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and e.CardGroupID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            //Lan
            if (!string.IsNullOrWhiteSpace(LaneID))
            {
                var t = LaneID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;
                    var _count = 0;
                    query.AppendLine("and (e.LaneIDIn IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" ) ");

                    query.AppendLine("OR e.[LaneIDOut] IN (");

                    foreach (var item in t)
                    {
                        _count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, _count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" ) )");
                }
            }

            //User
            if (!string.IsNullOrWhiteSpace(UserID))
            {
                var t = UserID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;
                    var _count = 0;
                    query.AppendLine("and (e.UserIDIn IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" ) ");

                    query.AppendLine("OR e.[UserIDOut] IN (");

                    foreach (var item in t)
                    {
                        _count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, _count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" ) )");
                }
            }

            //Nhom KH
            if (CustomerGroupID.Any())
            {
                query.AppendLine("AND e.CustomerGroupID IN (");

                var count = 0;
                foreach (var item in CustomerGroupID)
                {
                    count++;
                    query.AppendLine(string.Format("'{0}'{1}", item, count == CustomerGroupID.Count ? "" : ","));
                }

                query.AppendLine(")");
            }

            if (!string.IsNullOrWhiteSpace(KeyWord))
                query.AppendLine(string.Format("AND (e.[CardNumber] LIKE '%{0}%' OR e.[CardNo] LIKE '%{0}%' OR e.[PlateIn] LIKE '%{0}%' OR e.[PlateOut] LIKE '%{0}%')", KeyWord));

            if (!string.IsNullOrEmpty(number) && !number.Equals("100"))
            {
                query.AppendLine("ORDER BY NEWID()");
            }

            query.AppendLine(") as e");

            var list = ExcuteSQLEvent.GetDataSet(query.ToString(), false);
            total = list.Tables.Count > 1 ? Convert.ToInt32(list.Tables[1].Rows[0]["totalCount"].ToString()) : 0;
            // totalMoney = list.Tables.Count > 1 && !string.IsNullOrEmpty(list.Tables[1].Rows[0]["totalMoney"].ToString()) ? Convert.ToDecimal(list.Tables[1].Rows[0]["totalMoney"].ToString()) : 0;
            return ExcuteSQLEvent.ConvertTo<ReportInOut108>(list.Tables[0]);
        }

        public TotalEventDelete GetTotalMoney(string KeyWord, List<string> CustomerGroupID, bool IsFilterByTimeIn, string fromdate, string todate, string CardGroupID, string LaneID, string UserID, string CustomerGroupId)
        {
            var model = new TotalEventDelete();

            if (!string.IsNullOrEmpty(fromdate))
            {
                fromdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd HH:mm:ss");
            }
            if (!string.IsNullOrEmpty(todate))
            {
                todate = Convert.ToDateTime(todate).ToString("yyyy/MM/dd HH:mm:ss");
            }
            var query = new StringBuilder();
            query.AppendLine("SELECT COUNT(Id) as totalCount, SUM(Moneys) as totalMoney");
            query.AppendLine("FROM ( SELECT");

            query.AppendLine("Id,Moneys FROM dbo.[tblCardEvent]");
            query.AppendLine("e WITH(NOLOCK)");

            query.AppendLine("WHERE e.[IsDelete] = 0 and e.[EventCode] = '2' and e.[Moneys] > 0 and e.[IsFree] = 'False'");
            query.AppendLine(string.Format("{0}", IsFilterByTimeIn ? string.Format("AND e.[DatetimeIn] >= '{0}' AND e.[DatetimeIn] <= '{1}' ", fromdate, todate) : string.Format("AND e.[DatetimeOut] >= '{0}' AND e.[DatetimeOut] <= '{1}' ", fromdate, todate)));

            //Nhom the
            if (!string.IsNullOrWhiteSpace(CardGroupID))
            {
                var t = CardGroupID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and e.CardGroupID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            //Lan
            if (!string.IsNullOrWhiteSpace(LaneID))
            {
                var t = LaneID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;
                    var _count = 0;
                    query.AppendLine("and (e.LaneIDIn IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" ) ");

                    query.AppendLine("OR e.[LaneIDOut] IN (");

                    foreach (var item in t)
                    {
                        _count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, _count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" ) )");
                }
            }

            //User
            if (!string.IsNullOrWhiteSpace(UserID))
            {
                var t = UserID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;
                    var _count = 0;
                    query.AppendLine("and (e.UserIDIn IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" ) ");

                    query.AppendLine("OR e.[UserIDOut] IN (");

                    foreach (var item in t)
                    {
                        _count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, _count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" ) )");
                }
            }

            //Nhom KH
            if (CustomerGroupID.Any())
            {
                query.AppendLine("AND e.CustomerGroupID IN (");

                var count = 0;
                foreach (var item in CustomerGroupID)
                {
                    count++;
                    query.AppendLine(string.Format("'{0}'{1}", item, count == CustomerGroupID.Count ? "" : ","));
                }

                query.AppendLine(")");
            }

            if (!string.IsNullOrWhiteSpace(KeyWord))
                query.AppendLine(string.Format("AND (e.[CardNumber] LIKE '%{0}%' OR e.[CardNo] LIKE '%{0}%' OR e.[PlateIn] LIKE '%{0}%' OR e.[PlateOut] LIKE '%{0}%')", KeyWord));

            query.AppendLine(") as e");

            var list = ExcuteSQLEvent.GetDataSet(query.ToString(), false);

            model.TotalMoney = !string.IsNullOrEmpty(list.Tables[0].Rows[0]["totalMoney"].ToString()) && Convert.ToDecimal(list.Tables[0].Rows[0]["totalMoney"].ToString()) > 0 ? Convert.ToDecimal(list.Tables[0].Rows[0]["totalMoney"].ToString()).ToString("###,###") : "0";
            model.TotalCount = Convert.ToInt32(list.Tables[0].Rows[0]["totalCount"].ToString());

            return model;
        }

        public bool DeleteEvent(List<string> list, string userid)
        {
            int count = 0;
            string arrid = "";
            foreach (var item in list)
            {
                var arr = item.Split('_');
                count++;
                arrid += string.Format("'{0}'{1}", arr[0], count == list.Count ? "" : ",");
            }

            var query = new StringBuilder();
            query.AppendLine("UPDATE tblCardEvent");
            query.AppendLine("SET IsDelete = 'True'");
            query.AppendLine(string.Format("WHERE Id IN ({0})", arrid));
            query.AppendLine("INSERT INTO tblCardEventDelete(Id, EventId, DateCreated,UserId)");
            query.AppendLine("SELECT NEWID(),");
            query.AppendLine("(SELECT Top 1 e.Id),");
            query.AppendLine(string.Format("'{0}',", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")));
            query.AppendLine(string.Format("'{0}'", userid));
            query.AppendLine(string.Format("From tblCardEvent e WHERE Id IN ({0})", arrid));
            query.AppendLine("AND NOT EXISTS (SELECT * FROM tblCardEventDelete  WHERE EventId = CONVERT(varchar(50), e.Id))");
            return ExcuteSQLEvent.Execute(query.ToString());
        }

        public List<ReportInOut108> GetHistoryDeleteEvent(string KeyWord, List<string> CustomerGroupID, bool IsFilterByTimeIn, string fromdate, string todate, string CardGroupID, string LaneID, string UserID, string CustomerGroupId, int pageIndex, int pageSize, ref int total)
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
            query.AppendLine("SELECT * FROM(");
            query.AppendLine("SELECT ROW_NUMBER() OVER(ORDER BY a.[DateCreated] desc) as RowNumber,a.*");
            query.AppendLine("FROM(");
            query.AppendLine("SELECT CONVERT(nvarchar(50), e.[Id]) + '_CARD' as Id, e.[CardNo], e.[CardNumber], CAST(CASE WHEN e.[PlateIn] <> '' THEN e.[PlateIn] ELSE e.[PlateOut] END AS nvarchar(50)) as Plate, e.[DatetimeIn], e.[DatetimeOut], e.[PicDirIn] as PicIn1, REPLACE(e.[PicDirIn], 'PLATEIN.JPG', 'OVERVIEWIN.JPG') as PicIn2, e.[PicDirOut] as PicOut1, REPLACE(e.[PicDirOut], 'PLATEOUT.JPG', 'OVERVIEWOUT.JPG') as PicOut2, e.[CardGroupID], e.[CustomerName], e.[LaneIDIn], e.[LaneIDOut], e.[UserIDIn], e.[UserIDOut], e.[Moneys],d.[DateCreated],d.[UserId],d.[Id] as RowId");

            query.AppendLine("FROM dbo.[tblCardEvent] e WITH(NOLOCK)");
            query.AppendLine("LEFT JOIN dbo.[tblCardEventDelete] d WITH(NOLOCK)");
            query.AppendLine("ON d.EventId = CONVERT(nvarchar(50), e.Id)");
            query.AppendLine("WHERE 1 = 1 ");

            //query.AppendLine(string.Format("{0}", IsFilterByTimeIn ? string.Format("AND e.[DatetimeIn] >= '{0}' AND e.[DatetimeIn] <= '{1}' ", fromdate, todate) : string.Format("AND e.[DatetimeOut] >= '{0}' AND e.[DatetimeOut] <= '{1}' ", fromdate, todate)));
            query.AppendLine(string.Format("{0}", string.Format("AND d.[DateCreated] >= '{0}' AND d.[DateCreated] <= '{1}' ", fromdate, todate)));

            //Nhom the
            if (!string.IsNullOrWhiteSpace(CardGroupID))
            {
                var t = CardGroupID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and e.CardGroupID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            //Lan
            if (!string.IsNullOrWhiteSpace(LaneID))
            {
                var t = LaneID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;
                    var _count = 0;
                    query.AppendLine("and (e.LaneIDIn IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" ) ");

                    query.AppendLine("OR e.[LaneIDOut] IN (");

                    foreach (var item in t)
                    {
                        _count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, _count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" ) )");
                }
            }

            //User
            if (!string.IsNullOrWhiteSpace(UserID))
            {
                var t = UserID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and d.UserId IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            //Nhom KH
            if (CustomerGroupID.Any())
            {
                query.AppendLine("AND e.CustomerGroupID IN (");

                var count = 0;
                foreach (var item in CustomerGroupID)
                {
                    count++;
                    query.AppendLine(string.Format("'{0}'{1}", item, count == CustomerGroupID.Count ? "" : ","));
                }

                query.AppendLine(")");
            }

            if (!string.IsNullOrWhiteSpace(KeyWord))
                query.AppendLine(string.Format("AND (e.[CardNumber] LIKE '%{0}%' OR e.[CardNo] LIKE '%{0}%' OR e.[PlateIn] LIKE '%{0}%' OR e.[PlateOut] LIKE '%{0}%')", KeyWord.Trim()));

            query.AppendLine(") as a");
            query.AppendLine(") as TEMP");
            query.AppendLine(string.Format("WHERE RowNumber BETWEEN (({0}-1) * {1} + 1) AND ({0} * {1})", pageIndex, pageSize));

            //--Count Total
            query.AppendLine("SELECT COUNT(Id) as totalCount");

            query.AppendLine("FROM ( SELECT d.Id FROM dbo.[tblCardEvent] e WITH(NOLOCK)");
            query.AppendLine("LEFT JOIN dbo.[tblCardEventDelete] d");
            query.AppendLine("ON d.EventId = CONVERT(nvarchar(50), e.Id)");
            query.AppendLine("WHERE 1 = 1 ");
            query.AppendLine(string.Format("{0}", string.Format("AND d.[DateCreated] >= '{0}' AND d.[DateCreated] <= '{1}' ", fromdate, todate)));


            //Nhom the
            if (!string.IsNullOrWhiteSpace(CardGroupID))
            {
                var t = CardGroupID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and e.CardGroupID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            //Lan
            if (!string.IsNullOrWhiteSpace(LaneID))
            {
                var t = LaneID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;
                    var _count = 0;
                    query.AppendLine("and (e.LaneIDIn IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" ) ");

                    query.AppendLine("OR e.[LaneIDOut] IN (");

                    foreach (var item in t)
                    {
                        _count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, _count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" ) )");
                }
            }

            //User
            if (!string.IsNullOrWhiteSpace(UserID))
            {
                var t = UserID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and d.UserId IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            //Nhom KH
            if (CustomerGroupID.Any())
            {
                query.AppendLine("AND e.CustomerGroupID IN (");

                var count = 0;
                foreach (var item in CustomerGroupID)
                {
                    count++;
                    query.AppendLine(string.Format("'{0}'{1}", item, count == CustomerGroupID.Count ? "" : ","));
                }

                query.AppendLine(")");
            }

            if (!string.IsNullOrWhiteSpace(KeyWord))
                query.AppendLine(string.Format("AND (e.[CardNumber] LIKE '%{0}%' OR e.[CardNo] LIKE '%{0}%' OR e.[PlateIn] LIKE '%{0}%' OR e.[PlateOut] LIKE '%{0}%')", KeyWord.Trim()));

            query.AppendLine(") as e");

            var list = ExcuteSQLEvent.GetDataSet(query.ToString(), false);
            total = list.Tables.Count > 1 ? Convert.ToInt32(list.Tables[1].Rows[0]["totalCount"].ToString()) : 0;
            return ExcuteSQLEvent.ConvertTo<ReportInOut108>(list.Tables[0]);
        }

        public DataTable GetHistoryDeleteEvent_Excel(string KeyWord, List<string> CustomerGroupID, bool IsFilterByTimeIn, string fromdate, string todate, string CardGroupID, string LaneID, string UserID, string CustomerGroupId, ref int total)
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
            query.AppendLine("SELECT * FROM(");
            //query.AppendLine("SELECT ROW_NUMBER() OVER(ORDER BY a.[DateCreated] desc) as RowNumber,a.*");
            query.AppendLine("SELECT ROW_NUMBER() OVER(ORDER BY a.[DateCreated] desc) AS STT,");

            query.AppendLine("(select convert(varchar(10), a.DateCreated, 103) + ' ' + left(convert(varchar(32), a.DateCreated, 108), 8)) AS 'Ngày xóa',a.[UserId] as 'Người xóa', a.[Moneys] AS 'Số tiền',a.[CardNo], a.[CardNumber] AS 'Mã thẻ', a.[Plate] AS 'Biển số', (select convert(varchar(10), a.DateTimeIn, 103) + ' ' + left(convert(varchar(32), a.DateTimeIn, 108), 8)) AS 'Thời gian vào', (select convert(varchar(10), a.DatetimeOut, 103) + ' ' + left(convert(varchar(32), a.DatetimeOut, 108), 8)) AS 'Thời gian ra', a.[CardGroupID] AS 'Nhóm thẻ', a.[CustomerName] AS 'Khách hàng', a.[LaneIDIn] AS 'Làn vào', a.[LaneIDOut] AS 'Làn ra', a.[UserIDIn] AS 'Giám sát vào', a.[UserIDOut] AS 'Giám sát ra'");

            query.AppendLine("FROM(");

            query.AppendLine("SELECT CONVERT(nvarchar(50), e.[Id]) + '_CARD' as Id, e.[CardNo], e.[CardNumber], CAST(CASE WHEN e.[PlateIn] <> '' THEN e.[PlateIn] ELSE e.[PlateOut] END AS nvarchar(50)) as Plate, e.[DatetimeIn], e.[DatetimeOut], e.[PicDirIn] as PicIn1, REPLACE(e.[PicDirIn], 'PLATEIN.JPG', 'OVERVIEWIN.JPG') as PicIn2, e.[PicDirOut] as PicOut1, REPLACE(e.[PicDirOut], 'PLATEOUT.JPG', 'OVERVIEWOUT.JPG') as PicOut2, e.[CardGroupID], e.[CustomerName], e.[LaneIDIn], e.[LaneIDOut], e.[UserIDIn], e.[UserIDOut], e.[Moneys],d.[DateCreated],d.[UserId]");

            query.AppendLine("FROM dbo.[tblCardEvent] e WITH(NOLOCK)");
            query.AppendLine("LEFT JOIN dbo.[tblCardEventDelete] d WITH(NOLOCK)");
            query.AppendLine("ON d.EventId = CONVERT(nvarchar(50), e.Id)");
            query.AppendLine("WHERE 1 = 1 ");

            //query.AppendLine(string.Format("{0}", IsFilterByTimeIn ? string.Format("AND e.[DatetimeIn] >= '{0}' AND e.[DatetimeIn] <= '{1}' ", fromdate, todate) : string.Format("AND e.[DatetimeOut] >= '{0}' AND e.[DatetimeOut] <= '{1}' ", fromdate, todate)));
            query.AppendLine(string.Format("{0}", string.Format("AND d.[DateCreated] >= '{0}' AND d.[DateCreated] <= '{1}' ", fromdate, todate)));

            //Nhom the
            if (!string.IsNullOrWhiteSpace(CardGroupID))
            {
                var t = CardGroupID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and e.CardGroupID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            //Lan
            if (!string.IsNullOrWhiteSpace(LaneID))
            {
                var t = LaneID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;
                    var _count = 0;
                    query.AppendLine("and (e.LaneIDIn IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" ) ");

                    query.AppendLine("OR e.[LaneIDOut] IN (");

                    foreach (var item in t)
                    {
                        _count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, _count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" ) )");
                }
            }

            //User
            if (!string.IsNullOrWhiteSpace(UserID))
            {
                var t = UserID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and d.UserId IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            //Nhom KH
            if (CustomerGroupID.Any())
            {
                query.AppendLine("AND e.CustomerGroupID IN (");

                var count = 0;
                foreach (var item in CustomerGroupID)
                {
                    count++;
                    query.AppendLine(string.Format("'{0}'{1}", item, count == CustomerGroupID.Count ? "" : ","));
                }

                query.AppendLine(")");
            }

            if (!string.IsNullOrWhiteSpace(KeyWord))
                query.AppendLine(string.Format("AND (e.[CardNumber] LIKE '%{0}%' OR e.[CardNo] LIKE '%{0}%' OR e.[PlateIn] LIKE '%{0}%' OR e.[PlateOut] LIKE '%{0}%')", KeyWord.Trim()));

            query.AppendLine(") as a");
            query.AppendLine(") as TEMP");
            // query.AppendLine(string.Format("WHERE RowNumber BETWEEN (({0}-1) * {1} + 1) AND ({0} * {1})", pageIndex, pageSize));

            //--Count Total
            query.AppendLine("SELECT COUNT(Id) as totalCount, SUM(Moneys) as totalMoney");

            query.AppendLine("FROM ( SELECT d.Id,e.Moneys FROM dbo.[tblCardEvent] e WITH(NOLOCK)");
            query.AppendLine("LEFT JOIN dbo.[tblCardEventDelete] d");
            query.AppendLine("ON d.EventId = CONVERT(nvarchar(50), e.Id)");
            query.AppendLine("WHERE 1 = 1 ");
            query.AppendLine(string.Format("{0}", string.Format("AND d.[DateCreated] >= '{0}' AND d.[DateCreated] <= '{1}' ", fromdate, todate)));


            //Nhom the
            if (!string.IsNullOrWhiteSpace(CardGroupID))
            {
                var t = CardGroupID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and e.CardGroupID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            //Lan
            if (!string.IsNullOrWhiteSpace(LaneID))
            {
                var t = LaneID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;
                    var _count = 0;
                    query.AppendLine("and (e.LaneIDIn IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" ) ");

                    query.AppendLine("OR e.[LaneIDOut] IN (");

                    foreach (var item in t)
                    {
                        _count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, _count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" ) )");
                }
            }

            //User
            if (!string.IsNullOrWhiteSpace(UserID))
            {
                var t = UserID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and d.UserId IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            //Nhom KH
            if (CustomerGroupID.Any())
            {
                query.AppendLine("AND e.CustomerGroupID IN (");

                var count = 0;
                foreach (var item in CustomerGroupID)
                {
                    count++;
                    query.AppendLine(string.Format("'{0}'{1}", item, count == CustomerGroupID.Count ? "" : ","));
                }

                query.AppendLine(")");
            }

            if (!string.IsNullOrWhiteSpace(KeyWord))
                query.AppendLine(string.Format("AND (e.[CardNumber] LIKE '%{0}%' OR e.[CardNo] LIKE '%{0}%' OR e.[PlateIn] LIKE '%{0}%' OR e.[PlateOut] LIKE '%{0}%')", KeyWord.Trim()));

            query.AppendLine(") as e");

            var list = ExcuteSQLEvent.GetDataSet(query.ToString(), false);
            total = list.Tables.Count > 1 ? Convert.ToInt32(list.Tables[1].Rows[0]["totalCount"].ToString()) : 0;
            var totalmoney = list.Tables.Count > 1 && !string.IsNullOrEmpty(list.Tables[1].Rows[0]["totalMoney"].ToString()) && Convert.ToDecimal(list.Tables[1].Rows[0]["totalMoney"].ToString()) > 0 ? Convert.ToDecimal(list.Tables[1].Rows[0]["totalMoney"].ToString()).ToString("###,###") : "0";

            var dt = list.Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                dt.Rows.Add(null, "Tổng", "", totalmoney, "", "", "", "", "", "", "", "", "", "", "");
            }

            return dt;
        }

        public bool RestoreDeleteEvent(string KeyWord, List<string> CustomerGroupID, string fromdate, string todate, string CardGroupID, string LaneID, string UserID, string RowId)
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
            query.AppendLine("Update tblCardEvent ");
            query.AppendLine("set IsDelete = 'False'");
            query.AppendLine("where Id IN ( SELECT d.EventId FROM dbo.[tblCardEventDelete] d WITH(NOLOCK)");
            query.AppendLine("LEFT JOIN dbo.[tblCardEvent] e");
            query.AppendLine("ON d.EventId = CONVERT(nvarchar(50), e.Id)");
            query.AppendLine("WHERE 1 = 1 ");
            query.AppendLine(string.Format("{0}", string.Format("AND d.[DateCreated] >= '{0}' AND d.[DateCreated] <= '{1}' ", fromdate, todate)));

            //Nhom the
            if (!string.IsNullOrWhiteSpace(CardGroupID))
            {
                var t = CardGroupID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and e.CardGroupID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            //Lan
            if (!string.IsNullOrWhiteSpace(LaneID))
            {
                var t = LaneID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;
                    var _count = 0;
                    query.AppendLine("and (e.LaneIDIn IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" ) ");

                    query.AppendLine("OR e.[LaneIDOut] IN (");

                    foreach (var item in t)
                    {
                        _count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, _count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" ) )");
                }
            }

            //User
            if (!string.IsNullOrWhiteSpace(UserID))
            {
                var t = UserID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and d.UserId IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            //Nhom KH
            if (CustomerGroupID.Any())
            {
                query.AppendLine("AND e.CustomerGroupID IN (");

                var count = 0;
                foreach (var item in CustomerGroupID)
                {
                    count++;
                    query.AppendLine(string.Format("'{0}'{1}", item, count == CustomerGroupID.Count ? "" : ","));
                }

                query.AppendLine(")");
            }

            if (!string.IsNullOrWhiteSpace(KeyWord))
                query.AppendLine(string.Format("AND (e.[CardNumber] LIKE '%{0}%' OR e.[CardNo] LIKE '%{0}%' OR e.[PlateIn] LIKE '%{0}%' OR e.[PlateOut] LIKE '%{0}%')", KeyWord.Trim()));

            if (!string.IsNullOrEmpty(RowId))
                query.AppendLine(string.Format("AND d.Id = '{0}'", RowId));

            query.AppendLine(")");


            query.AppendLine("delete tblCardEventDelete ");
            query.AppendLine("where Id IN ( SELECT d.Id FROM dbo.[tblCardEventDelete] d WITH(NOLOCK)");
            query.AppendLine("LEFT JOIN dbo.[tblCardEvent] e");
            query.AppendLine("ON d.EventId = CONVERT(nvarchar(50), e.Id)");
            query.AppendLine("WHERE 1 = 1 ");
            query.AppendLine(string.Format("{0}", string.Format("AND d.[DateCreated] >= '{0}' AND d.[DateCreated] <= '{1}' ", fromdate, todate)));

            //Nhom the
            if (!string.IsNullOrWhiteSpace(CardGroupID))
            {
                var t = CardGroupID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and e.CardGroupID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            //Lan
            if (!string.IsNullOrWhiteSpace(LaneID))
            {
                var t = LaneID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;
                    var _count = 0;
                    query.AppendLine("and (e.LaneIDIn IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" ) ");

                    query.AppendLine("OR e.[LaneIDOut] IN (");

                    foreach (var item in t)
                    {
                        _count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, _count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" ) )");
                }
            }

            //User
            if (!string.IsNullOrWhiteSpace(UserID))
            {
                var t = UserID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and d.UserId IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            //Nhom KH
            if (CustomerGroupID.Any())
            {
                query.AppendLine("AND e.CustomerGroupID IN (");

                var count = 0;
                foreach (var item in CustomerGroupID)
                {
                    count++;
                    query.AppendLine(string.Format("'{0}'{1}", item, count == CustomerGroupID.Count ? "" : ","));
                }

                query.AppendLine(")");
            }

            if (!string.IsNullOrWhiteSpace(KeyWord))
                query.AppendLine(string.Format("AND (e.[CardNumber] LIKE '%{0}%' OR e.[CardNo] LIKE '%{0}%' OR e.[PlateIn] LIKE '%{0}%' OR e.[PlateOut] LIKE '%{0}%')", KeyWord.Trim()));

            if (!string.IsNullOrEmpty(RowId))
                query.AppendLine(string.Format("AND d.Id = '{0}'", RowId));

            query.AppendLine(")");
            return ExcuteSQLEvent.Execute(query.ToString());
        }
    }
}
