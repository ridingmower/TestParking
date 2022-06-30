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
    public interface IBM_ApartmentRoleService
    {
        IEnumerable<BM_ApartmentRole> GetAll();


        IPagedList<BM_ApartmentRole> GetAllPagingByFirst(string key, int pageNumber, int pageSize);


        BM_ApartmentRole GetById(string id);
        BM_ApartmentRole GetByName(string name);

        MessageReport Create(BM_ApartmentRole obj);

        MessageReport Update(BM_ApartmentRole obj);

        MessageReport DeleteById(string id, ref BM_ApartmentRole obj);
    }

    public class BM_ApartmentRoleService : IBM_ApartmentRoleService
    {
        private IBM_ApartmentRoleRepository _BM_ApartmentRoleRepository;

        private IUnitOfWork _UnitOfWork;

        public BM_ApartmentRoleService(IBM_ApartmentRoleRepository _BM_ApartmentRoleRepository, IUnitOfWork _UnitOfWork)
        {
            this._BM_ApartmentRoleRepository = _BM_ApartmentRoleRepository;

            this._UnitOfWork = _UnitOfWork;
        }

        private void Save()
        {
            _UnitOfWork.Commit();
        }

        public MessageReport Create(BM_ApartmentRole obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _BM_ApartmentRoleRepository.Add(obj);

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

        public MessageReport DeleteById(string id, ref BM_ApartmentRole obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                obj = GetById(id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    _BM_ApartmentRoleRepository.Update(obj);

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

        public IEnumerable<BM_ApartmentRole> GetAll()
        {
            var query = from n in _BM_ApartmentRoleRepository.Table
                        select n;

            return query;
        }
        public BM_ApartmentRole GetByName(string name)
        {
            var query = from n in _BM_ApartmentRoleRepository.Table
                        where n.Name.Contains(name)
                        select n;

            return query.FirstOrDefault();
        }



        public IPagedList<BM_ApartmentRole> GetAllPagingByFirst(string key, int pageNumber, int pageSize)
        {
            var query = from n in _BM_ApartmentRoleRepository.Table
                        where !n.IsDeleted
                        select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.Name.Contains(key));
            }


            var list = new PagedList<BM_ApartmentRole>(query.OrderByDescending(n => n.Name), pageNumber, pageSize);

            return list;
        }

        public BM_ApartmentRole GetById(string id)
        {
            return _BM_ApartmentRoleRepository.GetById(id);
        }



        public MessageReport Update(BM_ApartmentRole obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _BM_ApartmentRoleRepository.Update(obj);

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
