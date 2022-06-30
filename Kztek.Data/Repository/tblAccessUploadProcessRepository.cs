using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	public class tblAccessUploadProcessRepository : RepositoryBase<tblAccessUploadProcess>, ItblAccessUploadProcessRepository
	{
		public tblAccessUploadProcessRepository(IDatabaseFactory databaseFactory): base(databaseFactory)
		{
		}
	}
	public interface ItblAccessUploadProcessRepository : IRepository<tblAccessUploadProcess>
	{
	}
}
