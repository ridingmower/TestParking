using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	public class tblFeeRepository : RepositoryBase<tblFee>, ItblFeeRepository
	{
		public tblFeeRepository(IDatabaseFactory databaseFactory): base(databaseFactory)
		{
		}
	}
	public interface ItblFeeRepository : IRepository<tblFee>
	{
	}
}
