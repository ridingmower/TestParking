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
    public interface ItblUpdateActionsService
    {
        MessageReport Create(tblUpdateActions obj);
        MessageReport Update(tblUpdateActions obj);
        IQueryable<tblUpdateActions> GetAll();
    }

    public class tblUpdateActionsService : ItblUpdateActionsService
    {
        private readonly ItblUpdateActionsRepository _tblUpdateActionsRepository;
        private readonly IUnitOfWork _UnitOfWork;
        public tblUpdateActionsService(ItblUpdateActionsRepository _tblUpdateActionsRepository, IUnitOfWork _UnitOfWork)
        {
            this._tblUpdateActionsRepository = _tblUpdateActionsRepository;
            this._UnitOfWork = _UnitOfWork;
        }

        //Save change
        public void Save()
        {
            _UnitOfWork.Commit();
        }

        public MessageReport Create(tblUpdateActions obj)
        {
            MessageReport report;
            try
            {
                _tblUpdateActionsRepository.Add(obj);
                Save();
                report = new MessageReport(true, "Thêm thành công");
            }
            catch (Exception ex)
            {
                report = new MessageReport(false, ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
            }
            return report;
        }

        public MessageReport Update(tblUpdateActions obj)
        {
            MessageReport report;
            try
            {
                _tblUpdateActionsRepository.Update(obj);
                Save();
                report = new MessageReport(true, "Cập nhật thành công");
            }
            catch (Exception ex)
            {
                report = new MessageReport(false, ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
            }
            return report;
        }
        public IQueryable<tblUpdateActions> GetAll()
        {
            var query = from n in _tblUpdateActionsRepository.Table                       
                        select n;
            return query;
        }
    }
}
