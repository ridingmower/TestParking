using Kztek.Data.Infrastructure;
using Kztek.Data.Repository;
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
    public interface ISelfHostConfigService
    {
        IEnumerable<SelfHostConfig> GetAll();

        IEnumerable<SelfHostConfig> GetAllActiveByListPCId(List<string> ids);

        IEnumerable<SelfHostConfig> GetAllActiveByListLineId(List<string> ids);

        IPagedList<SelfHostConfig> GetAllPagingByFirst(string key, string pcs, int pageNumber, int pageSize);

        SelfHostConfig GetById(string id);

        SelfHostConfig GetByPCID(string id);

        MessageReport Create(SelfHostConfig obj);

        MessageReport Update(SelfHostConfig obj);

        MessageReport DeleteById(string id);
    }

    public class SelfHostConfigService : ISelfHostConfigService
    {
        private ISelfHostConfigRepository _SelfHostConfigRepository;
        private ItblAccessPCRepository _tblAccessPCRepository;
        private ItblAccessLineRepository _tblAccessLineRepository;
        private IUnitOfWork _UnitOfWork;

        public SelfHostConfigService(ISelfHostConfigRepository _SelfHostConfigRepository, ItblAccessPCRepository _tblAccessPCRepository, ItblAccessLineRepository _tblAccessLineRepository, IUnitOfWork _UnitOfWork)
        {
            this._SelfHostConfigRepository = _SelfHostConfigRepository;
            this._tblAccessPCRepository = _tblAccessPCRepository;
            this._tblAccessLineRepository = _tblAccessLineRepository;
            this._UnitOfWork = _UnitOfWork;
        }

        private void Save()
        {
            _UnitOfWork.Commit();
        }

        public MessageReport Create(SelfHostConfig obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _SelfHostConfigRepository.Add(obj);

                Save();

                re.Message = "Thêm mới thành công";
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
                var obj = GetById(id);
                if (obj != null)
                {
                    _SelfHostConfigRepository.Delete(obj);

                    Save();

                    re.Message = "Cập nhật thành công";
                    re.isSuccess = true;
                }
            }
            catch (Exception ex)
            {
                re.Message = ex.Message;
                re.isSuccess = false;
            }

            return re;
        }

        public IEnumerable<SelfHostConfig> GetAll()
        {
            return _SelfHostConfigRepository.Table;
        }

        public IPagedList<SelfHostConfig> GetAllPagingByFirst(string key, string pcs, int pageNumber, int pageSize)
        {
            var query = from n in _SelfHostConfigRepository.Table
                        select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.Hostname.Contains(key) && n.Address.Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(pcs))
            {
                query = query.Where(n => pcs.Contains(n.PCID));
            }

            var list = new PagedList<SelfHostConfig>(query.OrderBy(n => n.PCID), pageNumber, pageSize);

            return list;
        }

        public SelfHostConfig GetById(string id)
        {
            return _SelfHostConfigRepository.GetById(id);
        }

        public MessageReport Update(SelfHostConfig obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _SelfHostConfigRepository.Update(obj);

                Save();

                re.Message = "Cập nhật thành công";
                re.isSuccess = true;
            }
            catch (Exception ex)
            {
                re.Message = ex.Message;
                re.isSuccess = false;
            }

            return re;
        }

        public IEnumerable<SelfHostConfig> GetAllActiveByListPCId(List<string> ids)
        {
            var query = from n in _SelfHostConfigRepository.Table
                        where ids.Contains(n.PCID)
                        select n;

            return query.ToList();
        }

        public IEnumerable<SelfHostConfig> GetAllActiveByListLineId(List<string> ids)
        {
            var query = from sh in _SelfHostConfigRepository.Table
                        join pc in _tblAccessPCRepository.Table on sh.PCID equals pc.PCID.ToString()
                        join li in _tblAccessLineRepository.Table on pc.PCID.ToString() equals li.PCID

                        where ids.Contains(li.LineID.ToString())
                        select sh;

            return query;
        }

        public SelfHostConfig GetByPCID(string id)
        {
            var query = from n in _SelfHostConfigRepository.Table
                        where n.PCID == id
                        select n;

            return query.FirstOrDefault();
        }
    }
}
