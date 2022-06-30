using Kztek.Data.Infrastructure;
using Kztek.Data.Repository;
using Kztek.Model.CustomModel;
using Kztek.Model.CustomModel.iParking;
using Kztek.Model.Models;
using Kztek.Model.Models.API;
using Kztek.Web.Core.Functions;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Service.API
{
    public interface ItblCardSubmitEventService
    {
        IEnumerable<tblCardSubmitEvent> GetAll();

        MessageReport Create(tblCardSubmitEvent obj);

    }

    public class tblCardSubmitEventService : ItblCardSubmitEventService
    {
        private ItblCardSubmitEventRepository _tblCardSubmitEventRepository;
        private IUnitOfWork _UnitOfWork;

        public tblCardSubmitEventService(ItblCardSubmitEventRepository _tblCardSubmitEventRepository, IUnitOfWork _UnitOfWork)
        {
            this._tblCardSubmitEventRepository = _tblCardSubmitEventRepository;
            this._UnitOfWork = _UnitOfWork;
        }

        private void Save()
        {
            _UnitOfWork.Commit();
        }

        public MessageReport Create(tblCardSubmitEvent obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblCardSubmitEventRepository.Add(obj);

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


        public IEnumerable<tblCardSubmitEvent> GetAll()
        {
            var query = from n in _tblCardSubmitEventRepository.Table
                        select n;

            return query;
        }

    }
}
