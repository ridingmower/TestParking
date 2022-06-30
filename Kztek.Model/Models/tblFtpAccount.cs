using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class tblFtpAccount
    {
        [Key]
        public string Id { get; set; }

        public string FtpHost { get; set; }

        public string FtpUser { get; set; }

        public string FtpPass { get; set; }
    }
}
