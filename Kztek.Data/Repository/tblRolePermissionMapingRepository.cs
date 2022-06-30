using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	public class tblRolePermissionMapingRepository : RepositoryBase<tblRolePermissionMaping>, ItblRolePermissionMapingRepository
	{
		public tblRolePermissionMapingRepository(IDatabaseFactory databaseFactory): base(databaseFactory)
		{
		}
	}
	public interface ItblRolePermissionMapingRepository : IRepository<tblRolePermissionMaping>
	{
	}
}
