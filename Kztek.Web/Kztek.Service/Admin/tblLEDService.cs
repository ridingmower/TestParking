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
    public interface ItblLEDService
    {
        IEnumerable<tblLED> GetAll();

        IEnumerable<tblLED> GetAllByPC(string id);

        IEnumerable<tblLED> GetAllPagingByFirst(string key, string pc, int pageNumber, int pageSize, ref int totalPage, ref int totalItem);

        IPagedList<tblLEDCustomViewModel> GetAllCustomPagingByFirst(string key, string pc, int pageNumber, int pageSize);

        tblLED GetById(int id);

        tblLED GetByName(string name);

        tblLED GetByName_Id(string name, string id);

        MessageReport Create(tblLED obj);

        MessageReport Update(tblLED obj);

        MessageReport DeleteById(string id, ref tblLED obj);
    }

    public class tblLEDService : ItblLEDService
    {
        private ItblLEDRepository _tblLEDRepository;
        private ItblPCRepository _tblPCRepository;
        private IUnitOfWork _UnitOfWork;

        public tblLEDService(ItblLEDRepository _tblLEDRepository, ItblPCRepository _tblPCRepository, IUnitOfWork _UnitOfWork)
        {
            this._tblLEDRepository = _tblLEDRepository;
            this._tblPCRepository = _tblPCRepository;
            this._UnitOfWork = _UnitOfWork;
        }

        private void Save()
        {
            _UnitOfWork.Commit();
        }

        public MessageReport Create(tblLED obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblLEDRepository.Add(obj);

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

        public MessageReport DeleteById(string id, ref tblLED obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                obj = GetById(Convert.ToInt32(id));
                if (obj != null)
                {
                    _tblLEDRepository.Delete(n => n.LEDID.ToString() == id);

                    Save();

                    re.Message = FunctionHelper.GetLocalizeDictionary("Home", "notification")["DeleteSuccess"]; ;
                    re.isSuccess = true;
                }
                else
                {
                    re.Message = FunctionHelper.GetLocalizeDictionary("Home", "notification")["record_does_not_exist"]; ;
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

        public IEnumerable<tblLED> GetAll()
        {
            var query = from n in _tblLEDRepository.Table
                        select n;

            return query;
        }

        public IEnumerable<tblLED> GetAllByPC(string id)
        {
            var query = from n in _tblLEDRepository.Table
                        where n.PCID == id
                        select n;

            return query;
        }

        public IPagedList<tblLEDCustomViewModel> GetAllCustomPagingByFirst(string key, string pc, int pageNumber, int pageSize)
        {
            var query = (from n in _tblLEDRepository.Table
                         join m in _tblPCRepository.Table on n.PCID equals m.PCID.ToString() into n_m
                         from m in n_m.DefaultIfEmpty()
                         select new tblLEDCustomViewModel()
                         {
                             Baudrate = n.Baudrate,
                             Comport = n.Comport,
                             EnableLED = n.EnableLED,
                             LEDID = n.LEDID.ToString(),
                             LedType = n.LedType,
                             Name = n.LEDName,
                             PCID = n.PCID,
                             PCName = m.ComputerName,
                             SortOrder = 0
                         });

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.Name.Contains(key) || n.PCName.Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(pc))
            {
                query = query.Where(n => n.PCID == pc);
            }

            var list = new PagedList<tblLEDCustomViewModel>(query.OrderByDescending(n => n.SortOrder), pageNumber, pageSize);

            return list;
        }

        public IEnumerable<tblLED> GetAllPagingByFirst(string key, string pc, int pageNumber, int pageSize, ref int totalPage, ref int totalItem)
        {
            var query = from n in _tblLEDRepository.Table
                        select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.LEDName.Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(pc))
            {
                query = query.Where(n => n.PCID == pc);
            }

            var list = new PagedList<tblLED>(query.OrderByDescending(n => n.LEDName), pageNumber, pageSize);

            return list;
        }

        public tblLED GetById(int id)
        {
            return _tblLEDRepository.GetById(id);
        }

        public tblLED GetByName(string name)
        {
            var query = from n in _tblLEDRepository.Table
                        where n.LEDName.Equals(name)
                        select n;

            return query.FirstOrDefault();
        }

        public tblLED GetByName_Id(string name, string id)
        {
            var query = from n in _tblLEDRepository.Table
                        where n.LEDName.Equals(name) && n.LEDID.ToString() != id
                        select n;

            return query.FirstOrDefault();
        }

        public MessageReport Update(tblLED obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblLEDRepository.Update(obj);

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
    }
}
