using Kztek.Data.Infrastructure;
using Kztek.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Data.Repository
{
   
    
    public class MultipleLanesMapRepository : RepositoryBase<MultipleLanesMap>, IMultipleLanesMapRepository
    {
        public MultipleLanesMapRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }

    public interface IMultipleLanesMapRepository : IRepository<MultipleLanesMap>
    {
    }
}
