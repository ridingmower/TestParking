using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class Report
    {

    }
    public class TotalByCardGroup
    {
        public string VehicleGroupID { get; set; }
        public string CardGroupID { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get; set; }
    }
    public class ReportCardProcess
    {
        public string RowNumber { get; set; }
        public string CardNo { get; set; }
        public string CardNumber { get; set; }
        public string Plate { get; set; }
        public string Plate2 { get; set; }
        public string Date { get; set; }
        public string CardGroupID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerGroupID { get; set; }
        public string Address { get; set; }
        public string UserID { get; set; }
        public string Actions { get; set; }
        public string ADD { get; set; }
        public string RELEASE { get; set; }
        public string CHANGE { get; set; }
        public string RETURN { get; set; }
        public string LOCK { get; set; }
        public string UNLOCK { get; set; }
        public string DELETE { get; set; }
        public string ACTIVE { get; set; }
        public string CHANGEPLA { get; set; }
        public string UPDATEINFO { get; set; }
        public string Plates { get; set; }
        public string OldInfoCP { get; set; }
    }

    public class ReportIn
    {
        public string Id { get; set; }
        public string RowNumber { get; set; }
        public string CardNo { get; set; }
        public bool IsPlateInValid { get; set; }
        public string CardNumber { get; set; }

        public string Plate { get; set; }
        public string DateTimeIn { get; set; }
        public string CardGroupID { get; set; }
        public string CustomerName { get; set; }
        public string LaneIDIn { get; set; }
        public string LaneIDOut { get; set; }
        public string UserIDIn { get; set; }
        public string PicIn1 { get; set; }
        public string PicIn2 { get; set; }
        public string ViTriDo { get; set; }
        public string Moneys { get; set; }
    }
    public class ReportVehicleFreeAll
    {
        public string RowNumber { get; set; }
        public string CardNo { get; set; }
        public string CardNumber { get; set; }
        public string Plate { get; set; }
        public string DateTimeIn { get; set; }
        public string DateTimeOut { get; set; }
        public string CardGroupID { get; set; }
        public string CustomerName { get; set; }
        public string LaneIDIn { get; set; }
        public string LaneIDOut { get; set; }
        public string UserIDIn { get; set; }
        public string UserIDOut { get; set; }
        public double Moneys { get; set; }
        public double MoneyFree { get; set; }
        public string Voucher { get; set; }
        public string PicIn { get; set; }
    }

    public class ReportVehicleFreeAllTRANSERCO
    {
        public string RowNumber { get; set; }
        public string Id { get; set; }
        public string CardNo { get; set; }
        public string CardNumber { get; set; }
        public string Plate { get; set; }
        public string DateTimeIn { get; set; }
        public string DateTimeOut { get; set; }
        public string CardGroupID { get; set; }
        public string CustomerName { get; set; }
        public string LaneIDIn { get; set; }
        public string LaneIDOut { get; set; }
        public string UserIDIn { get; set; }
        public string UserIDOut { get; set; }
        public double Moneys { get; set; }
        public double MoneyFree { get; set; }
        public string Voucher { get; set; }
        public string PicIn { get; set; }
        public string Note { get; set; }
    }

    public class ReportVehicleInAnyTime
    {
        public int RowNumber { get; set; }

        public string VehicleGroupID { get; set; }

        public string VehicleGroupName { get; set; }

        public string VehicleCount { get; set; }
    }

    public class ReportInOut
    {
        public string Id { get; set; }
        public int RowNumber { get; set; }
        public string CardNo { get; set; }
        public string CardNumber { get; set; }
        public bool IsPlateOutValid { get; set; }
        public string Plate { get; set; }
        public string DateTimeIn { get; set; }
        public string DateTimeOut { get; set; }
        public string PicIn1 { get; set; }
        public string PicIn2 { get; set; }
        public string PicOut1 { get; set; }
        public string PicOut2 { get; set; }
        public string CardGroupID { get; set; }
        public string CustomerName { get; set; }
        public string LaneIDIn { get; set; }
        public string LaneIDOut { get; set; }
        public string UserIDIn { get; set; }
        public string UserIDOut { get; set; }
        public string Moneys { get; set; }
        public string DCTypeName { get; set; }
        public string ReducedMoney { get; set; }
    }
    public class ReportInOut_Excel
    {
       
        public int RowNumber { get; set; }
        public string CardNo { get; set; }
        public string CardNumber { get; set; }
        public string PlateOutValid { get; set; }
        public string Plate { get; set; }
        public string DateTimeIn { get; set; }
        public string DateTimeOut { get; set; }
        public string CardGroupName { get; set; }
        public string CustomerName { get; set; }
        public string LaneInName { get; set; }
        public string LaneOutName { get; set; }
        public string UserOutName { get; set; }
        public string UserInName { get; set; }
        public string DCTypeName { get; set; }
        public string Moneys { get; set; }
        public string ReducedMoney { get; set; }
        public string RestMoney { get; set; }
    }
    public class HOANHBO_ReportInOut
    {
        public string Id { get; set; }
        public int RowNumber { get; set; }
        public string CardNo { get; set; }
        public string CardNumber { get; set; }
        public string Plate { get; set; }
        public string DateTimeIn { get; set; }
        public string DateTimeOut { get; set; }
        public string PicIn1 { get; set; }
        public string PicIn2 { get; set; }
        public string PicOut1 { get; set; }
        public string PicOut2 { get; set; }
        public string CardGroupID { get; set; }
        public string CustomerName { get; set; }
        public string LaneIDIn { get; set; }
        public string LaneIDOut { get; set; }
        public string UserIDIn { get; set; }
        public string UserIDOut { get; set; }
        public string Moneys { get; set; }
        public int DVT { get; set; }
    }

    public class ReportVehicleComeIn
    {
        public int RowNumber { get; set; }
        public string CardNo { get; set; }
        public string CardNumber { get; set; }
        public string Plate { get; set; }
        public string DateTimeIn { get; set; }
        public string PicIn1 { get; set; }
        public string CardGroupID { get; set; }
        public string CustomerName { get; set; }
        public string LaneIDIn { get; set; }
        public string UserIDIn { get; set; }
        public bool IsPlateInValid { get; set; }
    }

    public class ReportDetailMoneyCardDay
    {
        public string Id { get; set; }
        public int RowNumber { get; set; }
        public string CardNo { get; set; }
        public string CardNumber { get; set; }
        public string Plate { get; set; }
        public string DateTimeIn { get; set; }
        public string DateTimeOut { get; set; }
        public string CardGroupID { get; set; }
        public string CustomerName { get; set; }
        public string LaneIDIn { get; set; }
        public string LaneIDOut { get; set; }
        public string UserIDIn { get; set; }
        public string UserIDOut { get; set; }
        public string DCTypeName { get; set; }      
        public string Moneys { get; set; }
        public string ReducedMoney { get; set; }
        public string TotalTimes { get; set; }
        public string Voucher { get; set; }
        public string EventIds { get; set; }
    }

    public class ReportDetailMoneyCardDay2
    {
        public int RowNumber { get; set; }
        public string CardNo { get; set; }
        public string CardNumber { get; set; }
        public string DateTimeIn { get; set; }
        public string DateTimeOut { get; set; }
        public string CardGroupID { get; set; }
        public string CustomerGroupID { get; set; }
        public string LaneIDIn { get; set; }
        public string LaneIDOut { get; set; }
        public string UserIDIn { get; set; }
        public string UserIDOut { get; set; }
        public string Money { get; set; }
        public string TotalTimes { get; set; }
        public string PrintIndex { get; set; }
        public string Para1 { get; set; }
        public string Para2 { get; set; }
    }

    public class ReportTotalMoneyByCardGroup
    {
        public string RowNumber { get; set; }
        public string CardGroupID { get; set; }
        public string CardGroupName { get; set; }
        public string Count { get; set; }
        public string Moneys { get; set; }
    }

    public class ReportTotalMoneyByLane
    {
        public string RowNumber { get; set; }
        public string LaneID { get; set; }
        public string LaneName { get; set; }
        public string Moneys { get; set; }
    }

    public class ReportTotalMoneyByUser
    {
        public string RowNumber { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string Moneys { get; set; }
    }

    public class ReportDetailMoneyCardMonth
    {
        public string RowNumber { get; set; }
        public string Id { get; set; }
        public string CardNo { get; set; }
        public string CardNumber { get; set; }
        public string Date { get; set; }
        public string CardGroupID { get; set; }
        public string CustomerGroupID { get; set; }
        public string CustomerID { get; set; }
        public string OldExpireDate { get; set; }
        public string NewExpireDate { get; set; }
        public string UserID { get; set; }
        public string FeeLevel { get; set; }
        public string Address { get; set; }
        public string Plate { get; set; }
    }
    public class ReportDetailMoneyCardMonthTRANSERCO
    {
        public string RowNumber { get; set; }
        public string Id { get; set; }
        public string CardNo { get; set; }
        public string CardNumber { get; set; }
        public string Date { get; set; }
        public string CardGroupID { get; set; }
        public string CustomerGroupID { get; set; }
        public string CustomerID { get; set; }
        public string OldExpireDate { get; set; }
        public string NewExpireDate { get; set; }
        public string UserID { get; set; }
        public string FeeLevel { get; set; }
        public string Plate { get; set; }
        public string Description { get; set; }
        public bool IsTransferPayment { get; set; }
    }
    public class ReportTotalMoneyCardMonthByCardGroup
    {
        public string Id { get; set; }

        public string CustomerGroupID { get; set; }

        public string GroupName { get; set; }

        public double Moneys { get; set; }

        public string Level { get; set; }
    }

    public class ReportTotalMoneyCardMonthByUser
    {
        public string UserID { get; set; }

        public string UserName { get; set; }

        public double Moneys { get; set; }
    }

    public class ReportCardExpire
    {
        public string RowNumber { get; set; }
        public string CardNo { get; set; }
        public string CardNumber { get; set; }
        public string Plate
        {
            get
            {
                string _Plate = "";
                if (!string.IsNullOrWhiteSpace(Plate1)) _Plate += ";" + Plate1;
                if (!string.IsNullOrWhiteSpace(Plate2)) _Plate += ";" + Plate2;
                if (!string.IsNullOrWhiteSpace(Plate3)) _Plate += ";" + Plate3;
                if (!string.IsNullOrWhiteSpace(_Plate)) _Plate = _Plate.Substring(1, _Plate.Length - 1);

                return _Plate;
            }
            set
            {

            }
        }
        public string Plate1 { get; set; }
        public string Plate2 { get; set; }
        public string Plate3 { get; set; }
        public string ExpireDate { get; set; }
        public string CardGroupName { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string TotalDayEx { get; set; }
    }

    public class ReportAlarm
    {
        public string RowNumber { get; set; }
        public string LaneName { get; set; }
        public string Moneys { get; set; }
        public int Id { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string CardNo { get; set; }
        public string CardNumber { get; set; }
        public string Plate { get; set; }
        public string UserID { get; set; }
        public string LaneID { get; set; }
        public string PicDir { get; set; }
        public string AlarmCode { get; set; }
        public string Description { get; set; }
    }

    public class ReportCustomerList
    {
        public string RowNumber { get; set; }
        public string CardNo { get; set; }
        public string CardNumber { get; set; }
        public string Plate { get; set; }
        public string DateTimeIn { get; set; }
        public string CardGroupID { get; set; }
        public string CardGroupName { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerGroupName { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string ExpireDate { get; set; }
        public string ImportDate { get; set; }
        public string IsLock { get; set; }
    }

    public class ReportCardDetailByCompartment
    {
        public string RowNumber { get; set; }
        public string LogID { get; set; }
        public string CompartmentName { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerGroup { get; set; }
        public string CardNumber { get; set; }
        public string CardNo { get; set; }
        public string CardGroup { get; set; }
        public string Plate { get; set; }
        public string DateRegister { get; set; }
        public string DateRelease { get; set; }
        public string DateCanceled { get; set; }
        public string UserID { get; set; }
    }

    public class ReportCardTotalByCompartment
    {
        public string RowNumber { get; set; }
        public string CompartmentName { get; set; }
        public string CountRegistedBicycle { get; set; }
        public string CountLockBicycle { get; set; }
        public string CountUseBicycle { get; set; }
        public string CountRegistedMotorcycle { get; set; }
        public string CountLockMotorcycle { get; set; }
        public string CountUseMotorcycle { get; set; }
        public string CountRegistedCar { get; set; }
        public string CountLockCar { get; set; }
        public string CountUseCar { get; set; }
    }

    public class ReportPrint
    {
        public string RowNumber { get; set; }
        public string Date { get; set; }
        public string UserName { get; set; }
        public string ObjectName { get; set; }
        public string Description { get; set; }
        public string SubSystemCode { get; set; }
        public string Actions { get; set; }

    }

    public class ReportCustomerExpire_Access
    {
        public string RowNumber { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerGroupName { get; set; }
        public string Description { get; set; }
        public string Inactive { get; set; }
        public string CardNo { get; set; }
        public string CardNumber { get; set; }
        public string Plate { get; set; }
        public string AccessExpireDateHtml { get; set; }
        public string AccessExpireDate { get; set; }
        public string UserIDofFinger { get; set; }
    }

    public class ReportCardExpire_Access
    {
        public string RowNumber { get; set; }
        public string CardNo { get; set; }
        public string CardNumber { get; set; }
        public string CardGroupName { get; set; }
        public string AccessExpireDate { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string CustomerGroupName { get; set; }
        public string IsLock { get; set; }
    }

    public class ReportEvent_Access
    {
        public string Date { get; set; }
        public string CardNo { get; set; }
        public string CardNumber { get; set; }
        public string CardGroupID { get; set; }
        public string ControllerID { get; set; }
        public string ReaderIndex { get; set; }
        public string EventStatus { get; set; }
        public string EventType { get; set; }
    }

    public class ReportVehicleTooDay
    {
        public string Id { get; set; }
        public int RowNumber { get; set; }
        public string CardNo { get; set; }
        public string CardNumber { get; set; }
        public string Plate { get; set; }
        public string DateTimeIn { get; set; }
        public string DateTimeOut { get; set; }
        public string PicIn1 { get; set; }
        public string PicIn2 { get; set; }
        public string PicOut1 { get; set; }
        public string PicOut2 { get; set; }
        public string CardGroupID { get; set; }
        public string CustomerName { get; set; }
        public string LaneIDIn { get; set; }
        public string LaneIDOut { get; set; }
        public string UserIDIn { get; set; }
        public string UserIDOut { get; set; }
        public string Moneys { get; set; }
        public string TooDay { get; set; }
    }

    public class ReportInOutByCustomer
    {
        public string Id { get; set; }
        public int RowNumber { get; set; }
        public string CardNo { get; set; }
        public string CardNumber { get; set; }
        public string CustomerName { get; set; }
        public string Day { get; set; }
    }

    public class ReportTotalVehicleMoneyByCardMonth
    {
        public string RowNumber { get; set; }
        public string CardNo { get; set; }
        public string CardNumber { get; set; }
        public string RegistedPlate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerGroupID { get; set; }
        public string CustomerGroupName { get; set; }
        public string Moneys { get; set; }
    }
    public class ReportChartInOutByCardGroup
    {
        public string CardGroupName { get; set; }
        public string TotalVehicleIn { get; set; }
        public string TotalVehicleOut { get; set; }
    }
    public class ReportChartInOutByLane
    {
        public string LaneName { get; set; }
        public string TotalVehicleIn { get; set; }
        public string TotalVehicleOut { get; set; }
    }
    public class ReportChartMoneyByLane
    {
        public string LaneName { get; set; }
        public string Money { get; set; }

    }
    public class ReportChartMoneyByLevel
    {
        public string LevelMoney { get; set; }
        public string TotalMoney { get; set; }

    }
    public class ReportInOut108
    {
        public string Id { get; set; }
        public string RowId { get; set; }
        public int RowNumber { get; set; }
        public string CardNo { get; set; }
        public string CardNumber { get; set; }
        public string Plate { get; set; }
        public string DateTimeIn { get; set; }
        public string DateTimeOut { get; set; }
        public string PicIn1 { get; set; }
        public string PicIn2 { get; set; }
        public string PicOut1 { get; set; }
        public string PicOut2 { get; set; }
        public string CardGroupID { get; set; }
        public string CustomerName { get; set; }
        public string LaneIDIn { get; set; }
        public string LaneIDOut { get; set; }
        public string UserIDIn { get; set; }
        public string UserIDOut { get; set; }
        public string Moneys { get; set; }
        public string TotalMoneyPercent { get; set; }
        public DateTime DateCreated { get; set; }
        public string UserId { get; set; }
    }

    public class AlarmTurnFPT
    {     
        public string RowNumber { get; set; } 
        public string PlateOut { get; set; }
        public string CardGroupID { get; set; }
        public string CardGroupName { get; set; }
        public int turn { get; set; }    
    }
    public class AlarmNotUseFPT
    {
        public string RowNumber { get; set; }
        public string CardNo { get; set; }
        public string CardNumber { get; set; }
        public string CardGroupName { get; set; }
        public string PlateOut { get; set; }
        public string CustomerName { get; set; }
        public string CustomerGroupName { get; set; }
        public bool IsLock { get; set; }
        public string DateTimeOut { get; set; }
        
        public int Number { get; set; }
    }

    public class ReportInvoiceBAVI
    {
        public string RowNumber { get; set; }
        public string Id { get; set; }
        public string CardNo { get; set; }
        public string CardNumber { get; set; }
        public string Plate { get; set; }
        public string DateTimeIn { get; set; }
        public string DateTimeOut { get; set; }
        public DateTime DateCreatedInvoice { get; set; }
        public int? IsSync { get; set; }
    }

    public class ReportInOut_API_3rd_data
    {
        public string Id { get; set; }
        public int RowNumber { get; set; }
        public string CardNumber { get; set; }
        public string Plate { get; set; }
        public string DateTimeIn { get; set; }
        public string DateTimeOut { get; set; }
        public string Moneys { get; set; }
        public string CardGroupId { get; set; }
        public string CardGroupName { get; set; }

        public string VehicleGroupID { get; set; }
        public string VehicleGroupName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerGroupID { get; set; }
        public string CustomerGroupName { get; set; }
    }

    public class ReportInOut_API_3rd
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalItem { get; set; }
        public int TotalPage { get; set; }
        public long TotalMoney { get; set; }
        public List <ReportInOut_API_3rd_data> ReportInOut_data { get; set; }
    }
    public class ExtendList
    {
        public string RowNumber { get; set; }
        public string Id { get; set; }
        public string CardNo { get; set; }
        public string CardNumber { get; set; }
        public string Date { get; set; }
        public string CardGroupID { get; set; }
        public string CustomerGroupID { get; set; }
        public string CustomerID { get; set; }
        public string OldExpireDate { get; set; }
        public string NewExpireDate { get; set; }
        public string UserID { get; set; }
        public string FeeLevel { get; set; }
        public string Address { get; set; }
        public string Plate { get; set; }
        public DateTime DateCreated { get; set; }
    }

    public class ReportDetailMoneyCardDay_LAOCAI
    {
        public string Id { get; set; }
        public int RowNumber { get; set; }
        public string CardNo { get; set; }
        public string CardNumber { get; set; }
        public string Plate { get; set; }
        public string DateTimeIn { get; set; }
        public string DateTimeOut { get; set; }
        public string CardGroupID { get; set; }
        public string CustomerName { get; set; }
        public string LaneIDIn { get; set; }
        public string LaneIDOut { get; set; }
        public string UserIDIn { get; set; }
        public string UserIDOut { get; set; }
        public string Moneys { get; set; }
        public string TotalTimes { get; set; }
        public string Voucher { get; set; }
        public string EventIds { get; set; }
        public decimal MoneyIn { get; set; }
        public int PayState { get; set; }
        public int PayInID { get; set; }
    }

    public class ReportTotalInOut_Day
    {
        public int STT { get; set; }
        public string Date { get; set; }
        public int TotalIn { get; set; }
        public int TotalOut { get; set; }
        public int TotalIn_Parking { get; set; }
        public List<ReportTotalInOut_Hours> InOutByHour { get; set; }
    }

    public class ReportTotalInOut_Hours
    {
        public int STT { get; set; }
        public string Hours { get; set; }
        public int TotalIn { get; set; }
        public int TotalOut { get; set; }
        public int TotalIn_Parking { get; set; }
    }

    public class ReportReduceMoneyByVoucher
    {
     

        public string DCTypeName { get; set; }
        public decimal TotalReduceVocher { get; set; }
    
    }
    public class ReportReduceMoneyByVoucher_Excel
    {

        public int Count { get; set; }
        public string DCTypeName { get; set; }
        public string TotalReduceVocher { get; set; }

    }
}
