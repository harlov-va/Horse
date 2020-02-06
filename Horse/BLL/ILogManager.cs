using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horse.BLL
{
    public interface ILogManager:IDisposable
    {
        #region logMoves
        List<h_logMoves> GetLogMoves(out string msg);
        h_logMoves GetLogMove(int id, out string msg);
        h_logMoves CreateLogMove(Dictionary<string, string> parameters, int countMove,out string msg);
        h_logMoves EditLogMove(Dictionary<string, string> parameters, int id, int countMove,out string msg);
        bool RemoveLogMove(int id, out string msg);
        
        #endregion
       
    }
}
