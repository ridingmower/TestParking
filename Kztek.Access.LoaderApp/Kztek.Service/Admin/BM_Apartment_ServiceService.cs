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
    public interface IBM_Apartment_ServiceService
    {
        IEnumerable<BM_Apartment_Service> GetAll();


        IPagedList<BM_Apartment_Service> GetAllPagingByFirst(string key, int pageNumber, int pageSize);


        BM_Apartment_Service GetById(string id);
 

        MessageReport Create(BM_Apartment_Service obj);

        MessageReport Update(BM_Apartment_Service obj);

        MessageReport DeleteById(string id, ref BM_Apartment_Service obj);

        List<BM_Building_ServiceCustom> GetServiceApartment(string ApartmentId);

        void DeleteServiceApartment(string ApartmentId);
    }

    public class BM_Apartment_ServiceService : IBM_Apartment_ServiceService
    {
        private IBM_Apartment_ServiceRepository _BM_Apartment_ServiceRepository;

        private IUnitOfWork _UnitOfWork;

        public BM_Apartment_ServiceService(IBM_Apartment_ServiceRepository _BM_Apartment_ServiceRepository, IUnitOfWork _UnitOfWork)
        {
            this._BM_Apartment_ServiceRepository = _BM_Apartment_ServiceRepository;

            this._UnitOfWork = _UnitOfWork;
        }

        private void Save()
        {
            _UnitOfWork.Commit();
        }

        public MessageReport Create(BM_Apartment_Service obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _BM_Apartment_ServiceRepository.Add(obj);

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

        public MessageReport DeleteById(string id, ref BM_Apartment_Service obj)
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
                    _BM_Apartment_ServiceRepository.Update(obj);

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

        public IEnumerable<BM_Apartment_Service> GetAll()
        {
            var query = from n in _BM_Apartment_ServiceRepository.Table
                        select n;

            return query;
        }
     


        public IPagedList<BM_Apartment_Service> GetAllPagingByFirst(string key, int pageNumber, int pageSize)
        {
            var query = from n in _BM_Apartment_ServiceRepository.Table
                        where !n.IsDeleted
                        select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.ApartmentId.Contains(key));
            }


            var list = new PagedList<BM_Apartment_Service>(query.OrderByDescending(n => n.ApartmentId), pageNumber, pageSize);

            return list;
        }

        public BM_Apartment_Service GetById(string id)
        {
            return _BM_Apartment_ServiceRepository.GetById(id);
        }



        public MessageReport Update(BM_Apartment_Service obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _BM_Apartment_ServiceRepository.Update(obj);

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

        public List<BM_Building_ServiceCustom> GetServiceApartment(string ApartmentId)
        {

            //Lấy danh sách
            var sb = new StringBuilder();

            sb.AppendLine("select r.Id, r.Name, m.SchedulePay, m.ScheduleType, m.Price, ");
            sb.AppendLine("CONVERT(varchar(50),FORMAT(m.StartDate,'dd/MM/yyyy')) as DateStart,");
            sb.AppendLine("CONVERT(varchar(50),FORMAT(m.EndDate,'dd/MM/yyyy')) as DateEnd");
            sb.AppendLine("from BM_Apartment_Service m");
            sb.AppendLine("left join BM_Building_Service r on r.Id = m.ServiceId");
            sb.AppendLine(string.Format("where m.IsDeleted = 'False' and m.ApartmentId = '{0}'", ApartmentId));

            var listData = SqlExQuery<BM_Building_ServiceCustom>.ExcuteQuery(sb.ToString());

            return listData;
        }

        public void DeleteServiceApartment(string ApartmentId)
        {
            //Lấy danh sách
            var sb = new StringBuilder();

            sb.AppendLine("delete BM_Apartment_Service");
            sb.AppendLine(string.Format("where ApartmentId = '{0}'", ApartmentId));

            ExcuteSQL.Execute(sb.ToString());
        }
    }
}
