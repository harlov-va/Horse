using Horse.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Horse.DAL
{
    public class Repository : IDisposable, IRepository
    {
        #region System
        private LocalSqlServer _db;
        public LocalSqlServer db
        {
            get
            {
                if (_db == null)
                    _db = new LocalSqlServer();
                return _db;
            }
            set
            {
                _db = value;
            }
        }
        private bool _disposed = false;
        public Repository(LocalSqlServer db)
        {
            if (db == null) this.db = new LocalSqlServer();
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                   if (db != null) Dispose(true);
                }
                db = null;
                _disposed = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public void Save()
        {
            db.SaveChanges();
        }
        //public List<T> GetListSQLData<T>(string sql, object parameters = null, CommandType type = CommandType.StoredProcedure)
        //{
        //    try
        //    {
        //        using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServerSimple"].ConnectionString))
        //        {
        //            conn.Open();
        //            var els = conn.Query<T>(sql, parameters, commandType: type);
        //            return els as List<T>;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        RDL.Debug.LogError(ex);
        //        return default(List<T>);
        //    }
        //}
        //public T GetSQLData<T>(string sql, object parameters = null, CommandType type = CommandType.StoredProcedure)
        //{
        //    try
        //    {
        //        using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServerSimple"].ConnectionString))
        //        {
        //            conn.Open();
        //            var els = conn.Query<T>(sql, parameters, commandType: type);
        //            return (T)els;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        RDL.Debug.LogError(ex);
        //        return default(T);
        //    }
        //}
        #endregion
        #region contragents
        public IQueryable<h_logMoves> GetLogMoves()
        {
            var res = db.h_logMoves;
            return res;
        }
        public h_logMoves GetLogMove(int ID)
        {
            var res = db.h_logMoves.FirstOrDefault(x => x.id == ID);
            return res;
        }
        public int SaveLogMove(h_logMoves element, bool withSave = true)
        {
            if (element.id == 0)
            {
                db.h_logMoves.Add(element);
                if (withSave) Save();
            }
            else
            {
                db.Entry(element).State = System.Data.Entity.EntityState.Modified;
                if (withSave) Save();
            }
            return element.id;
        }
        public bool DeleteLogMove(int ID)
        {
            bool res = false;
            var item = db.h_logMoves.SingleOrDefault(x => x.id == ID);
            if (item != null)
            {
                db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                Save();
                res = true;
            }
            return res;
        }
        #endregion
    }
}