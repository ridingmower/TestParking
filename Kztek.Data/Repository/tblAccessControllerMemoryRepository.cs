using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	public class tblAccessControllerMemoryRepository : RepositoryBase<tblAccessControllerMemory>, ItblAccessControllerMemoryRepository
	{
		public tblAccessControllerMemoryRepository(IDatabaseFactory databaseFactory): base(databaseFactory)
		{
		}
	}
	public interface ItblAccessControllerMemoryRepository : IRepository<tblAccessControllerMemory>
	{
	}
}
