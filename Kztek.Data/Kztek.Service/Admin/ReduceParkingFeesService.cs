using Kztek.Data.Infrastructure;
using Kztek.Data.Repository;
using Kztek.Model.CustomModel;
using Kztek.Model.CustomModel.iParking;
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
    public interface IReduceParkingFeesService
    {
        IPagedList<tblDiscountParking> GetAllPagingByFirst(string key, int page, int pageSize);
        MessageReport Create(tblDiscountParking obj);
        tblDiscountParking GetById(string id);
        MessageReport Update(tblDiscountParking oldObj);
        MessageReport DeleteById(string id, ref tblDiscountParking obj);
        tblDiscountParking_Submit GetCustomById(string id);
        IEnumerable <tblDiscountParking> GetAll();
    }
    public class ReduceParkingFeesService : IReduceParkingFeesService
    {
        private IReduceParkingFeesRepository _ReduceParkingFeesRepository;
        private IUnitOfWork _UnitOfWork;
        public ReduceParkingFeesService(IReduceParkingFeesRepository _ReduceParkingFeesRepository, IUnitOfWork _UnitOfWork)
        {
            this._ReduceParkingFeesRepository = _ReduceParkingFeesRepository;
            this._UnitOfWork = _UnitOfWork;
        }
        private void Save()
        {
            _UnitOfWork.Commit();
        }
        public MessageReport Create(tblDiscountParking obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _ReduceParkingFeesRepository.Add(obj);

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

        public IPagedList<tblDiscountParking> GetAllPagingByFirst(string key, int page, int pageSize)
        {
            var query = from n in _ReduceParkingFeesRepository.Table
                        select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.DCTypeName.Contains(key) || n.DCTypeName.Contains(key));
            }

            var list = new PagedList<tblDiscountParking>(query.OrderByDescending(n => n.DCTypeName), page, pageSize);

            return list; ;
        }

        public tblDiscountParking GetById(string id)
        {
            return _ReduceParkingFeesRepository.GetById(id);
        }

        public MessageReport Update(tblDiscountParking oldObj)
        {

            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _ReduceParkingFeesRepository.Update(oldObj);

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

        public MessageReport DeleteById(string id, ref tblDiscountParking obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                obj = GetById((id));
                if (obj != null)
                {
                    _ReduceParkingFeesRepository.Delete(n => n.Id.ToString() == id);

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

        public tblDiscountParking_Submit GetCustomById(string id)
        {
            var model = new tblDiscountParking_Submit();
            var obj = GetById(id);
            if (obj != null)
            {
                model.Id = obj.Id.ToString();
                model.NameDiscountType = obj.DCTypeName;
                model.CodeDiscountType = obj.DCTypeCode;
                model.DiscountMode = obj.DiscountMode;
                model.AmountReduced = obj.DiscountAmount.ToString(/*"###,###"*/);
                model.Note = obj.Note;
                model.Priority = obj.Priority.ToString();

            }
            return model;
        }

        public IEnumerable<tblDiscountParking> GetAll()
        {
            var query = from n in _ReduceParkingFeesRepository.Table
                        select n;
            return query.AsEnumerable().ToList();
        }
    }
}
