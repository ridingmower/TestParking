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
    public interface IBM_BuildingService
    {
        IEnumerable<BM_Building> GetAll();

        IPagedList<BM_Building> GetAllPagingByFirst(string key, int pageNumber, int pageSize);
        
        BM_Building GetById(string id);

        MessageReport Create(BM_Building obj);

        MessageReport Update(BM_Building obj);

        MessageReport DeleteById(string id, ref BM_Building obj);

        #region Select List
        /// <summary>
        ///  Select List Building
        /// </summary>
        /// <returns></returns>
        List<SelectListModel> BuildingIdToDDL();

        #endregion
    }

    public class BM_BuildingService : IBM_BuildingService
    {
        private IBM_BuildingRepository _BM_BuildingRepository;

        private IUnitOfWork _UnitOfWork;

        public BM_BuildingService(IBM_BuildingRepository _BM_BuildingRepository, IUnitOfWork _UnitOfWork)
        {
            this._BM_BuildingRepository = _BM_BuildingRepository;

            this._UnitOfWork = _UnitOfWork;
        }

        private void Save()
        {
            _UnitOfWork.Commit();
        }

        public MessageReport Create(BM_Building obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _BM_BuildingRepository.Add(obj);

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

        public MessageReport DeleteById(string id, ref BM_Building obj)
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
                    _BM_BuildingRepository.Update(obj);

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

        public IEnumerable<BM_Building> GetAll()
        {
            var query = from n in _BM_BuildingRepository.Table
                        where !n.IsDeleted
                        orderby n.Name ascending
                        select n;

            return query;
        }



        public BM_Building GetById(string id)
        {
            return _BM_BuildingRepository.GetById(id);
        }



        public MessageReport Update(BM_Building obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _BM_BuildingRepository.Update(obj);

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

        IPagedList<BM_Building> IBM_BuildingService.GetAllPagingByFirst(string key, int pageNumber, int pageSize)
        {
            var query = from n in _BM_BuildingRepository.Table
                         select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.Name.Contains(key) || n.Description.Contains(key) || n.Note.Contains(key));

                int floorNumber;
                bool isNumeric = int.TryParse(key, out floorNumber);
                if (isNumeric)
                {
                    query = query.Where(n => n.FloorNumber == floorNumber);
                }
            }
            var list = new PagedList<BM_Building>(query.OrderBy(n => n.DateCreated), pageNumber, pageSize);

            return list;

        }

        #region Select List

        /// <summary>
        /// Select List Building
        /// </summary>
        /// <returns></returns>
        public List<SelectListModel> BuildingIdToDDL()
        {
            var list = new List<SelectListModel> { new SelectListModel { ItemValue = "0", ItemText = "-- Lựa chọn --" }, };
            var lst = GetAll().ToList();
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
