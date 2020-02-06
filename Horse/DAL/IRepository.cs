using Horse.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horse.DAL
{
    public interface IRepository:IDisposable
    {
        #region System
        void Save();
        #endregion
        #region logMoves
        IQueryable<h_logMoves> GetLogMoves();
        h_logMoves GetLogMove(int ID);
        int SaveLogMove(h_logMoves element, bool withSave = true);
        bool DeleteLogMove(int ID);
        #endregion

    }
}
