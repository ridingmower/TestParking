using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
 public   class MultipleLanesMap
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string PCid { get; set; }
        public int ViewOrder { get; set; }
        public int SideIndex { get; set; }
        public int CurrentDirection { get; set; }
    }

    public class MultipleLanesMap_Cus
    {
       
        public string Id { get; set; }
        public string Name { get; set; }
        public string PCid { get; set; }
        public string ViewOrder { get; set; }
        public string SideIndex { get; set; }
        public string CurrentDirection { get; set; }
    }
}
