using Kztek.Data.SqlHelper;
using Kztek.Data.Event;
using Kztek.Data.Repository;
using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Web.Core.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kztek.Data.Infrastructure;

namespace Kztek.Service.Admin
{
    
    public interface IMultipleLanesMapService
    {
      
        MessageReport Create(MultipleLanesMap obj);
        IEnumerable<MultipleLanesMap> GetAll();
        MessageReport CreateQr(List<MultipleLanesMap> list);
        MessageReport DeleteAll();
        MultipleLanesMap GetById(string id);
        MultipleLanesMap GetByViewOrder(int viewOrder);
        MessageReport Update(MultipleLanesMap model);
    }
    public class MultipleLanesMapService : IMultipleLanesMapService
    {
        private IMultipleLanesMapRepository _MultipleLanesMapRepository;
        private IUnitOfWork unitOfWork;
        public MultipleLanesMapService(IMultipleLanesMapRepository _MultipleLanesMapRepository, IUnitOfWork unitOfWork)
        {
            this._MultipleLanesMapRepository = _MultipleLanesMapRepository;
            this.unitOfWork = unitOfWork;
        }
        private void Save()
        {
            unitOfWork.Commit();
        }
      

       
        public MessageReport Create(MultipleLanesMap obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _MultipleLanesMapRepository.Add(obj);

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

        public IEnumerable<MultipleLanesMap> GetAll()
        {
            var que = from n in _MultipleLanesMapRepository.Table
                      select n;
            return que.ToList();
        }

        public MessageReport CreateQr(List<MultipleLanesMap> list)
        {
            var result = new MessageReport(true, "ok");
            
            bool issuccess = false;
            foreach (var item in list)
            {
                var query = new StringBuilder();
                query.AppendLine(string.Format("insert into MultipleLanesMap (Id, PCid, ViewOrder, SideIndex, CurrentDirection) values ('{0}', '{1}', '{2}', '{3}', '{4}')", Guid.NewGuid().ToString(), item.PCid, Convert.ToInt32(item.ViewOrder), Convert.ToInt32(item.SideIndex), Convert.ToInt32(item.CurrentDirection)));
            issuccess =    ExcuteSQL.Execute(query.ToString());
            }
            if (issuccess)
            {
                result.isSuccess = issuccess;
                result.Message = "Thành công";
            }
            return result;
        }

        public MessageReport DeleteAll()
        {
            var re = new MessageReport(false, "Có lỗi");
            var q = new StringBuilder();
            q.AppendLine("Delete from MultipleLanesMap");
            bool  check = ExcuteSQL.Execute(   q .ToString());
            if (check)
            {
                re.isSuccess = check;
                re.Message = "Thành công";

            }
            return re;
        }

        public MultipleLanesMap GetById(string id)
        {
            var q = _MultipleLanesMapRepository.GetById(id);
            return q;
        }

        public MultipleLanesMap GetByViewOrder(int viewOrder)
        {
            var str = from n in _MultipleLanesMapRepository.Table
                      where n.ViewOrder == viewOrder
                      select n;
            return str.FirstOrDefault();
        }

        public MessageReport Update(MultipleLanesMap model)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _MultipleLanesMapRepository.Update(model);

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
