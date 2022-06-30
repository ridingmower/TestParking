using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class tblFee
    {
        [Key]
        public int FeeID { get; set; }

        public string FeeName { get; set; }

        public string CardGroupID { get; set; }

        public int FeeLevel { get; set; }

        public string Units { get; set; }

        public bool IsUseExtend { get; set; }
    }

    public class FeeCustom
    {
        public string _id { get; set; }

        public string FeeName { get; set; }

        public string CardGroupName { get; set; }
        public string CardGroupID { get; set; }

        public int FeeLevel { get; set; }

        public string Unit { get; set; }

        public string Period { get; set; }

        public bool IsUseExtend { get; set; }
    }
}
