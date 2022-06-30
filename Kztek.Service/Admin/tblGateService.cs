using Kztek.Data.Infrastructure;
using Kztek.Data.Repository;
using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Web.Core.Functions;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Service.Admin
{
    public interface ItblGateService
    {
        IEnumerable<tblGate> GetAll();

        IEnumerable<tblGate> GetAllActive();

        IEnumerable<tblGate> GetAllByFirst(string key);

        IPagedList<tblGate> GetPagingByFirst(string key, int pageNumber, int pageSize);

        tblGate GetById(Guid id);

        tblGate GetByName(string name);

        tblGate GetByName_Id(string name, string id);

        tblGate GetByName_Id(string name, Guid id);

        MessageReport Create(tblGate obj);

        MessageReport Update(tblGate obj);

        MessageReport DeleteById(string id, ref tblGate obj);
    }

    public class tblGateService : ItblGateService
    {
        private ItblGateRepository _tblGateRepository;
        private IUnitOfWork _UnitOfWork;
        public tblGateService(ItblGateRepository _tblGateRepository, IUnitOfWork _UnitOfWork)
        {
            this._tblGateRepository = _tblGateRepository;
            this._UnitOfWork = _UnitOfWork;
        }

        private void Save()
        {
            _UnitOfWork.Commit();
        }

        public MessageReport Create(tblGate obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblGateRepository.Add(obj);

                Save();

                re.Message = FunctionHelper.GetLocalizeDictionary("Home", "notification")["addSuccess"]; ;
                re.isSuccess = true;
            }
            catch (Exception ex)
            {
                re.Message = ex.Message;
                re.isSuccess = false;
            }

            return re;
        }

        public MessageReport DeleteById(string id, ref tblGate obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                obj = GetById(Guid.Parse(id));
                if (obj != null)
                {
                    _tblGateRepository.Delete(n => n.GateID.ToString() == id);

                    Save();

                    re.Message = FunctionHelper.GetLocalizeDictionary("Home", "notification")["DeleteSuccess"];
                    re.isSuccess = true;
                }
                else
                {
                    re.Message = FunctionHelper.GetLocalizeDictionary("Home", "notification")["record_does_not_exist"];
                    re.isSuccess = false;
                }
            }
            catch (Exception ex)
            {
                re.Message = ex.Message;
                re.isSuccess = false;
            }

            return re;
        }

        public IEnumerable<tblGate> GetAll()
        {
            var query = from n in _tblGateRepository.Table
                        select n;

            return query;
        }

        public IEnumerable<tblGate> GetAllActive()
        {
            var query = from n in _tblGateRepository.Table
                        where n.Inactive == false
                        select n;

            return query;
        }

        public IEnumerable<tblGate> GetAllByFirst(string key)
        {
            var query = from n in _tblGateRepository.Table
                        select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.GateName.Contains(key));
            }

            return query;
        }

        public tblGate GetById(Guid id)
        {
            return _tblGateRepository.GetById(id);
        }

        public tblGate GetByName(string name)
        {
            var query = from n in _tblGateRepository.Table
                        where n.GateName.Equals(name)
                        select n;

            return query.FirstOrDefault();
        }

        public tblGate GetByName_Id(string name, string id)
        {
            var query = from n in _tblGateRepository.Table
                        where n.GateName.Equals(name) && n.GateID.ToString() != id
                        select n;

            return query.FirstOrDefault();
        }

        public IPagedList<tblGate> GetPagingByFirst(string key, int pageNumber, int pageSize)
        {
            var query = from n in _tblGateRepository.Table
                        select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.GateName.Contains(key));
            }

            var list = new PagedList<tblGate>(query.OrderByDescending(n => n.SortOrder), pageNumber, pageSize);

            return list;
        }

        public MessageReport Update(tblGate obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblGateRepository.Update(obj);

                Save();

                re.Message = FunctionHelper.GetLocalizeDictionary("Home", "notification")["updateSuccess"];
                re.isSuccess = true;
            }
            catch (Exception ex)
            {
                re.Message = ex.Message;
                re.isSuccess = false;
            }

            return re;
        }

        public tblGate GetByName_Id(string name, Guid id)
        {
            var query = from n in _tblGateRepository.Table
                        where n.GateName.Equals(name) && n.GateID != id
                        select n;

            return query.FirstOrDefault();
        }
    }
}
