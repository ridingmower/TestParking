using Kztek.Data.Repository;
using Kztek.Data.Infrastructure;
using Kztek.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Service.Admin
{
    public interface IRoleService
    {
        IEnumerable<Role> GetAllByFirst(string key);
        IEnumerable<Role> GetAllActive();
        IEnumerable<Role> GetAllByUserId(string id);

        Role GetById(string id);
        Role GetByName(string name);

        bool Create(Role obj);
        bool Update(Role obj);
        bool DeleteById(string id);
    }
    public class RoleService: IRoleService
    {
        private readonly IUserRepository _UserRepository;
        private readonly IRoleRepository _RoleRepository;
        private readonly IUserRoleRepository _UserRoleRepository;
        private readonly IUnitOfWork _UnitOfWork;

        public RoleService(IRoleRepository _RoleRepository, IUnitOfWork _UnitOfWork, IUserRoleRepository _UserRoleRepository, IUserRepository _UserRepository)
        {
            this._RoleRepository = _RoleRepository;
            this._UnitOfWork = _UnitOfWork;
            this._UserRoleRepository = _UserRoleRepository;
            this._UserRepository = _UserRepository;
        }

        public bool Create(Role obj)
        {
            bool isSuccess = false;
            try
            {
                isSuccess = true;
                _RoleRepository.Add(obj);
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
                var obj = _RoleRepository.GetById(id);
                if (obj != null)
                {
                    if (obj.Active)
                    {
                        isSuccess = false;
                    }
                    else
                    {
                        isSuccess = true;
                        _RoleRepository.Delete(obj);
                        Save();
                    }
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
                throw ex;
            }
            return isSuccess;
        }

        public Role GetById(string id)
        {
            return _RoleRepository.GetById(id);
        }

        public bool Update(Role obj)
        {
            bool isSuccess = false;
            try
            {
                isSuccess = true;
                _RoleRepository.Update(obj);
                Save();
            }
            catch (Exception ex)
            {
                isSuccess = false;
                throw ex;
            }
            return isSuccess;
        }

        //Save change
        public void Save()
        {
            _UnitOfWork.Commit();
        }

        public IEnumerable<Role> GetAllByFirst(string key)
        {
            var query = from n in _RoleRepository.Table
                        select n;
            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.RoleName.Contains(key));
            }
            return query;
        }

        public IEnumerable<Role> GetAllActive()
        {
            var query = from n in _RoleRepository.Table
                        where n.Active
                        select n;
            return query;
        }

        public Role GetByName(string name)
        {
            var query = from n in _RoleRepository.Table
                        where n.RoleName.Equals(name)
                        select n;
            return query.FirstOrDefault();
        }

        public IEnumerable<Role> GetAllByUserId(string id)
        {
            var query = from ur in _UserRoleRepository.Table
                        join r in _RoleRepository.Table on ur.RoleId equals r.Id into ur_r
                        from r in ur_r.DefaultIfEmpty()
                        join u in _UserRepository.Table on ur.UserId equals u.Id into ur_u
                        from u in ur_u.DefaultIfEmpty()
                        where ur.UserId.Equals(id)
                        select r;
            return query;
        }
    }
}
