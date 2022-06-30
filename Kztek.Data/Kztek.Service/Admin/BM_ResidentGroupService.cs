using Kztek.Data.Infrastructure;
using Kztek.Data.Repository;
using Kztek.Data.SqlHelper;
using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Web.Core.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Service.Admin
{
    public interface IBM_ResidentGroupService
    {
        IEnumerable<BM_ResidentGroup> GetAll();

        IEnumerable<BM_ResidentGroup> GetAllChildByParentID(string id);

        IEnumerable<BM_ResidentGroup> GetAllActiveByListId(string listId);
        IEnumerable<BM_ResidentGroup> GetAllPagingByFirst(string key, int pageNumber, int pageSize, ref int totalPage, ref int totalItem);

        BM_ResidentGroup GetById(string id);

        BM_ResidentGroup GetByName(string name);

        MessageReport Create(BM_ResidentGroup obj);

        MessageReport Update(BM_ResidentGroup obj);

        MessageReport DeleteById(string id, ref BM_ResidentGroup obj);

        bool DeleteByIds(string lstId);
        int CountParent();

        IEnumerable<BM_ResidentGroup> GetAllChildActiveByParentID(string id);
    }

    public class BM_ResidentGroupService : IBM_ResidentGroupService
    {
        private IBM_ResidentGroupRepository _BM_ResidentGroupRepository;
        private readonly IUnitOfWork _UnitOfWork;

        public BM_ResidentGroupService(IBM_ResidentGroupRepository _BM_ResidentGroupRepository, IUnitOfWork _UnitOfWork)
        {
            this._BM_ResidentGroupRepository = _BM_ResidentGroupRepository;
            this._UnitOfWork = _UnitOfWork;
        }

        private void Save()
        {
            _UnitOfWork.Commit();
        }

        public MessageReport Create(BM_ResidentGroup obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _BM_ResidentGroupRepository.Add(obj);

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

        public MessageReport DeleteById(string id, ref BM_ResidentGroup obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                obj = GetById(id);
                if (obj != null)
                {
                    _BM_ResidentGroupRepository.Delete(n => n.Id.ToString() == id);

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

        public IEnumerable<BM_ResidentGroup> GetAll()
        {
            var query = from n in _BM_ResidentGroupRepository.Table
                        where !n.IsDeleted 
                        orderby n.Ordering ascending
                        select n;
            return query;
        }
    

        public IEnumerable<BM_ResidentGroup> GetAllActiveByListId(string listId)
        {
            var query = from n in _BM_ResidentGroupRepository.Table
                        where listId.Contains(n.Id.ToString())
                        select n;

            return query;
        }

    

        public IEnumerable<BM_ResidentGroup> GetAllChildByParentID(string id)
        {
            var query = from n in _BM_ResidentGroupRepository.Table
                        where n.ParentId == id
                        select n;
            return query;
        }

        public IEnumerable<BM_ResidentGroup> GetAllPagingByFirst(string key, int pageNumber, int pageSize, ref int totalPage, ref int totalItem)
        {
            throw new NotImplementedException();
        }

        public BM_ResidentGroup GetById(string id)
        {
            return _BM_ResidentGroupRepository.GetById(id);
        }

        public BM_ResidentGroup GetByName(string name)
        {
            var query = from n in _BM_ResidentGroupRepository.Table
                        where n.Name == name
                        select n;

            return query.FirstOrDefault();
        }

        public MessageReport Update(BM_ResidentGroup obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _BM_ResidentGroupRepository.Update(obj);

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

        public bool DeleteByIds(string lstId)
        {
            bool isSuccess = false;
            try
            {
                var lstParentId = (from m in _BM_ResidentGroupRepository.Table
                                   where lstId.Contains(m.Id.ToString())
                                   select m.ParentId).Distinct().ToList();

                string[] ids = lstId.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var id in ids)
                {
                    var str = "Delete from BM_ResidentGroup Where Id = '" + id + "'";
                    SqlExQuery<BM_ResidentGroup>.ExcuteNone(str);
                }

                isSuccess = true;

                if (isSuccess)
                {
                    #region cập nhật số TT                     
                    if (lstParentId.Any())
                    {
                        foreach (var item2 in lstParentId)
                        {
                            var lst = (from m in _BM_ResidentGroupRepository.Table
                                       where  m.ParentId == item2
                                       orderby m.Ordering ascending
                                       select m).ToList();

                            if (lst.Any())
                            {
                                int count = 0;
                                foreach (var item1 in lst)
                                {
                                    count++;
                                    item1.Ordering = count;
                                    _BM_ResidentGroupRepository.Update(item1);
                                    Save();
                                }
                            }

                        }
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                isSuccess = false;
            }
            return isSuccess;
        }

        public string GetChildCustomerGroupByparent(string parentId)
        {
            var str = string.Format("('{0}'", parentId);
            //Customergroup 
            var dtcustomergroup = GetAll().ToList();
            //DataTable dtcustomergroup = StaticPool.mdb.FillData("select CustomerGroupID, CustomerGroupName from BM_ResidentGroup order by SortOrder");
            if (dtcustomergroup != null && dtcustomergroup.Any())
            {
                var parentNote = dtcustomergroup.Where(n => n.ParentId == parentId);

                if (parentNote.Any())
                {
                    foreach (var item in parentNote)
                    {
                        // cbCustomerGroup.Items.Add(new ListItem(dr["CustomerGroupName"].ToString(), dr["CustomerGroupID"].ToString()));
                        str += ",'" + item.Id.ToString() + "'";
                        var child = dtcustomergroup.Where(n => n.ParentId == item.Id.ToString());
                        if (child.Any())
                        {
                            str += FindChildByCusGroupParent(item.Id.ToString(), dtcustomergroup);
                        }
                    }
                }

            }
            return str + ")";
        }

        private string FindChildByCusGroupParent(string parentId, List<BM_ResidentGroup> _dt)
        {
            var str = "";
            var child = _dt.Where(n => n.ParentId == parentId);
            if (child.Any())
            {
                foreach (var item in child)
                {
                    str += ",'" + item.Id.ToString() + "'";
                    var childN = _dt.Where(n => n.ParentId == item.Id.ToString());
                    if (childN.Any())
                    {
                        str += FindChildByCusGroupParent(item.Id.ToString(), _dt);
                    }
                }
            }

            return str;
        }
        public int CountParent()
        {
            var sb = new StringBuilder();

            sb.AppendLine("SELECT COUNT(*) AS COUNT FROM BM_ResidentGroup WHERE ParentId = '0' ");

            var lst = ExcuteSQL.GetDataSet(sb.ToString(), false);


            var count = Convert.ToInt32(lst.Tables[0].Rows[0][0].ToString());


            return count;
        }

        public IEnumerable<BM_ResidentGroup> GetAllChildActiveByParentID(string id)
        {
            var query = from n in _BM_ResidentGroupRepository.Table
                        where n.IsDeleted == false && n.ParentId == id
                        select n;
            return query;
        }
    }
}
