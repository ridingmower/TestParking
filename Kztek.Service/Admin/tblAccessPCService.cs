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
    public interface ItblAccessPCService
    {
        IQueryable<tblAccessPC> GetAll();

        IEnumerable<tblAccessPC> GetAllActive();

        IEnumerable<tblAccessPC> GetAllActiveByListLineId(List<string> ids);

        IPagedList<tblAccessPC> GetAllPagingByFirst(string key, int pageNumber, int pageSize);

        tblAccessPC GetById(string id);
        tblAccessPC GetById(Guid id);

        MessageReport Create(tblAccessPC obj);
        MessageReport Update(tblAccessPC obj);
        MessageReport DeleteById(string id);
    }

    public class tblAccessPCService : ItblAccessPCService
    {
        private readonly ItblAccessPCRepository _tblAccessPCRepository;
        private readonly ItblAccessLineRepository _tblAccessLineRepository;
        private readonly IUnitOfWork _UnitOfWork;
        public tblAccessPCService(ItblAccessPCRepository _tblAccessPCRepository, ItblAccessLineRepository _tblAccessLineRepository, IUnitOfWork _UnitOfWork)
        {
            this._tblAccessPCRepository = _tblAccessPCRepository;
            this._tblAccessLineRepository = _tblAccessLineRepository;
            this._UnitOfWork = _UnitOfWork;
        }

        public IQueryable<tblAccessPC> GetAll()
        {
            var query = from n in _tblAccessPCRepository.Table select n;
            return query;
        }

        public IPagedList<tblAccessPC> GetAllPagingByFirst(string key, int pageNumber, int pageSize)
        {
            var query = from n in _tblAccessPCRepository.Table select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
            }

            var list = new PagedList<tblAccessPC>(query.OrderBy(n => n.PCID), pageNumber, pageSize);
            return list;
        }

        public tblAccessPC GetById(string id)
        {
            return _tblAccessPCRepository.GetById(id);
        }

        public MessageReport Create(tblAccessPC obj)
        {
            MessageReport report;
            try
            {
                _tblAccessPCRepository.Add(obj);
                Save();
                report = new MessageReport(true, "Thêm thành công");
            }
            catch (Exception ex)
            {
                report = new MessageReport(false, ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
            }
            return report;
        }

        public MessageReport Update(tblAccessPC obj)
        {
            MessageReport report;
            try
            {
                _tblAccessPCRepository.Update(obj);
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
                var obj = _tblAccessPCRepository.GetById(Guid.Parse(id));
                if (obj != null)
                {
                    _tblAccessPCRepository.Delete(obj);
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

        public tblAccessPC GetById(Guid id)
        {
            return _tblAccessPCRepository.GetById(id);
        }

        public IEnumerable<tblAccessPC> GetAllActive()
        {
            var query = from n in _tblAccessPCRepository.Table
                        where n.Inactive == false
                        orderby n.PCName ascending
                        select n;
            return query;
        }

        public IEnumerable<tblAccessPC> GetAllActiveByListLineId(List<string> ids)
        {
            var query = from pc in _tblAccessPCRepository.Table
                        join li in _tblAccessLineRepository.Table on pc.PCID.ToString() equals li.PCID
                        where ids.Contains(li.LineID.ToString())
                        select pc;

            return query;
        }
    }
}
