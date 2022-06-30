using Kztek.Data.Event.Repository;
using Kztek.Data.Event.SqlHelper;
using Kztek.Data.Repository;
using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Model.Models.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Service.Admin.Event
{
    public interface ItblCardEventService
    {
        IEnumerable<tblCardEvent> GetAllByCardNumber(string cardnumber);

        List<BIkeInOUt> CountEventInOut(string fromdate, string todate);

        SelectListModel CountHOME(string fromdate, string todate);

        tblCardEvent GetById(string id);

        MessageReport DeleteById(string id, ref string cardnumber);
        MessageReport DeleteTempById(string id);
        void UpdatePayState(string payid);
        List<CarInOUt> CountEventEveryDayCar(string fromdate, string todate);
        SelectListModel CountEventEveryDay(string fromdate, string todate);
    }

    public class tblCardEventService : ItblCardEventService
    {
        private ItblCardEventRepository _tblCardEventRepository;
        private ItblLoopEventRepository _tblLoopEventRepository;

        private ItblCardService _tblCardService;
        private ItblLaneService _tblLaneService;
        private ItblCustomerService _tblCustomerService;
        private ItblCardGroupService _tblCardGroupService;
        private ItblVehicleGroupService _tblVehicleGroupService;
        public tblCardEventService(ItblCardGroupService _tblCardGroupService,ItblVehicleGroupService _tblVehicleGroupService,ItblCardEventRepository _tblCardEventRepository, ItblLoopEventRepository _tblLoopEventRepository, ItblLaneService _tblLaneService, ItblCardService _tblCardService, ItblCustomerService _tblCustomerService)
        {
            this._tblCardEventRepository = _tblCardEventRepository;
            this._tblLoopEventRepository = _tblLoopEventRepository;
            this._tblCardService = _tblCardService;
            this._tblLaneService = _tblLaneService;
            this._tblVehicleGroupService = _tblVehicleGroupService;
            this._tblCustomerService = _tblCustomerService;
            this._tblCardGroupService = _tblCardGroupService;
        }

        public List<BIkeInOUt> CountEventInOut(string fromdate, string todate)
        {
           

            //
            var fdate = "";
            var tdate = "";
           

            //Check using Loop
            var check = _tblLaneService.GetAllActive().FirstOrDefault(n => n.IsLoop == true);

            //Date time
            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd 00:00:00");
                tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd 00:00:00");
            }
            else
            {
                fdate = DateTime.Now.ToString("yyyy/MM/dd 00:00:00");
                tdate = DateTime.Now.AddDays(1).ToString("yyyy/MM/dd 00:00:00");
            }
            // Xe máy

            //IN
            // Xe máy
            var VehecleTypeObj = _tblVehicleGroupService.GetAll();
            var objBike = VehecleTypeObj.FirstOrDefault(n => n.VehicleType == 1);

            var objCardGrBike = _tblCardGroupService.GetAllByVehicleGroupId(objBike.VehicleGroupID.ToString().ToLower()).ToList();
            var str = new StringBuilder();
            str.AppendLine("SELECT COUNT(Id) AS BikeInCount,EventCode,(Case WHEN EventCode = 1 THEN N'XE VÀO' ELSE N'XE RA' END) AS INOUT, CardGroupID FROM tblCardEvent WITH (NOLOCK)");
            str.AppendLine("WHERE IsDelete = 0 AND EventCode IN  (1,2)");
            str.AppendLine(string.Format("AND ( DateTimeIn >= '{0}' AND DateTimeIn < '{1}' )", fdate, tdate));

            var count = 0;
            if (objCardGrBike.Count != 0)
            {
                str.AppendLine("AND CardGroupID IN ( ");

                foreach (var item in objCardGrBike)
                {
                    count++;
                    str.AppendLine(string.Format("'{0}'{1}", item.CardGroupID.ToString(), count == objCardGrBike.Count() ? "" : ","));

                }
                str.AppendLine(" )");
            }
         

         
            str.AppendLine("  GROUP BY CardGroupID ,EventCode");  
            var bikeInOuts = SqlExQuery<CountBike_CarInOut>.ExcuteQuery(str.ToString());

            var objBikes = _tblCardGroupService.GetCardGrByVehicleBike();
            var lst = new List<BIkeInOUt>();
            foreach (var item in objBikes)
            {

                var o1 = new BIkeInOUt();
                o1.CardGroupID = item.CardGroupID.ToString();
                if (bikeInOuts != null)
                {

                    foreach (var item1 in bikeInOuts)
                    {

                        if (item1.CardGroupID == item.CardGroupID.ToString())
                        {
                          
                            if (item1.EventCode == "1")
                            {
                                o1.BikeIn = item1.BikeInCount;
                            }
                            else if (item1.EventCode == "2")
                            {
                                o1.BikeOut = item1.BikeInCount;
                            }
                            
                        }
                    }
                }
                lst.Add(o1);
               
            }

            //// var obj = VehecleType.
            //var sqlCardEventIN = new StringBuilder();
            //sqlCardEventIN.AppendLine("SELECT * from tblCardEvent WITH (NOLOCK)");
            //sqlCardEventIN.AppendLine("WHERE IsDelete = 0 ");
            //sqlCardEventIN.AppendLine(string.Format("AND ( DateTimeIn >= '{0}' AND DateTimeIn < '{1}' )", fdate, tdate));

            //var cardeIN = SqlExQuery<tblCardEvent>.ExcuteQuery(sqlCardEventIN.ToString()).Count;

            //inNumber += cardeIN;

            //if (check != null)
            //{
            //    var sqlLoopEventIN = new StringBuilder();
            //    sqlLoopEventIN.AppendLine("SELECT * from tblLoopEvent  WITH (NOLOCK)");
            //    sqlLoopEventIN.AppendLine("WHERE IsDelete = 0 AND EventCode = '2'");
            //    sqlLoopEventIN.AppendLine(string.Format("AND ( DateTimeIn >= '{0}' AND DateTimeIn < '{1}' )", fdate, tdate));

            //    var loopeIN = SqlExQuery<tblLoopEvent>.ExcuteQuery(sqlLoopEventIN.ToString()).Count;

            //    inNumber += loopeIN;
            //}


            //OUT


            //var sqlCardEventOUT = new StringBuilder();
            //sqlCardEventOUT.AppendLine("SELECT * from tblCardEvent  WITH (NOLOCK)");
            //sqlCardEventOUT.AppendLine("WHERE IsDelete = 0 AND EventCode = '2'");
            //sqlCardEventOUT.AppendLine(string.Format("AND ( DateTimeOut >= '{0}' AND DateTimeOut < '{1}' )", fdate, tdate));

            //var cardeOUT = SqlExQuery<tblCardEvent>.ExcuteQuery(sqlCardEventOUT.ToString()).Count;

            //outNumber += cardeOUT;

            //if (check != null)
            //{
            //    var sqlLoopEventOUT = new StringBuilder();
            //    sqlLoopEventOUT.AppendLine("SELECT * from tblLoopEvent  WITH (NOLOCK)");
            //    sqlLoopEventOUT.AppendLine("where IsDelete = 0 AND EventCode = '2'");
            //    sqlLoopEventOUT.AppendLine(string.Format("AND ( DateTimeOut >= '{0}' AND DateTimeOut < '{1}' )", fdate, tdate));

            //    var loopeOUT = SqlExQuery<tblLoopEvent>.ExcuteQuery(sqlLoopEventOUT.ToString()).Count;

            //    outNumber += loopeOUT;
            //}

            //model.ItemValue = inNumber.ToString();
            //model.ItemText = outNumber.ToString();

            return lst;
        }

        public MessageReport DeleteById(string id, ref string cardnumber)
        {
            var result = new MessageReport();
            result.Message = "Có lỗi xảy ra";
            result.isSuccess = false;

            try
            {
                var obj = GetById(id);
                if (obj != null)
                {
                    cardnumber = obj.CardNumber;
                }

                var str = string.Format("UPDATE tblCardEvent SET EventCode='2', IsDelete=1 WHERE Id = '{0}'", id);

                SqlExQuery<tblCardEvent>.ExcuteNone(str);

                result.Message = "Xóa thành công";
                result.isSuccess = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.isSuccess = false;
            }

            return result;
        }

        public MessageReport DeleteTempById(string id)
        {
            var result = new MessageReport();
            result.Message = "Có lỗi xảy ra";
            result.isSuccess = false;

            try
            {
                var str = string.Format("Delete CardEventTemp WHERE Id = '{0}'", id);

                ExcuteSQLEvent.Execute(str);

                result.Message = "Xóa thành công";
                result.isSuccess = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.isSuccess = false;
            }

            return result;
        }

        public IEnumerable<tblCardEvent> GetAllByCardNumber(string cardnumber)
        {
            var query = from n in _tblCardEventRepository.Table
                        where n.CardNumber == cardnumber
                        select n;

            return query;
        }

        public SelectListModel CountHOME(string fromdate, string todate)
        {
            //Model send back
            var model = new SelectListModel();

            var card = _tblCardService.GetCount(fromdate,todate).ToList().Count;
            var customer = _tblCustomerService.GetAll().ToList().Count;

            model.ItemText = card.ToString();
            model.ItemValue = customer.ToString();

            return model;
        }

        public tblCardEvent GetById(string id)
        {
            return _tblCardEventRepository.GetById(Guid.Parse(id));
        }


        public void UpdatePayState(string payid)
        {

            var sql = new StringBuilder();
            sql.AppendLine("Update PayIn ");
            sql.AppendLine(string.Format("SET PayState = (CASE WHEN PayState = '1' THEN '0' ELSE'1' END )"));
            sql.AppendLine(string.Format("WHERE ID = {0}",payid));

            ExcuteSQLEvent.Execute(sql.ToString());
        }

        public List<CarInOUt> CountEventEveryDayCar(string fromdate, string todate)
        {
            //
            var fdate = "";
            var tdate = "";


            //Check using Loop
            var check = _tblLaneService.GetAllActive().FirstOrDefault(n => n.IsLoop == true);

            //Date time
            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd 00:00:00");
                tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd 00:00:00");
            }
            else
            {
                fdate = DateTime.Now.ToString("yyyy/MM/dd 00:00:00");
                tdate = DateTime.Now.AddDays(1).ToString("yyyy/MM/dd 00:00:00");
            }
            // O tô

            //In Out
           
            var VehecleTypeObj = _tblVehicleGroupService.GetAll();
            var objBike = VehecleTypeObj.FirstOrDefault(n => n.VehicleType == 0);
            var objCardGrCar = _tblCardGroupService.GetAllByVehicleGroupId(objBike.VehicleGroupID.ToString().ToLower()).ToList();
            var str = new StringBuilder();
            str.AppendLine("SELECT COUNT(Id) AS CarInCount,EventCode,(Case WHEN EventCode = 1 THEN N'XE VÀO' ELSE N'XE RA' END) AS INOUT, CardGroupID FROM tblCardEvent WITH (NOLOCK)");
            str.AppendLine("WHERE IsDelete = 0 AND EventCode IN  (1,2)");
            str.AppendLine(string.Format("AND ( DateTimeIn >= '{0}' AND DateTimeIn < '{1}' )", fdate, tdate));

            var count = 0;
            
            if (objCardGrCar.Count != 0)
            {
                str.AppendLine("AND CardGroupID IN ( ");
                foreach (var item in objCardGrCar)
                {
                    count++;
                    str.AppendLine(string.Format("'{0}'{1}", item.CardGroupID.ToString(), count == objCardGrCar.Count() ? "" : ","));

                }
                str.AppendLine(" )");
            }
           

          

            str.AppendLine("  GROUP BY CardGroupID ,EventCode");

            var CarInOuts = SqlExQuery<CountBike_CarInOut>.ExcuteQuery(str.ToString());

            var objCar = _tblCardGroupService.GetCardGrByVehicleCar();

            var lst = new List<CarInOUt>();
            foreach (var item in objCar)
            {

                var o1 = new CarInOUt();
                o1.CardGroupID = item.CardGroupID.ToString();
                if (CarInOuts != null)
                {

                    foreach (var item1 in CarInOuts)
                    {

                        if (item1.CardGroupID.ToString() == item.CardGroupID.ToString())
                        {

                            if (item1.EventCode == "1")
                            {
                                o1.CarIn = item1.CarInCount;
                            }
                            else if (item1.EventCode == "2")
                            {
                                o1.CarOut = item1.CarInCount;
                            }

                        }
                    }
                }
                lst.Add(o1);

            }
            return lst;
        }

        public SelectListModel CountEventEveryDay(string fromdate, string todate)
        {
            //Model send back
            var model = new SelectListModel();

            //
            var fdate = "";
            var tdate = "";
            int inNumber = 0;
            int outNumber = 0;

            //Check using Loop
            var check = _tblLaneService.GetAllActive().FirstOrDefault(n => n.IsLoop == true);

            //Date time
            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd 00:00:00");
                tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd 00:00:00");
            }
            else
            {
                fdate = DateTime.Now.ToString("yyyy/MM/dd 00:00:00");
                tdate = DateTime.Now.AddDays(1).ToString("yyyy/MM/dd 00:00:00");
            }

            //IN
            var sqlCardEventIN = new StringBuilder();
            sqlCardEventIN.AppendLine("SELECT * from tblCardEvent WITH (NOLOCK)");
            sqlCardEventIN.AppendLine("WHERE IsDelete = 0");
            sqlCardEventIN.AppendLine(string.Format("AND ( DateTimeIn >= '{0}' AND DateTimeIn < '{1}' )", fdate, tdate));

            var cardeIN = SqlExQuery<tblCardEvent>.ExcuteQuery(sqlCardEventIN.ToString()).Count;

            inNumber += cardeIN;

            if (check != null)
            {
                var sqlLoopEventIN = new StringBuilder();
                sqlLoopEventIN.AppendLine("SELECT * from tblLoopEvent  WITH (NOLOCK)");
                sqlLoopEventIN.AppendLine("WHERE IsDelete = 0 AND EventCode = '2'");
                sqlLoopEventIN.AppendLine(string.Format("AND ( DateTimeIn >= '{0}' AND DateTimeIn < '{1}' )", fdate, tdate));

                var loopeIN = SqlExQuery<tblLoopEvent>.ExcuteQuery(sqlLoopEventIN.ToString()).Count;

                inNumber += loopeIN;
            }


            //OUT
            var sqlCardEventOUT = new StringBuilder();
            sqlCardEventOUT.AppendLine("SELECT * from tblCardEvent  WITH (NOLOCK)");
            sqlCardEventOUT.AppendLine("WHERE IsDelete = 0 AND EventCode = '2'");
            sqlCardEventOUT.AppendLine(string.Format("AND ( DateTimeOut >= '{0}' AND DateTimeOut < '{1}' )", fdate, tdate));

            var cardeOUT = SqlExQuery<tblCardEvent>.ExcuteQuery(sqlCardEventOUT.ToString()).Count;

            outNumber += cardeOUT;

            if (check != null)
            {
                var sqlLoopEventOUT = new StringBuilder();
                sqlLoopEventOUT.AppendLine("SELECT * from tblLoopEvent  WITH (NOLOCK)");
                sqlLoopEventOUT.AppendLine("where IsDelete = 0 AND EventCode = '2'");
                sqlLoopEventOUT.AppendLine(string.Format("AND ( DateTimeOut >= '{0}' AND DateTimeOut < '{1}' )", fdate, tdate));

                var loopeOUT = SqlExQuery<tblLoopEvent>.ExcuteQuery(sqlLoopEventOUT.ToString()).Count;

                outNumber += loopeOUT;
            }

            model.ItemValue = inNumber.ToString();
            model.ItemText = outNumber.ToString();

            return model;
        }
    }
}
