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
    public interface ItblVehicleGroupService
    {
        IEnumerable<tblVehicleGroup> GetAll();

        IEnumerable<tblVehicleGroup> GetAllActive();

        tblVehicleGroup GetById(Guid id);

        MessageReport Create(tblVehicleGroup obj);

        MessageReport Update(tblVehicleGroup obj);

        MessageReport DeleteById(string id, ref tblVehicleGroup obj);
    }

    public class tblVehicleGroupService : ItblVehicleGroupService
    {
        private ItblVehicleGroupRepository _tblVehicleGroupRepository;
        private IUnitOfWork _UnitOfWork;

        public tblVehicleGroupService(ItblVehicleGroupRepository _tblVehicleGroupRepository, IUnitOfWork _UnitOfWork)
        {
            this._tblVehicleGroupRepository = _tblVehicleGroupRepository;
            this._UnitOfWork = _UnitOfWork;
        }

        private void Save()
        {
            _UnitOfWork.Commit();
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

        public MessageReport DeleteById(string id, ref tblVehicleGroup obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                obj = GetById(Guid.Parse(id));
                if (obj != null)
                {
                    _tblVehicleGroupRepository.Delete(n => n.VehicleGroupID.ToString() == id);

                    Save();

                    re.Message = "Xóa thành công";
                    re.isSuccess = true;
                }
                else
                {
                    re.Message = "Bản ghi không tồn tại";
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

        public IEnumerable<tblVehicleGroup> GetAll()
        {
            var query = from n in _tblVehicleGroupRepository.Table
                        select n;

            return query;
        }

        public IEnumerable<tblVehicleGroup> GetAllActive()
        {
            var query = from n in _tblVehicleGroupRepository.Table
                        where n.Inactive == false
                        select n;

            return query;
        }

        public tblVehicleGroup GetById(Guid id)
        {
            return _tblVehicleGroupRepository.GetById(id);
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
    }
}
