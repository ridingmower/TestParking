using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class tblCardProcess
    {
        [Key]
        public int Id { get; set; }

        public Nullable<System.DateTime> Date { get; set; }

        public string CardNumber { get; set; }

        public string Actions { get; set; }

        public string CardGroupID { get; set; }

        public string CustomerID { get; set; }

        public string UserID { get; set; }

        public string Description { get; set; }

        public string OldInfoCP { get; set; }

        public string Plates { get; set; }
    }
}
