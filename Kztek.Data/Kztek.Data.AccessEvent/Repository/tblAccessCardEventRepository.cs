using Kztek.Data.AccessEvent.Infrastructure;
using Kztek.Model.Models.AccessEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Data.AccessEvent.Repository
{
    public class tblAccessCardEventRepository : RepositoryBase<tblAccessCardEvent>, ItblAccessCardEventRepository
    {
        public tblAccessCardEventRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }
    public interface ItblAccessCardEventRepository : IRepository<tblAccessCardEvent>
    {

    }
}
