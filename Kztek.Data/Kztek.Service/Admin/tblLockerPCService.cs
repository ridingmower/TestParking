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
    public interface ItblLockerPCService
    {
        IQueryable<tblLockerPC> GetAll();

        IEnumerable<tblLockerPC> GetAllActive();

        IEnumerable<tblLockerPC> GetAllActiveByListLineId(List<string> ids);

        IPagedList<tblLockerPC> GetAllPagingByFirst(string key, int pageNumber, int pageSize);

        tblLockerPC GetById(string id);
        tblLockerPC GetById(Guid id);

        MessageReport Create(tblLockerPC obj);
        MessageReport Update(tblLockerPC obj);
        MessageReport DeleteById(string id);
    }

    public class tblLockerPCService : ItblLockerPCService
    {
        private readonly ItblLockerPCRepository _tblLockerPCRepository;
        private readonly ItblLockerLineRepository _tblLockerLineRepository;
        private readonly IUnitOfWork _UnitOfWork;

        public tblLockerPCService(ItblLockerPCRepository _tblLockerPCRepository, ItblLockerLineRepository _tblLockerLineRepository, IUnitOfWork _UnitOfWork)
        {
            this._tblLockerPCRepository = _tblLockerPCRepository;
            this._tblLockerLineRepository = _tblLockerLineRepository;
            this._UnitOfWork = _UnitOfWork;

        }

        public IQueryable<tblLockerPC> GetAll()
        {
            var query = from n in _tblLockerPCRepository.Table select n;
            return query;
        }

        public IPagedList<tblLockerPC> GetAllPagingByFirst(string key, int pageNumber, int pageSize)
        {
            var query = from n in _tblLockerPCRepository.Table select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
            }

            var list = new PagedList<tblLockerPC>(query.OrderBy(n => n.Id), pageNumber, pageSize);
            return list;
        }

        public tblLockerPC GetById(string id)
        {
            return _tblLockerPCRepository.GetById(id);
        }

        public MessageReport Create(tblLockerPC obj)
        {
            MessageReport report;
            try
            {
                _tblLockerPCRepository.Add(obj);
                Save();
                report = new MessageReport(true, "Thêm thành công");
            }
            catch (Exception ex)
            {
                report = new MessageReport(false, ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
            }
            return report;
        }

        public MessageReport Update(tblLockerPC obj)
        {
            MessageReport report;
            try
            {
                _tblLockerPCRepository.Update(obj);
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
                var obj = _tblLockerPCRepository.GetById(id);
                if (obj != null)
                {
                    _tblLockerPCRepository.Delete(obj);
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

        public tblLockerPC GetById(Guid id)
        {
            return _tblLockerPCRepository.GetById(id);
        }

        public IEnumerable<tblLockerPC> GetAllActive()
        {
            var query = from n in _tblLockerPCRepository.Table
                        where n.Active == true
                        orderby n.PCName ascending
                        select n;
            return query;
        }

        public IEnumerable<tblLockerPC> GetAllActiveByListLineId(List<string> ids)
        {
            var query = from pc in _tblLockerPCRepository.Table
                        join li in _tblLockerLineRepository.Table on pc.Id.ToString() equals li.PCID
                        where ids.Contains(li.Id.ToString())
                        select pc;

            return query;
        }
    }
}
