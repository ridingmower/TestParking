using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class MenuFunctionConfig
    {
        [Key]
        public string Id { get; set; }

        public string MenuFunctionId { get; set; }
    }
}
