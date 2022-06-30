using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class RoleMenu
    {
        [Key]
        public string Id { get; set; }

        [StringLength(150)]
        [Column(TypeName = "varchar")]
        public string MenuId { get; set; }

        [StringLength(150)]
        [Column(TypeName = "varchar")]
        public string RoleId { get; set; }
    }
}
