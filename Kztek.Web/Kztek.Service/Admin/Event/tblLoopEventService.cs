using Kztek.Data.Event.Repository;
using Kztek.Data.Event.SqlHelper;
using Kztek.Model.CustomModel;
using Kztek.Model.Models.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Service.Admin.Event
{
    public interface ItblLoopEventService
    {
        tblLoopEvent GetById(string id);

        MessageReport DeleteById(string id, ref string plate);
    }

    public class tblLoopEventService : ItblLoopEventService
    {
        private ItblLoopEventRepository _tblLoopEventRepository;

        public tblLoopEventService(ItblLoopEventRepository _tblLoopEventRepository)
        {
            this._tblLoopEventRepository = _tblLoopEventRepository;
        }

        public MessageReport DeleteById(string id, ref string plate)
        {
            var result = new MessageReport();
            result.Message = "Có lỗi xảy ra";
            result.isSuccess = false;

            try
            {
                //var obj = GetById(id);
                //if (obj != null)
                //{
                //    plate = obj.Plate;
                //}

                var str = string.Format("UPDATE tblLoopEvent SET EventCode='2', IsDelete=1 WHERE Id = '{0}'", id);

                SqlExQuery<tblLoopEvent>.ExcuteNone(str);

                result.Message = "Xóa thành công";
                result.isSuccess = true;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.isSuccess = false;
            }

            return result;
        }

        public tblLoopEvent GetById(string id)
        {
            return _tblLoopEventRepository.GetById(id);
        }
    }
}
