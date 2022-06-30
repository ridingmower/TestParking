using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	public class tblUserRepository : RepositoryBase<tblUser>, ItblUserRepository
	{
		public tblUserRepository(IDatabaseFactory databaseFactory): base(databaseFactory)
		{
		}
	}
	public interface ItblUserRepository : IRepository<tblUser>
	{
	}
}
