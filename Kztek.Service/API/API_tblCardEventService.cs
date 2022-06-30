using Kztek.Data.Infrastructure;
using Kztek.Data.Repository;
using Kztek.Data.SqlHelper;
using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kztek.Web.Core.Functions;
using Kztek.Data.Event.SqlHelper;
using Kztek.Model.Models.Event;
using Kztek.Data.Event.Repository;
using System.Data;

namespace Kztek.Service.API
{
    public interface IAPI_tblCardEventService
    {
        /// <summary>
        /// báo cáo sự kiện vào ra
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cardGroupId"></param>
        /// <param name="isHaveTimeIn"></param>
        /// <param name="vehicleGroupId"></param>
        /// <param name="fromdate"></param>
        /// <param name="todate"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalItem"></param>
        /// <param name="totalPage"></param>
        /// <returns></returns>
        List<ReportInOut_API_3rd_data> GetReportInOut(string key, string cardGroupId, bool isHaveTimeIn, string vehicleGroupId, string fromdate, string todate, int pageIndex, int pageSize, ref int totalItem, ref int totalPage);

        /// <summary>
        /// lấy số lượng xe vào ra, trong bãi theo ngày, theo giờ
        /// </summary>
        /// <param name="cardGroupId"></param>
        /// <param name="vehicleGroupId"></param>
        /// <param name="fromdate"></param>
        /// <param name="todate"></param>
        /// <returns></returns>
        List<ReportTotalInOut_Day> GetTotalInOut_Day(string cardGroupId, string vehicleGroupId, DateTime fromdate, DateTime todate);
        long GetTotalMoney(string Key, string CardGroupId, string VehicleGroupId, string Fromdate, string Todate);

        IEnumerable<tblCardEvent> GetAllByCardNumber(string cardnumber);
        bool CheckCardInEvent(string cardnumber);
        IEnumerable<tblVehicleGroup> GetAllVehicleGroup();
    }

    public class API_tblCardEventService : IAPI_tblCardEventService
    {
        private ItblCardRepository _tblCardRepository;
        private ItblCardEventRepository _tblCardEventRepository;
        private ItblVehicleGroupRepository _tblVehicleGroupRepository;

        public API_tblCardEventService(ItblCardRepository _tblCardRepository, ItblCardEventRepository _tblCardEventRepository, ItblVehicleGroupRepository _tblVehicleGroupRepository)
        {
            this._tblCardRepository = _tblCardRepository;
            this._tblCardEventRepository = _tblCardEventRepository;
            this._tblVehicleGroupRepository = _tblVehicleGroupRepository;
        }

        public bool CheckCardInEvent(string cardnumber)
        {
            var query = new StringBuilder();

            query.AppendLine("SELECT TOP 1(e.Id) FROM dbo.[tblCardEvent] e WITH(NOLOCK)");
            query.AppendLine(string.Format("Where e.[CardNumber] = '{0}'", cardnumber));
            var obj = ExcuteSQLEvent.GetDataSet(query.ToString(), false);
            return ExcuteSQLEvent.ConvertTo<ReportInOut_API_3rd_data>(obj.Tables[0]).Count > 0 ? true : false;
        }

        public IEnumerable<tblCardEvent> GetAllByCardNumber(string cardnumber)
        {
            var query = from n in _tblCardEventRepository.Table
                        where n.CardNumber == cardnumber
                        select n;

            return query;
        }

        public IEnumerable<tblVehicleGroup> GetAllVehicleGroup()
        {
            var query = from n in _tblVehicleGroupRepository.Table
                        select n;

            return query;
        }


        /// <summary>
        /// get event in out
        /// </summary>
        /// <param name="KeyWord"></param>
        /// <param name="IsHaveTimeIn"></param>
        /// <param name="fromdate"></param>
        /// <param name="todate"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<ReportInOut_API_3rd_data> GetReportInOut(string key, string cardGroupId, bool isHaveTimeIn, string vehicleGroupId, string fromdate, string todate, int pageIndex, int pageSize, ref int totalItem, ref int totalPage)
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
            query.AppendLine("SELECT ROW_NUMBER() OVER(ORDER BY a.[DatetimeOut] ASC) as RowNumber,a.*");
            query.AppendLine("FROM(");
            query.AppendLine("SELECT  e.Id, e.[CardNumber], CAST(CASE WHEN e.[PlateOut] <> '' THEN e.[PlateOut] ELSE e.[PlateIn] END AS nvarchar(50)) as Plate, CONVERT(VARCHAR, e.[DatetimeIn], 126) AS DatetimeIn ,CONVERT(VARCHAR, e.[DatetimeOut], 126) AS DatetimeOut,  e.[Moneys], e.[CardGroupID],e.[VehicleGroupID],e.CustomerName,e.CustomerGroupID");
            query.AppendLine("FROM dbo.[tblCardEvent] e WITH(NOLOCK)");

