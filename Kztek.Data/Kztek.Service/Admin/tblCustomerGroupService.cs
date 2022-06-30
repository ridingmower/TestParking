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
    public interface ItblCustomerGroupService
    {
        IEnumerable<tblCustomerGroup> GetAll();

        IEnumerable<tblCustomerGroup> GetAllActive();

        IEnumerable<tblCustomerGroup> GetAllActiveById(string id);

        IEnumerable<tblCustomerGroup> GetAllActiveByListId(string listId);

        IEnumerable<tblCustomerGroup> GetAllChildByParentID(string id);

        IEnumerable<tblCustomerGroup> GetAllChildActiveByParentID(string id);

        IEnumerable<tblCustomerGroup> GetAllPagingByFirst(string key, int pageNumber, int pageSize, ref int totalPage, ref int totalItem);

        tblCustomerGroup GetById(Guid id);

        tblCustomerGroup GetByName(string name);

        MessageReport Create(tblCustomerGroup obj);

        MessageReport Update(tblCustomerGroup obj);

        MessageReport DeleteById(string id, ref tblCustomerGroup obj);

        bool DeleteByIds(string lstId);
        int CountParent();
        IEnumerable<tblCustomerGroupExcel> GetExcel();
    }

    public class tblCustomerGroupService : ItblCustomerGroupService
    {
        private ItblCustomerGroupRepository _tblCustomerGroupRepository;
        private readonly IUnitOfWork _UnitOfWork;

        public tblCustomerGroupService(ItblCustomerGroupRepository _tblCustomerGroupRepository, IUnitOfWork _UnitOfWork)
        {
            this._tblCustomerGroupRepository = _tblCustomerGroupRepository;
            this._UnitOfWork = _UnitOfWork;
        }

        private void Save()
        {
            _UnitOfWork.Commit();
        }

        public MessageReport Create(tblCustomerGroup obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblCustomerGroupRepository.Add(obj);

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

        public MessageReport DeleteById(string id, ref tblCustomerGroup obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                obj = GetById(Guid.Parse(id));
                if (obj != null)
                {
                    _tblCustomerGroupRepository.Delete(n => n.CustomerGroupID.ToString() == id);

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

        public IEnumerable<tblCustomerGroup> GetAll()
        {
            var query = from n in _tblCustomerGroupRepository.Table
                        orderby n.Ordering ascending
                        select n;
            return query;
        }
        public IEnumerable<tblCustomerGroupExcel> GetExcel()
        {
            var query = from n in _tblCustomerGroupRepository.Table
                        where n.Inactive == false
                        orderby n.Ordering ascending
                        select new tblCustomerGroupExcel
                        {
                            CustomerGroupName = n.CustomerGroupName
                        };
            return query;
        }
        public IEnumerable<tblCustomerGroup> GetAllActive()
        {
            var query = from n in _tblCustomerGroupRepository.Table
                        where n.Inactive == false
                        select n;
            return query;
        }

        public IEnumerable<tblCustomerGroup> GetAllActiveById(string id)
        {
            var query = from n in _tblCustomerGroupRepository.Table
                        where n.Inactive == false && n.CustomerGroupID.ToString() == id
                        select n;
            return query;
        }

        public IEnumerable<tblCustomerGroup> GetAllActiveByListId(string listId)
        {
            var query = from n in _tblCustomerGroupRepository.Table
                        where listId.Contains(n.CustomerGroupID.ToString())
                        select n;

            return query;
        }

        public IEnumerable<tblCustomerGroup> GetAllChildActiveByParentID(string id)
        {
            var query = from n in _tblCustomerGroupRepository.Table
                        where n.Inactive == false && n.ParentID == id
                        select n;
            return query;
        }

        public IEnumerable<tblCustomerGroup> GetAllChildByParentID(string id)
        {
            var query = from n in _tblCustomerGroupRepository.Table
                        where n.ParentID == id
                        select n;
            return query;
        }

        public IEnumerable<tblCustomerGroup> GetAllPagingByFirst(string key, int pageNumber, int pageSize, ref int totalPage, ref int totalItem)
        {
            throw new NotImplementedException();
        }

        public tblCustomerGroup GetById(Guid id)
        {
            return _tblCustomerGroupRepository.GetById(id);
        }

        public tblCustomerGroup GetByName(string name)
        {
            var query = from n in _tblCustomerGroupRepository.Table
                        where n.CustomerGroupName == name
                        select n;

            return query.FirstOrDefault();
        }

        public MessageReport Update(tblCustomerGroup obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblCustomerGroupRepository.Update(obj);

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
                var lstParentId = (from m in _tblCustomerGroupRepository.Table
                                   where lstId.Contains(m.CustomerGroupID.ToString()) && !m.Inactive
                                   select m.ParentID).Distinct().ToList();

                string[] ids = lstId.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var id in ids)
                {
                    var str = "Delete from tblCustomerGroup Where CustomerGroupID = '" + id + "'";
                    SqlExQuery<tblCustomerGroup>.ExcuteNone(str);
                }

                isSuccess = true;

                if (isSuccess)
                {
                    #region cập nhật số TT                     
                    if (lstParentId.Any())
                    {
                        foreach (var item2 in lstParentId)
                        {
                            var lst = (from m in _tblCustomerGroupRepository.Table
                                       where !m.Inactive && m.ParentID == item2
                                       orderby m.Ordering ascending
                                       select m).ToList();

                            if (lst.Any())
                            {
                                int count = 0;
                                foreach (var item1 in lst)
                                {
                                    count++;
                                    item1.Ordering = count;
                                    _tblCustomerGroupRepository.Update(item1);
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
            //DataTable dtcustomergroup = StaticPool.mdb.FillData("select CustomerGroupID, CustomerGroupName from tblCustomerGroup order by SortOrder");
            if (dtcustomergroup != null && dtcustomergroup.Any())
            {
                var parentNote = dtcustomergroup.Where(n => n.ParentID == parentId);

                if (parentNote.Any())
                {
                    foreach (var item in parentNote)
                    {
                        // cbCustomerGroup.Items.Add(new ListItem(dr["CustomerGroupName"].ToString(), dr["CustomerGroupID"].ToString()));
                        str += ",'" + item.CustomerGroupID.ToString() + "'";
                        var child = dtcustomergroup.Where(n => n.ParentID == item.CustomerGroupID.ToString());
                        if (child.Any())
                        {
                            str += FindChildByCusGroupParent(item.CustomerGroupID.ToString(), dtcustomergroup);
                        }
                    }
                }

            }
            return str + ")";
        }

        private string FindChildByCusGroupParent(string parentId, List<tblCustomerGroup> _dt)
        {
            var str = "";
            var child = _dt.Where(n => n.ParentID == parentId);
            if (child.Any())
            {
                foreach (var item in child)
                {
                    str += ",'" + item.CustomerGroupID.ToString() + "'";
                    var childN = _dt.Where(n => n.ParentID == item.CustomerGroupID.ToString());
                    if (childN.Any())
                    {
                        str += FindChildByCusGroupParent(item.CustomerGroupID.ToString(), _dt);
                    }
                }
            }

            return str;
        }
        public int CountParent()
        {
            var sb = new StringBuilder();

            sb.AppendLine("SELECT COUNT(*) AS COUNT FROM tblCustomerGroup WHERE Inactive = 'False' AND ParentId = '0' ");

            var lst = ExcuteSQL.GetDataSet(sb.ToString(), false);


            var count = Convert.ToInt32(lst.Tables[0].Rows[0][0].ToString());


            return count;
        }
    }
}
