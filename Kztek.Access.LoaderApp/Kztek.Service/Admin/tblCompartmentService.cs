using Kztek.Data.Infrastructure;
using Kztek.Data.Repository;
using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Service.Admin
{
    public interface ItblCompartmentService
    {
        List<tblCompartment> GetAll();

        IEnumerable<tblCompartment> GetAllByFirst(string key);

        IPagedList<tblCompartment> GetPagingByFirst(string key, int pageNumber, int pageSize);

        tblCompartment GetById(Guid id);

        IEnumerable<tblCompartment> GetByName(string name);

        MessageReport Create(tblCompartment obj);

        MessageReport Update(tblCompartment obj);

        MessageReport DeleteById(string id, ref tblCompartment obj);
        tblCompartment GetByNameDetail(string v);
    }
    class tblCompartmentService : ItblCompartmentService
    {
        private ItblCompartmentRepository _tblCompartmentRepository;
        private IUnitOfWork _unitOfWork;

        public tblCompartmentService(ItblCompartmentRepository _tblCompartmentRepository, IUnitOfWork _unitOfWork)
        {
            this._tblCompartmentRepository = _tblCompartmentRepository;
            this._unitOfWork = _unitOfWork;
        }

        private void Save()
        {
            _unitOfWork.Commit();
        }

        public tblCompartmentService(tblCompartmentRepository _tblCompartmentRepository, IUnitOfWork _unitOfWork)
        {
            this._tblCompartmentRepository = _tblCompartmentRepository;
            this._unitOfWork = _unitOfWork;
        }

        public List<tblCompartment> GetAll()
        {
            var query = from n in _tblCompartmentRepository.Table
                        select n;

            return query.ToList();
        }

        public IEnumerable<tblCompartment> GetAllByFirst(string key)
        {
            var query = from n in _tblCompartmentRepository.Table
                        select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.CompartmentName.Contains(key));
            }

            return query;
        }

        public IPagedList<tblCompartment> GetPagingByFirst(string key, int pageNumber, int pageSize)
        {
            var query = from n in _tblCompartmentRepository.Table
                        select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.CompartmentName.Contains(key));
            }

            var list = new PagedList<tblCompartment>(query.OrderByDescending(n => n.CompartmentName), pageNumber, pageSize);

            return list;
        }

        public tblCompartment GetById(Guid id)
        {
            var m = id.ToString().Replace("{", "").Replace("}", "");
            return _tblCompartmentRepository.GetById(Guid.Parse(m));
        }

        public IEnumerable<tblCompartment> GetByName(string name)
        {
            var query = from n in _tblCompartmentRepository.Table
                        where n.CompartmentName.Equals(name)
                        select n;

            return query;
        }

        public MessageReport Create(tblCompartment obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblCompartmentRepository.Add(obj);

                Save();

                re.Message = "Thêm mới thành công";
                re.isSuccess = true;
            }
            catch (Exception ex)
            {
                re.Message = ex.Message;
                re.isSuccess = false;
            }

            return re;
        }

        public MessageReport Update(tblCompartment obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblCompartmentRepository.Update(obj);

                Save();

                re.Message = "Cập nhật thành công";
                re.isSuccess = true;
            }
            catch (Exception ex)
            {
                re.Message = ex.Message;
                re.isSuccess = false;
            }

            return re;
        }

        public MessageReport DeleteById(string id, ref tblCompartment obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                obj = GetById(Guid.Parse(id));
                if (obj != null)
                {
                    _tblCompartmentRepository.Delete(n => n.CompartmentID.ToString() == id);

                    Save();

                    re.Message = "Xóa thành công";
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

        public tblCompartment GetByNameDetail(string name)
        {
            var query = from n in _tblCompartmentRepository.Table
                        where n.CompartmentName.Equals(name)
                        select n;

            return query.FirstOrDefault();
        }
    }
}
