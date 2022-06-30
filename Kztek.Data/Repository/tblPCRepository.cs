using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	public class tblPCRepository : RepositoryBase<tblPC>, ItblPCRepository
	{
		public tblPCRepository(IDatabaseFactory databaseFactory): base(databaseFactory)
		{
		}
	}
	public interface ItblPCRepository : IRepository<tblPC>
	{
	}
}
