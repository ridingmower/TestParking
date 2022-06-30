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
    public interface ItblAccessDoorService
    {
        IQueryable<tblAccessDoor> GetAll();

        IQueryable<tblAccessDoor> GetAllActive();

        tblAccessDoor GetByController_Readerindex(string controllerid, string readerindex);

        IPagedList<tblAccessDoor> GetAllPagingByFirst(string key, string controllerid, int pageNumber, int pageSize);
        tblAccessDoor GetById(string id);
        tblAccessDoor GetById(Guid id);
        MessageReport Create(tblAccessDoor obj);
        MessageReport Update(tblAccessDoor obj);
        MessageReport DeleteById(string id);
    }

    public class tblAccessDoorService : ItblAccessDoorService
    {
        private readonly ItblAccessDoorRepository _tblAccessDoorRepository;
        private readonly IUnitOfWork _UnitOfWork;
        public tblAccessDoorService(ItblAccessDoorRepository _tblAccessDoorRepository, IUnitOfWork _UnitOfWork)
        {
            this._tblAccessDoorRepository = _tblAccessDoorRepository;
            this._UnitOfWork = _UnitOfWork;
        }

        public IQueryable<tblAccessDoor> GetAll()
        {
            var query = from n in _tblAccessDoorRepository.Table select n;
            return query;
        }
        public IPagedList<tblAccessDoor> GetAllPagingByFirst(string key, string controllerid, int pageNumber, int pageSize)
        {
            var query = from n in _tblAccessDoorRepository.Table select n;
            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.DoorName.Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(controllerid))
            {
                query = query.Where(n => n.ControllerID == controllerid);
            }

            var list = new PagedList<tblAccessDoor>(query.OrderBy(n => n.ControllerID).ThenBy(n => n.Ordering), pageNumber, pageSize);
            return list;
        }

        public tblAccessDoor GetById(string id)
        {
            return _tblAccessDoorRepository.GetById(id);
        }

        public MessageReport Create(tblAccessDoor obj)
        {
            MessageReport report;
            try
            {
                _tblAccessDoorRepository.Add(obj);
                Save();
                report = new MessageReport(true, "Thêm thành công");
            }
            catch (Exception ex)
            {
                report = new MessageReport(false, ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
            }
            return report;
        }

        public MessageReport Update(tblAccessDoor obj)
        {
            MessageReport report;
            try
            {
                _tblAccessDoorRepository.Update(obj);
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
                var obj = _tblAccessDoorRepository.GetById(Guid.Parse(id));
                if (obj != null)
                {
                    _tblAccessDoorRepository.Delete(obj);
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

        public tblAccessDoor GetById(Guid id)
        {
            return _tblAccessDoorRepository.GetById(id);
        }

        public IQueryable<tblAccessDoor> GetAllActive()
        {
            var query = from n in _tblAccessDoorRepository.Table
                        where n.Inactive == false
                        select n;
            return query;
        }

        public tblAccessDoor GetByController_Readerindex(string controllerid, string readerindex)
        {
            var query = from n in _tblAccessDoorRepository.Table
                        where n.Inactive == false && n.ControllerID.Equals(controllerid) && n.ReaderIndex.Equals(readerindex)
                        select n;
            return query.FirstOrDefault();
        }
    }
}
