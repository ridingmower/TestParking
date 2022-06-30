using Kztek.Data.Infrastructure;
using Kztek.Data.Repository;
using Kztek.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Service.Admin
{
    public interface IRoleMenuService
    {
        IEnumerable<RoleMenu> GetAllByMenuId(string id);
        IEnumerable<RoleMenu> GetAllByRoleId(string id);
        IEnumerable<RoleMenu> GetAllByRoleId(string id, List<string> menuids);

        bool Create(RoleMenu obj);

        bool DeleteById(string id);
    }
    public class RoleMenuService : IRoleMenuService
    {
        private readonly IRoleMenuRepository _RoleMenuRepository;
        private readonly IUnitOfWork _UnitOfWork;

        public RoleMenuService(IRoleMenuRepository _RoleMenuRepository, IUnitOfWork _UnitOfWork)
        {
            this._RoleMenuRepository = _RoleMenuRepository;
            this._UnitOfWork = _UnitOfWork;
        }

        public bool Create(RoleMenu obj)
        {
            bool isSuccess = false;
            try
            {
                isSuccess = true;
                _RoleMenuRepository.Add(obj);
                Save();
            }
            catch (Exception ex)
            {
                isSuccess = false;
                throw ex;
            }
            return isSuccess;
        }

        public bool DeleteById(string id)
        {
            bool isSuccess = false;
            try
            {
                var obj = _RoleMenuRepository.GetById(id);
                if (obj != null)
                {
                    isSuccess = true;
                    _RoleMenuRepository.Delete(obj);
                    Save();
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
                throw ex;
            }
            return isSuccess;
        }

        public IEnumerable<RoleMenu> GetAllByMenuId(string id)
        {
            var query = from n in _RoleMenuRepository.Table
                        where n.MenuId.Equals(id)
                        select n;
            return query;
        }

        public IEnumerable<RoleMenu> GetAllByRoleId(string id)
        {
            var query = from n in _RoleMenuRepository.Table
                        where n.RoleId.Equals(id)
                        select n;
            return query;
        }

        public IEnumerable<RoleMenu> GetAllByRoleId(string id,List<string> menuids)
        {
            var query = from n in _RoleMenuRepository.Table
                        where n.RoleId.Equals(id) && menuids.Contains(n.MenuId)
                        select n;
            return query;
        }

        //Save change
        public void Save()
        {
            _UnitOfWork.Commit();
        }
    }
}
