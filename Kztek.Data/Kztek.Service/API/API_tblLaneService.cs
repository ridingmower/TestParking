using Kztek.Data.Infrastructure;
using Kztek.Data.Repository;
using Kztek.Data.SqlHelper;
using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kztek.Web.Core.Functions;
using Kztek.Data.Event.SqlHelper;
using Kztek.Model.Models.Event;
using Kztek.Data.Event.Repository;

namespace Kztek.Service.API
{
    public interface IAPI_tblLaneService
    {
        IEnumerable<tblLane> GetAllLane();
        tblLane GetById(Guid id);

        tblLane GetByName(string name);

        MessageReport Create(tblLane obj);

        MessageReport Update(tblLane obj);

        MessageReport DeleteById(string id);
    }

    public class API_tblLaneService : IAPI_tblLaneService
    {
        private ItblLaneRepository _tblLaneRepository;
        private IUnitOfWork _UnitOfWork;

        public API_tblLaneService(ItblLaneRepository _tblLaneRepository, IUnitOfWork _UnitOfWork)
        {
            this._tblLaneRepository = _tblLaneRepository;
            this._UnitOfWork = _UnitOfWork;
        }

        private void Save()
        {
            _UnitOfWork.Commit();
        }

        public MessageReport DeleteById(string id)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblLaneRepository.Delete(n => n.LaneID.ToString() == id);
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

        public tblLane GetById(Guid id)
        {
            return _tblLaneRepository.GetById(id);
        }

        public tblLane GetByName(string name)
        {
            var query = from n in _tblLaneRepository.Table
                        where n.LaneName.Equals(name)
                        select n;

            return query.FirstOrDefault();
        }
        public MessageReport Create(tblLane obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblLaneRepository.Add(obj);

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

        public MessageReport Update(tblLane obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblLaneRepository.Update(obj);

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

       public IEnumerable<tblLane> GetAllLane()
        {
            var query = from n in _tblLaneRepository.Table
                        select n;
            return query.ToList();
        }
    }
}

