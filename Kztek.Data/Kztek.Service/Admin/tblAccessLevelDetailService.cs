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
    public interface ItblAccessLevelDetailService
    {
        IQueryable<tblAccessLevelDetail> GetAll();

        List<tblAccessLevelDetail> GetAllByLevelId(string id);

        IPagedList<tblAccessLevelDetail> GetAllPagingByFirst(string key, int pageNumber, int pageSize);
        tblAccessLevelDetail GetById(int id);
        MessageReport Create(tblAccessLevelDetail obj);
        MessageReport Update(tblAccessLevelDetail obj);
        MessageReport DeleteById(int id);
    }

    public class tblAccessLevelDetailService : ItblAccessLevelDetailService
    {
        private readonly ItblAccessLevelDetailRepository _tblAccessLevelDetailRepository;
        private readonly IUnitOfWork _UnitOfWork;
        public tblAccessLevelDetailService(ItblAccessLevelDetailRepository _tblAccessLevelDetailRepository, IUnitOfWork _UnitOfWork)
        {
            this._tblAccessLevelDetailRepository = _tblAccessLevelDetailRepository;
            this._UnitOfWork = _UnitOfWork;
        }

        public IQueryable<tblAccessLevelDetail> GetAll()
        {
            var query = from n in _tblAccessLevelDetailRepository.Table select n;
            return query;
        }
        public IPagedList<tblAccessLevelDetail> GetAllPagingByFirst(string key, int pageNumber, int pageSize)
        {
            var query = from n in _tblAccessLevelDetailRepository.Table select n;
            if (!string.IsNullOrWhiteSpace(key))
            {
            }
            var list = new PagedList<tblAccessLevelDetail>(query.OrderBy(n => n.Id), pageNumber, pageSize);
            return list;
        }

        public tblAccessLevelDetail GetById(int id)
        {
            return _tblAccessLevelDetailRepository.GetById(id);
        }

        public MessageReport Create(tblAccessLevelDetail obj)
        {
            MessageReport report;
            try
            {
                _tblAccessLevelDetailRepository.Add(obj);
                Save();
                report = new MessageReport(true, "Thêm thành công");
            }
            catch (Exception ex)
            {
                report = new MessageReport(false, ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
            }
            return report;
        }

        public MessageReport Update(tblAccessLevelDetail obj)
        {
            MessageReport report;
            try
            {
                _tblAccessLevelDetailRepository.Update(obj);
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
                var obj = _tblAccessLevelDetailRepository.GetById(id);
                if (obj != null)
                {
                    _tblAccessLevelDetailRepository.Delete(obj);
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

        public List<tblAccessLevelDetail> GetAllByLevelId(string id)
        {
            var query = from n in _tblAccessLevelDetailRepository.Table
                        where n.AccessLevelID == id
                        select n;
            return query.ToList();
        }
    }
}
