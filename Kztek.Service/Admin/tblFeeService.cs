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
    public interface ItblFeeService
    {
        IEnumerable<tblFee> GetAll();

        //IEnumerable<tblFee> GetAllPagingByFirst(string key, string cardgroup, int pageNumber, int pageSize, ref int totalPage, ref int totalItem);

        IPagedList<FeeCustom> GetAllCustomPagingByFirst(string key, string cardgroup, int pageNumber, int pageSize);

        tblFee GetById(int id);

        tblFee GetByName(string name);

        tblFee GetByName_Id(string name, int id);

        tblFee GetByCateId(string id);

        tblFee GetByCateId_Extend(string id);
        List<tblFee> Get_Extend();

        MessageReport Create(tblFee obj);

        MessageReport Update(tblFee obj);

        MessageReport DeleteById(int id, ref tblFee obj);
    }
    public class tblFeeService: ItblFeeService
    {
        private ItblFeeRepository _tblFeeRepository;
        private ItblCardGroupRepository _tblCardGroupRepository;
        private IUnitOfWork _UnitOfWork;

        public tblFeeService(ItblFeeRepository _tblFeeRepository, ItblCardGroupRepository _tblCardGroupRepository, IUnitOfWork _UnitOfWork)
        {
            this._tblFeeRepository = _tblFeeRepository;
            this._tblCardGroupRepository = _tblCardGroupRepository;
            this._UnitOfWork = _UnitOfWork;
        }

        private void Save()
        {
            _UnitOfWork.Commit();
        }

        public MessageReport Create(tblFee obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblFeeRepository.Add(obj);

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

        public MessageReport DeleteById(int id, ref tblFee obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                obj = GetById(id);
                if (obj != null)
                {
                    _tblFeeRepository.Delete(n => n.FeeID == id);

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

        public IEnumerable<tblFee> GetAll()
        {
            var query = from n in _tblFeeRepository.Table
                        select n;

            return query;
        }

        public IPagedList<FeeCustom> GetAllCustomPagingByFirst(string key, string cardgroup, int pageNumber, int pageSize)
        {
            var a = "";
            var query = (from n in _tblFeeRepository.Table
                        join m in _tblCardGroupRepository.Table on n.CardGroupID equals m.CardGroupID.ToString() into n_m
                        from m in n_m.DefaultIfEmpty()
                        select new FeeCustom()
                        {
                            _id = n.FeeID.ToString(),
                            CardGroupName = m != null ? m.CardGroupName : "",
                            FeeLevel = n.FeeLevel,
                            FeeName = n.FeeName,
                            CardGroupID = m.CardGroupID.ToString(),
                            Unit = n.Units
                        });

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.CardGroupName.Contains(key) || n.FeeName.Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(cardgroup))
            {
                query = query.Where(n => n.CardGroupID.Contains(cardgroup));
            }

            var list = new PagedList<FeeCustom>(query.OrderByDescending(n => n.CardGroupName), pageNumber, pageSize);
            return list;
        }

        //public IEnumerable<tblFee> GetAllPagingByFirst(string key, string cardgroup, int pageNumber, int pageSize, ref int totalPage, ref int totalItem)
        //{
        //    var query = Query.And(Query.Matches("FeeName", key), Query.Matches("CardGroupID", cardgroup));

        //    return _PK_FeeRepository.GetPagingByFirst(query, "FeeName", pageNumber, pageSize, ref totalPage, ref totalItem, true).Entities;
        //}

        public tblFee GetById(int id)
        {
            return _tblFeeRepository.GetById(id);
        }

        public MessageReport Update(tblFee obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblFeeRepository.Update(obj);

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

        public tblFee GetByCateId(string id)
        {
            var query = from n in _tblFeeRepository.Table
                        where n.CardGroupID == id
                        select n;

            return query.FirstOrDefault();
        }
        public tblFee GetByCateId_Extend(string id)
        {
            var query = from n in _tblFeeRepository.Table
                        where n.CardGroupID == id && n.IsUseExtend
                        select n;

            return query.FirstOrDefault();
        }
        public List<tblFee> Get_Extend()
        {
            var query = from n in _tblFeeRepository.Table
                        where n.IsUseExtend
                        select n;

            return query.ToList();
        }
        public tblFee GetByName(string name)
        {
            var query = from n in _tblFeeRepository.Table
                        where n.FeeName == name
                        select n;

            return query.FirstOrDefault();
        }

        public tblFee GetByName_Id(string name, int id)
        {
            var query = from n in _tblFeeRepository.Table
                        where n.FeeName == name && n.FeeID != id
                        select n;

            return query.FirstOrDefault();
        }
    }
}
