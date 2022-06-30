using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	public class tblRoleRepository : RepositoryBase<tblRole>, ItblRoleRepository
	{
		public tblRoleRepository(IDatabaseFactory databaseFactory): base(databaseFactory)
		{
		}
	}
	public interface ItblRoleRepository : IRepository<tblRole>
	{
	}
}
