using Kztek.Data.Infrastructure;
using Kztek.Data.Repository;
using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Service.Admin
{
    public interface ISystemRecordService
    {
        IEnumerable<SystemRecord> GetAll();

        MessageReport Create(SystemRecord obj);
    }

    public class SystemRecordService : ISystemRecordService
    {
        private ISystemRecordRepository _SystemRecordRepository;
        private IUnitOfWork _UnitOfWork;


        public SystemRecordService(ISystemRecordRepository _SystemRecordRepository, IUnitOfWork _UnitOfWork)
        {
            this._SystemRecordRepository = _SystemRecordRepository;
            this._UnitOfWork = _UnitOfWork;
        }

        private void Save()
        {
            _UnitOfWork.Commit();
        }

        public MessageReport Create(SystemRecord obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _SystemRecordRepository.Add(obj);

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

        public IEnumerable<SystemRecord> GetAll()
        {
            var query = from n in _SystemRecordRepository.Table
                        orderby n.DateCreated descending
                        select n;

            return query;
        }
    }
}
