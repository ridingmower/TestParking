using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class SelfHostConfig
    {
        [Key]
        public string Id { get; set; }

        public string Hostname { get; set; }

        public string Address { get; set; }

        public string PCID { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
