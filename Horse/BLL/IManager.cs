using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Horse.BLL
{
    public interface IManager:IDisposable
    {
        ILogManager Logs { get; set; }
        bool PutFigure(Dictionary<string,string> figure,out string msg, out int countMoves);
    }
}
