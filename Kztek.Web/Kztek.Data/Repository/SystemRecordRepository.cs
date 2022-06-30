using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	public class SystemRecordRepository : RepositoryBase<SystemRecord>, ISystemRecordRepository
	{
		public SystemRecordRepository(IDatabaseFactory databaseFactory): base(databaseFactory)
		{
		}
	}
	public interface ISystemRecordRepository : IRepository<SystemRecord>
	{
	}
}
