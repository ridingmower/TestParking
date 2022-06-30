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
    public interface ItblLockerControllerService
    {
        IQueryable<tblLockerController> GetAll();
        IQueryable<tblLockerController> GetAllActive();
        IQueryable<tblLockerController> GetAllActiveByLineId(string id);
        IQueryable<tblLockerController> GetAllByListId(List<string> Ids);
        IPagedList<tblLockerController> GetAllPagingByFirst(string key, string line, int pageNumber, int pageSize);
        IPagedList<tblLockerController> GetAllPagingByFirst(string key, string computerids, string line, int pageNumber, int pageSize);

        IEnumerable<tblLockerController> GetAllByFirst(string key, string computerids, string line);

        IEnumerable<tblLockerController> GetAllActiveByListId(string ids);

        IEnumerable<tblLockerController> GetAllActiveByListId(List<string> ids);

        tblLockerController GetById(string id);
        tblLockerController GetById(Guid id);

        MessageReport Create(tblLockerController obj);
        MessageReport Update(tblLockerController obj);
        MessageReport DeleteById(string id);
    }

    public class tblLockerControllerService : ItblLockerControllerService
    {
        private readonly ItblLockerControllerRepository _tblLockerControllerRepository;
        private readonly ItblLockerLineService _tblLockerLineService;
        private readonly IUnitOfWork _UnitOfWork;

        public tblLockerControllerService(ItblLockerControllerRepository _tblLockerControllerRepository, ItblLockerLineService _tblLockerLineService, IUnitOfWork _UnitOfWork)
        {
            this._tblLockerControllerRepository = _tblLockerControllerRepository;
            this._tblLockerLineService = _tblLockerLineService;
            this._UnitOfWork = _UnitOfWork;
        }

        public IQueryable<tblLockerController> GetAll()
        {
            var query = from n in _tblLockerControllerRepository.Table select n;
            return query;
        }
        public IPagedList<tblLockerController> GetAllPagingByFirst(string key, string line, int pageNumber, int pageSize)
        {
            var query = from n in _tblLockerControllerRepository.Table
                        select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.ControllerName.Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(line))
            {
                query = query.Where(n => n.LineID == line);
            }

            var list = new PagedList<tblLockerController>(query.OrderBy(n => n.Id), pageNumber, pageSize);
            return list;
        }

        public tblLockerController GetById(string id)
        {
            return _tblLockerControllerRepository.GetById(id);
        }

        public MessageReport Create(tblLockerController obj)
        {
            MessageReport report;
            try
            {
                _tblLockerControllerRepository.Add(obj);
                Save();
                report = new MessageReport(true, "Thêm thành công");
            }
            catch (Exception ex)
            {
                report = new MessageReport(false, ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
            }
            return report;
        }

        public MessageReport Update(tblLockerController obj)
        {
            MessageReport report;

            try
            {
                _tblLockerControllerRepository.Update(obj);
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
                var obj = _tblLockerControllerRepository.GetById(id);
                if (obj != null)
                {
                    _tblLockerControllerRepository.Delete(obj);
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

        public tblLockerController GetById(Guid id)
        {
            return _tblLockerControllerRepository.GetById(id);
        }

        public IQueryable<tblLockerController> GetAllActive()
        {
            var query = from n in _tblLockerControllerRepository.Table
                        where n.Active == true
                        orderby n.Address ascending
                        select n;
            return query;
        }

        public IQueryable<tblLockerController> GetAllByListId(List<string> Ids)
        {
            var query = from n in _tblLockerControllerRepository.Table
                        where n.Active == true
                        select n;

            if (Ids != null && Ids.Any())
            {
                query = query.Where(n => Ids.Contains(n.Id.ToString()));
            }

            return query;
        }

        public IPagedList<tblLockerController> GetAllPagingByFirst(string key, string computerids, string line, int pageNumber, int pageSize)
        {
            var query = from n in _tblLockerControllerRepository.Table
                        select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.ControllerName.Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(computerids))
            {
                var lines = _tblLockerLineService.GetAllActiveByListPC(computerids);
                if (lines.Any())
                {
                    var str = "";
                    foreach (var item in lines)
                    {
                        str += item.Id;
                    }

                    query = query.Where(n => str.Contains(n.LineID));
                }
            }

            var list = new PagedList<tblLockerController>(query.OrderBy(n => n.LineID), pageNumber, pageSize);
            return list;
        }

        public IEnumerable<tblLockerController> GetAllActiveByListId(string ids)
        {
            var query = from n in _tblLockerControllerRepository.Table
                        where n.Active == true && ids.Contains(n.Id.ToString())
                        select n;

            return query;
        }

        public IEnumerable<tblLockerController> GetAllActiveByListId(List<string> ids)
        {
            var query = from n in _tblLockerControllerRepository.Table
                        where n.Active == true && ids.Contains(n.Id.ToString())
                        select n;

            return query;
        }

        public IEnumerable<tblLockerController> GetAllByFirst(string key, string computerids, string line)
        {
            var query = from n in _tblLockerControllerRepository.Table
                        select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.ControllerName.Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(computerids))
            {
                var lines = _tblLockerLineService.GetAllActiveByListPC(computerids);
                if (lines.Any())
                {
                    var str = "";
                    foreach (var item in lines)
                    {
                        str += item.Id;
                    }

                    query = query.Where(n => str.Contains(n.LineID));
                }
            }

            return query;
        }

        public IQueryable<tblLockerController> GetAllActiveByLineId(string id)
        {
            var query = from n in _tblLockerControllerRepository.Table
                        where n.Active == true
                        select n;

            if (!string.IsNullOrWhiteSpace(id))
            {
                query = query.Where(n => n.LineID == id);
            }

            return query;
        }
    }
}
