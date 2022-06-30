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
    public interface IBM_ApartmentUseService
    {
        IEnumerable<BM_ApartmentUse> GetAll();

        IEnumerable<BM_ApartmentUse> GetAllByFirst(string key);

        IPagedList<BM_ApartmentUse> GetPagingByFirst(string key, int pageNumber, int pageSize);

        BM_ApartmentUse GetById(string id);

        BM_ApartmentUse GetByName(string name);

        MessageReport Create(BM_ApartmentUse obj);

        MessageReport Update(BM_ApartmentUse obj);

        MessageReport DeleteById(string id, ref BM_ApartmentUse obj);
    }

    public class BM_ApartmentUseService : IBM_ApartmentUseService
    {
        private IBM_ApartmentUseRepository _BM_ApartmentUseRepository;
        private IUnitOfWork _UnitOfWork;
        public BM_ApartmentUseService(IBM_ApartmentUseRepository _BM_ApartmentUseRepository, IUnitOfWork _UnitOfWork)
        {
            this._BM_ApartmentUseRepository = _BM_ApartmentUseRepository;
            this._UnitOfWork = _UnitOfWork;
        }

        private void Save()
        {
            _UnitOfWork.Commit();
        }

        public MessageReport Create(BM_ApartmentUse obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _BM_ApartmentUseRepository.Add(obj);

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

        public MessageReport DeleteById(string id, ref BM_ApartmentUse obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                obj = GetById(id);
                if (obj != null)
                {
                    _BM_ApartmentUseRepository.Delete(n => n.Id.ToString() == id);

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

        public IEnumerable<BM_ApartmentUse> GetAll()
        {
            var query = from n in _BM_ApartmentUseRepository.Table
                        orderby n.Ordering ascending
                        select n;

            return query;
        }


        public IEnumerable<BM_ApartmentUse> GetAllByFirst(string key)
        {
            var query = from n in _BM_ApartmentUseRepository.Table
                        select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.Name.Contains(key));
            }

            return query;
        }

        public BM_ApartmentUse GetById(string id)
        {
            return _BM_ApartmentUseRepository.GetById(id);
        }

        public BM_ApartmentUse GetByName(string name)
        {
            var query = from n in _BM_ApartmentUseRepository.Table
                        where n.Name.Equals(name)
                        select n;

            return query.FirstOrDefault();
        }

        public IPagedList<BM_ApartmentUse> GetPagingByFirst(string key, int pageNumber, int pageSize)
        {
            var query = from n in _BM_ApartmentUseRepository.Table
                        select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.Name.Contains(key));
            }

            var list = new PagedList<BM_ApartmentUse>(query.OrderByDescending(n => n.Ordering), pageNumber, pageSize);

            return list;
        }

        public MessageReport Update(BM_ApartmentUse obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _BM_ApartmentUseRepository.Update(obj);

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
