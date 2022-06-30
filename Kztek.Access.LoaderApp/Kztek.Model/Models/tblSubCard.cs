using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class tblSubCard
    {
        [Key]
        public int ID { get; set; }

        public string MainCard { get; set; }

        public string CardNo { get; set; }

        public string CardNumber { get; set; }

        public bool IsDelete { get; set; }
    }
}
