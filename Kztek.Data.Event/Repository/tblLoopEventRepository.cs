using Kztek.Data.Event.Infrastructure;
using Kztek.Model.Models.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Data.Event.Repository
{
    public class tblLoopEventRepository : RepositoryBase<tblLoopEvent>, ItblLoopEventRepository
    {
        public tblLoopEventRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }
    public interface ItblLoopEventRepository : IRepository<tblLoopEvent>
    {

    }
}
