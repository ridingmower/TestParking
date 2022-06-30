using Kztek.Data.Infrastructure;
using Kztek.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Data.Repository
{
    public class tblAccessCameraRepository : RepositoryBase<tblAccessCamera>, ItblAccessCameraRepository
    {
        public tblAccessCameraRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }

    public interface ItblAccessCameraRepository : IRepository<tblAccessCamera>
    {
    }
}
