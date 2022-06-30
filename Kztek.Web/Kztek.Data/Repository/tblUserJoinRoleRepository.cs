using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	public class tblUserJoinRoleRepository : RepositoryBase<tblUserJoinRole>, ItblUserJoinRoleRepository
	{
		public tblUserJoinRoleRepository(IDatabaseFactory databaseFactory): base(databaseFactory)
		{
		}
	}
	public interface ItblUserJoinRoleRepository : IRepository<tblUserJoinRole>
	{
	}
}
