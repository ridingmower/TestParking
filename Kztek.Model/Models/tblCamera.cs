using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models
{
    public class tblCamera
    {
        [Key]
        public System.Guid CameraID { get; set; }

        public string CameraCode { get; set; }

        public string CameraName { get; set; }

        public string HttpURL { get; set; }

        public string HttpPort { get; set; }

        public string RtspPort { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public Nullable<int> FrameRate { get; set; }

        public string Resolution { get; set; }

        public Nullable<int> Channel { get; set; }

        public string CameraType { get; set; }

        public string StreamType { get; set; }

        public string SDK { get; set; }

        public string Cgi { get; set; }

        public bool EnableRecording { get; set; }

        public string PCID { get; set; }

        public Nullable<int> PositionIndex { get; set; }

        public bool Inactive { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SortOrder { get; set; }
        public string ResizeImage { get; set; }//left,top,width,height
    }
}
