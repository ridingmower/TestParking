using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class tblLane
    {
        [Key]
        public System.Guid LaneID { get; set; }

        public string LaneCode { get; set; }

        public string LaneName { get; set; }

        public string PCID { get; set; }

        public Nullable<int> LaneType { get; set; }

        public bool IsLoop { get; set; }

        public int CheckPlateLevelIn { get; set; }

        public int CheckPlateLevelOut { get; set; } //option 1

        public int CheckPlateOutOption2 { get; set; } //option 2
        public int OpenBarrieIn { get; set; } //option 1
        public int OpenBarrieOption1 { get; set; } //option 1

        public int OpenBarrieOption2 { get; set; } //option 2

        public bool IsPrint { get; set; }

        public string C1 { get; set; }

        public string C2 { get; set; }

        public string C3 { get; set; }

        public string C4 { get; set; }

        public string C5 { get; set; }

        public string C6 { get; set; }

        public string Controller { get; set; }

        public bool Inactive { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SortOrder { get; set; }

        public bool IsLED { get; set; }

        public bool IsFree { get; set; }

        public bool AccessForEachSide { get; set; }
        public string VehicleTypeLeft { get; set; }
        public string CardTypeLeft { get; set; }
        public string VehicleTypeRight { get; set; }
        public string CardTypeRight { get; set; }
        public string CardGroupId { get; set; }//nhóm thẻ tính tiền khi tích loop
    }

    public class tblLaneAPI
    {
        public string LaneID { get; set; }

        public string LaneCode { get; set; }

        public string LaneName { get; set; }
    }
}
