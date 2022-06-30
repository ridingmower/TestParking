using Kztek.Data.Infrastructure;
using Kztek.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Data.Repository
{
    public class BM_Building_ServiceRepository : RepositoryBase<BM_Building_Service>, IBM_Building_ServiceRepository
    {
        public BM_Building_ServiceRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }

    public interface IBM_Building_ServiceRepository : IRepository<BM_Building_Service>
    {
    }
}
