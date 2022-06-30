using Kztek.Data.Infrastructure;
using Kztek.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Data.Repository
{
    public class BM_FloorRepository : RepositoryBase<BM_Floor>, IBM_FloorRepository
    {
        public BM_FloorRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }

    public interface IBM_FloorRepository : IRepository<BM_Floor>
    {
    }
}
