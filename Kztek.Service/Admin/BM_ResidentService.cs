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
    public interface IBM_ResidentService
    {
        IEnumerable<BM_Resident> GetAll();

        IPagedList<BM_Resident> GetAllPagingByFirst(string key, string residentGroup, int pageNumber, int pageSize);

        BM_Resident GetById(string id);

        BM_Resident GetByName(string name);


        MessageReport Create(BM_Resident obj);

        MessageReport Update(BM_Resident obj);

        MessageReport DeleteById(string id, ref BM_Resident obj);
        List<BM_ResidentCustom> GetResidentPaging(string key, string group, int pageNumber, int pageSize, ref int total);
    }

    public class BM_ResidentService : IBM_ResidentService
    {
        private IBM_ResidentRepository _BM_ResidentRepository;
        private IUnitOfWork _UnitOfWork;

        public BM_ResidentService(IBM_ResidentRepository _BM_ResidentRepository, IUnitOfWork _UnitOfWork)
        {
            this._BM_ResidentRepository = _BM_ResidentRepository;
            this._UnitOfWork = _UnitOfWork;

        }

        private void Save()
        {
            _UnitOfWork.Commit();
        }

        public MessageReport Create(BM_Resident obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _BM_ResidentRepository.Add(obj);

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

        public MessageReport DeleteById(string id, ref BM_Resident obj)
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
                    _BM_ResidentRepository.Update(obj);

                    Save();

                    re.Message = FunctionHelper.GetLocalizeDictionary("Home", "notification")["DeleteSuccess"];
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

     
        public IEnumerable<BM_Resident> GetAll()
        {
            var query = from n in _BM_ResidentRepository.Table
                        select n;

            return query;
        }


        public IPagedList<BM_Resident> GetAllPagingByFirst(string key, string residentGroup, int pageNumber, int pageSize)
        {
            var query = from n in _BM_ResidentRepository.Table
                        select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.Name.Contains(key) || n.Mobile.Contains(key) || n.Email.Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(residentGroup))
            {
                query = query.Where(n => residentGroup.Contains(n.ResidentGroupId));
            }


            var list = new PagedList<BM_Resident>(query.OrderBy(n => n.Name), pageNumber, pageSize);

            return list;

        }

        public BM_Resident GetById(string id)
        {
            return _BM_ResidentRepository.GetById(id);
        }

        public BM_Resident GetByName(string name)
        {
            var query = from n in _BM_ResidentRepository.Table
                        where n.Name == name
                        select n;

            return query.FirstOrDefault();
        }

        public MessageReport Update(BM_Resident obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _BM_ResidentRepository.Update(obj);

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

        public List<BM_ResidentCustom> GetResidentPaging(string key, string group, int pageNumber, int pageSize, ref int total)
        {

            //Lấy danh sách
            var sb = new StringBuilder();

            sb.AppendLine("SELECT * FROM(");
            sb.AppendLine("select ROW_NUMBER() OVER(ORDER BY a.Name asc) AS RowNumber, * from (");
            sb.AppendLine("select r.*,g.Name as GroupName from BM_Resident r");
            sb.AppendLine("left join BM_ResidentGroup g on g.Id = r.ResidentGroupId");
            sb.AppendLine("where r.IsDeleted = 'False'");

            if (!string.IsNullOrWhiteSpace(key))
                sb.AppendLine(string.Format(" AND (r.Name like N'%{0}%' OR r.Mobile like '%{0}%' OR r.Email like N'%{0}%')", key));

            if (!string.IsNullOrEmpty(group) && !group.Equals("0"))
                sb.AppendLine(string.Format(" AND (g.ParentId = '{0}' or g.Id = '{0}') ", group));

            sb.AppendLine(") as a");

            sb.AppendLine(") as b");

            sb.AppendLine(string.Format("WHERE RowNumber BETWEEN(({0}-1) * {1}+1) AND({0} * {1})", pageNumber, pageSize));
            var listData = SqlExQuery<BM_ResidentCustom>.ExcuteQuery(sb.ToString());
            //Tính tổng
            sb.Clear();

          
            sb.AppendLine("select count(r.Id) as TotalCount from BM_Resident r");
            sb.AppendLine("left join BM_ResidentGroup g on g.Id = r.ResidentGroupId");
            sb.AppendLine("where r.IsDeleted = 'False'");

            if (!string.IsNullOrWhiteSpace(key))
                sb.AppendLine(string.Format(" AND (r.Name like N'%{0}%' OR r.Mobile like '%{0}%' OR r.Email like N'%{0}%')", key));

            if (!string.IsNullOrEmpty(group) && !group.Equals("0"))
                sb.AppendLine(string.Format(" AND (g.ParentId = '{0}' or g.Id = '{0}') ", group));

           

            var _total = SqlExQuery<TotalPaging>.ExcuteQueryFirst(sb.ToString());
            total = _total != null ? _total.TotalCount : 0;
            return listData;
        }
    }
}
