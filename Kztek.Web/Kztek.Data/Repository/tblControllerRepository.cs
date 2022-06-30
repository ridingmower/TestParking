using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	public class tblControllerRepository : RepositoryBase<tblController>, ItblControllerRepository
	{
		public tblControllerRepository(IDatabaseFactory databaseFactory): base(databaseFactory)
		{
		}
	}
	public interface ItblControllerRepository : IRepository<tblController>
	{
	}
}
