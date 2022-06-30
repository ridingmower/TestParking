using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.CustomModel.Mobile
{
    public class API_Card
    {
        public string CardNo { get; set; }

        public string CardNumber { get; set; }

        public string CardNumber_Mix { get; set; }

        public string DateExpired { get; set; }

        public List<string> Plates { get; set; }
    }

    public class API_Card_Mobile
    {
        public string CardNumber { get; set; }

        public string DateExpired { get; set; }

        public string Plates { get; set; }
    }

    public class API_Card_Plate
    {
        public string Plate { get; set; }

        public string UnsignPlate { get; set; }
    }

    public class PMC_Customer_Company_Plate
    {
        public string AccessToken { get; set; }

        public string Plates { get; set; }
    }

    public class PMC_CustomerOrder
    {
        public string AccessToken { get; set; }

        public string CardNo { get; set; } //Số thẻ

        public string CardNumber { get; set; } //Mã thẻ

        public float Money { get; set; } //Tiền

        public int DayExtend { get; set; } //Số ngày thêm
    }

    public class MN_Card_Custom
    {
        public string Id { get; set; }
        public string CardNo { get; set; }
        public string CardNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerMobile { get; set; }
        public string CustomerAddress { get; set; }
        public string Avatar { get; set; }
        public string CustomerGroupId { get; set; }
        public string CustomerGroupName { get; set; }
        public string ServiceId { get; set; }
        public string UserCreatedId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
