using Horse.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Horse.BLL
{
    public class LogManager : ILogManager
    {
        #region System
        private IRepository _db;
        private bool _disposed;
        public LogManager(IRepository db)
        {
            _db = db;
            _disposed = false;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                    if (_db != null)
                        _db.Dispose();
                _db = null;
                _disposed = true;
            }
        }
        private void _debug(Exception ex, Object parameters = null, string additions = "")
        {
            RDL.Debug.LogError(ex, additions, parameters);
        }
        #endregion
        #region logMoves
        public List<h_logMoves> GetLogMoves( out string msg)
        {
            msg = "";
            List<h_logMoves> res;
            try
            {

                    res = _db.GetLogMoves().ToList();

            }
            catch (Exception e)
            { 
            _debug(e, new { }, "Ошибка возникла при получении списка документов");
            res = null;
            }
            return res;
        }
        public h_logMoves GetLogMove(int id, out string msg)
        {
            msg = "";
            h_logMoves res;
            try
            {

                    res = _db.GetLogMove(id);

            }
            catch (Exception e)
            {
                _debug(e, new { }, "Ошибка возникла при получении одного документа по id");
                res = null;
            }
            return res;
        }
        public h_logMoves CreateLogMove(Dictionary<string, string> parameters, int countMove,out string msg)
        {
            msg = "";
            h_logMoves res;
            try
            {
                    res = new h_logMoves();
                    
                    foreach (var key in parameters.Keys)
                    {
                        switch (key)
                        {
                            case "bN": res.coordinatesFigures = parameters[key];
                                break;
                        }
                    res.dateInsert = DateTime.Now;
                    res.nameFigure = "bN";
                    res.countMoves = countMove;
                    }
                _db.SaveLogMove(res);

            }
            catch (Exception e)
            {
                _debug(e, new { }, "Ошибка возникла при создании нового документа");
                res = null;
            }
            return res;
        }
        public h_logMoves EditLogMove(Dictionary<string ,string> parameters, int id, int countMove, out string msg)
        {
            msg = "";
            h_logMoves res;
            try
            {
                    res = _db.GetLogMove(id);
                    foreach(var key in parameters.Keys)
                    {
                         switch (key)
                            {
                                case "bN":
                                    res.coordinatesFigures = parameters[key];
                                break;
                             }
                    res.dateInsert = DateTime.Now;
                    res.countMoves = countMove;
                }
                    _db.SaveLogMove(res);
            }
            catch(Exception e)
            {
                _debug(e, new { }, "Ошибка возникла при изменении элемента");
                res = null;
            }
            return res;
        }

        public bool RemoveLogMove(int id, out string msg)
        {
            msg = "";
            bool res;
            try
            {
                _db.DeleteLogMove(id);
                    res = true;

            }
            catch (Exception e)
            {
                _debug(e, new { }, "Ошибка возникла при удалении элемента");
                res = false;
            }
            return res;
        }
        #endregion
     
    }
}