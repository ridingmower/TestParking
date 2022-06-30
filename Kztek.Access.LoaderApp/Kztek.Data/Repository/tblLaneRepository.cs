using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	public class tblLaneRepository : RepositoryBase<tblLane>, ItblLaneRepository
	{
		public tblLaneRepository(IDatabaseFactory databaseFactory): base(databaseFactory)
		{
		}
	}
	public interface ItblLaneRepository : IRepository<tblLane>
	{
	}
}
