using Kztek.Data.Infrastructure;
using Kztek.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Data.Repository
{
    public class BM_ResidentGroupRepository : RepositoryBase<BM_ResidentGroup>, IBM_ResidentGroupRepository
    {
        public BM_ResidentGroupRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }

    public interface IBM_ResidentGroupRepository : IRepository<BM_ResidentGroup>
    {
    }
}
