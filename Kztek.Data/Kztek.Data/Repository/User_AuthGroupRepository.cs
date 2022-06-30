using Kztek.Data.Infrastructure;
using Kztek.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kztek.Data.Repository
{
    public class User_AuthGroupRepository : RepositoryBase<User_AuthGroup>, IUser_AuthGroupRepository
    {
        public User_AuthGroupRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }
    }

    public interface IUser_AuthGroupRepository : IRepository<User_AuthGroup>
    {
    }
}