            if (isHaveTimeIn)
                query.AppendLine("WHERE e.[IsDelete] = 0 and e.[EventCode] = '1'");
            else
                query.AppendLine("WHERE e.[IsDelete] = 0 and e.[EventCode] = '2'");

            query.AppendLine(string.Format("{0}", isHaveTimeIn ? string.Format("AND e.[DatetimeIn] >= '{0}' AND e.[DatetimeIn] <= '{1}' ", fromdate, todate) : string.Format("AND e.[DatetimeOut] >= '{0}' AND e.[DatetimeOut] <= '{1}' ", fromdate, todate)));

            if (!string.IsNullOrWhiteSpace(key))
                query.AppendLine(string.Format("AND (e.[CardNumber] LIKE '%{0}%' OR REPLACE(REPLACE(e.[PlateIn], '-', ''), '.', '') LIKE '%{0}%' OR  REPLACE(REPLACE(e.[PlateOut], '-', ''), '.', '') LIKE '%{0}%')", key));
            if (!string.IsNullOrWhiteSpace(cardGroupId))
                query.AppendLine(string.Format("AND e.[CardGroupID] = '{0}'", cardGroupId));
            if (!string.IsNullOrWhiteSpace(vehicleGroupId))
                query.AppendLine(string.Format("AND e.[VehicleGroupID] = '{0}'", vehicleGroupId));

            query.AppendLine(") as a");
            query.AppendLine(") as TEMP");
            query.AppendLine(string.Format("WHERE RowNumber BETWEEN (({0}-1) * {1} + 1) AND ({0} * {1})", pageIndex, pageSize));

            //--Count Total
            query.AppendLine("SELECT COUNT(Id) as totalCount");
            query.AppendLine("FROM ( SELECT Id FROM dbo.[tblCardEvent]");
            query.AppendLine("e WITH(NOLOCK)");

            if (isHaveTimeIn)
                query.AppendLine("WHERE e.[IsDelete] = 0 and e.[EventCode] = '1'");
            else
                query.AppendLine("WHERE e.[IsDelete] = 0 and e.[EventCode] = '2'");

            query.AppendLine(string.Format("{0}", isHaveTimeIn ? string.Format("AND e.[DatetimeIn] >= '{0}' AND e.[DatetimeIn] <= '{1}' ", fromdate, todate) : string.Format("AND e.[DatetimeOut] >= '{0}' AND e.[DatetimeOut] <= '{1}' ", fromdate, todate)));

            ////Nhom the
            //if (!string.IsNullOrWhiteSpace(CardGroupID))
            //{
            //    var t = CardGroupID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //    if (t.Any())
            //    {
            //        var count = 0;

            //        query.AppendLine("and e.CardGroupID IN ( ");

            //        foreach (var item in t)
            //        {
            //            count++;

            //            query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
            //        }

            //        query.AppendLine(" )");
            //    }
            //}

            if (!string.IsNullOrWhiteSpace(key))
                query.AppendLine(string.Format("AND (e.[CardNumber] LIKE '%{0}%' OR REPLACE(REPLACE(e.[PlateIn], '-', ''), '.', '') LIKE '%{0}%' OR REPLACE(REPLACE(e.[PlateOut], '-', ''), '.', '') LIKE '%{0}%')", key));
            if (!string.IsNullOrWhiteSpace(cardGroupId))
                query.AppendLine(string.Format("AND e.[CardGroupID] = '{0}'", cardGroupId));
            if (!string.IsNullOrWhiteSpace(vehicleGroupId))
                query.AppendLine(string.Format("AND e.[VehicleGroupID] = '{0}'", vehicleGroupId));

            query.AppendLine(") as e");

            var list = ExcuteSQLEvent.GetDataSet(query.ToString(), false);

