using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models.Event
{
    public class tblCardEventDelete
    {
        [Key]
        public string Id { get; set; }
        public string EventId { get; set; }
        public string UserId { get; set; }
        public DateTime DateCreated { get; set; }
    }
    public class tblCardEventDeleteCustom
    {

        public string Id { get; set; }
        public string EventId { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public DateTime DateCreated { get; set; }
    }
    public class TotalEventDelete
    {
        public int TotalCount { get; set; }
        public string TotalMoney { get; set; }
    }
}
