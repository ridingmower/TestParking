using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Kztek.Data.LockerEvent.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        // Them mowi
        void Add(T entity);

        // Cap nhap
        void Update(T entity);

        // Xoa theo ID
        void Delete(T entity);

        // Xoa theo dieu kien
        void Delete(Expression<Func<T, bool>> where);

        // Lay theo Id kieu int
        T GetById(long id);

        // Lay theo Id kieu string
        T GetById(string id);

        // Lay theo Id kieu Guid
        T GetById(Guid id);

        // Lay theo dieu kien
        T Get(Expression<Func<T, bool>> where);

        // Tra ve dang IQueryable dung de where theo Linq
        IQueryable<T> Table { get; }

        List<T> ExcuteQuery(string storeName, params Object[] parameters);

        List<T> ExcuteQuery(string storeName);
    }
}