using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class BM_ResidentGroup
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string ParentId { get; set; }
        public int Ordering { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class BM_ResidentGroupSubmit
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ParentId { get; set; }
        public int Ordering { get; set; }
        public bool IsDeleted { get; set; }
    }
}

