using Kztek.Model.CustomModel.iAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Web.Core.Models
{
    public class AccessUploadMultiDevice
    {
        public string Address { get; set; }

        public List<Employee> DataSend { get; set; }

        public int pageIndex { get; set; }

        public int totalPage { get; set; }

        public int totalItem { get; set; }

        public int pageSize { get; set; }
        public int totalController { get; set; }
    }
}
