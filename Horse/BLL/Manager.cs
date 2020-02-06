using Horse.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Horse.BLL
{
    public class Manager : IManager
    {
        #region System
        private IRepository _db;
        private bool _disposed;
        public Manager(IRepository db)
        {
            _db = db;
            _disposed = true;
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
                {
                    if (_db != null) _db.Dispose();
                }
                _db = null;
                _disposed = true;
            }
        }
        private void _debug(Exception ex, Object parameters = null, string additions = "")
        {
            RDL.Debug.LogError(ex, additions, parameters);
        }
        #endregion
        ILogManager _logs;
        public ILogManager Logs
        {
            get
            {
                if (_logs == null)
                    _logs = new LogManager(_db);
                return _logs;
            }
            set
            {
                _logs = value;
            }
        }

        //public bool PutFigure(Dictionary<string, string> figure, out string msg, out int countMoves)
        //{
        //    bool res = false;
        //    msg = "";
        //    countMoves = 0;
        //    int[] dx = new[] { -2, -2, 2, 2, 1, -1, 1, -1 };
        //    int[] dy = new[] { -1, 1, -1, 1, 2, -2, -2, 2 };
        //    try
        //    {
        //        foreach(var item in figure.Keys)
        //        switch (item)
        //        {
        //            case "bN":
        //                    {
        //                        int x = (int)figure[item][0];
        //                        int y = int.Parse(figure[item][1].ToString());
        //                        for (int i = 0; i < dx.Count(); i++)
        //                        {

        //                            if (((x + dx[i]) > 96 && (y + dy[i]) > 0) && ((x + dx[i]) < 105 && (y + dy[i]) < 9)) countMoves++;
        //                        }
        //                    }
        //                    break;
        //        }
        //        res = true;
        //        msg = "Получен ответ";
        //        string msg2;
        //        //тут я записываю в логи
        //        Logs.CreateLogMove(figure,countMoves,out msg2);
        //    }
        //    catch (Exception e)
        //    {
        //        _debug(e, new { }, "Ошибка при получении значения");
        //        res = false;
        //        msg = "Ошибка при получении значения";
        //    }
        //    return res;
        //} 
        public bool PutFigure(Dictionary<string, string> figure, out string msg, out int countMoves)
        {
            bool res = false;
            msg = "";
            countMoves = 0;
            int[] dx = new[] { -2, -2, 2, 2, 1, -1, 1, -1 };
            int[] dy = new[] { -1, 1, -1, 1, 2, -2, -2, 2 };
            try
            {
                foreach (var item in figure.Keys)
                    switch (item)
                    {
                        case "bN":
                            {
                                int x = (int)figure[item][0]-97;
                                int y = Math.Abs(int.Parse(figure[item][1].ToString())-8);
                                for (int i = 0; i < dx.Count(); i++)
                                {

                                    //if (((x + dx[i]) > 96 && (y + dy[i]) > 0) && ((x + dx[i]) < 105 && (y + dy[i]) < 9)) countMoves++;
                                    countMoves = CountMovesHorse(x, y);
                                }
                            }
                            break;
                    }
                res = true;
                msg = "Получен ответ";
                string msg2;
                //тут я записываю в логи
                // отключил запись логов Logs.CreateLogMove(figure, countMoves, out msg2);

            }
            catch (Exception e)
            {
                _debug(e, new { }, "Ошибка при получении значения");
                res = false;
                msg = "Ошибка при получении значения";
            }
            return res;
        }
        public int CountMovesHorse(int current_row, int current_col)
        {
            //Правило Варнсдорфа. Это довольно эффективное правило заключается в следующем.

            //1) при обходе доски коня следует всякий раз ставить на поле, из которого он может сделать наименьшее число ходов на еще не пройденные поля;
            //2) если таких полей несколько, то разрешается выбирать любое из них.
            const int hor = 8, ver = 8; // задаю размеры доски
            int[,] board = new int[hor, ver]; // это двумерный массив куда записываются ходы, сначала он полностью пустой, каждый элемент = 0
            int[] horizont = new int[hor] { 2, 1, -1, -2, -2, -1, 1, 2 };// это два массива, которые отображают все 8 возможных хода коня 
            int[] vertical = new int[ver] { -1, -2, -2, -1, 1, 2, 2, 1 };

            int[,] access = new int[hor, ver] {// это массив доступности, здесь записывается информация о том, со скольких клеток можно походить на заданную
                { 2, 3, 4, 4, 4, 4, 3, 2},
                { 3, 4, 6, 6, 6, 6, 4, 3},
                { 4 ,6 ,8 ,8 ,8, 8, 6, 4},
                { 4 ,6 ,8 ,8 ,8, 8, 6, 4},
                { 4 ,6 ,8 ,8 ,8, 8, 6, 4},
                { 4 ,6 ,8 ,8 ,8, 8, 6, 4},
                { 3, 4, 6, 6, 6, 6, 4, 3},
                { 2, 3, 4, 4, 4, 4, 3, 2},
                };

            
            int mov_num;// количество ходов коня, от 0 до 7
            int count = 0;// количество ходов коня, которое он сделает по доске
            int main_row = current_row, main_col = current_col;// где стоит конь на доске, по началу он стоит там куда его поставил пользователь
            int row = 0, col = 0;// используются для вычисления лучшего хода

            for (int i = 0; i < 64; ++i)// начинаем ходить
            {
                board[main_row, main_col] = ++count;// присваем номер полю, в котором стоит конь, номер также отображает счет ходов

                int min_access = 8;// выставляем при каждом новом ходе максимальную доступность, но дальше ищем конечно же минимальную, чтобы туда поставить фигуру
                int minA = 8, minB = 8;// сравнением этих "доступностей" определяю наилучший ход, сравнение будет в самом низу

                for (mov_num = 0; mov_num <= 7; ++mov_num)// начинаю искать следующий ход, переставляя коня по всем возможным 8 позициям
                {                                         // и выискивая поле с минимальной доступностью, чтобы туда поставить фигуру
                                                          // если же встречаются 2 поля с одинаковой минимальной доступностью, то ставлю
                    current_row = main_row;               // фигуру туда, где через ход найдется поле с меньшей доступностью, т.е. если
                    /*конь стоит на ячейке (3,3), вес (доступность) этой ячейки 8, то я поставлю коня на ячейку (2,1), а не на (4,1), так как через ход
                     конь сможет встать на ячейку с весом 2 (0,0), а этот вес меньше чем вес 3 (6,1), если поставить коня на ячейку (4,1), уф, надеюсь объяснил*/
                    current_col = main_col;

                    current_row += horizont[mov_num];// ставлю коня
                    current_col += vertical[mov_num];

                    if (current_row >= 0 && current_row <= 7 && current_col >= 0 && current_col <= 7)// проверяю не выскочил ли он за доску
                    {

                        access[current_row, current_col]--;// уменьшаю доступность этого поля на 1

                        if (min_access > access[current_row, current_col] && board[current_row, current_col] == 0)// сравниваю доступность этой ячейки и не ходил ли я уже в это поле
                        {

                            min_access = access[current_row, current_col];// если доступность меньше, то уменьшаем минимальную доступность

                            if (board[current_row, current_col] == 0)
                            {
                                row = current_row;// если не ходил то записываем координаты ячейки
                                col = current_col;
                            }

                            int RowA = current_row, ColumnA = current_col;// и дальше уже идет осмотр доступностей на второй ход дальше, т.е. смотрим на 
                                                                          // два шага вперед
                            for (int moveA = 0; moveA <= 7; ++moveA)
                            {

                                RowA += horizont[moveA];
                                ColumnA += vertical[moveA];

                                if (RowA >= 0 && RowA <= 7 && ColumnA >= 0 && ColumnA <= 7)
                                {
                                    if (minA >= access[RowA, ColumnA] && board[RowA, ColumnA] == 0)
                                        minA = access[RowA, ColumnA];// и записываем минимальную доступность 
                                }
                            }
                        }
                        if (min_access == access[current_row, current_col] && board[current_row, current_col] == 0)
                        {// и вот тут и идет описанный выше выбор между двумя полями с одинаковой минимальной доступностью

                            min_access = access[current_row, current_col];

                            int RowB = current_row, ColumnB = current_col;

                            for (int moveB = 0; moveB <= 7; ++moveB)
                            {

                                RowB += horizont[moveB];
                                ColumnB += vertical[moveB];

                                if (RowB >= 0 && RowB <= 7 && ColumnB >= 0 && ColumnB <= 7)
                                {
                                    if (minB >= access[RowB, ColumnB] && board[RowB, ColumnB] == 0)
                                        minB = access[RowB, ColumnB];
                                }
                            }

                            if (board[current_row, current_col] == 0 && minB < minA)// выбираю ячейку (2,1)
                            {
                                row = current_row;
                                col = current_col;
                            }
                        }
                    }
                }

                main_row = row;// передвигаю коня на выбранную алгоритмом ячейку
                main_col = col;
            }

            return (board[main_row, main_col]-1);// отдаю результат, вычитая 1, так как пользователь ставит фигуру забирая тем самым один ход
        }
    }
}