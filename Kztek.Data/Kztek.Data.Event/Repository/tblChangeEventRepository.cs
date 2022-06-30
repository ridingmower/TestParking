using Kztek.Data.Event.Infrastructure;
using Kztek.Model.Models.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Data.Event.Repository
{
    public class tblChangeEventRepository : RepositoryBase<tblChangeEvent>, ItblChangeEventRepository
    {
        public tblChangeEventRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }
    public interface ItblChangeEventRepository : IRepository<tblChangeEvent>
    {

    }
}
