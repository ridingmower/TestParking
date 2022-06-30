using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Kztek.Data.Event.Infrastructure
{
    public abstract class RepositoryBase<T> where T : class
    {
        private KztekEventEntities dataContext;
        private readonly IDbSet<T> dbset;

        protected RepositoryBase(IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;
            dbset = DataContext.Set<T>();
        }

        protected IDatabaseFactory DatabaseFactory
        {
            get;
            private set;
        }

        protected KztekEventEntities DataContext
        {
            get { return dataContext ?? (dataContext = DatabaseFactory.Get()); }
        }

        public virtual void Add(T entity)
        {
            dbset.Add(entity);
        }

        public virtual void Update(T entity)
        {
            dbset.Attach(entity);
            dataContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(T entity)
        {
            dbset.Remove(entity);
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = dbset.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
                dbset.Remove(obj);
        }

        public virtual T GetById(long id)
        {
            return dbset.Find(id);
        }

        public virtual T GetById(string id)
        {
            return dbset.Find(id);
        }

        public virtual T GetById(Guid id)
        {
            return dbset.Find(id);
        }

        public T Get(Expression<Func<T, bool>> where)
        {
            return dbset.Where(where).FirstOrDefault<T>();
        }

        public virtual IQueryable<T> Table
        {
            get
            {
                return this.dbset;
            }
        }

        public virtual List<T> ExcuteQuery(string storeName, params Object[] parameters)
        {
            //var date = new SqlParameter("@date", _msg.MDate);
            //var subject = new SqlParameter("@subject", _msg.MSubject);
            //var body = new SqlParameter("@body", _msg.MBody);
            //var fid = new SqlParameter("@fid", _msg.FID);
            //this.Database.ExecuteSqlCommand("exec messageinsert @Date , @Subject , @Body , @Fid", date, subject, body, fid);
            var result = DataContext.Database.SqlQuery<T>(storeName, parameters);
            return result.ToList();
        }

        public virtual List<T> ExcuteQuery(string storeName)
        {
            var result = DataContext.Database.SqlQuery<T>(storeName);
            return result.ToList();
        }
    }
}