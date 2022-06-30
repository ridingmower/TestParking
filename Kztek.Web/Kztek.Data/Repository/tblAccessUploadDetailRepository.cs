using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	public class tblAccessUploadDetailRepository : RepositoryBase<tblAccessUploadDetail>, ItblAccessUploadDetailRepository
	{
		public tblAccessUploadDetailRepository(IDatabaseFactory databaseFactory): base(databaseFactory)
		{
		}
	}
	public interface ItblAccessUploadDetailRepository : IRepository<tblAccessUploadDetail>
	{
	}
}
