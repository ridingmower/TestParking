using Kztek.Data.Infrastructure;
using Kztek.Data.Repository;
using Kztek.Data.SqlHelper;
using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Web.Core.Functions;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Service.Admin
{
    public interface IBM_Building_ServiceService
    {
        IEnumerable<Model.Models.BM_Building_Service> GetAll();


        PagedList<Model.Models.BM_Building_Service> GetAllPagingByFirst(string key, int pageNumber, int pageSize);


        BM_Building_Service GetById(string id);

        MessageReport Create(BM_Building_Service obj);

        MessageReport Update(BM_Building_Service obj);

        MessageReport DeleteById(string id, ref Model.Models.BM_Building_Service obj);
        BM_Building_Service GetByName(string name);
        List<BM_Building_ServiceCustom> GetResidentPaging(string key, int pageNumber, int pageSize, ref int total);
    }

    public class BM_Building_ServiceService : IBM_Building_ServiceService
    {
        private IBM_Building_ServiceRepository _BM_Building_ServiceRepository;

        private IUnitOfWork _UnitOfWork;

        public BM_Building_ServiceService(IBM_Building_ServiceRepository _BM_Building_ServiceRepository, IUnitOfWork _UnitOfWork)
        {
            this._BM_Building_ServiceRepository = _BM_Building_ServiceRepository;

            this._UnitOfWork = _UnitOfWork;
        }

        private void Save()
        {
            _UnitOfWork.Commit();
        }

        public MessageReport Create(Model.Models.BM_Building_Service obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _BM_Building_ServiceRepository.Add(obj);

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

        public MessageReport DeleteById(string id, ref Model.Models.BM_Building_Service obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                obj = GetById(id);
                if (obj != null)
                {
                    obj.IsDeleted = true;
                    _BM_Building_ServiceRepository.Update(obj);

                    Save();

                    re.Message = FunctionHelper.GetLocalizeDictionary("Home", "notification")["DeleteSuccess"]; ;
                    re.isSuccess = true;
                }
                else
                {
                    re.Message = FunctionHelper.GetLocalizeDictionary("Home", "notification")["record_does_not_exist"]; ;
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

        public IEnumerable<Model.Models.BM_Building_Service> GetAll()
        {
            var query = from n in _BM_Building_ServiceRepository.Table
                        select n;

            return query;
        }

        public BM_Building_Service GetByName(string name)
        {
            var query = from n in _BM_Building_ServiceRepository.Table
                        where !n.IsDeleted && n.Name == name
                        select n;

            return query.FirstOrDefault();
        }

        public PagedList<Model.Models.BM_Building_Service> GetAllPagingByFirst(string key, int pageNumber, int pageSize)
        {
            var query = from n in _BM_Building_ServiceRepository.Table
                        where !n.IsDeleted
                        select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.Name.Contains(key));
            }


            var list = new PagedList<Model.Models.BM_Building_Service>(query.OrderByDescending(n => n.DateCreated), pageNumber, pageSize);

            return list;
        }

        public BM_Building_Service GetById(string id)
        {
            return _BM_Building_ServiceRepository.GetById(id);
        }



        public MessageReport Update(Model.Models.BM_Building_Service obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _BM_Building_ServiceRepository.Update(obj);

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

        public List<BM_Building_ServiceCustom> GetResidentPaging(string key, int pageNumber, int pageSize, ref int total)
        {

            //Lấy danh sách
            var sb = new StringBuilder();

            sb.AppendLine("SELECT * FROM(");
            sb.AppendLine("select ROW_NUMBER() OVER(ORDER BY a.Name asc) AS RowNumber, * from (");
            sb.AppendLine("select r.* from BM_Building_Service r");          
            sb.AppendLine("where r.IsDeleted = 'False'");

            if (!string.IsNullOrWhiteSpace(key))
                sb.AppendLine(string.Format(" AND (r.Name like N'%{0}%')", key));

            sb.AppendLine(") as a");

            sb.AppendLine(") as b");

            sb.AppendLine(string.Format("WHERE RowNumber BETWEEN(({0}-1) * {1}+1) AND({0} * {1})", pageNumber, pageSize));
            var listData = SqlExQuery<BM_Building_ServiceCustom>.ExcuteQuery(sb.ToString());
            //Tính tổng
            sb.Clear();


            sb.AppendLine("select count(r.Id) as TotalCount from BM_Building_Service r");
          
            sb.AppendLine("where r.IsDeleted = 'False'");

            if (!string.IsNullOrWhiteSpace(key))
                sb.AppendLine(string.Format(" AND (r.Name like N'%{0}%')", key));        

            var _total = SqlExQuery<TotalPaging>.ExcuteQueryFirst(sb.ToString());
            total = _total != null ? _total.TotalCount : 0;
            return listData;
        }
    }
}
