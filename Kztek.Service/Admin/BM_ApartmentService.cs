using Kztek.Data.Infrastructure;
using Kztek.Data.Repository;
using Kztek.Data.SqlHelper;
using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Web.Core.Functions;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Service.Admin
{
    public interface IBM_ApartmentService
    {
        IEnumerable<BM_Apartment> GetAll();
        IEnumerable<BM_Apartment> GetByBuilding(string buildingId);
        IEnumerable<BM_Apartment> GetByBuildingAndFloor(string buildingId, string floorId);


        IPagedList<BM_Apartment> GetAllPagingByFirst(string key, int pageNumber, int pageSize);


        BM_Apartment GetById(string id);
        BM_Apartment GetByName(string name);

        MessageReport Create(BM_Apartment obj);

        MessageReport Update(BM_Apartment obj);

        MessageReport DeleteById(string id, ref BM_Apartment obj);
        List<BM_ApartmentCustom> GetApartmentPaging(string key, int pageNumber, int pageSize, ref int total);

        #region Select List


        /// <summary>
        /// ApartmentToDDL
        /// string buildingId = "", string floorId = "" -- get ALL
        /// string buildingId != "", string floorId = "" -- get by building
        /// string buildingId != "", string floorId != "" -- get by building and floor
        /// </summary>
        /// <param name="buildingId"></param>
        /// <param name="floorId"></param>
        /// <returns></returns>
        List<SelectListModel> ApartmentToDDL(string buildingId, string floorId);

        #endregion
    }

    public class BM_ApartmentService : IBM_ApartmentService
    {
        private IBM_ApartmentRepository _BM_ApartmentRepository;

        private IUnitOfWork _UnitOfWork;

        public BM_ApartmentService(IBM_ApartmentRepository _BM_ApartmentRepository, IUnitOfWork _UnitOfWork)
        {
            this._BM_ApartmentRepository = _BM_ApartmentRepository;

            this._UnitOfWork = _UnitOfWork;
        }

        private void Save()
        {
            _UnitOfWork.Commit();
        }

        public MessageReport Create(BM_Apartment obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _BM_ApartmentRepository.Add(obj);

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

        public MessageReport DeleteById(string id, ref BM_Apartment obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                obj = GetById(id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    _BM_ApartmentRepository.Update(obj);

                    Save();

                    re.Message = FunctionHelper.GetLocalizeDictionary("Home", "notification")["DeleteSuccess"]; ;
                    re.isSuccess = true;
                }
                else
                {
                    re.Message = FunctionHelper.GetLocalizeDictionary("Home", "notification")["record_does_not_exist"]; ;
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

        public IEnumerable<BM_Apartment> GetAll()
        {
            var query = from n in _BM_ApartmentRepository.Table
                        select n;

            return query;
        }
        public BM_Apartment GetByName(string name)
        {
            var query = from n in _BM_ApartmentRepository.Table
                        where n.Name.Contains(name) && !n.IsDeleted
                        select n;

            return query.FirstOrDefault();
        }


        public IPagedList<BM_Apartment> GetAllPagingByFirst(string key, int pageNumber, int pageSize)
        {
            var query = from n in _BM_ApartmentRepository.Table
                        where !n.IsDeleted
                        select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.Name.Contains(key));
            }


            var list = new PagedList<BM_Apartment>(query.OrderByDescending(n => n.Name), pageNumber, pageSize);

            return list;
        }

        public BM_Apartment GetById(string id)
        {
            return _BM_ApartmentRepository.GetById(id);
        }



        public MessageReport Update(BM_Apartment obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _BM_ApartmentRepository.Update(obj);

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

        public List<BM_ApartmentCustom> GetApartmentPaging(string key, int pageNumber, int pageSize, ref int total)
        {

            //Lấy danh sách
            var sb = new StringBuilder();

            sb.AppendLine("SELECT * FROM(");
            sb.AppendLine("select ROW_NUMBER() OVER(ORDER BY a.Name asc) AS RowNumber, a.*,");
            sb.AppendLine("b.Name AS BuildingName,f.Name AS FloorName ");
            sb.AppendLine("from BM_Apartment a");
            sb.AppendLine("left join BM_Building b on b.Id = a.BuildingId");
            sb.AppendLine("left join BM_Floor f on f.Id = a.FloorId");
            sb.AppendLine("where a.IsDeleted = 'False'");

            if (!string.IsNullOrWhiteSpace(key))
                sb.AppendLine(string.Format(" AND (a.Name like N'%{0}%')", key));
         

            sb.AppendLine(") as b");

            sb.AppendLine(string.Format("WHERE RowNumber BETWEEN(({0}-1) * {1}+1) AND({0} * {1})", pageNumber, pageSize));
            var listData = SqlExQuery<BM_ApartmentCustom>.ExcuteQuery(sb.ToString());
            //Tính tổng
            sb.Clear();

            sb.AppendLine("select count(a.Id) as TotalCount");      
            sb.AppendLine("from BM_Apartment a");
            sb.AppendLine("left join BM_Building b on b.Id = a.BuildingId");
            sb.AppendLine("left join BM_Floor f on f.Id = a.FloorId");
            sb.AppendLine("where a.IsDeleted = 'False'");

            if (!string.IsNullOrWhiteSpace(key))
                sb.AppendLine(string.Format(" AND (a.Name like N'%{0}%')", key));


            var _total = SqlExQuery<TotalPaging>.ExcuteQueryFirst(sb.ToString());
            total = _total != null ? _total.TotalCount : 0;
            return listData;
        }


        public IEnumerable<BM_Apartment> GetByBuilding(string buildingId)
        {
            var query = from n in _BM_ApartmentRepository.Table
                        where n.BuildingId.Contains(buildingId) && !n.IsDeleted
                        select n;

            return query;
        }

        public IEnumerable<BM_Apartment> GetByBuildingAndFloor(string buildingId, string floorId)
        {
            var query = from n in _BM_ApartmentRepository.Table
                        where n.BuildingId.Contains(buildingId) && n.FloorId.Contains(floorId) && !n.IsDeleted
                        select n;

            return query;
        }

        #region select list
        public List<SelectListModel> ApartmentToDDL(string buildingId, string floorId)
        {
            var list = new List<SelectListModel> { new SelectListModel { ItemValue = "0", ItemText = "-- Lựa chọn --" }, };
            var lst = new List<BM_Apartment>();
            if (String.IsNullOrEmpty(floorId) && String.IsNullOrEmpty(buildingId))
            {
                lst = GetAll().ToList();
            }
            if (String.IsNullOrEmpty(floorId) && !String.IsNullOrEmpty(buildingId))
            {
                lst = GetByBuilding(buildingId).ToList();
            }
            else
            {
                lst = GetByBuildingAndFloor(buildingId, floorId).ToList();
            }
            if (lst.Any())
            {
                foreach (var item in lst)
                {
                    list.Add(new SelectListModel { ItemValue = item.Id, ItemText = item.Name });
                }
            }
            return list;
        }


        #endregion

    }
}
