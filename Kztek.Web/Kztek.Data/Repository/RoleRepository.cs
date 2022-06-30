using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	public class RoleRepository : RepositoryBase<Role>, IRoleRepository
	{
		public RoleRepository(IDatabaseFactory databaseFactory): base(databaseFactory)
		{
		}
	}
	public interface IRoleRepository : IRepository<Role>
	{
	}
}
