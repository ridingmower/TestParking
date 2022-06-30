using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	public class tblLogRepository : RepositoryBase<tblLog>, ItblLogRepository
	{
		public tblLogRepository(IDatabaseFactory databaseFactory): base(databaseFactory)
		{
		}
	}
	public interface ItblLogRepository : IRepository<tblLog>
	{
	}
}
