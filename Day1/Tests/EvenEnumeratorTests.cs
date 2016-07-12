using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Heap;

namespace Tests
{
    [TestClass]
    public class EvenEnumeratorTests
    {
        [TestMethod]
        public void GetDefaultState_ReturnMinusTwo()
        {
            EvenEnumerator it = new EvenEnumerator();
            Assert.AreEqual(it.Current, -2);
        }

        [TestMethod]
        public void GetFirstEvenNumber_ReturnZero()
        {
            EvenEnumerator it = new EvenEnumerator();
            it.MoveNext();
            Assert.AreEqual(it.Current, 0);
        }

        [TestMethod]
        public void GetLastEvenNumber_ReturnIntMaxValueMinusOne()
        {
            EvenEnumerator it = new EvenEnumerator();
            while (it.MoveNext()) ;
            Assert.AreEqual(it.Current, int.MaxValue - 1);
        }
    }
}
