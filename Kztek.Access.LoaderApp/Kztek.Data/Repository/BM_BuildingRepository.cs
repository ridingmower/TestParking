using Kztek.Data.Infrastructure;
using Kztek.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Data.Repository
{
    public class BM_BuildingRepository : RepositoryBase<BM_Building>, IBM_BuildingRepository
    {
        public BM_BuildingRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }

    public interface IBM_BuildingRepository : IRepository<BM_Building>
    {
    }
}
