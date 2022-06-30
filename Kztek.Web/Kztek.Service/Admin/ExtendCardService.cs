using Kztek.Data.Infrastructure;
using Kztek.Data.Repository;
using Kztek.Data.SqlHelper;
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
    public interface IExtendCardService
    {
        IEnumerable<ExtendCard> GetAll();
        ExtendCard GetById(string id);

        MessageReport Create(ExtendCard obj);

        MessageReport Update(ExtendCard obj);

        MessageReport DeleteById(string id);

        bool AddNew(string listCardNumber, int _feelevel, string _oldexpire, string _newexpire, string datecreated, string userId, bool chbEnableMinusActive, string subid = "", string Id = "", string dateextend = "");

        List<tblCard> GetListCard(string KeyWord, string AnotherKey, string CardGroupIDs, string CustomerID, string CustomerGroupID, string _newexpire, bool chbEnableMinusActive);
    }
    public class ExtendCardService : IExtendCardService
    {
        private IExtendCardRepository _ExtendCardRepository;
        private IUnitOfWork _UnitOfWork;
        private IUser_AuthGroupService _User_AuthGroupService;
        private string AuthCardGroupIds = "";
        public ExtendCardService(IExtendCardRepository _ExtendCardRepository, IUser_AuthGroupService _User_AuthGroupService, IUnitOfWork _UnitOfWork)
        {
            this._ExtendCardRepository = _ExtendCardRepository;
            this._UnitOfWork = _UnitOfWork;
            AuthCardGroupIds = _User_AuthGroupService.GetAuthCardGroupIds();
        }

        private void Save()
        {
            _UnitOfWork.Commit();
        }

        public MessageReport Create(ExtendCard obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _ExtendCardRepository.Add(obj);

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
                    _ExtendCardRepository.Delete(obj);

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

        public ExtendCard GetById(string id)
        {
            return _ExtendCardRepository.GetById(id);
        }


        public MessageReport Update(ExtendCard obj)
        {
            var re = new MessageReport();
            re.Message = "Error";
            re.isSuccess = false;

            try
            {
                _ExtendCardRepository.Update(obj);

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

        public IEnumerable<ExtendCard> GetAll()
        {
            var query = from n in _ExtendCardRepository.Table
                        select n;

            return query;
        }

        public bool AddNew(string listCardNumber, int _feelevel,string _oldexpire, string _newexpire,string datecreated, string userId, bool chbEnableMinusActive, string subid = "",string Id = "",string dateextend = "")
        {
            if(!string.IsNullOrEmpty(_oldexpire))
                _oldexpire = Convert.ToDateTime(_oldexpire).ToString("MM/dd/yyyy");

            if (!string.IsNullOrEmpty(_newexpire))
                _newexpire = Convert.ToDateTime(_newexpire).ToString("MM/dd/yyyy");

            if (!string.IsNullOrEmpty(datecreated))
                datecreated = Convert.ToDateTime(datecreated).ToString("MM/dd/yyyy");

         

            var sb = new StringBuilder();
            sb.AppendLine("INSERT INTO ExtendCard(Id,Code,DateCreated,[Date], CardNumber, CardNo, Plate, OldExpireDate, [Days], NewExpireDate, CardGroupID, CustomerGroupID, UserID, FeeLevel, CustomerID,IsDelete,IsTransferPayment,SubId)");
            sb.AppendLine(string.Format("SELECT '{1}', CASE WHEN cus.CustomerCode IS NULL THEN '0' ELSE cus.CustomerCode END,GetDate(),'{0}', ca.Cardnumber,ca.CardNo", datecreated, Id));
            sb.AppendLine(", CAST(CASE WHEN ca.Plate2 <> '' THEN ISNULL(ca.Plate1,'') + ';' + ISNULL(ca.Plate2,'') WHEN ca.Plate3 <> '' THEN ISNULL(ca.Plate1,'') + ';' + ISNULL(ca.Plate2,'') + ';' + ISNULL(ca.Plate3,'') WHEN ca.Plate1 IS NULL THEN '' ELSE ca.Plate1 END AS nvarchar(50)) as Plate");
            sb.AppendLine(string.Format(", {1}, DATEDIFF(DAY, ca.[ExpireDate], '{0}')", _newexpire, _oldexpire));
            sb.AppendLine(string.Format(", '{0}', ca.CardGroupID, CASE WHEN  cus.CustomerGroupID IS NULL THEN '0' ELSE cus.CustomerGroupID END,'{2}','{1}', CASE WHEN ca.CustomerID IS NULL THEN '0' ELSE ca.CustomerID END,0,0,'{3}'", _newexpire, _feelevel, userId, subid));
            sb.AppendLine("from tblCard ca");
            sb.AppendLine("LEFT join tblCustomer cus on ca.CustomerID = CONVERT(varchar(255), cus.CustomerID)");
            sb.AppendLine("where ca.IsDelete = 0 and ca.IsLock=0");
            //Neu so ngay gia han <0 va neu ko check thi ko cho gia han
            if (chbEnableMinusActive == false)
            {
                sb.AppendLine(string.Format("and DATEDIFF(DAY, ca.[ExpireDate], '{0}') >=0  AND ca.[ExpireDate] <= '{0}'", dateextend));
            }

            if (!string.IsNullOrWhiteSpace(listCardNumber))
            {
                //where in
                sb.AppendLine(string.Format(" and ca.CardNumber IN ({0})", listCardNumber));
            }

            return ExcuteSQL.Execute(sb.ToString());
        }

        public List<tblCard> GetListCard(string KeyWord, string AnotherKey, string CardGroupIDs, string CustomerID, string CustomerGroupID, string _newexpire, bool chbEnableMinusActive)
        {
            var sb = new StringBuilder();
      
            sb.AppendLine("SELECT ca.Cardnumber");

            sb.AppendLine("from tblCard ca");

            sb.AppendLine("LEFT JOIN dbo.tblCardGroup g ON ca.CardGroupID=g.CardGroupID");
            sb.AppendLine("LEFT join tblCustomer cus on ca.CustomerID = CONVERT(varchar(255), cus.CustomerID)");

            sb.AppendLine("where ca.IsDelete = 0 and ca.IsLock=0 and g.CardType=0");

            //Neu so ngay gia han <0 va neu ko check thi ko cho gia han
            if (chbEnableMinusActive == false)
            {
                sb.AppendLine(string.Format("and DATEDIFF(DAY, ca.[ExpireDate], '{0}') >=0  AND ca.[ExpireDate] <= '{0}'", _newexpire));
            }

            //Update theo filler
            if (!string.IsNullOrWhiteSpace(KeyWord))
            {
                sb.AppendLine(" and (ca.CardNo LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.CardNumber LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or cus.CustomerCode LIKE '%" + KeyWord + "%'");
                sb.AppendLine(" or cus.CustomerName LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or cus.Address LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or cus.AddressUnsign LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.Plate1 LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.Plate2 LIKE N'%" + KeyWord + "%'");
                sb.AppendLine(" or ca.Plate3 LIKE N'%" + KeyWord + "%')");
            }

            if (!string.IsNullOrWhiteSpace(AnotherKey))
            {
                sb.AppendLine(" and (ca.CardNo LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.CardNumber LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or cus.CustomerCode LIKE '%" + AnotherKey + "%'");
                sb.AppendLine(" or cus.CustomerName LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or cus.Address LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or cus.AddressUnsign LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.Plate1 LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.Plate2 LIKE N'%" + AnotherKey + "%'");
                sb.AppendLine(" or ca.Plate3 LIKE N'%" + AnotherKey + "%')");
            }

            if (!string.IsNullOrWhiteSpace(CardGroupIDs))
            {
                sb.AppendLine(" and ca.CardGroupID = '" + CardGroupIDs + "'");
            }
            if (!string.IsNullOrWhiteSpace(AuthCardGroupIds))
            {
                var t = AuthCardGroupIds.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (t.Any())
                {
                    var count = 0;

                    sb.AppendLine("AND ca.CardGroupID IN ( ");

                    foreach (var item in t)
                    {
                        count++;

                        sb.AppendLine(string.Format("'{0}'{1}", item, count == t.Length ? "" : ","));
                    }

                    sb.AppendLine(" )");
                }
            }
            if (!string.IsNullOrWhiteSpace(CustomerID))
            {
                sb.AppendLine(" and ca.CustomerID = '" + CustomerID + "'");
            }
            if (!string.IsNullOrWhiteSpace(CustomerGroupID))
            {
                sb.AppendLine(" and cus.CustomerGroupID IN ('" + CustomerGroupID + "')");
            }

            var list = ExcuteSQL.GetDataSet(sb.ToString(), false);
        
            return ExcuteSQL.ConvertTo<tblCard>(list.Tables[0]);
        }
    }
}
