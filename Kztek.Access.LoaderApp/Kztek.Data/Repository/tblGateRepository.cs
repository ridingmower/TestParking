using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	public class tblGateRepository : RepositoryBase<tblGate>, ItblGateRepository
	{
		public tblGateRepository(IDatabaseFactory databaseFactory): base(databaseFactory)
		{
		}
	}
	public interface ItblGateRepository : IRepository<tblGate>
	{
	}
}
