using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models.API
{
    public class tblCardSubmitEvent
    {
        public string Id { get; set; }
        public string CardId { get; set; }
        public string CardNo { get; set; }
        public string CardNumberBefore { get; set; }
        public string CardNumberSave { get; set; }
        public string CardGroupId { get; set; }
        public string Plate { get; set; }
        public string VehicleName { get; set; }
        public DateTime? DateExpired { get; set; }
        public DateTime? DateRegisted { get; set; }
        public DateTime DateCreated { get; set; }
        public string SubmitMessage { get; set; }
        public int SubmitStatus { get; set; }
        public string HTTP { get; set; }

    }
}
