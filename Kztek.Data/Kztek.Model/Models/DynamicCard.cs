using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class DynamicCard
    {
        [Key]
        public string Id { get; set; }

        public string PCID { get; set; }

        public string ControllerID { get; set; }

        public int Button { get; set; }
        public string CardGroupID { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
