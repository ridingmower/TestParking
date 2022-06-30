using Kztek.Data.Infrastructure;
using Kztek.Data.Repository;
using Kztek.Model.CustomModel;
using Kztek.Model.CustomModel.iParking;
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
    public interface ItblPCService
    {
        IEnumerable<tblPC> GetAll();

        IEnumerable<tblPC> GetAllActive();

        IEnumerable<tblPC> GetAllByGateId(string id);

        IEnumerable<tblPC> GetAllPagingByFirst(string key, string gate, int pageNumber, int pageSize, ref int totalPage, ref int totalItem);

        IEnumerable<tblPC> GetAllByListId(string ids);

        IPagedList<tblPCCustomViewModel> GetAllCustomPagingByFirst(string key, string gate, int pageNumber, int pageSize);

        tblPC GetById(Guid id);

        tblPC GetByAddressIPORAddressName(string addressip, string addressname);

        MessageReport Create(tblPC obj);

        MessageReport Update(tblPC obj);

        tblPC GetByName(string name);

        tblPC GetByName_Id(string name, Guid id);

        tblPC GetByAddress(string address);

        tblPC GetByAddress_Id(string address, Guid id);

        MessageReport DeleteById(string id, ref tblPC obj);
      
    }

    public class tblPCService : ItblPCService
    {
        private ItblPCRepository _tblPCRepository;
        private ItblGateRepository _tblGateRepository;
        private IUnitOfWork _UnitOfWork;

        public tblPCService(ItblPCRepository _tblPCRepository, ItblGateRepository _tblGateRepository, IUnitOfWork _UnitOfWork)
        {
            this._tblPCRepository = _tblPCRepository;
            this._tblGateRepository = _tblGateRepository;
            this._UnitOfWork = _UnitOfWork;
        }

        private void Save()
        {
            _UnitOfWork.Commit();
        }

        public MessageReport Create(tblPC obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblPCRepository.Add(obj);

                Save();

                re.Message = FunctionHelper.GetLocalizeDictionary("Home", "notification")["addSuccess"];
                re.isSuccess = true;
            }
            catch (Exception ex)
            {
                re.Message = ex.Message;
                re.isSuccess = false;
            }

            return re;
        }

        public MessageReport DeleteById(string id, ref tblPC obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                obj = GetById(Guid.Parse(id));
                if (obj != null)
                {
                    _tblPCRepository.Delete(n => n.PCID.ToString() == id);

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

        public IEnumerable<tblPC> GetAll()
        {
            var query = from n in _tblPCRepository.Table
                        select n;

            return query;
        }

        public IEnumerable<tblPC> GetAllActive()
        {
            var query = from n in _tblPCRepository.Table
                        where n.Inactive == false
                        select n;

            return query;
        }

        public IEnumerable<tblPC> GetAllByGateId(string id)
        {
            var query = from n in _tblPCRepository.Table
                        where n.GateID == id
                        select n;

            return query;
        }

        public IEnumerable<tblPC> GetAllByListId(string ids)
        {
            var query = from n in _tblPCRepository.Table
                        where ids.Contains(n.PCID.ToString())
                        select n;

            return query;
        }

        public IPagedList<tblPCCustomViewModel> GetAllCustomPagingByFirst(string key, string gate, int pageNumber, int pageSize)
        {
            var query = (from n in _tblPCRepository.Table
                         join m in _tblGateRepository.Table on n.GateID equals m.GateID.ToString() into n_m
                         from m in n_m.DefaultIfEmpty()
                         select new tblPCCustomViewModel()
                         {
                             ComputerName = n.ComputerName,
                             Description = n.Description,
                             GateID = n.GateID,
                             GateName = m.GateName,
                             Inactive = n.Inactive,
                             IPAddress = n.IPAddress,
                             PCID = n.PCID.ToString(),
                             SortOrder = n.SortOrder
                         });

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.GateName.Contains(key) || n.ComputerName.Contains(key) || n.Description.Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(gate))
            {
                query = query.Where(n => n.GateID == gate);
            }

            var list = new PagedList<tblPCCustomViewModel>(query.OrderByDescending(n => n.SortOrder), pageNumber, pageSize);

            return list;
        }

        public IEnumerable<tblPC> GetAllPagingByFirst(string key, string gate, int pageNumber, int pageSize, ref int totalPage, ref int totalItem)
        {
            var query = from n in _tblPCRepository.Table
                        select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.Description.Contains(key) || n.ComputerName.Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(gate))
            {
                query = query.Where(n => n.GateID == gate);
            }

            var list = new PagedList<tblPC>(query.OrderByDescending(n => n.SortOrder), pageNumber, pageSize);

            return list;
        }

        public tblPC GetByAddressIPORAddressName(string addressip, string addressname)
        {
            var query = from n in _tblPCRepository.Table
                        where n.IPAddress == addressip.Trim() && n.IPAddress == addressname.Trim()
                        select n;

            return query.FirstOrDefault();
        }

        public tblPC GetById(Guid id)
        {
            return _tblPCRepository.GetById(id);
        }

        public MessageReport Update(tblPC obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblPCRepository.Update(obj);

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

        public tblPC GetByName(string name)
        {
            var query = from n in _tblPCRepository.Table
                        where n.ComputerName == name
                        select n;

            return query.FirstOrDefault();
        }

        public tblPC GetByName_Id(string name, Guid id)
        {
            var query = from n in _tblPCRepository.Table
                        where n.ComputerName == name && n.PCID != id
                        select n;

            return query.FirstOrDefault();
        }

        public tblPC GetByAddress(string address)
        {
            var query = from n in _tblPCRepository.Table
                        where n.IPAddress == address
                        select n;

            return query.FirstOrDefault();
        }

        public tblPC GetByAddress_Id(string address, Guid id)
        {
            var query = from n in _tblPCRepository.Table
                        where n.IPAddress == address && n.PCID != id
                        select n;

            return query.FirstOrDefault();
        }
    }
}
