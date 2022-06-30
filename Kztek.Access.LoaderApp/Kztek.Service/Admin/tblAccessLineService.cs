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
    public interface ItblAccessLineService
    {
        IQueryable<tblAccessLine> GetAll();
        IQueryable<tblAccessLine> GetAllActive();

        List<tblAccessLine> GetAllActiveByListPC(string pcs);

        List<tblAccessLine> GetAllActiveByListLine(List<string> ids);

        IPagedList<tblAccessLine> GetAllPagingByFirst(string key, string pc, int pageNumber, int pageSize);
        tblAccessLine GetById(string id);
        tblAccessLine GetById(Guid id);
        MessageReport Create(tblAccessLine obj);
        MessageReport Update(tblAccessLine obj);
        MessageReport DeleteById(string id);
    }

    public class tblAccessLineService : ItblAccessLineService
    {
        private readonly ItblAccessLineRepository _tblAccessLineRepository;
        private readonly IUnitOfWork _UnitOfWork;
        public tblAccessLineService(ItblAccessLineRepository _tblAccessLineRepository, IUnitOfWork _UnitOfWork)
        {
            this._tblAccessLineRepository = _tblAccessLineRepository;
            this._UnitOfWork = _UnitOfWork;
        }

        public IQueryable<tblAccessLine> GetAll()
        {
            var query = from n in _tblAccessLineRepository.Table select n;
            return query;
        }
        public IPagedList<tblAccessLine> GetAllPagingByFirst(string key, string pc, int pageNumber, int pageSize)
        {
            var query = from n in _tblAccessLineRepository.Table
                        select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.LineName.Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(pc))
            {
                query = query.Where(n => n.PCID == pc);
            }

            var list = new PagedList<tblAccessLine>(query.OrderBy(n => n.PCID).ThenBy(n => n.LineName), pageNumber, pageSize);
            return list;
        }

        public tblAccessLine GetById(string id)
        {
            return _tblAccessLineRepository.GetById(id);
        }

        public MessageReport Create(tblAccessLine obj)
        {
            MessageReport report;
            try
            {
                _tblAccessLineRepository.Add(obj);
                Save();
                report = new MessageReport(true, "Thêm thành công");
            }
            catch (Exception ex)
            {
                report = new MessageReport(false, ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
            }
            return report;
        }

        public MessageReport Update(tblAccessLine obj)
        {
            MessageReport report;
            try
            {
                _tblAccessLineRepository.Update(obj);
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
                var obj = _tblAccessLineRepository.GetById(Guid.Parse(id));
                if (obj != null)
                {
                    _tblAccessLineRepository.Delete(obj);
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

        public tblAccessLine GetById(Guid id)
        {
            return _tblAccessLineRepository.GetById(id);
        }

        public IQueryable<tblAccessLine> GetAllActive()
        {
            var query = from n in _tblAccessLineRepository.Table
                        where n.Inactive == false
                        select n;
            return query;
        }

        public List<tblAccessLine> GetAllActiveByListPC(string pcs)
        {
            var query = from n in _tblAccessLineRepository.Table
                        where n.Inactive == false
                        select n;

            if (!string.IsNullOrEmpty(pcs))
            {
                query = query.Where(n => pcs.Contains(n.PCID));
            }

            return query.ToList();
        }

        public List<tblAccessLine> GetAllActiveByListLine(List<string> ids)
        {
            var query = from n in _tblAccessLineRepository.Table
                        where n.Inactive == false && ids.Contains(n.LineID.ToString())
                        select n;

            return query.ToList();
        }
    }
}
