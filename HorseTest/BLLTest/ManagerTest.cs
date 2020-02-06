using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Horse.BLL;

namespace HorseTest
{
    [TestClass]
    public class ManagerTest
    {
        [TestMethod]
        public void PutFigureTest()
        {
            Manager _mng = new Manager();
            var result1 = _mng.PutFigure("horse", 2, 2);
            var result2 = _mng.PutFigure("horse", 1, 1);
            var result3 = _mng.PutFigure("horse", 4, 4);
            var result4 = _mng.PutFigure("horse", 8, 1);
            var result5 = _mng.PutFigure("horse", 4, 2);
            var result6 = _mng.PutFigure("horse", 5, 2);
            var result7 = _mng.PutFigure("horse", 1, 8);
            var result8 = _mng.PutFigure("horse", 2, 4);
            var result9 = _mng.PutFigure("horse", 2, 5);
            var result10 = _mng.PutFigure("horse", 8, 7);
            var result11 = _mng.PutFigure("horse", 8, 3);
            var result12 = _mng.PutFigure("horse", 6, 8);
            

            //Assert.IsNotNull(result);
            Assert.AreEqual(4, result1);
            Assert.AreEqual(2, result2);
            Assert.AreEqual(8, result3);
            Assert.AreEqual(2, result4);
            Assert.AreEqual(6, result5);
            Assert.AreEqual(6, result6);
            Assert.AreEqual(2, result7);
            Assert.AreEqual(6, result8);
            Assert.AreEqual(6, result9);
            Assert.AreEqual(3, result10);
            Assert.AreEqual(4, result11);
            Assert.AreEqual(4, result12);
        }
    }
}
