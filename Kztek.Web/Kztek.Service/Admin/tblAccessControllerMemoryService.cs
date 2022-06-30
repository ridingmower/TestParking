using Kztek.Data.Infrastructure;
using Kztek.Data.Repository;
using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Web.Core.Functions;
using Kztek.Web.Core.Models;
using Kztek.Web.Core.Helpers;
using PagedList;
using System;
using System.Linq;
using System.Collections.Generic;
namespace Kztek.Service.Admin
{
    public interface ItblAccessControllerMemoryService
    {
        IQueryable<tblAccessControllerMemory> GetAll();
        IPagedList<tblAccessControllerMemory> GetAllPagingByFirst(string key, int pageNumber, int pageSize);
        tblAccessControllerMemory GetById(int id);
        MessageReport Create(tblAccessControllerMemory obj);
        MessageReport Update(tblAccessControllerMemory obj);
        MessageReport DeleteById(int id);
    }

    public class tblAccessControllerMemoryService : ItblAccessControllerMemoryService
    {
        private readonly ItblAccessControllerMemoryRepository _tblAccessControllerMemoryRepository;
        private readonly IUnitOfWork _UnitOfWork;
        public tblAccessControllerMemoryService(ItblAccessControllerMemoryRepository _tblAccessControllerMemoryRepository, IUnitOfWork _UnitOfWork)
        {
            this._tblAccessControllerMemoryRepository = _tblAccessControllerMemoryRepository;
            this._UnitOfWork = _UnitOfWork;
        }

        public IQueryable<tblAccessControllerMemory> GetAll()
        {
            var query = from n in _tblAccessControllerMemoryRepository.Table select n;
            return query;
        }
        public IPagedList<tblAccessControllerMemory> GetAllPagingByFirst(string key, int pageNumber, int pageSize)
        {
            var query = from n in _tblAccessControllerMemoryRepository.Table select n;
            if (!string.IsNullOrWhiteSpace(key))
            {
            }
            var list = new PagedList<tblAccessControllerMemory>(query.OrderBy(n => n.Id), pageNumber, pageSize);
            return list;
        }

        public tblAccessControllerMemory GetById(int id)
        {
            return _tblAccessControllerMemoryRepository.GetById(id);
        }

        public MessageReport Create(tblAccessControllerMemory obj)
        {
            MessageReport report;
            try
            {
                _tblAccessControllerMemoryRepository.Add(obj);
                Save();
                report = new MessageReport(true, "Thêm thành công");
            }
            catch (Exception ex)
            {
                report = new MessageReport(false, ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
            }
            return report;
        }

        public MessageReport Update(tblAccessControllerMemory obj)
        {
            MessageReport report;
            try
            {
                _tblAccessControllerMemoryRepository.Update(obj);
                Save();
                report = new MessageReport(true, "Cập nhật thành công");
            }
            catch (Exception ex)
            {
                report = new MessageReport(false, ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
            }
            return report;
        }

        public MessageReport DeleteById(int id)
        {
            MessageReport report;
            try
            {
                var obj = _tblAccessControllerMemoryRepository.GetById(id);
                if (obj != null)
                {
                    _tblAccessControllerMemoryRepository.Delete(obj);
                    Save();
                    report = new MessageReport(true, "Xóa thông tin thành công");
                }
                else
                {
                    report = new MessageReport(false, "Thông tin này không tồn tại");
                }
            }
            catch (Exception ex)
            {
                report = new MessageReport(false, ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
            }
            return report;
        }
        
        //Save change
        public void Save()
        {
            _UnitOfWork.Commit();
        }
    }
}
