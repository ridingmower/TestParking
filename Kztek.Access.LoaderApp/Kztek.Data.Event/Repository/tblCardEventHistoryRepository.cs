using Kztek.Data.Event.Infrastructure;
using Kztek.Model.Models.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Data.Event.Repository
{
    public class tblCardEventHistoryRepository : RepositoryBase<tblCardEventHistory>, ItblCardEventHistoryRepository
    {
        public tblCardEventHistoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }
    public interface ItblCardEventHistoryRepository : IRepository<tblCardEventHistory>
    {

    }
}
