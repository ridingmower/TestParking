using Kztek.Data.Repository;
using Kztek.Model.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Service.Admin
{
    public interface ItblLogService
    {
        IPagedList<tblLog> GetAllPagingByFirst(string key, string users, string actions, string fromdate, string todate, int pageNumber, int pageSize, string appcode = "");
    }

    public class tblLogService : ItblLogService
    {
        private ItblLogRepository _tblLogRepository;

        public tblLogService(ItblLogRepository _tblLogRepository)
        {
            this._tblLogRepository = _tblLogRepository;
        }

        public IPagedList<tblLog> GetAllPagingByFirst(string key, string users, string actions, string fromdate, string todate, int pageNumber, int pageSize, string appcode = "")
        {
            var query = from n in _tblLogRepository.Table
                        select n;

            if (!string.IsNullOrWhiteSpace(appcode))
            {
                query = query.Where(n => n.AppCode.Contains(appcode));
            }

            if (!string.IsNullOrWhiteSpace(key))
            {
                key = key.ToLower();

                query = query.Where(n => n.ComputerName.ToLower().Contains(key) || n.NewInfo.ToLower().Contains(key) || n.OldInfo.ToLower().Contains(key) || n.Actions.ToLower().Contains(key) || n.ComputerName.ToLower().Contains(key) || n.Description.Contains(key) || n.ObjectName.ToLower().Contains(key) || n.SubSystemCode.ToLower().Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(users))
            {
                query = query.Where(n => users.Contains(n.UserName));
            }

            if (!string.IsNullOrWhiteSpace(actions))
            {
                query = query.Where(n => actions.Contains(n.Actions));
            }

            if (!string.IsNullOrWhiteSpace(fromdate) || !string.IsNullOrWhiteSpace(todate))
            {
                var fdate = Convert.ToDateTime(fromdate);
                var tdate = Convert.ToDateTime(todate).AddDays(1);

                query = query.Where(n => n.Date.Value >= fdate && n.Date < tdate);
            }
            else
            {
                var fdate = DateTime.Now;
                var tdate = fdate.AddDays(1);

                query = query.Where(n => n.Date.Value >= fdate && n.Date < tdate);
            }

            var list = new PagedList<tblLog>(query.OrderByDescending(n => n.Date), pageNumber, pageSize);

            return list;
        }
    }
}
