using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.Models.API
{
    public class API_Auth
    {
        [Key]
        public string Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mã token (Lấy từ hệ thống pmc)")]
        public string AccessToken { get; set; }
    }
}
