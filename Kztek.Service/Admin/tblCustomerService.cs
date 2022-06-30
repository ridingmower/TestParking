using Kztek.Data.Infrastructure;
using Kztek.Data.Repository;
using Kztek.Data.SqlHelper;
using Kztek.Model.CustomModel;
using Kztek.Model.CustomModel.iParking;
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
    public interface ItblCustomerService
    {
        IEnumerable<tblCustomer> GetAll();

        IEnumerable<tblCustomer> GetAllActive();

        IEnumerable<tblCustomer> GetAllActiveByKey(string key);

        IEnumerable<tblCustomer> GetAllActiveByKeyMaxTake(string key, int max);

        List<tblCustomer> GetAllByListId(List<string> ids);

        IEnumerable<tblCustomerExtend> GetAllActiveByListIdForUpload(string ids);

        IEnumerable<tblCustomerExtend> GetAllActiveByListIdForUpload(List<string> ids);

        IPagedList<tblCustomer> GetAllPagingByFirst(string key, string customergroup, int pageNumber, int pageSize, string customerstatus = "");

        IPagedList<tblCustomerExtend> GetAllPagingByFirstForUpload(string key, string anotherkey, string customergroups, string fromdate, string todate, int pageNumber = 1, int pageSize = 20, string accesslevelids = "");

        List<tblCustomerExtend> GetAllByFirst(string key, string anotherkey, string customergroups, string fromdate, string todate, string accesslevelids = "");

        List<tblCustomerExtend> GetAllByFirst(List<tblCustomerExtend> session);

        List<tblCustomer_Excel> ExcelAllByFirst(string key, string customergroup, string customerstatus = "");
        List<tblCustomer_ExcelTRANSERCO> ExcelAllByFirstTRANSERCO(string key, string customergroup, string customerstatus = "");

        tblCustomer GetById(Guid id);

        tblCustomer GetByName(string name);

        tblCustomer GetByCode(string code);
        tblCustomer GetByDevPass(string DevPass);

        tblCustomerSubmit GetCustomByCode(string code);

        tblCustomer GetByCode_Id(string code, string id);
        tblCustomer GetByFingerID(int id);

        MessageReport Create(tblCustomer obj);

        MessageReport Update(tblCustomer obj);

        MessageReport DeleteById(string id, ref tblCustomer obj);

        MessageReport UpdateAuthorizeCustomer(string key, string customergroup, string levelid, string customerstatus = "");

        MessageReport UpdateCustomer(string useroffinger, string dateN, bool checkuse = false);

        IEnumerable<tblCustomerExtend> GetAllByFirstForUpload(string key, string anotherkey, string customergroups, string fromdate, string todate, string accesslevelids = "");
        List<tblCustomer> GetAllActiveByKeySQL(string key, int max);
        tblCustomer GetByCardNumber(string cardNumber);
        List<tblCardCustom> GetCustomerJoinCard();
    }

    public class tblCustomerService : ItblCustomerService
    {
        private ItblCardService _tblCardService;
        private ItblCardRepository _tblCardRepository;
        private ItblCustomerRepository _tblCustomerRepository;
        private IUnitOfWork _UnitOfWork;
        private ItblCustomerGroupService _tblCustomerGroupService;
        private ItblCustomerGroupRepository _tblCustomerGroupRepository;
        private ItblAccessLevelRepository _tblAccessLevelRepository;
        private ItblCompartmentService _CompartmentService;
        public tblCustomerService(ItblCardRepository _tblCardRepository,ItblCardService _tblCardService,ItblCompartmentService _CompartmentService, ItblCustomerRepository _tblCustomerRepository, IUnitOfWork _UnitOfWork, ItblCustomerGroupService _tblCustomerGroupService, ItblCustomerGroupRepository _tblCustomerGroupRepository, ItblAccessLevelRepository _tblAccessLevelRepository)
        {
            this._tblCardRepository = _tblCardRepository;
            this._tblCustomerRepository = _tblCustomerRepository;
            this._tblCardService = _tblCardService;
            this._UnitOfWork = _UnitOfWork;
            this._tblCustomerGroupService = _tblCustomerGroupService;
            this._tblCustomerGroupRepository = _tblCustomerGroupRepository;
            this._tblAccessLevelRepository = _tblAccessLevelRepository;
            this._CompartmentService = _CompartmentService;
        }

        private void Save()
        {
            _UnitOfWork.Commit();
        }

        public MessageReport Create(tblCustomer obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblCustomerRepository.Add(obj);

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

        public MessageReport DeleteById(string id, ref tblCustomer obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                obj = GetById(Guid.Parse(id));
                if (obj != null)
                {
                    _tblCustomerRepository.Delete(n => n.CustomerID.ToString() == id);

                    Save();

                    re.Message = FunctionHelper.GetLocalizeDictionary("Home", "notification")["DeleteSuccess"];
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

        public List<tblCustomer_Excel> ExcelAllByFirst(string key, string customergroup, string customerstatus = "")
        {
            var query = from n in _tblCustomerRepository.Table
                        select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.CustomerName.Contains(key) || n.CustomerCode.Contains(key) || n.Description.Contains(key) || n.CompartmentId.Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(customergroup))
            {
                query = query.Where(n => customergroup.Contains(n.CustomerGroupID));
            }

            if (!string.IsNullOrWhiteSpace(customerstatus))
            {
                if (customerstatus.Equals("0"))
                {
                    query = query.Where(n => n.Inactive == false);
                }
                else
                {
                    query = query.Where(n => n.Inactive == true);
                }
            }

            //
            var idCustomerGroups = "";
            var listCus = new List<tblCustomer_Excel>();

            if (query.Any())
            {
                //
                foreach (var item in query)
                {
                    idCustomerGroups += item.CustomerGroupID + ";";
                    //idCustomers += item._id.ToString() + ",";
                }

                //
                var listCustomerGroup = _tblCustomerGroupService.GetAllActiveByListId(idCustomerGroups).ToList();
                //var listCard = _PK_CardCustomerService.GetAllCardByListCustomerId(idCustomers).ToList();

                //
                var count = 0;

                foreach (var item in query)
                {
                    count++;
                    //var m = _tblCompartmentService.GetById(Guid.Parse(item.CompartmentId));
                    var objCompartment = new tblCompartment();
                    if (!string.IsNullOrEmpty(item.CompartmentId))
                    {
                        objCompartment = GetByIdCompart(( item.CompartmentId));
                    }


                    var cusgroupid = !string.IsNullOrEmpty(item.CustomerGroupID) ? item.CustomerGroupID.ToLower() : "";
                    var objCustomerGroup = listCustomerGroup.FirstOrDefault(n => n.CustomerGroupID.ToString().Equals(cusgroupid));

                    var obj = new tblCustomer_Excel()
                    {
                        NumberRow = count,
                        Active = item.Inactive == false ? "Hoạt động" : "Ngừng hoạt động",
                        //Cards = "",
                        CustomerAddress = item.Address,
                        CustomerCode = item.CustomerCode,
                        CustomerGroupName = objCustomerGroup != null ? objCustomerGroup.CustomerGroupName : "",
                        CustomerIdentify = item.IDNumber,
                        CustomerMobile = item.Mobile,
                        CustomerName = item.CustomerName,
                        CompartmentId = objCompartment != null ? objCompartment.CompartmentName : "",
                        //Plates = ""                      
                    };

                    listCus.Add(obj);
                }
            }

            return listCus;
        }

        private tblCompartment GetByIdCompart(string compartmentId)
        {
            var q = new StringBuilder();
            q.AppendLine("Select * from [tblCompartment] WHERE ");
            q.AppendLine(string.Format("CompartmentID = '{0}'", Guid.Parse( compartmentId)));
            var obj = new  tblCompartment();
            var dt = ExcuteSQL.GetDataSet(q.ToString());
            var dt1 = dt.Tables[0];
            foreach (DataRow item in dt1.Rows)
            {
                obj.CompartmentID = Guid.Parse(item["CompartmentID"].ToString());
                obj.CompartmentName = item["CompartmentName"].ToString();
                obj.SortOrder =Convert.ToInt32( item["SortOrder"].ToString());
            }

            return obj;
        }

        public List<tblCustomer_ExcelTRANSERCO> ExcelAllByFirstTRANSERCO(string key, string customergroup, string customerstatus = "")
        {
            var query = from n in _tblCustomerRepository.Table
                        select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.CustomerName.Contains(key) || n.CustomerCode.Contains(key) || n.Description.Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(customergroup))
            {
                query = query.Where(n => customergroup.Contains(n.CustomerGroupID));
            }

            if (!string.IsNullOrWhiteSpace(customerstatus))
            {
                if (customerstatus.Equals("0"))
                {
                    query = query.Where(n => n.Inactive == false);
                }
                else
                {
                    query = query.Where(n => n.Inactive == true);
                }
            }

            //
            var idCustomerGroups = "";
            var listCus = new List<tblCustomer_ExcelTRANSERCO>();

            query = query.OrderBy(n => n.CustomerName);

            if (query.Any())
            {
                //
                foreach (var item in query)
                {
                    idCustomerGroups += item.CustomerGroupID + ";";
                    //idCustomers += item._id.ToString() + ",";
                }

                //
                var listCustomerGroup = _tblCustomerGroupService.GetAllActiveByListId(idCustomerGroups).ToList();
                //var listCard = _PK_CardCustomerService.GetAllCardByListCustomerId(idCustomers).ToList();

                //
                var count = 0;

                foreach (var item in query)
                {
                    count++;

                    var objCustomerGroup = listCustomerGroup.FirstOrDefault(n => n.CustomerGroupID.ToString().Equals(item.CustomerGroupID));

                    var obj = new tblCustomer_ExcelTRANSERCO()
                    {
                        NumberRow = count,
                        Active = item.Inactive == false ? "Hoạt động" : "Ngừng hoạt động",
                        Cards = "",
                        CustomerAddress = item.Address,
                        CustomerCode = item.CustomerCode,
                        CustomerGroupName = objCustomerGroup != null ? objCustomerGroup.CustomerGroupName : "",
                        CustomerIdentify = item.IDNumber,
                        CustomerMobile = item.Mobile,
                        CustomerName = item.CustomerName,
                        Plates = "",
                        ContractCode = item.Description
                    };

                    listCus.Add(obj);
                }
            }

            return listCus;
        }
        public IEnumerable<tblCustomer> GetAll()
        {
            var query = from n in _tblCustomerRepository.Table
                        select n;

            return query;
        }

        public IEnumerable<tblCustomer> GetAllActive()
        {
            var query = from n in _tblCustomerRepository.Table
                        where n.Inactive == false
                        select n;

            return query;
        }

        public IEnumerable<tblCustomer> GetAllActiveByKey(string key)
        {
            var query = from n in _tblCustomerRepository.Table
                        where n.Inactive == false && n.CustomerCode.Contains(key)
                        select n;

            return query;
        }

        public IPagedList<tblCustomer> GetAllPagingByFirst(string key, string customergroup, int pageNumber, int pageSize, string customerstatus = "")
        {
            var query = from n in _tblCustomerRepository.Table
                        select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.CustomerName.Contains(key) || n.CustomerCode.Contains(key) || n.Description.Contains(key) || n.CompartmentId.Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(customergroup))
            {
                query = query.Where(n => customergroup.Contains(n.CustomerGroupID));
            }

            if (!string.IsNullOrWhiteSpace(customerstatus))
            {
                if (customerstatus.Equals("0"))
                {
                    query = query.Where(n => n.Inactive == false);
                }
                else
                {
                    query = query.Where(n => n.Inactive == true);
                }
            }

            var list = new PagedList<tblCustomer>(query.OrderBy(n => n.CustomerName), pageNumber, pageSize);

            return list;
        }
        public tblCustomer GetByDevPass(string DevPass)
        {
            var query = from n in _tblCustomerRepository.Table
                        where n.DevPass == DevPass
                        select n;

            return query.FirstOrDefault();
        }
        public tblCustomer GetByCode(string code)
        {
            var query = from n in _tblCustomerRepository.Table
                        where n.CustomerCode == code
                        select n;

            return query.FirstOrDefault();
        }

        public tblCustomer GetByFingerID(int id)
        {
            var query = from n in _tblCustomerRepository.Table
                        where n.UserIDofFinger == id
                        select n;

            return query.FirstOrDefault();
        }

        public tblCustomer GetByCode_Id(string code, string id)
        {
            var query = from n in _tblCustomerRepository.Table
                        where n.CustomerCode == code && n.CustomerID.ToString() != id
                        select n;

            return query.FirstOrDefault();
        }

        public tblCustomer GetById(Guid id)
        {
            return _tblCustomerRepository.GetById(id);
        }

        public tblCustomer GetByName(string name)
        {
            var query = from n in _tblCustomerRepository.Table
                        where n.CustomerName == name
                        select n;

            return query.FirstOrDefault();
        }

        public MessageReport Update(tblCustomer obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblCustomerRepository.Update(obj);

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

        public List<tblCustomer> GetAllByListId(List<string> ids)
        {
            var query = from n in _tblCustomerRepository.Table
                        where ids.Contains(n.CustomerID.ToString())
                        select n;
            var st = new StringBuilder();
            st.AppendLine("select * from  tblCustomer");
            st.AppendLine(string.Format("WHERE CustomerID In ( '{0}'", string.Join("','", ids.ToArray())));
            st.AppendLine(")");
            var lst = SqlExQuery<tblCustomer>.ExcuteQuery(st.ToString());
            return lst;
        }

        public tblCustomerSubmit GetCustomByCode(string code)
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

        public IEnumerable<tblCustomerExtend> GetAllActiveByListIdForUpload(string ids)
        {
            var query = from customer in _tblCustomerRepository.Table
                        join customergroup in _tblCustomerGroupRepository.Table on customer.CustomerGroupID equals customergroup.CustomerGroupID.ToString() into customer_group
                        from customergroup in customer_group.DefaultIfEmpty()
                        join accesslevel in _tblAccessLevelRepository.Table on customer.AccessLevelID equals accesslevel.AccessLevelID.ToString() into card_accesslevel
                        from accesslevel in card_accesslevel.DefaultIfEmpty()

                        where ids.Contains(customer.CustomerID.ToString())

                        select new tblCustomerExtend()
                        {
                            CustomerCode = customer.CustomerCode,
                            AccessExpireDate = customer.AccessExpireDate,
                            AccessLevelID = customer.AccessLevelID,
                            AccessLevelName = accesslevel.AccessLevelName,
                            Address = customer.Address,
                            CustomerGroupID = customer.CustomerGroupID,
                            CustomerGroupName = customergroup.CustomerGroupName,
                            CustomerID = customer.CustomerID.ToString(),
                            CustomerName = customer.CustomerName,
                            Finger1 = customer.Finger1,
                            Finger2 = customer.Finger2,
                            IDNumber = customer.IDNumber,
                            Mobile = customer.Mobile,
                            SortOrder = customer.SortOrder,
                            UserIDofFinger = customer.UserIDofFinger
                        };

            return query;
        }

        public IPagedList<tblCustomerExtend> GetAllPagingByFirstForUpload(string key, string anotherkey, string customergroups, string fromdate, string todate, int pageNumber = 1, int pageSize = 20, string accesslevelids = "")
        {
            var query = from customer in _tblCustomerRepository.Table
                        join customergroup in _tblCustomerGroupRepository.Table on customer.CustomerGroupID equals customergroup.CustomerGroupID.ToString() into customer_group
                        from customergroup in customer_group.DefaultIfEmpty()
                        join accesslevel in _tblAccessLevelRepository.Table on customer.AccessLevelID equals accesslevel.AccessLevelID.ToString() into card_accesslevel
                        from accesslevel in card_accesslevel.DefaultIfEmpty()

                        select new tblCustomerExtend()
                        {
                            AccessExpireDate = customer.AccessExpireDate,
                            AccessLevelID = customer.AccessLevelID,
                            AccessLevelName = accesslevel.AccessLevelName,
                            Address = customer.Address,
                            CustomerCode = customer.CustomerCode,
                            CustomerGroupID = customer.CustomerGroupID,
                            CustomerGroupName = customergroup.CustomerGroupName,
                            CustomerID = customer.CustomerID.ToString(),
                            CustomerName = customer.CustomerName,
                            Finger1 = customer.Finger1,
                            Finger2 = customer.Finger2,
                            IDNumber = customer.IDNumber,
                            Mobile = customer.Mobile,
                            SortOrder = customer.SortOrder,
                            UserIDofFinger = customer.UserIDofFinger
                        };

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.CustomerCode.Contains(key) || n.CustomerGroupName.Contains(key) || n.CustomerName.Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(anotherkey))
            {
                query = query.Where(n => n.CustomerCode.Contains(anotherkey) || n.CustomerGroupName.Contains(anotherkey) || n.CustomerName.Contains(anotherkey));
            }

            if (!string.IsNullOrWhiteSpace(customergroups))
            {
                query = query.Where(n => customergroups.Contains(n.CustomerGroupID));
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                var fdate = Convert.ToDateTime(fromdate);
                var tdate = Convert.ToDateTime(todate).AddDays(1);

                query = query.Where(n => n.AccessExpireDate >= fdate && n.AccessExpireDate < tdate);
            }

            if (!string.IsNullOrWhiteSpace(accesslevelids))
            {
                query = query.Where(n => accesslevelids.Contains(n.AccessLevelID));
            }

            var list = new PagedList<tblCustomerExtend>(query.OrderByDescending(n => n.AccessExpireDate), pageNumber, pageSize);

            return list;
        }

        public List<tblCustomerExtend> GetAllByFirst(string key, string anotherkey, string customergroups, string fromdate, string todate, string accesslevelids = "")
        {
            var query = from n in _tblCustomerRepository.Table

                        join customergroup in _tblCustomerGroupRepository.Table on n.CustomerGroupID equals customergroup.CustomerGroupID.ToString() into customer_group
                        from customergroup in customer_group.DefaultIfEmpty()

                        join accesslevel in _tblAccessLevelRepository.Table on n.AccessLevelID equals accesslevel.AccessLevelID.ToString() into customer_accesslevel
                        from accesslevel in customer_accesslevel.DefaultIfEmpty()

                        select new tblCustomerExtend() {

                            CustomerID = n.CustomerID.ToString(),
                            AccessExpireDate = n.AccessExpireDate,
                            AccessLevelID = n.AccessLevelID,
                            AccessLevelName = accesslevel.AccessLevelName,
                            Address = n.Address,
                            CustomerCode = n.CustomerCode,
                            CustomerGroupID = n.CustomerGroupID,
                            CustomerGroupName = customergroup.CustomerGroupName,
                            CustomerName = n.CustomerName,
                            Finger1 = n.Finger1,
                            Finger2 = n.Finger2,
                            IDNumber = n.IDNumber,
                            Mobile = n.Mobile,
                            Password = n.DevPass,
                            SortOrder = n.SortOrder,
                            UserIDofFinger = n.UserIDofFinger
                        };

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.CustomerCode.Contains(key) || n.CustomerGroupName.Contains(key) || n.CustomerName.Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(anotherkey))
            {
                query = query.Where(n => n.CustomerCode.Contains(key) || n.CustomerGroupName.Contains(key) || n.CustomerName.Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(customergroups))
            {
                query = query.Where(n => customergroups.Contains(n.CustomerGroupID));
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                var fdate = Convert.ToDateTime(fromdate);
                var tdate = Convert.ToDateTime(todate).AddDays(1);

                query = query.Where(n => n.AccessExpireDate >= fdate && n.AccessExpireDate < tdate);
            }

            if (!string.IsNullOrWhiteSpace(accesslevelids))
            {
                query = query.Where(n => accesslevelids.Contains(n.AccessLevelID));
            }

            return query.ToList();
        }

        public MessageReport UpdateAuthorizeCustomer(string key, string customergroup, string levelid, string customerstatus = "")
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");

            var str = new StringBuilder();
            str.AppendLine("UPDATE tblCustomer");
            str.AppendLine(string.Format("SET AccessLevelID = '{0}'", levelid));
            str.AppendLine("WHERE 1=1");


            var l = customergroup.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (l.Any())
            {
                str.AppendLine("AND CustomerGroupID IN (");

                var count = 0;
                foreach (var item in l)
                {
                    count++;

                    str.AppendLine(string.Format("'{0}'{1}", item, count == l.Count() ? "" : ","));
                }

                str.AppendLine(")");
            }


            try
            {
                var isSuccess = ExcuteSQL.Execute(str.ToString());

                result = new MessageReport(isSuccess, "Thành công");
            }
            catch (Exception ex)
            {

                result = new MessageReport(false, ex.Message);
            }

            return result;
        }

        public MessageReport UpdateCustomer(string useroffinger, string dateN, bool checkuse = false)
        {
            dateN = dateN.Substring(0, 4) + @"/" + dateN.Substring(4, 2) + @"/" + dateN.Substring(6, 2);

            var result = new MessageReport(false, "Có lỗi xảy ra");

            if (checkuse)
            {
                var str = string.Format("Update tblCustomer set AccessExpireDate = '{0}' where UserIDofFinger = '{1}'", dateN, useroffinger);

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
            }

            return result;
        }

        public IEnumerable<tblCustomerExtend> GetAllActiveByListIdForUpload(List<string> ids)
        {
            var query = from customer in _tblCustomerRepository.Table
                        join customergroup in _tblCustomerGroupRepository.Table on customer.CustomerGroupID equals customergroup.CustomerGroupID.ToString() into customer_group
                        from customergroup in customer_group.DefaultIfEmpty()
                        join accesslevel in _tblAccessLevelRepository.Table on customer.AccessLevelID equals accesslevel.AccessLevelID.ToString() into card_accesslevel
                        from accesslevel in card_accesslevel.DefaultIfEmpty()

                        where ids.Contains(customer.CustomerID.ToString())

                        select new tblCustomerExtend()
                        {
                            CustomerCode = customer.CustomerCode,
                            AccessExpireDate = customer.AccessExpireDate,
                            AccessLevelID = customer.AccessLevelID,
                            AccessLevelName = accesslevel.AccessLevelName,
                            Address = customer.Address,
                            CustomerGroupID = customer.CustomerGroupID,
                            CustomerGroupName = customergroup.CustomerGroupName,
                            CustomerID = customer.CustomerID.ToString(),
                            CustomerName = customer.CustomerName,
                            Finger1 = customer.Finger1,
                            Finger2 = customer.Finger2,
                            IDNumber = customer.IDNumber,
                            Mobile = customer.Mobile,
                            SortOrder = customer.SortOrder,
                            UserIDofFinger = customer.UserIDofFinger,
                            Password = customer.DevPass
                        };

            return query;
        }

        public IEnumerable<tblCustomerExtend> GetAllByFirstForUpload(string key, string anotherkey, string customergroups, string fromdate, string todate, string accesslevelids = "")
        {
            var query = from customer in _tblCustomerRepository.Table
                        join customergroup in _tblCustomerGroupRepository.Table on customer.CustomerGroupID equals customergroup.CustomerGroupID.ToString() into customer_group
                        from customergroup in customer_group.DefaultIfEmpty()
                        join accesslevel in _tblAccessLevelRepository.Table on customer.AccessLevelID equals accesslevel.AccessLevelID.ToString() into card_accesslevel
                        from accesslevel in card_accesslevel.DefaultIfEmpty()

                        select new tblCustomerExtend()
                        {
                            AccessExpireDate = customer.AccessExpireDate,
                            AccessLevelID = customer.AccessLevelID,
                            AccessLevelName = accesslevel.AccessLevelName,
                            Address = customer.Address,
                            CustomerCode = customer.CustomerCode,
                            CustomerGroupID = customer.CustomerGroupID,
                            CustomerGroupName = customergroup.CustomerGroupName,
                            CustomerID = customer.CustomerID.ToString(),
                            CustomerName = customer.CustomerName,
                            Finger1 = customer.Finger1,
                            Finger2 = customer.Finger2,
                            IDNumber = customer.IDNumber,
                            Mobile = customer.Mobile,
                            SortOrder = customer.SortOrder,
                            UserIDofFinger = customer.UserIDofFinger
                        };

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.CustomerCode.Contains(key) || n.CustomerGroupName.Contains(key) || n.CustomerName.Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(anotherkey))
            {
                query = query.Where(n => n.CustomerCode.Contains(anotherkey) || n.CustomerGroupName.Contains(anotherkey) || n.CustomerName.Contains(anotherkey));
            }

            if (!string.IsNullOrWhiteSpace(customergroups))
            {
                query = query.Where(n => customergroups.Contains(n.CustomerGroupID));
            }

            if (!string.IsNullOrWhiteSpace(fromdate) && !string.IsNullOrWhiteSpace(todate))
            {
                var fdate = Convert.ToDateTime(fromdate);
                var tdate = Convert.ToDateTime(todate).AddDays(1);

                query = query.Where(n => n.AccessExpireDate >= fdate && n.AccessExpireDate < tdate);
            }

            if (!string.IsNullOrWhiteSpace(accesslevelids))
            {
                query = query.Where(n => accesslevelids.Contains(n.AccessLevelID));
            }

            return query;
        }

        public IEnumerable<tblCustomer> GetAllActiveByKeyMaxTake(string key, int max)
        {
            var query = from n in _tblCustomerRepository.Table
                        where n.Inactive == false && (n.CustomerCode.Contains(key) || n.CustomerName.Contains(key))
                        select n;

            return query.OrderBy(n => n.CustomerName).Take(max);
        }

        public List<tblCustomer> GetAllActiveByKeySQL(string key, int max)
        {
            var query = new StringBuilder();
          
            query.AppendLine(string.Format("SELECT top {0} * FROM tblCustomer where Inactive = 'False'",max));

            if (!string.IsNullOrEmpty(key))
            {
                query.AppendLine(string.Format("AND (CustomerCode LIKE '%{0}%' OR CustomerName LIKE N'%{0}%' OR IDNumber LIKE '%{0}%' OR Mobile LIKE '%{0}%')", key));
            }

            var dataSet = ExcuteSQL.GetDataSet(query.ToString(), false);

            var list =  ExcuteSQL.ConvertTo<tblCustomer>(dataSet.Tables[0]);

            return list;

        }

        public List<tblCustomerExtend> GetAllByFirst(List<tblCustomerExtend> session)
        {
            var customerIDs = session.Select(n => n.CustomerID).ToList();

            var query = from n in _tblCustomerRepository.Table

                        join customergroup in _tblCustomerGroupRepository.Table on n.CustomerGroupID equals customergroup.CustomerGroupID.ToString() into customer_group
                        from customergroup in customer_group.DefaultIfEmpty()

                        join accesslevel in _tblAccessLevelRepository.Table on n.AccessLevelID equals accesslevel.AccessLevelID.ToString() into customer_accesslevel
                        from accesslevel in customer_accesslevel.DefaultIfEmpty()

                        where customerIDs.Contains(n.CustomerID.ToString())

                        select new tblCustomerExtend()
                        {

                            CustomerID = n.CustomerID.ToString(),
                            AccessExpireDate = n.AccessExpireDate,
                            AccessLevelID = n.AccessLevelID,
                            AccessLevelName = accesslevel.AccessLevelName,
                            Address = n.Address,
                            CustomerCode = n.CustomerCode,
                            CustomerGroupID = n.CustomerGroupID,
                            CustomerGroupName = customergroup.CustomerGroupName,
                            CustomerName = n.CustomerName,
                            Finger1 = n.Finger1,
                            Finger2 = n.Finger2,
                            IDNumber = n.IDNumber,
                            Mobile = n.Mobile,
                            Password = n.DevPass,
                            SortOrder = n.SortOrder,
                            UserIDofFinger = n.UserIDofFinger
                        };

            return query.ToList();
        }

        public tblCustomer GetByCardNumber(string cardNumber)
        {
            var objCard = _tblCardService.GetByCardNumber(cardNumber);
            var objCustomer = new tblCustomer();
            if (objCard.CustomerID != null && objCard.CustomerID !=  "")
            {
                 objCustomer = GetById(Guid.Parse( objCard.CustomerID));
            }
            return objCustomer;
        }

        public List<tblCardCustom> GetCustomerJoinCard()
        {
            var q = from n in _tblCardRepository.Table
                    join m in _tblCustomerRepository.Table
                    on n.CustomerID equals(m.CustomerID.ToString())
                    select new tblCardCustom()
                    {
                        CardID = n.CardID.ToString(),
                        CardNumber = n.CardNumber,
                        CustomerName = m.CustomerName,
                      
                    };
            return q.ToList();
        }
    }
}
