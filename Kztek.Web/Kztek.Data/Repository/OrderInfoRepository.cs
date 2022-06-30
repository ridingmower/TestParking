using Kztek.Data.Infrastructure;
using Kztek.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Data.Repository
{
    public class OrderInfoRepository : RepositoryBase<OrderInfo>, IOrderInfoRepository
    {
        public OrderInfoRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }
    public interface IOrderInfoRepository : IRepository<OrderInfo>
    {
    }
}
