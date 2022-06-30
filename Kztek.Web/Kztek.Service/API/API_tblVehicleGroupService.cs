
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
    public interface IAPI_tblVehicleGroupService
    {
        IEnumerable<tblVehicleGroup> GetAllVehicleGroup();
        tblVehicleGroup GetById(Guid id);

        tblVehicleGroup GetByName(string name);

        MessageReport Create(tblVehicleGroup obj);

        MessageReport Update(tblVehicleGroup obj);

        MessageReport DeleteById(string id);
    }

    public class API_tblVehicleGroupService : IAPI_tblVehicleGroupService
    {
        private ItblVehicleGroupRepository _tblVehicleGroupRepository;
        private IUnitOfWork _UnitOfWork;

        public API_tblVehicleGroupService(ItblVehicleGroupRepository _tblVehicleGroupRepository, IUnitOfWork _UnitOfWork)
        {
            this._tblVehicleGroupRepository = _tblVehicleGroupRepository;
            this._UnitOfWork = _UnitOfWork;
        }

        private void Save()
        {
            _UnitOfWork.Commit();
        }

        public IEnumerable<tblVehicleGroup> GetAllVehicleGroup()
        {
            var query = from n in _tblVehicleGroupRepository.Table
                        select n;

            return query;
        }
        public MessageReport DeleteById(string id)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblVehicleGroupRepository.Delete(n => n.VehicleGroupID.ToString() == id);
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

        public tblVehicleGroup GetById(Guid id)
        {
            return _tblVehicleGroupRepository.GetById(id);
        }

        public tblVehicleGroup GetByName(string name)
        {
            var query = from n in _tblVehicleGroupRepository.Table
                        where n.VehicleGroupName.Equals(name)
                        select n;

            return query.FirstOrDefault();
        }
        public MessageReport Create(tblVehicleGroup obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblVehicleGroupRepository.Add(obj);

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

        public MessageReport Update(tblVehicleGroup obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblVehicleGroupRepository.Update(obj);

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

