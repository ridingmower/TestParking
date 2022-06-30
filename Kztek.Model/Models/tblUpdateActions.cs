using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class tblUpdateActions
    {
        [Key]
        public string Id { get; set; }

        public bool IsUpdateDoor { get; set; }

        public bool IsUpdateLevel { get; set; }

        public bool IsUpdateLine { get; set; }

        public bool IsUpdatePC { get; set; }

        public bool IsUpdateTimeZone { get; set; }

        public bool IsUpdateController { get; set; }

    }
}
