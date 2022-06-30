using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	public class RoleMenuRepository : RepositoryBase<RoleMenu>, IRoleMenuRepository
	{
		public RoleMenuRepository(IDatabaseFactory databaseFactory): base(databaseFactory)
		{
		}
	}
	public interface IRoleMenuRepository : IRepository<RoleMenu>
	{
	}
}
