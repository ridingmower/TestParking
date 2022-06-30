using Kztek.Data.Infrastructure;
using Kztek.Data.Repository;
using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Web.Core.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Service.Admin
{
    public interface IUser_AuthGroupService
    {
        IEnumerable<User_AuthGroup> GetAll();

        User_AuthGroup GetById(string id);
        User_AuthGroup GetByUserId(string userid);
        MessageReport Create(User_AuthGroup obj);

        MessageReport Update(User_AuthGroup obj);

        MessageReport DeleteById(string id);
        string GetAuthCardGroupIds();
    }

    public class User_AuthGroupService : IUser_AuthGroupService
    {
        private IUser_AuthGroupRepository _User_AuthGroupRepository;
        private ItblSystemConfigService _tblSystemConfigService;
        private IUnitOfWork _UnitOfWork;

        public User_AuthGroupService(IUser_AuthGroupRepository _User_AuthGroupRepository, ItblSystemConfigService _tblSystemConfigService, IUnitOfWork _UnitOfWork)
        {
            this._User_AuthGroupRepository = _User_AuthGroupRepository;
            this._tblSystemConfigService = _tblSystemConfigService;
            this._UnitOfWork = _UnitOfWork;
        }

        private void Save()
        {
            _UnitOfWork.Commit();
        }

        public MessageReport Create(User_AuthGroup obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _User_AuthGroupRepository.Add(obj);

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

        public MessageReport DeleteById(string id)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                var obj = GetById(id);
                if (obj != null)
                {
                    _User_AuthGroupRepository.Delete(n => n.Id.ToString() == id);

                    Save();

                    re.Message = FunctionHelper.GetLocalizeDictionary("Home", "notification")["DeleteSuccess"];
                    re.isSuccess = true;
                }
                else
                {
                    re.Message = FunctionHelper.GetLocalizeDictionary("Home", "notification")["record_does_not_exist"];
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

        public IEnumerable<User_AuthGroup> GetAll()
        {
            var query = from n in _User_AuthGroupRepository.Table
                        select n;

            return query;
        }
        public User_AuthGroup GetByUserId(string userid)
        {
            var query = from n in _User_AuthGroupRepository.Table
                        where n.UserId == userid
                        select n;

            return query.FirstOrDefault();
        }

        public User_AuthGroup GetById(string id)
        {
            return _User_AuthGroupRepository.GetById(id);
        }


        public MessageReport Update(User_AuthGroup obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _User_AuthGroupRepository.Update(obj);

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
        /// <summary>
        /// lấy danh sách cardgroup được phân quyền theo user
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public string GetAuthCardGroupIds()
        {
            var user = GetCurrentUser.GetUser();
            var userid = user != null ? user.Id : "";

            string cardgroupids = "";

            if(user != null)
            {
                var objsystem = _tblSystemConfigService.GetDefault();

                if (!string.IsNullOrEmpty(userid) && objsystem != null && objsystem.isAuthInView)
                {
                    var user_auth = GetByUserId(userid);

                    if (user_auth != null && !string.IsNullOrEmpty(user_auth.CardGroupIds))
                    {
                        cardgroupids = user_auth.CardGroupIds;
                    }
                    else
                    {
                        cardgroupids = "NULL";
                    }
                }
            }
           
            return cardgroupids;
        }
    }
}
