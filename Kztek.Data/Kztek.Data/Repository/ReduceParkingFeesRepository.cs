using Kztek.Data.Infrastructure;
using Kztek.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Data.Repository
{
    
    public class ReduceParkingFeesRepository : RepositoryBase<tblDiscountParking>, IReduceParkingFeesRepository
    {

        public ReduceParkingFeesRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }

    public interface IReduceParkingFeesRepository : IRepository<tblDiscountParking>
    {
    }
}
