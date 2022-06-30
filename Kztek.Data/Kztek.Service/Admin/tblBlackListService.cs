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
    public interface ItblBlackListService
    {
        IEnumerable<tblBlackList> GetAll();

        IEnumerable<tblBlackList> GetAllByFirst(string key);

        IPagedList<tblBlackList> GetPagingByFirst(string key, int pageNumber, int pageSize);

        tblBlackList GetById(string id);

        IEnumerable<tblBlackList> GetByName_Plate(string name, string plate);

        MessageReport Create(tblBlackList obj);

        MessageReport Update(tblBlackList obj);

        MessageReport DeleteById(string id, ref tblBlackList obj);
    }
    class tblBlackListService : ItblBlackListService
    {
        private ItblBlackListRepository _tblBlackListRepository;
        private IUnitOfWork _UnitOfWork;
        public tblBlackListService(ItblBlackListRepository _tblBlackListRepository, IUnitOfWork _UnitOfWork)
        {
            this._tblBlackListRepository = _tblBlackListRepository;
            this._UnitOfWork = _UnitOfWork;
        }

        private void Save()
        {
            _UnitOfWork.Commit();
        }

        public IEnumerable<tblBlackList> GetAll()
        {
            var query = from n in _tblBlackListRepository.Table
                        select n;

            return query;
        }

        public IEnumerable<tblBlackList> GetAllByFirst(string key)
        {
            var query = from n in _tblBlackListRepository.Table
                        select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.Name.Contains(key) | n.Plate.Contains(key));
            }

            return query;
        }

        public IPagedList<tblBlackList> GetPagingByFirst(string key, int pageNumber, int pageSize)
        {
            var query = from n in _tblBlackListRepository.Table
                        select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.Name.Contains(key) | n.Plate.Contains(key));
            }

            var list = new PagedList<tblBlackList>(query.OrderByDescending(n => n.Name), pageNumber, pageSize);

            return list;
        }

        public tblBlackList GetById(string id)
        {
            return _tblBlackListRepository.GetById(int.Parse(id));
        }

        public IEnumerable<tblBlackList> GetByName_Plate(string name, string plate)
        {
            var query = from n in _tblBlackListRepository.Table
                        where n.Name.Equals(name) & n.Plate.Equals(plate)
                        select n;

            return query;
        }

        public MessageReport Create(tblBlackList obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblBlackListRepository.Add(obj);

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

        public MessageReport Update(tblBlackList obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblBlackListRepository.Update(obj);

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

        public MessageReport DeleteById(string id, ref tblBlackList obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                obj = GetById(id);
                if (obj != null)
                {
                    _tblBlackListRepository.Delete(n => n.Id.ToString() == id);

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
    }
}