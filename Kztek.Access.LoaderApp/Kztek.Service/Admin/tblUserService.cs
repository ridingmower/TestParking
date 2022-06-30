using Kztek.Data.Repository;
using Kztek.Model.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Service.Admin
{
    public interface ItblUserService
    {
        List<tblUser> GetAll();
        DataTable GetAllUser(string userId);
    }

    public class tblUserService : ItblUserService
    {
        private ItblUserRepository _tblUserRepository;

        public tblUserService(ItblUserRepository _tblUserRepository)
        {
            this._tblUserRepository = _tblUserRepository;
        }

        public List<tblUser> GetAll()
        {
            var query = from n in _tblUserRepository.Table
                        select n;

            return query.ToList();
        }

        public DataTable GetAllUser(string UserID)
        {
            var query = new StringBuilder();

            query.AppendLine("Select UserName, UserID from tblUser where IsLock=0 ");

            //User
            if (!string.IsNullOrWhiteSpace(UserID))
            {
                var t = UserID.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and UserID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            query.AppendLine("order by SortOrder");

            return Data.SqlHelper.ExcuteSQL.GetDataSet(query.ToString(), false).Tables[0];
        }
    }
}
