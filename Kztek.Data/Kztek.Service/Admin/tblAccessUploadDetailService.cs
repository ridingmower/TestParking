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
    public interface ItblAccessUploadDetailService
    {
        IQueryable<tblAccessUploadDetail> GetAll();
        IPagedList<tblAccessUploadDetail> GetAllPagingByFirst(string key, int pageNumber, int pageSize);
        tblAccessUploadDetail GetById(int id);
        MessageReport Create(tblAccessUploadDetail obj);
        MessageReport Update(tblAccessUploadDetail obj);
        MessageReport DeleteById(int id);

        List<tblAccessUploadDetail> GetAllByCardNumber(string cardnumber);

        List<tblAccessUploadDetail> GetAllByUserFinger(string userfinger);
    }

    public class tblAccessUploadDetailService : ItblAccessUploadDetailService
    {
        private readonly ItblAccessUploadDetailRepository _tblAccessUploadDetailRepository;
        private readonly IUnitOfWork _UnitOfWork;
        public tblAccessUploadDetailService(ItblAccessUploadDetailRepository _tblAccessUploadDetailRepository, IUnitOfWork _UnitOfWork)
        {
            this._tblAccessUploadDetailRepository = _tblAccessUploadDetailRepository;
            this._UnitOfWork = _UnitOfWork;
        }

        public IQueryable<tblAccessUploadDetail> GetAll()
        {
            var query = from n in _tblAccessUploadDetailRepository.Table select n;
            return query;
        }
        public IPagedList<tblAccessUploadDetail> GetAllPagingByFirst(string key, int pageNumber, int pageSize)
        {
            var query = from n in _tblAccessUploadDetailRepository.Table select n;
            if (!string.IsNullOrWhiteSpace(key))
            {
            }
            var list = new PagedList<tblAccessUploadDetail>(query.OrderBy(n => n.Id), pageNumber, pageSize);
            return list;
        }

        public tblAccessUploadDetail GetById(int id)
        {
            return _tblAccessUploadDetailRepository.GetById(id);
        }

        public MessageReport Create(tblAccessUploadDetail obj)
        {
            MessageReport report;
            try
            {
                _tblAccessUploadDetailRepository.Add(obj);
                Save();
                report = new MessageReport(true, "Thêm thành công");
            }
            catch (Exception ex)
            {
                report = new MessageReport(false, ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
            }
            return report;
        }

        public MessageReport Update(tblAccessUploadDetail obj)
        {
            MessageReport report;
            try
            {
                _tblAccessUploadDetailRepository.Update(obj);
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
                var obj = _tblAccessUploadDetailRepository.GetById(id);
                if (obj != null)
                {
                    _tblAccessUploadDetailRepository.Delete(obj);
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

        public List<tblAccessUploadDetail> GetAllByCardNumber(string cardnumber)
        {
            var query = from n in _tblAccessUploadDetailRepository.Table
                        where n.CardNumber == cardnumber
                        select n;

            return query.ToList();
        }

        public List<tblAccessUploadDetail> GetAllByUserFinger(string userfinger)
        {
            var query = from n in _tblAccessUploadDetailRepository.Table
                        where n.UserIDofFinger == userfinger
                        select n;

            return query.ToList();
        }
    }
}
