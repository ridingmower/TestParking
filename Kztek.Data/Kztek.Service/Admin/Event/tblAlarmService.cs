using Kztek.Data.Event.Infrastructure;
using Kztek.Data.Event.Repository;
using Kztek.Data.Event.SqlHelper;
using Kztek.Data.Repository;
using Kztek.Model.CustomModel.iParking.Event;
using Kztek.Model.Models;
using Kztek.Model.Models.Event;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Helpers;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Service.Admin.Event
{
    public interface ItblAlarmService
    {
        List<ReportAlarm> GetAllPagingByFirst(string cardnumber, string key, string user, string lane, string alarmCode, string fromdate, string todate, int pageNumber, int pageSize, ref int total);

        DataTable ReportGetAllPagingByFirst(string key, string user, string lane, string alarmCode, string dateFromPicker);

        List<ReportAlarm> GetAllPagingByFirst_TRANSERCO(string key, string user, string lane, string alarmCode, string fromdate, string todate, int pageNumber, int pageSize, ref int total);
        DataTable ReportGetAllPagingByFirst_TRANSERCO(string key, string user, string lane, string alarmCode, string dateFromPicker);
    }

    public class tblAlarmService : ItblAlarmService
    {
        private ItblAlarmRepository _tblAlarmRepository;
        private ItblLaneRepository _tblLaneRepository;
        private IUserRepository _UserRepository;
        private IUnitOfWork _UnitOfWork;
        private ItblLaneService _tblLaneService;
        private IUserService _UserService;

        public tblAlarmService(ItblAlarmRepository _tblAlarmRepository, ItblLaneRepository _tblLaneRepository, IUserRepository _UserRepository, ItblLaneService _tblLaneService, IUserService _UserService, IUnitOfWork _UnitOfWork)
        {
            this._tblAlarmRepository = _tblAlarmRepository;
            this._tblLaneRepository = _tblLaneRepository;
            this._UserRepository = _UserRepository;
            this._tblLaneService = _tblLaneService;
            this._UserService = _UserService;
            this._UnitOfWork = _UnitOfWork;
        }

        public List<ReportAlarm> GetAllPagingByFirst(string cardnumber, string key, string user, string lane, string alarmCode, string fromdate, string todate, int pageNumber, int pageSize, ref int total)
        {
            var keyword = key.Replace("-", "").Replace(".", "").Replace(" ", "");
            string database = "MPARKING";
            if (!string.IsNullOrEmpty(fromdate))
            {
                fromdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd HH:mm:ss");
            }
            else
            {
                fromdate = Convert.ToDateTime(DateTime.Now).ToString("yyyy/MM/dd HH:mm:ss");
            }
            if (!string.IsNullOrEmpty(todate))
            {
                todate = Convert.ToDateTime(todate).ToString("yyyy/MM/dd HH:mm:ss");
            }
            else
            {
                todate = Convert.ToDateTime(DateTime.Now).ToString("yyyy/MM/dd HH:mm:ss");
            }

            var query = new StringBuilder();
            query.AppendLine("select DISTINCT * FROM(");
            query.AppendLine("SELECT ROW_NUMBER() OVER(ORDER BY a.[Date] desc) AS RowNumber, a.*,c.CardNo");
            query.AppendLine("FROM dbo.[tblAlarm] a WITH (NOLOCK)");
            query.AppendLine("left join [MPARKING].dbo.[tblCard] c on c.CardNumber = a.CardNumber  and c.IsLock = '0' AND c.IsDelete = '0' ");

            query.AppendLine("WHERE 1 = 1");
            query.AppendLine(string.Format("AND a.[Date] >= '{0}' AND a.[Date] <= '{1}'", fromdate, todate));

            //if (!string.IsNullOrWhiteSpace(alarmCode))
            //    query.AppendLine(string.Format("AND a.[AlarmCode] = '{0}'", alarmCode));

            if (!string.IsNullOrWhiteSpace(alarmCode))
            {
                var t = alarmCode.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and a.[AlarmCode] IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            if (!string.IsNullOrWhiteSpace(lane))
            {
                var t = lane.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and LaneID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }
            //if (!string.IsNullOrWhiteSpace(cardnumber))
            //{
            //    var t = cardnumber.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //    if (t.Any())
            //    {
            //        var count = 0;

            //        query.AppendLine("and a.CardNumber IN ( ");

            //        foreach (var item in t)
            //        {
            //            count++;

            //            query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
            //        }

            //        query.AppendLine(" )");
            //    }
            //}
           if (!string.IsNullOrWhiteSpace(keyword))
                {
                    query.AppendLine(string.Format("AND (a.[CardNumber] LIKE '%{0}%' OR REPLACE(REPLACE([Plate], '-', ''), '.', '') LIKE '%{0}%'  OR c.CardNo LIKE '%{0}%')", keyword));

                }

            
            if (!string.IsNullOrWhiteSpace(user))
            {
                var t = user.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
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
            query.AppendLine(") as C");
            query.AppendLine(string.Format("WHERE RowNumber BETWEEN (({0}-1) * {1} + 1) AND ({0} * {1})", pageNumber, pageSize));

            //COUNT TOTAL
            query.AppendLine("SELECT COUNT(Id) totalCount");
            query.AppendLine("FROM dbo.[tblAlarm] a WITH (NOLOCK)");
            query.AppendLine("left join [MPARKING].dbo.[tblCard] c on c.CardNumber = a.CardNumber  and c.IsLock = '0' AND c.IsDelete = '0' ");

            query.AppendLine("WHERE 1 = 1");
            query.AppendLine(string.Format("AND a.[Date] >= '{0}' AND a.[Date] <= '{1}'", fromdate, todate));

            //if (!string.IsNullOrWhiteSpace(alarmCode))
            //    query.AppendLine(string.Format("AND a.[AlarmCode] = '{0}'", alarmCode));

            if (!string.IsNullOrWhiteSpace(alarmCode))
            {
                var t = alarmCode.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and a.[AlarmCode] IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            if (!string.IsNullOrWhiteSpace(lane))
            {
                var t = lane.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and LaneID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            if (!string.IsNullOrWhiteSpace(user))
            {
                var t = user.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
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

            //if (!string.IsNullOrWhiteSpace(cardnumber))
            //{
            //    var t = cardnumber.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //    if (t.Any())
            //    {
            //        var count = 0;

            //        query.AppendLine("and a.CardNumber IN ( ");

            //        foreach (var item in t)
            //        {
            //            count++;

            //            query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
            //        }

            //        query.AppendLine(" )");
            //    }
            //}
            //if
            //{
            //    if (!string.IsNullOrWhiteSpace(keyword))
                {
                    query.AppendLine(string.Format("AND (a.[CardNumber] LIKE '%{0}%' OR REPLACE(REPLACE([Plate], '-', ''), '.', '') LIKE '%{0}%'  OR c.CardNo LIKE '%{0}%')", keyword));

                }

            //}
            var list = ExcuteSQLEvent.GetDataSet(query.ToString(), false);
            total = list.Tables.Count > 1 ? Convert.ToInt32(list.Tables[1].Rows[0]["totalCount"].ToString()) : 0;
            return ExcuteSQLEvent.ConvertTo<ReportAlarm>(list.Tables[0]);
        }

        public List<ReportAlarm> GetAllPagingByFirst_TRANSERCO(string key, string user, string lane, string alarmCode, string fromdate, string todate, int pageNumber, int pageSize, ref int total)
        {
            if (!string.IsNullOrEmpty(fromdate))
            {
                fromdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd HH:mm:ss");
            }
            else
            {
                fromdate = Convert.ToDateTime(DateTime.Now).ToString("yyyy/MM/dd HH:mm:ss");
            }
            if (!string.IsNullOrEmpty(todate))
            {
                todate = Convert.ToDateTime(todate).ToString("yyyy/MM/dd HH:mm:ss");
            }
            else
            {
                todate = Convert.ToDateTime(DateTime.Now).ToString("yyyy/MM/dd HH:mm:ss");
            }

            var query = new StringBuilder();
            query.AppendLine("select DISTINCT * FROM(");
            query.AppendLine("SELECT ROW_NUMBER() OVER(ORDER BY a.[Date] desc) AS RowNumber, a.*");
            query.AppendLine("FROM dbo.[tblAlarm] a WITH (NOLOCK)");
            query.AppendLine("WHERE 1 = 1 AND a.[AlarmCode] != '001'");
            query.AppendLine(string.Format("AND a.[Date] >= '{0}' AND a.[Date] <= '{1}'", fromdate, todate));

            if (!string.IsNullOrWhiteSpace(alarmCode))
                query.AppendLine(string.Format("AND a.[AlarmCode] = '{0}'", alarmCode));

            if (!string.IsNullOrWhiteSpace(lane))
            {
                var t = lane.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and LaneID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            if (!string.IsNullOrWhiteSpace(user))
            {
                var t = user.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
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

            if (!string.IsNullOrWhiteSpace(key))
                query.AppendLine(string.Format("AND (a.[CardNumber] LIKE '%{0}%' OR a.[Plate] LIKE '%{0}%')", key));

            query.AppendLine(") as a");
            query.AppendLine(string.Format("WHERE RowNumber BETWEEN (({0}-1) * {1} + 1) AND ({0} * {1})", pageNumber, pageSize));

            //COUNT TOTAL
            query.AppendLine("SELECT COUNT(Id) totalCount");
            query.AppendLine("FROM dbo.[tblAlarm] a WITH (NOLOCK)");
            query.AppendLine("WHERE 1 = 1 AND a.[AlarmCode] != '001'");
            query.AppendLine(string.Format("AND a.[Date] >= '{0}' AND a.[Date] <= '{1}'", fromdate, todate));

            if (!string.IsNullOrWhiteSpace(alarmCode))
                query.AppendLine(string.Format("AND a.[AlarmCode] = '{0}'", alarmCode));
            if (!string.IsNullOrWhiteSpace(lane))
            {
                var t = lane.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and LaneID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            if (!string.IsNullOrWhiteSpace(user))
            {
                var t = user.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
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
            if (!string.IsNullOrWhiteSpace(key))
                query.AppendLine(string.Format("AND (a.[CardNumber] LIKE '%{0}%' OR a.[Plate] LIKE '%{0}%')", key));

            var list = ExcuteSQLEvent.GetDataSet(query.ToString(), false);
            total = list.Tables.Count > 1 ? Convert.ToInt32(list.Tables[1].Rows[0]["totalCount"].ToString()) : 0;
            return ExcuteSQLEvent.ConvertTo<ReportAlarm>(list.Tables[0]);
        }

        public DataTable ReportGetAllPagingByFirst(string key, string user, string lane, string alarmCode, string dateFromPicker)
        {
            string database = "MPARKING";
            var query = new StringBuilder();
            var keyword = key.Replace("-", "").Replace(".", "").Replace(" ", "");
            //tblAlarm
            //query.AppendLine("select * FROM(");
            //query.AppendLine("SELECT ROW_NUMBER() OVER(ORDER BY a.[Date] desc) AS NumberRow, (select convert(varchar(10), a.[Date], 103) + ' ' + left(convert(varchar(32), a.[Date], 108), 8)) as 'Date'");
            //query.AppendLine(string.Format(",ISNULL((select top 1 CardNo from [{0}].dbo.[tblCard] c where c.IsLock = '0' AND c.IsDelete = '0' and c.CardNumber = a.CardNumber),'') as CardNo", database));
            //query.AppendLine(",a.[CardNumber] as 'CardNumber', a.[Plate] as 'Plate', a.[AlarmCode] as 'AlarmName', a.[LaneID] as 'LaneName', a.Description, a.[UserID] as 'Username'");
            //query.AppendLine("FROM dbo.[tblAlarm] a WITH (NOLOCK)");

            //query.AppendLine("WHERE 1 = 1");

            query.AppendLine("select ROW_NUMBER() OVER(ORDER BY a.[Date] desc) AS NumberRow,  (select convert(varchar(10), a.[Date], 103) + ' ' + (convert(varchar(50), a.[Date], 108))) as 'Date',c.CardNo,");
            query.AppendLine("a.[CardNumber] as 'CardNumber', a.[Plate] as 'Plate', a.[AlarmCode] as 'AlarmName', a.[LaneID] as 'LaneName', a.Description, a.[UserID] as 'Username' ,a.[Description] as 'Description'");
            query.AppendLine("from tblAlarm a");
            query.AppendLine(string.Format("left join [{0}].dbo.[tblCard] c on c.CardNumber = a.CardNumber", database));
            query.AppendLine("WHERE 1 = 1");
            if (!string.IsNullOrWhiteSpace(dateFromPicker))
            {
                //ISODate("2017-10-01T00:00:00.000+07:00")
                var fromdate = Convert.ToDateTime(dateFromPicker.Split(new[] { '-' })[0]).ToString("yyyy-MM-dd HH:mm:00");
                var todate = Convert.ToDateTime(dateFromPicker.Split(new[] { '-' })[1]).ToString("yyyy-MM-dd HH:mm:59");

                query.AppendLine(string.Format("AND a.[Date] >= '{0}' AND a.[Date] <= '{1}'", fromdate, todate));
            }
            else
            {
                //ISODate("2017-10-01T00:00:00.000+07:00")
                var fromdate = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                var todate = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");

                query.AppendLine(string.Format("AND a.[Date] >= '{0}' AND a.[Date] <= '{1}'", fromdate, todate));
            }


            //if (!string.IsNullOrWhiteSpace(alarmCode))
            //    query.AppendLine(string.Format("AND a.[AlarmCode] = '{0}'", alarmCode));

            if (!string.IsNullOrWhiteSpace(alarmCode))
            {
                var t = alarmCode.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and a.[AlarmCode] IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            if (!string.IsNullOrWhiteSpace(lane))
            {
                var t = lane.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and LaneID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            if (!string.IsNullOrWhiteSpace(user))
            {
                var t = user.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
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


            if (!string.IsNullOrWhiteSpace(keyword))
                query.AppendLine(string.Format("AND (a.[CardNumber] LIKE '%{0}%' OR REPLACE(REPLACE(a.[Plate], '-', ''), '.', '') LIKE '%{0}%'  OR c.[CardNo] LIKE '%{0}%')", keyword));


            var list = ExcuteSQLEvent.GetDataSet(query.ToString(), false);

            return list.Tables[0];
        }

        public DataTable ReportGetAllPagingByFirst_TRANSERCO(string key, string user, string lane, string alarmCode, string dateFromPicker)
        {
            var query = new StringBuilder();

            //tblAlarm
            //query.AppendLine("select * FROM(");
            query.AppendLine("SELECT ROW_NUMBER() OVER(ORDER BY a.[Date] desc) AS NumberRow, (select convert(varchar(10), a.[Date], 103) + ' ' + left(convert(varchar(32), a.[Date], 108), 8)) as 'Date',a.[CardNumber] as 'CardNumber', a.[Plate] as 'Plate', a.[AlarmCode] as 'AlarmName', a.[LaneID] as 'LaneName', a.Description, a.[UserID] as 'Username'");

            query.AppendLine("FROM dbo.[tblAlarm] a WITH (NOLOCK)");
            query.AppendLine("WHERE 1 = 1 AND a.[AlarmCode] != '001'");

            if (!string.IsNullOrWhiteSpace(dateFromPicker))
            {
                //ISODate("2017-10-01T00:00:00.000+07:00")
                var fromdate = Convert.ToDateTime(dateFromPicker.Split(new[] { '-' })[0]).ToString("yyyy-MM-dd HH:mm:00");
                var todate = Convert.ToDateTime(dateFromPicker.Split(new[] { '-' })[1]).ToString("yyyy-MM-dd HH:mm:59");

                query.AppendLine(string.Format("AND a.[Date] >= '{0}' AND a.[Date] <= '{1}'", fromdate, todate));
            }
            else
            {
                //ISODate("2017-10-01T00:00:00.000+07:00")
                var fromdate = DateTime.Now.ToString("yyyy-MM-dd 00:00:00");
                var todate = DateTime.Now.ToString("yyyy-MM-dd 23:59:59");

                query.AppendLine(string.Format("AND a.[Date] >= '{0}' AND a.[Date] <= '{1}'", fromdate, todate));
            }


            if (!string.IsNullOrWhiteSpace(alarmCode))
                query.AppendLine(string.Format("AND a.[AlarmCode] = '{0}'", alarmCode));

            if (!string.IsNullOrWhiteSpace(lane))
            {
                var t = lane.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and LaneID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            if (!string.IsNullOrWhiteSpace(user))
            {
                var t = user.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
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

            if (!string.IsNullOrWhiteSpace(key))
                query.AppendLine(string.Format("AND (a.[CardNumber] LIKE '%{0}%' OR a.[Plate] LIKE '%{0}%')", key));

            var list = ExcuteSQLEvent.GetDataSet(query.ToString(), false);

            return list.Tables[0];
        }
    }
}
