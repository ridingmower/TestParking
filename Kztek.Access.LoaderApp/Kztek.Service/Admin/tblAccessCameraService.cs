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
    public interface ItblAccessCameraService
    {
        IEnumerable<tblAccessCamera> GetAll();

        IEnumerable<tblAccessCamera> GetAllByController(string id);

        IEnumerable<tblAccessCamera> GetAllActive();

        IEnumerable<tblAccessCamera> GetAllActiveByController(string id);

        IEnumerable<tblAccessCamera> GetAllPagingByFirst(string key, string pc, int pageNumber, int pageSize, ref int totalPage, ref int totalItem);

        IPagedList<tblAccessCameraCustomViewModel> GetAllCustomPagingByFirst(string key, string pc, int pageNumber, int pageSize);

        tblAccessCamera GetById(Guid id);

        tblAccessCamera GetByName(string name);

        tblAccessCamera GetByName_Id(string name, Guid id);

        MessageReport Create(tblAccessCamera obj);

        MessageReport Update(tblAccessCamera obj);

        MessageReport DeleteById(string id, ref tblAccessCamera obj);
    }

    public class tblAccessCameraService : ItblAccessCameraService
    {
        private ItblAccessCameraRepository _tblAccessCameraRepository;
        private ItblAccessControllerRepository _tblAccessControllerRepository;
        private IUnitOfWork _UnitOfWork;

        public tblAccessCameraService(ItblAccessCameraRepository _tblAccessCameraRepository, ItblAccessControllerRepository _tblAccessControllerRepository, IUnitOfWork _UnitOfWork)
        {
            this._tblAccessCameraRepository = _tblAccessCameraRepository;
            this._tblAccessControllerRepository = _tblAccessControllerRepository;
            this._UnitOfWork = _UnitOfWork;
        }

        private void Save()
        {
            _UnitOfWork.Commit();
        }

        public MessageReport Create(tblAccessCamera obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblAccessCameraRepository.Add(obj);

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

        public MessageReport DeleteById(string id, ref tblAccessCamera obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                obj = GetById(Guid.Parse(id));
                if (obj != null)
                {
                    _tblAccessCameraRepository.Delete(n => n.CameraID.ToString() == id);

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

        public IEnumerable<tblAccessCamera> GetAll()
        {
            var query = from n in _tblAccessCameraRepository.Table
                        select n;

            return query;
        }

        public IEnumerable<tblAccessCamera> GetAllActive()
        {
            var query = from n in _tblAccessCameraRepository.Table
                        where n.Inactive == false
                        select n;

            return query;
        }

        public IEnumerable<tblAccessCamera> GetAllActiveByController(string id)
        {
            var query = from n in _tblAccessCameraRepository.Table
                        where n.Inactive == false && n.ControllerID == id
                        select n;

            return query;
        }

        public IEnumerable<tblAccessCamera> GetAllByController(string id)
        {
            var query = from n in _tblAccessCameraRepository.Table
                        where n.ControllerID == id
                        select n;

            return query;
        }

        public IPagedList<tblAccessCameraCustomViewModel> GetAllCustomPagingByFirst(string key, string pc, int pageNumber, int pageSize)
        {
            var query = (from n in _tblAccessCameraRepository.Table
                         join m in _tblAccessControllerRepository.Table on n.ControllerID equals m.ControllerID.ToString() into n_m
                         from m in n_m.DefaultIfEmpty()

                         select new tblAccessCameraCustomViewModel()
                         {
                             CameraID = n.CameraID.ToString(),
                             CameraName = n.CameraName,
                             ControllerID = n.ControllerID,
                             ControllerName = m.ControllerName,
                             HttpUrl = n.HttpURL,
                             Inactive = n.Inactive,
                             SortOrder = n.SortOrder
                         });

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.CameraName.Contains(key) || n.ControllerName.Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(pc))
            {
                query = query.Where(n => n.ControllerID == pc);
            }

            var list = new PagedList<tblAccessCameraCustomViewModel>(query.OrderByDescending(n => n.SortOrder), pageNumber, pageSize);

            return list;
        }

        public IEnumerable<tblAccessCamera> GetAllPagingByFirst(string key, string pc, int pageNumber, int pageSize, ref int totalPage, ref int totalItem)
        {
            var query = from n in _tblAccessCameraRepository.Table
                        select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.CameraName.Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(pc))
            {
                query = query.Where(n => n.ControllerID == pc);
            }

            var list = new PagedList<tblAccessCamera>(query.OrderByDescending(n => n.SortOrder), pageNumber, pageSize);

            return list;
        }

        public tblAccessCamera GetById(Guid id)
        {
            return _tblAccessCameraRepository.GetById(id);
        }

        public MessageReport Update(tblAccessCamera obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblAccessCameraRepository.Update(obj);

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

        public tblAccessCamera GetByName(string name)
        {
            var query = from n in _tblAccessCameraRepository.Table
                        where n.CameraName == name
                        select n;

            return query.FirstOrDefault();
        }

        public tblAccessCamera GetByName_Id(string name, Guid id)
        {
            var query = from n in _tblAccessCameraRepository.Table
                        where n.CameraName == name && n.CameraID != id
                        select n;

            return query.FirstOrDefault();
        }
    }
}