            totalItem = list.Tables.Count > 1 ? Convert.ToInt32(list.Tables[1].Rows[0]["totalCount"].ToString()) : 0;
            totalPage = totalItem % pageSize > 0 ? totalItem / pageSize + 1 : totalItem / pageSize;
            return ExcuteSQLEvent.ConvertTo<ReportInOut_API_3rd_data>(list.Tables[0]);
        }

        public long GetTotalMoney(string Key, string CardGroupId, string VehicleGroupId, string Fromdate, string Todate)
        {
            long money = 0;
            if (!string.IsNullOrEmpty(Fromdate))
            {
                Fromdate = Convert.ToDateTime(Fromdate).ToString("yyyy/MM/dd HH:mm:ss");
            }
            if (!string.IsNullOrEmpty(Todate))
            {
                Todate = Convert.ToDateTime(Todate).ToString("yyyy/MM/dd HH:mm:ss");
            }
            var query = new StringBuilder();

            query.AppendLine("SELECT  ISNULL(SUM(e.Moneys), 0) AS Moneys");
            query.AppendLine("FROM dbo.[tblCardEvent] e WITH(NOLOCK)");
            query.AppendLine("WHERE e.[IsDelete] = 0 and e.[EventCode] = '2'");

            query.AppendLine(string.Format("AND e.[DatetimeOut] >= '{0}' AND e.[DatetimeOut] <= '{1}' ", Fromdate, Todate));

            if (!string.IsNullOrWhiteSpace(Key))
                query.AppendLine(string.Format("AND (e.[CardNumber] LIKE '%{0}%' OR REPLACE(REPLACE(e.[PlateIn], '-', ''), '.', '') LIKE '%{0}%' OR  REPLACE(REPLACE(e.[PlateOut], '-', ''), '.', '') LIKE '%{0}%')", Key));
            if (!string.IsNullOrWhiteSpace(CardGroupId))
                query.AppendLine(string.Format("AND e.[CardGroupID] = '{0}'", CardGroupId));
            if (!string.IsNullOrWhiteSpace(VehicleGroupId))
                query.AppendLine(string.Format("AND e.[VehicleGroupID] = '{0}'", VehicleGroupId));

            money = ExcuteSQLEvent.ExecuteReturnMoney(query.ToString());
            return money;
        }


