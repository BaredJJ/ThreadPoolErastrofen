using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThreadPoolErastrofen;

namespace ThreadPoolTest
{
    [TestClass]
    public class UnitTest1
    {
        int primeNumber = 2;
        int size = 100;

        private List<int> Getlist()
        {
            List<int> list = new List<int>( );
            for(int i = primeNumber; i < size; i++)
            {
                if ((i) % primeNumber == 0)
                    list.Add(i);
            }
            return list;
        }

        [TestMethod]
        public void TestMethod1()
        {
            Program.baseArray = Getlist( );


            ManualResetEvent[] reset = new ManualResetEvent[1];
            reset[0] = new ManualResetEvent(false);

            ThreadPool.QueueUserWorkItem(Program.Run, new object[] { primeNumber, reset[0] });

            WaitHandle.WaitAll(reset);

            Assert.IsTrue(Program.baseArray.Count == 1);
        }
    }
}
