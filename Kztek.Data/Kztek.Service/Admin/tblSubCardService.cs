using Kztek.Data.Infrastructure;
using Kztek.Data.Repository;
using Kztek.Data.SqlHelper;
using Kztek.Model.CustomModel;
using Kztek.Model.Models;
using Kztek.Web.Core.Functions;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Service.Admin
{
    public interface ItblSubCardService
    {
        IEnumerable<tblSubCard> GetAll();

        tblSubCard GetById(int id);

        MessageReport Create(tblSubCard obj);

        MessageReport Update(tblSubCard obj);

        MessageReport DeleteById(int id, ref tblSubCard obj);
        IPagedList<tblSubCard> GetAllPagingByFirst(string key, int pageNumber, int pageSize);
        tblSubCard GetByCard(string maincard = "", string cardnumber = "");
        List<SelectListModelAutocomplete> AutoComplete(string key = "");
        DataTable Excel(string KeyWord);
    }
    public class tblSubCardService : ItblSubCardService
    {
        private ItblSubCardRepository _tblSubCardRepository;
        private ItblCardRepository _tblCardRepository;
        private IUnitOfWork _UnitOfWork;

        public tblSubCardService(ItblSubCardRepository _tblSubCardRepository, ItblCardRepository _tblCardRepository, IUnitOfWork _UnitOfWork)
        {
            this._tblSubCardRepository = _tblSubCardRepository;
            this._tblCardRepository = _tblCardRepository;
            this._UnitOfWork = _UnitOfWork;
        }

        private void Save()
        {
            _UnitOfWork.Commit();
        }
        public IPagedList<tblSubCard> GetAllPagingByFirst(string key, int pageNumber, int pageSize)
        {
            var query = from n in _tblSubCardRepository.Table
                        where !n.IsDelete
                        select n;

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(n => n.MainCard.Contains(key) || n.CardNumber.Contains(key) || n.CardNo.Contains(key));
            }


            var list = new PagedList<tblSubCard>(query.OrderBy(n => n.MainCard), pageNumber, pageSize);

            return list;
        }
        public MessageReport Create(tblSubCard obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblSubCardRepository.Add(obj);

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

        public MessageReport DeleteById(int id, ref tblSubCard obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                obj = GetById(id);
                if (obj != null)
                {
                    obj.IsDelete = true;

                    _tblSubCardRepository.Update(obj);

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

        public IEnumerable<tblSubCard> GetAll()
        {
            var query = from n in _tblSubCardRepository.Table
                        select n;

            return query;
        }


        public tblSubCard GetById(int id)
        {
            return _tblSubCardRepository.GetById(id);
        }

        public MessageReport Update(tblSubCard obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _tblSubCardRepository.Update(obj);

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

        public tblSubCard GetByCard(string maincard = "",string cardnumber = "")
        {
            var query = from n in _tblSubCardRepository.Table
                        where !n.IsDelete
                        select n;

            if (!string.IsNullOrEmpty(maincard))
            {
                query = query.Where(n => n.MainCard == maincard);
            }

            if (!string.IsNullOrEmpty(cardnumber))
            {
                query = query.Where(n => n.CardNumber == cardnumber);
            }

            return query.FirstOrDefault();
        }

        public List<SelectListModelAutocomplete> AutoComplete(string key = "")
        {
            var cus = new List<SelectListModelAutocomplete>();

            var query = new StringBuilder();

            query.AppendLine("select top 10 CardNumber, CardNo from tblCard");

            query.AppendLine("where IsDelete = 'False'");

            if (!string.IsNullOrEmpty(key))
            {
                query.AppendLine(string.Format("AND (CardNumber like N'%{0}%' OR CardNo LIKE N'%{0}%')", key));
            }

            query.AppendLine("order by CardNumber,CardNo");

            var data = ExcuteSQL.GetDataSet(query.ToString(), false);

            var list = ExcuteSQL.ConvertTo<tblCard>(data.Tables[0]);

            foreach (var item in list)
            {
                var t = new SelectListModelAutocomplete();

                t.id = item.CardNumber;
                t.name = item.CardNumber;
                t.value = item.CardNumber;

                cus.Add(t);
            }

            return cus;
        }

        public DataTable Excel(string KeyWord)
        {

            var query = new StringBuilder();
            var dtrtn = new DataTable();
            try
            {
                //query.AppendLine("SELECT * FROM(");
                query.AppendLine(string.Format("select ROW_NUMBER() OVER(ORDER BY MainCard asc) AS STT,"));
                query.AppendLine("a.MainCard as 'Mã thẻ chính', a.[CardNumber] AS 'Mã thẻ',a.[CardNo]");
                query.AppendLine("FROM(");

                query.AppendLine("SELECT *");
                query.AppendLine("FROM dbo.[tblSubCard] e WITH (NOLOCK)");
                query.AppendLine("WHERE e.[IsDelete] = 0");
              
                if (!string.IsNullOrWhiteSpace(KeyWord))
                    query.AppendLine(string.Format("AND (e.[CardNumber] LIKE '%{0}%' OR e.[CardNo] LIKE '%{0}%' OR e.[MainCard] LIKE '%{0}%')", KeyWord));
            
                query.AppendLine(") as a");

                dtrtn = ExcuteSQL.GetDataSet(query.ToString(), false).Tables[0];

            }
            catch (Exception ex)
            {
            }

            return dtrtn;
        }
    }
}