        /// <summary>
        /// lấy số lượng xe vào ra, trong bãi theo ngày, theo giờ
        /// </summary>
        /// <param name="cardGroupId"></param>
        /// <param name="vehicleGroupId"></param>
        /// <param name="fromdate"></param>
        /// <param name="todate"></param>
        /// <returns></returns>
        public List<ReportTotalInOut_Day> GetTotalInOut_Day(string cardGroupId, string vehicleGroupId, DateTime fromdate, DateTime todate)
        {
            bool continueLoop = true;
            int i = 0;
            List<ReportTotalInOut_Day> lst = new List<ReportTotalInOut_Day>();
            while (continueLoop)
            {
                //Tdate tính theo ngày bằng fromdate + 1 day
                var Fdate = fromdate;
                var Tdate = fromdate.AddDays(1);

                string TDateStr = Tdate.ToString("yyyy/MM/dd 00:00:00");
                string FDateStr = fromdate.ToString("yyyy/MM/dd HH:mm:ss");

                if (Tdate > todate.AddDays(1))
                {
                    continueLoop = false;
                    break;
                }
                else
                {
                    var query = new StringBuilder();

                    //DS Tổng số xe vào bãi trong 1 ngày
                    query.AppendLine("SELECT e.Id, e.EventCode, e.DatetimeIn, e.DateTimeOut FROM dbo.[tblCardEvent] e WITH(NOLOCK)");
                    query.AppendLine("WHERE e.[IsDelete] = 0 ");

                    query.AppendLine(string.Format("and (e.[EventCode] = '1' AND e.[DatetimeIn] >= '{0}' AND e.[DatetimeIn] < '{1}') OR(e.[EventCode] = '2' AND e.[DatetimeIn] >= '{0}' AND e.[DatetimeIn] < '{1}')", FDateStr, TDateStr));

                    if (!string.IsNullOrWhiteSpace(cardGroupId))
                        query.AppendLine(string.Format("AND e.[CardGroupID] = '{0}'", cardGroupId));
                    if (!string.IsNullOrWhiteSpace(vehicleGroupId))
                        query.AppendLine(string.Format("AND e.[VehicleGroupID] = '{0}'", vehicleGroupId));

                    var Count_event_in = convertDataSet(ExcuteSQLEvent.GetDataSet(query.ToString(), false));

                    // DS tổng số xe ra khỏi bãi trong 1 ngày
                    query = new StringBuilder();
                    query.AppendLine("SELECT e.Id, e.EventCode, e.DatetimeIn, e.DateTimeOut FROM dbo.[tblCardEvent] e WITH(NOLOCK)");
                    query.AppendLine("WHERE e.[IsDelete] = 0 AND e.DatetimeOut is not null ");

                    query.AppendLine(string.Format("AND e.[EventCode] = '2' AND e.DatetimeOut >= '{0}' AND e.DatetimeOut < '{1}'", FDateStr, TDateStr));

                    if (!string.IsNullOrWhiteSpace(cardGroupId))
                        query.AppendLine(string.Format("AND e.[CardGroupID] = '{0}'", cardGroupId));
                    if (!string.IsNullOrWhiteSpace(vehicleGroupId))
                        query.AppendLine(string.Format("AND e.[VehicleGroupID] = '{0}'", vehicleGroupId));

                    var Count_event_out = convertDataSet(ExcuteSQLEvent.GetDataSet(query.ToString(), false));


                    //DS tổng số xe trong bãi trong 1 ngày
                    query = new StringBuilder();
                    query.AppendLine("SELECT e.Id, e.EventCode, e.DatetimeIn, e.DateTimeOut FROM dbo.[tblCardEvent] e WITH(NOLOCK)");
                    query.AppendLine("WHERE e.[IsDelete] = 0 ");

                    query.AppendLine(string.Format("and e.[EventCode] = '1' AND e.[DatetimeIn] >= '{0}' AND e.[DatetimeIn] < '{1}'", FDateStr, TDateStr));

                    if (!string.IsNullOrWhiteSpace(cardGroupId))
                        query.AppendLine(string.Format("AND e.[CardGroupID] = '{0}'", cardGroupId));
                    if (!string.IsNullOrWhiteSpace(vehicleGroupId))
                        query.AppendLine(string.Format("AND e.[VehicleGroupID] = '{0}'", vehicleGroupId));

                    var Count_Vehicle_In_Parking = convertDataSet(ExcuteSQLEvent.GetDataSet(query.ToString(), false));
                    var list_in_out_hour = new List<ReportTotalInOut_Hours>();
                    for (int hour = 0; hour < 24; hour++)
                    {
                        var timeStart = Convert.ToDateTime(fromdate.ToString("yyyy/MM/dd ") + hour + ":00");
                        var timeEnd = timeStart.AddHours(1);

                        var objHour = new ReportTotalInOut_Hours();
                        objHour.STT = hour + 1;
                        objHour.Hours = hour + ":00 - " + hour + ":59";
                        objHour.TotalIn = Count_event_in.Where(n => n.DatetimeIn >= timeStart && n.DatetimeIn < timeEnd).Count();
                        objHour.TotalOut = Count_event_out.Where(n => n.DateTimeOut >= timeStart && n.DateTimeOut < timeEnd).Count();
                        objHour.TotalIn_Parking = Count_Vehicle_In_Parking.Where(n => n.DatetimeIn < timeEnd).Count();
                        list_in_out_hour.Add(objHour);
                    }
                    var objDay = new ReportTotalInOut_Day();
                    objDay.STT = i+1;
                    objDay.Date = fromdate.ToString("dd/MM/yyyy");
                    objDay.TotalIn = Count_event_in.Count();
                    objDay.TotalOut = Count_event_out.Count();
                    objDay.TotalIn_Parking = Count_Vehicle_In_Parking.Count();
                    objDay.InOutByHour = list_in_out_hour;
                    lst.Add(objDay);

                    fromdate = Tdate;
                }



            }
            return lst;
        }

        private List<tblCardEvent_api> convertDataSet(DataSet ds)
        {
            var lstEvent = new List<tblCardEvent_api>();
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                tblCardEvent_api obj = new tblCardEvent_api()
                {
                    Id = item["Id"].ToString(),
                    EventCode = item["EventCode"].ToString(),
                    DatetimeIn = DateTime.Parse(item["DatetimeIn"].ToString()),
                    DateTimeOut = DateTime.Parse(item["DatetimeIn"].ToString()),
                };
                lstEvent.Add(obj);
            }
            return lstEvent;
        }
    }
}

