
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
    public interface ItblFtpAccountService
    {
        IEnumerable<tblFtpAccount> GetAll();

        IEnumerable<tblFtpAccount> GetAllActive();

        IEnumerable<tblFtpAccount> GetAllByFirst(string key);

        IPagedList<tblFtpAccount> GetPagingByFirst(string key, int pageNumber, int pageSize);

        tblFtpAccount GetById(string id);

        MessageReport Create(tblFtpAccount obj);

        MessageReport Update(tblFtpAccount obj);

        MessageReport DeleteById(string id, ref tblFtpAccount obj);
    }

    public class tblFtpAccountService : ItblFtpAccountService
    {
        private ItblFtpAccountRepository _tblFtpAccountRepository;
        private IUnitOfWork _UnitOfWork;
        public tblFtpAccountService(ItblFtpAccountRepository _tblFtpAccountRepository, IUnitOfWork _UnitOfWork)
        {
            this._tblFtpAccountRepository = _tblFtpAccountRepository;
            this._UnitOfWork = _UnitOfWork;
        }

        private void Save()
        {
            _UnitOfWork.Commit();
        }

        public MessageReport Create(tblFtpAccount obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblFtpAccountRepository.Add(obj);

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

        public MessageReport DeleteById(string id, ref tblFtpAccount obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                obj = GetById(id);
                if (obj != null)
                {
                    _tblFtpAccountRepository.Delete(n => n.Id == id);

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

        public IEnumerable<tblFtpAccount> GetAll()
        {
            var query = from n in _tblFtpAccountRepository.Table
                        select n;

            return query;
        }

        public IEnumerable<tblFtpAccount> GetAllActive()
        {
            var query = from n in _tblFtpAccountRepository.Table
                        select n;

            return query;
        }

        public IEnumerable<tblFtpAccount> GetAllByFirst(string key)
        {
            var query = from n in _tblFtpAccountRepository.Table
                        select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.FtpHost.Contains(key));
            }

            return query;
        }

        public tblFtpAccount GetById(string id)
        {
            return _tblFtpAccountRepository.GetById(id);
        }

        public IPagedList<tblFtpAccount> GetPagingByFirst(string key, int pageNumber, int pageSize)
        {
            var query = from n in _tblFtpAccountRepository.Table
                        select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.FtpHost.Contains(key));
            }

            var list = new PagedList<tblFtpAccount>(query.OrderByDescending(n => n.FtpHost), pageNumber, pageSize);

            return list;
        }

        public MessageReport Update(tblFtpAccount obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblFtpAccountRepository.Update(obj);

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
