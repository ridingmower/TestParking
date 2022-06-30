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
    public interface ItblLockerLineService
    {
        IQueryable<tblLockerLine> GetAll();
        IQueryable<tblLockerLine> GetAllActive();

        List<tblLockerLine> GetAllActiveByListPC(string pcs);

        List<tblLockerLine> GetAllActiveByListLine(List<string> ids);

        IPagedList<tblLockerLine> GetAllPagingByFirst(string key, string pc, int pageNumber, int pageSize);
        tblLockerLine GetById(string id);
        tblLockerLine GetById(Guid id);
        MessageReport Create(tblLockerLine obj);
        MessageReport Update(tblLockerLine obj);
        MessageReport DeleteById(string id);
    }

    public class tblLockerLineService : ItblLockerLineService
    {
        private readonly ItblLockerLineRepository _tblLockerLineRepository;
        private readonly IUnitOfWork _UnitOfWork;
        public tblLockerLineService(ItblLockerLineRepository _tblLockerLineRepository, IUnitOfWork _UnitOfWork)
        {
            this._tblLockerLineRepository = _tblLockerLineRepository;
            this._UnitOfWork = _UnitOfWork;
        }

        public IQueryable<tblLockerLine> GetAll()
        {
            var query = from n in _tblLockerLineRepository.Table select n;
            return query;
        }
        public IPagedList<tblLockerLine> GetAllPagingByFirst(string key, string pc, int pageNumber, int pageSize)
        {
            var query = from n in _tblLockerLineRepository.Table
                        select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.LineName.Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(pc))
            {
                query = query.Where(n => n.PCID == pc);
            }

            var list = new PagedList<tblLockerLine>(query.OrderBy(n => n.PCID).ThenBy(n => n.LineName), pageNumber, pageSize);
            return list;
        }

        public tblLockerLine GetById(string id)
        {
            return _tblLockerLineRepository.GetById(id);
        }

        public MessageReport Create(tblLockerLine obj)
        {
            MessageReport report;
            try
            {
                _tblLockerLineRepository.Add(obj);
                Save();
                report = new MessageReport(true, "Thêm thành công");
            }
            catch (Exception ex)
            {
                report = new MessageReport(false, ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
            }
            return report;
        }

        public MessageReport Update(tblLockerLine obj)
        {
            MessageReport report;
            try
            {
                _tblLockerLineRepository.Update(obj);
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
                var obj = _tblLockerLineRepository.GetById(id);
                if (obj != null)
                {
                    _tblLockerLineRepository.Delete(obj);
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

        public tblLockerLine GetById(Guid id)
        {
            return _tblLockerLineRepository.GetById(id);
        }

        public IQueryable<tblLockerLine> GetAllActive()
        {
            var query = from n in _tblLockerLineRepository.Table
                        where n.Active == true
                        select n;
            return query;
        }

        public List<tblLockerLine> GetAllActiveByListPC(string pcs)
        {
            var query = from n in _tblLockerLineRepository.Table
                        where n.Active == true && pcs.Contains(n.PCID)
                        select n;
            return query.ToList();
        }

        public List<tblLockerLine> GetAllActiveByListLine(List<string> ids)
        {
            var query = from n in _tblLockerLineRepository.Table
                        where n.Active == true && ids.Contains(n.Id.ToString())
                        select n;

            return query.ToList();
        }
    }
}
