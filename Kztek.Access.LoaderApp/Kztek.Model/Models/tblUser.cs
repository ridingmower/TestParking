using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class tblUser
    {
        [Key]
        public System.Guid UserID { get; set; }

        public string UserCode { get; set; }

        public string FullName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public Nullable<bool> IsLock { get; set; }

        public string Avatar { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SortOrder { get; set; }

        public bool IsSystem { get; set; }

        public string CardGroupIds { get; set; }

        public string CustomerGroupIds { get; set; }

        public string AccessControllerSelected { get; set; }
    }
}
