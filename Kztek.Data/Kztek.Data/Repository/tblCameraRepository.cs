using Kztek.Data.Infrastructure;
using Kztek.Model.Models;

namespace Kztek.Data.Repository
{
	public class tblCameraRepository : RepositoryBase<tblCamera>, ItblCameraRepository
	{
		public tblCameraRepository(IDatabaseFactory databaseFactory): base(databaseFactory)
		{
		}
	}
	public interface ItblCameraRepository : IRepository<tblCamera>
	{
	}
}
