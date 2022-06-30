using Kztek.Data.Repository;
using Kztek.Data.SqlHelper;
using Kztek.Model.CustomModel.Mobile;
using Kztek.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Service.Mobile
{
    public interface IAPI_CardService
    {
        List<API_Card> GetListActiveCardByPlates(List<API_Card_Plate> plates);

        List<API_Card> GetListActiveCardByPlates(string plates);

        API_Card GetByCardNumber(string cardnumber);
        MN_Card_Custom GetInfoByCardNumber(string cardnumber);
    }

    public class API_CardService : IAPI_CardService
    {
        private ItblCardRepository _tblCardRepository;
        public API_CardService(ItblCardRepository _tblCardRepository)
        {
            this._tblCardRepository = _tblCardRepository;
        }

        public API_Card GetByCardNumber(string cardnumber)
        {
            var model = new API_Card();

            var query = from n in _tblCardRepository.Table
                        where n.IsDelete == false && n.IsLock == false && n.CardNumber == cardnumber
                        select n;

            var t = query.FirstOrDefault();

            if (t != null)
            {
                model = FormatData(t);
            }

            return model;
        }

        public List<API_Card> GetListActiveCardByPlates(List<API_Card_Plate> plates)
        {
            var model = new List<API_Card>();

            var query = from n in _tblCardRepository.Table
                        where n.IsDelete == false && n.IsLock == false
                                && (plates.Any(m=>m.Plate == n.Plate1 || m.Plate == n.Plate2 || m.Plate == n.Plate3) || plates.Any(m => m.UnsignPlate == n.Plate1 || m.UnsignPlate == n.Plate2 || m.UnsignPlate == n.Plate3))
                        select n;

            var t = query.ToList();

            if (t.Any())
            {
                foreach (var item in t)
                {
                    var k = FormatData(item);

                    model.Add(k);
                }
            }

            return model;
        }

        public List<API_Card> GetListActiveCardByPlates(string plates)
        {
            var model = new List<API_Card>();

            var query = from n in _tblCardRepository.Table
                        where n.IsDelete == false && n.IsLock == false
                                && (plates.Contains(n.Plate1) || plates.Contains(n.Plate2) || plates.Contains(n.Plate3))
                        select n;

            var t = query.ToList();

            if (t.Any())
            {
                foreach (var item in t)
                {
                    var k = FormatData(item);

                    model.Add(k);
                }
            }

            return model;
        }

        public List<API_Card_Mobile> GetListActiveCardByPlates2(string plates)
        {
            throw new NotImplementedException();
        }

        private API_Card FormatData(tblCard model)
        {
            var t = new API_Card()
            {
                CardNo = model.CardNo,
                CardNumber = model.CardNumber,
                DateExpired = Convert.ToDateTime(model.ExpireDate).ToString("dd/MM/yyyy"),
                Plates = new List<string>()
            };

            if (!string.IsNullOrWhiteSpace(model.Plate1))
            {
                t.Plates.Add(model.Plate1);
            }

            if (!string.IsNullOrWhiteSpace(model.Plate2))
            {
                t.Plates.Add(model.Plate2);
            }

            if (!string.IsNullOrWhiteSpace(model.Plate3))
            {
                t.Plates.Add(model.Plate3);
            }

            var first = t.CardNumber.Substring(0, 2);
            var last = t.CardNumber.Substring(t.CardNumber.Length - 4, 3);
            t.CardNumber_Mix = first + "..." + last;

            return t;
        }

        public MN_Card_Custom GetInfoByCardNumber(string cardnumber)
        {
            var query = new StringBuilder();
            query.AppendLine("select c.CardNumber,c.CardNo,cu.CustomerName,cu.Mobile as CustomerMobile,cu.Address as CustomerAddress,cu.Avatar,cu.CustomerGroupID,cg.CustomerGroupName from tblCard c");
            query.AppendLine("left join tblCustomer cu on c.CustomerID =  CONVERT(varchar(255), cu.CustomerID)");
            query.AppendLine("left join tblCustomerGroup cg on cu.CustomerGroupID = CONVERT(varchar(255), cg.CustomerGroupID)");
            query.AppendLine(string.Format("where c.CardNumber = '{0}'", cardnumber));

            return SqlExQuery<MN_Card_Custom>.ExcuteQuery(query.ToString()).FirstOrDefault();
        }
    }
}
