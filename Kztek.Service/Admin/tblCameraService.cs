using Kztek.Data.Infrastructure;
using Kztek.Data.Repository;
using Kztek.Model.CustomModel;
using Kztek.Model.CustomModel.iParking;
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
    public interface ItblCameraService
    {
        IEnumerable<tblCamera> GetAll();

        IEnumerable<tblCamera> GetAllByPC(string id);

        IEnumerable<tblCamera> GetAllActive();

        IEnumerable<tblCamera> GetAllActiveByPC(string id);

        IEnumerable<tblCamera> GetAllPagingByFirst(string key, string pc, int pageNumber, int pageSize, ref int totalPage, ref int totalItem);

        IPagedList<tblCameraCustomViewModel> GetAllCustomPagingByFirst(string key, string pc, int pageNumber, int pageSize);

        tblCamera GetById(Guid id);

        tblCamera GetByName(string name);

        tblCamera GetByName_Id(string name, Guid id);

        MessageReport Create(tblCamera obj);

        MessageReport Update(tblCamera obj);

        MessageReport DeleteById(string id, ref tblCamera obj);
    }

    public class tblCameraService : ItblCameraService
    {
        private ItblCameraRepository _tblCameraRepository;
        private ItblPCRepository _tblPCRepository;
        private IUnitOfWork _UnitOfWork;

        public tblCameraService(ItblCameraRepository _tblCameraRepository, ItblPCRepository _tblPCRepository, IUnitOfWork _UnitOfWork)
        {
            this._tblCameraRepository = _tblCameraRepository;
            this._tblPCRepository = _tblPCRepository;
            this._UnitOfWork = _UnitOfWork;
        }

        private void Save()
        {
            _UnitOfWork.Commit();
        }

        public MessageReport Create(tblCamera obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblCameraRepository.Add(obj);

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

        public MessageReport DeleteById(string id, ref tblCamera obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                obj = GetById(Guid.Parse(id));
                if (obj != null)
                {
                    _tblCameraRepository.Delete(n => n.CameraID.ToString() == id);

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

        public IEnumerable<tblCamera> GetAll()
        {
            var query = from n in _tblCameraRepository.Table
                        select n;

            return query;
        }

        public IEnumerable<tblCamera> GetAllActive()
        {
            var query = from n in _tblCameraRepository.Table
                        where n.Inactive == false
                        select n;

            return query;
        }

        public IEnumerable<tblCamera> GetAllActiveByPC(string id)
        {
            var query = from n in _tblCameraRepository.Table
                        where n.Inactive == false && n.PCID == id
                        select n;

            return query;
        }

        public IEnumerable<tblCamera> GetAllByPC(string id)
        {
            var query = from n in _tblCameraRepository.Table
                        where  n.PCID == id
                        select n;

            return query;
        }

        public IPagedList<tblCameraCustomViewModel> GetAllCustomPagingByFirst(string key, string pc, int pageNumber, int pageSize)
        {
            var query = (from n in _tblCameraRepository.Table
                         join m in _tblPCRepository.Table on n.PCID equals m.PCID.ToString() into n_m
                         from m in n_m.DefaultIfEmpty()

                         select new tblCameraCustomViewModel()
                         {
                             CameraID = n.CameraID.ToString(),
                             CameraName = n.CameraName,
                             PCID = n.PCID,
                             PCName = m.ComputerName,
                             HttpUrl = n.HttpURL,
                             Inactive = n.Inactive,
                             SortOrder = n.SortOrder
                         });

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.CameraName.Contains(key) || n.PCName.Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(pc))
            {
                query = query.Where(n => n.PCID == pc);
            }

            var list = new PagedList<tblCameraCustomViewModel>(query.OrderByDescending(n => n.SortOrder), pageNumber, pageSize);

            return list;
        }

        public IEnumerable<tblCamera> GetAllPagingByFirst(string key, string pc, int pageNumber, int pageSize, ref int totalPage, ref int totalItem)
        {
            var query = from n in _tblCameraRepository.Table
                        select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.CameraName.Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(pc))
            {
                query = query.Where(n => n.PCID == pc);
            }

            var list = new PagedList<tblCamera>(query.OrderByDescending(n => n.SortOrder), pageNumber, pageSize);

            return list;
        }

        public tblCamera GetById(Guid id)
        {
            return _tblCameraRepository.GetById(id);
        }

        public MessageReport Update(tblCamera obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblCameraRepository.Update(obj);

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

        public tblCamera GetByName(string name)
        {
            var query = from n in _tblCameraRepository.Table
                        where n.CameraName == name
                        select n;

            return query.FirstOrDefault();
        }

        public tblCamera GetByName_Id(string name, Guid id)
        {
            var query = from n in _tblCameraRepository.Table
                        where n.CameraName == name && n.CameraID != id
                        select n;

            return query.FirstOrDefault();
        }
    }
}
