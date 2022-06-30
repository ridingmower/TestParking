using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class SystemRecord
    {
        public string Id { get; set; }

        public string Filename { get; set; }

        public string Description { get; set; }

        public string ComputerName { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
