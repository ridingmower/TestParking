using Kztek.Data.Event.Infrastructure;
using Kztek.Model.Models.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Data.Event.Repository
{
    public class PublicEventRepository : RepositoryBase<PublicEvent>, IPublicEventRepository
    {
        public PublicEventRepository(IDatabaseFactory databaseFactory)
           : base(databaseFactory)
        {

        }
    }
    public interface IPublicEventRepository : IRepository<PublicEvent>
    {

    }
}
