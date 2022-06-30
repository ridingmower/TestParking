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
    public interface ItblLockerSelfHostService
    {
        IEnumerable<tblLockerSelfHost> GetAll();

        IEnumerable<tblLockerSelfHost> GetAllActiveByListPCId(List<string> ids);

        IEnumerable<tblLockerSelfHost> GetAllActiveByListLineId(List<string> ids);

        IPagedList<tblLockerSelfHost> GetAllPagingByFirst(string key, string pcs, int pageNumber, int pageSize);

        tblLockerSelfHost GetById(string id);

        tblLockerSelfHost GetByPCID(string id);

        MessageReport Create(tblLockerSelfHost obj);

        MessageReport Update(tblLockerSelfHost obj);

        MessageReport DeleteById(string id);
    }

    public class tblLockerSelfHostService : ItblLockerSelfHostService
    {
        private ItblLockerSelfHostRepository _tblLockerSelfHostRepository;
        private ItblLockerPCRepository _tblLockerPCRepository;
        private ItblLockerLineRepository _tblLockerLineRepository;
        private IUnitOfWork _UnitOfWork;

        public tblLockerSelfHostService(ItblLockerSelfHostRepository _tblLockerSelfHostRepository, ItblLockerPCRepository _tblLockerPCRepository, ItblLockerLineRepository _tblLockerLineRepository, IUnitOfWork _UnitOfWork)
        {
            this._tblLockerSelfHostRepository = _tblLockerSelfHostRepository;
            this._tblLockerPCRepository = _tblLockerPCRepository;
            this._tblLockerLineRepository = _tblLockerLineRepository;
            this._UnitOfWork = _UnitOfWork;
        }

        private void Save()
        {
            _UnitOfWork.Commit();
        }

        public MessageReport Create(tblLockerSelfHost obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblLockerSelfHostRepository.Add(obj);

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
                    _tblLockerSelfHostRepository.Delete(obj);

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

        public IEnumerable<tblLockerSelfHost> GetAll()
        {
            return _tblLockerSelfHostRepository.Table;
        }

        public IPagedList<tblLockerSelfHost> GetAllPagingByFirst(string key, string pcs, int pageNumber, int pageSize)
        {
            var query = from n in _tblLockerSelfHostRepository.Table
                        select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.Hostname.Contains(key) && n.Address.Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(pcs))
            {
                query = query.Where(n => pcs.Contains(n.PCID));
            }

            var list = new PagedList<tblLockerSelfHost>(query.OrderBy(n => n.PCID), pageNumber, pageSize);

            return list;
        }

        public tblLockerSelfHost GetById(string id)
        {
            return _tblLockerSelfHostRepository.GetById(id);
        }

        public MessageReport Update(tblLockerSelfHost obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblLockerSelfHostRepository.Update(obj);

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

        public IEnumerable<tblLockerSelfHost> GetAllActiveByListPCId(List<string> ids)
        {
            var query = from n in _tblLockerSelfHostRepository.Table
                        where ids.Contains(n.PCID)
                        select n;

            return query.ToList();
        }

        public IEnumerable<tblLockerSelfHost> GetAllActiveByListLineId(List<string> ids)
        {
            var query = from sh in _tblLockerSelfHostRepository.Table
                        join pc in _tblLockerPCRepository.Table on sh.PCID equals pc.Id.ToString()
                        join li in _tblLockerLineRepository.Table on pc.Id.ToString() equals li.PCID

                        where ids.Contains(li.Id.ToString())
                        select sh;

            return query;
        }

        public tblLockerSelfHost GetByPCID(string id)
        {
            var query = from n in _tblLockerSelfHostRepository.Table
                        where n.PCID == id
                        select n;

            return query.FirstOrDefault();
        }
    }
}
