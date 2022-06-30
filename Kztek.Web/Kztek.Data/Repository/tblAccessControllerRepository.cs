using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	public class tblAccessControllerRepository : RepositoryBase<tblAccessController>, ItblAccessControllerRepository
	{
		public tblAccessControllerRepository(IDatabaseFactory databaseFactory): base(databaseFactory)
		{
		}
	}
	public interface ItblAccessControllerRepository : IRepository<tblAccessController>
	{
	}
}
