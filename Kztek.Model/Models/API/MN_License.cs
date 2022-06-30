using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models.API
{
    public class MN_License
    {
        public string Id { get; set; }

        public string ProjectName { get; set; }

        public bool IsExpire { get; set; }

        public string ExpireDate { get; set; }
    }
}
