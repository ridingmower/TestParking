using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class tblSystemConfig
    {
        [Key]
        public System.Guid SystemConfigID { get; set; }

        public string Company { get; set; }

        public string Address { get; set; }

        public string Tel { get; set; }

        public string Fax { get; set; }

        public string FeeName { get; set; }

        public bool EnableDeleteCardFailed { get; set; }

        public string SystemCode { get; set; }

        public string KeyA { get; set; }

        public string KeyB { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SortOrder { get; set; }

        public bool EnableSoundAlarm { get; set; }

        public string Logo { get; set; }

        public bool EnableAlarmMessageBox { get; set; }

        public bool EnableAlarmMessageBoxIn { get; set; }

        public string Tax { get; set; }

        public int DelayTime { get; set; }

        public string Para1 { get; set; }

        public string Para2 { get; set; }
        public bool isAuthInView { get; set; }
        public bool IsDisplay { get; set; }

        public string CustomInfo { get; set; }
        public bool IsAutoCapture { get; set; }
        public bool isCompartment { get; set; }
        public string Background { get; set; }
        public bool IsSaveButton { get; set; } //lưu tất cả sự kiện nút ấn
        public bool IsHideEventOut { get; set; } //ẩn sự kiện ra
    }
}
