using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.CustomModel.iParking
{
    public class tblCardViettel
    {
        public int statusCode { get; set; }

        public string message { get; set; }

        public string keyMessage { get; set; }

        public string currentTime { get; set; }

        public List<tblCardViettel_Data> results { get; set; }

        public bool success { get; set; }
    }

    public class tblCardViettel_Data
    {
        public int id { get; set; }

        public int parkingId { get; set; }

        public string cardGroup { get; set; }

        public string cardNo { get; set; }

        public string cardNumber { get; set; }

        public int cardGroupType { get; set; }

        public string numberPlate { get; set; }

        public string numberPlateName { get; set; }

        public string userCode { get; set; }

        public string userName { get; set; }

        public string groupUser { get; set; }

        public string cmnd { get; set; }

        public string phoneNumber { get; set; }

        public string address { get; set; }

        public int status { get; set; }

        public string startDate { get; set; }

        public string endDate { get; set; }

        public string updateDate { get; set; }

        public string type { get; set; }
    }
}
