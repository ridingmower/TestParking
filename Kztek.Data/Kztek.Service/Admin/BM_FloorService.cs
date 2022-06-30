using Kztek.Data.Infrastructure;
using Kztek.Data.Repository;
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
    public interface IBM_FloorService
    {
        IEnumerable<BM_Floor> GetAll();

        IEnumerable<BM_Floor> GetByBuilding(string buildingid);   

        IPagedList<BM_Floor> GetAllPagingByFirst(string key, string buildingId, int pageNumber, int pageSize);

        BM_Floor GetById(string id);

        MessageReport Create(BM_Floor obj);

        MessageReport Update(BM_Floor obj);

        MessageReport DeleteById(string id, ref BM_Floor obj);


        #region Select List


        /// <summary>
        /// FloorByBuildingToDDL
        /// </summary>
        /// <param name="buildingId"></param>
        /// <returns></returns>
        List<SelectListModel> FloorToDDL(string buildingId);

        #endregion
    }

    public class BM_FloorService : IBM_FloorService
    {
        private IBM_FloorRepository _BM_FloorRepository;

        private IUnitOfWork _UnitOfWork;

        public BM_FloorService(IBM_FloorRepository _BM_FloorRepository, IUnitOfWork _UnitOfWork)
        {
            this._BM_FloorRepository = _BM_FloorRepository;

            this._UnitOfWork = _UnitOfWork;
        }

        private void Save()
        {
            _UnitOfWork.Commit();
        }

        public MessageReport Create(BM_Floor obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _BM_FloorRepository.Add(obj);

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

        public MessageReport DeleteById(string id, ref BM_Floor obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                obj = GetById(id);
                if (obj != null)
                {
                    _BM_FloorRepository.Delete(n => n.Id.ToString() == id);

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

        public IEnumerable<BM_Floor> GetAll()
        {
            var query = from n in _BM_FloorRepository.Table
                        select n;

            return query;
        }

        public IEnumerable<BM_Floor> GetByBuilding(string buildingid)
        {
            var query = from n in _BM_FloorRepository.Table
                        where n.BuildingId == buildingid
                        orderby n.Name ascending
                        select n;

            return query;
        }


        public MessageReport Update(BM_Floor obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _BM_FloorRepository.Update(obj);

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


        public BM_Floor GetById(string id)
        {
            return _BM_FloorRepository.GetById(id);
        }

        IPagedList<BM_Floor> IBM_FloorService.GetAllPagingByFirst(string key, string buildingId, int pageNumber, int pageSize)
        {
            var query = from n in _BM_FloorRepository.Table
                        select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.Name.Contains(key) || n.Id.Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(buildingId))
            {
                query = query.Where(n => n.BuildingId.Contains(buildingId));
            }

            var list = new PagedList<BM_Floor>(query.OrderBy(n => n.Ordering), pageNumber, pageSize);

            return list;
        }

        #region Select List


        /// <summary>
        /// Select List Floor By buildingId
        /// </summary>
        /// <param name="buildingId"></param>
        /// <returns></returns>
        public List<SelectListModel> FloorToDDL(string buildingId)  //bind ServicePackageId to dropdownlist
        {
            var list = new List<SelectListModel> { new SelectListModel { ItemValue = "0", ItemText = "-- Lựa chọn --" }, };
            var lst = new List<BM_Floor>();
            if (String.IsNullOrEmpty(buildingId))
            {
                GetByBuilding(buildingId).ToList();
            }
            else
            {
                GetByBuilding(buildingId).ToList();
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
