using Kztek.Data.Repository;
using Kztek.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Service.API
{
    public interface IAPI_CardViettelService
    {
        tblCardGroup GetCardGroupByName(string name);

        tblCustomerGroup GetCustomerGroupByName(string name);

        tblCard GetCardByCardNumber(string cardnumber);

        tblCardSubmit GetCustomCardById(Guid id);

        tblCustomerSubmit GetCustomCustomerByCode(string code);
    }

    public class API_CardViettelService : IAPI_CardViettelService
    {
        private ItblCardGroupRepository _tblCardGroupRepository;
        private ItblCustomerGroupRepository _tblCustomerGroupRepository;
        private ItblCardRepository _tblCardRepository;
        private ItblCustomerRepository _tblCustomerRepository;

        public API_CardViettelService(ItblCardGroupRepository _tblCardGroupRepository, ItblCustomerGroupRepository _tblCustomerGroupRepository, ItblCardRepository _tblCardRepository, ItblCustomerRepository _tblCustomerRepository)
        {
            this._tblCardGroupRepository = _tblCardGroupRepository;
            this._tblCustomerGroupRepository = _tblCustomerGroupRepository;
            this._tblCardRepository = _tblCardRepository;
            this._tblCustomerRepository = _tblCustomerRepository;
        }

        public tblCardGroup GetCardGroupByName(string name)
        {
            var query = from n in _tblCardGroupRepository.Table
                        where n.CardGroupName.Equals(name)
                        select n;

            return query.FirstOrDefault();
        }

        public tblCustomerGroup GetCustomerGroupByName(string name)
        {
            var query = from n in _tblCustomerGroupRepository.Table
                        where n.CustomerGroupName == name
                        select n;

            return query.FirstOrDefault();
        }

        public tblCard GetCardByCardNumber(string cardnumber)
        {
            var query = from n in _tblCardRepository.Table
                        where /*n.IsLock == false &&*/ n.CardNumber == cardnumber && n.IsDelete == false
                        //orderby n.CardNo ascending
                        select n;

            return query.FirstOrDefault();
        }

        public tblCardSubmit GetCustomCardById(Guid id)
        {
            var query = from n in _tblCardRepository.Table
                        join m in _tblCustomerRepository.Table on n.CustomerID equals m.CustomerID.ToString() into n_m
                        from m in n_m.DefaultIfEmpty()
                        where n.CardID == id
                        select new tblCardSubmit()
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

                            //Khách hàng
                            CustomerID = n.CustomerID,
                            CustomerAddress = m.Address,
                            CustomerAvatar = m.Avatar,
                            CustomerIdentify = m.IDNumber,
                            CustomerCode = m.CustomerCode,
                            CustomerGroupID = m.CustomerGroupID,
                            CustomerMobile = m.Mobile,
                            CustomerName = m.CustomerName,

                            //Ngày tháng
                            //DtpDateExpired = Convert.ToDateTime(n.ExpireDate).ToString("dd/MM/yyyy"),
                            //DtpDateRegisted = Convert.ToDateTime(n.DateRegister).ToString("dd/MM/yyyy"),
                            //DtpDateReleased = Convert.ToDateTime(n.DateRelease).ToString("dd/MM/yyyy"),

                            //Dữ liệu cũ dành cho process
                            OldCardInActive = n.IsLock,
                            OldCardNo = n.CardNo,
                            OldCardNumber = n.CardNumber,
                            OldCardDescription = n.Description,
                            OldCardGroupID = n.CardGroupID,

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

        public tblCustomerSubmit GetCustomCustomerByCode(string code)
        {
            var obj = GetByCode(code);
            if (obj != null)
            {
                var t = new tblCustomerSubmit()
                {
                    AccessLevelID = obj.AccessLevelID,
                    Account = obj.Account,
                    Address = obj.Address,
                    Avatar = obj.Avatar,
                    CompartmentId = obj.CompartmentId,
                    CustomerCode = obj.CustomerCode,
                    CustomerGroupID = obj.CustomerGroupID,
                    CustomerID = obj.CustomerID.ToString(),
                    CustomerName = obj.CustomerName,
                    Description = obj.Description,
                    DevPass = obj.DevPass,
                    EnableAccount = obj.EnableAccount,
                    Finger1 = obj.Finger1,
                    Finger2 = obj.Finger2,
                    IDNumber = obj.IDNumber,
                    Inactive = obj.Inactive,
                    Mobile = obj.Mobile,
                    Password = obj.Password,
                    SortOrder = obj.SortOrder,
                    UserIDofFinger = obj.UserIDofFinger
                };

                return t;
            }

            return null;
        }

        private tblCustomer GetByCode(string code)
        {
            var query = from n in _tblCustomerRepository.Table
                        where n.CustomerCode == code
                        select n;

            return query.FirstOrDefault();
        }
    }
}
