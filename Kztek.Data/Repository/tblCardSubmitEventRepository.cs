using System;
using Kztek.Data.Infrastructure;
using Kztek.Model.Models;
using Kztek.Model.Models.API;

namespace Kztek.Data.Repository
{
	public class tblCardSubmitEventRepository : RepositoryBase<tblCardSubmitEvent>, ItblCardSubmitEventRepository
	{
		public tblCardSubmitEventRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
		{
		}
	}
	public interface ItblCardSubmitEventRepository : IRepository<tblCardSubmitEvent>
	{
	}
}
