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
    public interface IBM_Apartment_MemberService
    {
        IEnumerable<BM_Apartment_Member> GetAll();

        BM_Apartment_Member GetById(Guid id);

        MessageReport Create(BM_Apartment_Member obj);

        MessageReport Update(BM_Apartment_Member obj);

        MessageReport DeleteById(string id, ref BM_Apartment_Member obj);
        List<BM_ResidentCustom> GetMemberApartment(string ApartmentId);

        void DeleteMemberApartment(string ApartmentId);
    }

    public class BM_Apartment_MemberService : IBM_Apartment_MemberService
    {
        private IBM_Apartment_MemberRepository _BM_Apartment_MemberRepository;
        private IUnitOfWork _UnitOfWork;
        public BM_Apartment_MemberService(IBM_Apartment_MemberRepository _BM_Apartment_MemberRepository, IUnitOfWork _UnitOfWork)
        {
            this._BM_Apartment_MemberRepository = _BM_Apartment_MemberRepository;
            this._UnitOfWork = _UnitOfWork;
        }

        private void Save()
        {
            _UnitOfWork.Commit();
        }

        public MessageReport Create(BM_Apartment_Member obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _BM_Apartment_MemberRepository.Add(obj);

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

        public MessageReport DeleteById(string id, ref BM_Apartment_Member obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                obj = GetById(Guid.Parse(id));
                if (obj != null)
                {
                    _BM_Apartment_MemberRepository.Delete(n => n.Id.ToString() == id);

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

        public IEnumerable<BM_Apartment_Member> GetAll()
        {
            var query = from n in _BM_Apartment_MemberRepository.Table
                        select n;

            return query;
        }


        public BM_Apartment_Member GetById(Guid id)
        {
            return _BM_Apartment_MemberRepository.GetById(id);
        }

        public MessageReport Update(BM_Apartment_Member obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _BM_Apartment_MemberRepository.Update(obj);

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
        public List<BM_ResidentCustom> GetMemberApartment(string ApartmentId)
        {

            //Lấy danh sách
            var sb = new StringBuilder();

            sb.AppendLine("select m.RoleId,r.Id,r.Name,r.Mobile,r.Email");
            sb.AppendLine("from BM_Apartment_Member m");
            sb.AppendLine("left join BM_Resident r on r.Id = m.ResidentId");
            sb.AppendLine(string.Format("where m.IsDeleted = 'False' and m.ApartmentId = '{0}'", ApartmentId));

            var listData = SqlExQuery<BM_ResidentCustom>.ExcuteQuery(sb.ToString());

            return listData;
        }

        public void DeleteMemberApartment(string ApartmentId)
        {
            //Lấy danh sách
            var sb = new StringBuilder();

            sb.AppendLine("delete BM_Apartment_Member");          
            sb.AppendLine(string.Format("where ApartmentId = '{0}'", ApartmentId));

            ExcuteSQL.Execute(sb.ToString());
        }
    }
}

