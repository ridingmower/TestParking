using Kztek.Data.Event.Infrastructure;
using Kztek.Data.Event.Repository;
using Kztek.Data.Event.SqlHelper;
using Kztek.Data.Repository;
using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Model.Models.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Service.Admin.Event
{
    public interface IPublicEventService
    {
        IEnumerable<PublicEvent> GetAll();
        List<string> GetId();
        PublicEvent GetById(Guid id);
        PublicEvent GetByEventId(string id);
        MessageReport Create(PublicEvent obj);
        MessageReport Update(PublicEvent obj);
        MessageReport DeleteById(string id);
        MessageReport DeleteMulti(List<string> lstid);
    }
    public class PublicEventService : IPublicEventService
    {
        private IPublicEventRepository _PublicEventRepository;
        private IUnitOfWork _UnitOfWork;

        public PublicEventService(IPublicEventRepository _PublicEventRepository, IUnitOfWork _UnitOfWork)
        {
            this._PublicEventRepository = _PublicEventRepository;
            this._UnitOfWork = _UnitOfWork;
        }

        private void Save()
        {
            _UnitOfWork.Commit();
        }

        public MessageReport Create(PublicEvent obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _PublicEventRepository.Add(obj);

                Save();

                re.Message = "Thêm mới thành công";
                re.isSuccess = true;
            }
            catch (Exception ex)
            {
                re.Message = ex.Message;
                re.isSuccess = false;
            }

            return re;
        }

        public MessageReport DeleteById(string id)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                var obj = GetById(Guid.Parse(id));
                if (obj != null)
                {
                    _PublicEventRepository.Delete(n => n.Id.ToString() == id);

                    Save();

                    re.Message = "Xóa thành công";
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
        public MessageReport DeleteMulti(List<string> lstid)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {               
                if (lstid != null && lstid.Count > 0)
                {
                    var str = new StringBuilder();
                    str.AppendLine("delete from PublicEvent where");

                    var count = 0;

                    str.AppendLine(" EventID IN ( ");

                    foreach (var item in lstid)
                    {
                        count++;

                        str.AppendLine(string.Format("'{0}'{1}", item, count == lstid.Count ? "" : ","));
                    }

                    str.AppendLine(" )");

                    ExcuteSQLEvent.GetDataSet(str.ToString(), false);

                    re.Message = "Xóa thành công";
                    re.isSuccess = true;
                }
                else
                {
                    re.Message = "Có lỗi xảy ra";
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

        public IEnumerable<PublicEvent> GetAll()
        {
            var query = from n in _PublicEventRepository.Table
                        select n;

            return query;
        }
        public PublicEvent GetByEventId(string id)
        {
            var query = from n in _PublicEventRepository.Table
                        where n.EventID.Equals(id)
                        select n;

            return query.FirstOrDefault();
        }
        public List<string> GetId()
        {
            var query = from n in _PublicEventRepository.Table
                        select n.EventID;

            return query.ToList();
        }


        public PublicEvent GetById(Guid id)
        {
            return _PublicEventRepository.GetById(id);
        }

        public MessageReport Update(PublicEvent obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _PublicEventRepository.Update(obj);

                Save();

                re.Message = "Cập nhật thành công";
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
