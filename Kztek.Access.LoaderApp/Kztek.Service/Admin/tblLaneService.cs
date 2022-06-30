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
    public interface ItblLaneService
    {
        IEnumerable<tblLane> GetAll();

        IEnumerable<tblLane> GetAllByCamera(string id);

        IEnumerable<tblLane> GetAllActive();

        IEnumerable<tblLane> GetAllActiveById(string id);

        IEnumerable<tblLane> GetAllActiveByListId(string ids);
        IEnumerable<tblLane> GetAllByListId(string ids);

        IEnumerable<tblLane> GetAllPagingByFirst(string key, string pc, int pageNumber, int pageSize, ref int totalPage, ref int totalItem);

        IPagedList<tblLane> GetAllCustomPagingByFirst(string key, string pc, int pageNumber, int pageSize);

        tblLane GetById(Guid id);

        tblLane GetByName(string name);

        tblLane GetByName_Id(string name, Guid id);

        MessageReport Create(tblLane obj);

        MessageReport Update(tblLane obj);

        MessageReport DeleteById(string id, ref tblLane obj);

        string GetTitle(string landids);
    }

    public class tblLaneService : ItblLaneService
    {
        private ItblLaneRepository _tblLaneRepository;
        private ItblPCRepository _tblPCRepository;
        private IUnitOfWork _UnitOfWork;

        public tblLaneService(ItblLaneRepository _tblLaneRepository, ItblPCRepository _tblPCRepository, IUnitOfWork _UnitOfWork)
        {
            this._tblLaneRepository = _tblLaneRepository;
            this._tblPCRepository = _tblPCRepository;
            this._UnitOfWork = _UnitOfWork;
        }

        private void Save()
        {
            _UnitOfWork.Commit();
        }

        public MessageReport Create(tblLane obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblLaneRepository.Add(obj);

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

        public MessageReport DeleteById(string id, ref tblLane obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                obj = GetById(Guid.Parse(id));
                if (obj != null)
                {
                    _tblLaneRepository.Delete(n => n.LaneID.ToString() == id);

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

        public IEnumerable<tblLane> GetAll()
        {
            var query = from n in _tblLaneRepository.Table
                        select n;

            return query;
        }

        public IEnumerable<tblLane> GetAllActive()
        {
            var query = from n in _tblLaneRepository.Table
                        where n.Inactive == false
                        select n;

            return query;
        }

        public IEnumerable<tblLane> GetAllActiveById(string id)
        {
            var query = from n in _tblLaneRepository.Table
                        where n.Inactive == false && n.LaneID.ToString() == id
                        select n;

            return query;
        }

        public IEnumerable<tblLane> GetAllActiveByListId(string ids)
        {
            var query = from n in _tblLaneRepository.Table
                        where n.Inactive == false && ids.Contains(n.LaneID.ToString())
                        select n;

            return query;
        }
        public IEnumerable<tblLane> GetAllByListId(string ids)
        {
            var query = from n in _tblLaneRepository.Table
                        where ids.Contains(n.LaneID.ToString())
                        select n;

            return query;
        }

        public IEnumerable<tblLane> GetAllByCamera(string id)
        {
            var query = from n in _tblLaneRepository.Table
                        where n.Inactive == false && (n.C1 == id || n.C2 == id || n.C3 == id || n.C4 == id || n.C5 == id || n.C6 == id )
                        select n;

            return query;
        }

        public IPagedList<tblLane> GetAllCustomPagingByFirst(string key, string pc, int pageNumber, int pageSize)
        {
            var query = from n in _tblLaneRepository.Table
                        select n;
            //var query = (from n in _tblLaneRepository.Table
            //             join m in _tblPCRepository.Table on n.PCID equals m.PCID.ToString() into n_m
            //             from m in n_m.DefaultIfEmpty()

            //             select new tblLaneCustomViewModel()
            //             {
            //                 Inactive = n.Inactive,
            //                 LaneID = n.LaneID.ToString(),
            //                 LaneName = n.LaneName,
            //                 LaneType = n.LaneType.Value,
            //                 PCID = n.PCID,
            //                 PCName = m.ComputerName,
            //                 SortOrder = n.SortOrder,
            //             });

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.LaneName.Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(pc))
            {
                query = query.Where(n => n.PCID == pc);
            }

            var list = new PagedList<tblLane>(query.OrderByDescending(n => n.LaneName), pageNumber, pageSize);

            return list;
        }

        public IEnumerable<tblLane> GetAllPagingByFirst(string key, string pc, int pageNumber, int pageSize, ref int totalPage, ref int totalItem)
        {
            var query = from n in _tblLaneRepository.Table
                        select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.LaneName.Contains(key));
            }

            if (!string.IsNullOrWhiteSpace(pc))
            {
                query = query.Where(n => n.PCID == pc);
            }

            var list = new PagedList<tblLane>(query.OrderByDescending(n => n.SortOrder), pageNumber, pageSize);

            return list;
        }

        public tblLane GetById(Guid id)
        {
            return _tblLaneRepository.GetById(id);
        }

        public MessageReport Update(tblLane obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblLaneRepository.Update(obj);

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

        public tblLane GetByName(string name)
        {
            var query = from n in _tblLaneRepository.Table
                        where n.LaneName == name
                        select n;

            return query.FirstOrDefault();
        }

        public tblLane GetByName_Id(string name, Guid id)
        {
            var query = from n in _tblLaneRepository.Table
                        where n.LaneName == name && n.LaneID != id
                        select n;

            return query.FirstOrDefault();
        }

        public string GetTitle(string landids)
        {
            var str = "";

            var query = from n in _tblLaneRepository.Table
                        select n;

            var listdata = query.ToList();

            if (!string.IsNullOrEmpty(landids))
            {
                var obj = listdata.Where(n => landids.Contains(n.LaneID.ToString())).FirstOrDefault();
               
                var listid = landids.Split(',');

                if(listdata.Count() > 0 && obj != null)
                {                
                    str = ((listid.Length - 1) < listdata.Count()) ? obj.LaneCode : "";
                }

               
            }
          
            return str;
        }
    }
}
