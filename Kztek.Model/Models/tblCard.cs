using Kztek.Model.CustomModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Kztek.Model.Models
{
    public class tblCard
    {
        [Key]
        public System.Guid CardID { get; set; }

        public string CardNo { get; set; }

        public string CardNumber { get; set; }

        public string CustomerID { get; set; }

        public string CardGroupID { get; set; }

        public Nullable<System.DateTime> ImportDate { get; set; }

        public Nullable<System.DateTime> ExpireDate { get; set; }

        public Nullable<System.DateTime> DateUpdate { get; set; }

        public string Plate1 { get; set; }

        public string VehicleName1 { get; set; }

        public string Plate2 { get; set; }

        public string VehicleName2 { get; set; }

        public string Plate3 { get; set; }

        public string VehicleName3 { get; set; }

        public bool IsLock { get; set; }

        public bool IsDelete { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SortOrder { get; set; }

        public string Description { get; set; }

        public Nullable<System.DateTime> DateRegister { get; set; }

        public Nullable<System.DateTime> DateRelease { get; set; }

        public Nullable<System.DateTime> DateCancel { get; set; }

        public Nullable<System.DateTime> DateActive { get; set; }

        public DateTime AccessExpireDate { get; set; }

        public string AccessLevelID { get; set; } //quyền truy cập thay biển số

        public bool ChkRelease { get; set; }

        //Chạy tự động
        public bool isAutoCapture { get; set; } = false;

        public string ViettelId { get; set; } = "";

        public string ViettelType { get; set; } = "";
        public bool IsLost { get; set; } = false;

        public int DVT { get; set; } //đơn vị tấn m3

        public int CardType { get; set; }
    }

    public class tblCardExtend
    {
        public string CardID { get; set; }

        public string CardNo { get; set; }

        public string CardNumber { get; set; }

        public string CustomerCode { get; set; }

        public string CustomerName { get; set; }

        public string CardGroupName { get; set; }

        public string CustomerGroupName { get; set; }

        public string CustomerId { get; set; }

        public string CardGroupId { get; set; }

        public string CustomerGroupId { get; set; }

        public Nullable<System.DateTime> ImportDate { get; set; }

        public Nullable<System.DateTime> ExpireDate { get; set; }

        public Nullable<System.DateTime> DateUpdate { get; set; }

        public string Plate1 { get; set; }

        public string VehicleName1 { get; set; }

        public string Plate2 { get; set; }

        public string VehicleName2 { get; set; }

        public string Plate3 { get; set; }

        public string VehicleName3 { get; set; }

        public string AccessLevelID { get; set; }

        public string AccessLevelName { get; set; }

        public Nullable<System.DateTime> AccessExpireDate { get; set; }

        public string LockerInfo { get; set; } //Danh sách locker đi theo
        public string Address { get; set; }
        public string AddressUnsign { get; set; }
        public string Price { get; set; }

        public int CardType { get; set; }
    }

    public class tblCardActive
    {
        public string CardID { get; set; }

        public string CardNo { get; set; }

        public string CardNumber { get; set; }

        public string CustomerCode { get; set; }

        public string CustomerName { get; set; }

        public string CardGroupName { get; set; }

        public string CustomerGroupName { get; set; }

        public string CustomerId { get; set; }

        public string CardGroupId { get; set; }

        public string CustomerGroupId { get; set; }

        public Nullable<System.DateTime> ImportDate { get; set; }

        public Nullable<System.DateTime> DateActive { get; set; }

        public string Plate1 { get; set; }

        public string VehicleName1 { get; set; }

        public string Plate2 { get; set; }

        public string VehicleName2 { get; set; }

        public string Plate3 { get; set; }

        public string VehicleName3 { get; set; }
    }

    public class tblCardCustomViewModel
    {
        public string CardID { get; set; }

        public string CardNo { get; set; }

        public string CardNumber { get; set; }

        public string CustomerCode { get; set; }

        public string CustomerName { get; set; }

        public string CardGroupName { get; set; }

        public string CustomerGroupName { get; set; }

        public string CustomerMobile { get; set; }

        public string CustomerAddress { get; set; }

        public string CustomerIDNumber { get; set; }

        public string CustomerId { get; set; }

        public string CardGroupId { get; set; }

        public string CustomerGroupId { get; set; }
        public string CompartmentId { get; set; }

        public Nullable<System.DateTime> ImportDate { get; set; }

        public Nullable<System.DateTime> ExpireDate { get; set; }
        public Nullable<System.DateTime> DateRegister { get; set; }

        public Nullable<System.DateTime> DateRelease { get; set; }

        public DateTime AccessExpireDate { get; set; }

        public string AccessLevelID { get; set; }

        public string Plate1 { get; set; }

        public string VehicleName1 { get; set; }

        public string Plate2 { get; set; }

        public string VehicleName2 { get; set; }

        public string Plate3 { get; set; }

        public string VehicleName3 { get; set; }

        public bool IsLock { get; set; }
        public string Description { get; set; }
        public string LockerName { get; set; }
        public string DescriptionCard { get; set; }
        public Int64 RowNumber { get; set; }
        public int CardType { get; set; }
    }
    public class tblCardCustomViewModel1
    {
        public Guid CardID { get; set; }

        public string CardNo { get; set; }

        public string CardNumber { get; set; }

        public string CustomerCode { get; set; }

        public string CustomerName { get; set; }

        public string CardGroupName { get; set; }

        public string CustomerGroupName { get; set; }

        public string CustomerMobile { get; set; }

        public string CustomerAddress { get; set; }

        public string CustomerIDNumber { get; set; }

        public string CustomerId { get; set; }

        public string CardGroupId { get; set; }

        public string CustomerGroupId { get; set; }
        public string CompartmentId { get; set; }

        public Nullable<System.DateTime> ImportDate { get; set; }

        public Nullable<System.DateTime> ExpireDate { get; set; }
        public Nullable<System.DateTime> DateRegister { get; set; }

        public Nullable<System.DateTime> DateRelease { get; set; }

        public DateTime AccessExpireDate { get; set; }

        public string AccessLevelID { get; set; }

        public string Plate1 { get; set; }

        public string VehicleName1 { get; set; }

        public string Plate2 { get; set; }

        public string VehicleName2 { get; set; }

        public string Plate3 { get; set; }

        public string VehicleName3 { get; set; }

        public bool IsLock { get; set; }
        public string Description { get; set; }
        public string LockerName { get; set; }
        public string DescriptionCard { get; set; }
        public Int64 RowNumber { get; set; }
    }

    public class tblCardSubmit
    {
        public string CardID { get; set; }

        #region Liên quan tới thẻ
        //[Required(ErrorMessage = "Vui lòng nhập số thẻ")]
        public string CardNo { get; set; } = "";

        public string OldCardNo { get; set; } = "";

        //[Required(ErrorMessage = "Vui lòng nhập mã thẻ")]
        public string CardNumber { get; set; } = "";

        public string OldCardNumber { get; set; } = "";

        public string CardDescription { get; set; } = "";

        public string OldCardDescription { get; set; } = "";

        //[Required(ErrorMessage = "Vui lòng chọn nhóm thẻ")]
        public string CardGroupID { get; set; } = "";

        public string OldCardGroupID { get; set; } = "";

        public string AccessLevelID { get; set; } = "";

        public string OldAccessLevelID { get; set; } = "";

        public string OldPlate1 { get; set; } = "";

        public string OldPlate2 { get; set; } = "";

        public string OldPlate3 { get; set; } = "";

        //public string OldCustomerName { get; set; } = "";
        public bool CardInActive { get; set; }

        public bool OldCardInActive { get; set; }

        public bool CardIsLost { get; set; }

        public bool OldCardIsLost { get; set; }

        public string DtpDateRegisted { get; set; } = "";

        public string OldDtpDateRegisted { get; set; } = "";

        public string DtpDateReleased { get; set; } = "";

        public string OldDtpDateReleased { get; set; } = "";

        public string DtpDateActive { get; set; } = "";

        public string OldDtpDateActive { get; set; } = "";

        public string DtpDateExpired { get; set; } = "";

        public bool isChangeAccessCard { get; set; }

        public bool isModifiedCard { get; set; }

        public bool isChangeCard { get; set; }

        public bool isReturnCard { get; set; }

        public bool isCHANGEPLA { get; set; }

        public bool isUpdateInfo { get; set; }

        public bool isModifiedBaseInfo { get; set; }

        public bool isModifiedStopCard { get; set; }


        public bool isReleaseCard { get; set; }

        public bool isChangeActiveCard { get; set; }
        #endregion

        #region Liên quan tới khách hàng
        public string CustomerID { get; set; } = "";

        public string OldCustomerID { get; set; } = "";

        /// <summary>
        /// Cư dân - 0 / Khách hàng thuê bao - 1
        /// </summary>
        public string CustomerType { get; set; } = "";

        public string OldCustomerType { get; set; } = "";

        public string CustomerCode { get; set; } = "";

        public string OldCustomerCode { get; set; } = "";

        public string CustomerName { get; set; } = "";

        public string OldCustomerName { get; set; } = "";

        public string CustomerIdentify { get; set; } = "";

        public string OldCustomerIdentify { get; set; } = "";

        public string CustomerAddress { get; set; } = "";

        public string OldCustomerAddress { get; set; } = "";

        public string CustomerAvatar { get; set; } = "";

        public string OldCustomerAvatar { get; set; } = "";

        public string CustomerMobile { get; set; } = "";

        public string OldCustomerMobile { get; set; } = "";

        public string CustomerGroupID { get; set; } = "";

        public string OldCustomerGroupID { get; set; } = "";
        public string CompartmentId { get; set; } = "";
        public bool isChangeCustomer { get; set; }


        public bool isModifiedCustomer { get; set; }
        #endregion

        #region Liên quan tới phương tiện
        public string VehiclePlateList { get; set; } = "";

        public bool isModifiedVehicle { get; set; }

        public string VehicleName1 { get; set; } = "";

        public string VehicleName2 { get; set; } = "";

        public string VehicleName3 { get; set; } = "";

        public string Plate1 { get; set; } = "";

        public string Plate2 { get; set; } = "";

        public string Plate3 { get; set; } = "";

        #endregion

        public string CardSubList { get; set; } = "";

        public string CardType { get; set; } = "";

        public string Note { get; set; } = "";

        public bool IsAutoCapture { get; set; }

        public string ViettelId { get; set; } = "";

        public string ViettelType { get; set; } = "";
        public int DVT { get; set; } //đơn vị tấn m3
    }

    public class tblCardExcel
    {
        public int NumberRow { get; set; }

        public string CardNo { get; set; }

        public string CardNumber { get; set; }

        public string CardGroupName { get; set; }

        public string DateExpire { get; set; }

        public string Plates { get; set; }
        public string Description { get; set; }

        public string VehicleNames { get; set; }

        public string CustomerCode { get; set; }

        public string CustomerName { get; set; }

        public string CustomerGroupName { get; set; }
        public string CompartmentId { get; set; }
        public string CMT { get; set; }

        public string SĐT { get; set; }

        public string Address { get; set; }

        public string Inactive { get; set; }
        public string DateCreated { get; set; }
    }
    public class tbl_CardExcel
    {
        public int NumberRow { get; set; }

        public string CardNo { get; set; }

        public string CardNumber { get; set; }

        public string CardGroupName { get; set; }

        public string DateExpire { get; set; }

        public string Plates { get; set; }
        public string Description { get; set; }

        public string VehicleNames { get; set; }

        public string CustomerCode { get; set; }

        public string CustomerName { get; set; }

        public string CustomerGroupName { get; set; }
        public string CompartmentId { get; set; }
        public string CMT { get; set; }

        public string SĐT { get; set; }

        public string Address { get; set; }

        public string Inactive { get; set; }
        public string DateRegister { get; set; }
    }
    public class tblCardExcel_v2
    {
        public int NumberRow { get; set; }

        public string CardNo { get; set; }

        public string CardNumber { get; set; }

        public string CardGroupName { get; set; }

        public string DateExpire { get; set; }

        public string Plates { get; set; }
        public string Description { get; set; }

        public string VehicleNames { get; set; }

        public string CustomerCode { get; set; }

        public string CustomerName { get; set; }

        public string CustomerGroupName { get; set; }
        public string CompartmentId { get; set; }

        public string CMT { get; set; }

        public string SĐT { get; set; }

        public string Address { get; set; }

        public string Inactive { get; set; }
        public string DescriptionCard { get; set; }
        public string DateCreated { get; set; }
    }

    public class DetailCardDepartmentExcel
    {
        public int NumberRow { get; set; }
        public string CompartmentName { get; set; }
        public string CustomerCode { get; set; }

        public string CustomerName { get; set; }

        public string CustomerGroupName { get; set; }
        public string CardNumber { get; set; }
        public string CardNo { get; set; }

        public string CardGroupName { get; set; }
        public string Plates { get; set; }
        public string DateRegister { get; set; }
        public string DateRelease { get; set; }
        public string DateExpire { get; set; }

        public string DateCreated { get; set; }
    }

    public class TotalCardDepartment
    {
        public Int64 RowNumber { get; set; }
        public string CompartmentId { get; set; }
        public int CAR_REG { get; set; }
        public int CAR_NOTUSE { get; set; }
        public int CAR_USE { get; set; }
        public int CYCLE_REG { get; set; }
        public int CYCLE_NOTUSE { get; set; }
        public int CYCLE_USE { get; set; }
        public int BIKE_REG { get; set; }
        public int BIKE_NOTUSE { get; set; }
        public int BIKE_USE { get; set; }
    }


    public class TotalCardDepartment1
    {
        public Int64 RowNumber { get; set; }
        public string CompartmentName { get; set; }
        public int CAR_REG { get; set; }
        public int CAR_NOTUSE { get; set; }
        public int CAR_USE { get; set; }
        public int CYCLE_REG { get; set; }
        public int CYCLE_NOTUSE { get; set; }
        public int CYCLE_USE { get; set; }
        public int BIKE_REG { get; set; }
        public int BIKE_NOTUSE { get; set; }
        public int BIKE_USE { get; set; }
    }
    public class tblAccessCardExcel
    {
        public int NumberRow { get; set; }

        public string CardNo { get; set; }

        public string CardNumber { get; set; }

        public string CardGroupName { get; set; }

        public string DateExpire { get; set; }

        public string CustomerCode { get; set; }

        public string CustomerName { get; set; }

        public string CustomerGroupName { get; set; }

        public string CMT { get; set; }

        public string SĐT { get; set; }

        public string Address { get; set; }

        public string AccessLevelName { get; set; }

        public string Inactive { get; set; }

        public string DateCreated { get; set; }
        public string CardType { get; set; }
    }

    public class tblLockerCardExcel
    {
        public int NumberRow { get; set; }

        public string CardNo { get; set; }

        public string CardNumber { get; set; }

        public string CardGroupName { get; set; }

        public string DateExpire { get; set; }

        public string CustomerCode { get; set; }

        public string CustomerName { get; set; }

        public string CustomerGroupName { get; set; }

        public string CMT { get; set; }

        public string SĐT { get; set; }

        public string Address { get; set; }

        public string LockerName { get; set; }

        public string Inactive { get; set; }

        public string DateCreated { get; set; }
    }

    public class tblCardUpload
    {
        public string CardNumber { get; set; }

        public string CardGroupID { get; set; }

        public string CustomerID { get; set; }

        public string CustomerGroupID { get; set; }

        public DateTime AccessExpireDate { get; set; }
    }

    public class AutoCapture
    {
        public string key { get; set; }
        public string cardgroups { get; set; }
        public string customergroups { get; set; }
        public string active { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
        public string isCheckByTime { get; set; }
        public bool chkFindAutoCapture { get; set; }
        public string type { get; set; }
    }

    /// <summary>
    /// api sigma-C51
    /// </summary>
    public class tblCard_API
    {
        public string CardId { get; set; }
        public string CardNo { get; set; }

        public string CardNumber { get; set; }

        public string CardGroupID { get; set; }
        public string Plate { get; set; }
        public string VehicleName { get; set; }

        public Nullable<System.DateTime> ExpireDate { get; set; }


        public Nullable<System.DateTime> RegisterDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAdd { get; set; }
        public string CustomerId { get; set; }
        public bool isAutoCapture { get; set; }
        public bool IsLock { get; set; }
    }

    public class tblCardCustom
    {
        public string CardID { get; set; }

        public string CardNumber { get; set; }

        public string CustomerCode { get; set; }

        public string CustomerName { get; set; }

    }
    #region API


    #region Api_Old
    /// <summary>
    /// api 3rd all
    /// </summary>
    public class tblCard_API_3rd
    {
        public string CardId { get; set; }
        public string CardNo { get; set; }

        public string CardNumber { get; set; }

        public string CardGroupID { get; set; }
        public string Plate { get; set; }
        public string Plate1 { get; set; }
        public string Plate2 { get; set; }
        public string Plate3 { get; set; }

        public string VehicleName { get; set; }
        public Nullable<System.DateTime> ExpireDate { get; set; }
        public Nullable<System.DateTime> RegisterDate { get; set; }
        public bool IsDelete { get; set; }
        public bool IsLock { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerMobile { get; set; }
        public string Money { get; set; }






    }

    public class API_3rd_list_tblCard
    {
        public string CardId { get; set; }
        public string CardNo { get; set; }
        public string CardNumber { get; set; }
        public string CardGroupName { get; set; }
        public string Plate1 { get; set; }
        public string VehicleName { get; set; }
        public string CustomerGroupName { get; set; }
        public Nullable<System.DateTime> ExpireDate { get; set; }
        public Nullable<System.DateTime> RegisterDate { get; set; }
        public bool IsDelete { get; set; }
        public bool IsLock { get; set; }
        public string CustomerName { get; set; }

    }
    public class tblCard_API_3rd_Out
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalItem { get; set; }
        public int TotalPage { get; set; }
        public List<API_3rd_list_tblCard> Out_data { get; set; }
    }

    #endregion


    public class Result_tblCardAPI
    {
        public string CardNumber { get; set; }
        public bool isSuccess { get; set; }
        public string Message { get; set; }

    }
    public class tblCardSubmit_API
    {
        public string CardID { get; set; }

        #region Liên quan tới thẻ
        //[Required(ErrorMessage = "Vui lòng nhập số thẻ")]
        public string CardNo { get; set; } = "";

        public string OldCardNo { get; set; } = "";

        //[Required(ErrorMessage = "Vui lòng nhập mã thẻ")]
        public string CardNumber { get; set; } = "";

        public string OldCardNumber { get; set; } = "";

        public string CardDescription { get; set; } = "";

        public string OldCardDescription { get; set; } = "";

        //[Required(ErrorMessage = "Vui lòng chọn nhóm thẻ")]
        public string CardGroupID { get; set; } = "";

        public string OldCardGroupID { get; set; } = "";

        public string AccessLevelID { get; set; } = "";

        public string OldAccessLevelID { get; set; } = "";

        public string OldPlate1 { get; set; } = "";

        public string OldPlate2 { get; set; } = "";

        public string OldPlate3 { get; set; } = "";

        //public string OldCustomerName { get; set; } = "";
        public bool CardInActive { get; set; }

        public bool OldCardInActive { get; set; }

        public bool CardIsLost { get; set; }

        public bool OldCardIsLost { get; set; }

        public string DtpDateRegisted { get; set; } = "";

        public string OldDtpDateRegisted { get; set; } = "";

        public string DtpDateReleased { get; set; } = "";

        public string OldDtpDateReleased { get; set; } = "";

        public string DtpDateActive { get; set; } = "";

        public string OldDtpDateActive { get; set; } = "";

        public string DtpDateExpired { get; set; } = "";

        public bool isChangeAccessCard { get; set; }

        public bool isModifiedCard { get; set; }

        public bool isChangeCard { get; set; }

        public bool isReturnCard { get; set; }

        public bool isCHANGEPLA { get; set; }

        public bool isUpdateInfo { get; set; }

        public bool isModifiedBaseInfo { get; set; }

        public bool isModifiedStopCard { get; set; }


        public bool isReleaseCard { get; set; }

        public bool isChangeActiveCard { get; set; }
        #endregion

        #region Liên quan tới khách hàng
        public string CustomerID { get; set; } = "";

        public string OldCustomerID { get; set; } = "";

        /// <summary>
        /// Cư dân - 0 / Khách hàng thuê bao - 1
        /// </summary>
        public string CustomerType { get; set; } = "";

        public string OldCustomerType { get; set; } = "";

        public string CustomerCode { get; set; } = "";

        public string OldCustomerCode { get; set; } = "";

        public string CustomerName { get; set; } = "";

        public string OldCustomerName { get; set; } = "";

        public string CustomerIdentify { get; set; } = "";

        public string OldCustomerIdentify { get; set; } = "";

        public string CustomerAddress { get; set; } = "";

        public string OldCustomerAddress { get; set; } = "";

        public string CustomerAvatar { get; set; } = "";

        public string OldCustomerAvatar { get; set; } = "";

        public string CustomerMobile { get; set; } = "";

        public string OldCustomerMobile { get; set; } = "";

        public string CustomerGroupID { get; set; } = "";

        public string OldCustomerGroupID { get; set; } = "";
        public string CompartmentId { get; set; } = "";
        public bool isChangeCustomer { get; set; }


        public bool isModifiedCustomer { get; set; }
        #endregion

        #region Liên quan tới phương tiện
        public string VehiclePlateList { get; set; } = "";

        public bool isModifiedVehicle { get; set; }

        public string VehicleName1 { get; set; } = "";

        public string VehicleName2 { get; set; } = "";

        public string VehicleName3 { get; set; } = "";

        public string Plate1 { get; set; } = "";

        public string Plate2 { get; set; } = "";

        public string Plate3 { get; set; } = "";

        #endregion

        public string CardSubList { get; set; } = "";

        public string CardType { get; set; } = "";

        public string Note { get; set; } = "";

        public bool IsAutoCapture { get; set; }

        public string ViettelId { get; set; } = "";

        public string ViettelType { get; set; } = "";
        public int DVT { get; set; } //đơn vị tấn m3
    }
    public class tblCard_API_3rd_ByCardNumber
    {
        public string CardNo { get; set; }
        public string CardNumber { get; set; }
        public string CardGroupID { get; set; }
        public string Plate { get; set; }
        public string VehicleName { get; set; }
        public string CustomerID { get; set; }
        public Nullable<System.DateTime> ExpireDate { get; set; }
        public Nullable<System.DateTime> RegisterDate { get; set; }
        public bool IsLock { get; set; }

    }

    #endregion
}
