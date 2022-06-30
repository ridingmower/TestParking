using Kztek.Data.Infrastructure;
using Kztek.Data.Repository;
using Kztek.Data.SqlHelper;
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
    public interface IDynamicCardService
    {
        IEnumerable<DynamicCard> GetAll();


        IEnumerable<DynamicCard> GetAllPagingByFirst(string key, string pc, int pageNumber, int pageSize, ref int totalPage, ref int totalItem);

        List<DynamicCardCustomViewModel> GetPaging(string key, string controller, string pc, string cardgroup, int pageNumber, int pageSize, ref int total);

        DynamicCard GetById(string id);

        DynamicCard CheckExist(string pc, string controller, int button);

        MessageReport Create(DynamicCard obj);

        MessageReport Update(DynamicCard obj);

        MessageReport DeleteById(string id, ref DynamicCard obj);
    }

    public class DynamicCardService : IDynamicCardService
    {
        private IDynamicCardRepository _DynamicCardRepository;      
        private IUnitOfWork _UnitOfWork;

        public DynamicCardService(IDynamicCardRepository _DynamicCardRepository, IUnitOfWork _UnitOfWork)
        {
            this._DynamicCardRepository = _DynamicCardRepository;
        
            this._UnitOfWork = _UnitOfWork;
        }

        private void Save()
        {
            _UnitOfWork.Commit();
        }

        public MessageReport Create(DynamicCard obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _DynamicCardRepository.Add(obj);

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

        public MessageReport DeleteById(string id, ref DynamicCard obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                obj = GetById(id);
                if (obj != null)
                {
                    _DynamicCardRepository.Delete(n => n.Id == id);

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

        public IEnumerable<DynamicCard> GetAll()
        {
            var query = from n in _DynamicCardRepository.Table
                        select n;

            return query;
        }


        public List<DynamicCardCustomViewModel> GetPaging(string key,string controller, string pc,string cardgroup, int pageNumber, int pageSize, ref int total)
        {
            var query = new StringBuilder();
           
            query.AppendLine("SELECT * FROM(");
            query.AppendLine("SELECT ROW_NUMBER() OVER(ORDER BY a.[DateCreated] desc) as RowNumber,a.*");
            query.AppendLine("FROM(");

            query.AppendLine("Select c.ControllerID, c.ControllerName,p.PCID,p.ComputerName,cg.CardGroupID,cg.CardGroupName,d.Id,d.Button,d.DateCreated from DynamicCard d");
            query.AppendLine("Left join tblController c ON d.ControllerID = Convert(varchar(50),c.ControllerID)");
            query.AppendLine("Left join tblPC p ON d.PCID = Convert(varchar(50),p.PCID)");
            query.AppendLine("Left join tblCardGroup cg ON d.CardGroupID = Convert(varchar(50),cg.CardGroupID)");

            query.AppendLine("where 1=1");

            //Nhom the
            if (!string.IsNullOrWhiteSpace(cardgroup))
            {
                var t = cardgroup.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and d.CardGroupID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            //bđk
            if (!string.IsNullOrWhiteSpace(controller))
            {
                var t = controller.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and d.ControllerID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            //pc
            if (!string.IsNullOrWhiteSpace(pc))
            {
                var t = pc.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and d.PCID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            if (!string.IsNullOrEmpty(key))
                query.AppendLine(string.Format("and d.Button = {0}", key));

            query.AppendLine(") as a");
            query.AppendLine(") as C1");
            query.AppendLine(string.Format("WHERE RowNumber BETWEEN (({0}-1) * {1} + 1) AND ({0} * {1})", pageNumber, pageSize));


            query.AppendLine("SELECT COUNT(*) as totalCount");
            query.AppendLine("FROM(");

            query.AppendLine("Select c.ControllerID, c.ControllerName,p.PCID,p.ComputerName,cg.CardGroupID,cg.CardGroupName,d.Id,d.Button,d.DateCreated from DynamicCard d");
            query.AppendLine("Left join tblController c ON d.ControllerID = Convert(varchar(50),c.ControllerID)");
            query.AppendLine("Left join tblPC p ON d.PCID = Convert(varchar(50),p.PCID)");
            query.AppendLine("Left join tblCardGroup cg ON d.CardGroupID = Convert(varchar(50),cg.CardGroupID)");

            query.AppendLine("where 1=1");

            //Nhom the
            if (!string.IsNullOrWhiteSpace(cardgroup))
            {
                var t = cardgroup.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and d.CardGroupID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            //bđk
            if (!string.IsNullOrWhiteSpace(controller))
            {
                var t = controller.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and d.ControllerID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            //pc
            if (!string.IsNullOrWhiteSpace(pc))
            {
                var t = pc.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    query.AppendLine("and d.PCID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        query.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    query.AppendLine(" )");
                }
            }

            if (!string.IsNullOrEmpty(key))
                query.AppendLine(string.Format("and d.Button = {0}", key));

            query.AppendLine(") as a");

            var list = ExcuteSQL.GetDataSet(query.ToString(), false);
            total = list.Tables.Count > 1 ? Convert.ToInt32(list.Tables[1].Rows[0]["totalCount"].ToString()) : 0;
            return ExcuteSQL.ConvertTo<DynamicCardCustomViewModel>(list.Tables[0]);
        }

        public IEnumerable<DynamicCard> GetAllPagingByFirst(string key, string pc, int pageNumber, int pageSize, ref int totalPage, ref int totalItem)
        {
            var query = from n in _DynamicCardRepository.Table
                        select n;

            //if (!string.IsNullOrWhiteSpace(key))
            //{
            //    query = query.Where(n => n.CameraName.Contains(key));
            //}

            if (!string.IsNullOrWhiteSpace(pc))
            {
                query = query.Where(n => n.PCID == pc);
            }

            var list = new PagedList<DynamicCard>(query.OrderByDescending(n => n.DateCreated), pageNumber, pageSize);

            return list;
        }

        public DynamicCard GetById(string id)
        {
            return _DynamicCardRepository.GetById(id);
        }

        public MessageReport Update(DynamicCard obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _DynamicCardRepository.Update(obj);

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

        public DynamicCard CheckExist(string pc,string controller,int button)
        {
            var query = from n in _DynamicCardRepository.Table
                        where n.PCID == pc && n.ControllerID == controller && n.Button == button
                        select n;

            return query.FirstOrDefault();
        }
    }
}
