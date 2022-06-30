using Kztek.Data.Infrastructure;
using Kztek.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Data.Repository
{
    public class tblLockerSelfHostRepository : RepositoryBase<tblLockerSelfHost>, ItblLockerSelfHostRepository
    {
        public tblLockerSelfHostRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
    public interface ItblLockerSelfHostRepository : IRepository<tblLockerSelfHost>
    {
    }
}
