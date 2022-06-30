using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class BM_Floor
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string BuildingId { get; set; }    
        public int Ordering { get; set; }
        public DateTime DateCreated { get; set; }   
    }
}
