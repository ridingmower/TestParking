using Kztek.Data.Infrastructure;
using Kztek.Data.Repository;
using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Service.Admin
{
    public interface ItblSubSystemService
    {
        IEnumerable<tblSubSystem> GetAll();

        IEnumerable<tblSubSystem> GetAllChildByParentId(string id);

        tblSubSystem GetById(string id);

        MessageReport Create(tblSubSystem obj);

        MessageReport Update(tblSubSystem obj);

        MessageReport DeleteById(string id, ref tblSubSystem obj);
    }

    public class tblSubSystemService : ItblSubSystemService
    {
        private ItblSubSystemRepository _tblSubSystemRepository;
        private IUnitOfWork _UnitOfWork;

        public tblSubSystemService(ItblSubSystemRepository _tblSubSystemRepository, IUnitOfWork _UnitOfWork)
        {
            this._tblSubSystemRepository = _tblSubSystemRepository;
            this._UnitOfWork = _UnitOfWork;
        }

        private void Save()
        {
            _UnitOfWork.Commit();
        }

        public MessageReport Create(tblSubSystem obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblSubSystemRepository.Add(obj);

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

        public MessageReport DeleteById(string id, ref tblSubSystem obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                obj = GetById(id);
                if (obj != null)
                {
                    _tblSubSystemRepository.Delete(n => n.SubSystemID.ToString() == id);

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

        public IEnumerable<tblSubSystem> GetAll()
        {
            var query = from n in _tblSubSystemRepository.Table
                        select n;

            return query.OrderBy(n => n.SortOrder);
        }

        public IEnumerable<tblSubSystem> GetAllChildByParentId(string id)
        {
            var query = from n in _tblSubSystemRepository.Table
                        where n.ParentID == id
                        select n;

            return query.OrderBy(n => n.SortOrder);
        }

        public tblSubSystem GetById(string id)
        {
            return _tblSubSystemRepository.GetById(id);
        }

        public MessageReport Update(tblSubSystem obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblSubSystemRepository.Update(obj);

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
