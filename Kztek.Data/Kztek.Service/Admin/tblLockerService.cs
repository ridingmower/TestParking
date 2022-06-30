using Kztek.Data.Infrastructure;
using Kztek.Data.Repository;
using Kztek.Data.SqlHelper;
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
    public interface ItblLockerService
    {
        IQueryable<tblLocker> GetAll();

        tblLocker GetByController_Readerindex(string controllerid, string readerindex);

        IPagedList<tblLocker> GetAllPagingByFirst(string key, string controllerid, int pageNumber, int pageSize);
        tblLocker GetById(string id);
        tblLocker GetById(Guid id);
        tblLocker GetByName(string name);
        tblLocker GetByName_Id(string name, string id);

        tblLocker GetByControllerID_ReaderIndex(string controllerid, int index);

        tblLocker GetByControllerID_ReaderIndex_Id(string controllerid, int index, string id);

        MessageReport Create(tblLocker obj);
        MessageReport Update(tblLocker obj);
        MessageReport UpdateSql(tblLocker obj);
        MessageReport DeleteById(string id);

        MessageReport CreateSQL(tblLocker obj);

        IEnumerable<tblLocker> GetAllByType_Controllers(string type, List<string> Controllers);

        IEnumerable<tblLocker> GetAllByCards(List<tblCardExtend> data);
        IEnumerable<tblLocker> GetAllByCards(List<string> data);

        IEnumerable<tblLocker> GetAllByCards_Controllers(List<tblCardExtend> data, List<string> Controllers);

        IEnumerable<tblLocker> GetAllByCards_Controllers(List<string> data, List<string> Controllers);
    }

    public class tblLockerService : ItblLockerService
    {
        private readonly ItblLockerRepository _tblLockerRepository;
        private readonly IUnitOfWork _UnitOfWork;
        public tblLockerService(ItblLockerRepository _tblLockerRepository, IUnitOfWork _UnitOfWork)
        {
            this._tblLockerRepository = _tblLockerRepository;
            this._UnitOfWork = _UnitOfWork;
        }

        public IQueryable<tblLocker> GetAll()
        {
            var query = from n in _tblLockerRepository.Table
                        orderby n.ReaderIndex ascending
                        select n;
            return query;
        }
        public IPagedList<tblLocker> GetAllPagingByFirst(string key, string controllerid, int pageNumber, int pageSize)
        {
            var query = from n in _tblLockerRepository.Table select n;
            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.Name.Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(controllerid))
            {
                query = query.Where(n => n.ControllerID == controllerid);
            }

            var list = new PagedList<tblLocker>(query.OrderBy(n => n.ControllerID).ThenBy(n => n.ReaderIndex), pageNumber, pageSize);
            return list;
        }

        public tblLocker GetById(string id)
        {
            return _tblLockerRepository.GetById(id);
        }

        public MessageReport Create(tblLocker obj)
        {
            MessageReport report;
            try
            {
                _tblLockerRepository.Add(obj);
                Save();
                report = new MessageReport(true, "Thêm thành công");
            }
            catch (Exception ex)
            {
                report = new MessageReport(false, ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
            }
            return report;
        }

        public MessageReport Update(tblLocker obj)
        {
            MessageReport report;
            try
            {
                _tblLockerRepository.Update(obj);
                Save();
                report = new MessageReport(true, "Cập nhật thành công");
            }
            catch (Exception ex)
            {
                report = new MessageReport(false, ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
            }
            return report;
        }

        public MessageReport UpdateSql(tblLocker obj)
        {
            MessageReport report;
            try
            {
                var query = new StringBuilder();
                query.AppendLine("UPDATE [dbo].[tblLocker]");           
                query.AppendLine(string.Format("SET [Name] = N'{0}'",obj.Name));
                query.AppendLine(string.Format(",[ReaderIndex] = '{0}'",obj.ReaderIndex));
                query.AppendLine(string.Format(",[CardNo] = '{0}'",obj.CardNo));
                query.AppendLine(string.Format(",[CardNumber] = '{0}'",obj.CardNumber));
                query.AppendLine(string.Format(",[ControllerID] = '{0}'", obj.ControllerID));        
                query.AppendLine(string.Format(",[LockerType] = '{0}'", obj.LockerType));
                query.AppendLine(string.Format("WHERE Id = '{0}'",obj.Id));

                ExcuteSQL.Execute(query.ToString());

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
                var obj = _tblLockerRepository.GetById(id);
                if (obj != null)
                {
                    _tblLockerRepository.Delete(obj);
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

        public tblLocker GetById(Guid id)
        {
            return _tblLockerRepository.GetById(id);
        }

        public tblLocker GetByController_Readerindex(string controllerid, string readerindex)
        {
            var query = from n in _tblLockerRepository.Table
                        where n.ControllerID.Equals(controllerid) && n.ReaderIndex.Equals(readerindex)
                        select n;
            return query.FirstOrDefault();
        }

        public tblLocker GetByName(string name)
        {
            var query = from n in _tblLockerRepository.Table
                        where n.Name == name
                        select n;

            return query.FirstOrDefault();
        }

        public tblLocker GetByName_Id(string name, string id)
        {
            var query = from n in _tblLockerRepository.Table
                        where n.Name == name && n.Id != id
                        select n;

            return query.FirstOrDefault();
        }

        public tblLocker GetByControllerID_ReaderIndex(string controllerid, int index)
        {
            var query = from n in _tblLockerRepository.Table
                        where n.ControllerID == controllerid && n.ReaderIndex == index
                        select n;

            return query.FirstOrDefault();
        }

        public tblLocker GetByControllerID_ReaderIndex_Id(string controllerid, int index, string id)
        {
            var query = from n in _tblLockerRepository.Table
                        where n.ControllerID == controllerid && n.ReaderIndex == index && n.Id != id
                        select n;

            return query.FirstOrDefault();
        }

        public MessageReport CreateSQL(tblLocker obj)
        {
            var str = new StringBuilder();
            str.AppendLine("INSERT INTO tblLocker (");

            str.AppendLine("Id, Name, ReaderIndex, CardNo, CardNumber, ControllerID, DateCreated, LockerType");

            str.AppendLine(") VALUES (");

            str.AppendLine(string.Format("'{0}', N'{1}', {2}, '{3}', '{4}', '{5}', GETDATE(), '0'", obj.Id, obj.Name, obj.ReaderIndex, obj.CardNo, obj.CardNumber, obj.ControllerID));

            str.AppendLine(")");

            var result = new MessageReport(false, "Có lỗi xảy ra");

            try
            {

                ExcuteSQL.Execute(str.ToString());

            }
            catch (Exception ex)
            {
                result = new MessageReport(false, ex.Message);
            }

            return result;
        }

        public IEnumerable<tblLocker> GetAllByType_Controllers(string type, List<string> Controllers)
        {
            var query = from n in _tblLockerRepository.Table
                        where Controllers.Contains(n.ControllerID) && type.Contains(n.LockerType)
                        select n;

            return query;
        }

        public IEnumerable<tblLocker> GetAllByCards(List<tblCardExtend> data)
        {
            var k = new List<string>();
            foreach (var item in data)
            {
                k.Add(item.CardNumber);
            }

            var query = from n in _tblLockerRepository.Table
                        where k.Contains(n.CardNumber)
                        select n;

            return query;
        }

        public IEnumerable<tblLocker> GetAllByCards(List<string> data)
        {
            var query = from n in _tblLockerRepository.Table
                        where data.Contains(n.CardNumber)
                        select n;

            return query.OrderBy(n => n.ControllerID).ThenBy(n => n.ReaderIndex);
        }

        public IEnumerable<tblLocker> GetAllByCards_Controllers(List<tblCardExtend> data, List<string> Controllers)
        {
            var k = new List<string>();
            foreach (var item in data)
            {
                k.Add(item.CardNumber);
            }

            var query = from n in _tblLockerRepository.Table
                        where k.Contains(n.CardNumber) && Controllers.Contains(n.ControllerID)
                        select n;

            return query;
        }

        public IEnumerable<tblLocker> GetAllByCards_Controllers(List<string> data, List<string> Controllers)
        {
            var query = from n in _tblLockerRepository.Table
                        where data.Contains(n.CardNumber) && Controllers.Contains(n.ControllerID)
                        select n;

            return query.OrderBy(n => n.ControllerID).ThenBy(n => n.ReaderIndex);
        }
    }
}
