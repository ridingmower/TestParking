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
    public interface IBM_ApartmentEWPriceService
    {
        IEnumerable<BM_ApartmentEWPrice> GetAll();


        IEnumerable<BM_ApartmentEWPrice> GetAllPagingByFirst(int pageNumber, int pageSize, ref int totalPage, ref int totalItem);


        BM_ApartmentEWPrice GetById(int id);

        MessageReport Create(BM_ApartmentEWPrice obj);

        MessageReport Update(BM_ApartmentEWPrice obj);

        MessageReport DeleteById(string id, ref BM_ApartmentEWPrice obj);
        BM_ApartmentEWPrice GetDefault();
    }

    public class BM_ApartmentEWPriceService : IBM_ApartmentEWPriceService
    {
        private IBM_ApartmentEWPriceRepository _BM_ApartmentEWPriceRepository;

        private IUnitOfWork _UnitOfWork;

        public BM_ApartmentEWPriceService(IBM_ApartmentEWPriceRepository _BM_ApartmentEWPriceRepository, IUnitOfWork _UnitOfWork)
        {
            this._BM_ApartmentEWPriceRepository = _BM_ApartmentEWPriceRepository;

            this._UnitOfWork = _UnitOfWork;
        }

        private void Save()
        {
            _UnitOfWork.Commit();
        }
        public BM_ApartmentEWPrice GetDefault()
        {
            var query = from n in _BM_ApartmentEWPriceRepository.Table
                        select n;

            return query.FirstOrDefault();
        }
        public MessageReport Create(BM_ApartmentEWPrice obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _BM_ApartmentEWPriceRepository.Add(obj);

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

        public MessageReport DeleteById(string id, ref BM_ApartmentEWPrice obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                obj = GetById(Convert.ToInt32(id));
                if (obj != null)
                {
                    _BM_ApartmentEWPriceRepository.Delete(n => n.Id.ToString() == id);

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

        public IEnumerable<BM_ApartmentEWPrice> GetAll()
        {
            var query = from n in _BM_ApartmentEWPriceRepository.Table
                        select n;

            return query;
        }



        public IEnumerable<BM_ApartmentEWPrice> GetAllPagingByFirst(int pageNumber, int pageSize, ref int totalPage, ref int totalItem)
        {
            var query = from n in _BM_ApartmentEWPriceRepository.Table
                        select n;

          

            var list = new PagedList<BM_ApartmentEWPrice>(query.OrderByDescending(n => n.DateCreated), pageNumber, pageSize);

            return list;
        }

        public BM_ApartmentEWPrice GetById(int id)
        {
            return _BM_ApartmentEWPriceRepository.GetById(id);
        }



        public MessageReport Update(BM_ApartmentEWPrice obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _BM_ApartmentEWPriceRepository.Update(obj);

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
    }
}
