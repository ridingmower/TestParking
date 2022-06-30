using Kztek.Data.AccessEvent.SqlHelper;
using Kztek.Data.Infrastructure;
using Kztek.Data.Repository;
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
    public interface IBM_Apartment_ReceiptService
    {
        IEnumerable<BM_Apartment_Receipt> GetAll();

        IEnumerable<BM_Apartment_Receipt> GetAllByFirst(string key);

        IPagedList<BM_Apartment_Receipt> GetPagingByFirst(string key, int pageNumber, int pageSize);
        List<BM_Apartment_ReceiptView> GetAllPagingByFirstTSQL(string key, string buildingIdSearch,string floorIdSearch, string apartmentIdSearch, string residentIdSearch, string typeSearch, string statusSearch, string userSearch, string userCreatedIdSearch, string fromDatePaid, string toDatePaid, int pageNumber, int pageSize, ref int total);

        BM_Apartment_Receipt GetById(string id);

        BM_Apartment_Receipt GetByName(string name);

        MessageReport Create(BM_Apartment_Receipt obj);

        MessageReport Update(BM_Apartment_Receipt obj);

        MessageReport DeleteById(string id, ref BM_Apartment_Receipt obj);
    }

    public class BM_Apartment_ReceiptService : IBM_Apartment_ReceiptService
    {
        private IBM_Apartment_ReceiptRepository _BM_Apartment_ReceiptRepository;
        private IUnitOfWork _UnitOfWork;
        public BM_Apartment_ReceiptService(IBM_Apartment_ReceiptRepository _BM_Apartment_ReceiptRepository, IUnitOfWork _UnitOfWork)
        {
            this._BM_Apartment_ReceiptRepository = _BM_Apartment_ReceiptRepository;
            this._UnitOfWork = _UnitOfWork;
        }

        private void Save()
        {
            _UnitOfWork.Commit();
        }

        public MessageReport Create(BM_Apartment_Receipt obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _BM_Apartment_ReceiptRepository.Add(obj);

                Save();

                re.Message = FunctionHelper.GetLocalizeDictionary("Home", "notification")["addSuccess"]; ;
                re.isSuccess = true;
            }
            catch (Exception ex)
            {
                re.Message = ex.Message;
                re.isSuccess = false;
            }

            return re;
        }

        public MessageReport DeleteById(string id, ref BM_Apartment_Receipt obj)
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
                    _BM_Apartment_ReceiptRepository.Update(obj);

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

        public IEnumerable<BM_Apartment_Receipt> GetAll()
        {
            var query = from n in _BM_Apartment_ReceiptRepository.Table
                        select n;

            return query;
        }


        public IEnumerable<BM_Apartment_Receipt> GetAllByFirst(string key)
        {
            var query = from n in _BM_Apartment_ReceiptRepository.Table
                        select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.Title.Contains(key));
            }

            return query;
        }

        public BM_Apartment_Receipt GetById(string id)
        {
            return _BM_Apartment_ReceiptRepository.GetById(id);
        }

        public BM_Apartment_Receipt GetByName(string name)
        {
            var query = from n in _BM_Apartment_ReceiptRepository.Table
                        where n.Title.Equals(name)
                        select n;

            return query.FirstOrDefault();
        }

        public IPagedList<BM_Apartment_Receipt> GetPagingByFirst(string key, int pageNumber, int pageSize)
        {
            var query = from n in _BM_Apartment_ReceiptRepository.Table
                        select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.Title.Contains(key));
            }

            var list = new PagedList<BM_Apartment_Receipt>(query.OrderByDescending(n => n.Id), pageNumber, pageSize);

            return list;
        }

        public MessageReport Update(BM_Apartment_Receipt obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _BM_Apartment_ReceiptRepository.Update(obj);

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


        public List<BM_Apartment_ReceiptView> GetAllPagingByFirstTSQL(string key, string buildingIdSearch, string floorIdSearch, string apartmentIdSearch, string residentIdSearch, string typeSearch, string statusSearch, string userSearch, string userCreatedIdSearch, string fromDatePaid, string toDatePaid, int pageNumber, int pageSize, ref int total)
        {

            //if (!string.IsNullOrEmpty(fromDatePaid))
            //{
            //    fromDatePaid = Convert.ToDateTime(fromDatePaid).ToString("yyyy/MM/dd HH:mm:ss");
            //}
            //if (!string.IsNullOrEmpty(toDatePaid))
            //{
            //    toDatePaid = Convert.ToDateTime(toDatePaid).ToString("yyyy/MM/dd HH:mm:ss");
            //}

            //if (!string.IsNullOrEmpty(key))
            //    query.AppendLine(string.Format("AND (LP.CardNo LIKE '%{0}%' OR LP.CardNumber LIKE '%{0}%' OR U.Name LIKE '%{0}%' OR LC.ControllerName LIKE '%{0}%')", KeyWord));

            ////search toDate - fromdate
            //if (!string.IsNullOrEmpty(fromDatePaid) && !string.IsNullOrEmpty(toDatePaid))
            //{
            //    query.AppendLine(string.Format("AND LP.DateCreated >= '{0}' AND LP.DateCreated <= '{1}'", fromDatePaid, toDatePaid));
            //}
            //else if (!string.IsNullOrEmpty(fromDatePaid) && string.IsNullOrEmpty(toDatePaid))
            //{
            //    query.AppendLine(string.Format("AND LP.DateCreated >= '{0}'", fromDatePaid));
            //}
            //else if (string.IsNullOrEmpty(fromDatePaid) && !string.IsNullOrEmpty(toDatePaid))
            //{
            //    query.AppendLine(string.Format("AND LP.DateCreated <= '{0}'", toDatePaid));
            //}

            var query = new StringBuilder();

            //danh sách
            query.AppendLine("SELECT * FROM(");
            query.AppendLine("SELECT ROW_NUMBER() OVER(ORDER BY a.DateCreated desc) as RowNumber,a.*");
            query.AppendLine("FROM(");
            query.AppendLine("SELECT AR.* from BM_Apartment_Receipt AR");
            //query.AppendLine("inner join [User] U on U.Id = LP.UserId ");
            //query.AppendLine("left join [tblLockerController] LC on LC.Id = LP.ControllerID ");
            query.AppendLine("where 1=1");

            

            query.AppendLine(") AS a");
            query.AppendLine(") AS b");
            query.AppendLine(string.Format("WHERE RowNumber BETWEEN (({0}-1) * {1} + 1) AND ({0} * {1})", pageNumber, pageSize));

            var listData = SqlExQuery<BM_Apartment_ReceiptView>.ExcuteQuery(query.ToString());

            //Tính tổng
            query.Clear();
            query.AppendLine("SELECT COUNT(*) TotalCount");

            query.AppendLine("FROM BM_Apartment_Receipt AR WITH(NOLOCK)");

            //query.AppendLine("LEFT JOIN tblCardGroup cg ON c.CardGroupID = CONVERT(varchar(50), cg.CardGroupID)");
            //query.AppendLine("LEFT JOIN tblCustomer cu ON c.CustomerID = CONVERT(varchar(50), cu.CustomerID)");
            //query.AppendLine("LEFT JOIN tblCustomerGroup cug ON cu.CustomerGroupID = CONVERT(varchar(50), cug.CustomerGroupID)");

            query.AppendLine("WHERE 1=1");

            var _total = SqlExQuery<TotalPaging>.ExcuteQueryFirst(query.ToString());
            total = _total != null ? _total.TotalCount : 0;

            return listData;
        }





    }
}
