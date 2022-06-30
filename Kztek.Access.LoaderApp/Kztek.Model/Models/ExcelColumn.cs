using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class ExcelColumn
    {
        [Key]
        public string Id { get; set; }
        public string MenuFunctionId { get; set; }
        public string ColName { get; set; }
        public string ColValue { get; set; }
        public bool Active { get; set; }
    }

    public class ExcelColumnCustom
    {
        public string Id { get; set; }
        public string MenuFunctionId { get; set; }
        public string MenuFunctionName { get; set; }
        public string ColName { get; set; }
        public string ColValue { get; set; }
        public bool Active { get; set; }
    }
}
