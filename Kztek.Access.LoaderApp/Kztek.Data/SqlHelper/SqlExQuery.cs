using Kztek.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kztek.Data.SqlHelper
{
    public class SqlExQuery<T> where T : class
    {
        /*
         * var date = new SqlParameter("@date", _msg.MDate);
        var subject = new SqlParameter("@subject", _msg.MSubject);
        var body = new SqlParameter("@body", _msg.MBody);
        var fid = new SqlParameter("@fid", _msg.FID);
        this.Database.ExecuteSqlCommand("exec messageinsert @Date , @Subject , @Body , @Fid", date, subject, body, fid);
         */

        public static List<T> ExcuteQuery(string storeName, params Object[] parameters)
        {
            using (var _DataContext = new DatabaseFactory().Get())
            {
                var result = _DataContext.Database.SqlQuery<T>(storeName, parameters);
                return result.ToList();
            }
        }

        // Goi store, cau truy van ma khong can tham so truyen vao
        public static List<T> ExcuteQuery(string storeName)
        {
            using (var _DataContext = new DatabaseFactory().Get())
            {
                var result = _DataContext.Database.SqlQuery<T>(storeName);
                return result.ToList();
            }
        }

        public static T ExcuteQueryRtnEntity(string storeName, params Object[] parameters)
        {
            using (var _DataContext = new DatabaseFactory().Get())
            {
                var result = _DataContext.Database.SqlQuery<T>(storeName, parameters);
                return result.FirstOrDefault();
            }
        }

        // Goi store, cau truy van ma khong can tham so truyen vao
        public static T ExcuteQueryRtnEntity(string storeName)
        {
            using (var _DataContext = new DatabaseFactory().Get())
            {
                var result = _DataContext.Database.SqlQuery<T>(storeName);
                return result.FirstOrDefault();
            }
        }

        public static T ExcuteQueryFirst(string storeName, params Object[] parameters)
        {
            using (var _DataContext = new DatabaseFactory().Get())
            {
                var result = _DataContext.Database.SqlQuery<T>(storeName, parameters);
                return result.FirstOrDefault();
            }
        }

        public static T ExcuteQueryFirst(string storeName)
        {
            using (var _DataContext = new DatabaseFactory().Get())
            {
                var result = _DataContext.Database.SqlQuery<T>(storeName);
                return result.FirstOrDefault();
            }
        }

        // Goi truy van ma khong can tra ve gi ca
        public static int ExcuteNone(string storeName, params Object[] parameters)
        {
            using (var _DataContext = new DatabaseFactory().Get())
            {
                var result = _DataContext.Database.ExecuteSqlCommand(storeName);
                return result;
            }
        }

        public static int ExcuteNone(string storeName)
        {
            using (var _DataContext = new DatabaseFactory().Get())
            {
                var result = _DataContext.Database.ExecuteSqlCommand(storeName);
                return result;
            }
        }
    }
}