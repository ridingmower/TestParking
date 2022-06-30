using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class tblUserJoinRole
    {
        [Key]
        public System.Guid UserJoinRoleID { get; set; }

        public string UserID { get; set; }

        public string RoleID { get; set; }
    }
}
