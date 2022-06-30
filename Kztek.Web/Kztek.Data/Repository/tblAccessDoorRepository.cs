using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	public class tblAccessDoorRepository : RepositoryBase<tblAccessDoor>, ItblAccessDoorRepository
	{
		public tblAccessDoorRepository(IDatabaseFactory databaseFactory): base(databaseFactory)
		{
		}
	}
	public interface ItblAccessDoorRepository : IRepository<tblAccessDoor>
	{
	}
}
