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
    public interface ItblAccessTimezoneService
    {
        IQueryable<tblAccessTimezone> GetAll();
        IQueryable<tblAccessTimezone> GetAllActive();
        IPagedList<tblAccessTimezone> GetAllPagingByFirst(string key, int pageNumber, int pageSize);
        tblAccessTimezone GetById(int id);

        tblAccessTimezoneSubmit GetMappingById(int id);

        MessageReport Create(tblAccessTimezone obj);
        MessageReport Update(tblAccessTimezone obj);
        MessageReport DeleteById(int id);
    }

    public class tblAccessTimezoneService : ItblAccessTimezoneService
    {
        private readonly ItblAccessTimezoneRepository _tblAccessTimezoneRepository;
        private readonly IUnitOfWork _UnitOfWork;
        public tblAccessTimezoneService(ItblAccessTimezoneRepository _tblAccessTimezoneRepository, IUnitOfWork _UnitOfWork)
        {
            this._tblAccessTimezoneRepository = _tblAccessTimezoneRepository;
            this._UnitOfWork = _UnitOfWork;
        }
        
        public IQueryable<tblAccessTimezone> GetAll()
        {
            var query = from n in _tblAccessTimezoneRepository.Table select n;
            return query;
        }
        public IPagedList<tblAccessTimezone> GetAllPagingByFirst(string key, int pageNumber, int pageSize)
        {
            var query = from n in _tblAccessTimezoneRepository.Table select n;
            if (!string.IsNullOrWhiteSpace(key))
            {
            }
            var list = new PagedList<tblAccessTimezone>(query.OrderBy(n => n.Id), pageNumber, pageSize);
            return list;
        }

        public tblAccessTimezone GetById(int id)
        {
            return _tblAccessTimezoneRepository.GetById(id);
        }

        public MessageReport Create(tblAccessTimezone obj)
        {
            MessageReport report;
            try
            {
                _tblAccessTimezoneRepository.Add(obj);
                Save();
                report = new MessageReport(true, "Thêm thành công");
            }
            catch (Exception ex)
            {
                report = new MessageReport(false, ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
            }
            return report;
        }

        public MessageReport Update(tblAccessTimezone obj)
        {
            MessageReport report;
            try
            {
                _tblAccessTimezoneRepository.Update(obj);
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
                var obj = _tblAccessTimezoneRepository.GetById(id);
                if (obj != null)
                {
                    _tblAccessTimezoneRepository.Delete(obj);
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

        public tblAccessTimezoneSubmit GetMappingById(int id)
        {
            var obj = GetById(id);

            var objMap = new tblAccessTimezoneSubmit();
            objMap.Id = obj.Id;
            objMap.TimeZoneName = obj.TimezoneName;
            objMap.TimeZoneID = obj.TimeZoneID.Value;
            objMap.MonFrom = obj.Mon.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries)[0].ToString();
            objMap.MonTo = obj.Mon.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries)[1].ToString();
            objMap.TueFrom = obj.Tue.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries)[0].ToString();
            objMap.TueTo = obj.Tue.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries)[1].ToString();
            objMap.WedFrom = obj.Wed.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries)[0].ToString();
            objMap.WedTo = obj.Wed.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries)[1].ToString();
            objMap.ThuFrom = obj.Thu.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries)[0].ToString();
            objMap.ThuTo = obj.Thu.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries)[1].ToString();
            objMap.FriFrom = obj.Fri.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries)[0].ToString();
            objMap.FriTo = obj.Fri.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries)[1].ToString();
            objMap.SatFrom = obj.Sat.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries)[0].ToString();
            objMap.SatTo = obj.Sat.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries)[1].ToString();
            objMap.SunFrom = obj.Sun.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries)[0].ToString();
            objMap.SunTo = obj.Sun.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries)[1].ToString();
            objMap.Inactive = obj.Inactive.Value;

            return objMap;
        }

        public IQueryable<tblAccessTimezone> GetAllActive()
        {
            var query = from n in _tblAccessTimezoneRepository.Table
                        where n.Inactive == false
                        select n;
            return query;
        }
    }
}
