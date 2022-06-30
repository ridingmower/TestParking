using Kztek.Data.Event.Infrastructure;
using Kztek.Model.Models.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Data.Event.Repository
{
    public class tblCardEventDeleteRepository : RepositoryBase<tblCardEventDelete>, ItblCardEventDeleteRepository
    {
        public tblCardEventDeleteRepository(IDatabaseFactory databaseFactory)
           : base(databaseFactory)
        {

        }
    }
    public interface ItblCardEventDeleteRepository : IRepository<tblCardEventDelete>
    {

    }
}
