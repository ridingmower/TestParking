using Kztek.Data.Infrastructure;
using Kztek.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Data.Repository
{
    public class BM_ResidentRepository : RepositoryBase<BM_Resident>, IBM_ResidentRepository
    {
        public BM_ResidentRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }

    public interface IBM_ResidentRepository : IRepository<BM_Resident>
    {
    }
}
