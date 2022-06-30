using Kztek.Data.Event.Infrastructure;
using Kztek.Model.Models.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Data.Event.Repository
{
    public class tblDispenserRepository : RepositoryBase<tblDispenser>, ItblDispenserRepository
    {
        public tblDispenserRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {

        }
    }
    public interface ItblDispenserRepository : IRepository<tblDispenser>
    {

    }
}
