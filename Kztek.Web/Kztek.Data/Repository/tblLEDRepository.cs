using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	public class tblLEDRepository : RepositoryBase<tblLED>, ItblLEDRepository
	{
		public tblLEDRepository(IDatabaseFactory databaseFactory): base(databaseFactory)
		{
		}
	}
	public interface ItblLEDRepository : IRepository<tblLED>
	{
	}
}
