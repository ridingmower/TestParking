using Kztek.Data.Infrastructure;
using Kztek.Data.Repository;
using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Service.Admin
{
    public interface IExcelColumnService
    {
        IEnumerable<ExcelColumn> GetAll();
        IEnumerable<ExcelColumn> GetAllActive();

        //IEnumerable<tblFee> GetAllPagingByFirst(string key, string cardgroup, int pageNumber, int pageSize, ref int totalPage, ref int totalItem);

        IPagedList<ExcelColumnCustom> GetAllPagingByFirst(string key, int pageNumber, int pageSize, ref int totalPage, ref int totalItem);

        ExcelColumn GetById(string id);

        ExcelColumn GetByMenuFunctionId(string id);

        MessageReport Create(ExcelColumn obj);

        MessageReport Update(ExcelColumn obj);

        MessageReport DeleteById(string id);
    }
    public class ExcelColumnService: IExcelColumnService
    {
        private IExcelColumnRepository _ExcelColumnRepository;
        private IMenuFunctionRepository _MenuFunctionRepository;
        private IUnitOfWork _UnitOfWork;

        public ExcelColumnService(IExcelColumnRepository _ExcelColumnRepository, IMenuFunctionRepository _MenuFunctionRepository, IUnitOfWork _UnitOfWork)
        {       
            this._ExcelColumnRepository = _ExcelColumnRepository;
            this._MenuFunctionRepository = _MenuFunctionRepository;
            this._UnitOfWork = _UnitOfWork;
        }

        private void Save()
        {
            _UnitOfWork.Commit();
        }

        public MessageReport Create(ExcelColumn obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _ExcelColumnRepository.Add(obj);

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
                var obj = GetById(id);
                if (obj != null)
                {
                    _ExcelColumnRepository.Delete(obj);

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

        public ExcelColumn GetById(string id)
        {
            return _ExcelColumnRepository.GetById(id);
        }

        public ExcelColumn GetByMenuFunctionId(string id)
        {
            var query = from n in _ExcelColumnRepository.Table
                        where n.MenuFunctionId.Equals(id)
                        select n;

            return query.FirstOrDefault();
        }

        public MessageReport Update(ExcelColumn obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _ExcelColumnRepository.Update(obj);

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

        public IEnumerable<ExcelColumn> GetAll()
        {
            var query = from n in _ExcelColumnRepository.Table
                        select n;

            return query;
        }

        public IEnumerable<ExcelColumn> GetAllActive()
        {
            var query = from n in _ExcelColumnRepository.Table
                        where n.Active
                        select n;

            return query;
        }

        public IPagedList<ExcelColumnCustom> GetAllPagingByFirst(string key, int pageNumber, int pageSize, ref int totalPage, ref int totalItem)
        {
            var a = "";
            var query = (from n in _ExcelColumnRepository.Table
                         join m in _MenuFunctionRepository.Table on n.MenuFunctionId equals m.Id.ToString() into n_m
                         from m in n_m.DefaultIfEmpty()
                         select new ExcelColumnCustom()
                         {
                             Id = n.Id,
                             ColName = n.ColName,
                             ColValue = n.ColValue,
                             Active = n.Active,
                             MenuFunctionId = n.MenuFunctionId,
                             MenuFunctionName = m.MenuName
                         });

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.MenuFunctionName.Contains(key));
            }

           
            var list = new PagedList<ExcelColumnCustom>(query.OrderByDescending(n => n.MenuFunctionName), pageNumber, pageSize);

            return list;
        }
    }
}
