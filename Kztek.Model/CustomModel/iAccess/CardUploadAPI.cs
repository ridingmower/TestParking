using Kztek.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Model.CustomModel.iAccess
{
    public class CardUploadAPI
    {
        public SelectListModelCardUpload ListFilter { get; set; }

        public List<SelfHostConfig> ListSelfHost { get; set; }

        public List<tblAccessController> ListController { get; set; }

        public List<string> ListCardId { get; set; }

        public List<string> ListCustomerId { get; set; }

        public User CurrentUser { get; set; }
    }
}
