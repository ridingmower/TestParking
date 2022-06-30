using Kztek.Data.Infrastructure;
using Kztek.Data.Repository;
using Kztek.Model.CustomModel;
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
    public interface ItblCardGroupService
    {
        IEnumerable<tblCardGroup> GetAll();

        IEnumerable<tblCardGroup> GetAllActive();
        IEnumerable<tblCardGroup> GetAllActive_v2();
        IEnumerable<tblCardGroup> GetAllActiveMonth();
        IEnumerable<tblCardGroup> GetAllActiveById(string id);

        IEnumerable<tblCardGroup> GetAllActiveByType(int type);

        IEnumerable<tblCardGroup> GetAllActiveByType(string type);

        IEnumerable<tblCardGroup> GetAllActiveCardGroupMonth(string cardgroupid);
        DataTable GetActiveCardGroupMonth(string cardgroupid);
        DataTable GetActiveCardGroup(string cardgroupid);

        IPagedList<tblCardGroup> GetAllPagingByFirst(string key, int pageNumber, int pageSize);

        IEnumerable<tblCardGroup> GetAllByVehicleGroupId(string id);

        tblCardGroup GetById(Guid id);

        tblCardGroup GetByName(string name);

        MessageReport Create(tblCardGroup obj);

        MessageReport Update(tblCardGroup obj);

        MessageReport DeleteById(string id, ref tblCardGroup obj);
        IEnumerable<tblCardGroup> GetByLane(string laneid);
        List<tblCardGroup> GetCardGrByVehicleBike();
        List<tblCardGroup> GetCardGrByVehicleCar();
    }

    public class tblCardGroupService : ItblCardGroupService
    {
        private ItblCardGroupRepository _tblCardGroupRepository;
        private IUnitOfWork _UnitOfWork;
        private IUser_AuthGroupService _User_AuthGroupService;
        private string AuthCardGroupIds = "";
        private ItblVehicleGroupService _tblVehicleGroupService;
        public tblCardGroupService(ItblVehicleGroupService _tblVehicleGroupService,ItblCardGroupRepository _tblCardGroupRepository, IUser_AuthGroupService _User_AuthGroupService, IUnitOfWork _UnitOfWork)
        {
            this._tblCardGroupRepository = _tblCardGroupRepository;
            this._UnitOfWork = _UnitOfWork;
            this._tblVehicleGroupService = _tblVehicleGroupService;
            this._User_AuthGroupService = _User_AuthGroupService;

            AuthCardGroupIds = _User_AuthGroupService.GetAuthCardGroupIds();
        }

        private void Save()
        {
            _UnitOfWork.Commit();
        }

        public MessageReport Create(tblCardGroup obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblCardGroupRepository.Add(obj);

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

        public MessageReport DeleteById(string id, ref tblCardGroup obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                obj = GetById(Guid.Parse(id));
                if (obj != null)
                {
                    _tblCardGroupRepository.Delete(n => n.CardGroupID.ToString() == id);

                    Save();

                    re.Message = FunctionHelper.GetLocalizeDictionary("Home", "notification")["DeleteSuccess"];
                    re.isSuccess = true;
                }
                else
                {
                    re.Message = FunctionHelper.GetLocalizeDictionary("Home", "notification")["record_does_not_exist"];
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

        public IEnumerable<tblCardGroup> GetAll()
        {
            var query = from n in _tblCardGroupRepository.Table
                        select n;

            //if (!string.IsNullOrEmpty(AuthCardGroupIds))
            //{
            //    var list = AuthCardGroupIds.Split(';');
            //    query = query.Where(n => list.Contains(n.CardGroupID.ToString()));
            //}

            return query;
        }

        public IEnumerable<tblCardGroup> GetAllActive()
        {
            var query = from n in _tblCardGroupRepository.Table
                        where n.Inactive == false
                        select n;

            if (!string.IsNullOrEmpty(AuthCardGroupIds))
            {
                var list = AuthCardGroupIds.Split(';');
                query = query.Where(n => list.Contains(n.CardGroupID.ToString()));
            }

            return query;
        }
        public IEnumerable<tblCardGroup> GetAllActive_v2()
        {
            var query = from n in _tblCardGroupRepository.Table
                        where n.Inactive == false
                        select n;

            return query;
        }
        public IEnumerable<tblCardGroup> GetAllActiveMonth()
        {
            var query = from n in _tblCardGroupRepository.Table
                        where n.CardType == 0
                        select n;
            if (!string.IsNullOrEmpty(AuthCardGroupIds))
            {
                var list = AuthCardGroupIds.Split(';');
                query = query.Where(n => list.Contains(n.CardGroupID.ToString()));
            }
            return query;
        }

        public IEnumerable<tblCardGroup> GetAllActiveCardGroupMonth(string cardgroupid)
        {
            if (!string.IsNullOrEmpty(cardgroupid))
            {
                var query = from n in _tblCardGroupRepository.Table
                            where n.Inactive == false && n.CardType == 0 && n.CardGroupID.ToString().Contains(cardgroupid)
                            orderby n.SortOrder
                            select n;

                return query;
            }
            else
            {
                var query = from n in _tblCardGroupRepository.Table
                            where n.Inactive == false && n.CardType == 0
                            orderby n.SortOrder
                            select n;

                return query;
            }
        }

        public IEnumerable<tblCardGroup> GetAllActiveById(string id)
        {
            var query = from n in _tblCardGroupRepository.Table
                        where n.Inactive == false && n.CardGroupID.ToString() == id
                        select n;

            return query;
        }

        public IEnumerable<tblCardGroup> GetAllActiveByType(int type)
        {
            var query = from n in _tblCardGroupRepository.Table
                        where n.Inactive == false && n.CardType == type
                        select n;

            return query;
        }

        public IEnumerable<tblCardGroup> GetAllActiveByType(string type)
        {
            var query = from n in _tblCardGroupRepository.Table
                        where n.Inactive == false && n.CardType == int.Parse(type)
                        select n;

            return query;
        }

        public IEnumerable<tblCardGroup> GetAllByVehicleGroupId(string id)
        {
            var query = from n in _tblCardGroupRepository.Table
                        where n.VehicleGroupID == id
                        select n;

            return query;
        }

        public IPagedList<tblCardGroup> GetAllPagingByFirst(string key, int pageNumber, int pageSize)
        {
            var query = from n in _tblCardGroupRepository.Table
                        select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.CardGroupName.Contains(key));
            }

            if (!string.IsNullOrEmpty(AuthCardGroupIds))
            {
                var lst = AuthCardGroupIds.Split(';');
                query = query.Where(n => lst.Contains(n.CardGroupID.ToString()));
            }

            var list = new PagedList<tblCardGroup>(query.OrderByDescending(n => n.CardGroupName), pageNumber, pageSize);

            return list;
        }

        public tblCardGroup GetById(Guid id)
        {
            return _tblCardGroupRepository.GetById(id);
        }

        public tblCardGroup GetByName(string name)
        {
            var query = from n in _tblCardGroupRepository.Table
                        where n.CardGroupName.Equals(name)
                        select n;

            return query.FirstOrDefault();
        }
        public IEnumerable<tblCardGroup> GetByLane(string laneid)
        {
            var query = from n in _tblCardGroupRepository.Table
                        where !string.IsNullOrEmpty(n.LaneIDs) && n.LaneIDs.Contains(laneid) && !n.Inactive
                        select n;

            return query;
        }

        public MessageReport Update(tblCardGroup obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblCardGroupRepository.Update(obj);

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

        public DataTable GetActiveCardGroupMonth(string CardGroupID)
        {
            var query = new StringBuilder();

            query.AppendLine("Select * from tblCardGroup where Inactive='false' and CardType = '0' ");


            //Nhom the
            if (!string.IsNullOrWhiteSpace(CardGroupID))
            {
                var t = CardGroupID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and CardGroupID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            if (!string.IsNullOrWhiteSpace(AuthCardGroupIds))
            {
                var t = AuthCardGroupIds.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and CardGroupID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            query.AppendLine("order by SortOrder asc");

            return Data.SqlHelper.ExcuteSQL.GetDataSet(query.ToString(), false).Tables[0];
        }

        public DataTable GetActiveCardGroup(string CardGroupID)
        {
            var query = new StringBuilder();

            query.AppendLine("Select * from tblCardGroup where Inactive='false' ");

            //Nhom the
            if (!string.IsNullOrWhiteSpace(CardGroupID))
            {
                var t = CardGroupID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and CardGroupID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            query.AppendLine("order by SortOrder asc");

            return Data.SqlHelper.ExcuteSQL.GetDataSet(query.ToString(), false).Tables[0];

        }

        public List<tblCardGroup> GetCardGrByVehicleBike()
        {
           
            var objVehicle = _tblVehicleGroupService.GetAllActive();
            var obj = objVehicle.FirstOrDefault(n => n.VehicleType == 1);
            var query = from n in _tblCardGroupRepository.Table
                        where n.VehicleGroupID == obj.VehicleGroupID.ToString()
                        select n;

            return query.ToList();
          
        }

        public List<tblCardGroup> GetCardGrByVehicleCar()
        {
            var objVehicle = _tblVehicleGroupService.GetAllActive();
            var obj = objVehicle.FirstOrDefault(n => n.VehicleType == 0);
            var query = from n in _tblCardGroupRepository.Table
                        where n.VehicleGroupID == obj.VehicleGroupID.ToString()
                        select n;

            return query.ToList();
        }

        }   

    }

