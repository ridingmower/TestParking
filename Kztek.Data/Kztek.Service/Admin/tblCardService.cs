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
using Kztek.Web.Core.Extensions;

namespace Kztek.Service.Admin
{
    public interface ItblCardService
    {
        IEnumerable<tblCard> GetAll();
        IEnumerable<tblCard> GetCount(string fromdate, string todate);

        IPagedList<tblCardExtend> GetAllPagingByFirst(string key, string anotherkey, string cardgroups, string customergroups, string fromdate, string todate, int pageNumber = 1, int pageSize = 20, string accesslevelids = "");

        IPagedList<tblCardExtend> AQUA_GetAllPagingByFirst(string key,string address, string strIDCards, string anotherkey, string cardgroups, string customergroups, string fromdate, string todate, int pageNumber = 1, int pageSize = 20, string accesslevelids = "");  

        IPagedList<tblCardExtend> GetAllPagingByFirstForUpload(string key, string anotherkey, string cardgroups, string customergroups, string fromdate, string todate, int pageNumber = 1, int pageSize = 20, string accesslevelids = "", string column = "", string sort = "");

        IPagedList<tblCardExtend> GetAllPagingByFirstForUploadLocker(string key, string anotherkey, string cardgroups, string customergroups, string fromdate, string todate, int pageNumber = 1, int pageSize = 20);

        IEnumerable<tblCardExtend> GetAllByFirstForUploadLocker(string key, string anotherkey, string cardgroups, string customergroups, string fromdate, string todate);

        List<tblCardExtend> GetAllByFirst(string key, string anotherkey, string cardgroups, string customergroups, string fromdate, string todate, string accesslevelids = "");

        IPagedList<tblCardActive> GetAllCPagingByFirst(string key, string anotherkey, string cardgroups, string customergroups, string fromdate, string todate, int pageNumber = 1, int pageSize = 20);

        //IPagedList<tblCardExtend> GetAllPagingByFirst(string key, string cardgroups, string customerid, string customergroups, string fromdate, string todate, string ischeckbytime = "0", int pageNumber = 1, int pageSize = 20);

        IPagedList<tblCardCustomViewModel> GetAllPagingByFirst(string key, string cardgroups, string customerid, string customergroups, string fromdate, string todate, string ischeckbytime = "0", int pageNumber = 1, int pageSize = 20, string accesslevelids = "", string active = "");

        List<tblCardCustomViewModel> GetAllPagingByFirstTSQL(string key, string cardgroups, string customerid, string customergroups, string fromdate, string todate, bool desc, string columnQuery, ref int total, string ischeckbytime = "0", int pageNumber = 1, int pageSize = 20, string accesslevelids = "", string active = "", string cardtype = "");

        IPagedList<tblCardCustomViewModel> GetAllPagingByFirstParking(string key, string cardgroups, string customerid, string customergroups, string fromdate, string todate, bool desc, string columnQuery, string ischeckbytime = "0", int pageNumber = 1, int pageSize = 20, string accesslevelids = "", string active = "");

        List<tblCardCustomViewModel> GetAllPagingByFirstParkingTSQL(string key, string cardgroups, string customerid, string customergroups, string fromdate, string todate, bool desc, string columnQuery, ref int total, string ischeckbytime = "0", int pageNumber = 1, int pageSize = 20, string accesslevelids = "", string active = "", bool isfindautocapture = false);

        MessageReport UpdateMultiCardLevel(string key, string cardgroups, string customerid, string customergroups, string fromdate, string todate, string ischeckbytime = "0", string accesslevelids = "", string newvalue = "");

        MessageReport UpdateMultiCardType(string key, string cardgroups, string customerid, string customergroups, string fromdate, string todate, string ischeckbytime = "0", string accesslevelids = "", string cardType = "");

        IEnumerable<tblCardExtend> GetAllActiveByListId(string ids);

        IEnumerable<tblCardExtend> GetAllActiveByListIdForUpload(string ids);

        IEnumerable<tblCardExtend> GetAllActiveByListIdForUpload(List<string> ids);

        IEnumerable<tblCardActive> GetAllCActiveByListId(string ids);
        List<tblCard> GetByCustomerId(string customerid);
        IEnumerable<tblCard> GetAllActiveByKey(string key);

        IEnumerable<tblCard> GetAllByCustomerId(string customerid);

        List<tblCardExcel> GetExcelCardByFirst(string key, string cardgroups, string customerid, string customergroups, string fromdate, string todate, string ischeckbytime = "0", string accesslevelids = "", string active = "");

        List<tblAccessCardExcel> GetAccessExcelCardByFirstTSQL(string key, string cardgroups, string customerid, string customergroups, string fromdate, string todate, bool desc, string columnQuery, string ischeckbytime = "0", string accesslevelids = "", string active = "", string cardtype = "");

        List<tblCardExcel> GetExcelCardByFirstParking(string key, string cardgroups, string customerid, string customergroups, string fromdate, string todate, bool desc, string columnQuery, string ischeckbytime = "0", string accesslevelids = "", string active = "");
        List<tbl_CardExcel> GetExcelCardByFirstParkingTSQL(string key, string cardgroups, string customerid, string customergroups, string fromdate, string todate, bool desc, string columnQuery, string ischeckbytime = "0", string accesslevelids = "", string active = "", bool isfindautocapture = false);

        tblCard GetById(Guid id);

        tblCardSubmit GetCustomById(Guid id);

        tblCard GetByCardNumber(string cardnumber);

        tblCardUpload GetCustomByCardNumber(string cardnumber);

        tblCard GetByCardNumber_Id(string cardnumber, Guid id);

        MessageReport Create(tblCard obj);

        MessageReport Update(tblCard obj);

        MessageReport DeleteById(string id);

        MessageReport UpdateCard(string action, string currentuser, string cardnumber, string dateN, bool checkuse = false);

        bool AddCardExpire(string KeyWord, string AnotherKey, string CardGroupIDs, string CustomerID, string CustomerGroupID, int _feelevel, string _newexpire, string userId, bool chbEnableMinusActive, bool chbEnableDateActive = false, string dateactive = "");     

        //dùng cho trường chinh
        bool AddCardExpireTRANSERCO(string KeyWord,string fromdate,string todate, string AnotherKey, string CardGroupIDs, string CustomerID, string CustomerGroupID, int _feelevel, string _newexpire, string userId, bool chbEnableMinusActive, bool istransfer, bool chbEnableDateActive = false, string dateactive = "");
        //
        bool AddCardExpireByListCardNumber(string listCardNumber, int _feelevel, string _newexpire, string userId, bool chbEnableMinusActive, bool chbEnableDateActive = false, string dateactive = "");

        //dùng cho trường chinh
        bool AddCardExpireByListCardNumberTRANSERCO(string listCardNumber, int _feelevel, string _newexpire, string userId, bool chbEnableMinusActive, string arrCardID, bool chbEnableDateActive = false, string dateactive = "");

        bool AddCardDateActive(string KeyWord, string AnotherKey, string CardGroupIDs, string CustomerID, string CustomerGroupID, int _feelevel, string _newexpire, string userId, bool chbEnableMinusActive, bool chbEnableDateActive = false, string dateactive = "");

        //dùng cho trường chinh
        bool AddCardDateActiveTRANSERCO(string KeyWord, string AnotherKey, string CardGroupIDs, string CustomerID, string CustomerGroupID, int _feelevel, string _newexpire, string userId, bool chbEnableMinusActive, bool chbEnableDateActive = false, string dateactive = "");

        bool AddCardDateActiveByListCardNumber(string listCardNumber, int _feelevel, string _newexpire, string userId, bool chbEnableMinusActive, bool chbEnableDateActive = false, string dateactive = "");

        //dùng cho trường chinh
        bool AddCardDateActiveByListCardNumberTRANSERCO(string listCardNumber, int _feelevel, string _newexpire, string userId, bool chbEnableMinusActive, bool chbEnableDateActive = false, string dateactive = "");

        IEnumerable<tblCardExtend> GetAllByFirstForUpload(string key, string anotherkey, string cardgroups, string customergroups, string fromdate, string todate, string accesslevelids = "");

        List<tblActiveCard_TC> GetMoneyByCardNumber();

        List<tblCardCustomViewModel> GetAllPagingByFirstTSQL_Locker(string key, string cardgroups, string customerid, string customergroups, string fromdate, string todate, bool desc, string columnQuery, ref int total, string ischeckbytime = "0", int pageNumber = 1, int pageSize = 20, string accesslevelids = "", string active = "");
        List<tblLockerCardExcel> GetLockerExcelCardByFirstTSQL(string key, string cardgroups, string customerid, string customergroups, string fromdate, string todate, bool desc, string columnQuery, string ischeckbytime = "0", string accesslevelids = "", string active = "");

        //AQUA
        List<tblCardExtend> AQUA_GetAllPagingByFirstSQL(string key, string address, string strIDCards, string anotherkey, string cardgroups, string customergroups, string fromdate, string todate,string newdate, ref double _totalmoneys, ref int total, int pageNumber = 1, int pageSize = 20);

        bool AQUA_AddCardExpireByListCardNumber(string listCardNumber, int _feelevel, string _newexpire, string userId, bool chbEnableMinusActive, bool chbEnableDateActive = false, string dateactive = "");

        bool AQUA_AddCardExpire(string KeyWord, string orderId, string strIDCards, string AnotherKey, string CardGroupIDs, string CustomerID, string CustomerGroupID, int _feelevel, string _newexpire, string userId, bool chbEnableMinusActive, bool chbEnableDateActive = false, string dateactive = "");

        tblCardExtend AQUA_GetListCardNumberExtendAll(string key, string strIDCards, string anotherkey, string cardgroups, string customergroups, string fromdate, string todate,string newDate);
        List<tblCardExtend> AQUA_MoneyByGroup(string key, string address, string strIDCards, string anotherkey, string cardgroups, string customergroups, string fromdate, string todate, string newdate);
        List<tblCardExtend> GetAllByFirst_v2(List<tblCardExtend> list);

        List<tblCardExcel_v2> GetExcelCardByFirstParkingTSQL_v2(string key, string cardgroups, string customerid, string customergroups, string fromdate, string todate, bool desc, string columnQuery, string ischeckbytime = "0", string accesslevelids = "", string active = "", bool isfindautocapture = false);

        #region coma6
        bool AddCardExpireOneMonth(string KeyWord, string AnotherKey, string CardGroupIDs, string CustomerID, string CustomerGroupID, int _feelevel, string _newexpire, string userId, bool chbEnableMinusActive, bool chbEnableDateActive = false, string dateactive = "");

        bool AddCardExpireByListCardNumberOneMonth(string listCardNumber, int _feelevel, string _newexpire, string userId, bool chbEnableMinusActive, bool chbEnableDateActive = false, string dateactive = "");
        #endregion

        bool AQUA_AddCardExpire_v2(string KeyWord, string orderId, string strIDCards, string AnotherKey, string CardGroupIDs, string CustomerID, string CustomerGroupID, int _feelevel, string _newexpire, string userId, bool chbEnableMinusActive, bool chbEnableDateActive = false, string dateactive = "");

        void AutoTakePhoto(string strCards);
        List<string> GetListCardNumber(string key, string cardgroups, string customergroups, string fromdate, string todate, string ischeckbytime = "0", string active = "", bool isfindautocapture = false);
        tblCard GetCardByCardNumberOrCardNo(string key);
        tblCard GetByCardNumber_Id(string cardnumber);
        List<string> GetCard(string key, string anotherkey, string cardgroups, string customergroups, string fromdate, string todate, string accesslevelids = "");

        bool AddCardExpireByListCardNumber_V2(string listCardNumber, int _feelevel, string _newexpire, string userId, bool chbEnableMinusActive, bool chbEnableDateActive = false, string dateactive = "", string subid = "");

        List<tblCardExtend> GetAllPagingByFirst_SQL(string key, string anotherkey, string cardgroups, string customergroups, string fromdate, string todate, ref int total, int pageNumber = 1, int pageSize = 20, string accesslevelids = "");

        IEnumerable<tblCard> GetCardByCardNumbers(string cardnumbers);

        List<string> GetCustomCard_Customer(string key, string cardgr ,string cus);

        tblCard GetCardByCardNo(string key);
    }

    public class tblCardService : ItblCardService
    {
        private ItblCardRepository _tblCardRepository;
        private ItblCardGroupRepository _tblCardGroupRepository;
        private ItblCustomerRepository _tblCustomerRepository;
        private ItblCustomerGroupRepository _tblCustomerGroupRepository;
        private ItblAccessLevelRepository _tblAccessLevelRepository;
        private IUnitOfWork _UnitOfWork;

        private ItblAccessLevelService _tblAccessLevelService;
        private IUser_AuthGroupService _User_AuthGroupService;
        private string AuthCardGroupIds = "";
        private ItblCompartmentService _tblCompartmentService;
        public tblCardService(ItblCompartmentService _tblCompartmentService,ItblCardRepository _tblCardRepository, ItblCardGroupRepository _tblCardGroupRepository, ItblCustomerRepository _tblCustomerRepository, ItblCustomerGroupRepository _tblCustomerGroupRepository, ItblAccessLevelRepository _tblAccessLevelRepository, IUnitOfWork _UnitOfWork, ItblAccessLevelService _tblAccessLevelService, IUser_AuthGroupService _User_AuthGroupService)
        {
            this._tblCardRepository = _tblCardRepository;
            this._tblCardGroupRepository = _tblCardGroupRepository;
            this._tblCustomerRepository = _tblCustomerRepository;
            this._tblCustomerGroupRepository = _tblCustomerGroupRepository;
            this._tblAccessLevelRepository = _tblAccessLevelRepository;
            this._UnitOfWork = _UnitOfWork;
            this._tblCompartmentService = _tblCompartmentService;
            this._tblAccessLevelService = _tblAccessLevelService;
            this._User_AuthGroupService = _User_AuthGroupService;

            AuthCardGroupIds = _User_AuthGroupService.GetAuthCardGroupIds();
        }

        private void Save()
        {
            _UnitOfWork.Commit();
        }

        public IEnumerable<tblCardExtend> GetAllActiveByListId(string ids)
        {
            var query = from card in _tblCardRepository.Table
                        join cardgroup in _tblCardGroupRepository.Table on card.CardGroupID equals cardgroup.CardGroupID.ToString() into card_cardgroup
                        from cardgroup in card_cardgroup.DefaultIfEmpty()
                        join customer in _tblCustomerRepository.Table on card.CustomerID equals customer.CustomerID.ToString() into card_customer
                        from customer in card_customer.DefaultIfEmpty()
                        join customergroup in _tblCustomerGroupRepository.Table on customer.CustomerGroupID equals customergroup.CustomerGroupID.ToString() into customer_group
                        from customergroup in customer_group.DefaultIfEmpty()

                        where /*cardgroup.CardType == 0 && */card.IsDelete == false && card.IsLock == false && ids.Contains(card.CardID.ToString())

                        select new tblCardExtend()
                        {
                            CardID = card.CardID.ToString(),
                            CardNo = card.CardNo,
                            CardNumber = card.CardNumber,
                            ExpireDate = card.ExpireDate,
                            ImportDate = card.ImportDate,
                            Plate1 = card.Plate1,
                            Plate2 = card.Plate2,
                            Plate3 = card.Plate3,
                            VehicleName1 = card.VehicleName1,
                            VehicleName2 = card.VehicleName2,
                            VehicleName3 = card.VehicleName3,

                            CustomerId = customer.CustomerID.ToString(),
                            CustomerCode = customer.CustomerCode,
                            CustomerName = customer.CustomerName,
                            Address = customer.Address,
                            AddressUnsign = customer.AddressUnsign,

                            CustomerGroupId = customergroup.CustomerGroupID.ToString(),
                            CustomerGroupName = customergroup.CustomerGroupName,

                            CardGroupId = cardgroup.CardGroupID.ToString(),
                            CardGroupName = cardgroup.CardGroupName,

                            AccessLevelID = card.AccessLevelID,
                        };

            return query;
        }

        public IPagedList<tblCardExtend> GetAllPagingByFirst(string key, string anotherkey, string cardgroups, string customergroups, string fromdate, string todate, int pageNumber = 1, int pageSize = 20, string accesslevelids = "")
        {
            var query = from card in _tblCardRepository.Table
                        join cardgroup in _tblCardGroupRepository.Table on card.CardGroupID equals cardgroup.CardGroupID.ToString() into card_cardgroup
                        from cardgroup in card_cardgroup.DefaultIfEmpty()
                        join customer in _tblCustomerRepository.Table on card.CustomerID equals customer.CustomerID.ToString() into card_customer
                        from customer in card_customer.DefaultIfEmpty()
                        join customergroup in _tblCustomerGroupRepository.Table on customer.CustomerGroupID equals customergroup.CustomerGroupID.ToString() into customer_group
                        from customergroup in customer_group.DefaultIfEmpty()
                        join accesslevel in _tblAccessLevelRepository.Table on card.AccessLevelID equals accesslevel.AccessLevelID.ToString() into card_accesslevel
                        from accesslevel in card_accesslevel.DefaultIfEmpty()

                        where cardgroup.CardType == 0 && card.IsDelete == false && card.IsLock == false

                        select new tblCardExtend()
                        {
                            CardID = card.CardID.ToString(),
                            CardNo = card.CardNo,
                            CardNumber = card.CardNumber,
                            ExpireDate = card.ExpireDate,
                            ImportDate = card.ImportDate,
                            Plate1 = card.Plate1,
                            Plate2 = card.Plate2,
                            Plate3 = card.Plate3,
                            VehicleName1 = card.VehicleName1,
                            VehicleName2 = card.VehicleName2,
                            VehicleName3 = card.VehicleName3,

                            CustomerId = customer.CustomerID.ToString(),
                            CustomerCode = customer.CustomerCode,
                            CustomerName = customer.CustomerName,

                            CustomerGroupId = customergroup.CustomerGroupID.ToString(),
                            CustomerGroupName = customergroup.CustomerGroupName,

                            CardGroupId = cardgroup.CardGroupID.ToString(),
                            CardGroupName = cardgroup.CardGroupName,

                            AccessLevelID = card.AccessLevelID,
                            AccessLevelName = accesslevel.AccessLevelName,

                            AccessExpireDate = card.AccessExpireDate,
                            Address = customer.Address,    
                            AddressUnsign = customer.AddressUnsign
                        };

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.CardNo.Contains(key) || n.CardNumber.Contains(key) || n.Plate1.Contains(key) || n.Plate2.Contains(key) || n.Plate3.Contains(key) || n.CustomerCode.Contains(key) || n.CustomerName.Contains(key) || n.Address.Contains(key) || n.AddressUnsign.Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(anotherkey))
            {
                query = query.Where(n => n.CardNo.Contains(anotherkey) || n.CardNumber.Contains(anotherkey) || n.Plate1.Contains(anotherkey) || n.Plate2.Contains(anotherkey) || n.Plate3.Contains(anotherkey) || n.CustomerCode.Contains(anotherkey) || n.CustomerName.Contains(anotherkey) || n.Address.Contains(anotherkey) || n.AddressUnsign.Contains(anotherkey));
            }

            if (!string.IsNullOrWhiteSpace(cardgroups))
            {
                query = query.Where(n => cardgroups.Contains(n.CardGroupId));
            }

            if (!string.IsNullOrEmpty(AuthCardGroupIds))
            {
                query = query.Where(n => AuthCardGroupIds.Contains(n.CardGroupId));
            }

            if (!string.IsNullOrWhiteSpace(customergroups))
            {
                query = query.Where(n => customergroups.Contains(n.CustomerGroupId));
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                var fdate = Convert.ToDateTime(fromdate);
                var tdate = Convert.ToDateTime(todate).AddDays(1);

                query = query.Where(n => n.ExpireDate >= fdate && n.ExpireDate < tdate);
            }

            if (!string.IsNullOrWhiteSpace(accesslevelids))
            {
                query = query.Where(n => accesslevelids.Contains(n.AccessLevelID));
            }

            var list = new PagedList<tblCardExtend>(query.OrderBy(n => n.CardNo), pageNumber, pageSize);

            return list;
        }

