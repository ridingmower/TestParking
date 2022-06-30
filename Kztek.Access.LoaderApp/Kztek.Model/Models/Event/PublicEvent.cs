using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models.Event
{
    public class PublicEvent
    {
        [Key]
        public Guid Id { get; set; }       
        public string EventID { get; set; }
       
    }
}
