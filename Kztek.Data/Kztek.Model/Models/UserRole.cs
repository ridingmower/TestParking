using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class UserRole
    {
        [Key]
        public string Id { get; set; }

        [StringLength(150)]
        [Column(TypeName = "varchar")]
        public string UserId { get; set; }

        [StringLength(150)]
        [Column(TypeName = "varchar")]
        public string RoleId { get; set; }
    }
}