        public IPagedList<tblCardExtend> AQUA_GetAllPagingByFirst(string key,string address,string strIDCards, string anotherkey, string cardgroups, string customergroups, string fromdate, string todate, int pageNumber = 1, int pageSize = 20, string accesslevelids = "")
        {
            var query = from card in _tblCardRepository.Table
                        join cardgroup in _tblCardGroupRepository.Table on card.CardGroupID equals cardgroup.CardGroupID.ToString() into card_cardgroup
                        from cardgroup in card_cardgroup.DefaultIfEmpty()
                        join customer in _tblCustomerRepository.Table on card.CustomerID equals customer.CustomerID.ToString() into card_customer
                        from customer in card_customer.DefaultIfEmpty()
                        join customergroup in _tblCustomerGroupRepository.Table on customer.CustomerGroupID equals customergroup.CustomerGroupID.ToString() into customer_group
                        from customergroup in customer_group.DefaultIfEmpty()
                        join accesslevel in _tblAccessLevelRepository.Table on card.AccessLevelID equals accesslevel.AccessLevelID.ToString() into card_accesslevel
                        from accesslevel in card_accesslevel.DefaultIfEmpty()

                        where cardgroup.CardType == 0 && card.IsDelete == false && card.IsLock == false

                        select new tblCardExtend()
                        {
                            CardID = card.CardID.ToString(),
                            CardNo = card.CardNo,
                            CardNumber = card.CardNumber,
                            ExpireDate = card.ExpireDate,
                            ImportDate = card.ImportDate,
                            Plate1 = card.Plate1,
                            Plate2 = card.Plate2,
                            Plate3 = card.Plate3,
                            VehicleName1 = card.VehicleName1,
                            VehicleName2 = card.VehicleName2,
                            VehicleName3 = card.VehicleName3,

                            CustomerId = customer.CustomerID.ToString(),
                            CustomerCode = customer.CustomerCode,
                            CustomerName = customer.CustomerName,

                            CustomerGroupId = customergroup.CustomerGroupID.ToString(),
                            CustomerGroupName = customergroup.CustomerGroupName,

                            CardGroupId = cardgroup.CardGroupID.ToString(),
                            CardGroupName = cardgroup.CardGroupName,

                            AccessLevelID = card.AccessLevelID,
                            AccessLevelName = accesslevel.AccessLevelName,

                            AccessExpireDate = card.AccessExpireDate,
                            Address = customer.Address,
                            AddressUnsign = customer.AddressUnsign
                        };

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.CardNo.Contains(key) || n.CardNumber.Contains(key) || n.Plate1.Contains(key) || n.Plate2.Contains(key) || n.Plate3.Contains(key) || n.Address.Contains(key.Trim()) || n.AddressUnsign.Contains(key.Trim().Replace(",", "").Replace(".", "").ToLower()));
            }

            //if (!string.IsNullOrWhiteSpace(strIDCards))
            //{
            //    query = query.Where(n => !strIDCards.Contains(n.CardID));
            //}

            if (!string.IsNullOrWhiteSpace(anotherkey))
            {
                query = query.Where(n => n.CardNo.Contains(anotherkey) || n.CardNumber.Contains(anotherkey) || n.Plate1.Contains(anotherkey) || n.Plate2.Contains(anotherkey) || n.Plate3.Contains(anotherkey));
            }

            if (!string.IsNullOrWhiteSpace(cardgroups))
            {
                query = query.Where(n => cardgroups.Contains(n.CardGroupId));
            }

            if (!string.IsNullOrWhiteSpace(customergroups))
            {
                query = query.Where(n => customergroups.Contains(n.CustomerGroupId));
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                var fdate = Convert.ToDateTime(fromdate);
                var tdate = Convert.ToDateTime(todate).AddDays(1);

                query = query.Where(n => n.ExpireDate >= fdate && n.ExpireDate < tdate);
            }

            if (!string.IsNullOrWhiteSpace(accesslevelids))
            {
                query = query.Where(n => accesslevelids.Contains(n.AccessLevelID));
            }

            var list = new PagedList<tblCardExtend>(query.OrderBy(n => n.CardNo), pageNumber, pageSize);

            return list;
        }

        public List<tblCardExtend> AQUA_GetAllPagingByFirstSQL(string key, string address, string strIDCards, string anotherkey, string cardgroups, string customergroups, string fromdate, string todate,string newdate,ref double _totalmoneys,ref int total, int pageNumber = 1, int pageSize = 20)
        {
            if (string.IsNullOrEmpty(newdate))
            {
                newdate = DateTime.Now.ToString("yyyy/MM/dd");
            }
            else
            {
                newdate = Convert.ToDateTime(newdate).ToString("yyyy/MM/dd");
            }
            
            var query = new StringBuilder();

            #region Danh sách
            query.AppendLine("SELECT * FROM(");

            query.AppendLine("select ROW_NUMBER() OVER(ORDER BY c.[CardNo] asc) as RowNumber, c.CardID,c.CardNo,c.CardNumber,c.ExpireDate,c.ImportDate,c.Plate1,c.Plate2,c.Plate3,");
            query.AppendLine("cus.CustomerID,cus.CustomerCode,cus.CustomerName,cusg.CustomerGroupID,cus.Address,cusg.CustomerGroupName,");

            #region Tính tiền theo hạn mới và phí thuê bao
            query.AppendLine("ISNULL((select (CASE WHEN Price > 0 then Price else 0 END) as Price from ( select (");
            query.AppendLine("CASE");
            query.AppendLine("WHEN a.Units LIKE N'%Ngày%' THEN (a.FeeLevel / CONVERT(int,(SELECT LEFT(a.Units, CHARINDEX('_', a.Units) -1)))) ");
            query.AppendLine(string.Format("* CONVERT(int,DATEDIFF(DAY,c.ExpireDate,CONVERT(datetime,'{0}')))", newdate));
            query.AppendLine("WHEN a.Units LIKE N'%Tháng%' THEN (a.FeeLevel / (CONVERT(int,(SELECT LEFT(a.Units, CHARINDEX('_', a.Units) -1))) * 30))");
            query.AppendLine(string.Format("* CONVERT(int,DATEDIFF(DAY,c.ExpireDate,CONVERT(datetime,'{0}')))", newdate));
            query.AppendLine("WHEN a.Units LIKE N'%Quý%' THEN (a.FeeLevel / (CONVERT(int,(SELECT LEFT(a.Units, CHARINDEX('_', a.Units) -1))) * 90))");
            query.AppendLine(string.Format("* CONVERT(int,DATEDIFF(DAY,c.ExpireDate,CONVERT(datetime,'{0}')))", newdate));
            query.AppendLine("WHEN a.Units LIKE N'%Năm%' THEN (a.FeeLevel / (CONVERT(int,(SELECT LEFT(a.Units, CHARINDEX('_', a.Units) -1))) * 365))");
            query.AppendLine(string.Format("* CONVERT(int,DATEDIFF(DAY,c.ExpireDate,CONVERT(datetime,'{0}')))", newdate));         
            query.AppendLine("END ) as Price from (");
            query.AppendLine("select FeeLevel,Units from tblFee");
            query.AppendLine("where CardGroupID = c.CardGroupID AND IsUseExtend = 'True'");
            query.AppendLine(") as a ) as b),0) as Price");
            #endregion

            query.AppendLine(",cg.CardGroupID,cg.CardGroupName from tblCard c");
            query.AppendLine("left join tblCardGroup cg on c.CardGroupID = CONVERT(nvarchar(50), cg.CardGroupID)");
            query.AppendLine("left join tblCustomer cus on c.CustomerID = CONVERT(nvarchar(50), cus.CustomerID)");
            query.AppendLine("left join tblCustomerGroup cusg on cus.CustomerGroupID = CONVERT(nvarchar(50), cusg.CustomerGroupID)");
            query.AppendLine("where cg.CardType = 0 and c.IsDelete = 'False' and c.IsLock = 'False'");

            if (!string.IsNullOrWhiteSpace(key))
            {               
                query.AppendLine(string.Format("and (c.CardNumber LIKE '%{0}%' or c.CardNo LIKE '%{0}%' or c.Plate1 LIKE '%{0}%' or c.Plate2 LIKE '%{0}%' or c.Plate3 LIKE '%{0}%'", key));

                query.AppendLine(string.Format("or cus.Address LIKE N'%{0}%'", key.Trim()));

                query.AppendLine(string.Format("or cus.AddressUnsign LIKE N'%{0}%')", key.Trim().Replace(",", "").Replace(".", "").ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(anotherkey))
            {
                query.AppendLine(string.Format("and (c.CardNumber LIKE '%{0}%' or c.CardNo LIKE '%{0}%' or c.Plate1 LIKE '%{0}%' or c.Plate2 LIKE '%{0}%' or c.Plate3 LIKE '%{0}%'", anotherkey));

                query.AppendLine(string.Format("or cus.Address LIKE N'%{0}%'", anotherkey.Trim()));

                query.AppendLine(string.Format("or cus.AddressUnsign LIKE N'%{0}%')", anotherkey.Trim().Replace(",", "").Replace(".", "").ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(cardgroups))
            {
                query.AppendLine(string.Format("and c.CardGroupID = '{0}'", cardgroups));
            }     

            if (!string.IsNullOrWhiteSpace(customergroups))
            {
                var t = customergroups.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
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

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd HH:mm:ss");
                var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd HH:mm:ss");
                query.AppendLine(string.Format("and c.ExpireDate >= '{0}' and c.ExpireDate <= '{1}'", fdate, tdate));             
            }

            query.AppendLine(") as C1");
            query.AppendLine(string.Format("WHERE RowNumber BETWEEN (({0}-1) * {1} + 1) AND ({0} * {1})", pageNumber, pageSize));
            #endregion

            #region Tính tổng tiền
            query.AppendLine("select Count(*) as totalCount, SUM(cast(Price as bigint)) as totalMoney from (");

            query.AppendLine("select");        

            #region Tính tiền theo hạn mới và phí thuê bao
            query.AppendLine("ISNULL((select (CASE WHEN Price > 0 then Price else 0 END) as Price from ( select (");
            query.AppendLine("CASE");
            query.AppendLine("WHEN a.Units LIKE N'%Ngày%' THEN (a.FeeLevel / CONVERT(int,(SELECT LEFT(a.Units, CHARINDEX('_', a.Units) -1)))) ");
            query.AppendLine(string.Format("* CONVERT(int,DATEDIFF(DAY,c.ExpireDate,CONVERT(datetime,'{0}')))", newdate));
            query.AppendLine("WHEN a.Units LIKE N'%Tháng%' THEN (a.FeeLevel / (CONVERT(int,(SELECT LEFT(a.Units, CHARINDEX('_', a.Units) -1))) * 30))");
            query.AppendLine(string.Format("* CONVERT(int,DATEDIFF(DAY,c.ExpireDate,CONVERT(datetime,'{0}')))", newdate));
            query.AppendLine("WHEN a.Units LIKE N'%Quý%' THEN (a.FeeLevel / (CONVERT(int,(SELECT LEFT(a.Units, CHARINDEX('_', a.Units) -1))) * 90))");
            query.AppendLine(string.Format("* CONVERT(int,DATEDIFF(DAY,c.ExpireDate,CONVERT(datetime,'{0}')))", newdate));
            query.AppendLine("WHEN a.Units LIKE N'%Năm%' THEN (a.FeeLevel / (CONVERT(int,(SELECT LEFT(a.Units, CHARINDEX('_', a.Units) -1))) * 365))");
            query.AppendLine(string.Format("* CONVERT(int,DATEDIFF(DAY,c.ExpireDate,CONVERT(datetime,'{0}')))", newdate));
            query.AppendLine("END ) as Price from (");
            query.AppendLine("select FeeLevel,Units from tblFee");
            query.AppendLine("where CardGroupID = c.CardGroupID AND IsUseExtend = 'True'");
            query.AppendLine(") as a ) as b),0) as Price");
            #endregion

            query.AppendLine("from tblCard c");
            query.AppendLine("left join tblCardGroup cg on c.CardGroupID = CONVERT(nvarchar(50), cg.CardGroupID)");
            query.AppendLine("left join tblCustomer cus on c.CustomerID = CONVERT(nvarchar(50), cus.CustomerID)");
            query.AppendLine("left join tblCustomerGroup cusg on cus.CustomerGroupID = CONVERT(nvarchar(50), cusg.CustomerGroupID)");
            query.AppendLine("where cg.CardType = 0 and c.IsDelete = 'False' and c.IsLock = 'False'");

            if (!string.IsNullOrWhiteSpace(key))
            {
                query.AppendLine(string.Format("and (c.CardNumber LIKE '%{0}%' or c.CardNo LIKE '%{0}%' or c.Plate1 LIKE '%{0}%' or c.Plate2 LIKE '%{0}%' or c.Plate3 LIKE '%{0}%'", key));

                query.AppendLine(string.Format("or cus.Address LIKE N'%{0}%'", key.Trim()));

                query.AppendLine(string.Format("or cus.AddressUnsign LIKE N'%{0}%')", key.Trim().Replace(",", "").Replace(".", "").ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(anotherkey))
            {
                query.AppendLine(string.Format("and (c.CardNumber LIKE '%{0}%' or c.CardNo LIKE '%{0}%' or c.Plate1 LIKE '%{0}%' or c.Plate2 LIKE '%{0}%' or c.Plate3 LIKE '%{0}%'", anotherkey));

                query.AppendLine(string.Format("or cus.Address LIKE N'%{0}%'", anotherkey.Trim()));

                query.AppendLine(string.Format("or cus.AddressUnsign LIKE N'%{0}%')", anotherkey.Trim().Replace(",", "").Replace(".", "").ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(cardgroups))
            {
                query.AppendLine(string.Format("and c.CardGroupID = '{0}'", cardgroups));
            }

            if (!string.IsNullOrWhiteSpace(customergroups))
            {
                var t = customergroups.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
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

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd HH:mm:ss");
                var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd HH:mm:ss");
                query.AppendLine(string.Format("and c.ExpireDate >= '{0}' and c.ExpireDate <= '{1}'", fdate, tdate));
            }
            query.AppendLine(") as b");
            #endregion

            var list = ExcuteSQL.GetDataSet(query.ToString(), false);
            total = list.Tables.Count > 1 ? Convert.ToInt32(list.Tables[1].Rows[0]["totalCount"].ToString()) : 0;
            var a = list.Tables[1].Rows[0]["totalMoney"].ToString();
            _totalmoneys = !string.IsNullOrEmpty(list.Tables[1].Rows[0]["totalMoney"].ToString()) ? Convert.ToDouble(list.Tables[1].Rows[0]["totalMoney"].ToString()) : 0;

            return ExcuteSQL.ConvertTo<tblCardExtend>(list.Tables[0]);
        }

        public List<tblCardExtend> AQUA_MoneyByGroup(string key, string address, string strIDCards, string anotherkey, string cardgroups, string customergroups, string fromdate, string todate, string newdate)
        {
            if (string.IsNullOrEmpty(newdate))
            {
                newdate = DateTime.Now.ToString("yyyy/MM/dd");
            }
            else
            {
                newdate = Convert.ToDateTime(newdate).ToString("yyyy/MM/dd");
            }

            var query = new StringBuilder();       

            #region Tính tổng tiền
            query.AppendLine("select SUM(cast(Price as bigint)) as Price,CardGroupID as CardGroupId from (");

            query.AppendLine("select");

            #region Tính tiền theo hạn mới và phí thuê bao
            query.AppendLine("ISNULL((select (CASE WHEN Price > 0 then Price else 0 END) as Price from ( select (");
            query.AppendLine("CASE");
            query.AppendLine("WHEN a.Units LIKE N'%Ngày%' THEN (a.FeeLevel / CONVERT(int,(SELECT LEFT(a.Units, CHARINDEX('_', a.Units) -1)))) ");
            query.AppendLine(string.Format("* CONVERT(int,DATEDIFF(DAY,c.ExpireDate,CONVERT(datetime,'{0}')))", newdate));
            query.AppendLine("WHEN a.Units LIKE N'%Tháng%' THEN (a.FeeLevel / (CONVERT(int,(SELECT LEFT(a.Units, CHARINDEX('_', a.Units) -1))) * 30))");
            query.AppendLine(string.Format("* CONVERT(int,DATEDIFF(DAY,c.ExpireDate,CONVERT(datetime,'{0}')))", newdate));
            query.AppendLine("WHEN a.Units LIKE N'%Quý%' THEN (a.FeeLevel / (CONVERT(int,(SELECT LEFT(a.Units, CHARINDEX('_', a.Units) -1))) * 90))");
            query.AppendLine(string.Format("* CONVERT(int,DATEDIFF(DAY,c.ExpireDate,CONVERT(datetime,'{0}')))", newdate));
            query.AppendLine("WHEN a.Units LIKE N'%Năm%' THEN (a.FeeLevel / (CONVERT(int,(SELECT LEFT(a.Units, CHARINDEX('_', a.Units) -1))) * 365))");
            query.AppendLine(string.Format("* CONVERT(int,DATEDIFF(DAY,c.ExpireDate,CONVERT(datetime,'{0}')))", newdate));
            query.AppendLine("END ) as Price from (");
            query.AppendLine("select FeeLevel,Units from tblFee");
            query.AppendLine("where CardGroupID = c.CardGroupID AND IsUseExtend = 'True'");
            query.AppendLine(") as a ) as b),0) as Price,c.CardGroupID");
            #endregion

            query.AppendLine("from tblCard c");
            query.AppendLine("left join tblCardGroup cg on c.CardGroupID = CONVERT(nvarchar(50), cg.CardGroupID)");
            query.AppendLine("left join tblCustomer cus on c.CustomerID = CONVERT(nvarchar(50), cus.CustomerID)");
            query.AppendLine("left join tblCustomerGroup cusg on cus.CustomerGroupID = CONVERT(nvarchar(50), cusg.CustomerGroupID)");
            query.AppendLine("where cg.CardType = 0 and c.IsDelete = 'False' and c.IsLock = 'False'");

            if (!string.IsNullOrWhiteSpace(key))
            {
                query.AppendLine(string.Format("and (c.CardNumber LIKE '%{0}%' or c.CardNo LIKE '%{0}%' or c.Plate1 LIKE '%{0}%' or c.Plate2 LIKE '%{0}%' or c.Plate3 LIKE '%{0}%'", key));

                query.AppendLine(string.Format("or cus.Address LIKE N'%{0}%'", key.Trim()));

                query.AppendLine(string.Format("or cus.AddressUnsign LIKE N'%{0}%')", key.Trim().Replace(",", "").Replace(".", "").ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(anotherkey))
            {
                query.AppendLine(string.Format("and (c.CardNumber LIKE '%{0}%' or c.CardNo LIKE '%{0}%' or c.Plate1 LIKE '%{0}%' or c.Plate2 LIKE '%{0}%' or c.Plate3 LIKE '%{0}%'", anotherkey));

                query.AppendLine(string.Format("or cus.Address LIKE N'%{0}%'", anotherkey.Trim()));

                query.AppendLine(string.Format("or cus.AddressUnsign LIKE N'%{0}%')", anotherkey.Trim().Replace(",", "").Replace(".", "").ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(cardgroups))
            {
                query.AppendLine(string.Format("and c.CardGroupID = '{0}'", cardgroups));
            }

            if (!string.IsNullOrWhiteSpace(customergroups))
            {
                var t = customergroups.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
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

            if (!string.IsNullOrWhiteSpace(strIDCards))
            {
                var t = strIDCards.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and c.CardID NOT IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd HH:mm:ss");
                var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd HH:mm:ss");
                query.AppendLine(string.Format("and c.ExpireDate >= '{0}' and c.ExpireDate <= '{1}'", fdate, tdate));
            }
            query.AppendLine(") as b");
            query.AppendLine("group by CardGroupID");
            #endregion

            var list = ExcuteSQL.GetDataSet(query.ToString(), false);
        
            return ExcuteSQL.ConvertTo<tblCardExtend>(list.Tables[0]);
        }

        public tblCardExtend AQUA_GetListCardNumberExtendAll(string key, string strIDCards, string anotherkey, string cardgroups, string customergroups, string fromdate, string todate, string newDate)
        {          
            var query = new StringBuilder();

            #region Danh sách
            query.AppendLine("select STUFF((");
            query.AppendLine("SELECT ',' + CardNumber FROM(");

            query.AppendLine("select c.CardNumber from tblCard c");
            query.AppendLine("left join tblCardGroup cg on c.CardGroupID = CONVERT(nvarchar(50), cg.CardGroupID)");
            query.AppendLine("left join tblCustomer cus on c.CustomerID = CONVERT(nvarchar(50), cus.CustomerID)");
            query.AppendLine("left join tblCustomerGroup cusg on cus.CustomerGroupID = CONVERT(nvarchar(50), cusg.CustomerGroupID)");
            query.AppendLine("where cg.CardType = 0 and c.IsDelete = 'False' and c.IsLock = 'False'");

            if (!string.IsNullOrWhiteSpace(key))
            {
                query.AppendLine(string.Format("and (c.CardNumber LIKE '%{0}%' or c.CardNo LIKE '%{0}%' or c.Plate1 LIKE '%{0}%' or c.Plate2 LIKE '%{0}%' or c.Plate3 LIKE '%{0}%'", key));

                query.AppendLine(string.Format("or cus.Address LIKE N'%{0}%'", key.Trim()));

                query.AppendLine(string.Format("or cus.AddressUnsign LIKE N'%{0}%')", key.Trim().Replace(",", "").Replace(".", "").ToLower()));
            }

            if (!string.IsNullOrEmpty(strIDCards))
            {
                var t = strIDCards.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and c.CardID NOT IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            if (!string.IsNullOrWhiteSpace(anotherkey))
            {
                query.AppendLine(string.Format("and (c.CardNumber LIKE '%{0}%' or c.CardNo LIKE '%{0}%' or c.Plate1 LIKE '%{0}%' or c.Plate2 LIKE '%{0}%' or c.Plate3 LIKE '%{0}%'", anotherkey));

                query.AppendLine(string.Format("or cus.Address LIKE N'%{0}%'", anotherkey.Trim()));

                query.AppendLine(string.Format("or cus.AddressUnsign LIKE N'%{0}%')", anotherkey.Trim().Replace(",", "").Replace(".", "").ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(cardgroups))
            {
                query.AppendLine(string.Format("and c.CardGroupID = '{0}'", cardgroups));
            }

          
            if (!string.IsNullOrWhiteSpace(customergroups))
            {
                var t = customergroups.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
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

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd HH:mm:ss");
                var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd HH:mm:ss");
                query.AppendLine(string.Format("and c.ExpireDate >= '{0}' and c.ExpireDate <= '{1}'", fdate, tdate));
            }

            //if (!string.IsNullOrWhiteSpace(newDate))
            //{
            //    var ndate = Convert.ToDateTime(newDate).ToString("yyyy/MM/dd HH:mm:ss");             
            //    query.AppendLine(string.Format("and c.ExpireDate = '{0}'", ndate));
            //}

            query.AppendLine(") as C1");
            query.AppendLine("FOR XML PATH ('')");
            query.AppendLine("), 1, 1, '') as CardNumber");
            #endregion
       

            var list = ExcuteSQL.GetDataSet(query.ToString(), false);
      
            return ExcuteSQL.ConvertTo<tblCardExtend>(list.Tables[0]).FirstOrDefault();
        }

        public tblCard GetById(Guid id)
        {
            return _tblCardRepository.GetById(id);
        }

        public MessageReport Update(tblCard obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblCardRepository.Update(obj);

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

        public bool AddCardExpire(string KeyWord, string AnotherKey, string CardGroupIDs, string CustomerID, string CustomerGroupID, int _feelevel, string _newexpire, string userId, bool chbEnableMinusActive, bool chbEnableDateActive = false, string dateactive = "")
        {
            var sb = new StringBuilder();
            sb.AppendLine("INSERT INTO tblActiveCard(Code, [Date], CardNumber, CardNo, Plate, OldExpireDate, [Days], NewExpireDate, CardGroupID, CustomerGroupID, UserID, FeeLevel, CustomerID,IsDelete)");

            sb.AppendLine("SELECT CASE WHEN cus.CustomerCode IS NULL THEN '0' ELSE cus.CustomerCode END,GETDATE(), ca.Cardnumber,ca.CardNo");
            sb.AppendLine(", CAST(CASE WHEN ca.Plate2 <> '' THEN ISNULL(ca.Plate1,'') + ';' + ISNULL(ca.Plate2,'') WHEN ca.Plate3 <> '' THEN ISNULL(ca.Plate1,'') + ';' + ISNULL(ca.Plate2,'') + ';' + ISNULL(ca.Plate3,'') WHEN ca.Plate1 IS NULL THEN '' ELSE ca.Plate1 END AS nvarchar(50)) as Plate");
            sb.AppendLine(string.Format(", ca.[ExpireDate], DATEDIFF(DAY, ca.[ExpireDate], '{0}')", _newexpire));
            sb.AppendLine(string.Format(", '{0}', ca.CardGroupID, CASE WHEN  cus.CustomerGroupID IS NULL THEN '0' ELSE cus.CustomerGroupID END,'{2}','{1}', CASE WHEN ca.CustomerID IS NULL THEN '0' ELSE ca.CustomerID END,0", _newexpire, _feelevel, userId));

            sb.AppendLine("from tblCard ca");

            sb.AppendLine("LEFT JOIN dbo.tblCardGroup g ON ca.CardGroupID=g.CardGroupID");
            sb.AppendLine("LEFT join tblCustomer cus on ca.CustomerID = CONVERT(varchar(255), cus.CustomerID)");

            sb.AppendLine("where ca.IsDelete = 0 and ca.IsLock=0 /*and g.CardType=0*/");

            //Neu so ngay gia han <0 va neu ko check thi ko cho gia han
            if (chbEnableMinusActive == false)
            {
                sb.AppendLine(string.Format("and DATEDIFF(DAY, ca.[ExpireDate], '{0}') >=0  AND ca.[ExpireDate] <= '{0}'", _newexpire));
            }

            //Update theo filler
            if (!string.IsNullOrWhiteSpace(KeyWord))
            {
                sb.AppendLine(" and (ca.CardNo LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.CardNumber LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or cus.CustomerCode LIKE '%" + KeyWord + "%'");
                sb.AppendLine(" or cus.CustomerName LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or cus.Address LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or cus.AddressUnsign LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.Plate1 LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.Plate2 LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.Plate3 LIKE N'%" + KeyWord + "%')");
            }

            if (!string.IsNullOrWhiteSpace(AnotherKey))
            {
                sb.AppendLine(" and (ca.CardNo LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.CardNumber LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or cus.CustomerCode LIKE '%" + AnotherKey + "%'");
                sb.AppendLine(" or cus.CustomerName LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or cus.Address LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or cus.AddressUnsign LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.Plate1 LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.Plate2 LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.Plate3 LIKE N'%" + AnotherKey + "%')");
            }

            if (!string.IsNullOrWhiteSpace(CardGroupIDs))
            {
                sb.AppendLine(" and ca.CardGroupID = '" + CardGroupIDs + "'");
            }
            if (!string.IsNullOrWhiteSpace(AuthCardGroupIds))
            {
                var t = AuthCardGroupIds.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    sb.AppendLine("AND ca.CardGroupID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        sb.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    sb.AppendLine(" )");
                }
            }
            if (!string.IsNullOrWhiteSpace(CustomerID))
            {
                sb.AppendLine(" and ca.CustomerID = '" + CustomerID + "'");
            }
            if (!string.IsNullOrWhiteSpace(CustomerGroupID))
            {
                sb.AppendLine(" and cus.CustomerGroupID IN ('" + CustomerGroupID + "')");
            }

            //Update lai bang tblCard
            sb.AppendLine("UPDATE");
            sb.AppendLine("ca");
            sb.AppendLine(string.Format("SET ca.ExpireDate = '{0}'", _newexpire));

            if (chbEnableDateActive)
            {
                sb.AppendLine(string.Format(", ca.DateActive = '{0}'", dateactive));
            }

            sb.AppendLine("FROM");
            sb.AppendLine("tblCard AS ca");
            sb.AppendLine("LEFT JOIN dbo.tblCardGroup AS g ON ca.CardGroupID = g.CardGroupID");
            sb.AppendLine("LEFT join tblCustomer c on ca.CustomerID = CONVERT(varchar(255), c.CustomerID)");
            sb.AppendLine("WHERE IsDelete = 0 and ca.IsLock=0 /*and g.CardType = 0*/");

            if (chbEnableMinusActive == false)
            {
                sb.AppendLine(string.Format("and DATEDIFF(DAY, ca.[ExpireDate], '{0}') >=0  AND ca.[ExpireDate] <= '{0}'", _newexpire));
            }

            //Update theo filler
            if (!string.IsNullOrWhiteSpace(KeyWord))
            {
                sb.AppendLine(" and (ca.CardNo LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.CardNumber LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or c.CustomerCode LIKE '%" + KeyWord + "%'");
                sb.AppendLine(" or c.CustomerName LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or c.Address LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or c.AddressUnsign LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.Plate1 LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.Plate2 LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.Plate3 LIKE N'%" + KeyWord + "%')");
            }

            if (!string.IsNullOrWhiteSpace(AnotherKey))
            {
                sb.AppendLine(" and (ca.CardNo LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.CardNumber LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or c.CustomerCode LIKE '%" + AnotherKey + "%'");
                sb.AppendLine(" or c.CustomerName LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or c.Address LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or c.AddressUnsign LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.Plate1 LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.Plate2 LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.Plate3 LIKE N'%" + AnotherKey + "%')");
            }

            if (!string.IsNullOrWhiteSpace(CardGroupIDs))
            {
                sb.AppendLine(" and ca.CardGroupID = '" + CardGroupIDs + "'");
            }
            if (!string.IsNullOrWhiteSpace(AuthCardGroupIds))
            {
                var t = AuthCardGroupIds.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    sb.AppendLine("AND ca.CardGroupID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        sb.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    sb.AppendLine(" )");
                }
            }
            if (!string.IsNullOrWhiteSpace(CustomerID))
            {
                sb.AppendLine(" and ca.CustomerID = '" + CustomerID + "'");
            }

            if (!string.IsNullOrWhiteSpace(CustomerGroupID))
            {
                sb.AppendLine(" and c.CustomerGroupID IN ('" + CustomerGroupID + "')");
            }

            if (chbEnableDateActive)
            {
                sb.AppendLine("insert into tblCardProcess(Date, Actions, CardGroupID, UserID, CustomerID, CardNumber)");
                sb.AppendLine(string.Format("SELECT GETDATE(),'ACTIVE', ca.CardGroupID,  '{0}', CASE WHEN ca.CustomerID IS NULL THEN '0' ELSE ca.CustomerID END , ca.Cardnumber", userId));

                sb.AppendLine("from tblCard ca");

                sb.AppendLine("LEFT JOIN dbo.tblCardGroup g ON ca.CardGroupID=g.CardGroupID");
                sb.AppendLine("LEFT join tblCustomer cus on ca.CustomerID = CONVERT(varchar(255), cus.CustomerID)");

                sb.AppendLine("where ca.IsDelete = 0 and ca.IsLock=0 /*and g.CardType=0*/");

                //Neu so ngay gia han <0 va neu ko check thi ko cho gia han
                if (chbEnableMinusActive == false)
                {
                    sb.AppendLine(string.Format("and DATEDIFF(DAY, ca.[ExpireDate], '{0}') >=0  AND ca.[ExpireDate] <= '{0}'", _newexpire));
                }

                //Update theo filler
                if (!string.IsNullOrWhiteSpace(KeyWord))
                {
                    sb.AppendLine(" and (ca.CardNo LIKE N'%" + KeyWord + "%'");
                    sb.AppendLine(" or ca.CardNumber LIKE N'%" + KeyWord + "%'");
                    sb.AppendLine(" or cus.CustomerCode LIKE '%" + KeyWord + "%'");
                    sb.AppendLine(" or cus.CustomerName LIKE N'%" + KeyWord + "%'");
                    sb.AppendLine(" or cus.Address LIKE N'%" + KeyWord + "%'");
                    sb.AppendLine(" or cus.AddressUnsign LIKE N'%" + KeyWord + "%'");
                    sb.AppendLine(" or ca.Plate1 LIKE N'%" + KeyWord + "%'");
                    sb.AppendLine(" or ca.Plate2 LIKE N'%" + KeyWord + "%'");
                    sb.AppendLine(" or ca.Plate3 LIKE N'%" + KeyWord + "%')");
                }

                if (!string.IsNullOrWhiteSpace(AnotherKey))
                {
                    sb.AppendLine(" and (ca.CardNo LIKE N'%" + AnotherKey + "%'");
                    sb.AppendLine(" or ca.CardNumber LIKE N'%" + AnotherKey + "%'");
                    sb.AppendLine(" or cus.CustomerCode LIKE '%" + AnotherKey + "%'");
                    sb.AppendLine(" or cus.CustomerName LIKE N'%" + AnotherKey + "%'");
                    sb.AppendLine(" or cus.Address LIKE N'%" + AnotherKey + "%'");
                    sb.AppendLine(" or cus.AddressUnsign LIKE N'%" + AnotherKey + "%'");
                    sb.AppendLine(" or ca.Plate1 LIKE N'%" + AnotherKey + "%'");
                    sb.AppendLine(" or ca.Plate2 LIKE N'%" + AnotherKey + "%'");
                    sb.AppendLine(" or ca.Plate3 LIKE N'%" + AnotherKey + "%')");
                }

                if (!string.IsNullOrWhiteSpace(CardGroupIDs))
                {
                    sb.AppendLine(" and ca.CardGroupID = '" + CardGroupIDs + "'");
                }
                if (!string.IsNullOrWhiteSpace(AuthCardGroupIds))
                {
                    var t = AuthCardGroupIds.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    if (t.Any())
                    {
                        var count = 0;

                        sb.AppendLine("AND ca.CardGroupID IN ( ");

                        foreach (var item in t)
                        {
                            count++;

                            sb.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                        }

                        sb.AppendLine(" )");
                    }
                }

                if (!string.IsNullOrWhiteSpace(CustomerID))
                {
                    sb.AppendLine(" and ca.CustomerID = '" + CustomerID + "'");
                }

                if (!string.IsNullOrWhiteSpace(CustomerGroupID))
                {
                    sb.AppendLine(" and cus.CustomerGroupID IN ('" + CustomerGroupID + " ')");
                }
            }

            return ExcuteSQL.Execute(sb.ToString());
        }

        public bool AddCardExpireOneMonth(string KeyWord, string AnotherKey, string CardGroupIDs, string CustomerID, string CustomerGroupID, int _feelevel, string _newexpire, string userId, bool chbEnableMinusActive, bool chbEnableDateActive = false, string dateactive = "")
        {
            var sb = new StringBuilder();
            sb.AppendLine("INSERT INTO tblActiveCard(Code, [Date], CardNumber, CardNo, Plate, OldExpireDate, [Days], NewExpireDate, CardGroupID, CustomerGroupID, UserID, FeeLevel, CustomerID,IsDelete)");

            sb.AppendLine("SELECT CASE WHEN cus.CustomerCode IS NULL THEN '0' ELSE cus.CustomerCode END,GETDATE(), ca.Cardnumber,ca.CardNo");
            sb.AppendLine(", CAST(CASE WHEN ca.Plate2 <> '' THEN ISNULL(ca.Plate1,'') + ';' + ISNULL(ca.Plate2,'') WHEN ca.Plate3 <> '' THEN ISNULL(ca.Plate1,'') + ';' + ISNULL(ca.Plate2,'') + ';' + ISNULL(ca.Plate3,'') WHEN ca.Plate1 IS NULL THEN '' ELSE ca.Plate1 END AS nvarchar(50)) as Plate");
            sb.AppendLine(string.Format(", ca.[ExpireDate], DATEDIFF(DAY, ca.[ExpireDate], DATEADD(MONTH, 1, ca.[ExpireDate]))"));
            sb.AppendLine(string.Format(", DATEADD(MONTH, 1, ca.[ExpireDate]), ca.CardGroupID, CASE WHEN  cus.CustomerGroupID IS NULL THEN '0' ELSE cus.CustomerGroupID END,'{1}','{0}', CASE WHEN ca.CustomerID IS NULL THEN '0' ELSE ca.CustomerID END,0", _feelevel, userId));

            sb.AppendLine("from tblCard ca");

            sb.AppendLine("LEFT JOIN dbo.tblCardGroup g ON ca.CardGroupID=g.CardGroupID");
            sb.AppendLine("LEFT join tblCustomer cus on ca.CustomerID = CONVERT(varchar(255), cus.CustomerID)");

            sb.AppendLine("where ca.IsDelete = 0 and ca.IsLock=0 and g.CardType=0");

            //Neu so ngay gia han <0 va neu ko check thi ko cho gia han
            //if (chbEnableMinusActive == false)
            //{
            //    sb.AppendLine(string.Format("and DATEDIFF(DAY, ca.[ExpireDate], '{0}') >=0  AND ca.[ExpireDate] <= '{0}'", _newexpire));
            //}

            //Update theo filler
            if (!string.IsNullOrWhiteSpace(KeyWord))
            {
                sb.AppendLine(" and (ca.CardNo LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.CardNumber LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or cus.CustomerCode LIKE '%" + KeyWord + "%'");
                sb.AppendLine(" or cus.CustomerName LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or cus.Address LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or cus.AddressUnsign LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.Plate1 LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.Plate2 LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.Plate3 LIKE N'%" + KeyWord + "%')");
            }

            if (!string.IsNullOrWhiteSpace(AnotherKey))
            {
                sb.AppendLine(" and (ca.CardNo LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.CardNumber LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or cus.CustomerCode LIKE '%" + AnotherKey + "%'");
                sb.AppendLine(" or cus.CustomerName LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or cus.Address LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or cus.AddressUnsign LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.Plate1 LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.Plate2 LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.Plate3 LIKE N'%" + AnotherKey + "%')");
            }

            if (!string.IsNullOrWhiteSpace(CardGroupIDs))
            {
                sb.AppendLine(" and ca.CardGroupID = '" + CardGroupIDs + "'");
            }
            if (!string.IsNullOrWhiteSpace(CustomerID))
            {
                sb.AppendLine(" and ca.CustomerID = '" + CustomerID + "'");
            }
            if (!string.IsNullOrWhiteSpace(CustomerGroupID))
            {
                sb.AppendLine(" and cus.CustomerGroupID IN ('" + CustomerGroupID + "')");
            }

            //Update lai bang tblCard
            sb.AppendLine("UPDATE");
            sb.AppendLine("ca");
            sb.AppendLine(string.Format("SET ca.ExpireDate = DATEADD(MONTH, 1, ca.ExpireDate)"));

            if (chbEnableDateActive)
            {
                sb.AppendLine(string.Format(", ca.DateActive = '{0}'", dateactive));
            }

            sb.AppendLine("FROM");
            sb.AppendLine("tblCard AS ca");
            sb.AppendLine("LEFT JOIN dbo.tblCardGroup AS g ON ca.CardGroupID = g.CardGroupID");
            sb.AppendLine("LEFT join tblCustomer c on ca.CustomerID = CONVERT(varchar(255), c.CustomerID)");
            sb.AppendLine("WHERE IsDelete = 0 and ca.IsLock=0 and g.CardType = 0");

            //if (chbEnableMinusActive == false)
            //{
            //    sb.AppendLine(string.Format("and DATEDIFF(DAY, ca.[ExpireDate], '{0}') >=0  AND ca.[ExpireDate] <= '{0}'", _newexpire));
            //}

            //Update theo filler
            if (!string.IsNullOrWhiteSpace(KeyWord))
            {
                sb.AppendLine(" and (ca.CardNo LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.CardNumber LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or c.CustomerCode LIKE '%" + KeyWord + "%'");
                sb.AppendLine(" or c.CustomerName LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or c.Address LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or c.AddressUnsign LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.Plate1 LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.Plate2 LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.Plate3 LIKE N'%" + KeyWord + "%')");
            }

            if (!string.IsNullOrWhiteSpace(AnotherKey))
            {
                sb.AppendLine(" and (ca.CardNo LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.CardNumber LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or c.CustomerCode LIKE '%" + AnotherKey + "%'");
                sb.AppendLine(" or c.CustomerName LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or c.Address LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or c.AddressUnsign LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.Plate1 LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.Plate2 LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.Plate3 LIKE N'%" + AnotherKey + "%')");
            }

            if (!string.IsNullOrWhiteSpace(CardGroupIDs))
            {
                sb.AppendLine(" and ca.CardGroupID = '" + CardGroupIDs + "'");
            }

            if (!string.IsNullOrWhiteSpace(CustomerID))
            {
                sb.AppendLine(" and ca.CustomerID = '" + CustomerID + "'");
            }

            if (!string.IsNullOrWhiteSpace(CustomerGroupID))
            {
                sb.AppendLine(" and c.CustomerGroupID IN ('" + CustomerGroupID + "')");
            }

            if (chbEnableDateActive)
            {
                sb.AppendLine("insert into tblCardProcess(Date, Actions, CardGroupID, UserID, CustomerID, CardNumber)");
                sb.AppendLine(string.Format("SELECT GETDATE(),'ACTIVE', ca.CardGroupID,  '{0}', CASE WHEN ca.CustomerID IS NULL THEN '0' ELSE ca.CustomerID END , ca.Cardnumber", userId));

                sb.AppendLine("from tblCard ca");

                sb.AppendLine("LEFT JOIN dbo.tblCardGroup g ON ca.CardGroupID=g.CardGroupID");
                sb.AppendLine("LEFT join tblCustomer cus on ca.CustomerID = CONVERT(varchar(255), cus.CustomerID)");

                sb.AppendLine("where ca.IsDelete = 0 and ca.IsLock=0 and g.CardType=0");

                //Neu so ngay gia han <0 va neu ko check thi ko cho gia han
                //if (chbEnableMinusActive == false)
                //{
                //    sb.AppendLine(string.Format("and DATEDIFF(DAY, ca.[ExpireDate], '{0}') >=0  AND ca.[ExpireDate] <= '{0}'", _newexpire));
                //}

                //Update theo filler
                if (!string.IsNullOrWhiteSpace(KeyWord))
                {
                    sb.AppendLine(" and (ca.CardNo LIKE N'%" + KeyWord + "%'");
                    sb.AppendLine(" or ca.CardNumber LIKE N'%" + KeyWord + "%'");
                    sb.AppendLine(" or cus.CustomerCode LIKE '%" + KeyWord + "%'");
                    sb.AppendLine(" or cus.CustomerName LIKE N'%" + KeyWord + "%'");
                    sb.AppendLine(" or cus.Address LIKE N'%" + KeyWord + "%'");
                    sb.AppendLine(" or cus.AddressUnsign LIKE N'%" + KeyWord + "%'");
                    sb.AppendLine(" or ca.Plate1 LIKE N'%" + KeyWord + "%'");
                    sb.AppendLine(" or ca.Plate2 LIKE N'%" + KeyWord + "%'");
                    sb.AppendLine(" or ca.Plate3 LIKE N'%" + KeyWord + "%')");
                }

                if (!string.IsNullOrWhiteSpace(AnotherKey))
                {
                    sb.AppendLine(" and (ca.CardNo LIKE N'%" + AnotherKey + "%'");
                    sb.AppendLine(" or ca.CardNumber LIKE N'%" + AnotherKey + "%'");
                    sb.AppendLine(" or cus.CustomerCode LIKE '%" + AnotherKey + "%'");
                    sb.AppendLine(" or cus.CustomerName LIKE N'%" + AnotherKey + "%'");
                    sb.AppendLine(" or cus.Address LIKE N'%" + AnotherKey + "%'");
                    sb.AppendLine(" or cus.AddressUnsign LIKE N'%" + AnotherKey + "%'");
                    sb.AppendLine(" or ca.Plate1 LIKE N'%" + AnotherKey + "%'");
                    sb.AppendLine(" or ca.Plate2 LIKE N'%" + AnotherKey + "%'");
                    sb.AppendLine(" or ca.Plate3 LIKE N'%" + AnotherKey + "%')");
                }

                if (!string.IsNullOrWhiteSpace(CardGroupIDs))
                {
                    sb.AppendLine(" and ca.CardGroupID = '" + CardGroupIDs + "'");
                }

                if (!string.IsNullOrWhiteSpace(CustomerID))
                {
                    sb.AppendLine(" and ca.CustomerID = '" + CustomerID + "'");
                }

                if (!string.IsNullOrWhiteSpace(CustomerGroupID))
                {
                    sb.AppendLine(" and cus.CustomerGroupID IN ('" + CustomerGroupID + " ')");
                }
            }

            return ExcuteSQL.Execute(sb.ToString());
        }

        public bool AQUA_AddCardExpire(string KeyWord, string orderId, string strIDCards, string AnotherKey, string CardGroupIDs, string CustomerID, string CustomerGroupID, int _feelevel, string _newexpire, string userId, bool chbEnableMinusActive, bool chbEnableDateActive = false, string dateactive = "")
        {
            var sb = new StringBuilder();
            sb.AppendLine("INSERT INTO tblActiveCard(Code, [Date], CardNumber, CardNo, Plate, OldExpireDate, [Days], NewExpireDate, CardGroupID, CustomerGroupID, UserID, FeeLevel, CustomerID,IsDelete,OrderId)");

            sb.AppendLine("SELECT CASE WHEN cus.CustomerCode IS NULL THEN '0' ELSE cus.CustomerCode END,GETDATE(), ca.Cardnumber,ca.CardNo");
            sb.AppendLine(", CAST(CASE WHEN ca.Plate2 <> '' THEN ISNULL(ca.Plate1,'') + ';' + ISNULL(ca.Plate2,'') WHEN ca.Plate3 <> '' THEN ISNULL(ca.Plate1,'') + ';' + ISNULL(ca.Plate2,'') + ';' + ISNULL(ca.Plate3,'') WHEN ca.Plate1 IS NULL THEN '' ELSE ca.Plate1 END AS nvarchar(50)) as Plate");
            sb.AppendLine(string.Format(", ca.[ExpireDate], DATEDIFF(DAY, ca.[ExpireDate], '{0}')", _newexpire));
            sb.AppendLine(string.Format(", '{0}', ca.CardGroupID, CASE WHEN  cus.CustomerGroupID IS NULL THEN '0' ELSE cus.CustomerGroupID END,'{1}',", _newexpire, userId));

            #region Tính tiền theo hạn mới và phí thuê bao
            sb.AppendLine("Convert(int,ISNULL((select (CASE WHEN Price > 0 then Price else 0 END) as Price from ( select (");
            sb.AppendLine("CASE");
            sb.AppendLine("WHEN a.Units LIKE N'%Ngày%' THEN (a.FeeLevel / CONVERT(int,(SELECT LEFT(a.Units, CHARINDEX('_', a.Units) -1)))) ");
            sb.AppendLine(string.Format("* CONVERT(int,DATEDIFF(DAY,ca.ExpireDate,CONVERT(datetime,'{0}')))", _newexpire));
            sb.AppendLine("WHEN a.Units LIKE N'%Tháng%' THEN (a.FeeLevel / (CONVERT(int,(SELECT LEFT(a.Units, CHARINDEX('_', a.Units) -1))) * 30))");
            sb.AppendLine(string.Format("* CONVERT(int,DATEDIFF(DAY,ca.ExpireDate,CONVERT(datetime,'{0}')))", _newexpire));
            sb.AppendLine("WHEN a.Units LIKE N'%Quý%' THEN (a.FeeLevel / (CONVERT(int,(SELECT LEFT(a.Units, CHARINDEX('_', a.Units) -1))) * 90))");
            sb.AppendLine(string.Format("* CONVERT(int,DATEDIFF(DAY,ca.ExpireDate,CONVERT(datetime,'{0}')))", _newexpire));
            sb.AppendLine("WHEN a.Units LIKE N'%Năm%' THEN (a.FeeLevel / (CONVERT(int,(SELECT LEFT(a.Units, CHARINDEX('_', a.Units) -1))) * 365))");
            sb.AppendLine(string.Format("* CONVERT(int,DATEDIFF(DAY,ca.ExpireDate,CONVERT(datetime,'{0}')))", _newexpire));
            sb.AppendLine("END ) as Price from (");
            sb.AppendLine("select FeeLevel,Units from tblFee");
            sb.AppendLine("where CardGroupID = ca.CardGroupID AND IsUseExtend = 'True'");
            sb.AppendLine(") as a ) as b),0))");
            #endregion

            sb.AppendLine(string.Format(",CASE WHEN ca.CustomerID IS NULL THEN '0' ELSE ca.CustomerID END,0,'{0}'",orderId));

            sb.AppendLine("from tblCard ca");

            sb.AppendLine("LEFT JOIN dbo.tblCardGroup g ON ca.CardGroupID=CONVERT(varchar(255), g.CardGroupID)");
            sb.AppendLine("LEFT join tblCustomer cus on ca.CustomerID = CONVERT(varchar(255), cus.CustomerID)");

            sb.AppendLine("where ca.IsDelete = 0 and ca.IsLock=0 and g.CardType=0");

            //Neu so ngay gia han <0 va neu ko check thi ko cho gia han
            if (chbEnableMinusActive == false)
            {
                sb.AppendLine(string.Format("and DATEDIFF(DAY, ca.[ExpireDate], '{0}') >=0  AND ca.[ExpireDate] <= '{0}'", _newexpire));
            }

            //Update theo filler
            if (!string.IsNullOrWhiteSpace(KeyWord))
            {
                sb.AppendLine(" and (ca.CardNo LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.CardNumber LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or cus.Address LIKE N'%" + KeyWord.Trim() + "%'");
                sb.AppendLine(" or cus.AddressUnsign LIKE N'%" + KeyWord.Trim().Replace(",", "").Replace(".", "").ToLower() + "%'");
                sb.AppendLine(" or ca.Plate1 LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.Plate2 LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.Plate3 LIKE N'%" + KeyWord + "%')");
            }

            if (!string.IsNullOrEmpty(strIDCards))
            {
                var t = strIDCards.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    sb.AppendLine("and ca.CardID NOT IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        sb.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    sb.AppendLine(" )");
                }
            }

            if (!string.IsNullOrWhiteSpace(AnotherKey))
            {
                sb.AppendLine(" and (ca.CardNo LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.CardNumber LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or cus.Address LIKE N'%" + AnotherKey.Trim() + "%'");
                sb.AppendLine(" or cus.AddressUnsign LIKE N'%" + AnotherKey.Trim().Replace(",", "").Replace(".", "").ToLower() + "%'");
                sb.AppendLine(" or ca.Plate1 LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.Plate2 LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.Plate3 LIKE N'%" + AnotherKey + "%')");          
            }

            if (!string.IsNullOrWhiteSpace(CardGroupIDs))
            {
                sb.AppendLine(" and ca.CardGroupID = '" + CardGroupIDs + "'");
            }
            if (!string.IsNullOrWhiteSpace(CustomerID))
            {
                sb.AppendLine(" and ca.CustomerID = '" + CustomerID + "'");
            }
            //if (!string.IsNullOrWhiteSpace(CustomerGroupID))
            //{
            //    sb.AppendLine(" and cus.CustomerGroupID IN ('" + CustomerGroupID + "')");
            //}
            if (!string.IsNullOrWhiteSpace(CustomerGroupID))
            {
                var t = CustomerGroupID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    sb.AppendLine("and cus.CustomerGroupID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        sb.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    sb.AppendLine(" )");
                }
            }

            //Update lai bang tblCard
            sb.AppendLine("UPDATE");
            sb.AppendLine("ca");
            sb.AppendLine(string.Format("SET ca.ExpireDate = '{0}'", _newexpire));

            if (chbEnableDateActive)
            {
                sb.AppendLine(string.Format(", ca.DateActive = '{0}'", dateactive));
            }

            sb.AppendLine("FROM");
            sb.AppendLine("tblCard AS ca");
            sb.AppendLine("LEFT JOIN dbo.tblCardGroup AS g ON ca.CardGroupID = CONVERT(varchar(255), g.CardGroupID)");
            sb.AppendLine("LEFT join tblCustomer c on ca.CustomerID = CONVERT(varchar(255), c.CustomerID)");
            sb.AppendLine("WHERE IsDelete = 0 and ca.IsLock=0 and g.CardType = 0");

            if (chbEnableMinusActive == false)
            {
                sb.AppendLine(string.Format("and DATEDIFF(DAY, ca.[ExpireDate], '{0}') >=0  AND ca.[ExpireDate] <= '{0}'", _newexpire));
            }

            //Update theo filler
            if (!string.IsNullOrWhiteSpace(KeyWord))
            {
                sb.AppendLine(" and (ca.CardNo LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.CardNumber LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or c.Address LIKE N'%" + KeyWord.Trim() + "%'");
                sb.AppendLine(" or c.AddressUnsign LIKE N'%" + KeyWord.Trim().Replace(",", "").Replace(".", "").ToLower() + "%'");
                sb.AppendLine(" or ca.Plate1 LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.Plate2 LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.Plate3 LIKE N'%" + KeyWord + "%')");
            }

            if (!string.IsNullOrEmpty(strIDCards))
            {
                var t = strIDCards.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    sb.AppendLine("and ca.CardID NOT IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        sb.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    sb.AppendLine(" )");
                }
            }

            if (!string.IsNullOrWhiteSpace(AnotherKey))
            {
                sb.AppendLine(" and (ca.CardNo LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.CardNumber LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or c.Address LIKE N'%" + AnotherKey.Trim() + "%'");
                sb.AppendLine(" or c.AddressUnsign LIKE N'%" + AnotherKey.Trim().Replace(",", "").Replace(".", "").ToLower() + "%'");
                sb.AppendLine(" or ca.Plate1 LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.Plate2 LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.Plate3 LIKE N'%" + AnotherKey + "%')");
            }

            if (!string.IsNullOrWhiteSpace(CardGroupIDs))
            {
                sb.AppendLine(" and ca.CardGroupID = '" + CardGroupIDs + "'");
            }

            if (!string.IsNullOrWhiteSpace(CustomerID))
            {
                sb.AppendLine(" and ca.CustomerID = '" + CustomerID + "'");
            }

            //if (!string.IsNullOrWhiteSpace(CustomerGroupID))
            //{
            //    sb.AppendLine(" and c.CustomerGroupID IN ('" + CustomerGroupID + "')");
            //}
            if (!string.IsNullOrWhiteSpace(CustomerGroupID))
            {
                var t = CustomerGroupID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    sb.AppendLine("and c.CustomerGroupID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        sb.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    sb.AppendLine(" )");
                }
            }

            if (chbEnableDateActive)
            {
                sb.AppendLine("insert into tblCardProcess(Date, Actions, CardGroupID, UserID, CustomerID, CardNumber)");
                sb.AppendLine(string.Format("SELECT GETDATE(),'ACTIVE', ca.CardGroupID,  '{0}', CASE WHEN ca.CustomerID IS NULL THEN '0' ELSE ca.CustomerID END , ca.Cardnumber", userId));

                sb.AppendLine("from tblCard ca");

                sb.AppendLine("LEFT JOIN dbo.tblCardGroup g ON ca.CardGroupID= CONVERT(varchar(255), g.CardGroupID)");
                sb.AppendLine("LEFT join tblCustomer cus on ca.CustomerID = CONVERT(varchar(255), cus.CustomerID)");

                sb.AppendLine("where ca.IsDelete = 0 and ca.IsLock=0 and g.CardType=0");

                //Neu so ngay gia han <0 va neu ko check thi ko cho gia han
                if (chbEnableMinusActive == false)
                {
                    sb.AppendLine(string.Format("and DATEDIFF(DAY, ca.[ExpireDate], '{0}') >=0  AND ca.[ExpireDate] <= '{0}'", _newexpire));
                }

                //Update theo filler             
                if (!string.IsNullOrWhiteSpace(KeyWord))
                {
                    sb.AppendLine(" and (ca.CardNo LIKE N'%" + KeyWord + "%'");
                    sb.AppendLine(" or ca.CardNumber LIKE N'%" + KeyWord + "%'");
                    sb.AppendLine(" or cus.Address LIKE N'%" + KeyWord.Trim() + "%'");
                    sb.AppendLine(" or cus.AddressUnsign LIKE N'%" + KeyWord.Trim().Replace(",", "").Replace(".", "").ToLower() + "%'");
                    sb.AppendLine(" or ca.Plate1 LIKE N'%" + KeyWord + "%'");
                    sb.AppendLine(" or ca.Plate2 LIKE N'%" + KeyWord + "%'");
                    sb.AppendLine(" or ca.Plate3 LIKE N'%" + KeyWord + "%')");
                }

                if (!string.IsNullOrWhiteSpace(AnotherKey))
                {
                    sb.AppendLine(" and (ca.CardNo LIKE N'%" + AnotherKey + "%'");
                    sb.AppendLine(" or ca.CardNumber LIKE N'%" + AnotherKey + "%'");
                    sb.AppendLine(" or cus.Address LIKE N'%" + AnotherKey.Trim() + "%'");
                    sb.AppendLine(" or cus.AddressUnsign LIKE N'%" + AnotherKey.Trim().Replace(",", "").Replace(".", "").ToLower() + "%'");
                    sb.AppendLine(" or ca.Plate1 LIKE N'%" + AnotherKey + "%'");
                    sb.AppendLine(" or ca.Plate2 LIKE N'%" + AnotherKey + "%'");
                    sb.AppendLine(" or ca.Plate3 LIKE N'%" + AnotherKey + "%')");
                }

                if (!string.IsNullOrWhiteSpace(CardGroupIDs))
                {
                    sb.AppendLine(" and ca.CardGroupID = '" + CardGroupIDs + "'");
                }

                if (!string.IsNullOrWhiteSpace(CustomerID))
                {
                    sb.AppendLine(" and ca.CustomerID = '" + CustomerID + "'");
                }

                //if (!string.IsNullOrWhiteSpace(CustomerGroupID))
                //{
                //    sb.AppendLine(" and cus.CustomerGroupID IN ('" + CustomerGroupID + " ')");
                //}

                if (!string.IsNullOrWhiteSpace(CustomerGroupID))
                {
                    var t = CustomerGroupID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (t.Any())
                    {
                        var count = 0;

                        sb.AppendLine("and cus.CustomerGroupID IN ( ");

                        foreach (var item in t)
                        {
                            count++;

                            sb.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                        }

                        sb.AppendLine(" )");
                    }
                }
            }

            return ExcuteSQL.Execute(sb.ToString());
        }

        public bool AQUA_AddCardExpire_v2(string KeyWord, string orderId, string strIDCards, string AnotherKey, string CardGroupIDs, string CustomerID, string CustomerGroupID, int _feelevel, string _newexpire, string userId, bool chbEnableMinusActive, bool chbEnableDateActive = false, string dateactive = "")
        {
            var sb = new StringBuilder();
            sb.AppendLine("INSERT INTO tblActiveCard(Code, [Date], CardNumber, CardNo, Plate, OldExpireDate, [Days], NewExpireDate, CardGroupID, CustomerGroupID, UserID, FeeLevel, CustomerID,IsDelete,OrderId)");

            sb.AppendLine("SELECT CASE WHEN cus.CustomerCode IS NULL THEN '0' ELSE cus.CustomerCode END,GETDATE(), ca.Cardnumber,ca.CardNo");
            sb.AppendLine(", CAST(CASE WHEN ca.Plate2 <> '' THEN ISNULL(ca.Plate1,'') + ';' + ISNULL(ca.Plate2,'') WHEN ca.Plate3 <> '' THEN ISNULL(ca.Plate1,'') + ';' + ISNULL(ca.Plate2,'') + ';' + ISNULL(ca.Plate3,'') WHEN ca.Plate1 IS NULL THEN '' ELSE ca.Plate1 END AS nvarchar(50)) as Plate");
            sb.AppendLine(string.Format(", ca.[ExpireDate], DATEDIFF(DAY, ca.[ExpireDate], '{0}')", _newexpire));
            sb.AppendLine(string.Format(", '{0}', ca.CardGroupID, CASE WHEN  cus.CustomerGroupID IS NULL THEN '0' ELSE cus.CustomerGroupID END,'{1}','{2}'", _newexpire, userId, _feelevel));

           

            sb.AppendLine(string.Format(",CASE WHEN ca.CustomerID IS NULL THEN '0' ELSE ca.CustomerID END,0,'{0}'", orderId));

            sb.AppendLine("from tblCard ca");

            sb.AppendLine("LEFT JOIN dbo.tblCardGroup g ON ca.CardGroupID=g.CardGroupID");
            sb.AppendLine("LEFT join tblCustomer cus on ca.CustomerID = CONVERT(varchar(255), cus.CustomerID)");

            sb.AppendLine("where ca.IsDelete = 0 and ca.IsLock=0 and g.CardType=0");

            //Neu so ngay gia han <0 va neu ko check thi ko cho gia han
            if (chbEnableMinusActive == false)
            {
                sb.AppendLine(string.Format("and DATEDIFF(DAY, ca.[ExpireDate], '{0}') >=0  AND ca.[ExpireDate] <= '{0}'", _newexpire));
            }

            //Update theo filler
            if (!string.IsNullOrWhiteSpace(KeyWord))
            {
                sb.AppendLine(" and (ca.CardNo LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.CardNumber LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or cus.Address LIKE N'%" + KeyWord.Trim() + "%'");
                sb.AppendLine(" or cus.AddressUnsign LIKE N'%" + KeyWord.Trim().Replace(",", "").Replace(".", "").ToLower() + "%'");
                sb.AppendLine(" or ca.Plate1 LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.Plate2 LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.Plate3 LIKE N'%" + KeyWord + "%')");
            }

            if (!string.IsNullOrEmpty(strIDCards))
            {
                var t = strIDCards.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    sb.AppendLine("and ca.CardID NOT IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        sb.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    sb.AppendLine(" )");
                }
            }

            if (!string.IsNullOrWhiteSpace(AnotherKey))
            {
                sb.AppendLine(" and (ca.CardNo LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.CardNumber LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or cus.Address LIKE N'%" + AnotherKey.Trim() + "%'");
                sb.AppendLine(" or cus.AddressUnsign LIKE N'%" + AnotherKey.Trim().Replace(",", "").Replace(".", "").ToLower() + "%'");
                sb.AppendLine(" or ca.Plate1 LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.Plate2 LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.Plate3 LIKE N'%" + AnotherKey + "%')");
            }

            if (!string.IsNullOrWhiteSpace(CardGroupIDs))
            {
                sb.AppendLine(" and ca.CardGroupID = '" + CardGroupIDs + "'");
            }
            if (!string.IsNullOrWhiteSpace(CustomerID))
            {
                sb.AppendLine(" and ca.CustomerID = '" + CustomerID + "'");
            }
            //if (!string.IsNullOrWhiteSpace(CustomerGroupID))
            //{
            //    sb.AppendLine(" and cus.CustomerGroupID IN ('" + CustomerGroupID + "')");
            //}
            if (!string.IsNullOrWhiteSpace(CustomerGroupID))
            {
                var t = CustomerGroupID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    sb.AppendLine("and cus.CustomerGroupID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        sb.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    sb.AppendLine(" )");
                }
            }

            //Update lai bang tblCard
            sb.AppendLine("UPDATE");
            sb.AppendLine("ca");
            sb.AppendLine(string.Format("SET ca.ExpireDate = '{0}'", _newexpire));

            if (chbEnableDateActive)
            {
                sb.AppendLine(string.Format(", ca.DateActive = '{0}'", dateactive));
            }

            sb.AppendLine("FROM");
            sb.AppendLine("tblCard AS ca");
            sb.AppendLine("LEFT JOIN dbo.tblCardGroup AS g ON ca.CardGroupID = g.CardGroupID");
            sb.AppendLine("LEFT join tblCustomer c on ca.CustomerID = CONVERT(varchar(255), c.CustomerID)");
            sb.AppendLine("WHERE IsDelete = 0 and ca.IsLock=0 and g.CardType = 0");

            if (chbEnableMinusActive == false)
            {
                sb.AppendLine(string.Format("and DATEDIFF(DAY, ca.[ExpireDate], '{0}') >=0  AND ca.[ExpireDate] <= '{0}'", _newexpire));
            }

            //Update theo filler
            if (!string.IsNullOrWhiteSpace(KeyWord))
            {
                sb.AppendLine(" and (ca.CardNo LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.CardNumber LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or c.Address LIKE N'%" + KeyWord.Trim() + "%'");
                sb.AppendLine(" or c.AddressUnsign LIKE N'%" + KeyWord.Trim().Replace(",", "").Replace(".", "").ToLower() + "%'");
                sb.AppendLine(" or ca.Plate1 LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.Plate2 LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.Plate3 LIKE N'%" + KeyWord + "%')");
            }

            if (!string.IsNullOrEmpty(strIDCards))
            {
                var t = strIDCards.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    sb.AppendLine("and ca.CardID NOT IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        sb.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    sb.AppendLine(" )");
                }
            }

            if (!string.IsNullOrWhiteSpace(AnotherKey))
            {
                sb.AppendLine(" and (ca.CardNo LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.CardNumber LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or c.Address LIKE N'%" + AnotherKey.Trim() + "%'");
                sb.AppendLine(" or c.AddressUnsign LIKE N'%" + AnotherKey.Trim().Replace(",", "").Replace(".", "").ToLower() + "%'");
                sb.AppendLine(" or ca.Plate1 LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.Plate2 LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.Plate3 LIKE N'%" + AnotherKey + "%')");
            }

            if (!string.IsNullOrWhiteSpace(CardGroupIDs))
            {
                sb.AppendLine(" and ca.CardGroupID = '" + CardGroupIDs + "'");
            }

            if (!string.IsNullOrWhiteSpace(CustomerID))
            {
                sb.AppendLine(" and ca.CustomerID = '" + CustomerID + "'");
            }

            //if (!string.IsNullOrWhiteSpace(CustomerGroupID))
            //{
            //    sb.AppendLine(" and c.CustomerGroupID IN ('" + CustomerGroupID + "')");
            //}
            if (!string.IsNullOrWhiteSpace(CustomerGroupID))
            {
                var t = CustomerGroupID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    sb.AppendLine("and c.CustomerGroupID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        sb.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    sb.AppendLine(" )");
                }
            }

            if (chbEnableDateActive)
            {
                sb.AppendLine("insert into tblCardProcess(Date, Actions, CardGroupID, UserID, CustomerID, CardNumber)");
                sb.AppendLine(string.Format("SELECT GETDATE(),'ACTIVE', ca.CardGroupID,  '{0}', CASE WHEN ca.CustomerID IS NULL THEN '0' ELSE ca.CustomerID END , ca.Cardnumber", userId));

                sb.AppendLine("from tblCard ca");

                sb.AppendLine("LEFT JOIN dbo.tblCardGroup g ON ca.CardGroupID=g.CardGroupID");
                sb.AppendLine("LEFT join tblCustomer cus on ca.CustomerID = CONVERT(varchar(255), cus.CustomerID)");

                sb.AppendLine("where ca.IsDelete = 0 and ca.IsLock=0 and g.CardType=0");

                //Neu so ngay gia han <0 va neu ko check thi ko cho gia han
                if (chbEnableMinusActive == false)
                {
                    sb.AppendLine(string.Format("and DATEDIFF(DAY, ca.[ExpireDate], '{0}') >=0  AND ca.[ExpireDate] <= '{0}'", _newexpire));
                }

                //Update theo filler             
                if (!string.IsNullOrWhiteSpace(KeyWord))
                {
                    sb.AppendLine(" and (ca.CardNo LIKE N'%" + KeyWord + "%'");
                    sb.AppendLine(" or ca.CardNumber LIKE N'%" + KeyWord + "%'");
                    sb.AppendLine(" or cus.Address LIKE N'%" + KeyWord.Trim() + "%'");
                    sb.AppendLine(" or cus.AddressUnsign LIKE N'%" + KeyWord.Trim().Replace(",", "").Replace(".", "").ToLower() + "%'");
                    sb.AppendLine(" or ca.Plate1 LIKE N'%" + KeyWord + "%'");
                    sb.AppendLine(" or ca.Plate2 LIKE N'%" + KeyWord + "%'");
                    sb.AppendLine(" or ca.Plate3 LIKE N'%" + KeyWord + "%')");
                }

                if (!string.IsNullOrWhiteSpace(AnotherKey))
                {
                    sb.AppendLine(" and (ca.CardNo LIKE N'%" + AnotherKey + "%'");
                    sb.AppendLine(" or ca.CardNumber LIKE N'%" + AnotherKey + "%'");
                    sb.AppendLine(" or cus.Address LIKE N'%" + AnotherKey.Trim() + "%'");
                    sb.AppendLine(" or cus.AddressUnsign LIKE N'%" + AnotherKey.Trim().Replace(",", "").Replace(".", "").ToLower() + "%'");
                    sb.AppendLine(" or ca.Plate1 LIKE N'%" + AnotherKey + "%'");
                    sb.AppendLine(" or ca.Plate2 LIKE N'%" + AnotherKey + "%'");
                    sb.AppendLine(" or ca.Plate3 LIKE N'%" + AnotherKey + "%')");
                }

                if (!string.IsNullOrWhiteSpace(CardGroupIDs))
                {
                    sb.AppendLine(" and ca.CardGroupID = '" + CardGroupIDs + "'");
                }

                if (!string.IsNullOrWhiteSpace(CustomerID))
                {
                    sb.AppendLine(" and ca.CustomerID = '" + CustomerID + "'");
                }

                //if (!string.IsNullOrWhiteSpace(CustomerGroupID))
                //{
                //    sb.AppendLine(" and cus.CustomerGroupID IN ('" + CustomerGroupID + " ')");
                //}

                if (!string.IsNullOrWhiteSpace(CustomerGroupID))
                {
                    var t = CustomerGroupID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (t.Any())
                    {
                        var count = 0;

                        sb.AppendLine("and cus.CustomerGroupID IN ( ");

                        foreach (var item in t)
                        {
                            count++;

                            sb.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                        }

                        sb.AppendLine(" )");
                    }
                }
            }

            return ExcuteSQL.Execute(sb.ToString());
        }

        //dùng cho trường chinh
        public bool AddCardExpireTRANSERCO(string KeyWord, string fromdate, string todate, string AnotherKey, string CardGroupIDs, string CustomerID, string CustomerGroupID, int _feelevel, string _newexpire, string userId, bool chbEnableMinusActive, bool istransfer, bool chbEnableDateActive = false, string dateactive = "")
        {
            var transfer = istransfer ? "True" : "False";



            var sb = new StringBuilder();
            sb.AppendLine("INSERT INTO tblActiveCard(Code, [Date], CardNumber, CardNo, Plate, OldExpireDate, [Days], NewExpireDate, CardGroupID, CustomerGroupID, UserID, FeeLevel, CustomerID,IsDelete,ContractCode,IsTransferPayment,ContractStartDate,ContractEndDate)");

            sb.AppendLine("SELECT CASE WHEN cus.CustomerCode IS NULL THEN '0' ELSE cus.CustomerCode END,GETDATE(), ca.Cardnumber,ca.CardNo");
            sb.AppendLine(", CAST(CASE WHEN ca.Plate2 <> '' THEN ca.Plate1 + ';' + ca.Plate2 WHEN ca.Plate3 <> '' THEN ca.Plate1 + ';' + ca.Plate2 + ';' + ca.Plate3 WHEN ca.Plate1 IS NULL THEN '' ELSE ca.Plate1 END AS nvarchar(50)) as Plate");
            sb.AppendLine(string.Format(", ca.[ExpireDate], DATEDIFF(DAY, ca.[ExpireDate], '{0}')", _newexpire));
            sb.AppendLine(string.Format(", '{0}', ca.CardGroupID, CASE WHEN  cus.CustomerGroupID IS NULL THEN '0' ELSE cus.CustomerGroupID END,'{2}','{1}', CASE WHEN ca.CustomerID IS NULL THEN '0' ELSE ca.CustomerID END,0,ISNULL(cus.Description,''),'{3}',cus.ContractStartDate,cus.ContractEndDate", _newexpire, _feelevel, userId, transfer));

            sb.AppendLine("from tblCard ca");

            sb.AppendLine("LEFT JOIN dbo.tblCardGroup g ON ca.CardGroupID=g.CardGroupID");
            sb.AppendLine("LEFT join tblCustomer cus on ca.CustomerID = CONVERT(varchar(255), cus.CustomerID)");

            sb.AppendLine("where ca.IsDelete = 0 and ca.IsLock=0 and g.CardType=0");

            //Neu so ngay gia han <0 va neu ko check thi ko cho gia han
            if (chbEnableMinusActive == false)
            {
                sb.AppendLine(string.Format("and DATEDIFF(DAY, ca.[ExpireDate], '{0}') >=0  AND ca.[ExpireDate] <= '{0}'", _newexpire));
            }

            //Update theo filler
            if (!string.IsNullOrWhiteSpace(KeyWord))
            {
                sb.AppendLine(" and (ca.CardNo LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.CardNumber LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.Plate1 LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.Plate2 LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.Plate3 LIKE N'%" + KeyWord + "%')");
            }

            if(!string.IsNullOrEmpty(fromdate) && !string.IsNullOrEmpty(todate))
            {
                fromdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd");
                todate = Convert.ToDateTime(todate).ToString("yyyy/MM/dd");
                sb.AppendLine(string.Format(" and ca.ExpireDate >= '{0}' and ca.ExpireDate <= '{1}'", fromdate, todate));
            }
               

            if (!string.IsNullOrWhiteSpace(AnotherKey))
            {
                sb.AppendLine(" and (ca.CardNo LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.CardNumber LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.Plate1 LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.Plate2 LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.Plate3 LIKE N'%" + KeyWord + "%')");
            }

            if (!string.IsNullOrWhiteSpace(CardGroupIDs))
            {
                sb.AppendLine(" and ca.CardGroupID = '" + CardGroupIDs + "'");
            }
            if (!string.IsNullOrWhiteSpace(CustomerID))
            {
                sb.AppendLine(" and ca.CustomerID = '" + CustomerID + "'");
            }
            if (!string.IsNullOrWhiteSpace(CustomerGroupID))
            {
                sb.AppendLine(" and cus.CustomerGroupID IN ('" + CustomerGroupID + "')");
            }

            //Update lai bang tblCard
            sb.AppendLine("UPDATE");
            sb.AppendLine("ca");
            sb.AppendLine(string.Format("SET ca.ExpireDate = '{0}'", _newexpire));

            if (chbEnableDateActive)
            {
                sb.AppendLine(string.Format(", ca.DateActive = '{0}'", dateactive));
            }

            sb.AppendLine("FROM");
            sb.AppendLine("tblCard AS ca");
            sb.AppendLine("LEFT JOIN dbo.tblCardGroup AS g ON ca.CardGroupID = g.CardGroupID");
            sb.AppendLine("LEFT join tblCustomer c on ca.CustomerID = CONVERT(varchar(255), c.CustomerID)");
            sb.AppendLine("WHERE IsDelete = 0 and ca.IsLock=0 and g.CardType = 0");

            if (chbEnableMinusActive == false)
            {
                sb.AppendLine(string.Format("and DATEDIFF(DAY, ca.[ExpireDate], '{0}') >=0  AND ca.[ExpireDate] <= '{0}'", _newexpire));
            }

            //Update theo filler
            if (!string.IsNullOrWhiteSpace(KeyWord))
            {
                sb.AppendLine(" and (ca.CardNo LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.CardNumber LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.Plate1 LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.Plate2 LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.Plate3 LIKE N'%" + KeyWord + "%')");
            }

            if (!string.IsNullOrEmpty(fromdate) && !string.IsNullOrEmpty(todate))
            {
                fromdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd");
                todate = Convert.ToDateTime(todate).ToString("yyyy/MM/dd");
                sb.AppendLine(string.Format(" and ca.ExpireDate >= '{0}' and ca.ExpireDate <= '{1}'", fromdate, todate));
            }

            if (!string.IsNullOrWhiteSpace(AnotherKey))
            {
                sb.AppendLine(" and (ca.CardNo LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.CardNumber LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.Plate1 LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.Plate2 LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.Plate3 LIKE N'%" + AnotherKey + "%')");
            }

            if (!string.IsNullOrWhiteSpace(CardGroupIDs))
            {
                sb.AppendLine(" and ca.CardGroupID = '" + CardGroupIDs + "'");
            }

            if (!string.IsNullOrWhiteSpace(CustomerID))
            {
                sb.AppendLine(" and ca.CustomerID = '" + CustomerID + "'");
            }

            if (!string.IsNullOrWhiteSpace(CustomerGroupID))
            {
                sb.AppendLine(" and c.CustomerGroupID IN ('" + CustomerGroupID + "')");
            }

            if (chbEnableDateActive)
            {
                sb.AppendLine("insert into tblCardProcess(Date, Actions, CardGroupID, UserID, CustomerID, CardNumber,Description)");
                sb.AppendLine(string.Format("SELECT GETDATE(),'ACTIVE', ca.CardGroupID,  '{0}', CASE WHEN ca.CustomerID IS NULL THEN '0' ELSE ca.CustomerID END , ca.Cardnumber,cus.Description", userId));

                sb.AppendLine("from tblCard ca");

                sb.AppendLine("LEFT JOIN dbo.tblCardGroup g ON ca.CardGroupID=g.CardGroupID");
                sb.AppendLine("LEFT join tblCustomer cus on ca.CustomerID = CONVERT(varchar(255), cus.CustomerID)");

                sb.AppendLine("where ca.IsDelete = 0 and ca.IsLock=0 and g.CardType=0");

                //Neu so ngay gia han <0 va neu ko check thi ko cho gia han
                if (chbEnableMinusActive == false)
                {
                    sb.AppendLine(string.Format("and DATEDIFF(DAY, ca.[ExpireDate], '{0}') >=0  AND ca.[ExpireDate] <= '{0}'", _newexpire));
                }

                //Update theo filler
                if (!string.IsNullOrWhiteSpace(KeyWord))
                {
                    sb.AppendLine(" and (ca.CardNo LIKE N'%" + KeyWord + "%'");
                    sb.AppendLine(" or ca.CardNumber LIKE N'%" + KeyWord + "%'");
                    sb.AppendLine(" or ca.Plate1 LIKE N'%" + KeyWord + "%'");
                    sb.AppendLine(" or ca.Plate2 LIKE N'%" + KeyWord + "%'");
                    sb.AppendLine(" or ca.Plate3 LIKE N'%" + KeyWord + "%')");
                }

                if (!string.IsNullOrWhiteSpace(AnotherKey))
                {
                    sb.AppendLine(" and (ca.CardNo LIKE N'%" + AnotherKey + "%'");
                    sb.AppendLine(" or ca.CardNumber LIKE N'%" + AnotherKey + "%'");
                    sb.AppendLine(" or ca.Plate1 LIKE N'%" + AnotherKey + "%'");
                    sb.AppendLine(" or ca.Plate2 LIKE N'%" + AnotherKey + "%'");
                    sb.AppendLine(" or ca.Plate3 LIKE N'%" + AnotherKey + "%')");
                }

                if (!string.IsNullOrWhiteSpace(CardGroupIDs))
                {
                    sb.AppendLine(" and ca.CardGroupID = '" + CardGroupIDs + "'");
                }

                if (!string.IsNullOrWhiteSpace(CustomerID))
                {
                    sb.AppendLine(" and ca.CustomerID = '" + CustomerID + "'");
                }

                if (!string.IsNullOrWhiteSpace(CustomerGroupID))
                {
                    sb.AppendLine(" and cus.CustomerGroupID IN ('" + CustomerGroupID + " ')");
                }
            }

            return ExcuteSQL.Execute(sb.ToString());
        }

        public bool AddCardExpireByListCardNumber(string listCardNumber, int _feelevel, string _newexpire, string userId, bool chbEnableMinusActive, bool chbEnableDateActive = false, string dateactive = "")
        {
            var sb = new StringBuilder();
            sb.AppendLine("INSERT INTO tblActiveCard(Code, [Date], CardNumber, CardNo, Plate, OldExpireDate, [Days], NewExpireDate, CardGroupID, CustomerGroupID, UserID, FeeLevel, CustomerID,IsDelete)");
            sb.AppendLine("SELECT CASE WHEN cus.CustomerCode IS NULL THEN '0' ELSE cus.CustomerCode END,GETDATE(), ca.Cardnumber,ca.CardNo");
            sb.AppendLine(", CAST(CASE WHEN ca.Plate2 <> '' THEN ISNULL(ca.Plate1,'') + ';' + ISNULL(ca.Plate2,'') WHEN ca.Plate3 <> '' THEN ISNULL(ca.Plate1,'') + ';' + ISNULL(ca.Plate2,'') + ';' + ISNULL(ca.Plate3,'') WHEN ca.Plate1 IS NULL THEN '' ELSE ca.Plate1 END AS nvarchar(50)) as Plate");
            sb.AppendLine(string.Format(", ca.[ExpireDate], DATEDIFF(DAY, ca.[ExpireDate], '{0}')", _newexpire));
            sb.AppendLine(string.Format(", '{0}', ca.CardGroupID, CASE WHEN  cus.CustomerGroupID IS NULL THEN '0' ELSE cus.CustomerGroupID END,'{2}','{1}', CASE WHEN ca.CustomerID IS NULL THEN '0' ELSE ca.CustomerID END,0", _newexpire, _feelevel, userId));
            sb.AppendLine("from tblCard ca");
            sb.AppendLine("LEFT join tblCustomer cus on ca.CustomerID = CONVERT(varchar(255), cus.CustomerID)");
            sb.AppendLine("where ca.IsDelete = 0 and ca.IsLock=0");
            //Neu so ngay gia han <0 va neu ko check thi ko cho gia han
            if (chbEnableMinusActive == false)
            {
                sb.AppendLine(string.Format("and DATEDIFF(DAY, ca.[ExpireDate], '{0}') >=0  AND ca.[ExpireDate] <= '{0}'", _newexpire));
            }

            if (!string.IsNullOrWhiteSpace(listCardNumber))
            {
                //where in
                sb.AppendLine(string.Format(" and ca.CardNumber IN ({0})", listCardNumber));
            }

            //Update lai bang tblCard
            sb.AppendLine("UPDATE");
            sb.AppendLine("ca");
            sb.AppendLine(string.Format("SET ca.ExpireDate = '{0}'", _newexpire));

            if (chbEnableDateActive)
            {
                sb.AppendLine(string.Format(", ca.DateActive = '{0}'", dateactive));
            }

            sb.AppendLine("FROM");
            sb.AppendLine("tblCard AS ca");
            sb.AppendLine("LEFT join tblCustomer c on ca.CustomerID = CONVERT(varchar(255), c.CustomerID)");
            sb.AppendLine("WHERE IsDelete = 0 and ca.IsLock=0 ");

            if (chbEnableMinusActive == false)
            {
                sb.AppendLine(string.Format("and DATEDIFF(DAY, ca.[ExpireDate], '{0}') >=0  AND ca.[ExpireDate] <= '{0}'", _newexpire));
            }
            if (!string.IsNullOrWhiteSpace(listCardNumber))
            {
                //where in
                sb.AppendLine(string.Format(" and ca.CardNumber IN ({0})", listCardNumber));
            }

            if (chbEnableDateActive)
            {
                sb.AppendLine("insert into tblCardProcess(Date, Actions, CardGroupID, UserID, CustomerID, CardNumber)");
                sb.AppendLine(string.Format("SELECT GETDATE(),'ACTIVE', ca.CardGroupID,  '{0}', CASE WHEN ca.CustomerID IS NULL THEN '0' ELSE ca.CustomerID END , ca.Cardnumber", userId));
                sb.AppendLine("from tblCard ca");
                sb.AppendLine("LEFT join tblCustomer cus on ca.CustomerID = CONVERT(varchar(255), cus.CustomerID)");
                sb.AppendLine("where ca.IsDelete = 0 and ca.IsLock=0");
                //Neu so ngay gia han <0 va neu ko check thi ko cho gia han
                if (chbEnableMinusActive == false)
                {
                    sb.AppendLine(string.Format("and DATEDIFF(DAY, ca.[ExpireDate], '{0}') >=0  AND ca.[ExpireDate] <= '{0}'", _newexpire));
                }

                if (!string.IsNullOrWhiteSpace(listCardNumber))
                {
                    //where in
                    sb.AppendLine(string.Format(" and ca.CardNumber IN ({0})", listCardNumber));
                }
            }

            return ExcuteSQL.Execute(sb.ToString());
        }

        public bool AddCardExpireByListCardNumberOneMonth(string listCardNumber, int _feelevel, string _newexpire, string userId, bool chbEnableMinusActive, bool chbEnableDateActive = false, string dateactive = "")
        {
            var sb = new StringBuilder();
            sb.AppendLine("INSERT INTO tblActiveCard(Code, [Date], CardNumber, CardNo, Plate, OldExpireDate, [Days], NewExpireDate, CardGroupID, CustomerGroupID, UserID, FeeLevel, CustomerID,IsDelete)");
            sb.AppendLine("SELECT CASE WHEN cus.CustomerCode IS NULL THEN '0' ELSE cus.CustomerCode END,GETDATE(), ca.Cardnumber,ca.CardNo");
            sb.AppendLine(", CAST(CASE WHEN ca.Plate2 <> '' THEN ISNULL(ca.Plate1,'') + ';' + ISNULL(ca.Plate2,'') WHEN ca.Plate3 <> '' THEN ISNULL(ca.Plate1,'') + ';' + ISNULL(ca.Plate2,'') + ';' + ISNULL(ca.Plate3,'') WHEN ca.Plate1 IS NULL THEN '' ELSE ca.Plate1 END AS nvarchar(50)) as Plate");
            sb.AppendLine(string.Format(", ca.[ExpireDate], DATEDIFF(DAY, ca.[ExpireDate], DATEADD(MONTH, 1, ca.ExpireDate))"));
            sb.AppendLine(string.Format(", DATEADD(MONTH, 1, ca.ExpireDate), ca.CardGroupID, CASE WHEN  cus.CustomerGroupID IS NULL THEN '0' ELSE cus.CustomerGroupID END,'{1}','{0}', CASE WHEN ca.CustomerID IS NULL THEN '0' ELSE ca.CustomerID END,0", _feelevel, userId));
            sb.AppendLine("from tblCard ca");
            sb.AppendLine("LEFT join tblCustomer cus on ca.CustomerID = CONVERT(varchar(255), cus.CustomerID)");
            sb.AppendLine("where ca.IsDelete = 0 and ca.IsLock=0");
            //Neu so ngay gia han <0 va neu ko check thi ko cho gia han
            //if (chbEnableMinusActive == false)
            //{
            //    sb.AppendLine(string.Format("and DATEDIFF(DAY, ca.[ExpireDate], '{0}') >=0  AND ca.[ExpireDate] <= '{0}'", _newexpire));
            //}

            if (!string.IsNullOrWhiteSpace(listCardNumber))
            {
                //where in
                sb.AppendLine(string.Format(" and ca.CardNumber IN ({0})", listCardNumber));
            }

            //Update lai bang tblCard
            sb.AppendLine("UPDATE");
            sb.AppendLine("ca");
            sb.AppendLine(string.Format("SET ca.ExpireDate = DATEADD(MONTH, 1, ca.ExpireDate)"));

            if (chbEnableDateActive)
            {
                sb.AppendLine(string.Format(", ca.DateActive = '{0}'", dateactive));
            }

            sb.AppendLine("FROM");
            sb.AppendLine("tblCard AS ca");
            sb.AppendLine("LEFT join tblCustomer c on ca.CustomerID = CONVERT(varchar(255), c.CustomerID)");
            sb.AppendLine("WHERE IsDelete = 0 and ca.IsLock=0 ");

            //if (chbEnableMinusActive == false)
            //{
            //    sb.AppendLine(string.Format("and DATEDIFF(DAY, ca.[ExpireDate], '{0}') >=0  AND ca.[ExpireDate] <= '{0}'", _newexpire));
            //}
            if (!string.IsNullOrWhiteSpace(listCardNumber))
            {
                //where in
                sb.AppendLine(string.Format(" and ca.CardNumber IN ({0})", listCardNumber));
            }

            if (chbEnableDateActive)
            {
                sb.AppendLine("insert into tblCardProcess(Date, Actions, CardGroupID, UserID, CustomerID, CardNumber)");
                sb.AppendLine(string.Format("SELECT GETDATE(),'ACTIVE', ca.CardGroupID,  '{0}', CASE WHEN ca.CustomerID IS NULL THEN '0' ELSE ca.CustomerID END , ca.Cardnumber", userId));
                sb.AppendLine("from tblCard ca");
                sb.AppendLine("LEFT join tblCustomer cus on ca.CustomerID = CONVERT(varchar(255), cus.CustomerID)");
                sb.AppendLine("where ca.IsDelete = 0 and ca.IsLock=0");
                //Neu so ngay gia han <0 va neu ko check thi ko cho gia han
                //if (chbEnableMinusActive == false)
                //{
                //    sb.AppendLine(string.Format("and DATEDIFF(DAY, ca.[ExpireDate], '{0}') >=0  AND ca.[ExpireDate] <= '{0}'", _newexpire));
                //}

                if (!string.IsNullOrWhiteSpace(listCardNumber))
                {
                    //where in
                    sb.AppendLine(string.Format(" and ca.CardNumber IN ({0})", listCardNumber));
                }
            }

            return ExcuteSQL.Execute(sb.ToString());
        }

        public bool AQUA_AddCardExpireByListCardNumber(string listCardNumber, int _feelevel, string _newexpire, string userId, bool chbEnableMinusActive, bool chbEnableDateActive = false, string dateactive = "")
        {
            var sb = new StringBuilder();
            sb.AppendLine("INSERT INTO tblActiveCard(Code, [Date], CardNumber, CardNo, Plate, OldExpireDate, [Days], NewExpireDate, CardGroupID, CustomerGroupID, UserID, FeeLevel, CustomerID,IsDelete)");
            sb.AppendLine("SELECT CASE WHEN cus.CustomerCode IS NULL THEN '0' ELSE cus.CustomerCode END,GETDATE(), ca.Cardnumber,ca.CardNo");
            sb.AppendLine(", CAST(CASE WHEN ca.Plate2 <> '' THEN ISNULL(ca.Plate1,'') + ';' + ISNULL(ca.Plate2,'') WHEN ca.Plate3 <> '' THEN ISNULL(ca.Plate1,'') + ';' + ISNULL(ca.Plate2,'') + ';' + ISNULL(ca.Plate3,'') WHEN ca.Plate1 IS NULL THEN '' ELSE ca.Plate1 END AS nvarchar(50)) as Plate");
            sb.AppendLine(string.Format(", ca.[ExpireDate], DATEDIFF(DAY, ca.[ExpireDate], '{0}')", _newexpire));
            sb.AppendLine(string.Format(", '{0}', ca.CardGroupID, CASE WHEN  cus.CustomerGroupID IS NULL THEN '0' ELSE cus.CustomerGroupID END,'{1}',", _newexpire, userId));

            #region Tính tiền theo hạn mới và phí thuê bao
            sb.AppendLine("Convert(int,ISNULL((select (CASE WHEN Price > 0 then Price else 0 END) as Price from ( select (");
            sb.AppendLine("CASE");
            sb.AppendLine("WHEN a.Units LIKE N'%Ngày%' THEN (a.FeeLevel / CONVERT(int,(SELECT LEFT(a.Units, CHARINDEX('_', a.Units) -1)))) ");
            sb.AppendLine(string.Format("* CONVERT(int,DATEDIFF(DAY,ca.ExpireDate,CONVERT(datetime,'{0}')))", _newexpire));
            sb.AppendLine("WHEN a.Units LIKE N'%Tháng%' THEN (a.FeeLevel / (CONVERT(int,(SELECT LEFT(a.Units, CHARINDEX('_', a.Units) -1))) * 30))");
            sb.AppendLine(string.Format("* CONVERT(int,DATEDIFF(DAY,ca.ExpireDate,CONVERT(datetime,'{0}')))", _newexpire));
            sb.AppendLine("WHEN a.Units LIKE N'%Quý%' THEN (a.FeeLevel / (CONVERT(int,(SELECT LEFT(a.Units, CHARINDEX('_', a.Units) -1))) * 90))");
            sb.AppendLine(string.Format("* CONVERT(int,DATEDIFF(DAY,ca.ExpireDate,CONVERT(datetime,'{0}')))", _newexpire));
            sb.AppendLine("WHEN a.Units LIKE N'%Năm%' THEN (a.FeeLevel / (CONVERT(int,(SELECT LEFT(a.Units, CHARINDEX('_', a.Units) -1))) * 365))");
            sb.AppendLine(string.Format("* CONVERT(int,DATEDIFF(DAY,ca.ExpireDate,CONVERT(datetime,'{0}')))", _newexpire));
            sb.AppendLine("END ) as Price from (");
            sb.AppendLine("select FeeLevel,Units from tblFee");
            sb.AppendLine("where CardGroupID = ca.CardGroupID AND IsUseExtend = 'True'");
            sb.AppendLine(") as a ) as b),0) )");
            #endregion

            sb.AppendLine(string.Format(", CASE WHEN ca.CustomerID IS NULL THEN '0' ELSE ca.CustomerID END,0"));

            sb.AppendLine("from tblCard ca");
            sb.AppendLine("LEFT join tblCustomer cus on ca.CustomerID = CONVERT(varchar(255), cus.CustomerID)");
            sb.AppendLine("where ca.IsDelete = 0 and ca.IsLock=0");
            //Neu so ngay gia han <0 va neu ko check thi ko cho gia han
            if (chbEnableMinusActive == false)
            {
                sb.AppendLine(string.Format("and DATEDIFF(DAY, ca.[ExpireDate], '{0}') >=0  AND ca.[ExpireDate] <= '{0}'", _newexpire));
            }

            if (!string.IsNullOrWhiteSpace(listCardNumber))
            {
                //where in
                sb.AppendLine(string.Format(" and ca.CardNumber IN ({0})", listCardNumber));
            }

            //Update lai bang tblCard
            sb.AppendLine("UPDATE");
            sb.AppendLine("ca");
            sb.AppendLine(string.Format("SET ca.ExpireDate = '{0}'", _newexpire));

            if (chbEnableDateActive)
            {
                sb.AppendLine(string.Format(", ca.DateActive = '{0}'", dateactive));
            }

            sb.AppendLine("FROM");
            sb.AppendLine("tblCard AS ca");
            sb.AppendLine("LEFT join tblCustomer c on ca.CustomerID = CONVERT(varchar(255), c.CustomerID)");
            sb.AppendLine("WHERE IsDelete = 0 and ca.IsLock=0 ");

            if (chbEnableMinusActive == false)
            {
                sb.AppendLine(string.Format("and DATEDIFF(DAY, ca.[ExpireDate], '{0}') >=0  AND ca.[ExpireDate] <= '{0}'", _newexpire));
            }
            if (!string.IsNullOrWhiteSpace(listCardNumber))
            {
                //where in
                sb.AppendLine(string.Format(" and ca.CardNumber IN ({0})", listCardNumber));
            }

            if (chbEnableDateActive)
            {
                sb.AppendLine("insert into tblCardProcess(Date, Actions, CardGroupID, UserID, CustomerID, CardNumber)");
                sb.AppendLine(string.Format("SELECT GETDATE(),'ACTIVE', ca.CardGroupID,  '{0}', CASE WHEN ca.CustomerID IS NULL THEN '0' ELSE ca.CustomerID END , ca.Cardnumber", userId));
                sb.AppendLine("from tblCard ca");
                sb.AppendLine("LEFT join tblCustomer cus on ca.CustomerID = CONVERT(varchar(255), cus.CustomerID)");
                sb.AppendLine("where ca.IsDelete = 0 and ca.IsLock=0");
                //Neu so ngay gia han <0 va neu ko check thi ko cho gia han
                if (chbEnableMinusActive == false)
                {
                    sb.AppendLine(string.Format("and DATEDIFF(DAY, ca.[ExpireDate], '{0}') >=0  AND ca.[ExpireDate] <= '{0}'", _newexpire));
                }

                if (!string.IsNullOrWhiteSpace(listCardNumber))
                {
                    //where in
                    sb.AppendLine(string.Format(" and ca.CardNumber IN ({0})", listCardNumber));
                }
            }

            return ExcuteSQL.Execute(sb.ToString());
        }

        //Dùng cho trường chinh
        public bool AddCardExpireByListCardNumberTRANSERCO(string listCardNumber, int _feelevel, string _newexpire, string userId, bool chbEnableMinusActive, string arrCardID = "", bool chbEnableDateActive = false, string dateactive = "")
        {
            var sb = new StringBuilder();
            sb.AppendLine("INSERT INTO tblActiveCard(Code, [Date], CardNumber, CardNo, Plate, OldExpireDate, [Days], NewExpireDate, CardGroupID, CustomerGroupID, UserID, FeeLevel, CustomerID,IsDelete,ContractCode,IsTransferPayment,ContractStartDate,ContractEndDate)");
            sb.AppendLine("SELECT CASE WHEN cus.CustomerCode IS NULL THEN '0' ELSE cus.CustomerCode END,GETDATE(), ca.Cardnumber,ca.CardNo");
            sb.AppendLine(", CAST(CASE WHEN ca.Plate2 <> '' THEN ca.Plate1 + ';' + ca.Plate2 WHEN ca.Plate3 <> '' THEN ca.Plate1 + ';' + ca.Plate2 + ';' + ca.Plate3 WHEN ca.Plate1 IS NULL THEN '' ELSE ca.Plate1 END AS nvarchar(50)) as Plate");
            sb.AppendLine(string.Format(", ca.[ExpireDate], DATEDIFF(DAY, ca.[ExpireDate], '{0}')", _newexpire));
            sb.AppendLine(string.Format(", '{0}', ca.CardGroupID, CASE WHEN  cus.CustomerGroupID IS NULL THEN '0' ELSE cus.CustomerGroupID END,'{2}','{1}', CASE WHEN ca.CustomerID IS NULL THEN '0' ELSE ca.CustomerID END,0,ISNULL(cus.Description,'')", _newexpire, _feelevel, userId));

            if (!string.IsNullOrEmpty(arrCardID) && !arrCardID.Equals("("))
            {
                sb.AppendLine(", CASE");
                sb.AppendLine(string.Format("WHEN ca.CardID IN {0} THEN 'True'", arrCardID));
                sb.AppendLine(string.Format("WHEN ca.CardID NOT IN {0} THEN 'False'", arrCardID));
                sb.AppendLine("END");
            }
            else
            {
                sb.AppendLine(", 'False'");
            }
            sb.AppendLine(",cus.ContractStartDate,cus.ContractEndDate");
            sb.AppendLine("from tblCard ca");
            sb.AppendLine("LEFT join tblCustomer cus on ca.CustomerID = CONVERT(varchar(255), cus.CustomerID)");
            sb.AppendLine("where ca.IsDelete = 0 and ca.IsLock=0");
            //Neu so ngay gia han <0 va neu ko check thi ko cho gia han
            if (chbEnableMinusActive == false)
            {
                sb.AppendLine(string.Format("and DATEDIFF(DAY, ca.[ExpireDate], '{0}') >=0  AND ca.[ExpireDate] <= '{0}'", _newexpire));
            }

            if (!string.IsNullOrWhiteSpace(listCardNumber))
            {
                //where in
                sb.AppendLine(string.Format(" and ca.CardNumber IN ({0})", listCardNumber));
            }

            //Update lai bang tblCard
            sb.AppendLine("UPDATE");
            sb.AppendLine("ca");
            sb.AppendLine(string.Format("SET ca.ExpireDate = '{0}'", _newexpire));

            if (chbEnableDateActive)
            {
                sb.AppendLine(string.Format(", ca.DateActive = '{0}'", dateactive));
            }

            sb.AppendLine("FROM");
            sb.AppendLine("tblCard AS ca");
            sb.AppendLine("LEFT join tblCustomer c on ca.CustomerID = CONVERT(varchar(255), c.CustomerID)");
            sb.AppendLine("WHERE IsDelete = 0 and ca.IsLock=0 ");

            if (chbEnableMinusActive == false)
            {
                sb.AppendLine(string.Format("and DATEDIFF(DAY, ca.[ExpireDate], '{0}') >=0  AND ca.[ExpireDate] <= '{0}'", _newexpire));
            }
            if (!string.IsNullOrWhiteSpace(listCardNumber))
            {
                //where in
                sb.AppendLine(string.Format(" and ca.CardNumber IN ({0})", listCardNumber));
            }

            if (chbEnableDateActive)
            {
                sb.AppendLine("insert into tblCardProcess(Date, Actions, CardGroupID, UserID, CustomerID, CardNumber,Description)");
                sb.AppendLine(string.Format("SELECT GETDATE(),'ACTIVE', ca.CardGroupID,  '{0}', CASE WHEN ca.CustomerID IS NULL THEN '0' ELSE ca.CustomerID END , ca.Cardnumber,cus.Description", userId));
                sb.AppendLine("from tblCard ca");
                sb.AppendLine("LEFT join tblCustomer cus on ca.CustomerID = CONVERT(varchar(255), cus.CustomerID)");
                sb.AppendLine("where ca.IsDelete = 0 and ca.IsLock=0");
                //Neu so ngay gia han <0 va neu ko check thi ko cho gia han
                if (chbEnableMinusActive == false)
                {
                    sb.AppendLine(string.Format("and DATEDIFF(DAY, ca.[ExpireDate], '{0}') >=0  AND ca.[ExpireDate] <= '{0}'", _newexpire));
                }

                if (!string.IsNullOrWhiteSpace(listCardNumber))
                {
                    //where in
                    sb.AppendLine(string.Format(" and ca.CardNumber IN ({0})", listCardNumber));
                }
            }

            return ExcuteSQL.Execute(sb.ToString());
        }

        public MessageReport Create(tblCard obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblCardRepository.Add(obj);

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
                var obj = GetById(Guid.Parse(id));
                if (obj != null)
                {
                    obj.IsDelete = true;

                    _tblCardRepository.Update(obj);

                    Save();
                }

                re.Message = FunctionHelper.GetLocalizeDictionary("Home", "notification")["DeleteSuccess"];
                re.isSuccess = true;
            }
            catch (Exception ex)
            {
                re.Message = ex.Message;
                re.isSuccess = false;
            }

            return re;
        }

        public IPagedList<tblCardCustomViewModel> GetAllPagingByFirst(string key, string cardgroups, string customerid, string customergroups, string fromdate, string todate, string ischeckbytime = "0", int pageNumber = 1, int pageSize = 20, string accesslevelids = "", string active = "")
        {
            var query = (from card in _tblCardRepository.Table
                         join cardgroup in _tblCardGroupRepository.Table on card.CardGroupID equals cardgroup.CardGroupID.ToString() into card_cardgroup
                         from cardgroup in card_cardgroup.DefaultIfEmpty()
                         join customer in _tblCustomerRepository.Table on card.CustomerID equals customer.CustomerID.ToString() into card_customer
                         from customer in card_customer.DefaultIfEmpty()
                         join customergroup in _tblCustomerGroupRepository.Table on customer.CustomerGroupID equals customergroup.CustomerGroupID.ToString() into customer_group
                         from customergroup in customer_group.DefaultIfEmpty()

                         where /*cardgroup.CardType == 0 &&*/ card.IsDelete == false /*&& card.IsLock == false*/

                         select new tblCardCustomViewModel()
                         {
                             CardID = card.CardID.ToString(),
                             CardNo = card.CardNo,
                             CardNumber = card.CardNumber,
                             CardGroupId = cardgroup.CardGroupID.ToString(),
                             CardGroupName = cardgroup.CardGroupName,
                             ExpireDate = card.ExpireDate,
                             ImportDate = card.ImportDate,
                             Plate1 = card.Plate1,
                             Plate2 = card.Plate2,
                             Plate3 = card.Plate3,
                             VehicleName1 = card.VehicleName1,
                             VehicleName2 = card.VehicleName2,
                             VehicleName3 = card.VehicleName3,
                             IsLock = card.IsLock,

                             CustomerId = customer.CustomerID.ToString(),
                             CustomerCode = customer.CustomerCode,
                             CustomerMobile = customer.Mobile,
                             CustomerAddress = customer.Address,
                             CustomerIDNumber = customer.IDNumber,
                             Description = customer.Description,
                             CustomerName = customer.CustomerName,
                             CustomerGroupId = customergroup.CustomerGroupID.ToString(),
                             CustomerGroupName = customergroup.CustomerGroupName,

                             AccessExpireDate = card.AccessExpireDate,
                             AccessLevelID = card.AccessLevelID
                         });

            //Từ khóa
            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.CardNo.Contains(key) || n.CardNumber.Contains(key) || n.Plate1.Contains(key) || n.Plate2.Contains(key) || n.Plate3.Contains(key));
            }

            //Nhóm thẻ
            if (!string.IsNullOrWhiteSpace(cardgroups))
            {
                query = query.Where(n => cardgroups.Contains(n.CardGroupId));
            }

            //Nhóm khách hàng
            if (!string.IsNullOrWhiteSpace(customergroups))
            {
                query = query.Where(n => customergroups.Contains(n.CustomerGroupId));
            }

            //Id người dùng
            if (!string.IsNullOrWhiteSpace(customerid))
            {
                query = query.Where(n => n.CustomerId == customerid);
            }

            if (!string.IsNullOrWhiteSpace(accesslevelids))
            {
                query = query.Where(n => accesslevelids.Contains(n.AccessLevelID));
            }

            switch (ischeckbytime)
            {
                case "1"://Ngày nhập thẻ
                    if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
                    {
                        var fdate = Convert.ToDateTime(fromdate);
                        var tdate = Convert.ToDateTime(todate).AddDays(1);

                        query = query.Where(n => n.ImportDate >= fdate && n.ImportDate < tdate);
                    }
                    break;
                case "2"://Ngày hết hạn
                    if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
                    {
                        var fdate = Convert.ToDateTime(fromdate);
                        var tdate = Convert.ToDateTime(todate).AddDays(1);

                        query = query.Where(n => n.ExpireDate >= fdate && n.ExpireDate < tdate);
                    }
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrWhiteSpace(active))
            {
                switch (active)
                {
                    case "0":
                        query = query.Where(n => n.IsLock == false);
                        break;
                    case "1":
                        query = query.Where(n => n.IsLock == true);
                        break;
                    default:
                        break;
                }
            }

            var list = new PagedList<tblCardCustomViewModel>(query.OrderByDescending(n => n.ImportDate), pageNumber, pageSize);

            return list;
        }

        public IPagedList<tblCardCustomViewModel> GetAllPagingByFirstParking(string key, string cardgroups, string customerid, string customergroups, string fromdate, string todate, bool desc, string columnQuery, string ischeckbytime = "0", int pageNumber = 1, int pageSize = 20, string accesslevelids = "", string active = "")
        {
            var query = (from card in _tblCardRepository.Table
                         join cardgroup in _tblCardGroupRepository.Table on card.CardGroupID equals cardgroup.CardGroupID.ToString() into card_cardgroup
                         from cardgroup in card_cardgroup.DefaultIfEmpty()
                         join customer in _tblCustomerRepository.Table on card.CustomerID equals customer.CustomerID.ToString() into card_customer
                         from customer in card_customer.DefaultIfEmpty()
                         join customergroup in _tblCustomerGroupRepository.Table on customer.CustomerGroupID equals customergroup.CustomerGroupID.ToString() into customer_group
                         from customergroup in customer_group.DefaultIfEmpty()

                         where /*cardgroup.CardType == 0 &&*/ card.IsDelete == false /*&& card.IsLock == false*/

                         select new tblCardCustomViewModel()
                         {
                             CardID = card.CardID.ToString(),
                             CardNo = card.CardNo,
                             CardNumber = card.CardNumber,
                             CardGroupId = card.CardGroupID,
                             CardGroupName = "",
                             ExpireDate = card.ExpireDate,
                             ImportDate = card.ImportDate,
                             Plate1 = card.Plate1,
                             Plate2 = card.Plate2,
                             Plate3 = card.Plate3,
                             VehicleName1 = card.VehicleName1,
                             VehicleName2 = card.VehicleName2,
                             VehicleName3 = card.VehicleName3,
                             IsLock = card.IsLock,

                             CustomerId = card.CustomerID,
                             CustomerCode = customer.CustomerCode,
                             CustomerMobile = customer.Mobile,
                             CustomerAddress = customer.Address,
                             CustomerIDNumber = customer.IDNumber,
                             Description = customer.Description,
                             CustomerName = customer.CustomerName,
                             CustomerGroupId = customer.CustomerGroupID,
                             CustomerGroupName = customergroup.CustomerGroupName,

                             AccessExpireDate = card.AccessExpireDate,
                             AccessLevelID = card.AccessLevelID
                         });

            //Từ khóa
            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.CardNo.Contains(key) || n.CardNumber.Contains(key) || n.Plate1.Contains(key) || n.Plate2.Contains(key) || n.Plate3.Contains(key) || n.CustomerName.Contains(key) /*|| n.CustomerCode.Contains(key) || n.CustomerMobile.Contains(key) || n.CustomerAddress.Contains(key)*/);
            }

            //Nhóm thẻ
            if (!string.IsNullOrWhiteSpace(cardgroups))
            {
                query = query.Where(n => cardgroups.Contains(n.CardGroupId));
            }

            //Nhóm khách hàng
            if (!string.IsNullOrWhiteSpace(customergroups))
            {
                query = query.Where(n => customergroups.Contains(n.CustomerGroupId));
            }

            //Id người dùng
            if (!string.IsNullOrWhiteSpace(customerid))
            {
                query = query.Where(n => n.CustomerId == customerid);
            }

            if (!string.IsNullOrWhiteSpace(accesslevelids))
            {
                query = query.Where(n => accesslevelids.Contains(n.AccessLevelID));
            }

            switch (ischeckbytime)
            {
                case "1"://Ngày nhập thẻ
                    if (!string.IsNullOrWhiteSpace(fromdate))
                    {
                        var fdate = Convert.ToDateTime(fromdate);

                        query = query.Where(n => n.ImportDate >= fdate);
                    }

                    if (!string.IsNullOrWhiteSpace(todate))
                    {
                        var tdate = Convert.ToDateTime(todate).AddDays(1);

                        query = query.Where(n => n.ImportDate < tdate);
                    }
                    break;
                case "2"://Ngày hết hạn
                    if (!string.IsNullOrWhiteSpace(fromdate))
                    {
                        var fdate = Convert.ToDateTime(fromdate);

                        query = query.Where(n => n.ExpireDate >= fdate);
                    }

                    if (!string.IsNullOrWhiteSpace(todate))
                    {
                        var tdate = Convert.ToDateTime(todate).AddDays(1);

                        query = query.Where(n => n.ExpireDate < tdate);
                    }
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrWhiteSpace(active))
            {
                switch (active)
                {
                    case "0":
                        query = query.Where(n => n.IsLock == false);
                        break;
                    case "1":
                        query = query.Where(n => n.IsLock == true);
                        break;
                    default:
                        break;
                }
            }
            var list = new PagedList<tblCardCustomViewModel>(query.OrderByDescending(n => n.ImportDate), pageNumber, pageSize);

            if (columnQuery.Contains("CardNo"))
            {
                if (desc)
                {
                    list = new PagedList<tblCardCustomViewModel>(query.OrderByDescending(n => n.CardNo), pageNumber, pageSize);
                }
                else
                {
                    list = new PagedList<tblCardCustomViewModel>(query.OrderBy(n => n.CardNo), pageNumber, pageSize);
                }

            }
            if (columnQuery.Contains("CardNumber"))
            {
                if (desc)
                {
                    list = new PagedList<tblCardCustomViewModel>(query.OrderByDescending(n => n.CardNumber), pageNumber, pageSize);
                }
                else
                {
                    list = new PagedList<tblCardCustomViewModel>(query.OrderBy(n => n.CardNumber), pageNumber, pageSize);
                }
            }

            if (columnQuery.Contains("ImportDate"))
            {
                if (desc)
                {
                    list = new PagedList<tblCardCustomViewModel>(query.OrderByDescending(n => n.ImportDate), pageNumber, pageSize);
                }
                else
                {
                    list = new PagedList<tblCardCustomViewModel>(query.OrderBy(n => n.ImportDate), pageNumber, pageSize);
                }
            }

            if (columnQuery.Contains("IsLock"))
            {
                if (desc)
                {
                    list = new PagedList<tblCardCustomViewModel>(query.OrderByDescending(n => n.IsLock), pageNumber, pageSize);
                }
                else
                {
                    list = new PagedList<tblCardCustomViewModel>(query.OrderBy(n => n.IsLock), pageNumber, pageSize);
                }
            }

            return list;
        }

        public IEnumerable<tblCard> GetAllActiveByKey(string key)
        {
            var query = from n in _tblCardRepository.Table
                        where n.IsLock == false && (n.CardNo.Contains(key) || n.CardNumber.Contains(key))
                        orderby n.CardNo ascending
                        select n;

            return query;
        }

        public tblCard GetByCardNumber(string cardnumber)
        {
            var query = from n in _tblCardRepository.Table
                        where /*n.IsLock == false &&*/ n.CardNumber == cardnumber && n.IsDelete == false
                        //orderby n.CardNo ascending
                        select n;

            return query.FirstOrDefault();
        }

        public tblCardSubmit GetCustomById(Guid id)
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
                            DVT = n.DVT,
                            CardType = n.CardType.ToString(),

                            //Khách hàng
                            CustomerID = n.CustomerID,
                            CustomerAddress = m.Address,
                            CustomerAvatar = m.Avatar,
                            CustomerIdentify = m.IDNumber,
                            CustomerCode = m.CustomerCode,
                            CustomerGroupID = m.CustomerGroupID,
                            CustomerMobile = m.Mobile,
                            CustomerName = m.CustomerName,
                            CompartmentId = m.CompartmentId,
                            //Ngày tháng
                            //DtpDateExpired = Convert.ToDateTime(n.ExpireDate).ToString("dd/MM/yyyy"),
                            //DtpDateRegisted = Convert.ToDateTime(n.DateRegister).ToString("dd/MM/yyyy"),
                            //DtpDateReleased = Convert.ToDateTime(n.DateRelease).ToString("dd/MM/yyyy"),
                            //DtpDateActive = Convert.ToDateTime(n.DateActive).ToString("dd/MM/yyyy"),
                            //DtpDateActive = string.Format("{0:dd.MM.yy}", n.DateActive),

                            //Dữ liệu cũ dành cho process
                            OldCardInActive = n.IsLock,
                            OldCardNo = n.CardNo,
                            OldCardNumber = n.CardNumber,
                            OldCardDescription = n.Description,
                            OldCardGroupID = n.CardGroupID,
                            OldPlate1 =  n.Plate1,
                            OldPlate2 = n.Plate2,
                            OldPlate3 = n.Plate3,
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

        public tblCard GetByCardNumber_Id(string cardnumber, Guid id)
        {
            var query = from n in _tblCardRepository.Table
                        where n.IsLock == false && n.CardNumber == cardnumber && n.CardID != id && n.IsDelete == false
                        select n;

            return query.FirstOrDefault();
        }

        public IEnumerable<tblCard> GetAllByCustomerId(string customerid)
        {
            var query = from n in _tblCardRepository.Table
                        where n.CustomerID == customerid
                        select n;

            return query;
        }

        public List<tblCard> GetByCustomerId(string customerid)
        {
            var query = from n in _tblCardRepository.Table
                        where n.CustomerID == customerid
                        select n;

            return query.ToList();
        }

        public IEnumerable<tblCard> GetAll()
        {
            var query = from n in _tblCardRepository.Table
                        where n.IsDelete == false
                        select n;

            return query;
        }

        public IEnumerable<tblCard> GetCount(string fromdate, string todate)
        {
            var query = from n in _tblCardRepository.Table
                        where n.IsDelete == false
                        select n;

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                var fdate = Convert.ToDateTime(fromdate);
                var tdate = Convert.ToDateTime(todate).AddDays(1);

                query = query.Where(n => n.ExpireDate >= fdate && n.ExpireDate < tdate);
            }

            if (!string.IsNullOrEmpty(AuthCardGroupIds))
            {
                var lst = AuthCardGroupIds.Split(';');
                query = query.Where(n => lst.Contains(n.CardGroupID.ToString()));
            }

            return query;
        }

        public List<tblCardExcel> GetExcelCardByFirst(string key, string cardgroups, string customerid, string customergroups, string fromdate, string todate, string ischeckbytime = "0", string accesslevelids = "", string active = "")
        {
            var query = (from card in _tblCardRepository.Table
                         join cardgroup in _tblCardGroupRepository.Table on card.CardGroupID equals cardgroup.CardGroupID.ToString() into card_cardgroup
                         from cardgroup in card_cardgroup.DefaultIfEmpty()
                         join customer in _tblCustomerRepository.Table on card.CustomerID equals customer.CustomerID.ToString() into card_customer
                         from customer in card_customer.DefaultIfEmpty()
                         join customergroup in _tblCustomerGroupRepository.Table on customer.CustomerGroupID equals customergroup.CustomerGroupID.ToString() into customer_group
                         from customergroup in customer_group.DefaultIfEmpty()

                         where /*cardgroup.CardType == 0 &&*/ card.IsDelete == false /*&& card.IsLock == false*/

                         select new tblCardCustomViewModel()
                         {
                             CardID = card.CardID.ToString(),
                             CardNo = card.CardNo,
                             CardNumber = card.CardNumber,
                             CardGroupId = cardgroup.CardGroupID.ToString(),
                             CardGroupName = cardgroup.CardGroupName,
                             ExpireDate = card.ExpireDate,
                             ImportDate = card.ImportDate,
                             Plate1 = card.Plate1,
                             Plate2 = card.Plate2,
                             Plate3 = card.Plate3,
                             VehicleName1 = card.VehicleName1,
                             VehicleName2 = card.VehicleName2,
                             VehicleName3 = card.VehicleName3,
                             IsLock = card.IsLock,

                             CustomerId = customer.CustomerID.ToString(),
                             CustomerCode = customer.CustomerCode,
                             CustomerName = customer.CustomerName,
                             CustomerGroupId = customergroup.CustomerGroupID.ToString(),
                             CustomerGroupName = customergroup.CustomerGroupName,
                             CustomerAddress = customer.Address,
                             CustomerMobile = customer.Mobile,
                             CustomerIDNumber = customer.IDNumber
                         });

            //Từ khóa
            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.CardNo.Contains(key) || n.CardNumber.Contains(key) || n.Plate1.Contains(key) || n.Plate2.Contains(key) || n.Plate3.Contains(key));
            }

            //Nhóm thẻ
            if (!string.IsNullOrWhiteSpace(cardgroups))
            {
                query = query.Where(n => cardgroups.Contains(n.CardGroupId));
            }

            //Nhóm khách hàng
            if (!string.IsNullOrWhiteSpace(customergroups))
            {
                query = query.Where(n => customergroups.Contains(n.CustomerGroupId));
            }

            //Id người dùng
            if (!string.IsNullOrWhiteSpace(customerid))
            {
                query = query.Where(n => n.CustomerId == customerid);
            }

            if (!string.IsNullOrWhiteSpace(accesslevelids))
            {
                query = query.Where(n => accesslevelids.Contains(n.AccessLevelID));
            }

            switch (ischeckbytime)
            {
                case "1"://Ngày nhập thẻ
                    if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
                    {
                        var fdate = Convert.ToDateTime(fromdate);
                        var tdate = Convert.ToDateTime(todate).AddDays(1);

                        query = query.Where(n => n.ImportDate >= fdate && n.ImportDate < tdate);
                    }
                    break;
                case "2"://Ngày hết hạn
                    if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
                    {
                        var fdate = Convert.ToDateTime(fromdate);
                        var tdate = Convert.ToDateTime(todate).AddDays(1);

                        query = query.Where(n => n.ExpireDate >= fdate && n.ExpireDate < tdate);
                    }
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrWhiteSpace(active))
            {
                switch (active)
                {
                    case ("0"):
                        {
                            query = query.Where(n => n.IsLock == false);
                            break;
                        }
                    case ("1"):
                        {
                            query = query.Where(n => n.IsLock == true);
                            break;
                        }
                    default:
                        break;
                }
            }

            var list = new List<tblCardExcel>();

            if (query.Any())
            {
                var count = 0;
                foreach (var item in query)
                {
                    count++;
                    var o = new tblCardExcel()
                    {
                        Address = item.CustomerAddress,
                        CardGroupName = item.CardGroupName,
                        CardNo = item.CardNo,
                        CardNumber = item.CardNumber,
                        CMT = item.CustomerIDNumber,
                        CustomerCode = item.CustomerCode,
                        CustomerGroupName = item.CustomerGroupName,
                        CustomerName = item.CustomerName,
                        DateCreated = Convert.ToDateTime(item.ImportDate).ToString("dd/MM/yyyy"),
                        DateExpire = Convert.ToDateTime(item.ExpireDate).ToString("dd/MM/yyyy"),
                        Inactive = item.IsLock ? "Khóa" : "Hoạt động",
                        NumberRow = count,
                        Plates = item.Plate1 + ";" + item.Plate2 + ";" + item.Plate3,
                        SĐT = item.CustomerMobile
                    };

                    list.Add(o);
                }
            }

            return list;
        }

        public List<tblCardExcel> GetExcelCardByFirstParking(string key, string cardgroups, string customerid, string customergroups, string fromdate, string todate, bool desc, string columnQuery, string ischeckbytime = "0", string accesslevelids = "", string active = "")
        {
            var query = (from card in _tblCardRepository.Table
                         join cardgroup in _tblCardGroupRepository.Table on card.CardGroupID equals cardgroup.CardGroupID.ToString() into card_cardgroup
                         from cardgroup in card_cardgroup.DefaultIfEmpty()
                         join customer in _tblCustomerRepository.Table on card.CustomerID equals customer.CustomerID.ToString() into card_customer
                         from customer in card_customer.DefaultIfEmpty()
                         join customergroup in _tblCustomerGroupRepository.Table on customer.CustomerGroupID equals customergroup.CustomerGroupID.ToString() into customer_group
                         from customergroup in customer_group.DefaultIfEmpty()

                         where /*cardgroup.CardType == 0 &&*/ card.IsDelete == false /*&& card.IsLock == false*/

                         select new tblCardCustomViewModel()
                         {
                             CardID = card.CardID.ToString(),
                             CardNo = card.CardNo,
                             CardNumber = card.CardNumber,
                             CardGroupId = cardgroup.CardGroupID.ToString(),
                             CardGroupName = cardgroup.CardGroupName,
                             ExpireDate = card.ExpireDate,
                             ImportDate = card.ImportDate,
                             Plate1 = card.Plate1,
                             Plate2 = card.Plate2,
                             Plate3 = card.Plate3,
                             VehicleName1 = card.VehicleName1,
                             VehicleName2 = card.VehicleName2,
                             VehicleName3 = card.VehicleName3,
                             IsLock = card.IsLock,

                             CustomerId = customer.CustomerID.ToString(),
                             CustomerCode = customer.CustomerCode,
                             CustomerName = customer.CustomerName,
                             CustomerGroupId = customergroup.CustomerGroupID.ToString(),
                             CustomerGroupName = customergroup.CustomerGroupName,
                             CustomerAddress = customer.Address,
                             CustomerMobile = customer.Mobile,
                             CustomerIDNumber = customer.IDNumber,
                             Description = customer.Description
                         });

            //Từ khóa
            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.CardNo.Contains(key) || n.CardNumber.Contains(key) || n.Plate1.Contains(key) || n.Plate2.Contains(key) || n.Plate3.Contains(key) || n.CustomerName.Contains(key) || n.CustomerCode.Contains(key) || n.CustomerMobile.Contains(key) || n.CustomerAddress.Contains(key));
            }

            //Nhóm thẻ
            if (!string.IsNullOrWhiteSpace(cardgroups))
            {
                query = query.Where(n => cardgroups.Contains(n.CardGroupId));
            }

            //Nhóm khách hàng
            if (!string.IsNullOrWhiteSpace(customergroups))
            {
                query = query.Where(n => customergroups.Contains(n.CustomerGroupId));
            }

            //Id người dùng
            if (!string.IsNullOrWhiteSpace(customerid))
            {
                query = query.Where(n => n.CustomerId == customerid);
            }

            if (!string.IsNullOrWhiteSpace(accesslevelids))
            {
                query = query.Where(n => accesslevelids.Contains(n.AccessLevelID));
            }

            switch (ischeckbytime)
            {
                case "1"://Ngày nhập thẻ
                    if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
                    {
                        var fdate = Convert.ToDateTime(fromdate);
                        var tdate = Convert.ToDateTime(todate).AddDays(1);

                        query = query.Where(n => n.ImportDate >= fdate && n.ImportDate < tdate);
                    }
                    break;
                case "2"://Ngày hết hạn
                    if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
                    {
                        var fdate = Convert.ToDateTime(fromdate);
                        var tdate = Convert.ToDateTime(todate).AddDays(1);

                        query = query.Where(n => n.ExpireDate >= fdate && n.ExpireDate < tdate);
                    }
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrWhiteSpace(active))
            {
                switch (active)
                {
                    case ("0"):
                        {
                            query = query.Where(n => n.IsLock == false);
                            break;
                        }
                    case ("1"):
                        {
                            query = query.Where(n => n.IsLock == true);
                            break;
                        }
                    default:
                        break;
                }
            }

            if (columnQuery.Contains("CardNo"))
            {
                if (desc)
                {
                    query = query.OrderByDescending(n => n.CardNo);
                }
                else
                {
                    query = query.OrderBy(n => n.CardNo);
                }

            }
            if (columnQuery.Contains("CardNumber"))
            {
                if (desc)
                {
                    query = query.OrderByDescending(n => n.CardNumber);
                }
                else
                {
                    query = query.OrderBy(n => n.CardNumber);
                }
            }

            var list = new List<tblCardExcel>();

            if (query.Any())
            {
                var count = 0;
                foreach (var item in query)
                {
                    count++;
                    var o = new tblCardExcel()
                    {
                        Address = item.CustomerAddress,
                        CardGroupName = item.CardGroupName,
                        CardNo = item.CardNo,
                        CardNumber = item.CardNumber,
                        CMT = item.CustomerIDNumber,
                        CustomerCode = item.CustomerCode,
                        CustomerGroupName = item.CustomerGroupName,
                        CustomerName = item.CustomerName,
                        DateCreated = Convert.ToDateTime(item.ImportDate).ToString("dd/MM/yyyy"),
                        DateExpire = Convert.ToDateTime(item.ExpireDate).ToString("dd/MM/yyyy"),
                        Inactive = item.IsLock ? "Khóa" : "Hoạt động",
                        NumberRow = count,
                        Plates = item.Plate1 + ";" + item.Plate2 + ";" + item.Plate3,
                        SĐT = item.CustomerMobile,
                        //mã hợp đồng trường chinh
                        Description = item.Description
                    };

                    list.Add(o);
                }
            }

            return list;
        }

        public IPagedList<tblCardActive> GetAllCPagingByFirst(string key, string anotherkey, string cardgroups, string customergroups, string fromdate, string todate, int pageNumber = 1, int pageSize = 20)
        {
            var query = from card in _tblCardRepository.Table
                        join cardgroup in _tblCardGroupRepository.Table on card.CardGroupID equals cardgroup.CardGroupID.ToString() into card_cardgroup
                        from cardgroup in card_cardgroup.DefaultIfEmpty()
                        join customer in _tblCustomerRepository.Table on card.CustomerID equals customer.CustomerID.ToString() into card_customer
                        from customer in card_customer.DefaultIfEmpty()
                        join customergroup in _tblCustomerGroupRepository.Table on customer.CustomerGroupID equals customergroup.CustomerGroupID.ToString() into customer_group
                        from customergroup in customer_group.DefaultIfEmpty()

                        where cardgroup.CardType == 0 && card.IsDelete == false && card.IsLock == false

                        select new tblCardActive()
                        {
                            CardID = card.CardID.ToString(),
                            CardNo = card.CardNo,
                            CardNumber = card.CardNumber,
                            DateActive = card.DateActive,
                            ImportDate = card.ImportDate,
                            Plate1 = card.Plate1,
                            Plate2 = card.Plate2,
                            Plate3 = card.Plate3,
                            VehicleName1 = card.VehicleName1,
                            VehicleName2 = card.VehicleName2,
                            VehicleName3 = card.VehicleName3,

                            CustomerId = customer.CustomerID.ToString(),
                            CustomerCode = customer.CustomerCode,
                            CustomerName = customer.CustomerName,

                            CustomerGroupId = customergroup.CustomerGroupID.ToString(),
                            CustomerGroupName = customergroup.CustomerGroupName,

                            CardGroupId = cardgroup.CardGroupID.ToString(),
                            CardGroupName = cardgroup.CardGroupName
                        };

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.CardNo.Contains(key) || n.CardNumber.Contains(key) || n.Plate1.Contains(key) || n.Plate2.Contains(key) || n.Plate3.Contains(key) || n.CustomerCode.Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(anotherkey))
            {
                query = query.Where(n => n.CardNo.Contains(anotherkey) || n.CardNumber.Contains(anotherkey) || n.Plate1.Contains(anotherkey) || n.Plate2.Contains(anotherkey) || n.Plate3.Contains(anotherkey) || n.CustomerName.Contains(anotherkey));
            }

            if (!string.IsNullOrWhiteSpace(cardgroups))
            {
                query = query.Where(n => cardgroups.Contains(n.CardGroupId));
            }
            if (!string.IsNullOrEmpty(AuthCardGroupIds))
            {
                var lst = AuthCardGroupIds.Split(';');
                query = query.Where(n => lst.Contains(n.CardGroupId));
            }

            if (!string.IsNullOrWhiteSpace(customergroups))
            {
                query = query.Where(n => customergroups.Contains(n.CustomerGroupId));
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                var fdate = Convert.ToDateTime(fromdate);
                var tdate = Convert.ToDateTime(todate).AddDays(1);

                query = query.Where(n => n.DateActive >= fdate && n.DateActive < tdate);
            }

            var list = new PagedList<tblCardActive>(query.OrderBy(n => n.CardNo), pageNumber, pageSize);

            return list;
        }

        public IEnumerable<tblCardActive> GetAllCActiveByListId(string ids)
        {
            var query = from card in _tblCardRepository.Table
                        join cardgroup in _tblCardGroupRepository.Table on card.CardGroupID equals cardgroup.CardGroupID.ToString() into card_cardgroup
                        from cardgroup in card_cardgroup.DefaultIfEmpty()
                        join customer in _tblCustomerRepository.Table on card.CustomerID equals customer.CustomerID.ToString() into card_customer
                        from customer in card_customer.DefaultIfEmpty()
                        join customergroup in _tblCustomerGroupRepository.Table on customer.CustomerGroupID equals customergroup.CustomerGroupID.ToString() into customer_group
                        from customergroup in customer_group.DefaultIfEmpty()

                        where cardgroup.CardType == 0 && card.IsDelete == false && card.IsLock == false && ids.Contains(card.CardID.ToString())

                        select new tblCardActive()
                        {
                            CardID = card.CardID.ToString(),
                            CardNo = card.CardNo,
                            CardNumber = card.CardNumber,
                            DateActive = card.DateActive,
                            ImportDate = card.ImportDate,
                            Plate1 = card.Plate1,
                            Plate2 = card.Plate2,
                            Plate3 = card.Plate3,
                            VehicleName1 = card.VehicleName1,
                            VehicleName2 = card.VehicleName2,
                            VehicleName3 = card.VehicleName3,

                            CustomerId = customer.CustomerID.ToString(),
                            CustomerCode = customer.CustomerCode,
                            CustomerName = customer.CustomerName,

                            CustomerGroupId = customergroup.CustomerGroupID.ToString(),
                            CustomerGroupName = customergroup.CustomerGroupName,

                            CardGroupId = cardgroup.CardGroupID.ToString(),
                            CardGroupName = cardgroup.CardGroupName
                        };

            return query;
        }

        public bool AddCardDateActive(string KeyWord, string AnotherKey, string CardGroupIDs, string CustomerID, string CustomerGroupID, int _feelevel, string _newexpire, string userId, bool chbEnableMinusActive, bool chbEnableDateActive = false, string dateactive = "")
        {
            var sb = new StringBuilder();
            //Update lai bang tblCard
            sb.AppendLine("UPDATE");
            sb.AppendLine("ca");
            sb.AppendLine(string.Format("SET ca.DateActive = '{0}'", _newexpire));

            sb.AppendLine("FROM");
            sb.AppendLine("tblCard AS ca");
            sb.AppendLine("LEFT JOIN dbo.tblCardGroup AS g ON ca.CardGroupID = g.CardGroupID");
            sb.AppendLine("LEFT join tblCustomer c on ca.CustomerID = CONVERT(varchar(255), c.CustomerID)");
            sb.AppendLine("WHERE IsDelete = 0 and ca.IsLock=0 and g.CardType = 0");

            //Update theo filler
            if (!string.IsNullOrWhiteSpace(KeyWord))
            {
                sb.AppendLine(" and (ca.CardNo LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.CardNumber LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.Plate1 LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.Plate2 LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.Plate3 LIKE N'%" + KeyWord + "%')");
            }

            if (!string.IsNullOrWhiteSpace(AnotherKey))
            {
                sb.AppendLine(" and (ca.CardNo LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.CardNumber LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.Plate1 LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.Plate2 LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.Plate3 LIKE N'%" + AnotherKey + "%')");
            }

            if (!string.IsNullOrWhiteSpace(CardGroupIDs))
            {
                sb.AppendLine(" and ca.CardGroupID = '" + CardGroupIDs + "'");
            }

            if (!string.IsNullOrWhiteSpace(AuthCardGroupIds))
            {
                var t = AuthCardGroupIds.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    sb.AppendLine("AND ca.CardGroupID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        sb.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    sb.AppendLine(" )");
                }
            }

            if (!string.IsNullOrWhiteSpace(CustomerID))
            {
                sb.AppendLine(" and ca.CustomerID = '" + CustomerID + "'");
            }

            if (!string.IsNullOrWhiteSpace(CustomerGroupID))
            {
                sb.AppendLine(" and c.CustomerGroupID IN ('" + CustomerGroupID + "')");
            }

            sb.AppendLine("insert into tblCardProcess(Date, Actions, CardGroupID, UserID, CustomerID, CardNumber)");
            sb.AppendLine(string.Format("SELECT GETDATE(),'ACTIVE', ca.CardGroupID,  '{0}', CASE WHEN ca.CustomerID IS NULL THEN '0' ELSE ca.CustomerID END , ca.Cardnumber", userId));

            sb.AppendLine("from tblCard ca");

            sb.AppendLine("LEFT JOIN dbo.tblCardGroup g ON ca.CardGroupID=g.CardGroupID");
            sb.AppendLine("LEFT join tblCustomer cus on ca.CustomerID = CONVERT(varchar(255), cus.CustomerID)");

            sb.AppendLine("where ca.IsDelete = 0 and ca.IsLock=0 and g.CardType=0");

            //Neu so ngay gia han <0 va neu ko check thi ko cho gia han
            if (chbEnableMinusActive == false)
            {
                sb.AppendLine(string.Format("and DATEDIFF(DAY, ca.[ExpireDate], '{0}') >=0  AND ca.[ExpireDate] <= '{0}'", _newexpire));
            }

            //Update theo filler
            if (!string.IsNullOrWhiteSpace(KeyWord))
            {
                sb.AppendLine(" and (ca.CardNo LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.CardNumber LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.Plate1 LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.Plate2 LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.Plate3 LIKE N'%" + KeyWord + "%')");
            }

            if (!string.IsNullOrWhiteSpace(AnotherKey))
            {
                sb.AppendLine(" and (ca.CardNo LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.CardNumber LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.Plate1 LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.Plate2 LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.Plate3 LIKE N'%" + AnotherKey + "%')");
            }

            if (!string.IsNullOrWhiteSpace(CardGroupIDs))
            {
                sb.AppendLine(" and ca.CardGroupID = '" + CardGroupIDs + "'");
            }
            if (!string.IsNullOrWhiteSpace(AuthCardGroupIds))
            {
                var t = AuthCardGroupIds.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    sb.AppendLine("AND ca.CardGroupID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        sb.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    sb.AppendLine(" )");
                }
            }

            if (!string.IsNullOrWhiteSpace(CustomerID))
            {
                sb.AppendLine(" and ca.CustomerID = '" + CustomerID + "'");
            }

            if (!string.IsNullOrWhiteSpace(CustomerGroupID))
            {
                sb.AppendLine(" and cus.CustomerGroupID IN ('" + CustomerGroupID + "')");
            }

            return ExcuteSQL.Execute(sb.ToString());
        }

        //dùng cho trường chinh
        public bool AddCardDateActiveTRANSERCO(string KeyWord, string AnotherKey, string CardGroupIDs, string CustomerID, string CustomerGroupID, int _feelevel, string _newexpire, string userId, bool chbEnableMinusActive, bool chbEnableDateActive = false, string dateactive = "")
        {
            var sb = new StringBuilder();
            //Update lai bang tblCard
            sb.AppendLine("UPDATE");
            sb.AppendLine("ca");
            sb.AppendLine(string.Format("SET ca.DateActive = '{0}'", _newexpire));

            sb.AppendLine("FROM");
            sb.AppendLine("tblCard AS ca");
            sb.AppendLine("LEFT JOIN dbo.tblCardGroup AS g ON ca.CardGroupID = g.CardGroupID");
            sb.AppendLine("LEFT join tblCustomer c on ca.CustomerID = CONVERT(varchar(255), c.CustomerID)");
            sb.AppendLine("WHERE IsDelete = 0 and ca.IsLock=0 and g.CardType = 0");

            //Update theo filler
            if (!string.IsNullOrWhiteSpace(KeyWord))
            {
                sb.AppendLine(" and (ca.CardNo LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.CardNumber LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.Plate1 LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.Plate2 LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.Plate3 LIKE N'%" + KeyWord + "%')");
            }

            if (!string.IsNullOrWhiteSpace(AnotherKey))
            {
                sb.AppendLine(" and (ca.CardNo LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.CardNumber LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.Plate1 LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.Plate2 LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.Plate3 LIKE N'%" + AnotherKey + "%')");
            }

            if (!string.IsNullOrWhiteSpace(CardGroupIDs))
            {
                sb.AppendLine(" and ca.CardGroupID = '" + CardGroupIDs + "'");
            }

            if (!string.IsNullOrWhiteSpace(CustomerID))
            {
                sb.AppendLine(" and ca.CustomerID = '" + CustomerID + "'");
            }

            if (!string.IsNullOrWhiteSpace(CustomerGroupID))
            {
                sb.AppendLine(" and c.CustomerGroupID IN ('" + CustomerGroupID + "')");
            }

            sb.AppendLine("insert into tblCardProcess(Date, Actions, CardGroupID, UserID, CustomerID, CardNumber,Description)");
            sb.AppendLine(string.Format("SELECT GETDATE(),'ACTIVE', ca.CardGroupID,  '{0}', CASE WHEN ca.CustomerID IS NULL THEN '0' ELSE ca.CustomerID END , ca.Cardnumber,cus.Description", userId));

            sb.AppendLine("from tblCard ca");

            sb.AppendLine("LEFT JOIN dbo.tblCardGroup g ON ca.CardGroupID=g.CardGroupID");
            sb.AppendLine("LEFT join tblCustomer cus on ca.CustomerID = CONVERT(varchar(255), cus.CustomerID)");

            sb.AppendLine("where ca.IsDelete = 0 and ca.IsLock=0 and g.CardType=0");

            //Neu so ngay gia han <0 va neu ko check thi ko cho gia han
            if (chbEnableMinusActive == false)
            {
                sb.AppendLine(string.Format("and DATEDIFF(DAY, ca.[ExpireDate], '{0}') >=0  AND ca.[ExpireDate] <= '{0}'", _newexpire));
            }

            //Update theo filler
            if (!string.IsNullOrWhiteSpace(KeyWord))
            {
                sb.AppendLine(" and (ca.CardNo LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.CardNumber LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.Plate1 LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.Plate2 LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.Plate3 LIKE N'%" + KeyWord + "%')");
            }

            if (!string.IsNullOrWhiteSpace(AnotherKey))
            {
                sb.AppendLine(" and (ca.CardNo LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.CardNumber LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.Plate1 LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.Plate2 LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.Plate3 LIKE N'%" + AnotherKey + "%')");
            }

            if (!string.IsNullOrWhiteSpace(CardGroupIDs))
            {
                sb.AppendLine(" and ca.CardGroupID = '" + CardGroupIDs + "'");
            }

            if (!string.IsNullOrWhiteSpace(CustomerID))
            {
                sb.AppendLine(" and ca.CustomerID = '" + CustomerID + "'");
            }

            if (!string.IsNullOrWhiteSpace(CustomerGroupID))
            {
                sb.AppendLine(" and cus.CustomerGroupID IN ('" + CustomerGroupID + "')");
            }

            return ExcuteSQL.Execute(sb.ToString());
        }

        public bool AddCardDateActiveByListCardNumber(string listCardNumber, int _feelevel, string _newexpire, string userId, bool chbEnableMinusActive, bool chbEnableDateActive = false, string dateactive = "")
        {
            var sb = new StringBuilder();
            //Update lai bang tblCard
            sb.AppendLine("UPDATE");
            sb.AppendLine("ca");
            sb.AppendLine(string.Format("SET ca.DateActive = '{0}'", _newexpire));

            sb.AppendLine("FROM");
            sb.AppendLine("tblCard AS ca");
            sb.AppendLine("LEFT join tblCustomer c on ca.CustomerID = CONVERT(varchar(255), c.CustomerID)");
            sb.AppendLine("WHERE IsDelete = 0 and ca.IsLock=0 ");

            if (!string.IsNullOrWhiteSpace(listCardNumber))
            {
                //where in
                sb.AppendLine(string.Format(" and ca.CardNumber IN ({0})", listCardNumber));
            }

            sb.AppendLine("insert into tblCardProcess(Date, Actions, CardGroupID, UserID, CustomerID, CardNumber)");
            sb.AppendLine(string.Format("SELECT GETDATE(),'ACTIVE', ca.CardGroupID,  '{0}', CASE WHEN ca.CustomerID IS NULL THEN '0' ELSE ca.CustomerID END , ca.Cardnumber", userId));
            sb.AppendLine("from tblCard ca");
            sb.AppendLine("LEFT join tblCustomer cus on ca.CustomerID = CONVERT(varchar(255), cus.CustomerID)");
            sb.AppendLine("where ca.IsDelete = 0 and ca.IsLock=0");

            if (!string.IsNullOrWhiteSpace(listCardNumber))
            {
                //where in
                sb.AppendLine(string.Format(" and ca.CardNumber IN ({0})", listCardNumber));
            }

            return ExcuteSQL.Execute(sb.ToString());
        }

        //dùng cho trường chinh
        public bool AddCardDateActiveByListCardNumberTRANSERCO(string listCardNumber, int _feelevel, string _newexpire, string userId, bool chbEnableMinusActive, bool chbEnableDateActive = false, string dateactive = "")
        {
            var sb = new StringBuilder();
            //Update lai bang tblCard
            sb.AppendLine("UPDATE");
            sb.AppendLine("ca");
            sb.AppendLine(string.Format("SET ca.DateActive = '{0}'", _newexpire));

            sb.AppendLine("FROM");
            sb.AppendLine("tblCard AS ca");
            sb.AppendLine("LEFT join tblCustomer c on ca.CustomerID = CONVERT(varchar(255), c.CustomerID)");
            sb.AppendLine("WHERE IsDelete = 0 and ca.IsLock=0 ");

            if (!string.IsNullOrWhiteSpace(listCardNumber))
            {
                //where in
                sb.AppendLine(string.Format(" and ca.CardNumber IN ({0})", listCardNumber));
            }

            sb.AppendLine("insert into tblCardProcess(Date, Actions, CardGroupID, UserID, CustomerID, CardNumber,Description)");
            sb.AppendLine(string.Format("SELECT GETDATE(),'ACTIVE', ca.CardGroupID,  '{0}', CASE WHEN ca.CustomerID IS NULL THEN '0' ELSE ca.CustomerID END , ca.Cardnumber,cus.Description", userId));
            sb.AppendLine("from tblCard ca");
            sb.AppendLine("LEFT join tblCustomer cus on ca.CustomerID = CONVERT(varchar(255), cus.CustomerID)");
            sb.AppendLine("where ca.IsDelete = 0 and ca.IsLock=0");

            if (!string.IsNullOrWhiteSpace(listCardNumber))
            {
                //where in
                sb.AppendLine(string.Format(" and ca.CardNumber IN ({0})", listCardNumber));
            }

            return ExcuteSQL.Execute(sb.ToString());
        }

        public List<tblCardExtend> GetAllByFirst(string key, string anotherkey, string cardgroups, string customergroups, string fromdate, string todate, string accesslevelids = "")
        {
            var query = from card in _tblCardRepository.Table
                        join cardgroup in _tblCardGroupRepository.Table on card.CardGroupID equals cardgroup.CardGroupID.ToString() into card_cardgroup
                        from cardgroup in card_cardgroup.DefaultIfEmpty()
                        join customer in _tblCustomerRepository.Table on card.CustomerID equals customer.CustomerID.ToString() into card_customer
                        from customer in card_customer.DefaultIfEmpty()
                        join customergroup in _tblCustomerGroupRepository.Table on customer.CustomerGroupID equals customergroup.CustomerGroupID.ToString() into customer_group
                        from customergroup in customer_group.DefaultIfEmpty()
                        join accesslevel in _tblAccessLevelRepository.Table on card.AccessLevelID equals accesslevel.AccessLevelID.ToString() into card_accesslevel
                        from accesslevel in card_accesslevel.DefaultIfEmpty()

                        where card.IsDelete == false && card.IsLock == false

                        select new tblCardExtend()
                        {
                            CardID = card.CardID.ToString(),
                            CardNo = card.CardNo,
                            CardNumber = card.CardNumber,
                            ExpireDate = card.ExpireDate,
                            ImportDate = card.ImportDate,
                            Plate1 = card.Plate1,
                            Plate2 = card.Plate2,
                            Plate3 = card.Plate3,
                            VehicleName1 = card.VehicleName1,
                            VehicleName2 = card.VehicleName2,
                            VehicleName3 = card.VehicleName3,

                            CustomerId = customer.CustomerID.ToString(),
                            CustomerCode = customer.CustomerCode,
                            CustomerName = customer.CustomerName,

                            CustomerGroupId = customergroup.CustomerGroupID.ToString(),
                            CustomerGroupName = customergroup.CustomerGroupName,

                            CardGroupId = cardgroup.CardGroupID.ToString(),
                            CardGroupName = cardgroup.CardGroupName,

                            AccessLevelID = card.AccessLevelID,
                            AccessLevelName = accesslevel.AccessLevelName,

                            AccessExpireDate = card.AccessExpireDate
                        };

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.CardNo.Contains(key) || n.CardNumber.Contains(key) || n.Plate1.Contains(key) || n.Plate2.Contains(key) || n.Plate3.Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(anotherkey))
            {
                query = query.Where(n => n.CardNo.Contains(anotherkey) || n.CardNumber.Contains(anotherkey) || n.Plate1.Contains(anotherkey) || n.Plate2.Contains(anotherkey) || n.Plate3.Contains(anotherkey));
            }

            if (!string.IsNullOrWhiteSpace(cardgroups))
            {
                query = query.Where(n => cardgroups.Contains(n.CardGroupId));
            }

            if (!string.IsNullOrWhiteSpace(customergroups))
            {
                query = query.Where(n => customergroups.Contains(n.CustomerGroupId));
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                var fdate = Convert.ToDateTime(fromdate);
                var tdate = Convert.ToDateTime(todate).AddDays(1);

                query = query.Where(n => n.ExpireDate >= fdate && n.ExpireDate < tdate);
            }

            if (!string.IsNullOrWhiteSpace(accesslevelids))
            {
                query = query.Where(n => accesslevelids.Contains(n.AccessLevelID));
            }


            return query.ToList();
        }

        public List<tblCardExtend> GetAllByFirst_v2(List<tblCardExtend> list)
        {
            var listid = list.Select(n => n.CardID).ToList();

            var query = from card in _tblCardRepository.Table
                        join cardgroup in _tblCardGroupRepository.Table on card.CardGroupID equals cardgroup.CardGroupID.ToString() into card_cardgroup
                        from cardgroup in card_cardgroup.DefaultIfEmpty()
                        join customer in _tblCustomerRepository.Table on card.CustomerID equals customer.CustomerID.ToString() into card_customer
                        from customer in card_customer.DefaultIfEmpty()
                        join customergroup in _tblCustomerGroupRepository.Table on customer.CustomerGroupID equals customergroup.CustomerGroupID.ToString() into customer_group
                        from customergroup in customer_group.DefaultIfEmpty()
                        join accesslevel in _tblAccessLevelRepository.Table on card.AccessLevelID equals accesslevel.AccessLevelID.ToString() into card_accesslevel
                        from accesslevel in card_accesslevel.DefaultIfEmpty()

                        where card.IsDelete == false && card.IsLock == false && listid.Contains(card.CardID.ToString())

                        select new tblCardExtend()
                        {
                            CardID = card.CardID.ToString(),
                            CardNo = card.CardNo,
                            CardNumber = card.CardNumber,
                            ExpireDate = card.ExpireDate,
                            ImportDate = card.ImportDate,
                            Plate1 = card.Plate1,
                            Plate2 = card.Plate2,
                            Plate3 = card.Plate3,
                            VehicleName1 = card.VehicleName1,
                            VehicleName2 = card.VehicleName2,
                            VehicleName3 = card.VehicleName3,

                            CustomerId = customer.CustomerID.ToString(),
                            CustomerCode = customer.CustomerCode,
                            CustomerName = customer.CustomerName,

                            CustomerGroupId = customergroup.CustomerGroupID.ToString(),
                            CustomerGroupName = customergroup.CustomerGroupName,

                            CardGroupId = cardgroup.CardGroupID.ToString(),
                            CardGroupName = cardgroup.CardGroupName,

                            AccessLevelID = card.AccessLevelID,
                            AccessLevelName = accesslevel.AccessLevelName,

                            AccessExpireDate = card.AccessExpireDate,

                            CardType = card.CardType
                        };

            

            return query.ToList();
        }

        public IPagedList<tblCardExtend> GetAllPagingByFirstForUpload(string key, string anotherkey, string cardgroups, string customergroups, string fromdate, string todate, int pageNumber = 1, int pageSize = 20, string accesslevelids = "",string column = "",string sort = "")
        {
            var query = from card in _tblCardRepository.Table
                        join cardgroup in _tblCardGroupRepository.Table on card.CardGroupID equals cardgroup.CardGroupID.ToString() into card_cardgroup
                        from cardgroup in card_cardgroup.DefaultIfEmpty()
                        join customer in _tblCustomerRepository.Table on card.CustomerID equals customer.CustomerID.ToString() into card_customer
                        from customer in card_customer.DefaultIfEmpty()
                        join customergroup in _tblCustomerGroupRepository.Table on customer.CustomerGroupID equals customergroup.CustomerGroupID.ToString() into customer_group
                        from customergroup in customer_group.DefaultIfEmpty()
                        join accesslevel in _tblAccessLevelRepository.Table on card.AccessLevelID equals accesslevel.AccessLevelID.ToString() into card_accesslevel
                        from accesslevel in card_accesslevel.DefaultIfEmpty()

                        where card.IsDelete == false && card.IsLock == false

                        select new tblCardExtend()
                        {
                            CardID = card.CardID.ToString(),
                            CardNo = card.CardNo,
                            CardNumber = card.CardNumber,
                            ExpireDate = card.ExpireDate,
                            ImportDate = card.ImportDate,
                            Plate1 = card.Plate1,
                            Plate2 = card.Plate2,
                            Plate3 = card.Plate3,
                            VehicleName1 = card.VehicleName1,
                            VehicleName2 = card.VehicleName2,
                            VehicleName3 = card.VehicleName3,

                            CustomerId = customer.CustomerID.ToString(),
                            CustomerCode = customer.CustomerCode,
                            CustomerName = customer.CustomerName,

                            CustomerGroupId = customergroup.CustomerGroupID.ToString(),
                            CustomerGroupName = customergroup.CustomerGroupName,

                            CardGroupId = cardgroup.CardGroupID.ToString(),
                            CardGroupName = cardgroup.CardGroupName,

                            AccessLevelID = card.AccessLevelID,
                            AccessLevelName = accesslevel.AccessLevelName,

                            AccessExpireDate = card.AccessExpireDate,

                            CardType = card.CardType
                        };

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.CardNo.Contains(key) || n.CardNumber.Contains(key) || n.Plate1.Contains(key) || n.Plate2.Contains(key) || n.Plate3.Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(anotherkey))
            {
                query = query.Where(n => n.CardNo.Contains(anotherkey) || n.CardNumber.Contains(anotherkey) || n.Plate1.Contains(anotherkey) || n.Plate2.Contains(anotherkey) || n.Plate3.Contains(anotherkey));
            }

            if (!string.IsNullOrWhiteSpace(cardgroups))
            {
                query = query.Where(n => cardgroups.Contains(n.CardGroupId));
            }

            if (!string.IsNullOrWhiteSpace(customergroups))
            {
                query = query.Where(n => customergroups.Contains(n.CustomerGroupId));
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                var fdate = Convert.ToDateTime(fromdate);
                var tdate = Convert.ToDateTime(todate).AddDays(1);

                query = query.Where(n => n.ExpireDate >= fdate && n.ExpireDate < tdate);
            }

            if (!string.IsNullOrWhiteSpace(accesslevelids))
            {
                query = query.Where(n => accesslevelids.Contains(n.AccessLevelID));
            }

            var list = new PagedList<tblCardExtend>(query.OrderBy(n => n.CardNo).ThenBy(n=>n.CardGroupId), pageNumber, pageSize);

            if (!string.IsNullOrEmpty(column) && !string.IsNullOrEmpty(sort))
            {
                bool asc = sort.Contains("asc") ? true : false;
                switch (column)
                {
                    case "CardNo":
                        if (asc)
                        {
                            list = new PagedList<tblCardExtend>(query.OrderBy(n => n.CardNo), pageNumber, pageSize);
                        }
                        else
                        {
                            list = new PagedList<tblCardExtend>(query.OrderByDescending(n => n.CardNo), pageNumber, pageSize);
                        }                       
                        break;
                }
            }
           
            return list;
        }

        public IEnumerable<tblCardExtend> GetAllActiveByListIdForUpload(string ids)
        {
            var query = from card in _tblCardRepository.Table
                        join cardgroup in _tblCardGroupRepository.Table on card.CardGroupID equals cardgroup.CardGroupID.ToString() into card_cardgroup
                        from cardgroup in card_cardgroup.DefaultIfEmpty()
                        join customer in _tblCustomerRepository.Table on card.CustomerID equals customer.CustomerID.ToString() into card_customer
                        from customer in card_customer.DefaultIfEmpty()
                        join customergroup in _tblCustomerGroupRepository.Table on customer.CustomerGroupID equals customergroup.CustomerGroupID.ToString() into customer_group
                        from customergroup in customer_group.DefaultIfEmpty()
                        join accesslevel in _tblAccessLevelRepository.Table on card.AccessLevelID equals accesslevel.AccessLevelID.ToString() into card_accesslevel
                        from accesslevel in card_accesslevel.DefaultIfEmpty()

                        where card.IsDelete == false && card.IsLock == false && ids.Contains(card.CardID.ToString())

                        select new tblCardExtend()
                        {
                            CardID = card.CardID.ToString(),
                            CardNo = card.CardNo,
                            CardNumber = card.CardNumber,
                            ExpireDate = card.ExpireDate,
                            ImportDate = card.ImportDate,
                            Plate1 = card.Plate1,
                            Plate2 = card.Plate2,
                            Plate3 = card.Plate3,
                            VehicleName1 = card.VehicleName1,
                            VehicleName2 = card.VehicleName2,
                            VehicleName3 = card.VehicleName3,

                            CustomerId = customer.CustomerID.ToString(),
                            CustomerCode = customer.CustomerCode,
                            CustomerName = customer.CustomerName,

                            CustomerGroupId = customergroup.CustomerGroupID.ToString(),
                            CustomerGroupName = customergroup.CustomerGroupName,

                            CardGroupId = cardgroup.CardGroupID.ToString(),
                            CardGroupName = cardgroup.CardGroupName,

                            AccessLevelID = card.AccessLevelID,

                            AccessExpireDate = card.AccessExpireDate,
                            AccessLevelName = accesslevel.AccessLevelName
                        };

            return query;
        }

        public MessageReport UpdateMultiCardLevel(string key, string cardgroups, string customerid, string customergroups, string fromdate, string todate, string ischeckbytime = "0", string accesslevelids = "", string newvalue = "")
        {
            var query = (from card in _tblCardRepository.Table
                         join cardgroup in _tblCardGroupRepository.Table on card.CardGroupID equals cardgroup.CardGroupID.ToString() into card_cardgroup
                         from cardgroup in card_cardgroup.DefaultIfEmpty()
                         join customer in _tblCustomerRepository.Table on card.CustomerID equals customer.CustomerID.ToString() into card_customer
                         from customer in card_customer.DefaultIfEmpty()
                         join customergroup in _tblCustomerGroupRepository.Table on customer.CustomerGroupID equals customergroup.CustomerGroupID.ToString() into customer_group
                         from customergroup in customer_group.DefaultIfEmpty()

                         where /*cardgroup.CardType == 0 &&*/ card.IsDelete == false /*&& card.IsLock == false*/

                         select new tblCardCustomViewModel()
                         {
                             CardID = card.CardID.ToString(),
                             CardNo = card.CardNo,
                             CardNumber = card.CardNumber,
                             CardGroupId = cardgroup.CardGroupID.ToString(),
                             CardGroupName = cardgroup.CardGroupName,
                             ExpireDate = card.ExpireDate,
                             ImportDate = card.ImportDate,
                             Plate1 = card.Plate1,
                             Plate2 = card.Plate2,
                             Plate3 = card.Plate3,
                             VehicleName1 = card.VehicleName1,
                             VehicleName2 = card.VehicleName2,
                             VehicleName3 = card.VehicleName3,
                             IsLock = card.IsLock,

                             CustomerId = customer.CustomerID.ToString(),
                             CustomerCode = customer.CustomerCode,
                             CustomerName = customer.CustomerName,
                             CustomerGroupId = customergroup.CustomerGroupID.ToString(),
                             CustomerGroupName = customergroup.CustomerGroupName,

                             AccessExpireDate = card.AccessExpireDate,
                             AccessLevelID = card.AccessLevelID
                         });

            //Từ khóa
            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.CardNo.Contains(key) || n.CardNumber.Contains(key) || n.Plate1.Contains(key) || n.Plate2.Contains(key) || n.Plate3.Contains(key));
            }

            //Nhóm thẻ
            if (!string.IsNullOrWhiteSpace(cardgroups))
            {
                query = query.Where(n => cardgroups.Contains(n.CardGroupId));
            }

            //Nhóm khách hàng
            if (!string.IsNullOrWhiteSpace(customergroups))
            {
                query = query.Where(n => customergroups.Contains(n.CustomerGroupId));
            }

            //Id người dùng
            if (!string.IsNullOrWhiteSpace(customerid))
            {
                query = query.Where(n => n.CustomerId == customerid);
            }

            if (!string.IsNullOrWhiteSpace(accesslevelids))
            {
                query = query.Where(n => accesslevelids.Contains(n.AccessLevelID));
            }

            switch (ischeckbytime)
            {
                case "1"://Ngày nhập thẻ
                    if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
                    {
                        var fdate = Convert.ToDateTime(fromdate);
                        var tdate = Convert.ToDateTime(todate).AddDays(1);

                        query = query.Where(n => n.ImportDate >= fdate && n.ImportDate < tdate);
                    }
                    break;
                case "2"://Ngày hết hạn
                    if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
                    {
                        var fdate = Convert.ToDateTime(fromdate);
                        var tdate = Convert.ToDateTime(todate).AddDays(1);

                        query = query.Where(n => n.ExpireDate >= fdate && n.ExpireDate < tdate);
                    }
                    break;
                default:
                    break;
            }

            var str = new StringBuilder();

            str.AppendLine(string.Format("UPDATE tblCard SET AccessLevelID = '{0}'", newvalue));

            if (query.Any())
            {
                str.AppendLine("WHERE CardNumber IN (");

                var count = 0;

                var list = query.ToList();

                foreach (var item in list)
                {
                    count++;
                    str.AppendLine(string.Format("'{0}'{1}", item.CardNumber, count == query.Count() ? "" : ","));
                }

                str.AppendLine(")");
            }

            var report = new MessageReport(false, "Có lỗi xảy ra");

            try
            {
                ExcuteSQL.Execute(str.ToString());

                report = new MessageReport(true, "Cập nhật thành công");
            }
            catch (Exception ex)
            {
                report = new MessageReport(false, ex.Message);
            }

            return report;
        }

        public MessageReport UpdateMultiCardType(string key, string cardgroups, string customerid, string customergroups, string fromdate, string todate, string ischeckbytime = "0", string accesslevelids = "", string cardtype = "")
        {
            var query = (from card in _tblCardRepository.Table
                         join cardgroup in _tblCardGroupRepository.Table on card.CardGroupID equals cardgroup.CardGroupID.ToString() into card_cardgroup
                         from cardgroup in card_cardgroup.DefaultIfEmpty()
                         join customer in _tblCustomerRepository.Table on card.CustomerID equals customer.CustomerID.ToString() into card_customer
                         from customer in card_customer.DefaultIfEmpty()
                         join customergroup in _tblCustomerGroupRepository.Table on customer.CustomerGroupID equals customergroup.CustomerGroupID.ToString() into customer_group
                         from customergroup in customer_group.DefaultIfEmpty()

                         where /*cardgroup.CardType == 0 &&*/ card.IsDelete == false /*&& card.IsLock == false*/

                         select new tblCardCustomViewModel()
                         {
                             CardID = card.CardID.ToString(),
                             CardNo = card.CardNo,
                             CardNumber = card.CardNumber,
                             CardGroupId = cardgroup.CardGroupID.ToString(),
                             CardGroupName = cardgroup.CardGroupName,
                             ExpireDate = card.ExpireDate,
                             ImportDate = card.ImportDate,
                             Plate1 = card.Plate1,
                             Plate2 = card.Plate2,
                             Plate3 = card.Plate3,
                             VehicleName1 = card.VehicleName1,
                             VehicleName2 = card.VehicleName2,
                             VehicleName3 = card.VehicleName3,
                             IsLock = card.IsLock,

                             CustomerId = customer.CustomerID.ToString(),
                             CustomerCode = customer.CustomerCode,
                             CustomerName = customer.CustomerName,
                             CustomerGroupId = customergroup.CustomerGroupID.ToString(),
                             CustomerGroupName = customergroup.CustomerGroupName,

                             AccessExpireDate = card.AccessExpireDate,
                             AccessLevelID = card.AccessLevelID
                         });

            //Từ khóa
            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.CardNo.Contains(key) || n.CardNumber.Contains(key) || n.Plate1.Contains(key) || n.Plate2.Contains(key) || n.Plate3.Contains(key));
            }

            //Nhóm thẻ
            if (!string.IsNullOrWhiteSpace(cardgroups))
            {
                query = query.Where(n => cardgroups.Contains(n.CardGroupId));
            }

            //Nhóm khách hàng
            if (!string.IsNullOrWhiteSpace(customergroups))
            {
                query = query.Where(n => customergroups.Contains(n.CustomerGroupId));
            }

            //Id người dùng
            if (!string.IsNullOrWhiteSpace(customerid))
            {
                query = query.Where(n => n.CustomerId == customerid);
            }

            if (!string.IsNullOrWhiteSpace(accesslevelids))
            {
                query = query.Where(n => accesslevelids.Contains(n.AccessLevelID));
            }

            switch (ischeckbytime)
            {
                case "1"://Ngày nhập thẻ
                    if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
                    {
                        var fdate = Convert.ToDateTime(fromdate);
                        var tdate = Convert.ToDateTime(todate).AddDays(1);

                        query = query.Where(n => n.ImportDate >= fdate && n.ImportDate < tdate);
                    }
                    break;
                case "2"://Ngày hết hạn
                    if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
                    {
                        var fdate = Convert.ToDateTime(fromdate);
                        var tdate = Convert.ToDateTime(todate).AddDays(1);

                        query = query.Where(n => n.ExpireDate >= fdate && n.ExpireDate < tdate);
                    }
                    break;
                default:
                    break;
            }

            var str = new StringBuilder();

            str.AppendLine(string.Format("UPDATE tblCard SET CardType = '{0}'", Convert.ToInt32(cardtype)));

            if (query.Any())
            {
                str.AppendLine("WHERE CardNumber IN (");

                var count = 0;

                var list = query.ToList();

                foreach (var item in list)
                {
                    count++;
                    str.AppendLine(string.Format("'{0}'{1}", item.CardNumber, count == query.Count() ? "" : ","));
                }

                str.AppendLine(")");
            }

            var report = new MessageReport(false, "Có lỗi xảy ra");

            try
            {
                ExcuteSQL.Execute(str.ToString());

                report = new MessageReport(true, "Cập nhật thành công");
            }
            catch (Exception ex)
            {
                report = new MessageReport(false, ex.Message);
            }

            return report;
        }
        public tblCardUpload GetCustomByCardNumber(string cardnumber)
        {
            var query = from n in _tblCardRepository.Table
                        join c in _tblCustomerRepository.Table on n.CustomerID equals c.CustomerID.ToString() into n_c
                        from c in n_c.DefaultIfEmpty()
                        where n.CardNumber == cardnumber && n.IsDelete == false
                        select new tblCardUpload()
                        {
                            CardNumber = n.CardNumber,
                            CardGroupID = n.CardGroupID,
                            CustomerID = n.CustomerID,
                            CustomerGroupID = c.CustomerGroupID,
                            AccessExpireDate = n.AccessExpireDate
                        };

            return query.FirstOrDefault();
        }

        public MessageReport UpdateCard(string action, string currentuser, string cardnumber, string dateN, bool checkuse = false)
        {
            dateN = dateN.Substring(0, 4) + @"/" + dateN.Substring(4, 2) + @"/" + dateN.Substring(6, 2);

            var result = new MessageReport(false, "Có lỗi xảy ra");

            var str = "Update tblCard set {0} = '{1}', {2} = GETDATE() {4} where CardNumber = '{3}'";

            switch (action)
            {
                case "UPLOAD":
                    str = string.Format(str, "UserIDUpload", currentuser, "DateUpload", cardnumber, checkuse ? ", AccessExpireDate='" + dateN + "'" : "");


                    break;
                case "DELETE":
                    str = string.Format(str, "UserIDDelete", currentuser, "DateDelete", cardnumber, checkuse ? ", AccessExpireDate='" + dateN + "'" : "");

                    break;
                default:
                    break;
            }

            try
            {
                result.isSuccess = ExcuteSQL.Execute(str);
                result.Message = "Thành công";
            }
            catch (Exception ex)
            {
                result.isSuccess = false;
                result.Message = ex.Message;
            }

            return result;
        }

        public IEnumerable<tblCardExtend> GetAllActiveByListIdForUpload(List<string> ids)
        {
            var query = from card in _tblCardRepository.Table
                        join cardgroup in _tblCardGroupRepository.Table on card.CardGroupID equals cardgroup.CardGroupID.ToString() into card_cardgroup
                        from cardgroup in card_cardgroup.DefaultIfEmpty()
                        join customer in _tblCustomerRepository.Table on card.CustomerID equals customer.CustomerID.ToString() into card_customer
                        from customer in card_customer.DefaultIfEmpty()
                        join customergroup in _tblCustomerGroupRepository.Table on customer.CustomerGroupID equals customergroup.CustomerGroupID.ToString() into customer_group
                        from customergroup in customer_group.DefaultIfEmpty()
                        join accesslevel in _tblAccessLevelRepository.Table on card.AccessLevelID equals accesslevel.AccessLevelID.ToString() into card_accesslevel
                        from accesslevel in card_accesslevel.DefaultIfEmpty()

                        where card.IsDelete == false && card.IsLock == false && ids.Contains(card.CardID.ToString())

                        select new tblCardExtend()
                        {
                            CardID = card.CardID.ToString(),
                            CardNo = card.CardNo,
                            CardNumber = card.CardNumber,
                            ExpireDate = card.ExpireDate,
                            ImportDate = card.ImportDate,
                            Plate1 = card.Plate1,
                            Plate2 = card.Plate2,
                            Plate3 = card.Plate3,
                            VehicleName1 = card.VehicleName1,
                            VehicleName2 = card.VehicleName2,
                            VehicleName3 = card.VehicleName3,

                            CustomerId = customer.CustomerID.ToString(),
                            CustomerCode = customer.CustomerCode,
                            CustomerName = customer.CustomerName,

                            CustomerGroupId = customergroup.CustomerGroupID.ToString(),
                            CustomerGroupName = customergroup.CustomerGroupName,

                            CardGroupId = cardgroup.CardGroupID.ToString(),
                            CardGroupName = cardgroup.CardGroupName,

                            AccessLevelID = card.AccessLevelID,

                            AccessExpireDate = card.AccessExpireDate,
                            AccessLevelName = accesslevel.AccessLevelName
                        };

            return query;
        }

        public IEnumerable<tblCardExtend> GetAllByFirstForUpload(string key, string anotherkey, string cardgroups, string customergroups, string fromdate, string todate, string accesslevelids = "")
        {
            var query = from card in _tblCardRepository.Table
                        join cardgroup in _tblCardGroupRepository.Table on card.CardGroupID equals cardgroup.CardGroupID.ToString() into card_cardgroup
                        from cardgroup in card_cardgroup.DefaultIfEmpty()
                        join customer in _tblCustomerRepository.Table on card.CustomerID equals customer.CustomerID.ToString() into card_customer
                        from customer in card_customer.DefaultIfEmpty()
                        join customergroup in _tblCustomerGroupRepository.Table on customer.CustomerGroupID equals customergroup.CustomerGroupID.ToString() into customer_group
                        from customergroup in customer_group.DefaultIfEmpty()
                        join accesslevel in _tblAccessLevelRepository.Table on card.AccessLevelID equals accesslevel.AccessLevelID.ToString() into card_accesslevel
                        from accesslevel in card_accesslevel.DefaultIfEmpty()

                        where card.IsDelete == false && card.IsLock == false

                        select new tblCardExtend()
                        {
                            CardID = card.CardID.ToString(),
                            CardNo = card.CardNo,
                            CardNumber = card.CardNumber,
                            ExpireDate = card.ExpireDate,
                            ImportDate = card.ImportDate,
                            Plate1 = card.Plate1,
                            Plate2 = card.Plate2,
                            Plate3 = card.Plate3,
                            VehicleName1 = card.VehicleName1,
                            VehicleName2 = card.VehicleName2,
                            VehicleName3 = card.VehicleName3,

                            CustomerId = customer.CustomerID.ToString(),
                            CustomerCode = customer.CustomerCode,
                            CustomerName = customer.CustomerName,

                            CustomerGroupId = customergroup.CustomerGroupID.ToString(),
                            CustomerGroupName = customergroup.CustomerGroupName,

                            CardGroupId = cardgroup.CardGroupID.ToString(),
                            CardGroupName = cardgroup.CardGroupName,

                            AccessLevelID = card.AccessLevelID,
                            AccessLevelName = accesslevel.AccessLevelName,

                            AccessExpireDate = card.AccessExpireDate
                        };

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.CardNo.Contains(key) || n.CardNumber.Contains(key) || n.Plate1.Contains(key) || n.Plate2.Contains(key) || n.Plate3.Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(anotherkey))
            {
                query = query.Where(n => n.CardNo.Contains(anotherkey) || n.CardNumber.Contains(anotherkey) || n.Plate1.Contains(anotherkey) || n.Plate2.Contains(anotherkey) || n.Plate3.Contains(anotherkey));
            }

            if (!string.IsNullOrWhiteSpace(cardgroups))
            {
                query = query.Where(n => cardgroups.Contains(n.CardGroupId));
            }

            if (!string.IsNullOrWhiteSpace(customergroups))
            {
                query = query.Where(n => customergroups.Contains(n.CustomerGroupId));
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                var fdate = Convert.ToDateTime(fromdate);
                var tdate = Convert.ToDateTime(todate).AddDays(1);

                query = query.Where(n => n.ExpireDate >= fdate && n.ExpireDate < tdate);
            }

            if (!string.IsNullOrWhiteSpace(accesslevelids))
            {
                query = query.Where(n => accesslevelids.Contains(n.AccessLevelID));
            }

            return query;
        }

        public List<tblCardCustomViewModel> GetAllPagingByFirstParkingTSQL(string key, string cardgroups, string customerid, string customergroups, string fromdate, string todate, bool desc, string columnQuery, ref int total, string ischeckbytime = "0", int pageNumber = 1, int pageSize = 20, string accesslevelids = "", string active = "", bool isfindautocapture = false)
        {          
            var sb = new StringBuilder();
            sb.AppendLine("SELECT * FROM (");
            sb.AppendLine(string.Format("Select ROW_NUMBER() OVER(ORDER BY c.{0} {1}) AS RowNumber,", columnQuery, desc ? "desc" : "asc"));
            sb.AppendLine("CONVERT(varchar(50), c.CardID) AS 'CardID',");
            sb.AppendLine("c.CardNo,");
            sb.AppendLine("c.CardNumber,");
            sb.AppendLine("c.CardGroupID,");
            sb.AppendLine("c.CustomerID,");
            sb.AppendLine("c.ImportDate,");
            sb.AppendLine("c.DateRegister,");
            sb.AppendLine("c.ExpireDate,");
            sb.AppendLine("c.AccessExpireDate,");
            sb.AppendLine("c.AccessLevelID,");
            sb.AppendLine("c.Plate1,");
            sb.AppendLine("c.Plate2,");
            sb.AppendLine("c.Plate3,");
            sb.AppendLine("c.VehicleName1,");
            sb.AppendLine("c.VehicleName2,");
            sb.AppendLine("c.VehicleName3,");
            sb.AppendLine("c.IsLock,");
            sb.AppendLine("c.Description as DescriptionCard,");
            sb.AppendLine("cu.Description,");
            sb.AppendLine("cg.CardGroupName,");
            sb.AppendLine("cu.CustomerName,");
            sb.AppendLine("cu.CustomerCode,");
            sb.AppendLine("cu.Mobile AS 'CustomerMobile',");
            sb.AppendLine("cu.Address AS 'CustomerAddress',");
            sb.AppendLine("cu.CompartmentID,");
            sb.AppendLine("cu.IDNumber AS 'CustomerIDNumber',");
            sb.AppendLine("cu.CustomerGroupID,");
            sb.AppendLine("cug.CustomerGroupName");

            sb.AppendLine("FROM tblCard c WITH(NOLOCK)");

            sb.AppendLine("LEFT JOIN tblCardGroup cg ON c.CardGroupID = CONVERT(varchar(50), cg.CardGroupID)");
            sb.AppendLine("LEFT JOIN tblCustomer cu ON c.CustomerID = CONVERT(varchar(50), cu.CustomerID)");
            sb.AppendLine("LEFT JOIN tblCustomerGroup cug ON cu.CustomerGroupID = CONVERT(varchar(50), cug.CustomerGroupID)");

            sb.AppendLine("WHERE 1=1 AND c.IsDelete = 0");

            if (isfindautocapture)
            {
                sb.AppendLine(" AND c.isAutoCapture = 1");
            }

            //Điều kiện chính

            if (!string.IsNullOrWhiteSpace(key))
            {
                sb.AppendLine(string.Format("AND (c.CardNo LIKE '%{0}%' OR c.CardNumber LIKE '%{0}%' OR c.Plate1 LIKE '%{0}%' OR c.Plate2 LIKE '%{0}%' OR c.Plate3 LIKE '%{0}%' OR c.VehicleName1 LIKE N'%{0}%' OR c.VehicleName2 LIKE N'%{0}%' OR c.VehicleName3 LIKE N'%{0}%' OR cu.CustomerName LIKE N'%{0}%' OR cu.CustomerCode LIKE N'%{0}%' OR cu.Mobile LIKE '%{0}%' OR cu.Address LIKE '%{0}%')", key));
            }

            if (!string.IsNullOrWhiteSpace(cardgroups))
            {
                var arrCardGroup = cardgroups.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var attCG = string.Join(",", arrCardGroup);

                sb.AppendLine("AND c.CardGroupID IN (");

                sb.AppendLine(attCG);

                sb.AppendLine(")");
            }

            if (!string.IsNullOrWhiteSpace(AuthCardGroupIds))
            {
                var t = AuthCardGroupIds.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    sb.AppendLine("AND c.CardGroupID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        sb.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    sb.AppendLine(" )");
                }
            }       

            if (!string.IsNullOrWhiteSpace(customergroups))
            {
                var arrCustomerGroup = customergroups.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var attCG = string.Join(",", arrCustomerGroup);

                sb.AppendLine("AND cu.CustomerGroupID IN (");

                sb.AppendLine(attCG);

                sb.AppendLine(")");
            }

            if (!string.IsNullOrWhiteSpace(customerid))
            {
                sb.AppendLine(string.Format("AND c.CustomerID = '{0}'", customerid));
            }

            if (!string.IsNullOrWhiteSpace(accesslevelids))
            {
                var arrAcLevel = accesslevelids.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var attAL = string.Join(",", arrAcLevel);

                sb.AppendLine("AND c.AccessLevelID IN (");

                sb.AppendLine(attAL);

                sb.AppendLine(")");
            }

            switch (ischeckbytime)
            {
                case "1"://Ngày đăng ký
                    if (!string.IsNullOrWhiteSpace(fromdate))
                    {
                        var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd");

                        sb.AppendLine(string.Format("AND c.DateRegister >= '{0}'", fdate));
                    }

                    if (!string.IsNullOrWhiteSpace(todate))
                    {
                        var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd");

                        //query = query.Where(n => n.ImportDate < tdate);
                        sb.AppendLine(string.Format("AND c.DateRegister < '{0}'", tdate));
                    }
                    break;
                case "2"://Ngày hết hạn
                    if (!string.IsNullOrWhiteSpace(fromdate))
                    {
                        var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd");

                        //query = query.Where(n => n.ExpireDate >= fdate);
                        sb.AppendLine(string.Format("AND c.ExpireDate >= '{0}'", fdate));
                    }

                    if (!string.IsNullOrWhiteSpace(todate))
                    {
                        var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd");

                        //query = query.Where(n => n.ExpireDate < tdate);
                        sb.AppendLine(string.Format("AND c.ExpireDate < '{0}'", tdate));
                    }
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrWhiteSpace(active))
            {
                switch (active)
                {
                    case "0":

                        sb.AppendLine(string.Format("AND c.IsLock = {0}", "0"));

                        break;
                    case "1":

                        sb.AppendLine(string.Format("AND c.IsLock = {0}", "1"));

                        break;
                    default:
                        break;
                }
            }

            sb.AppendLine(") as a");
            sb.AppendLine(string.Format("WHERE RowNumber BETWEEN (({0}-1) * {1} + 1) AND ({0} * {1})", pageNumber, pageSize));

            var listData = SqlExQuery<tblCardCustomViewModel>.ExcuteQuery(sb.ToString());

            //Tính tổng
            sb.Clear();
            sb.AppendLine("SELECT COUNT(*) TotalCount");

            sb.AppendLine("FROM tblCard c WITH(NOLOCK)");

            sb.AppendLine("LEFT JOIN tblCardGroup cg ON c.CardGroupID = CONVERT(varchar(50), cg.CardGroupID)");
            sb.AppendLine("LEFT JOIN tblCustomer cu ON c.CustomerID = CONVERT(varchar(50), cu.CustomerID)");
            sb.AppendLine("LEFT JOIN tblCustomerGroup cug ON cu.CustomerGroupID = CONVERT(varchar(50), cug.CustomerGroupID)");

            sb.AppendLine("WHERE 1=1 AND c.IsDelete = 0");

            if (isfindautocapture)
            {
                sb.AppendLine(" AND c.isAutoCapture = 1");
            }

            //Điều kiện chính

            if (!string.IsNullOrWhiteSpace(key))
            {
                sb.AppendLine(string.Format("AND (c.CardNo LIKE '%{0}%' OR c.CardNumber LIKE '%{0}%' OR c.Plate1 LIKE '%{0}%' OR c.Plate2 LIKE '%{0}%' OR c.Plate3 LIKE '%{0}%' OR c.VehicleName1 LIKE N'%{0}%' OR c.VehicleName2 LIKE N'%{0}%' OR c.VehicleName3 LIKE N'%{0}%' OR cu.CustomerName LIKE N'%{0}%' OR cu.CustomerCode LIKE N'%{0}%' OR cu.Mobile LIKE '%{0}%' OR cu.Address LIKE '%{0}%')", key));
            }

            if (!string.IsNullOrWhiteSpace(cardgroups))
            {
                var arrCardGroup = cardgroups.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var attCG = string.Join(",", arrCardGroup);

                sb.AppendLine("AND c.CardGroupID IN (");

                sb.AppendLine(attCG);

                sb.AppendLine(")");
            }

            if (!string.IsNullOrWhiteSpace(AuthCardGroupIds))
            {
                var t = AuthCardGroupIds.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    sb.AppendLine("AND c.CardGroupID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        sb.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    sb.AppendLine(" )");
                }
            }

            if (!string.IsNullOrWhiteSpace(customergroups))
            {
                var arrCustomerGroup = customergroups.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var attCG = string.Join(",", arrCustomerGroup);

                sb.AppendLine("AND cu.CustomerGroupID IN (");

                sb.AppendLine(attCG);

                sb.AppendLine(")");
            }

            if (!string.IsNullOrWhiteSpace(customerid))
            {
                sb.AppendLine(string.Format("AND c.CustomerID = '{0}'", customerid));
            }

            if (!string.IsNullOrWhiteSpace(accesslevelids))
            {
                var arrAcLevel = accesslevelids.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var attAL = string.Join(",", arrAcLevel);

                sb.AppendLine("AND c.AccessLevelID IN (");

                sb.AppendLine(attAL);

                sb.AppendLine(")");
            }

            switch (ischeckbytime)
            {
                case "1"://Ngày nhập thẻ
                    if (!string.IsNullOrWhiteSpace(fromdate))
                    {
                        var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd");

                        sb.AppendLine(string.Format("AND c.ImportDate >= '{0}'", fdate));
                    }

                    if (!string.IsNullOrWhiteSpace(todate))
                    {
                        var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd");

                        //query = query.Where(n => n.ImportDate < tdate);
                        sb.AppendLine(string.Format("AND c.ImportDate < '{0}'", tdate));
                    }
                    break;
                case "2"://Ngày hết hạn
                    if (!string.IsNullOrWhiteSpace(fromdate))
                    {
                        var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd");

                        //query = query.Where(n => n.ExpireDate >= fdate);
                        sb.AppendLine(string.Format("AND c.ExpireDate >= '{0}'", fdate));
                    }

                    if (!string.IsNullOrWhiteSpace(todate))
                    {
                        var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd");

                        //query = query.Where(n => n.ExpireDate < tdate);
                        sb.AppendLine(string.Format("AND c.ExpireDate < '{0}'", tdate));
                    }
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrWhiteSpace(active))
            {
                switch (active)
                {
                    case "0":

                        sb.AppendLine(string.Format("AND c.IsLock = {0}", "0"));

                        break;
                    case "1":

                        sb.AppendLine(string.Format("AND c.IsLock = {0}", "1"));

                        break;
                    default:
                        break;
                }
            }

            var _total = SqlExQuery<TotalPaging>.ExcuteQueryFirst(sb.ToString());
            total = _total != null ? _total.TotalCount : 0;

            return listData;
        }

        public List<tbl_CardExcel> GetExcelCardByFirstParkingTSQL(string key, string cardgroups, string customerid, string customergroups, string fromdate, string todate, bool desc, string columnQuery, string ischeckbytime = "0", string accesslevelids = "", string active = "", bool isfindautocapture = false)
        {
         
            var sb = new StringBuilder();
            sb.AppendLine("SELECT * FROM (");
            sb.AppendLine(string.Format("Select ROW_NUMBER() OVER(ORDER BY c.{0} {1}) AS RowNumber,", columnQuery, desc ? "desc" : "asc"));
            sb.AppendLine("CONVERT(varchar(50), c.CardID) AS 'CardID',");
            sb.AppendLine("c.CardNo,");
            sb.AppendLine("c.CardNumber,");
            sb.AppendLine("c.CardGroupID,");
            sb.AppendLine("c.CustomerID,");
            sb.AppendLine("c.ImportDate,");
            sb.AppendLine("c.DateRegister,");
            sb.AppendLine("c.ExpireDate,");
            sb.AppendLine("c.AccessExpireDate,");
            sb.AppendLine("c.AccessLevelID,");
            sb.AppendLine("c.Plate1,");
            sb.AppendLine("c.Plate2,");
            sb.AppendLine("c.Plate3,");
            sb.AppendLine("c.VehicleName1,");
            sb.AppendLine("c.VehicleName2,");
            sb.AppendLine("c.VehicleName3,");
            sb.AppendLine("c.IsLock,");   
            sb.AppendLine("cu.Description,");
            sb.AppendLine("cg.CardGroupName,");
            sb.AppendLine("cu.CustomerName,");
            sb.AppendLine("cu.CustomerCode,");
            sb.AppendLine("cu.Mobile AS 'CustomerMobile',");
            sb.AppendLine("cu.Address AS 'CustomerAddress',");
            sb.AppendLine("cu.IDNumber AS 'CustomerIDNumber',");
            sb.AppendLine("cu.CustomerGroupID,");
            sb.AppendLine("cug.CustomerGroupName,");
            sb.AppendLine("cu.CompartmentId ");
            sb.AppendLine("FROM tblCard c WITH(NOLOCK)");

            sb.AppendLine("LEFT JOIN tblCardGroup cg ON c.CardGroupID = CONVERT(varchar(50), cg.CardGroupID)");
            sb.AppendLine("LEFT JOIN tblCustomer cu ON c.CustomerID = CONVERT(varchar(50), cu.CustomerID)");
            sb.AppendLine("LEFT JOIN tblCustomerGroup cug ON cu.CustomerGroupID = CONVERT(varchar(50), cug.CustomerGroupID)");

            sb.AppendLine("WHERE 1=1 AND c.IsDelete = 0");

            if (isfindautocapture)
            {
                sb.AppendLine(" AND c.isAutoCapture = 1");
            }

            //Điều kiện chính

            if (!string.IsNullOrWhiteSpace(key))
            {
                sb.AppendLine(string.Format("AND (c.CardNo LIKE '%{0}%' OR c.CardNumber LIKE '%{0}%' OR c.Plate1 LIKE '%{0}%' OR c.Plate2 LIKE '%{0}%' OR c.Plate3 LIKE '%{0}%' OR c.VehicleName1 LIKE N'%{0}%' OR c.VehicleName2 LIKE N'%{0}%' OR c.VehicleName3 LIKE N'%{0}%' OR cu.CustomerName LIKE N'%{0}%' OR cu.CustomerCode LIKE N'%{0}%' OR cu.Mobile LIKE '%{0}%' OR cu.Address LIKE '%{0}%')", key));
            }

            if (!string.IsNullOrWhiteSpace(cardgroups))
            {
                var arrCardGroup = cardgroups.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var attCG = string.Join(",", arrCardGroup);

                sb.AppendLine("AND c.CardGroupID IN (");

                sb.AppendLine(attCG);

                sb.AppendLine(")");
            }

            if (!string.IsNullOrWhiteSpace(AuthCardGroupIds))
            {
                var t = AuthCardGroupIds.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    sb.AppendLine("AND c.CardGroupID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        sb.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    sb.AppendLine(" )");
                }
            }

            if (!string.IsNullOrWhiteSpace(customergroups))
            {
                var arrCustomerGroup = customergroups.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var attCG = string.Join(",", arrCustomerGroup);

                sb.AppendLine("AND cu.CustomerGroupID IN (");

                sb.AppendLine(attCG);

                sb.AppendLine(")");
            }

            if (!string.IsNullOrWhiteSpace(customerid))
            {
                sb.AppendLine(string.Format("AND c.CustomerID = '{0}'", customerid));
            }

            if (!string.IsNullOrWhiteSpace(accesslevelids))
            {
                var arrAcLevel = accesslevelids.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var attAL = string.Join(",", arrAcLevel);

                sb.AppendLine("AND c.AccessLevelID IN (");

                sb.AppendLine(attAL);

                sb.AppendLine(")");
            }

            switch (ischeckbytime)
            {
                case "1"://Ngày nhập thẻ
                    if (!string.IsNullOrWhiteSpace(fromdate))
                    {
                        var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd");

                        sb.AppendLine(string.Format("AND c.ImportDate >= '{0}'", fdate));
                    }

                    if (!string.IsNullOrWhiteSpace(todate))
                    {
                        var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd");

                        //query = query.Where(n => n.ImportDate < tdate);
                        sb.AppendLine(string.Format("AND c.ImportDate < '{0}'", tdate));
                    }
                    break;
                case "2"://Ngày hết hạn
                    if (!string.IsNullOrWhiteSpace(fromdate))
                    {
                        var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd");

                        //query = query.Where(n => n.ExpireDate >= fdate);
                        sb.AppendLine(string.Format("AND c.ExpireDate >= '{0}'", fdate));
                    }

                    if (!string.IsNullOrWhiteSpace(todate))
                    {
                        var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd");

                        //query = query.Where(n => n.ExpireDate < tdate);
                        sb.AppendLine(string.Format("AND c.ExpireDate < '{0}'", tdate));
                    }
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrWhiteSpace(active))
            {
                switch (active)
                {
                    case "0":

                        sb.AppendLine(string.Format("AND c.IsLock = {0}", "0"));

                        break;
                    case "1":

                        sb.AppendLine(string.Format("AND c.IsLock = {0}", "1"));

                        break;
                    default:
                        break;
                }
            }

            sb.AppendLine(") as a");

            var listData = SqlExQuery<tblCardCustomViewModel>.ExcuteQuery(sb.ToString());

            var list = new List<tbl_CardExcel>();
            var lstCompartment = _tblCompartmentService.GetAll();
            if (listData.Any())
            {
                var count = 0;
                foreach (var item in listData)
                {
                    count++;
                    var objCompartment = new tblCompartment();
                    if (!string.IsNullOrEmpty( item.CompartmentId) )
                    {
                        if (ExtensionMethodHelper.CheckGuid(item.CompartmentId))
                        {
                            objCompartment = _tblCompartmentService.GetById(Guid.Parse(item.CompartmentId));
                        }
                      
                    }
                   
                    var s = Convert.ToDateTime(item.DateRegister).ToString("dd/MM/yyyy").Equals("01/01/0001") ;
                    var o = new tbl_CardExcel()
                    {
                        Address = item.CustomerAddress,
                        CardGroupName = item.CardGroupName,
                        CardNo = item.CardNo,
                        CardNumber = item.CardNumber,
                        CMT = item.CustomerIDNumber,
                        CustomerCode = item.CustomerCode,
                        CustomerGroupName = item.CustomerGroupName,
                        CustomerName = item.CustomerName,
                        DateRegister = (!Convert.ToDateTime(item.DateRegister).ToString("dd/MM/yyyy").Equals("01/01/0001")) ? Convert.ToDateTime(item.DateRegister).ToString("dd/MM/yyyy") : DateTime.Now.ToString("dd/MM/yyyy"),
                        DateExpire = Convert.ToDateTime(item.ExpireDate).ToString("dd/MM/yyyy"),
                        Inactive = item.IsLock ? "Khóa" : "Hoạt động",
                        CompartmentId = objCompartment != null ? objCompartment.CompartmentName : "",
                        NumberRow = count,
                        Plates = "",
                        SĐT = item.CustomerMobile,
                        //mã hợp đồng trường chinh
                        Description = item.Description
                    };

                    if (!string.IsNullOrWhiteSpace(item.Plate1))
                    {
                        o.Plates += item.Plate1;
                    }
                  
                    if (!string.IsNullOrWhiteSpace(item.Plate2))
                    {
                        o.Plates += ";" + item.Plate2;
                    }

                    if (!string.IsNullOrWhiteSpace(item.Plate3))
                    {
                        o.Plates += ";" + item.Plate3;
                    }

                    if (!string.IsNullOrWhiteSpace(item.VehicleName1))
                    {
                        o.VehicleNames += item.VehicleName1;
                    }

                    if (!string.IsNullOrWhiteSpace(item.VehicleName2))
                    {
                        o.VehicleNames += ";" + item.VehicleName2;
                    }

                    if (!string.IsNullOrWhiteSpace(item.VehicleName3))
                    {
                        o.VehicleNames += ";" + item.VehicleName3;
                    }

                    list.Add(o);
                }
            }

            return list;
        }

        public List<tblCardExcel_v2> GetExcelCardByFirstParkingTSQL_v2(string key, string cardgroups, string customerid, string customergroups, string fromdate, string todate, bool desc, string columnQuery, string ischeckbytime = "0", string accesslevelids = "", string active = "", bool isfindautocapture = false)
        {
            var sb = new StringBuilder();
            sb.AppendLine("SELECT * FROM (");
            sb.AppendLine(string.Format("Select ROW_NUMBER() OVER(ORDER BY c.{0} {1}) AS RowNumber,", columnQuery, desc ? "desc" : "asc"));
            sb.AppendLine("CONVERT(varchar(50), c.CardID) AS 'CardID',");
            sb.AppendLine("c.CardNo,");
            sb.AppendLine("c.CardNumber,");
            sb.AppendLine("c.CardGroupID,");
            sb.AppendLine("c.CustomerID,");
            sb.AppendLine("c.ImportDate,");
            sb.AppendLine("c.ExpireDate,");
            sb.AppendLine("c.AccessExpireDate,");
            sb.AppendLine("c.AccessLevelID,");
            sb.AppendLine("c.Plate1,");
            sb.AppendLine("c.Plate2,");
            sb.AppendLine("c.Plate3,");
            sb.AppendLine("c.VehicleName1,");
            sb.AppendLine("c.VehicleName2,");
            sb.AppendLine("c.VehicleName3,");
            sb.AppendLine("c.IsLock,");
            sb.AppendLine("c.Description as DescriptionCard,");
            sb.AppendLine("cu.Description,");
            sb.AppendLine("cg.CardGroupName,");
            sb.AppendLine("cu.CustomerName,");
            sb.AppendLine("cu.CustomerCode,");
            sb.AppendLine("cu.Mobile AS 'CustomerMobile',");
            sb.AppendLine("cu.Address AS 'CustomerAddress',");
            sb.AppendLine("cu.IDNumber AS 'CustomerIDNumber',");
            sb.AppendLine("cu.CustomerGroupID,");
            sb.AppendLine("cug.CustomerGroupName");

            sb.AppendLine("FROM tblCard c WITH(NOLOCK)");

            sb.AppendLine("LEFT JOIN tblCardGroup cg ON c.CardGroupID = CONVERT(varchar(50), cg.CardGroupID)");
            sb.AppendLine("LEFT JOIN tblCustomer cu ON c.CustomerID = CONVERT(varchar(50), cu.CustomerID)");
            sb.AppendLine("LEFT JOIN tblCustomerGroup cug ON cu.CustomerGroupID = CONVERT(varchar(50), cug.CustomerGroupID)");

            sb.AppendLine("WHERE 1=1 AND c.IsDelete = 0");

            if (isfindautocapture)
            {
                sb.AppendLine(" AND c.isAutoCapture = 1");
            }

            //Điều kiện chính

            if (!string.IsNullOrWhiteSpace(key))
            {
                sb.AppendLine(string.Format("AND (c.CardNo LIKE '%{0}%' OR c.CardNumber LIKE '%{0}%' OR c.Plate1 LIKE '%{0}%' OR c.Plate2 LIKE '%{0}%' OR c.Plate3 LIKE '%{0}%' OR c.VehicleName1 LIKE N'%{0}%' OR c.VehicleName2 LIKE N'%{0}%' OR c.VehicleName3 LIKE N'%{0}%' OR cu.CustomerName LIKE N'%{0}%' OR cu.CustomerCode LIKE N'%{0}%' OR cu.Mobile LIKE '%{0}%' OR cu.Address LIKE '%{0}%')", key));
            }

            if (!string.IsNullOrWhiteSpace(cardgroups))
            {
                var arrCardGroup = cardgroups.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var attCG = string.Join(",", arrCardGroup);

                sb.AppendLine("AND c.CardGroupID IN (");

                sb.AppendLine(attCG);

                sb.AppendLine(")");
            }

            if (!string.IsNullOrWhiteSpace(customergroups))
            {
                var arrCustomerGroup = customergroups.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var attCG = string.Join(",", arrCustomerGroup);

                sb.AppendLine("AND cu.CustomerGroupID IN (");

                sb.AppendLine(attCG);

                sb.AppendLine(")");
            }

            if (!string.IsNullOrWhiteSpace(customerid))
            {
                sb.AppendLine(string.Format("AND c.CustomerID = '{0}'", customerid));
            }

            if (!string.IsNullOrWhiteSpace(accesslevelids))
            {
                var arrAcLevel = accesslevelids.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var attAL = string.Join(",", arrAcLevel);

                sb.AppendLine("AND c.AccessLevelID IN (");

                sb.AppendLine(attAL);

                sb.AppendLine(")");
            }

            switch (ischeckbytime)
            {
                case "1"://Ngày nhập thẻ
                    if (!string.IsNullOrWhiteSpace(fromdate))
                    {
                        var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd");

                        sb.AppendLine(string.Format("AND c.ImportDate >= '{0}'", fdate));
                    }

                    if (!string.IsNullOrWhiteSpace(todate))
                    {
                        var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd");

                        //query = query.Where(n => n.ImportDate < tdate);
                        sb.AppendLine(string.Format("AND c.ImportDate < '{0}'", tdate));
                    }
                    break;
                case "2"://Ngày hết hạn
                    if (!string.IsNullOrWhiteSpace(fromdate))
                    {
                        var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd");

                        //query = query.Where(n => n.ExpireDate >= fdate);
                        sb.AppendLine(string.Format("AND c.ExpireDate >= '{0}'", fdate));
                    }

                    if (!string.IsNullOrWhiteSpace(todate))
                    {
                        var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd");

                        //query = query.Where(n => n.ExpireDate < tdate);
                        sb.AppendLine(string.Format("AND c.ExpireDate < '{0}'", tdate));
                    }
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrWhiteSpace(active))
            {
                switch (active)
                {
                    case "0":

                        sb.AppendLine(string.Format("AND c.IsLock = {0}", "0"));

                        break;
                    case "1":

                        sb.AppendLine(string.Format("AND c.IsLock = {0}", "1"));

                        break;
                    default:
                        break;
                }
            }

            sb.AppendLine(") as a");

            var listData = SqlExQuery<tblCardCustomViewModel>.ExcuteQuery(sb.ToString());

            var list = new List<tblCardExcel_v2>();

            if (listData.Any())
            {
                var count = 0;
                foreach (var item in listData)
                {
                    count++;
                    var o = new tblCardExcel_v2()
                    {
                        Address = item.CustomerAddress,
                        CardGroupName = item.CardGroupName,
                        CardNo = item.CardNo,
                        CardNumber = item.CardNumber,
                        CMT = item.CustomerIDNumber,
                        CustomerCode = item.CustomerCode,
                        CustomerGroupName = item.CustomerGroupName,
                        CustomerName = item.CustomerName,
                        DateCreated = Convert.ToDateTime(item.ImportDate).ToString("dd/MM/yyyy"),
                        DateExpire = Convert.ToDateTime(item.ExpireDate).ToString("dd/MM/yyyy"),
                        Inactive = item.IsLock ? "Khóa" : "Hoạt động",
                        CompartmentId = item.CompartmentId,
                        NumberRow = count,
                        Plates = "",
                        SĐT = item.CustomerMobile,
                        //mã hợp đồng trường chinh
                        Description = item.Description,
                        DescriptionCard = item.DescriptionCard
                    };

                    if (!string.IsNullOrWhiteSpace(item.Plate1))
                    {
                        o.Plates += item.Plate1;
                    }

                    if (!string.IsNullOrWhiteSpace(item.Plate2))
                    {
                        o.Plates += ";" + item.Plate2;
                    }

                    if (!string.IsNullOrWhiteSpace(item.Plate3))
                    {
                        o.Plates += ";" + item.Plate3;
                    }

                    if (!string.IsNullOrWhiteSpace(item.VehicleName1))
                    {
                        o.VehicleNames += item.VehicleName1;
                    }

                    if (!string.IsNullOrWhiteSpace(item.VehicleName2))
                    {
                        o.VehicleNames += ";" + item.VehicleName2;
                    }

                    if (!string.IsNullOrWhiteSpace(item.VehicleName3))
                    {
                        o.VehicleNames += ";" + item.VehicleName3;
                    }

                    list.Add(o);
                }
            }

            return list;
        }

        public List<tblCardCustomViewModel> GetAllPagingByFirstTSQL(string key, string cardgroups, string customerid, string customergroups, string fromdate, string todate, bool desc, string columnQuery, ref int total, string ischeckbytime = "0", int pageNumber = 1, int pageSize = 20, string accesslevelids = "", string active = "", string cardtype = "")
        {
            var sb = new StringBuilder();
            sb.AppendLine("SELECT * FROM (");
            sb.AppendLine(string.Format("Select ROW_NUMBER() OVER(ORDER BY c.{0} {1}) AS RowNumber,", columnQuery, desc ? "desc" : "asc"));
            sb.AppendLine("CONVERT(varchar(50), c.CardID) AS 'CardID',");
            sb.AppendLine("c.CardNo,");
            sb.AppendLine("c.CardNumber,");
            sb.AppendLine("c.CardGroupID,");
            sb.AppendLine("c.CustomerID,");
            sb.AppendLine("c.ImportDate,");
            sb.AppendLine("cu.Description,");
            sb.AppendLine("cg.CardGroupName,");
            sb.AppendLine("cu.CustomerName,");
            sb.AppendLine("c.IsLock,");
            sb.AppendLine("c.CardType,");
            sb.AppendLine("c.Plate1,");
            sb.AppendLine("c.Plate2,");
            sb.AppendLine("c.Plate3,");
            sb.AppendLine("c.AccessExpireDate,");
            sb.AppendLine("c.AccessLevelID,");
           
            //sb.AppendLine("c.VehicleName1,");
            //sb.AppendLine("c.VehicleName2,");
            //sb.AppendLine("c.VehicleName3,");
          
         
            
         
            sb.AppendLine("cu.CustomerCode,");
            sb.AppendLine("cu.Mobile AS 'CustomerMobile',");
            sb.AppendLine("cu.Address AS 'CustomerAddress',");
            sb.AppendLine("cu.IDNumber AS 'CustomerIDNumber',");
            sb.AppendLine("cu.CustomerGroupID,");
            sb.AppendLine("cug.CustomerGroupName,");

            sb.AppendLine("c.ExpireDate");
            sb.AppendLine("FROM tblCard c WITH(NOLOCK)");

            sb.AppendLine("LEFT JOIN tblCardGroup cg ON c.CardGroupID = CONVERT(varchar(50), cg.CardGroupID)");
            sb.AppendLine("LEFT JOIN tblCustomer cu ON c.CustomerID = CONVERT(varchar(50), cu.CustomerID)");
            sb.AppendLine("LEFT JOIN tblCustomerGroup cug ON cu.CustomerGroupID = CONVERT(varchar(50), cug.CustomerGroupID)");

            sb.AppendLine("WHERE 1=1 AND c.IsDelete = 0");

            //Điều kiện chính

            if (!string.IsNullOrWhiteSpace(key))
            {
                sb.AppendLine(string.Format("AND (c.CardNo LIKE '%{0}%' OR c.CardNumber LIKE '%{0}%' OR c.Plate1 LIKE '%{0}%' OR c.Plate2 LIKE '%{0}%' OR c.Plate3 LIKE '%{0}%'   OR cu.CustomerName LIKE N'%{0}%' OR cu.CustomerCode LIKE N'%{0}%' OR cu.Mobile LIKE '%{0}%' OR cu.Address LIKE '%{0}%')", key));
            }

            if (!string.IsNullOrWhiteSpace(cardgroups))
            {
                var arrCardGroup = cardgroups.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var attCG = string.Join(",", arrCardGroup);

                sb.AppendLine("AND c.CardGroupID IN (");

                sb.AppendLine(attCG);

                sb.AppendLine(")");
            }

            if (!string.IsNullOrWhiteSpace(customergroups))
            {
                var arrCustomerGroup = customergroups.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);



                var attCG = string.Join(",", arrCustomerGroup);

                sb.AppendLine("AND cu.CustomerGroupID IN (");

                sb.AppendLine(attCG);

                sb.AppendLine(")");
            }

            if (!string.IsNullOrWhiteSpace(customerid))
            {
                sb.AppendLine(string.Format("AND c.CustomerID = '{0}'", customerid));
            }

            if (!string.IsNullOrWhiteSpace(accesslevelids))
            {
                var arrAcLevel = accesslevelids.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var attAL = string.Join(",", arrAcLevel);

                sb.AppendLine("AND c.AccessLevelID IN (");

                sb.AppendLine(attAL);

                sb.AppendLine(")");
            }

            switch (ischeckbytime)
            {
                case "1"://Ngày nhập thẻ
                    if (!string.IsNullOrWhiteSpace(fromdate))
                    {
                        var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd");

                        sb.AppendLine(string.Format("AND c.ImportDate >= '{0}'", fdate));
                    }

                    if (!string.IsNullOrWhiteSpace(todate))
                    {
                        var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd");

                        //query = query.Where(n => n.ImportDate < tdate);
                        sb.AppendLine(string.Format("AND c.ImportDate < '{0}'", tdate));
                    }
                    break;
                case "2"://Ngày hết hạn
                    if (!string.IsNullOrWhiteSpace(fromdate))
                    {
                        var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd");

                        //query = query.Where(n => n.ExpireDate >= fdate);
                        sb.AppendLine(string.Format("AND c.AccessExpireDate >= '{0}'", fdate));
                    }

                    if (!string.IsNullOrWhiteSpace(todate))
                    {
                        var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd");

                        //query = query.Where(n => n.ExpireDate < tdate);
                        sb.AppendLine(string.Format("AND c.AccessExpireDate < '{0}'", tdate));
                    }
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrWhiteSpace(active))
            {
                switch (active)
                {
                    case "0":

                        sb.AppendLine(string.Format("AND c.IsLock = {0}", "0"));

                        break;
                    case "1":

                        sb.AppendLine(string.Format("AND c.IsLock = {0}", "1"));

                        break;
                    default:
                        break;
                }
            }

            if(!string.IsNullOrWhiteSpace(cardtype))
            {
                sb.AppendLine(string.Format("AND c.CardType = {0}", cardtype));
            }

            sb.AppendLine(") as a");
            sb.AppendLine(string.Format("WHERE RowNumber BETWEEN (({0}-1) * {1} + 1) AND ({0} * {1})", pageNumber, pageSize));

            var listData = SqlExQuery<tblCardCustomViewModel>.ExcuteQuery(sb.ToString());

            //Tính tổng
            sb.Clear();
            sb.AppendLine("SELECT COUNT(*) TotalCount");

            sb.AppendLine("FROM tblCard c WITH(NOLOCK)");

            sb.AppendLine("LEFT JOIN tblCardGroup cg ON c.CardGroupID = CONVERT(varchar(50), cg.CardGroupID)");
            sb.AppendLine("LEFT JOIN tblCustomer cu ON c.CustomerID = CONVERT(varchar(50), cu.CustomerID)");
            sb.AppendLine("LEFT JOIN tblCustomerGroup cug ON cu.CustomerGroupID = CONVERT(varchar(50), cug.CustomerGroupID)");

            sb.AppendLine("WHERE 1=1 AND c.IsDelete = 0");

            //Điều kiện chính

            if (!string.IsNullOrWhiteSpace(key))
            {
                sb.AppendLine(string.Format("AND (c.CardNo LIKE '%{0}%' OR c.CardNumber LIKE '%{0}%' OR c.Plate1 LIKE '%{0}%' OR c.Plate2 LIKE '%{0}%' OR c.Plate3 LIKE '%{0}%'   OR cu.CustomerName LIKE N'%{0}%' OR cu.CustomerCode LIKE N'%{0}%' OR cu.Mobile LIKE '%{0}%' OR cu.Address LIKE '%{0}%')", key));
            }


            if (!string.IsNullOrWhiteSpace(cardgroups))
            {
                var arrCardGroup = cardgroups.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var attCG = string.Join(",", arrCardGroup);

                sb.AppendLine("AND c.CardGroupID IN (");

                sb.AppendLine(attCG);

                sb.AppendLine(")");
            }

            if (!string.IsNullOrWhiteSpace(customergroups))
            {
                var arrCustomerGroup = customergroups.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var attCG = string.Join(",", arrCustomerGroup);

                sb.AppendLine("AND cu.CustomerGroupID IN (");

                sb.AppendLine(attCG);

                sb.AppendLine(")");
            }

            if (!string.IsNullOrWhiteSpace(customerid))
            {
                sb.AppendLine(string.Format("AND c.CustomerID = '{0}'", customerid));
            }

            if (!string.IsNullOrWhiteSpace(accesslevelids))
            {
                var arrAcLevel = accesslevelids.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var attAL = string.Join(",", arrAcLevel);

                sb.AppendLine("AND c.AccessLevelID IN (");

                sb.AppendLine(attAL);

                sb.AppendLine(")");
            }

            switch (ischeckbytime)
            {
                case "1"://Ngày nhập thẻ
                    if (!string.IsNullOrWhiteSpace(fromdate))
                    {
                        var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd");

                        sb.AppendLine(string.Format("AND c.ImportDate >= '{0}'", fdate));
                    }

                    if (!string.IsNullOrWhiteSpace(todate))
                    {
                        var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd");

                        //query = query.Where(n => n.ImportDate < tdate);
                        sb.AppendLine(string.Format("AND c.ImportDate < '{0}'", tdate));
                    }
                    break;
                case "2"://Ngày hết hạn
                    if (!string.IsNullOrWhiteSpace(fromdate))
                    {
                        var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd");

                        //query = query.Where(n => n.ExpireDate >= fdate);
                        sb.AppendLine(string.Format("AND c.AccessExpireDate >= '{0}'", fdate));
                    }

                    if (!string.IsNullOrWhiteSpace(todate))
                    {
                        var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd");

                        //query = query.Where(n => n.ExpireDate < tdate);
                        sb.AppendLine(string.Format("AND c.AccessExpireDate < '{0}'", tdate));
                    }
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrWhiteSpace(active))
            {
                switch (active)
                {
                    case "0":

                        sb.AppendLine(string.Format("AND c.IsLock = {0}", "0"));

                        break;
                    case "1":

                        sb.AppendLine(string.Format("AND c.IsLock = {0}", "1"));

                        break;
                    default:
                        break;
                }
            }

            if (!string.IsNullOrWhiteSpace(cardtype))
            {
                sb.AppendLine(string.Format("AND c.CardType = {0}", cardtype));
            }

            var _total = SqlExQuery<TotalPaging>.ExcuteQueryFirst(sb.ToString());
            total = _total != null ? _total.TotalCount : 0;

            return listData;
        }

        public List<tblAccessCardExcel> GetAccessExcelCardByFirstTSQL(string key, string cardgroups, string customerid, string customergroups, string fromdate, string todate, bool desc, string columnQuery, string ischeckbytime = "0", string accesslevelids = "", string active = "", string cardtype = "")
        {
            var sb = new StringBuilder();
            sb.AppendLine("SELECT * FROM (");
            sb.AppendLine(string.Format("Select ROW_NUMBER() OVER(ORDER BY c.{0} {1}) AS RowNumber,", columnQuery, desc ? "desc" : "asc"));
            sb.AppendLine("CONVERT(varchar(50), c.CardID) AS 'CardID',");
            sb.AppendLine("c.CardNo,");
            sb.AppendLine("c.CardNumber,");
            sb.AppendLine("c.CardGroupID,");
            sb.AppendLine("c.CustomerID,");
            sb.AppendLine("c.ImportDate,");
            sb.AppendLine("cu.Description,");
            sb.AppendLine("cg.CardGroupName,");
            sb.AppendLine("cu.CustomerName,");
            sb.AppendLine("c.IsLock,");
            sb.AppendLine("c.CardType,");
            sb.AppendLine("c.Plate1,");
            sb.AppendLine("c.Plate2,");
            sb.AppendLine("c.Plate3,");
            sb.AppendLine("c.AccessExpireDate,");
            sb.AppendLine("c.AccessLevelID,");

            //sb.AppendLine("c.VehicleName1,");
            //sb.AppendLine("c.VehicleName2,");
            //sb.AppendLine("c.VehicleName3,");




            sb.AppendLine("cu.CustomerCode,");
            sb.AppendLine("cu.Mobile AS 'CustomerMobile',");
            sb.AppendLine("cu.Address AS 'CustomerAddress',");
            sb.AppendLine("cu.IDNumber AS 'CustomerIDNumber',");
            sb.AppendLine("cu.CustomerGroupID,");
            sb.AppendLine("cug.CustomerGroupName,");

            sb.AppendLine("c.ExpireDate");

            sb.AppendLine("FROM tblCard c WITH(NOLOCK)");

            sb.AppendLine("LEFT JOIN tblCardGroup cg ON c.CardGroupID = CONVERT(varchar(50), cg.CardGroupID)");
            sb.AppendLine("LEFT JOIN tblCustomer cu ON c.CustomerID = CONVERT(varchar(50), cu.CustomerID)");
            sb.AppendLine("LEFT JOIN tblCustomerGroup cug ON cu.CustomerGroupID = CONVERT(varchar(50), cug.CustomerGroupID)");

            sb.AppendLine("WHERE 1=1 AND c.IsDelete = 0");

            //Điều kiện chính

            if (!string.IsNullOrWhiteSpace(key))
            {
                sb.AppendLine(string.Format("AND (c.CardNo LIKE '%{0}%' OR c.CardNumber LIKE '%{0}%' OR c.Plate1 LIKE '%{0}%' OR c.Plate2 LIKE '%{0}%' OR c.Plate3 LIKE '%{0}%' OR c.VehicleName1 LIKE N'%{0}%' OR c.VehicleName2 LIKE N'%{0}%' OR c.VehicleName3 LIKE N'%{0}%' OR cu.CustomerName LIKE N'%{0}%' OR cu.CustomerCode LIKE N'%{0}%' OR cu.Mobile LIKE '%{0}%' OR cu.Address LIKE '%{0}%')", key));
            }

            if (!string.IsNullOrWhiteSpace(cardgroups))
            {
                var arrCardGroup = cardgroups.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var attCG = string.Join(",", arrCardGroup);

                sb.AppendLine("AND c.CardGroupID IN (");

                sb.AppendLine(attCG);

                sb.AppendLine(")");
            }

            if (!string.IsNullOrWhiteSpace(customergroups))
            {
                var arrCustomerGroup = customergroups.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var attCG = string.Join(",", arrCustomerGroup);

                sb.AppendLine("AND cu.CustomerGroupID IN (");

                sb.AppendLine(attCG);

                sb.AppendLine(")");
            }

            if (!string.IsNullOrWhiteSpace(customerid))
            {
                sb.AppendLine(string.Format("AND c.CustomerID = '{0}'", customerid));
            }

            if (!string.IsNullOrWhiteSpace(accesslevelids))
            {
                var arrAcLevel = accesslevelids.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var attAL = string.Join(",", arrAcLevel);

                sb.AppendLine("AND c.AccessLevelID IN (");

                sb.AppendLine(attAL);

                sb.AppendLine(")");
            }

            switch (ischeckbytime)
            {
                case "1"://Ngày nhập thẻ
                    if (!string.IsNullOrWhiteSpace(fromdate))
                    {
                        var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd");

                        sb.AppendLine(string.Format("AND c.ImportDate >= '{0}'", fdate));
                    }

                    if (!string.IsNullOrWhiteSpace(todate))
                    {
                        var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd");

                        //query = query.Where(n => n.ImportDate < tdate);
                        sb.AppendLine(string.Format("AND c.ImportDate < '{0}'", tdate));
                    }
                    break;
                case "2"://Ngày hết hạn
                    if (!string.IsNullOrWhiteSpace(fromdate))
                    {
                        var fdate = Convert.ToDateTime(fromdate);

                        //query = query.Where(n => n.ExpireDate >= fdate);
                        sb.AppendLine(string.Format("AND c.ExpireDate >= '{0}'", fdate));
                    }

                    if (!string.IsNullOrWhiteSpace(todate))
                    {
                        var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd");

                        //query = query.Where(n => n.ExpireDate < tdate);
                        sb.AppendLine(string.Format("AND c.ExpireDate < '{0}'", tdate));
                    }
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrWhiteSpace(active))
            {
                switch (active)
                {
                    case "0":

                        sb.AppendLine(string.Format("AND c.IsLock = {0}", "0"));

                        break;
                    case "1":

                        sb.AppendLine(string.Format("AND c.IsLock = {0}", "1"));

                        break;
                    default:
                        break;
                }
            }

            if(!string.IsNullOrWhiteSpace(cardtype))
            {
                sb.AppendLine(string.Format("AND c.CardType = {0}", cardtype));
            }

            sb.AppendLine(") as a");

            var listData = SqlExQuery<tblCardCustomViewModel>.ExcuteQuery(sb.ToString());

            var listLevel = _tblAccessLevelService.GetAllActive().ToList();

            var list = new List<tblAccessCardExcel>();

            if (listData.Any())
            {
                var count = 0;
                foreach (var item in listData)
                {
                    var levelname = listLevel.FirstOrDefault(n => n.AccessLevelID.ToString() == item.AccessLevelID);

                    count++;
                    var o = new tblAccessCardExcel()
                    {
                        Address = item.CustomerAddress,
                        CardGroupName = item.CardGroupName,
                        CardNo = item.CardNo,
                        CardNumber = item.CardNumber,
                        CMT = item.CustomerIDNumber,
                        CustomerCode = item.CustomerCode,
                        CustomerGroupName = item.CustomerGroupName,
                        CustomerName = item.CustomerName,
                        DateCreated = Convert.ToDateTime(item.ImportDate).ToString("dd/MM/yyyy"),
                        DateExpire = Convert.ToDateTime(item.AccessExpireDate).ToString("dd/MM/yyyy"),
                        Inactive = item.IsLock ? "Khóa" : "Hoạt động",
                        NumberRow = count,
                        SĐT = item.CustomerMobile,
                        AccessLevelName = levelname != null ? levelname.AccessLevelName : "",
                    };

                    var _cardType = FunctionHelper.CardChipTypes().Where(p => p.ItemValue == item.CardType.ToString()).Select(p=>p.ItemText).FirstOrDefault();

                    o.CardType = _cardType;

                    list.Add(o);
                }
            }

            return list;
        }
        public List<tblActiveCard_TC> GetMoneyByCardNumber()
        {
            var query = new StringBuilder();
            query.AppendLine("Select top 1 with ties CardNumber,FeeLevel");
            query.AppendLine("From tblActiveCard");
            query.AppendLine("where FeeLevel > 0 AND IsDelete = 0");
            query.AppendLine("Order By Row_Number() over (Partition By CardNumber Order By date Desc)");

            var listData = SqlExQuery<tblActiveCard_TC>.ExcuteQuery(query.ToString());

            return listData;
        }

        public IPagedList<tblCardExtend> GetAllPagingByFirstForUploadLocker(string key, string anotherkey, string cardgroups, string customergroups, string fromdate, string todate, int pageNumber = 1, int pageSize = 20)
        {
            var query = from card in _tblCardRepository.Table
                        join cardgroup in _tblCardGroupRepository.Table on card.CardGroupID equals cardgroup.CardGroupID.ToString() into card_cardgroup
                        from cardgroup in card_cardgroup.DefaultIfEmpty()
                        join customer in _tblCustomerRepository.Table on card.CustomerID equals customer.CustomerID.ToString() into card_customer
                        from customer in card_customer.DefaultIfEmpty()
                        join customergroup in _tblCustomerGroupRepository.Table on customer.CustomerGroupID equals customergroup.CustomerGroupID.ToString() into customer_group
                        from customergroup in customer_group.DefaultIfEmpty()
                        join accesslevel in _tblAccessLevelRepository.Table on card.AccessLevelID equals accesslevel.AccessLevelID.ToString() into card_accesslevel
                        from accesslevel in card_accesslevel.DefaultIfEmpty()

                        where card.IsDelete == false && card.IsLock == false

                        select new tblCardExtend()
                        {
                            CardID = card.CardID.ToString(),
                            CardNo = card.CardNo,
                            CardNumber = card.CardNumber,
                            ExpireDate = card.ExpireDate,
                            ImportDate = card.ImportDate,
                            Plate1 = card.Plate1,
                            Plate2 = card.Plate2,
                            Plate3 = card.Plate3,
                            VehicleName1 = card.VehicleName1,
                            VehicleName2 = card.VehicleName2,
                            VehicleName3 = card.VehicleName3,

                            CustomerId = customer.CustomerID.ToString(),
                            CustomerCode = customer.CustomerCode,
                            CustomerName = customer.CustomerName,

                            CustomerGroupId = customergroup.CustomerGroupID.ToString(),
                            CustomerGroupName = customergroup.CustomerGroupName,

                            CardGroupId = cardgroup.CardGroupID.ToString(),
                            CardGroupName = cardgroup.CardGroupName,

                            AccessLevelID = card.AccessLevelID,
                            AccessLevelName = accesslevel.AccessLevelName,

                            AccessExpireDate = card.AccessExpireDate
                        };

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.CardNo.Contains(key) || n.CardNumber.Contains(key) || n.Plate1.Contains(key) || n.Plate2.Contains(key) || n.Plate3.Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(anotherkey))
            {
                query = query.Where(n => n.CardNo.Contains(anotherkey) || n.CardNumber.Contains(anotherkey) || n.Plate1.Contains(anotherkey) || n.Plate2.Contains(anotherkey) || n.Plate3.Contains(anotherkey));
            }

            if (!string.IsNullOrWhiteSpace(cardgroups))
            {
                query = query.Where(n => cardgroups.Contains(n.CardGroupId));
            }

            if (!string.IsNullOrWhiteSpace(customergroups))
            {
                query = query.Where(n => customergroups.Contains(n.CustomerGroupId));
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                var fdate = Convert.ToDateTime(fromdate);
                var tdate = Convert.ToDateTime(todate).AddDays(1);

                query = query.Where(n => n.ExpireDate >= fdate && n.ExpireDate < tdate);
            }

            var list = new PagedList<tblCardExtend>(query.OrderByDescending(n => n.AccessExpireDate), pageNumber, pageSize);

            return list;
        }

        public IEnumerable<tblCardExtend> GetAllByFirstForUploadLocker(string key, string anotherkey, string cardgroups, string customergroups, string fromdate, string todate)
        {
            var query = from card in _tblCardRepository.Table
                        join cardgroup in _tblCardGroupRepository.Table on card.CardGroupID equals cardgroup.CardGroupID.ToString() into card_cardgroup
                        from cardgroup in card_cardgroup.DefaultIfEmpty()
                        join customer in _tblCustomerRepository.Table on card.CustomerID equals customer.CustomerID.ToString() into card_customer
                        from customer in card_customer.DefaultIfEmpty()
                        join customergroup in _tblCustomerGroupRepository.Table on customer.CustomerGroupID equals customergroup.CustomerGroupID.ToString() into customer_group
                        from customergroup in customer_group.DefaultIfEmpty()
                        join accesslevel in _tblAccessLevelRepository.Table on card.AccessLevelID equals accesslevel.AccessLevelID.ToString() into card_accesslevel
                        from accesslevel in card_accesslevel.DefaultIfEmpty()

                        where card.IsDelete == false && card.IsLock == false

                        select new tblCardExtend()
                        {
                            CardID = card.CardID.ToString(),
                            CardNo = card.CardNo,
                            CardNumber = card.CardNumber,
                            ExpireDate = card.ExpireDate,
                            ImportDate = card.ImportDate,
                            Plate1 = card.Plate1,
                            Plate2 = card.Plate2,
                            Plate3 = card.Plate3,
                            VehicleName1 = card.VehicleName1,
                            VehicleName2 = card.VehicleName2,
                            VehicleName3 = card.VehicleName3,

                            CustomerId = customer.CustomerID.ToString(),
                            CustomerCode = customer.CustomerCode,
                            CustomerName = customer.CustomerName,

                            CustomerGroupId = customergroup.CustomerGroupID.ToString(),
                            CustomerGroupName = customergroup.CustomerGroupName,

                            CardGroupId = cardgroup.CardGroupID.ToString(),
                            CardGroupName = cardgroup.CardGroupName,

                            AccessLevelID = card.AccessLevelID,
                            AccessLevelName = accesslevel.AccessLevelName,

                            AccessExpireDate = card.AccessExpireDate
                        };

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.CardNo.Contains(key) || n.CardNumber.Contains(key) || n.Plate1.Contains(key) || n.Plate2.Contains(key) || n.Plate3.Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(anotherkey))
            {
                query = query.Where(n => n.CardNo.Contains(anotherkey) || n.CardNumber.Contains(anotherkey) || n.Plate1.Contains(anotherkey) || n.Plate2.Contains(anotherkey) || n.Plate3.Contains(anotherkey));
            }

            if (!string.IsNullOrWhiteSpace(cardgroups))
            {
                query = query.Where(n => cardgroups.Contains(n.CardGroupId));
            }

            if (!string.IsNullOrWhiteSpace(customergroups))
            {
                query = query.Where(n => customergroups.Contains(n.CustomerGroupId));
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                var fdate = Convert.ToDateTime(fromdate);
                var tdate = Convert.ToDateTime(todate).AddDays(1);

                query = query.Where(n => n.ExpireDate >= fdate && n.ExpireDate < tdate);
            }

            return query;
        }

        //dùng cho tủ đồ
        public List<tblCardCustomViewModel> GetAllPagingByFirstTSQL_Locker(string key, string cardgroups, string customerid, string customergroups, string fromdate, string todate, bool desc, string columnQuery, ref int total, string ischeckbytime = "0", int pageNumber = 1, int pageSize = 20, string accesslevelids = "", string active = "")
        {
            var sb = new StringBuilder();
            sb.AppendLine("SELECT * FROM (");
            sb.AppendLine(string.Format("Select ROW_NUMBER() OVER(ORDER BY c.{0} {1}) AS RowNumber,", columnQuery, desc ? "desc" : "asc"));
            sb.AppendLine("CONVERT(varchar(50), c.CardID) AS 'CardID',");
            sb.AppendLine("c.CardNo,");
            sb.AppendLine("c.CardNumber,");
            sb.AppendLine("c.CardGroupID,");
            sb.AppendLine("c.CustomerID,");
            sb.AppendLine("c.ImportDate,");
            sb.AppendLine("c.ExpireDate,");
            sb.AppendLine("c.AccessExpireDate,");
            sb.AppendLine("c.AccessLevelID,");
            sb.AppendLine("c.Plate1,");
            sb.AppendLine("c.Plate2,");
            sb.AppendLine("c.Plate3,");
            sb.AppendLine("c.VehicleName1,");
            sb.AppendLine("c.VehicleName2,");
            sb.AppendLine("c.VehicleName3,");
            sb.AppendLine("c.IsLock,");
            sb.AppendLine("cu.Description,");
            sb.AppendLine("cg.CardGroupName,");
            sb.AppendLine("cu.CustomerName,");
            sb.AppendLine("cu.CustomerCode,");
            sb.AppendLine("cu.Mobile AS 'CustomerMobile',");
            sb.AppendLine("cu.Address AS 'CustomerAddress',");
            sb.AppendLine("cu.IDNumber AS 'CustomerIDNumber',");
            sb.AppendLine("cu.CustomerGroupID,");
            sb.AppendLine("cug.CustomerGroupName,");

            //tủ đồ
            sb.AppendLine("(SELECT  ");
            sb.AppendLine("STUFF((SELECT '; ' + l.Name ");
            sb.AppendLine("FROM tblLocker l");
            sb.AppendLine("WHERE l.CardNumber = c.CardNumber");
            sb.AppendLine("FOR XML PATH('')), 1, 1, '') [LockerName]) as LockerName");

            sb.AppendLine("FROM tblCard c WITH(NOLOCK)");

            sb.AppendLine("LEFT JOIN tblCardGroup cg ON c.CardGroupID = CONVERT(varchar(50), cg.CardGroupID)");
            sb.AppendLine("LEFT JOIN tblCustomer cu ON c.CustomerID = CONVERT(varchar(50), cu.CustomerID)");
            sb.AppendLine("LEFT JOIN tblCustomerGroup cug ON cu.CustomerGroupID = CONVERT(varchar(50), cug.CustomerGroupID)");

            sb.AppendLine("WHERE 1=1 AND c.IsDelete = 0");

            //Điều kiện chính

            if (!string.IsNullOrWhiteSpace(key))
            {
                sb.AppendLine(string.Format("AND (c.CardNo LIKE '%{0}%' OR c.CardNumber LIKE '%{0}%' OR c.Plate1 LIKE '%{0}%' OR c.Plate2 LIKE '%{0}%' OR c.Plate3 LIKE '%{0}%' OR c.VehicleName1 LIKE N'%{0}%' OR c.VehicleName2 LIKE N'%{0}%' OR c.VehicleName3 LIKE N'%{0}%' OR cu.CustomerName LIKE N'%{0}%' OR cu.CustomerCode LIKE N'%{0}%' OR cu.Mobile LIKE '%{0}%' OR cu.Address LIKE '%{0}%')", key));
            }

            if (!string.IsNullOrWhiteSpace(cardgroups))
            {
                var arrCardGroup = cardgroups.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var attCG = string.Join(",", arrCardGroup);

                sb.AppendLine("AND c.CardGroupID IN (");

                sb.AppendLine(attCG);

                sb.AppendLine(")");
            }

            if (!string.IsNullOrWhiteSpace(customergroups))
            {
                var arrCustomerGroup = customergroups.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);



                var attCG = string.Join(",", arrCustomerGroup);

                sb.AppendLine("AND cu.CustomerGroupID IN (");

                sb.AppendLine(attCG);

                sb.AppendLine(")");
            }

            if (!string.IsNullOrWhiteSpace(customerid))
            {
                sb.AppendLine(string.Format("AND c.CustomerID = '{0}'", customerid));
            }

            if (!string.IsNullOrWhiteSpace(accesslevelids))
            {
                var arrAcLevel = accesslevelids.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var attAL = string.Join(",", arrAcLevel);

                sb.AppendLine("AND c.AccessLevelID IN (");

                sb.AppendLine(attAL);

                sb.AppendLine(")");
            }

            switch (ischeckbytime)
            {
                case "1"://Ngày nhập thẻ
                    if (!string.IsNullOrWhiteSpace(fromdate))
                    {
                        var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd");

                        sb.AppendLine(string.Format("AND c.ImportDate >= '{0}'", fdate));
                    }

                    if (!string.IsNullOrWhiteSpace(todate))
                    {
                        var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd");

                        //query = query.Where(n => n.ImportDate < tdate);
                        sb.AppendLine(string.Format("AND c.ImportDate < '{0}'", tdate));
                    }
                    break;
                case "2"://Ngày hết hạn
                    if (!string.IsNullOrWhiteSpace(fromdate))
                    {
                        var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd");

                        //query = query.Where(n => n.ExpireDate >= fdate);
                        sb.AppendLine(string.Format("AND c.AccessExpireDate >= '{0}'", fdate));
                    }

                    if (!string.IsNullOrWhiteSpace(todate))
                    {
                        var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd");

                        //query = query.Where(n => n.ExpireDate < tdate);
                        sb.AppendLine(string.Format("AND c.AccessExpireDate < '{0}'", tdate));
                    }
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrWhiteSpace(active))
            {
                switch (active)
                {
                    case "0":

                        sb.AppendLine(string.Format("AND c.IsLock = {0}", "0"));

                        break;
                    case "1":

                        sb.AppendLine(string.Format("AND c.IsLock = {0}", "1"));

                        break;
                    default:
                        break;
                }
            }

            sb.AppendLine(") as a");
            sb.AppendLine(string.Format("WHERE RowNumber BETWEEN (({0}-1) * {1} + 1) AND ({0} * {1})", pageNumber, pageSize));

            var listData = SqlExQuery<tblCardCustomViewModel>.ExcuteQuery(sb.ToString());

            //Tính tổng
            sb.Clear();
            sb.AppendLine("SELECT COUNT(*) TotalCount");

            sb.AppendLine("FROM tblCard c WITH(NOLOCK)");

            sb.AppendLine("LEFT JOIN tblCardGroup cg ON c.CardGroupID = CONVERT(varchar(50), cg.CardGroupID)");
            sb.AppendLine("LEFT JOIN tblCustomer cu ON c.CustomerID = CONVERT(varchar(50), cu.CustomerID)");
            sb.AppendLine("LEFT JOIN tblCustomerGroup cug ON cu.CustomerGroupID = CONVERT(varchar(50), cug.CustomerGroupID)");

            sb.AppendLine("WHERE 1=1 AND c.IsDelete = 0");

            //Điều kiện chính

            if (!string.IsNullOrWhiteSpace(key))
            {
                sb.AppendLine(string.Format("AND (c.CardNo LIKE '%{0}%' OR c.CardNumber LIKE '%{0}%' OR c.Plate1 LIKE '%{0}%' OR c.Plate2 LIKE '%{0}%' OR c.Plate3 LIKE '%{0}%' OR c.VehicleName1 LIKE N'%{0}%' OR c.VehicleName2 LIKE N'%{0}%' OR c.VehicleName3 LIKE N'%{0}%' OR cu.CustomerName LIKE N'%{0}%' OR cu.CustomerCode LIKE N'%{0}%' OR cu.Mobile LIKE '%{0}%' OR cu.Address LIKE '%{0}%')", key));
            }

            if (!string.IsNullOrWhiteSpace(cardgroups))
            {
                var arrCardGroup = cardgroups.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var attCG = string.Join(",", arrCardGroup);

                sb.AppendLine("AND c.CardGroupID IN (");

                sb.AppendLine(attCG);

                sb.AppendLine(")");
            }

            if (!string.IsNullOrWhiteSpace(customergroups))
            {
                var arrCustomerGroup = customergroups.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var attCG = string.Join(",", arrCustomerGroup);

                sb.AppendLine("AND cu.CustomerGroupID IN (");

                sb.AppendLine(attCG);

                sb.AppendLine(")");
            }

            if (!string.IsNullOrWhiteSpace(customerid))
            {
                sb.AppendLine(string.Format("AND c.CustomerID = '{0}'", customerid));
            }

            if (!string.IsNullOrWhiteSpace(accesslevelids))
            {
                var arrAcLevel = accesslevelids.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var attAL = string.Join(",", arrAcLevel);

                sb.AppendLine("AND c.AccessLevelID IN (");

                sb.AppendLine(attAL);

                sb.AppendLine(")");
            }

            switch (ischeckbytime)
            {
                case "1"://Ngày nhập thẻ
                    if (!string.IsNullOrWhiteSpace(fromdate))
                    {
                        var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd");

                        sb.AppendLine(string.Format("AND c.ImportDate >= '{0}'", fdate));
                    }

                    if (!string.IsNullOrWhiteSpace(todate))
                    {
                        var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd");

                        //query = query.Where(n => n.ImportDate < tdate);
                        sb.AppendLine(string.Format("AND c.ImportDate < '{0}'", tdate));
                    }
                    break;
                case "2"://Ngày hết hạn
                    if (!string.IsNullOrWhiteSpace(fromdate))
                    {
                        var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd");

                        //query = query.Where(n => n.ExpireDate >= fdate);
                        sb.AppendLine(string.Format("AND c.AccessExpireDate >= '{0}'", fdate));
                    }

                    if (!string.IsNullOrWhiteSpace(todate))
                    {
                        var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd");

                        //query = query.Where(n => n.ExpireDate < tdate);
                        sb.AppendLine(string.Format("AND c.AccessExpireDate < '{0}'", tdate));
                    }
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrWhiteSpace(active))
            {
                switch (active)
                {
                    case "0":

                        sb.AppendLine(string.Format("AND c.IsLock = {0}", "0"));

                        break;
                    case "1":

                        sb.AppendLine(string.Format("AND c.IsLock = {0}", "1"));

                        break;
                    default:
                        break;
                }
            }

            var _total = SqlExQuery<TotalPaging>.ExcuteQueryFirst(sb.ToString());
            total = _total != null ? _total.TotalCount : 0;

            return listData;
        }

        public List<tblLockerCardExcel> GetLockerExcelCardByFirstTSQL(string key, string cardgroups, string customerid, string customergroups, string fromdate, string todate, bool desc, string columnQuery, string ischeckbytime = "0", string accesslevelids = "", string active = "")
        {
            var sb = new StringBuilder();
            sb.AppendLine("SELECT * FROM (");
            sb.AppendLine("SELECT CONVERT(varchar(50), c.CardID) AS 'CardID',");
            sb.AppendLine("c.CardNo,");
            sb.AppendLine("c.CardNumber,");
            sb.AppendLine("c.CardGroupID,");
            sb.AppendLine("c.CustomerID,");
            sb.AppendLine("c.ImportDate,");
            sb.AppendLine("c.ExpireDate,");
            sb.AppendLine("c.AccessExpireDate,");
            sb.AppendLine("c.AccessLevelID,");
            sb.AppendLine("c.Plate1,");
            sb.AppendLine("c.Plate2,");
            sb.AppendLine("c.Plate3,");
            sb.AppendLine("c.VehicleName1,");
            sb.AppendLine("c.VehicleName2,");
            sb.AppendLine("c.VehicleName3,");
            sb.AppendLine("c.IsLock,");
            sb.AppendLine("cu.Description,");
            sb.AppendLine("cg.CardGroupName,");
            sb.AppendLine("cu.CustomerName,");
            sb.AppendLine("cu.CustomerCode,");
            sb.AppendLine("cu.Mobile AS 'CustomerMobile',");
            sb.AppendLine("cu.Address AS 'CustomerAddress',");
            sb.AppendLine("cu.IDNumber AS 'CustomerIDNumber',");
            sb.AppendLine("cu.CustomerGroupID,");
            sb.AppendLine("cug.CustomerGroupName,");

            //tủ đồ
            sb.AppendLine("(SELECT  ");
            sb.AppendLine("STUFF((SELECT '; ' + l.Name ");
            sb.AppendLine("FROM tblLocker l");
            sb.AppendLine("WHERE l.CardNumber = c.CardNumber");
            sb.AppendLine("FOR XML PATH('')), 1, 1, '') [LockerName]) as LockerName");

            sb.AppendLine("FROM tblCard c WITH(NOLOCK)");

            sb.AppendLine("LEFT JOIN tblCardGroup cg ON c.CardGroupID = CONVERT(varchar(50), cg.CardGroupID)");
            sb.AppendLine("LEFT JOIN tblCustomer cu ON c.CustomerID = CONVERT(varchar(50), cu.CustomerID)");
            sb.AppendLine("LEFT JOIN tblCustomerGroup cug ON cu.CustomerGroupID = CONVERT(varchar(50), cug.CustomerGroupID)");

            sb.AppendLine("WHERE 1=1 AND c.IsDelete = 0");

            //Điều kiện chính

            if (!string.IsNullOrWhiteSpace(key))
            {
                sb.AppendLine(string.Format("AND (c.CardNo LIKE '%{0}%' OR c.CardNumber LIKE '%{0}%' OR c.Plate1 LIKE '%{0}%' OR c.Plate2 LIKE '%{0}%' OR c.Plate3 LIKE '%{0}%' OR c.VehicleName1 LIKE N'%{0}%' OR c.VehicleName2 LIKE N'%{0}%' OR c.VehicleName3 LIKE N'%{0}%' OR cu.CustomerName LIKE N'%{0}%' OR cu.CustomerCode LIKE N'%{0}%' OR cu.Mobile LIKE '%{0}%' OR cu.Address LIKE '%{0}%')", key));
            }

            if (!string.IsNullOrWhiteSpace(cardgroups))
            {
                var arrCardGroup = cardgroups.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var attCG = string.Join(",", arrCardGroup);

                sb.AppendLine("AND c.CardGroupID IN (");

                sb.AppendLine(attCG);

                sb.AppendLine(")");
            }

            if (!string.IsNullOrWhiteSpace(customergroups))
            {
                var arrCustomerGroup = customergroups.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var attCG = string.Join(",", arrCustomerGroup);

                sb.AppendLine("AND cu.CustomerGroupID IN (");

                sb.AppendLine(attCG);

                sb.AppendLine(")");
            }

            if (!string.IsNullOrWhiteSpace(customerid))
            {
                sb.AppendLine(string.Format("AND c.CustomerID = '{0}'", customerid));
            }

            if (!string.IsNullOrWhiteSpace(accesslevelids))
            {
                var arrAcLevel = accesslevelids.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var attAL = string.Join(",", arrAcLevel);

                sb.AppendLine("AND c.AccessLevelID IN (");

                sb.AppendLine(attAL);

                sb.AppendLine(")");
            }

            switch (ischeckbytime)
            {
                case "1"://Ngày nhập thẻ
                    if (!string.IsNullOrWhiteSpace(fromdate))
                    {
                        var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd");

                        sb.AppendLine(string.Format("AND c.ImportDate >= '{0}'", fdate));
                    }

                    if (!string.IsNullOrWhiteSpace(todate))
                    {
                        var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd");

                        //query = query.Where(n => n.ImportDate < tdate);
                        sb.AppendLine(string.Format("AND c.ImportDate < '{0}'", tdate));
                    }
                    break;
                case "2"://Ngày hết hạn
                    if (!string.IsNullOrWhiteSpace(fromdate))
                    {
                        var fdate = Convert.ToDateTime(fromdate);

                        //query = query.Where(n => n.ExpireDate >= fdate);
                        sb.AppendLine(string.Format("AND c.ExpireDate >= '{0}'", fdate));
                    }

                    if (!string.IsNullOrWhiteSpace(todate))
                    {
                        var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd");

                        //query = query.Where(n => n.ExpireDate < tdate);
                        sb.AppendLine(string.Format("AND c.ExpireDate < '{0}'", tdate));
                    }
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrWhiteSpace(active))
            {
                switch (active)
                {
                    case "0":

                        sb.AppendLine(string.Format("AND c.IsLock = {0}", "0"));

                        break;
                    case "1":

                        sb.AppendLine(string.Format("AND c.IsLock = {0}", "1"));

                        break;
                    default:
                        break;
                }
            }

            sb.AppendLine(") as a");

            var listData = SqlExQuery<tblCardCustomViewModel>.ExcuteQuery(sb.ToString());

            var listLevel = _tblAccessLevelService.GetAllActive().ToList();

            var list = new List<tblLockerCardExcel>();

            if (listData.Any())
            {
                var count = 0;
                foreach (var item in listData)
                {
                    var levelname = listLevel.FirstOrDefault(n => n.AccessLevelID.ToString() == item.AccessLevelID);

                    count++;
                    var o = new tblLockerCardExcel()
                    {
                        Address = item.CustomerAddress,
                        CardGroupName = item.CardGroupName,
                        CardNo = item.CardNo,
                        CardNumber = item.CardNumber,
                        CMT = item.CustomerIDNumber,
                        CustomerCode = item.CustomerCode,
                        CustomerGroupName = item.CustomerGroupName,
                        CustomerName = item.CustomerName,
                        DateCreated = Convert.ToDateTime(item.ImportDate).ToString("dd/MM/yyyy"),
                        DateExpire = Convert.ToDateTime(item.ExpireDate).ToString("dd/MM/yyyy"),
                        Inactive = item.IsLock ? "Khóa" : "Hoạt động",
                        NumberRow = count,
                        SĐT = item.CustomerMobile,
                        LockerName = item.LockerName
                    };


                    list.Add(o);
                }
            }

            return list;
        }

        public void AutoTakePhoto(string strCards)
        {
            var sb = new StringBuilder();
            sb.AppendLine("update tblCard");
            sb.AppendLine("Set isAutoCapture = (CASE");
            sb.AppendLine("    WHEN isAutoCapture = 'True' THEN 'False' ");
            sb.AppendLine("    ELSE 'True'");
            sb.AppendLine("END)");
            sb.AppendLine(string.Format("where CardNumber IN ({0})",strCards));
            ExcuteSQL.Execute(sb.ToString());
        }    

        public List<string> GetListCardNumber(string key, string cardgroups, string customergroups, string fromdate, string todate,string ischeckbytime = "0", string active = "", bool isfindautocapture = false)
        {
            var sb = new StringBuilder();
          
            sb.AppendLine(string.Format("Select c.CardNumber"));        
            sb.AppendLine("FROM tblCard c WITH(NOLOCK)");

            sb.AppendLine("LEFT JOIN tblCardGroup cg ON c.CardGroupID = CONVERT(varchar(50), cg.CardGroupID)");
            sb.AppendLine("LEFT JOIN tblCustomer cu ON c.CustomerID = CONVERT(varchar(50), cu.CustomerID)");
            sb.AppendLine("LEFT JOIN tblCustomerGroup cug ON cu.CustomerGroupID = CONVERT(varchar(50), cug.CustomerGroupID)");

            sb.AppendLine("WHERE 1=1 AND c.IsDelete = 0");

            if (isfindautocapture)
            {
                sb.AppendLine(" AND c.isAutoCapture = 1");
            }

            //Điều kiện chính

            if (!string.IsNullOrWhiteSpace(key))
            {
                sb.AppendLine(string.Format("AND (c.CardNo LIKE '%{0}%' OR c.CardNumber LIKE '%{0}%' OR c.Plate1 LIKE '%{0}%' OR c.Plate2 LIKE '%{0}%' OR c.Plate3 LIKE '%{0}%' OR c.VehicleName1 LIKE N'%{0}%' OR c.VehicleName2 LIKE N'%{0}%' OR c.VehicleName3 LIKE N'%{0}%' OR cu.CustomerName LIKE N'%{0}%' OR cu.CustomerCode LIKE N'%{0}%' OR cu.Mobile LIKE '%{0}%' OR cu.Address LIKE '%{0}%')", key));
            }

            if (!string.IsNullOrWhiteSpace(cardgroups))
            {
                var arrCardGroup = cardgroups.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var attCG = string.Join(",", arrCardGroup);

                sb.AppendLine("AND c.CardGroupID IN (");

                sb.AppendLine(attCG);

                sb.AppendLine(")");
            }

            if (!string.IsNullOrWhiteSpace(AuthCardGroupIds))
            {
                var t = AuthCardGroupIds.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    sb.AppendLine("AND c.CardGroupID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        sb.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    sb.AppendLine(" )");
                }
            }

            if (!string.IsNullOrWhiteSpace(customergroups))
            {
                var arrCustomerGroup = customergroups.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                var attCG = string.Join(",", arrCustomerGroup);

                sb.AppendLine("AND cu.CustomerGroupID IN (");

                sb.AppendLine(attCG);

                sb.AppendLine(")");
            }        

            switch (ischeckbytime)
            {
                case "1"://Ngày nhập thẻ
                    if (!string.IsNullOrWhiteSpace(fromdate))
                    {
                        var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd");

                        sb.AppendLine(string.Format("AND c.ImportDate >= '{0}'", fdate));
                    }

                    if (!string.IsNullOrWhiteSpace(todate))
                    {
                        var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd");

                        //query = query.Where(n => n.ImportDate < tdate);
                        sb.AppendLine(string.Format("AND c.ImportDate < '{0}'", tdate));
                    }
                    break;
                case "2"://Ngày hết hạn
                    if (!string.IsNullOrWhiteSpace(fromdate))
                    {
                        var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd");

                        //query = query.Where(n => n.ExpireDate >= fdate);
                        sb.AppendLine(string.Format("AND c.ExpireDate >= '{0}'", fdate));
                    }

                    if (!string.IsNullOrWhiteSpace(todate))
                    {
                        var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd");

                        //query = query.Where(n => n.ExpireDate < tdate);
                        sb.AppendLine(string.Format("AND c.ExpireDate < '{0}'", tdate));
                    }
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrWhiteSpace(active))
            {
                switch (active)
                {
                    case "0":

                        sb.AppendLine(string.Format("AND c.IsLock = {0}", "0"));

                        break;
                    case "1":

                        sb.AppendLine(string.Format("AND c.IsLock = {0}", "1"));

                        break;
                    default:
                        break;
                }
            }   

            var listData = SqlExQuery<string>.ExcuteQuery(sb.ToString());

            return listData;
        }

        public tblCard GetCardByCardNumberOrCardNo(string key)
        {
            var query = from n in _tblCardRepository.Table
                        where /*n.IsLock == false &&*/ (n.CardNumber == key || n.CardNo == key) && n.IsDelete == false
                        //orderby n.CardNo ascending
                        select n;

            return query.FirstOrDefault();
        }

        public tblCard GetByCardNumber_Id(string cardnumber)
        {
            var query = from n in _tblCardRepository.Table
                        where n.IsLock == false && n.CardNumber == cardnumber && n.IsDelete == false
                        select n;

            return query.FirstOrDefault();
        }

        public List<string> GetCard(string key, string anotherkey, string cardgroups, string customergroups, string fromdate, string todate, string accesslevelids = "")
        {
            var query = from card in _tblCardRepository.Table
                        join cardgroup in _tblCardGroupRepository.Table on card.CardGroupID equals cardgroup.CardGroupID.ToString() into card_cardgroup
                        from cardgroup in card_cardgroup.DefaultIfEmpty()
                        join customer in _tblCustomerRepository.Table on card.CustomerID equals customer.CustomerID.ToString() into card_customer
                        from customer in card_customer.DefaultIfEmpty()
                        join customergroup in _tblCustomerGroupRepository.Table on customer.CustomerGroupID equals customergroup.CustomerGroupID.ToString() into customer_group
                        from customergroup in customer_group.DefaultIfEmpty()
                        join accesslevel in _tblAccessLevelRepository.Table on card.AccessLevelID equals accesslevel.AccessLevelID.ToString() into card_accesslevel
                        from accesslevel in card_accesslevel.DefaultIfEmpty()

                        where cardgroup.CardType == 0 && card.IsDelete == false && card.IsLock == false

                        select new tblCardExtend()
                        {
                            CardID = card.CardID.ToString(),
                            CardNo = card.CardNo,
                            CardNumber = card.CardNumber,
                            ExpireDate = card.ExpireDate,
                            ImportDate = card.ImportDate,
                            Plate1 = card.Plate1,
                            Plate2 = card.Plate2,
                            Plate3 = card.Plate3,
                            VehicleName1 = card.VehicleName1,
                            VehicleName2 = card.VehicleName2,
                            VehicleName3 = card.VehicleName3,

                            CustomerId = customer.CustomerID.ToString(),
                            CustomerCode = customer.CustomerCode,
                            CustomerName = customer.CustomerName,

                            CustomerGroupId = customergroup.CustomerGroupID.ToString(),
                            CustomerGroupName = customergroup.CustomerGroupName,

                            CardGroupId = cardgroup.CardGroupID.ToString(),
                            CardGroupName = cardgroup.CardGroupName,

                            AccessLevelID = card.AccessLevelID,
                            AccessLevelName = accesslevel.AccessLevelName,

                            AccessExpireDate = card.AccessExpireDate,
                            Address = customer.Address,
                            AddressUnsign = customer.AddressUnsign
                        };

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.CardNo.Contains(key) || n.CardNumber.Contains(key) || n.Plate1.Contains(key) || n.Plate2.Contains(key) || n.Plate3.Contains(key) || n.CustomerCode.Contains(key) || n.CustomerName.Contains(key) || n.Address.Contains(key) || n.AddressUnsign.Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(anotherkey))
            {
                query = query.Where(n => n.CardNo.Contains(anotherkey) || n.CardNumber.Contains(anotherkey) || n.Plate1.Contains(anotherkey) || n.Plate2.Contains(anotherkey) || n.Plate3.Contains(anotherkey) || n.CustomerCode.Contains(anotherkey) || n.CustomerName.Contains(anotherkey) || n.Address.Contains(anotherkey) || n.AddressUnsign.Contains(anotherkey));
            }

            if (!string.IsNullOrWhiteSpace(cardgroups))
            {
                query = query.Where(n => cardgroups.Contains(n.CardGroupId));
            }

            if (!string.IsNullOrEmpty(AuthCardGroupIds))
            {
                query = query.Where(n => AuthCardGroupIds.Contains(n.CardGroupId));
            }

            if (!string.IsNullOrWhiteSpace(customergroups))
            {
                query = query.Where(n => customergroups.Contains(n.CustomerGroupId));
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                var fdate = Convert.ToDateTime(fromdate);
                var tdate = Convert.ToDateTime(todate).AddDays(1);

                query = query.Where(n => n.ExpireDate >= fdate && n.ExpireDate < tdate);
            }

            if (!string.IsNullOrWhiteSpace(accesslevelids))
            {
                query = query.Where(n => accesslevelids.Contains(n.AccessLevelID));
            }



            return query.Select(n => n.CardNumber).ToList();
        }

        public bool AddCardExpireByListCardNumber_V2(string listCardNumber, int _feelevel, string _newexpire, string userId, bool chbEnableMinusActive, bool chbEnableDateActive = false, string dateactive = "",string subid = "")
        {
            var sb = new StringBuilder();
            sb.AppendLine("INSERT INTO tblActiveCard(Code, [Date], CardNumber, CardNo, Plate, OldExpireDate, [Days], NewExpireDate, CardGroupID, CustomerGroupID, UserID, FeeLevel, CustomerID,IsDelete,SubId)");
            sb.AppendLine("SELECT CASE WHEN cus.CustomerCode IS NULL THEN '0' ELSE cus.CustomerCode END,GETDATE(), ca.Cardnumber,ca.CardNo");
            sb.AppendLine(", CAST(CASE WHEN ca.Plate2 <> '' THEN ISNULL(ca.Plate1,'') + ';' + ISNULL(ca.Plate2,'') WHEN ca.Plate3 <> '' THEN ISNULL(ca.Plate1,'') + ';' + ISNULL(ca.Plate2,'') + ';' + ISNULL(ca.Plate3,'') WHEN ca.Plate1 IS NULL THEN '' ELSE ca.Plate1 END AS nvarchar(50)) as Plate");
            sb.AppendLine(string.Format(", ca.[ExpireDate], DATEDIFF(DAY, ca.[ExpireDate], '{0}')", _newexpire));
            sb.AppendLine(string.Format(", '{0}', ca.CardGroupID, CASE WHEN  cus.CustomerGroupID IS NULL THEN '0' ELSE cus.CustomerGroupID END,'{2}','{1}', CASE WHEN ca.CustomerID IS NULL THEN '0' ELSE ca.CustomerID END,0,'{3}'", _newexpire, _feelevel, userId, subid));
            sb.AppendLine("from tblCard ca");
            sb.AppendLine("LEFT join tblCustomer cus on ca.CustomerID = CONVERT(varchar(255), cus.CustomerID)");
            sb.AppendLine("where ca.IsDelete = 0 and ca.IsLock=0");
            //Neu so ngay gia han <0 va neu ko check thi ko cho gia han
            if (chbEnableMinusActive == false)
            {
                sb.AppendLine(string.Format("and DATEDIFF(DAY, ca.[ExpireDate], '{0}') >=0  AND ca.[ExpireDate] <= '{0}'", _newexpire));
            }

            if (!string.IsNullOrWhiteSpace(listCardNumber))
            {
                //where in
                sb.AppendLine(string.Format(" and ca.CardNumber IN ({0})", listCardNumber));
            }

            //Update lai bang tblCard
            sb.AppendLine("UPDATE");
            sb.AppendLine("ca");
            sb.AppendLine(string.Format("SET ca.ExpireDate = '{0}'", _newexpire));

            if (chbEnableDateActive)
            {
                sb.AppendLine(string.Format(", ca.DateActive = '{0}'", dateactive));
            }

            sb.AppendLine("FROM");
            sb.AppendLine("tblCard AS ca");
            sb.AppendLine("LEFT join tblCustomer c on ca.CustomerID = CONVERT(varchar(255), c.CustomerID)");
            sb.AppendLine("WHERE IsDelete = 0 and ca.IsLock=0 ");

            if (chbEnableMinusActive == false)
            {
                sb.AppendLine(string.Format("and DATEDIFF(DAY, ca.[ExpireDate], '{0}') >=0  AND ca.[ExpireDate] <= '{0}'", _newexpire));
            }
            if (!string.IsNullOrWhiteSpace(listCardNumber))
            {
                //where in
                sb.AppendLine(string.Format(" and ca.CardNumber IN ({0})", listCardNumber));
            }

            if (chbEnableDateActive)
            {
                sb.AppendLine("insert into tblCardProcess(Date, Actions, CardGroupID, UserID, CustomerID, CardNumber)");
                sb.AppendLine(string.Format("SELECT GETDATE(),'ACTIVE', ca.CardGroupID,  '{0}', CASE WHEN ca.CustomerID IS NULL THEN '0' ELSE ca.CustomerID END , ca.Cardnumber", userId));
                sb.AppendLine("from tblCard ca");
                sb.AppendLine("LEFT join tblCustomer cus on ca.CustomerID = CONVERT(varchar(255), cus.CustomerID)");
                sb.AppendLine("where ca.IsDelete = 0 and ca.IsLock=0");
                //Neu so ngay gia han <0 va neu ko check thi ko cho gia han
                if (chbEnableMinusActive == false)
                {
                    sb.AppendLine(string.Format("and DATEDIFF(DAY, ca.[ExpireDate], '{0}') >=0  AND ca.[ExpireDate] <= '{0}'", _newexpire));
                }

                if (!string.IsNullOrWhiteSpace(listCardNumber))
                {
                    //where in
                    sb.AppendLine(string.Format(" and ca.CardNumber IN ({0})", listCardNumber));
                }
            }

            return ExcuteSQL.Execute(sb.ToString());
        }

        public List<tblCardExtend> GetAllPagingByFirst_SQL(string key, string anotherkey, string cardgroups, string customergroups, string fromdate, string todate, ref int total, int pageNumber = 1, int pageSize = 20, string accesslevelids = "")
        {           
            var query = new StringBuilder();

            #region Danh sách
            query.AppendLine("SELECT * FROM(");
            query.AppendLine("select ROW_NUMBER() OVER(ORDER BY c.[CardNo] asc) as RowNumber, c.CardID,c.CardNo,c.CardNumber,");
            query.AppendLine("c.ExpireDate,c.ImportDate,c.Plate1,c.Plate2,c.Plate3,");
            query.AppendLine("c.VehicleName1,c.VehicleName2,c.VehicleName3,c.AccessLevelID,c.AccessExpireDate,");
            query.AppendLine("cus.CustomerID,cus.CustomerCode,cus.CustomerName,cus.Address,cus.AddressUnsign,");
            query.AppendLine("cusg.CustomerGroupID,cusg.CustomerGroupName,");
            query.AppendLine("cg.CardGroupID,cg.CardGroupName,al.AccessLevelName ");
            query.AppendLine("from tblCard c");
            query.AppendLine("left join tblCardGroup cg on c.CardGroupID = CONVERT(varchar(50),cg.CardGroupID)");
            query.AppendLine("left join tblCustomer cus on c.CustomerID = CONVERT(varchar(50),cus.CustomerID)");
            query.AppendLine("left join tblCustomerGroup cusg on cus.CustomerGroupID = CONVERT(varchar(50),cusg.CustomerGroupID)");
            query.AppendLine("left join tblAccessLevel al on c.AccessLevelID = CONVERT(varchar(50),al.AccessLevelID)");
            query.AppendLine("where /*cg.CardType = 0 and*/ c.IsDelete = 'false' and c.IsLock = 'false'");

            if (!string.IsNullOrWhiteSpace(key))
            {
                query.AppendLine(string.Format("and (c.CardNumber LIKE '%{0}%' or c.CardNo LIKE '%{0}%' or c.Plate1 LIKE '%{0}%' or c.Plate2 LIKE '%{0}%' or c.Plate3 LIKE '%{0}%' or cus.CustomerCode LIKE '%{0}%' or cus.CustomerName LIKE N'%{0}%' or cus.Address LIKE N'%{0}%' or cus.AddressUnsign LIKE N'%{0}%')", key));
            }

            if (!string.IsNullOrWhiteSpace(anotherkey))
            {
                query.AppendLine(string.Format("and (c.CardNumber LIKE '%{0}%' or c.CardNo LIKE '%{0}%' or c.Plate1 LIKE '%{0}%' or c.Plate2 LIKE '%{0}%' or c.Plate3 LIKE '%{0}%' or cus.CustomerCode LIKE '%{0}%' or cus.CustomerName LIKE N'%{0}%' or cus.Address LIKE N'%{0}%' or cus.AddressUnsign LIKE N'%{0}%')", anotherkey));
            }

            if (!string.IsNullOrWhiteSpace(cardgroups))
            {
                query.AppendLine(string.Format("and c.CardGroupID = '{0}'", cardgroups));
            }



            if (!string.IsNullOrWhiteSpace(AuthCardGroupIds))
            {
                var t = AuthCardGroupIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
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

            if (!string.IsNullOrWhiteSpace(customergroups))
            {
                var t = customergroups.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
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

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd HH:mm:ss");
                var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd HH:mm:ss");
                query.AppendLine(string.Format("and c.ExpireDate >= '{0}' and c.ExpireDate <= '{1}'", fdate, tdate));
            }

            if (!string.IsNullOrWhiteSpace(accesslevelids))
            {
                var t = accesslevelids.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and c.AccessLevelID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            query.AppendLine(") as C1");
            query.AppendLine(string.Format("WHERE RowNumber BETWEEN (({0}-1) * {1} + 1) AND ({0} * {1})", pageNumber, pageSize));

            query.AppendLine("select count(c.CardID) as totalCount");
            query.AppendLine("from tblCard c");
            query.AppendLine("left join tblCardGroup cg on c.CardGroupID = CONVERT(varchar(50),cg.CardGroupID)");
            query.AppendLine("left join tblCustomer cus on c.CustomerID = CONVERT(varchar(50),cus.CustomerID)");
            query.AppendLine("left join tblCustomerGroup cusg on cus.CustomerGroupID = CONVERT(varchar(50),cusg.CustomerGroupID)");
            query.AppendLine("left join tblAccessLevel al on c.AccessLevelID = CONVERT(varchar(50),al.AccessLevelID)");
            query.AppendLine("where /*cg.CardType = 0 OR  and*/ c.IsDelete = 'false' and c.IsLock = 'false'");

            if (!string.IsNullOrWhiteSpace(key))
            {
                query.AppendLine(string.Format("and (c.CardNumber LIKE '%{0}%' or c.CardNo LIKE '%{0}%' or c.Plate1 LIKE '%{0}%' or c.Plate2 LIKE '%{0}%' or c.Plate3 LIKE '%{0}%' or cus.CustomerCode LIKE '%{0}%' or cus.CustomerName LIKE N'%{0}%' or cus.Address LIKE N'%{0}%' or cus.AddressUnsign LIKE N'%{0}%')", key));
            }

            if (!string.IsNullOrWhiteSpace(anotherkey))
            {
                query.AppendLine(string.Format("and (c.CardNumber LIKE '%{0}%' or c.CardNo LIKE '%{0}%' or c.Plate1 LIKE '%{0}%' or c.Plate2 LIKE '%{0}%' or c.Plate3 LIKE '%{0}%' or cus.CustomerCode LIKE '%{0}%' or cus.CustomerName LIKE N'%{0}%' or cus.Address LIKE N'%{0}%' or cus.AddressUnsign LIKE N'%{0}%')", anotherkey));
            }

            if (!string.IsNullOrWhiteSpace(cardgroups))
            {
                query.AppendLine(string.Format("and c.CardGroupID = '{0}'", cardgroups));
            }

            if (!string.IsNullOrWhiteSpace(AuthCardGroupIds))
            {
                var t = AuthCardGroupIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
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

            if (!string.IsNullOrWhiteSpace(customergroups))
            {
                var t = customergroups.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
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

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                var fdate = Convert.ToDateTime(fromdate).ToString("yyyy/MM/dd HH:mm:ss");
                var tdate = Convert.ToDateTime(todate).AddDays(1).ToString("yyyy/MM/dd HH:mm:ss");
                query.AppendLine(string.Format("and c.ExpireDate >= '{0}' and c.ExpireDate <= '{1}'", fdate, tdate));
            }

            if (!string.IsNullOrWhiteSpace(accesslevelids))
            {
                var t = accesslevelids.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and c.AccessLevelID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }
            #endregion



            var list = ExcuteSQL.GetDataSet(query.ToString(), false);
            total = list.Tables.Count > 1 ? Convert.ToInt32(list.Tables[1].Rows[0]["totalCount"].ToString()) : 0;
         

            return ExcuteSQL.ConvertTo<tblCardExtend>(list.Tables[0]);
        }

        public IEnumerable<tblCard> GetCardByCardNumbers(string cardnumbers)
        {
            var query = from n in _tblCardRepository.Table
                        where cardnumbers.Contains(n.CardNumber) && n.IsDelete == false
                        select n;

            return query;
        }

        public List<string> GetCustomCard_Customer(string key, string cardgr ,string cus)
        {
            var query = new StringBuilder();
            query.AppendLine("select * from tblCard c");
            query.AppendLine("left join tblCustomer cu On CONVERT(varchar(50), cu.CustomerID) = c.CustomerID");
            query.AppendLine("LEFT JOIN tblCardGroup cg ON c.CardGroupID = CONVERT(varchar(50), cg.CardGroupID)"); 
            query.AppendLine("LEFT JOIN tblCustomerGroup cug ON cu.CustomerGroupID = CONVERT(varchar(50), cug.CustomerGroupID)");
            query.AppendLine("WHERE 1=1 AND c.IsDelete = 0");
           
            if (!string.IsNullOrWhiteSpace(key))
            {
                query.AppendLine(string.Format("AND (c.CardNo LIKE '%{0}%' OR c.CardNumber LIKE '%{0}%' OR c.Plate1 LIKE '%{0}%' OR c.Plate2 LIKE '%{0}%' OR c.Plate3 LIKE '%{0}%' OR c.VehicleName1 LIKE N'%{0}%' OR c.VehicleName2 LIKE N'%{0}%' OR c.VehicleName3 LIKE N'%{0}%' OR cu.CustomerName LIKE N'%{0}%' OR cu.CustomerCode LIKE N'%{0}%' OR cu.Mobile LIKE '%{0}%' OR cu.Address LIKE '%{0}%')", key));
            }

            if (!string.IsNullOrWhiteSpace(cardgr))
            {
                //cardgr.Replace(",", "");
                var arrCardGroup = cardgr.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                
            
                var attCG = string.Join(",", arrCardGroup);

                query.AppendLine("AND c.CardGroupID IN (");

                query.AppendLine(attCG);

                query.AppendLine(")");
            }
            if (!string.IsNullOrWhiteSpace(cus))
            {
                //var arrCustomerGroup = cus.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                //var attCG = string.Join(",", arrCustomerGroup);

                query.AppendLine(string.Format("AND cu.CustomerGroupID = '{0}'",cus));

                //query.AppendLine(attCG);

                //query.AppendLine(")");
            }
            var listData = SqlExQuery<tblCardCustomViewModel1>.ExcuteQuery(query.ToString());
            var lst = new List<string>();
            var str = "";
            foreach (var item in listData)
            {
                str = item.CardNumber ;
            lst.Add(str);
            }
            return lst;
        }

        public tblCard GetCardByCardNo(string key)
        {
            var query = from n in _tblCardRepository.Table
                        where key == n.CardNo 
                        select n;

            return query.FirstOrDefault();
        }
    }

}
