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
    public interface ItblAccessLevelService
    {
        IQueryable<tblAccessLevel> GetAll();

        IQueryable<tblAccessLevel> GetAllActive();

        IPagedList<tblAccessLevel> GetAllPagingByFirst(string key, int pageNumber, int pageSize);
        tblAccessLevel GetById(string id);
        tblAccessLevel GetById(Guid id);

        MessageReport Create(tblAccessLevel obj);
        MessageReport Update(tblAccessLevel obj);
        MessageReport DeleteById(string id);
    }

    public class tblAccessLevelService : ItblAccessLevelService
    {
        private readonly ItblAccessLevelRepository _tblAccessLevelRepository;
        private readonly IUnitOfWork _UnitOfWork;
        public tblAccessLevelService(ItblAccessLevelRepository _tblAccessLevelRepository, IUnitOfWork _UnitOfWork)
        {
            this._tblAccessLevelRepository = _tblAccessLevelRepository;
            this._UnitOfWork = _UnitOfWork;
        }

        public IQueryable<tblAccessLevel> GetAll()
        {
            var query = from n in _tblAccessLevelRepository.Table select n;
            return query;
        }
        public IPagedList<tblAccessLevel> GetAllPagingByFirst(string key, int pageNumber, int pageSize)
        {
            var query = from n in _tblAccessLevelRepository.Table select n;
            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.AccessLevelName.Contains(key));
            }
            var list = new PagedList<tblAccessLevel>(query.OrderBy(n => n.AccessLevelName), pageNumber, pageSize);
            return list;
        }

        public tblAccessLevel GetById(string id)
        {
            return _tblAccessLevelRepository.GetById(id);
        }

        public MessageReport Create(tblAccessLevel obj)
        {
            MessageReport report;
            try
            {
                _tblAccessLevelRepository.Add(obj);
                Save();
                report = new MessageReport(true, "Thêm thành công");
            }
            catch (Exception ex)
            {
                report = new MessageReport(false, ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
            }
            return report;
        }

        public MessageReport Update(tblAccessLevel obj)
        {
            MessageReport report;
            try
            {
                _tblAccessLevelRepository.Update(obj);
                Save();
                report = new MessageReport(true, "Cập nhật thành công");
            }
            catch (Exception ex)
            {
                report = new MessageReport(false, ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
            }
            return report;
        }

        public MessageReport DeleteById(string id)
        {
            MessageReport report;
            try
            {
                var obj = _tblAccessLevelRepository.GetById(Guid.Parse(id));
                if (obj != null)
                {
                    _tblAccessLevelRepository.Delete(obj);
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

        public tblAccessLevel GetById(Guid id)
        {
            return _tblAccessLevelRepository.GetById(id);
        }

        public IQueryable<tblAccessLevel> GetAllActive()
        {
            var query = from n in _tblAccessLevelRepository.Table
                        where n.Inactive == false
                        select n;

            return query;
        }
    }
}
