using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class tblExitButtonEvent
    {
        [Key]
        public string ID { get; set; }

        public DateTime CreatedDate { get; set; }

        public string ControllerID { get; set; }

        public string UserID { get; set; }

    }
    public class tblExitButtonEvent_Custom
    {

        public Int32 RowNumber { get; set; }

        public DateTime CreatedDate { get; set; }

        public string ControllerID { get; set; }

    }
    public class tblExitButtonEvent_Excel
    {

        public Int32 RowNumber { get; set; }

        public string CreatedDate { get; set; }

        public string ControllerName { get; set; }

    }
}
