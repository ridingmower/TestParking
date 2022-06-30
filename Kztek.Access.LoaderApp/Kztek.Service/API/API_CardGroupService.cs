using Kztek.Data.Infrastructure;
using Kztek.Data.Repository;
using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Web.Core.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Service.API
{
    public interface IAPI_CardGroupService
    {
        IEnumerable<tblCardGroup> GetAll();
        IEnumerable<tblCardGroup> GetList(string key);
        tblCardGroup GetById(Guid id);

        tblCardGroup GetByName(string name);

        MessageReport Create(tblCardGroup obj);

        MessageReport Update(tblCardGroup obj);

        MessageReport DeleteById(string id);
    }

    public class API_CardGroupService : IAPI_CardGroupService
    {
        private ItblCardGroupRepository _tblCardGroupRepository;
        private IUnitOfWork _UnitOfWork;

        public API_CardGroupService(ItblCardGroupRepository _tblCardGroupRepository, IUnitOfWork _UnitOfWork)
        {
            this._tblCardGroupRepository = _tblCardGroupRepository;
            this._UnitOfWork = _UnitOfWork;
        }
        private void Save()
        {
            _UnitOfWork.Commit();
        }
        public MessageReport Create(tblCardGroup obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblCardGroupRepository.Add(obj);

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

        public MessageReport DeleteById(string id)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblCardGroupRepository.Delete(n => n.CardGroupID.ToString() == id);
                Save();
                re.Message = FunctionHelper.GetLocalizeDictionary("Home", "notification")["DeleteSuccess"];
                re.isSuccess = true;
            }
            catch (Exception ex)
            {
                re.Message = ex.Message;
                re.isSuccess = false;
            }

            return re;
        }

        /// <summary>
        /// get all card group
        /// </summary>
        /// <returns></returns>
        public IEnumerable<tblCardGroup> GetAll()
        {
            var query = from n in _tblCardGroupRepository.Table
                        select n;

            //if (!string.IsNullOrEmpty(AuthCardGroupIds))
            //{
            //    var list = AuthCardGroupIds.Split(';');
            //    query = query.Where(n => list.Contains(n.CardGroupID.ToString()));
            //}

            return query;
        }

        public tblCardGroup GetById(Guid id)
        {
            var obj = _tblCardGroupRepository.GetById(id);
            return obj;
        }

        public tblCardGroup GetByName(string name)
        {
            var query = from n in _tblCardGroupRepository.Table
                        where n.CardGroupName.Equals(name)
                        select n;

            return query.FirstOrDefault();
        }

        public MessageReport Update(tblCardGroup obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblCardGroupRepository.Update(obj);

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

        public IEnumerable<tblCardGroup> key()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<tblCardGroup> GetList(string key)
        {
            var query = from n in _tblCardGroupRepository.Table
                        select n;

            if (!string.IsNullOrEmpty(key))
                query = query.Where(n => n.CardGroupName.Contains(key));
            return query;
        }
    }
}
