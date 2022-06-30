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
    public interface IUserRoleService
    {
        IEnumerable<UserRole> GetAllByUserId(string id);
        IEnumerable<UserRole> GetAllByRoleId(string id);

        UserRole GetById(string id);
        UserRole GetByUserId_RoleId(string userid, string roleid);

        bool Create(UserRole obj);
        bool DeleteById(string id);
    }
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleRepository _UserRoleRepository;
        private readonly IUnitOfWork _UnitOfWork;

        public UserRoleService(IUserRoleRepository _UserRoleRepository, IUnitOfWork _UnitOfWork)
        {
            this._UserRoleRepository = _UserRoleRepository;
            this._UnitOfWork = _UnitOfWork;
        }

        //Save change
        public void Save()
        {
            _UnitOfWork.Commit();
        }

        public IEnumerable<UserRole> GetAllByUserId(string id)
        {
            var query = from n in _UserRoleRepository.Table
                        where n.UserId.Equals(id)
                        select n;
            return query;
        }

        public bool Create(UserRole obj)
        {
            bool isSuccess = false;
            try
            {
                isSuccess = true;
                _UserRoleRepository.Add(obj);
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
                var obj = _UserRoleRepository.GetById(id);
                if (obj!=null)
                {
                    isSuccess = true;
                    _UserRoleRepository.Delete(obj);
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

        public UserRole GetById(string id)
        {
            return _UserRoleRepository.GetById(id);
        }

        public UserRole GetByUserId_RoleId(string userid, string roleid)
        {
            var query = from n in _UserRoleRepository.Table
                        where n.UserId.Equals(userid) && n.RoleId.Equals(roleid)
                        select n;
            return query.FirstOrDefault();
        }

        public IEnumerable<UserRole> GetAllByRoleId(string id)
        {
            var query = from n in _UserRoleRepository.Table
                        where n.RoleId.Equals(id)
                        select n;
            return query;
        }
    }
}
