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
    public interface ItblAccessControllerGroupService
    {
        IQueryable<tblAccessControllerGroup> GetAll();

        IPagedList<tblAccessControllerGroup> GetAllPagingByFirst(string key, int pageNumber, int pageSize);
        tblAccessControllerGroup GetById(string id);
        tblAccessControllerGroup GetById(Guid id);
        MessageReport Create(tblAccessControllerGroup obj);
        MessageReport Update(tblAccessControllerGroup obj);
        MessageReport DeleteById(string id);
    }

    public class tblAccessControllerGroupService : ItblAccessControllerGroupService
    {
        private readonly ItblAccessControllerGroupRepository _tblAccessControllerGroupRepository;
        private readonly IUnitOfWork _UnitOfWork;
        public tblAccessControllerGroupService(ItblAccessControllerGroupRepository _tblAccessControllerGroupRepository, IUnitOfWork _UnitOfWork)
        {
            this._tblAccessControllerGroupRepository = _tblAccessControllerGroupRepository;
            this._UnitOfWork = _UnitOfWork;
        }

        public IQueryable<tblAccessControllerGroup> GetAll()
        {
            var query = from n in _tblAccessControllerGroupRepository.Table select n;
            return query;
        }
        public IPagedList<tblAccessControllerGroup> GetAllPagingByFirst(string key, int pageNumber, int pageSize)
        {
            var query = from n in _tblAccessControllerGroupRepository.Table select n;
            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.Name.Contains(key));
            }

            var list = new PagedList<tblAccessControllerGroup>(query.OrderBy(n => n.SortOrder), pageNumber, pageSize);
            return list;
        }

        public tblAccessControllerGroup GetById(string id)
        {
            return _tblAccessControllerGroupRepository.GetById(id);
        }

        public MessageReport Create(tblAccessControllerGroup obj)
        {
            MessageReport report;
            try
            {
                _tblAccessControllerGroupRepository.Add(obj);
                Save();
                report = new MessageReport(true, "Thêm thành công");
            }
            catch (Exception ex)
            {
                report = new MessageReport(false, ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
            }
            return report;
        }

        public MessageReport Update(tblAccessControllerGroup obj)
        {
            MessageReport report;
            try
            {
                _tblAccessControllerGroupRepository.Update(obj);
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
                var obj = _tblAccessControllerGroupRepository.GetById(id);
                if (obj != null)
                {
                    _tblAccessControllerGroupRepository.Delete(obj);
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

        public tblAccessControllerGroup GetById(Guid id)
        {
            return _tblAccessControllerGroupRepository.GetById(id);
        }


        //public tblAccessControllerGroup GetByController_Readerindex(string controllerid, string readerindex)
        //{
        //    var query = from n in _tblAccessControllerGroupRepository.Table
        //                where n.Inactive == false && n.ControllerID.Equals(controllerid) && n.ReaderIndex.Equals(readerindex)
        //                select n;
        //    return query.FirstOrDefault();
        //}
    }
}
