using Kztek.Data.Infrastructure;
using Kztek.Data.Repository;
using Kztek.Model.CustomModel;
using Kztek.Model.Models.API;
using Kztek.Web.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Service.API
{
    public interface IAPI_AuthService
    {
        API_Auth GetDefault();

        MessageReport Create(API_Auth obj);

        MessageReport Update(API_Auth obj);
    }

    public class API_AuthService : IAPI_AuthService
    {
        private IAPI_AuthRepository _API_AuthRepository;
        private IUnitOfWork _UnitOfWork;

        public API_AuthService(IAPI_AuthRepository _API_AuthRepository, IUnitOfWork _UnitOfWork)
        {
            this._API_AuthRepository = _API_AuthRepository;
            this._UnitOfWork = _UnitOfWork;
        }

        private void Save()
        {
            _UnitOfWork.Commit();
        }

        public MessageReport Create(API_Auth obj)
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");

            try
            {
                _API_AuthRepository.Add(obj);
                Save();

                result = new MessageReport(true, "Hoàn thành");
            }
            catch (Exception ex)
            {
                result = new MessageReport(false, ex.Message);
            }

            return result;
        }

        public API_Auth GetDefault()
        {
            var query = from n in _API_AuthRepository.Table
                        select n;

            var obj = query.FirstOrDefault();
            if (obj == null)
            {
                obj = new API_Auth();
                obj.Id = Common.GenerateId();
                obj.AccessToken = "token";

                Create(obj);
            }

            return obj;
        }

        public MessageReport Update(API_Auth obj)
        {
            var result = new MessageReport(false, "Có lỗi xảy ra");

            try
            {
                _API_AuthRepository.Update(obj);
                Save();

                result = new MessageReport(true, "Hoàn thành");
            }
            catch (Exception ex)
            {
                result = new MessageReport(false, ex.Message);
            }

            return result;
        }
    }
}
