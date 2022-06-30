using Kztek.Data.Infrastructure;
using Kztek.Data.Repository;
using Kztek.Data.SqlHelper;
using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Service.Admin
{
    public interface IMenuFunctionConfigService
    {
        IEnumerable<MenuFunctionConfig> GetAll();

        MessageReport Create(MenuFunctionConfig obj);

        void DeleteAll();
    }

    public class MenuFunctionConfigService : IMenuFunctionConfigService
    {
        private IMenuFunctionConfigRepository _MenuFunctionConfigRepository;
        private IUnitOfWork _UnitOfWork;

        public MenuFunctionConfigService(IMenuFunctionConfigRepository _MenuFunctionConfigRepository, IUnitOfWork _UnitOfWork)
        {
            this._MenuFunctionConfigRepository = _MenuFunctionConfigRepository;
            this._UnitOfWork = _UnitOfWork;
        }

        private void Save()
        {
            _UnitOfWork.Commit();
        }

        public MessageReport Create(MenuFunctionConfig obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                var str = string.Format("INSERT INTO MenuFunctionConfig (Id, MenuFunctionId) VALUES ('{0}', '{1}')", obj.Id, obj.MenuFunctionId);

                SqlExQuery<MenuFunctionConfig>.ExcuteNone(str);

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

        public IEnumerable<MenuFunctionConfig> GetAll()
        {
            return _MenuFunctionConfigRepository.Table;
        }

        public void DeleteAll()
        {
            var str = "Delete from MenuFunctionConfig";
            var i = SqlExQuery<MenuFunctionConfig>.ExcuteNone(str);
        }
    }
}
