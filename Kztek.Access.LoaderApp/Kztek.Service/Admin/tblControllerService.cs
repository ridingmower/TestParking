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
    public interface ItblControllerService
    {
        IEnumerable<tblController> GetAll();

        IEnumerable<tblController> GetAllByPC(string id);

        IEnumerable<tblController> GetAllActive();

        IEnumerable<tblController> GetAllPagingByFirst(string key, string pc, int pageNumber, int pageSize, ref int totalPage, ref int totalItem);

        IPagedList<tblControllerCustomViewModel> GetAllCustomPagingByFirst(string key, string pc, int pageNumber, int pageSize);

        tblController GetById(Guid id);

        tblController GetByName(string name);

        tblController GetByName_Id(string name, Guid id);

        MessageReport Create(tblController obj);

        MessageReport Update(tblController obj);

        MessageReport DeleteById(string id, ref tblController obj);
    }

    public class tblControllerService : ItblControllerService
    {
        private ItblControllerRepository _tblControllerRepository;
        private ItblPCRepository _tblPCRepository;
        private IUnitOfWork _UnitOfWork;

        public tblControllerService(ItblControllerRepository _tblControllerRepository, ItblPCRepository _tblPCRepository, IUnitOfWork _UnitOfWork)
        {
            this._tblControllerRepository = _tblControllerRepository;
            this._tblPCRepository = _tblPCRepository;
            this._UnitOfWork = _UnitOfWork;
        }

        private void Save()
        {
            _UnitOfWork.Commit();
        }

        public MessageReport Create(tblController obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblControllerRepository.Add(obj);

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

        public MessageReport DeleteById(string id, ref tblController obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                obj = GetById(Guid.Parse(id));
                if (obj != null)
                {
                    _tblControllerRepository.Delete(n => n.ControllerID.ToString() == id);

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

        public IEnumerable<tblController> GetAll()
        {
            var query = from n in _tblControllerRepository.Table
                        select n;

            return query;
        }

        public IEnumerable<tblController> GetAllActive()
        {
            var query = from n in _tblControllerRepository.Table
                        where n.Inactive == false
                        select n;

            return query;
        }

        public IEnumerable<tblController> GetAllByPC(string id)
        {
            var query = from n in _tblControllerRepository.Table
                        where n.PCID == id
                        select n;

            return query;
        }

        public IPagedList<tblControllerCustomViewModel> GetAllCustomPagingByFirst(string key, string pc, int pageNumber, int pageSize)
        {
            var query = (from n in _tblControllerRepository.Table
                         join m in _tblPCRepository.Table on n.PCID equals m.PCID.ToString() into n_m
                         from m in n_m.DefaultIfEmpty()

                         select new tblControllerCustomViewModel()
                         {
                             ControllerID = n.ControllerID.ToString(),
                             PCID = n.PCID,
                             PCName = m.ComputerName,
                             Comport = n.Comport,
                             ControllerName = n.ControllerName,
                             Inactive = n.Inactive,
                             SortOrder = n.SortOrder
                         });

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.ControllerName.Contains(key) || n.PCName.Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(pc))
            {
                query = query.Where(n => n.PCID == pc);
            }

            var list = new PagedList<tblControllerCustomViewModel>(query.OrderByDescending(n => n.SortOrder), pageNumber, pageSize);

            return list;
        }

        public IEnumerable<tblController> GetAllPagingByFirst(string key, string pc, int pageNumber, int pageSize, ref int totalPage, ref int totalItem)
        {
            var query = from n in _tblControllerRepository.Table
                        select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.ControllerName.Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(pc))
            {
                query = query.Where(n => n.PCID == pc);
            }

            var list = new PagedList<tblController>(query.OrderByDescending(n => n.SortOrder), pageNumber, pageSize);

            return list;
        }

        public tblController GetById(Guid id)
        {
            return _tblControllerRepository.GetById(id);
        }

        public MessageReport Update(tblController obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblControllerRepository.Update(obj);

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

        public tblController GetByName(string name)
        {
            var query = from n in _tblControllerRepository.Table
                        where n.ControllerName == name
                        select n;

            return query.FirstOrDefault();
        }

        public tblController GetByName_Id(string name, Guid id)
        {
            var query = from n in _tblControllerRepository.Table
                        where n.ControllerName == name && n.ControllerID != id
                        select n;

            return query.FirstOrDefault();
        }
    }
}
