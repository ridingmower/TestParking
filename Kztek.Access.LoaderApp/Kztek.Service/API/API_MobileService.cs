using Kztek.Data.Event.Infrastructure;
using Kztek.Data.Event.Repository;
using Kztek.Data.Repository;
using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Model.Models.API;
using Kztek.Model.Models.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Repository.API
{
    public interface IAPI_MobileService
    {
        IEnumerable<tblLane> GetLanesList();

        User GetUserById(string id);

        tblLane GetLaneById(string id);

        tblCardEvent GetRecentEventInByCardnumber(string cardnumber);

        IEnumerable<tblCardEvent> GetEventInByCardNumber(string cardnumber);

        tblCard GetCardByCardnumber(string cardnumber);

        tblCardGroup GetCardgroupById(string id);

        tblBlackList GetCurrentBlackListByPlate(string plate);

        API_CardInfo GetCardInfoByCardNumber(string cardnumber);

        MessageReport CreateEventIn(tblCardEvent ev);

        MessageReport UpdateEvent(tblCardEvent ev);

        MessageReport CreateAlarm(tblAlarm alarm);

        tblVehicleGroup GetVehicleGroupById(string id);
    }
    public class API_MobileService : IAPI_MobileService
    {
        private IUserRepository _UserRepository;

        private ItblLaneRepository _tblLaneRepository;
        private ItblPCRepository _tblPCRepository;

        private ItblCardRepository _tblCardRepository;
        private ItblCardGroupRepository _tblCardGroupRepository;
        private ItblVehicleGroupRepository _tblVehicleGroupRepository;
        private ItblBlackListRepository _tblBlackListRepository;
        private ItblCustomerRepository _tblCustomerRepository;
        private ItblCustomerGroupRepository _tblCustomerGroupRepository;

        private ItblCardEventRepository _tblCardEventRepository;
        private ItblAlarmRepository _tblAlarmRepository;

        private Kztek.Data.Infrastructure.IUnitOfWork _UnitOfWork;
        private Kztek.Data.Event.Infrastructure.IUnitOfWork _UnitOfWorkEv;
        public API_MobileService(
            IUserRepository _UserRepository,
            ItblLaneRepository _tblLaneRepository,
            ItblPCRepository _tblPCRepository,
            ItblCardRepository _tblCardRepository,
            ItblCardGroupRepository _tblCardGroupRepository,
            ItblVehicleGroupRepository _tblVehicleGroupRepository,
            ItblBlackListRepository _tblBlackListRepository,
            ItblCustomerRepository _tblCustomerRepository,
            ItblCustomerGroupRepository _tblCustomerGroupRepository,
            ItblCardEventRepository _tblCardEventRepository,
            ItblAlarmRepository _tblAlarmRepository,
            Kztek.Data.Infrastructure.IUnitOfWork _UnitOfWork,
            Kztek.Data.Event.Infrastructure.IUnitOfWork _UnitOfWorkEv
            )
        {
            this._UserRepository = _UserRepository;
            this._tblLaneRepository = _tblLaneRepository;
            this._tblPCRepository = _tblPCRepository;
            this._tblCardRepository = _tblCardRepository;
            this._tblCardGroupRepository = _tblCardGroupRepository;
            this._tblVehicleGroupRepository = _tblVehicleGroupRepository;
            this._tblBlackListRepository = _tblBlackListRepository;
            this._tblCustomerRepository = _tblCustomerRepository;
            this._tblCustomerGroupRepository = _tblCustomerGroupRepository;
            this._tblCardEventRepository = _tblCardEventRepository;
            this._tblAlarmRepository = _tblAlarmRepository;
            this._UnitOfWork = _UnitOfWork;
            this._UnitOfWorkEv = _UnitOfWorkEv;
        }

        public IEnumerable<tblLane> GetLanesList()
        {
            var result = _tblLaneRepository.Table;
            return result;
        }

        public User GetUserById(string id)
        {
            var result = _UserRepository.GetById(id);
            return result;
        }

        public tblLane GetLaneById(string id)
        {
            var result = _tblLaneRepository.GetById(Guid.Parse(id));
            return result;
        }

        public tblCardEvent GetRecentEventInByCardnumber(string cardnumber)
        {
            var result = _tblCardEventRepository.Table.Where(p => p.CardNumber == cardnumber && p.EventCode == "1").FirstOrDefault();
            return result;
        }

        public IEnumerable<tblCardEvent> GetEventInByCardNumber(string cardnumber)
        {
            var result = _tblCardEventRepository.Table.Where(p => p.CardNumber == cardnumber && p.EventCode == "1");

            return result;
        }

        public tblCard GetCardByCardnumber(string cardnumber)
        {
            var result = _tblCardRepository.Table.Where(p => p.CardNumber == cardnumber && p.IsDelete == false).FirstOrDefault();
            return result;
        }

        public tblCardGroup GetCardgroupById(string id)
        {
            var result = _tblCardGroupRepository.GetById(Guid.Parse(id));
            return result;
        }

        public tblBlackList GetCurrentBlackListByPlate(string plate)
        {
            var result = _tblBlackListRepository.Table.Where(p => p.Plate == plate.Replace("-", "").Replace(".", "").Replace(" ", "")).FirstOrDefault();
            return result;
        }

        public API_CardInfo GetCardInfoByCardNumber(string cardnumber)
        {
            var query = from card in _tblCardRepository.Table
                        join cardgroup in _tblCardGroupRepository.Table on card.CardGroupID equals cardgroup.CardGroupID.ToString()
                        join vehiclegroup in _tblVehicleGroupRepository.Table on cardgroup.VehicleGroupID equals vehiclegroup.VehicleGroupID.ToString()
                        join customer in _tblCustomerRepository.Table on card.CustomerID equals customer.CustomerID.ToString()
                        join customergroup in _tblCustomerGroupRepository.Table on customer.CustomerGroupID equals customergroup.CustomerGroupID.ToString()
                        where card.CardNumber == cardnumber && card.IsDelete == false
                        select new API_CardInfo()
                        {
                            CardNo = card.CardNo,
                            CardNumber = card.CardNumber,
                            ExpireDate = card.ExpireDate,
                            Plate1 = card.Plate1,
                            Plate2 = card.Plate2,
                            Plate3 = card.Plate3,
                            VehicleName1 = card.VehicleName1,
                            VehicleName2 = card.VehicleName2,
                            VehicleName3 = card.VehicleName3,
                            IsLock = card.IsLock,
                            CardDescription = card.Description,
                            DateActive = card.DateActive,

                            CardGroupID = card.CardGroupID,
                            CardGroupName = cardgroup.CardGroupName,
                            CardType = cardgroup.CardType,
                            IsHaveMoneyExpiredDate = cardgroup.IsHaveMoneyExpiredDate,
                            CardGroupInactive = cardgroup.Inactive,

                            VehicleGroupID = cardgroup.VehicleGroupID,
                            VehicleGroupName = vehiclegroup.VehicleGroupName,
                            VehicleType = vehiclegroup.VehicleType,

                            Address = customer.Address,
                            Avatar = customer.Avatar,
                            CustomerCode = customer.CustomerCode,
                            CustomerID = card.CustomerID,
                            CustomerName = customer.CustomerName,
                            CustomerDescription = customer.Description,
                            IDNumber = customer.IDNumber,
                            Mobile = customer.Mobile,

                            CustomerGroupID = customer.CustomerGroupID,
                            CustomerGroupName = customergroup.CustomerGroupName,
                            CustomerGroupDescription = customergroup.Description
                        };

            var result = query.FirstOrDefault();

            return result;
        }

        public MessageReport CreateEventIn(tblCardEvent ev)
        {
            var result = new MessageReport();
            try
            {
                _tblCardEventRepository.Add(ev);
                _UnitOfWorkEv.Commit();
                result.isSuccess = true;
            }
            catch (Exception ex)
            {
                result.isSuccess = false;
                result.Message = ex.Message;
            }
            return result;
        }

        public MessageReport UpdateEvent(tblCardEvent ev)
        {
            var result = new MessageReport();
            try
            {
                _tblCardEventRepository.Update(ev);
                _UnitOfWorkEv.Commit();
                result.isSuccess = true;
            }
            catch (Exception ex)
            {
                result.isSuccess = false;
                result.Message = ex.Message;
            }
            return result;
        }

        public MessageReport CreateAlarm(tblAlarm alarm)
        {
            var result = new MessageReport();
            try
            {
                _tblAlarmRepository.Add(alarm);
                _UnitOfWorkEv.Commit();
                result.isSuccess = true;
            }
            catch (Exception ex)
            {
                result.isSuccess = false;
                result.Message = ex.Message;
            }
            return result;
        }

        public tblVehicleGroup GetVehicleGroupById(string id)
        {
            var result = _tblVehicleGroupRepository.GetById(Guid.Parse(id));

            return result;
        }
    }
}
